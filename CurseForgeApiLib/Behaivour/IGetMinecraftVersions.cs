namespace CurseForgeApiLib.Behaivour
{
    public interface IGetMinecraftVersions
    {
        public Task<string> GetMinecraftVersions(bool sortDescending = false);
    }
}