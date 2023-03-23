using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurseForgeApiLib.Behaivour
{
    public interface IGetModDescription
    {
        public Task<string> GetModDescription(int modId);
    }
}
