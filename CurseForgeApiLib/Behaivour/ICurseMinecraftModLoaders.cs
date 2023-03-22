namespace CurseForgeApiLib.Behaivour
{
    public interface ICurseMinecraftModLoaders
    {
        public Task<string> GetMinecraftModLoaders(string version = default, bool includeAll = true);
    }
}