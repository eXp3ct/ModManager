using CurseForgeApiLib.Behaivour;
using CurseForgeApiLib.Enums;
using CurseForgeApiLib.HttpClients;
using CurseForgeApiLib.Uris;
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

            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> GetModFile(int modId, int fileId)
        {
            var url = CurseForgeUris.GetEndpoint(RequestType.GetModFile, modId, fileId);
            using var response = await Client.GetAsync(url);

            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> GetModFileDownloadUrl(int modId, int fileId)
        {
            var url = CurseForgeUris.GetEndpoint(RequestType.GetModFileDownloadUrl, modId: modId, fileId: fileId);
            using var response = await Client.GetAsync(url);

            return await response.Content.ReadAsStringAsync();
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

            return await response.Content.ReadAsStringAsync();
        }
    }
}