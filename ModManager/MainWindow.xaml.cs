using Core.Model;
using CurseForgeApiLib.Client;
using CurseForgeApiLib.Enums;
using Features.Attributes;
using ModManager.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace ModManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const int PaginationLimit = 10000;
        public ViewState State { get; set; } = new()
        {
            GameId = 432,
            ClassId = 6,
            AuthorId = null,
            CategoryId = null,
            GameVersion = null,
            GameVersionTypeId = null,
            Index = 0,
            ModLoaderType = ModLoaderType.Any,
            PageSize = 10,
            SearchFilter = null,
            Slug = null,
            SortFields = null,
            SortOrder = null
        };

        private readonly List<int> PageSizes = new() { 5, 10, 15, 20, 30, 50};
        private readonly CurseModApiDeserializer _modDeserializer = new(new CurseModApiService());
        private readonly CurseFeaturesApiDeserializer _featuresDeserializer = new();
        private int PageNumber { get; set; } = 1;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

        public async Task FetchMods()
        {
            var mods = await _modDeserializer.SearchMods(State);
            datagrid.ItemsSource = mods;
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

        private void datagrid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            var propertyDescriptor = e.PropertyDescriptor as PropertyDescriptor;

            if (propertyDescriptor.Attributes.OfType<HideInDataGridAttribute>().Any())
                e.Cancel = true;
            if(e.Column is DataGridTextColumn column)
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
            State.CategoryId = (CategoryComboBox.SelectedItem as Category).Id;
        }

        private void GameVersionComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            State.GameVersion = (GameVersionComboBox.SelectedItem as MinecraftGameVersion).VersionString;
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
            if(e.Key == System.Windows.Input.Key.Enter)
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
        }

        private void PageSizeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            State.PageSize = (int)PageSizeComboBox.SelectedItem;
        }

        private void datagrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (datagrid.SelectedItem == null)
                return;
            var selectedMod = (Mod)datagrid.SelectedItem;
            ModName.Text = selectedMod.Name;
            ModDescription.Text = selectedMod.Summary;
            ModWebSiteUrl.NavigateUri = new Uri(selectedMod.Links.WebsiteUrl);
            ModWebSiteUrl.Inlines.Clear();
            ModWebSiteUrl.Inlines.Add(selectedMod.Links.WebsiteUrl);
        }

        private void ModWebSiteUrl_RequestNavigate_1(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
        {
            string url = e.Uri.AbsoluteUri;
            try
            {
                Process.Start(new ProcessStartInfo("chrome.exe", url));
            }
            catch (Win32Exception)
            {
                Process.Start(new ProcessStartInfo(@"C:\Program Files\Google\Chrome\Application\chrome.exe", url));
            }
            e.Handled = true;
        }
    }
}
