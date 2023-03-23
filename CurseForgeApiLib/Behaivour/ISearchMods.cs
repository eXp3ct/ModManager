using CurseForgeApiLib.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurseForgeApiLib.Behaivour
{
    public interface ISearchMods
    {
        public Task<string> SearchMods(int gameId, int classId = default,
            int categoryId = default, string gameVersion = default, string searchFilter = default,
            SearchSortFields sortField = default, string sortOrder = "asc", ModLoaderType modLoaderType = default,
            int gameVersionTypeId = default, int authorId = default, string slug = default, int index = default,
            int pageSize = default);
    }
}
