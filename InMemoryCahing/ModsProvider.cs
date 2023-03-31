using Core.Model;
using CurseForgeApiLib.Client;
using Microsoft.Extensions.Caching.Memory;
using ModManager.Model;
using NLog;
namespace InMemoryCahing
{
    public class ModsProvider
    {
        public event EventHandler<IList<Mod>> SelectedModsChanged;
        private static readonly ILogger logger = LogManager.GetCurrentClassLogger();
        private readonly IMemoryCache _cache = new MemoryCache(new MemoryCacheOptions
        {
            SizeLimit = 1000,
            ExpirationScanFrequency = TimeSpan.FromMinutes(5)
        });
        private readonly MemoryCacheEntryOptions _entryOptions = new MemoryCacheEntryOptions()
            .SetSlidingExpiration(TimeSpan.FromHours(1))
            .SetAbsoluteExpiration(TimeSpan.FromHours(3));
        private List<Mod> _selectedMods { get; set; } = new();

        public ModsProvider()
        {
            LogManager.Configuration = new NLog.Config.XmlLoggingConfiguration(@"D:\Projects\C# Projects\ModManager\InMemoryCahing\NLog.config");
        }
        public IList<Mod> GetSelectedMods()
        {
            return _selectedMods.ToList();
        }
        public void ClearSelectedMods()
        {
            _selectedMods.Clear();
        }

        public void SetSelectedMods(List<Mod> mods)
        {
            mods.ForEach(mod => _selectedMods.Add(mod.Clone()));
        }

        private void OnSelectedModsChanged()
        {
            SelectedModsChanged?.Invoke(this, _selectedMods);
        }
        public async Task<IList<Mod>> GetMods(ViewState state)
        {
            string cacheKey = GetCacheKey(state);

            if (_cache.TryGetValue(cacheKey, out IList<Mod> mods))
            {
                logger.Info($"Cache hit for key {cacheKey}");
                return mods;
            }
            logger.Info($"Cache miss for key {cacheKey}");
            mods = await FetchMods(state);
            _cache.Set(cacheKey, mods, _entryOptions.SetSize(10));

            return mods;
        }

        private async Task<IList<Mod>> FetchMods(ViewState state)
        {
            var modDeserializer = new CurseModApiDeserializer(new CurseModApiService());
            var mods = await modDeserializer.SearchMods(state);
            var clonnedMods = mods.Select(mod => mod.Clone()).ToList();

            foreach(var mod in clonnedMods)
            {
                mod.PropertyChanged += Mod_PropertyChanged;
            }

            return clonnedMods;
        }

        private void Mod_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Mod.Selected))
            {
                var mod = (Mod)sender;
                if (mod.Selected)
                {
                    _selectedMods.Add(mod);
                }
                else
                {
                    _selectedMods.Remove(mod);
                }

                OnSelectedModsChanged();
            }
        }

        private string GetCacheKey(ViewState state)
        {
            return $"Mods_{state.GameId}_{state.ClassId}_{state.AuthorId}_{state.CategoryId}_{state.GameVersion}_{state.GameVersionTypeId}_{state.Index}_{state.ModLoaderType}_{state.PageSize}_{state.SearchFilter}_{state.Slug}_{state.SortFields}_{state.SortOrder}";
        }
    }
}