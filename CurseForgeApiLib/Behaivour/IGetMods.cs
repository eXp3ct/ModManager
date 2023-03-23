using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurseForgeApiLib.Behaivour
{
    public interface IGetMods
    {
        public Task<string> GetMods(List<int> mods);
    }
}
