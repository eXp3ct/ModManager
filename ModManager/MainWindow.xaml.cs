using Core.Model;
using CurseForgeApiLib.Client;
using CurseForgeApiLib.Enums;
using Features.Attributes;
using ModManager.Model;
using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
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
        public ViewState CurrentState { get; set; } = new()
        {
            GameId = 432,
            ClassId = 6,
            AuthorId = null,
            CategoryId = null,
            GameVersion = null,
            GameVersionTypeId = null,
            Index = 0,
            ModLoaderType = null,
            PageSize = 50,
            SearchFilter = null,
            Slug = null,
            SortFields = null,
            SortOrder = null
        };
        private int PageNumber { get; set; } = 1;
        private readonly CurseModApiDeserializer _modDeserializer = new(new CurseModApiService());
        private readonly CurseFeaturesApiDeserializer _featuresDeserializer = new();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

        private async Task FetchData()
        {
            //TODO кэшировать
            var mods = await _modDeserializer.SearchMods(CurrentState);
            datagrid.ItemsSource = mods;
        }

        private async void Window_Activated(object sender, EventArgs e)
        {
            pageNumber.Content = PageNumber;

            var gameVersions = await _featuresDeserializer.GetMinecraftGameVersions();
            var categories = await _featuresDeserializer.GetCategories(CurrentState.GameId, CurrentState.ClassId);

            CategoryComboBox.ItemsSource = categories;
            GameVersionComboBox.ItemsSource = gameVersions;
            foreach(var sortField in Enum.GetNames(typeof(SearchSortFields)))
                SortFieldComboBox.Items.Add(sortField);
            foreach(var modLoader in Enum.GetNames(typeof(ModLoaderType)))
                ModLoaderComboBox.Items.Add(modLoader);
        }

        private void PreviousPage_Click(object sender, RoutedEventArgs e)
        {
            pageNumber.Content = --PageNumber;
        }

        private void NextPage_Click(object sender, RoutedEventArgs e)
        {
            pageNumber.Content = ++PageNumber;
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

        private async void GameVersionComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CurrentState.GameVersion = (GameVersionComboBox.SelectedItem as MinecraftGameVersion).VersionString;
            var mods = await _modDeserializer.SearchMods(CurrentState);
            datagrid.ItemsSource = mods;
        }

        private async void CategoryComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CurrentState.CategoryId = (CategoryComboBox.SelectedItem as Category).Id;

            var mods = await _modDeserializer.SearchMods(CurrentState);
            datagrid.ItemsSource = mods;
        }
    }
}
