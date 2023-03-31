using CurseForgeApiLib.Behaivour;
using CurseForgeApiLib.Enums;
using CurseForgeApiLib.HttpClients;
using CurseForgeApiLib.Uris;
using Logging;
using Newtonsoft.Json;
using System.Text;

namespace CurseForgeApiLib.Client
{
    public class CurseModFileApiService : IGetModFile, IGetModFiles, IGetFiles, IGetModFileDownloadUrl
    {
        private CurseApiClient Client { get; } = new();

        public async Task<string> GetFiles(List<int> fileIds)
        {
            var url = CurseForgeUris.GetEndpoint(RequestType.GetFiles);
            var requestBody = JsonConvert.SerializeObject(new { fileIds });

            // Create a new HTTP request message with the POST method and request body
            var request = new HttpRequestMessage(HttpMethod.Post, url)
            {
                Content = new StringContent(requestBody, Encoding.UTF8, "application/json")
            };

            // Send the request and wait for the response
            using var response = await Client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                LoggerService.Logger.Info($"Successfuly fetched files {string.Join(',', fileIds)}");
                return await response.Content.ReadAsStringAsync(); 
            }
            else
            {
                LoggerService.Logger.Error($"Cannot fetch files {string.Join(',', fileIds)}");
                return null;
            }
        }

        public async Task<string> GetModFile(int modId, int fileId)
        {
            var url = CurseForgeUris.GetEndpoint(RequestType.GetModFile, modId, fileId);
            using var response = await Client.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                LoggerService.Logger.Info($"Successfuly fetched mods's {modId} file {fileId}");
                return await response.Content.ReadAsStringAsync(); 
            }
            else
            {
                LoggerService.Logger.Error($"Cannot fetched mods's {modId} file {fileId}");
                return null;
            }
        }

        public async Task<string> GetModFileDownloadUrl(int modId, int fileId)
        {
            var url = CurseForgeUris.GetEndpoint(RequestType.GetModFileDownloadUrl, modId: modId, fileId: fileId);
            using var response = await Client.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                LoggerService.Logger.Info($"Successfuly fetched mod's {modId} file's {fileId} download url");
                return await response.Content.ReadAsStringAsync(); 
            }
            else
            {
                LoggerService.Logger.Error($"Cannot fetch mod's {modId} file's {fileId} download url");
                return null;
            }
        }

        public async Task<string> GetModFiles(int modId, string gameVersion = "", 
            ModLoaderType modLoaderType = ModLoaderType.Any, int gameVersionTypeId = 0, int index = 0, int pageSize = 50)
        {
            var queryParams = new Dictionary<string, string>
            {
                { "gameVersion", gameVersion },
                { "modLoader", ((int)modLoaderType).ToString() },
                { "gameVersionTypeId", gameVersionTypeId == 0 ? "" : gameVersionTypeId.ToString() },
                { "index", index.ToString() },
                { "pageSize", pageSize.ToString() }
            };
            var url = CurseForgeUris.GetEndpoint(RequestType.GetModFiles, modId);
            if (queryParams.Any())
                url += "?" + string.Join("&", queryParams.Select(q => $"{q.Key}={q.Value}"));

            using var response = await Client.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                LoggerService.Logger.Info($"Successfuly fetched mod's {modId} files for the game version {gameVersion}, mod loader {modLoaderType}");
                return await response.Content.ReadAsStringAsync(); 
            }
            else
            {
                LoggerService.Logger.Error($"Cannot fetch mod's {modId} files for the game version {gameVersion}, mod loader {modLoaderType}");
                return null;
            }
        }
    }
}