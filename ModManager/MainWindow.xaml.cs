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
        public ViewState State { get; set; } = new()
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

        public async Task FetchMods()
        {
            var mods = await _modDeserializer.SearchMods(State);
            datagrid.ItemsSource = mods;
        }
        private async void Window_Activated(object sender, EventArgs e)
        {
            pageNumber.Content = PageNumber;
            await FetchMods();
            State.PropertyChanged += CurrentState_PropertyChanged;

            var gameVersions = await _featuresDeserializer.GetMinecraftGameVersions();
            var categories = await _featuresDeserializer.GetCategories(State.GameId, State.ClassId);

            CategoryComboBox.ItemsSource = categories;
            GameVersionComboBox.ItemsSource = gameVersions;
            foreach(var sortField in Enum.GetNames(typeof(SearchSortFields)))
                SortFieldComboBox.Items.Add(sortField);
            foreach(var modLoader in Enum.GetNames(typeof(ModLoaderType)))
                ModLoaderComboBox.Items.Add(modLoader);
        }

        private async void CurrentState_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            await FetchMods();
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
            if(Enum.TryParse<ModLoaderType>((string)ModLoaderComboBox.SelectedItem, true, out var result)){
                State.ModLoaderType = result;
            }
        }

        private void SortFieldComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Enum.TryParse<SearchSortFields>((string)SortFieldComboBox.SelectedItem, true, out var result))
            {
                State.SortFields = result;
            }
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
    }
}
