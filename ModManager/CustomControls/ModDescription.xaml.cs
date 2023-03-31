using Core.Model;
using CurseForgeApiLib.Client;
using ModManager.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ModManager.CustomControls
{
    /// <summary>
    /// Логика взаимодействия для ModDescription.xaml
    /// </summary>
    public partial class ModDescription : UserControl
    {
        public static readonly DependencyProperty ModProperty =
            DependencyProperty.Register("Mod", typeof(Mod), typeof(ModDescription), new PropertyMetadata(null, OnModChanged));

        private readonly CurseModFileApiDeserializer _fileDeserialier = new(new CurseModFileApiService());
        private readonly CurseModApiDeserializer _modDeserializer = new(new CurseModApiService());

        public Mod? Mod
        {
            get { return (Mod)GetValue(ModProperty); }
            set { SetValue(ModProperty, value); }
        }

        public ModDescription()
        {
            InitializeComponent();
            DataContext = this;

        }
        private static async void OnModChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ModDescription modDescription && e.NewValue is Mod mod)
            {
                // Fetch the dependency mods for the selected mod
                var dependencyMods = await modDescription.FetchDependencies(mod);
                var modLogo = await modDescription.FetchImage(mod.Logo.Url);

                // Bind the dependency mods to the listbox
                modDescription.DependecyList.ItemsSource = dependencyMods;
                modDescription.ModImage.Source = modLogo;
            }
        }

        public async Task<List<string>> FetchDependencies(Mod mod)
        {
            var dependencyMods = new List<string>();

            // Fetch the dependency mods for the selected mod
            var modFiles = await _fileDeserialier.GetModFiles(mod.Id, MainWindow.State.GameVersion, MainWindow.State.ModLoaderType, pageSize: 10);
            var file = modFiles.FirstOrDefault();
            if(file != null)
            {
                var dependency = file.Dependencies.Where(dep => dep.RelationType == Core.Enums.FileRelationType.RequiredDependency);

                if (dependency.Any())
                {
                    foreach(var dep in dependency)
                    {
                        dependencyMods.Add((await _modDeserializer.GetMod(dep.ModId)).Name);
                    }
                }
            }
            return dependencyMods;
        }
        
        public async Task<ImageSource> FetchImage(string url)
        {
            using var client = new HttpClient();
            using var response = await client.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                using var stream = await response.Content.ReadAsStreamAsync();
                var bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.StreamSource = stream;
                bitmap.EndInit();
                bitmap.Freeze();
                return bitmap;
            }

            return new BitmapImage();
        }
    }
}
