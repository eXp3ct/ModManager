﻿using CurseForgeApiLib.Enums;

namespace CurseForgeApiLib.Behaivour
{
    public interface IGetModFiles
    {
        public Task<string> GetModFiles(int modId, string gameVersion = "", ModLoaderType modLoaderType = default,
            int gameVersionTypeId = default, int index = 0, int pageSize = 50);
    }
}