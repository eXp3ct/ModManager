using Core.Model;
using Core.Model.Data;
using CurseForgeApiLib.Behaivour;
using Logging;
using Newtonsoft.Json;

namespace CurseForgeApiLib.Client
{
    public class CurseFeaturesApiDeserializer : ICurseFeaturesApiDeserializer
    {
        private readonly CurseFeaturesApiService _service = new();

        public async Task<List<Category>> GetCategories(int gameId, int classId = 0)
        {
            var response = await _service.GetCategories(gameId, classId);
            var categoryData = JsonConvert.DeserializeObject<CategoryData>(response);

            if(categoryData != null)
            {
                LoggerService.Logger.Info($"Successfuly deserialized categories");

                return categoryData.Data;
            }
            else
            {
                LoggerService.Logger.Error("Cannot deserizler categories");

                return null;
            }
        }

        public async Task<List<MinecraftGameVersion>> GetMinecraftGameVersions(bool sortDescending = false)
        {
            var response = await _service.GetMinecraftVersions(sortDescending);
            var versionsData = JsonConvert.DeserializeObject<MinecraftVersionsData>(response);

            if(versionsData != null)
            {
                LoggerService.Logger.Info($"Successfuly deserialized minecraft game versions");

                return versionsData.Data;
            }
            else
            {
                LoggerService.Logger.Error("Cannot deserizele minecraft game versions");

                return null;
            }
        }

        public async Task<List<MinecraftModLoaderIndex>> GetMinecraftModLoaders(string version = null, bool includeAll = true)
        {
            var response = await _service.GetMinecraftModLoaders(version, includeAll);
            var modloadersData = JsonConvert.DeserializeObject<MinecraftModLoadersData>(response);

            if(modloadersData != null)
            {
                LoggerService.Logger.Info($"Successfuly deserialized minecraft mod loaders");

                return modloadersData.Data;
            }
            else
            {
                LoggerService.Logger.Error($"Cannot deserizler minecraft mod loaders");

                return null;
            }
        }
    }
}
