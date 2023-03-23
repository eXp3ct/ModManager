using Core.Model;
using Core.Model.Data;
using CurseForgeApiLib.Behaivour;
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

            return categoryData.Data;
        }

        public async Task<List<MinecraftGameVersion>> GetMinecraftGameVersions(bool sortDescending = false)
        {
            var response = await _service.GetMinecraftVersions(sortDescending);
            var versionsData = JsonConvert.DeserializeObject<MinecraftVersionsData>(response);

            return versionsData.Data;
        }

        public async Task<List<MinecraftModLoaderIndex>> GetMinecraftModLoaders(string version = null, bool includeAll = true)
        {
            var response = await _service.GetMinecraftModLoaders(version, includeAll);
            var modloadersData = JsonConvert.DeserializeObject<MinecraftModLoadersData>(response);

            return modloadersData.Data;
        }
    }
}
