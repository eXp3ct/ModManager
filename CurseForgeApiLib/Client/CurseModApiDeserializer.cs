using Core.Model;
using Core.Model.Data;
using CurseForgeApiLib.Behaivour;
using CurseForgeApiLib.Enums;
using ModManager.Model;
using Newtonsoft.Json;

namespace CurseForgeApiLib.Client
{
    public class CurseModApiDeserializer : ICurseModApiDeserializer
    {
        private readonly CurseModApiService _service;

        public CurseModApiDeserializer(CurseModApiService service)
        {
            _service = service;
        }

        public async Task<Mod> GetMod(int modId)
        {
            var response = await _service.GetMod(modId);
            var modData = JsonConvert.DeserializeObject<ModData>(response);

            return modData.Data;
        }

        public async Task<string> GetModDescription(int modId)
        {
            var response = await _service.GetModDescription(modId);
            var descriptionData = JsonConvert.DeserializeObject<ModDescriptionData>(response);

            return descriptionData.Data;
        }

        public async Task<List<Mod>> GetMods(List<int> modIds)
        {
            var response = await _service.GetMods(modIds);

            var modsData = JsonConvert.DeserializeObject<ModsData>(response);

            return modsData.Data;
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

            return modsData.Data;
        }

        public async Task<List<Mod>> SearchMods(ViewState state)
        {
            return await SearchMods(
                gameId: state.GameId,
                classId: state.ClassId,
                categoryId: state.CategoryId ?? 0,
                gameVersion: state.GameVersion,
                searchFilter: state.SearchFilter,
                sortField: state.SortFields ?? 0,
                sortOrder: state.SortOrder ?? "asc",
                modLoaderType: state.ModLoaderType ?? ModLoaderType.Any,
                gameVersionTypeId: state.GameVersionTypeId ?? 0,
                authorId: state.AuthorId ?? 0,
                slug: state.Slug,
                index: state.Index,
                pageSize: state.PageSize
            );
        }
    }
}
