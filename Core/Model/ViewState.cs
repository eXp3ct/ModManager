using CurseForgeApiLib.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ModManager.Model
{
    public class ViewState : INotifyPropertyChanged
    {
        private int _gameId;
        public int GameId
        {
            get { return _gameId; }
            set { _gameId = value; OnPropertyChanged(); }
        }

        private int _classId;
        public int ClassId
        {
            get { return _classId; }
            set { _classId = value; OnPropertyChanged(); }
        }

        private int? _categoryId;
        public int? CategoryId
        {
            get { return _categoryId; }
            set { _categoryId = value; OnPropertyChanged(); }
        }

        private string _gameVersion;
        public string GameVersion
        {
            get { return _gameVersion; }
            set
            {
                if (_gameVersion != value)
                {
                    _gameVersion = value;
                    OnPropertyChanged(nameof(GameVersion));
                }
            }
        }

        private string? _searchFilter;
        public string? SearchFilter
        {
            get { return _searchFilter; }
            set { _searchFilter = value; OnPropertyChanged(); }
        }

        private SearchSortFields? _sortFields;
        public SearchSortFields? SortFields
        {
            get { return _sortFields; }
            set { _sortFields = value; OnPropertyChanged(); }
        }

        private string? _sortOrder;
        public string? SortOrder
        {
            get { return _sortOrder; }
            set { _sortOrder = value; OnPropertyChanged(); }
        }

        private ModLoaderType? _modLoaderType;
        public ModLoaderType? ModLoaderType
        {
            get { return _modLoaderType; }
            set { _modLoaderType = value; OnPropertyChanged(); }
        }

        private int? _gameVersionTypeId;
        public int? GameVersionTypeId
        {
            get { return _gameVersionTypeId; }
            set { _gameVersionTypeId = value; OnPropertyChanged(); }
        }

        private int? _authorId;
        public int? AuthorId
        {
            get { return _authorId; }
            set { _authorId = value; OnPropertyChanged(); }
        }

        private string? _slug;
        public string? Slug
        {
            get { return _slug; }
            set { _slug = value; OnPropertyChanged(); }
        }

        private int _index;
        public int Index
        {
            get { return _index; }
            set { _index = value; OnPropertyChanged(); }
        }

        private int _pageSize;
        public int PageSize
        {
            get { return _pageSize; }
            set { _pageSize = value; OnPropertyChanged(); }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
