using Core.Model;
using CurseForgeApiLib.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurseForgeApiLib.Behaivour
{
    public interface ICurseModFileApiDeserializer
    {
        public Task<ModFile> GetModFile(int modId, int fileId);
        public Task<List<ModFile>> GetFiles(List<int> files);
        public Task<string?> GetModFileDownloadUrl(int modId, int fileId);
        public Task<List<ModFile>> GetModFiles(int modId, string gameVersion = "", ModLoaderType modLoaderType = default,
            int gameVersionTypeId = default, int index = 0, int pageSize = 50);
    }
}
