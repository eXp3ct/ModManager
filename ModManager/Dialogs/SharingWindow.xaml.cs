using Core.Model;
using Sharing;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace ModManager.Dialogs
{
    /// <summary>
    /// Логика взаимодействия для SharingWindow.xaml
    /// </summary>
    public partial class SharingWindow : Window
    {
        private readonly string _url = "https://api.bitbucket.org/2.0/repositories/mmmodmanager/sharing/downloads";
        private List<Artifact> _artifacts = new();
        private Artifacts _artifact = new();
        private string FolderPath { get; set; }
        public SharingWindow(string folderPath)
        {
            InitializeComponent();
            FolderPath = folderPath;
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            await FetchArtifacts();
        }

        private async Task FetchArtifacts()
        {
            _artifacts = await _artifact.GetListArtifactsAsync(_url);

            DataGrid.ItemsSource = _artifacts;
        }

        private void DataGrid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.Column.Header.ToString() == nameof(Artifact.Selected))
                e.Column.IsReadOnly = false;
            else
                e.Column.IsReadOnly = true;
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            if (_artifacts.Where(artifact => artifact.Selected).Count() > 1)
            {
                MessageBox.Show("Нельзя установить больше одной сборки", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            else if (_artifacts.Where(artifact => artifact.Selected).Count() == 0)
            {
                MessageBox.Show("Выберите сборку", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            var random = new Random(DateTime.Now.Millisecond);
            ProgressBar.Value = random.Next(1, 70);
            var fileName = _artifacts.FirstOrDefault().Name;
            var bytes = await _artifact.GetFileBytes(fileName);
            using var stream = new FileStream($"{FolderPath}/{fileName}", FileMode.Create);
            stream.Write(bytes, 0, bytes.Length);
            ProgressBar.Value = 100;
            stream.Dispose();
            await ExtractZipAsync($"{FolderPath}/{fileName}", FolderPath);

            File.Delete($"{FolderPath}/{fileName}");
            MessageBox.Show("Сборка установлена!", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private static async Task ExtractZipAsync(string zipFilePath, string extractPath)
        {
            await Task.Run(() =>
            {
                ZipFile.ExtractToDirectory(zipFilePath, extractPath);
            });
        }

        private async void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var share = new Share();
            var userName = Environment.MachineName;

            ZipFile.CreateFromDirectory(FolderPath, "mods.zip");
            if (await share.UploadFiles("mods.zip", userName))
            {
                File.Delete("mods.zip");
                MessageBox.Show("Сборка успешно загружена", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Ошибка при загрузке сборки", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            await FetchArtifacts();
        }
    }
}
