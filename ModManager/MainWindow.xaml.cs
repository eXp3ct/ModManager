using AutoUpdaterDotNET;
using Core.Model;
using CurseForgeApiLib.Client;
using CurseForgeApiLib.Enums;
using Features.Attributes;
using HttpDownloader;
using InMemoryCahing;
using Logging;
using Microsoft.VisualBasic.ApplicationServices;
using ModManager.Model;
using Newtonsoft.Json;
using Ookii.Dialogs.Wpf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace ModManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const int PaginationLimit = 10000;

        public static ViewState State { get; set; } = new()
        {
            GameId = 432,
            ClassId = 6,
            AuthorId = null,
            CategoryId = null,
            GameVersion = null,
            GameVersionTypeId = null,
            Index = 0,
            ModLoaderType = ModLoaderType.Forge,
            PageSize = 10,
            SearchFilter = null,
            Slug = null,
            SortFields = null,
            SortOrder = null
        };

        private readonly List<int> PageSizes = new() { 5, 10, 15, 20, 30, 50 };
        private List<Mod> DownloadedMods = new();
        private readonly CurseFeaturesApiDeserializer _featuresDeserializer = new();
        private readonly ModsProvider _modsProvider = new();
        private readonly Updater _updater = new();
        private int PageNumber { get; set; } = 1;
        private string FolderPath { get; set; }


        public MainWindow()
        {
            InitializeComponent();

            _modsProvider.SelectedModsChanged += _modsProvider_SelectedModsChanged;
        }

        private void _modsProvider_SelectedModsChanged(object? sender, IList<Mod> e)
        {
            //TODO Downloading
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

        public async Task FetchMods()
        {
            var mods = await _modsProvider.GetMods(State);
            datagrid.ItemsSource = mods;
        }

        private void FetchMods(bool saved)
        {
            if (saved)
            {
                var mods = _modsProvider.GetSelectedMods();
                datagrid.ItemsSource = mods;
            }
        }

        private async void CurrentState_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            await FetchMods();
        }

        private void PreviousPage_Click(object sender, RoutedEventArgs e)
        {
            if (PageNumber == 1)
                return;
            pageNumber.Content = --PageNumber;
            State.Index -= State.PageSize;
        }

        private void NextPage_Click(object sender, RoutedEventArgs e)
        {
            if (State.Index + State.PageSize > PaginationLimit)
                return;
            pageNumber.Content = ++PageNumber;
            State.Index += State.PageSize;
        }

        private void Datagrid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            var propertyDescriptor = e.PropertyDescriptor as PropertyDescriptor;

            if (propertyDescriptor.Attributes.OfType<HideInDataGridAttribute>().Any())
                e.Cancel = true;
            if (e.Column is DataGridTextColumn column)
            {
                var style = new Style(typeof(TextBlock));
                style.Setters.Add(new Setter(TextBlock.TextWrappingProperty, TextWrapping.Wrap));
                column.ElementStyle = style;
            }
            if (e.Column.Header.Equals(nameof(Mod.Selected)))
            {
                var selectionColumn = e.Column as DataGridCheckBoxColumn;
                selectionColumn.IsReadOnly = false;
            }
            else
            {
                e.Column.IsReadOnly = true;
            }
        }

        private void CategoryComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            State.CategoryId = ((Category)CategoryComboBox.SelectedItem).Id;
        }

        private void GameVersionComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            State.GameVersion = ((MinecraftGameVersion)GameVersionComboBox.SelectedItem).VersionString;
            _modsProvider.ClearSelectedMods();
        }

        private void ModLoaderComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            State.ModLoaderType = (ModLoaderType)ModLoaderComboBox.SelectedItem;
        }

        private void SortFieldComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            State.SortFields = (SearchSortFields)SortFieldComboBox.SelectedItem;
        }

        private void DescendingCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (DescendingCheckBox.IsChecked == true)
                State.SortOrder = "desc";
            else
                State.SortOrder = "asc";
        }

        private void SearchFilter_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                var searchString = SearchFilter.Text;
                State.SearchFilter = searchString;
            }
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            pageNumber.Content = PageNumber;
            await FetchMods();
            State.PropertyChanged += CurrentState_PropertyChanged;

            var gameVersions = await _featuresDeserializer.GetMinecraftGameVersions();
            var categories = await _featuresDeserializer.GetCategories(State.GameId, State.ClassId);

            CategoryComboBox.ItemsSource = categories;
            GameVersionComboBox.ItemsSource = gameVersions;
            SortFieldComboBox.ItemsSource = (SearchSortFields[])Enum.GetValues(typeof(SearchSortFields));
            ModLoaderComboBox.ItemsSource = (ModLoaderType[])Enum.GetValues(typeof(ModLoaderType));
            PageSizeComboBox.ItemsSource = PageSizes;

            _updater.CheckForUpdates();
        }

        private void PageSizeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            State.PageSize = (int)PageSizeComboBox.SelectedItem;
        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            var dialog = new VistaFolderBrowserDialog();
            if (dialog.ShowDialog() == true)
            {
                FolderPath = dialog.SelectedPath;
            }
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(FolderPath))
            {
                MessageBox.Show("Выберите путь до папки", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            if (string.IsNullOrEmpty(State.GameVersion))
            {
                MessageBox.Show("Выберите версию", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            var downloader = new ModDownloader(State, FolderPath);

            ProgressBar.Maximum = 100;
            ProgressBar.Minimum = 0;

            var progress = new Progress<double>(value => ProgressBar.Value += value);


            var selectedMods = (List<Mod>)_modsProvider.GetSelectedMods();
            DownloadedMods.AddRange(selectedMods);
            await downloader.StartDownloading(selectedMods, progress);

            MessageBox.Show($"{selectedMods.Count} was successfuly installed!");
            LoggerService.Logger.Info($"Successfuly installed {selectedMods.Count} mods");
            _modsProvider.ClearSelectedMods();
            ProgressBar.Value = 0;
        }

        private async void MenuItem_Click_2(object sender, RoutedEventArgs e)
        {
            var dialog = new VistaFolderBrowserDialog();
            if (dialog.ShowDialog() == true)
            {
                var path = dialog.SelectedPath;

                var json = JsonConvert.SerializeObject(DownloadedMods, Formatting.Indented);

                await System.IO.File.WriteAllTextAsync($"{path}/save.json", json);
            }
        }

        private async void MenuItem_Click_3(object sender, RoutedEventArgs e)
        {
            var dialog = new VistaOpenFileDialog();
            if (dialog.ShowDialog() == true)
            {
                var path = dialog.FileName;
                var jsonString = await System.IO.File.ReadAllTextAsync(path);
                var mods = JsonConvert.DeserializeObject<List<Mod>>(jsonString);

                _modsProvider.SetSelectedMods(mods);
                FetchMods(true);
            }
        }

        private void CheckUpdates_Click(object sender, RoutedEventArgs e)
        {
            _updater.CheckForUpdates();
        }
    }
}
