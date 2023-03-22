using Core.Model;
using CurseForgeApiLib.Behaivour;
using CurseForgeApiLib.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurseForgeApiLib.Client
{
    public class CurseApiDeserializer : ICurseApiDeserializer
    {
        private CurseModApiService _service;

        public CurseApiDeserializer(CurseModApiService service)
        {
            _service = service;
        }

        public Task<List<MinecraftGameVersion>> GetMinecraftGameVersions(bool sortDescending = false)
        {
            throw new NotImplementedException();
        }

        public Task<List<MinecraftModLoaderIndex>> GetMinecraftModLoaders(string version = null, bool includeAll = true)
        {
            throw new NotImplementedException();
        }

        public async Task<Mod> GetMod(int modId)
        {
            var response = await _service.GetMod(modId);
            var modData = JsonConvert.DeserializeObject<ModData>(response);

            return modData.Data;
        }

        public Task<List<Mod>> GetMods(List<int> modIds)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Mod>> SearchMods(int gameId = 432, int classId = 6, 
            int categoryId = 0, string gameVersion = null, 
            string searchFilter = null, SearchSortFields sortField = 0, 
            string sortOrder = "asc", ModLoaderType modLoaderType = ModLoaderType.Any, 
            int gameVersionTypeId = 0, int authorId = 0, 
            string slug = null, int index = 0, int pageSize = 50)
        {
            var response = await _service.SearchMods(gameId: 432, classId: 6, categoryId: categoryId, gameVersion: gameVersion,
                searchFilter: searchFilter, sortField: sortField, sortOrder: sortOrder, modLoaderType: modLoaderType,
                gameVersionTypeId: gameVersionTypeId, authorId: authorId, slug: slug, index: index, pageSize: pageSize);

            var modsData = JsonConvert.DeserializeObject<ModsData>(response);
            var mods = new List<Mod>(modsData.Data.Count);

            foreach (var mod in modsData.Data)
                mods.Add(mod);

            return mods;

        }
    }
}
