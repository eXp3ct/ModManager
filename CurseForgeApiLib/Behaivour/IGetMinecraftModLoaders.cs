namespace CurseForgeApiLib.Behaivour
{
    public interface IGetMinecraftModLoaders
    {
        public Task<string> GetMinecraftModLoaders(string version = default, bool includeAll = true);
    }
}