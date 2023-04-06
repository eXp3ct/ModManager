using CurseForgeApiLib.Behaivour;
using CurseForgeApiLib.Enums;
using CurseForgeApiLib.HttpClients;
using CurseForgeApiLib.Uris;
using Logging;

namespace CurseForgeApiLib.Client
{
    public class CurseFeaturesApiService : IGetCategories, IGetMinecraftVersions, IGetMinecraftModLoaders
    {
        private CurseApiClient Client { get; } = new();


        public async Task<string> GetCategories(int gameId, int classId = 0)
        {
            using var response = await Client.GetAsync(CurseForgeUris.GetEndpoint(RequestType.GetCategories) + $"?gameId={gameId}&classId={classId}&classesOnly=false");
            if (response.IsSuccessStatusCode)
            {
                LoggerService.Logger.Info("Successfuly fetched categories");
                return await response.Content.ReadAsStringAsync();
            }
            else
            {
                LoggerService.Logger.Error($"Cannot get categories for game {gameId}");
                return string.Empty;
            }
        }

        public async Task<string> GetMinecraftModLoaders(string version = null, bool includeAll = true)
        {
            var url = CurseForgeUris.GetEndpoint(RequestType.GetMinecraftModLoaders) + $"?versions={version}&includeAll={includeAll}";
            using var response = await Client.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                LoggerService.Logger.Info("Successfuly fetched minecraft mod loaders");

                return await response.Content.ReadAsStringAsync();
            }
            else
            {
                LoggerService.Logger.Error($"Cannot get minecraft mod loaders for version {version}");
                return string.Empty;
            }
        }

        public async Task<string> GetMinecraftVersions(bool sortDescending = false)
        {
            var url = CurseForgeUris.GetEndpoint(RequestType.GetMinecraftVersions) + $"?sortDescending={sortDescending}";
            using var response = await Client.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                LoggerService.Logger.Info("Successfuly fetched minecraft versions");

                return await response.Content.ReadAsStringAsync();
            }
            else
            {
                LoggerService.Logger.Error($"Cannot get minecraft versions");
                return string.Empty;
            }
        }
    }
}
