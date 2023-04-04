using Core.Model;
using CurseForgeApiLib.Client;
using ModManager.Model;
using Core.Enums;
using System.Text;
using System.Net;
using Logging;

namespace HttpDownloader
{
    public class ModDownloader : Downloader
    {
        private readonly CurseModFileApiDeserializer _deserializer = new(new CurseModFileApiService());
        private readonly CurseModApiDeserializer _modDeserizlier = new(new CurseModApiService());
        private readonly string _folderPath;
        private readonly ViewState _viewState;
        
        //private List<Mod> _dependenciesMods = new();
        public ModDownloader(ViewState state, string folderPath)
        {
            _folderPath = folderPath;
            _viewState = state; 
        }

        public async Task StartDownloading(List<Mod> mods, IProgress<double> progress)
        {
            LoggerService.Logger.Info($"Started downloading {mods.Count} mods");
            var files = await GetDownloadRequest(mods);
            LoggerService.Logger.Info($"Total bytes to install {files.BytesToDownload}");
            var modFilePair = files.ModFiles;

            var downloadedBytes = default(long);

            foreach (var mod in modFilePair.Keys)
            {
                var modFile = modFilePair[mod];
                var fileDownloadUrl = await _deserializer.GetModFileDownloadUrl(mod.Id, modFile.Id);
                if (fileDownloadUrl != null)
                {
                    if (!File.Exists($"{_folderPath}/{modFile.FileName}"))
                    {
                        LoggerService.Logger.Info($"Successfuly fetched mod's {mod.Name} file's download url");
                        var fileBytes = await Download(fileDownloadUrl);
                        if (fileBytes == Array.Empty<byte>())
                            continue;
                        using var fileStream = new FileStream($"{_folderPath}/{modFile.FileName}", FileMode.Create);
                        fileStream.Write(fileBytes, 0, fileBytes.Length);
                        downloadedBytes += modFile.FileLength;
                        double percentage = ((double)downloadedBytes / (double)files.BytesToDownload) * 100;
                        progress?.Report(percentage);
                        LoggerService.Logger.Info($"Successfuly installed {mod.Name}");
                    }
                    else
                    {
                        LoggerService.Logger.Warn($"Skipping installing {mod.Name} because its already exist");
                        continue;
                    }
                }
                else
                {
                    LoggerService.Logger.Error($"Cannot fetch mod's {mod.Name} download url, may be there is no download url");
                }
            }
        }

        private async Task<DownloadRequest> GetDownloadRequest(List<Mod> mods)
        {
            long bytesToDownload = 0;
            Dictionary<Mod, ModFile> modFiles = new Dictionary<Mod, ModFile>();

            foreach (var mod in mods)
            {
                var files = await _deserializer.GetModFiles(mod.Id, _viewState);
                var latestFile = files.FirstOrDefault();

                if (latestFile != null)
                {
                    bytesToDownload += latestFile.FileLength;
                    modFiles.Add(mod, latestFile);

                    var dependencyMods = new List<Mod>();
                    var dependencyModFiles = new List<ModFile>();
                    var visitedModIds = new HashSet<int>();

                    var queue = new Queue<ModFile>();
                    queue.Enqueue(latestFile);

                    while (queue.Count > 0)
                    {
                        var currentFile = queue.Dequeue();

                        foreach (var dependency in currentFile.Dependencies.Where(dep => dep.RelationType == FileRelationType.RequiredDependency))
                        {
                            if (visitedModIds.Contains(dependency.ModId))
                            {
                                // Skip dependencies that have already been processed
                                LoggerService.Logger.Warn($"Skipping fetching file dependency of mod {dependency.ModId} because its duplicate");
                                continue;
                            }

                            var dependencyMod = await _modDeserizlier.GetMod(dependency.ModId);
                            if (dependencyMod == null)
                            {
                                // Skip missing mods
                                LoggerService.Logger.Warn($"Skipping fetching mod {dependency.ModId} beacuse of have fetched null");
                                continue;
                            }

                            var dependencyFiles = await _deserializer.GetModFiles(dependency.ModId, _viewState);
                            var latestDependencyFile = dependencyFiles.FirstOrDefault(f => f.GameId == currentFile.GameId);

                            if (latestDependencyFile != null)
                            {
                                bytesToDownload += latestDependencyFile.FileLength;
                                LoggerService.Logger.Info($"Added mod {dependencyMod.Name} to download list");
                                modFiles.Add(dependencyMod, latestDependencyFile);
                                visitedModIds.Add(dependency.ModId);
                                dependencyMods.Add(dependencyMod);
                                dependencyModFiles.Add(latestDependencyFile);
                                queue.Enqueue(latestDependencyFile);
                            }
                        }
                    }
                }
            }

            return new DownloadRequest
            {
                BytesToDownload = bytesToDownload,
                ModFiles = modFiles
            };
        }
    }
}