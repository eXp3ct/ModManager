using Core.Model;
using Core.Model.Data;
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
    public class CurseModFileApiDeserializer : ICurseModFileApiDeserializer
    {
        private readonly CurseModFileApiService _service;

        public CurseModFileApiDeserializer(CurseModFileApiService service)
        {
            _service = service;
        }

        public async Task<List<ModFile>> GetFiles(List<int> files)
        {
            var response = await _service.GetFiles(files);
            var filesData = JsonConvert.DeserializeObject<ModsFileData>(response);

            return filesData.Data;
        }

        public async Task<ModFile> GetModFile(int modId, int fileId)
        {
            var response = await _service.GetModFile(modId, fileId);
            var fileData = JsonConvert.DeserializeObject<ModFileData>(response);

            return fileData.Data;
        }

        public async Task<string> GetModFileDownloadUrl(int modId, int fileId)
        {
            var response = await _service.GetModFileDownloadUrl(modId, fileId);
            var fileDownloadUrlData = JsonConvert.DeserializeObject<ModFileDownloadUrlData>(response);

            return fileDownloadUrlData.Data;
        }

        public async Task<List<ModFile>> GetModFiles(int modId, string gameVersion = "", 
            ModLoaderType modLoaderType = ModLoaderType.Any, int gameVersionTypeId = 0, 
            int index = 0, int pageSize = 50)
        {
            var response = await _service.GetModFiles(modId: modId, gameVersion: gameVersion, modLoaderType: modLoaderType, gameVersionTypeId
                : gameVersionTypeId, index: index, pageSize: pageSize);
            var filesData = JsonConvert.DeserializeObject<ModsFileData>(response);

            return filesData.Data;
        }
    }
}
