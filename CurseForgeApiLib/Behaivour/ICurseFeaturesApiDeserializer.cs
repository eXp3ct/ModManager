using Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurseForgeApiLib.Behaivour
{
    public interface ICurseFeaturesApiDeserializer
    {
        public Task<List<MinecraftGameVersion>> GetMinecraftGameVersions(bool sortDescending = false);
        public Task<List<MinecraftModLoaderIndex>> GetMinecraftModLoaders(string version = null, bool includeAll = true);
        public Task<List<Category>> GetCategories(int gameId, int classId = 0);
    }
}
