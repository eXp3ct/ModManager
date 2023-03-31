using Core.Enums;
using Features.Attributes;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Core.Model
{
    public class Mod : INotifyPropertyChanged
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
        [HideInDataGrid] public List<ModFile> LatestFiles { get; set; }
        [HideInDataGrid] public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        [HideInDataGrid] public DateTime DateReleased { get; set; }
        [HideInDataGrid] public int GamePopularityRank { get; set; }
        public long DownloadCount { get; set; }
        public bool IsAvailable { get; set; }
        //public bool Selected { get; set; }

        private bool _selected;

        public bool Selected
        {
            get { return _selected; }
            set { _selected = value; OnPropertyChanged(); }
        }


        public event PropertyChangedEventHandler? PropertyChanged;

        public Mod Clone()
        {
            return new Mod
            {
                Id = this.Id,
                GameId = this.GameId,
                Name = this.Name,
                Slug = this.Slug,
                Summary = this.Summary,
                ClassId = this.ClassId,
                Status = this.Status,
                Links = this.Links,
                Categories = this.Categories,
                Logo = this.Logo,
                LatestFiles = this.LatestFiles,
                DateCreated = this.DateCreated,
                DateModified = this.DateModified,
                DateReleased = this.DateReleased,
                GamePopularityRank = this.GamePopularityRank,
                DownloadCount = this.DownloadCount,
                IsAvailable = this.IsAvailable,
                Selected = this.Selected
            };
        }

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}