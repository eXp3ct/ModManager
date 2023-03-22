namespace CurseForgeApiLib.Behaivour
{
    public interface ICurseGetModFile
    {
        public Task<string> GetModFile(int modId, int fileId);
    }
}