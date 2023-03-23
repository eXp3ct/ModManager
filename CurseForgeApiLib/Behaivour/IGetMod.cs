using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurseForgeApiLib.Behaivour
{
    public interface IGetMod
    {
        public Task<string> GetMod(int modId);
    }
}
