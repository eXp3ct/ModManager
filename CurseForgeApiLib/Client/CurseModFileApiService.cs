using CurseForgeApiLib.Behaivour;
using CurseForgeApiLib.Uris;
using CurseForgeApiLib.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Model;
using Newtonsoft.Json;

namespace CurseForgeApiLib.Client
{
    public class CurseModFileApiService : ICurseGetModFile, ICurseGetModFiles, ICurseGetFiles, ICurseGetModFileDownloadUrl
    {
        public async Task<string> GetFiles(List<int> files)
        {
            using var client = new WebClient.Client();
            var json = JsonConvert.SerializeObject(files);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            using var response = await client.PostAsync(CurseForgeUris.GetEndpoint(RequestType.GetFiles), content);

            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> GetModFile(int modId, int fileId)
        {
            using var client = new WebClient.Client();
            var url = CurseForgeUris.GetEndpoint(RequestType.GetModFile, modId, fileId);
            using var response = await client.GetAsync(url);

            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> GetModFileDownloadUrl(int modId, int fileId)
        {
            using var client = new WebClient.Client();
            var url = CurseForgeUris.GetEndpoint(RequestType.GetModFileDownloadUrl, modId: modId, fileId: fileId);
            using var response = await client.GetAsync(url);

            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> GetModFiles(int modId, string gameVersion = "", ModLoaderType modLoaderType = ModLoaderType.Any, int gameVersionTypeId = 0, int index = 0, int pageSize = 50)
        {
            using var client = new WebClient.Client();
            var queryParams = new Dictionary<string, string>
            {
                { "gameVersion", gameVersion },
                { "modLoader", modLoaderType.ToString().ToLowerInvariant() },
                { "gameVersionTypeId", gameVersionTypeId.ToString() },
                { "index", index.ToString() },
                { "pageSize", pageSize.ToString() }
            };
            var url = CurseForgeUris.GetEndpoint(RequestType.GetModFiles, modId);
            if (queryParams.Any())
                url += "?" + string.Join("&", queryParams.Select(q => $"{q.Key}={q.Value}"));

            using var response = await client.GetAsync(url);

            return await response.Content.ReadAsStringAsync();
        }
    }
}
