using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Model
{
    public class DownloadRequest
    {
        public long BytesToDownload { get; set; }
        public Dictionary<Mod, ModFile> ModFiles { get; set; }
    }
}
