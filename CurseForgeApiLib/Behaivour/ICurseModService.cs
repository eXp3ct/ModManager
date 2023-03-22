using Core.Model;
using CurseForgeApiLib.Enums;

namespace CurseForgeApiLib.Behaivour
{
    public interface ICurseModService
    {
        public Task<string> SearchMods(int gameId, int classId = default,
            int categoryId = default, string gameVersion = default, string searchFilter = default,
            SearchSortFields sortField = default, string sortOrder = "asc", ModLoaderType modLoaderType = default,
            int gameVersionTypeId = default, int authorId = default, string slug = default, int index = default,
            int pageSize = default);
        public Task<string> GetMod(int modId);
        public Task<string> GetMods(List<Mod> mods);
        public Task<string> GetModDescription(int modId);
    }
}
