using Core.Model;
using CurseForgeApiLib.Enums;

namespace CurseForgeApiLib.Behaivour
{
    public interface ICurseModApiDeserializer
    {
        public Task<Mod> GetMod(int modId);
        public Task<List<Mod>> SearchMods(int gameId, int classId = default,
            int categoryId = default, string gameVersion = default, string searchFilter = default,
            SearchSortFields sortField = default, string sortOrder = "asc", ModLoaderType modLoaderType = default,
            int gameVersionTypeId = default, int authorId = default, string slug = default, int index = default,
            int pageSize = default);
        public Task<List<Mod>> GetMods(List<int> modIds);
        //TODO Deserialize HTML 
        public Task<List<MinecraftGameVersion>> GetMinecraftGameVersions(bool sortDescending = false);
        public Task<List<MinecraftModLoaderIndex>> GetMinecraftModLoaders(string version = null, bool includeAll = true);
        public Task<string> GetModDescription(int modId);
    }
}