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
            var modFileService = new CurseModFileApiService();
            var modDeserializer = new CurseModApiDeserializer(new CurseModApiService());
            var modFileDeserializer = new CurseModFileApiDeserializer(new CurseModFileApiService());
            //var mods = await deserializer.SearchMods();
            var modIds = new List<int> { 222880, 325739, 291737, 390003, 819355,
            810803,222880,325739, 291737};
            var mods = await modDeserializer.GetMods(modIds);

            datagrid.ItemsSource = mods;
            var links = mods[0].Links.WebsiteUrl.ToString();
            MessageBox.Show(links);
            //await StringToJson(await modFileService.GetModFiles(390003, gameVersion: "1.19.2"));
        }

        private static async Task StringToJson(string response)
        {
            var json = JsonConvert.DeserializeObject<dynamic>(response);

            var path = System.IO.Path.Combine(@"D:\Projects\C# Projects\ModManager\Jsons", "modFiles.json");
            await File.WriteAllTextAsync(path, JsonConvert.SerializeObject(json, Formatting.Indented));
        }
    }
}
