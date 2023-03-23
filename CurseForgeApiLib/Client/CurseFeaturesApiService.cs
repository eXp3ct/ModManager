using CurseForgeApiLib.Behaivour;
using CurseForgeApiLib.Enums;
using CurseForgeApiLib.HttpClients;
using CurseForgeApiLib.Uris;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurseForgeApiLib.Client
{
    public class CurseFeaturesApiService : IGetCategories, IGetMinecraftVersions, IGetMinecraftModLoaders
    {
        private CurseApiClient Client { get; } = new();

        public async Task<string> GetCategories(int gameId, int classId = 0)
        {
            using var response = await Client.GetAsync(CurseForgeUris.GetEndpoint(RequestType.GetCategories) + $"?gameId={gameId}&classId={classId}&classesOnly=false");

            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> GetMinecraftModLoaders(string version = null, bool includeAll = true)
        {
            var url = CurseForgeUris.GetEndpoint(RequestType.GetMinecraftModLoaders) + $"?versions={version}&includeAll={includeAll}";
            using var response = await Client.GetAsync(url);

            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> GetMinecraftVersions(bool sortDescending = false)
        {
            var url = CurseForgeUris.GetEndpoint(RequestType.GetMinecraftVersions) + $"?sortDescending={sortDescending}";
            using var response = await Client.GetAsync(url);

            return await response.Content.ReadAsStringAsync();
        }
    }
}
