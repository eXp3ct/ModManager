using Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Model
{
    public class ModFile
    {
        public int Id { get; set; }
        public int GameId { get; set; }
        public int ModId { get; set; }
        public bool IsAvailable { get; set; }
        public string DisplayName { get; set; }
        public string FileName { get; set; }
        public FileStatus FileStatus { get; set; }
        public DateTime FileDate { get; set; }
        public long FileLength { get; set; }
        public long DownloadCount { get; set; }
        public string DownloadUrl { get; set; }
        public List<string> GameVersions { get; set; }
        public List<FileDependency> Dependencies { get; set; }
    }
}
