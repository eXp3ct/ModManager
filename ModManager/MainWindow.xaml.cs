using Core.Model;
using CurseForgeApiLib.Client;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ModManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

        private async void Window_Activated(object sender, EventArgs e)
        {
            var deserializer = new CurseApiDeserializer(new CurseModApiService());
            var mods = await deserializer.SearchMods();

            datagrid.ItemsSource = mods;
        }

        private static async Task StringToJson(string response)
        {
            var json = JsonConvert.DeserializeObject<dynamic>(response);

            var path = System.IO.Path.Combine(@"D:\Projects\C# Projects\ModManager\Jsons", "mods.json");
            await File.WriteAllTextAsync(path, JsonConvert.SerializeObject(json, Formatting.Indented));
        }
    }
}
