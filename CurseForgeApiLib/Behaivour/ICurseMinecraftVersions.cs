namespace CurseForgeApiLib.Behaivour
{
    public interface ICurseMinecraftVersions
    {
        public Task<string> GetMinecraftVersions(bool sortDescending = false);
    }
}