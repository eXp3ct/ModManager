using Core.Model;
using Core.Model.Data;
using CurseForgeApiLib.Behaivour;
using CurseForgeApiLib.Enums;
using Logging;
using ModManager.Model;
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

            if (filesData != null)
            {
                LoggerService.Logger.Info($"Successfuly deserialized files {string.Join(',', files)}");
                return filesData.Data; 
            }
            else
            {
                LoggerService.Logger.Error($"Cannot deserialize files {string.Join(',', files)}");
                return null;
            }
        }

        public async Task<ModFile> GetModFile(int modId, int fileId)
        {
            var response = await _service.GetModFile(modId, fileId);
            var fileData = JsonConvert.DeserializeObject<ModFileData>(response);

            if (fileData != null)
            {
                LoggerService.Logger.Info($"Successfuly deserialized mod's {modId} file {fileId}");
                return fileData.Data; 
            }
            else
            {
                LoggerService.Logger.Error($"Cannot deserialize mod's {modId} file {fileId}");
                return null;
            }
        }

        public async Task<string?> GetModFileDownloadUrl(int modId, int fileId)
        {
            var response = await _service.GetModFileDownloadUrl(modId, fileId);
            if (string.IsNullOrEmpty(response))
                return null;
            var fileDownloadUrlData = JsonConvert.DeserializeObject<ModFileDownloadUrlData>(response);

            if (fileDownloadUrlData != null)
            {
                LoggerService.Logger.Info($"Successfuly deserialized mod's {modId} file's {fileId} download url");
                return fileDownloadUrlData.Data; 
            }
            else
            {
                LoggerService.Logger.Error($"Cannot deserialize mod's {modId} file's {fileId} download url");
                return null;
            }
        }

        public async Task<List<ModFile>> GetModFiles(int modId, string gameVersion = "", 
            ModLoaderType modLoaderType = ModLoaderType.Any, int gameVersionTypeId = 0, 
            int index = 0, int pageSize = 50)
        {
            var response = await _service.GetModFiles(modId: modId, gameVersion: gameVersion, modLoaderType: modLoaderType, gameVersionTypeId
                : gameVersionTypeId, index: index, pageSize: pageSize);
            var filesData = JsonConvert.DeserializeObject<ModsFileData>(response);

            if (filesData != null)
            {
                LoggerService.Logger.Info($"Successfuly deserialized mod's {modId} for game version {gameVersion}, mod loader {modLoaderType}");
                return filesData.Data; 
            }
            else
            {
                LoggerService.Logger.Error($"Cannot deserialize mod's {modId} for game version {gameVersion}, mod loader {modLoaderType}");
                return null;
            }
        }

        public async Task<List<ModFile>> GetModFiles(int modId, ViewState state)
        {
            return await GetModFiles(
                modId: modId,
                gameVersion: state.GameVersion,
                modLoaderType: state.ModLoaderType);
        }
    }
}
