using Core.Enums;
using Features.Attributes;
using System.ComponentModel;

namespace Core.Model
{
    public class Mod
    {
        [HideInDataGrid] public int Id { get; set; }
        [HideInDataGrid] public int GameId { get; set; }
        public string Name { get; set; }
        [HideInDataGrid] public string Slug { get; set; }
        public string Summary { get; set; }
        [HideInDataGrid] public int? ClassId { get; set; }
        public ModStatus Status { get; set; }
        [HideInDataGrid] public ModLinks Links { get; set; }
        [HideInDataGrid] public List<Category> Categories { get; set; }
        [HideInDataGrid] public ModAsset Logo { get; set; }
        [HideInDataGrid]
        public List<ModFile> LatestFiles { get; set; }
        [HideInDataGrid] public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        [HideInDataGrid] public DateTime DateReleased { get; set; }
        [HideInDataGrid] public int GamePopularityRank { get; set; }
        public long DownloadCount { get; set; }
        public bool IsAvailable { get; set; }
        public bool Selected { get; set; }
    }
}