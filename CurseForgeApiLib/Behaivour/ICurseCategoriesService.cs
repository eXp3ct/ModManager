﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurseForgeApiLib.Behaivour
{
    public interface ICurseCategoriesService
    {
        public Task<string> GetCategories(int gameId, int classId = default);
    }
}
