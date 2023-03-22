using Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Model
{
    public class Mod
    {
        public int Id { get; set; }
        public int GameId { get; set; }
        public string Name { get; set; } 
        public string Slug { get; set; }
        public string Summary { get; set; }
        public int? ClassId { get; set; }
        public ModStatus Status { get; set; }
        public ModLinks Links { get; set; }
        public List<Category> Categories { get; set; }
        public ModAsset Logo { get; set; }
        public List<ModFile> LatestFiles { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public DateTime DateReleased { get; set; }
        public int GamePopularityRank { get; set; }
        public bool IsAvailable { get; set; }
    }
}
