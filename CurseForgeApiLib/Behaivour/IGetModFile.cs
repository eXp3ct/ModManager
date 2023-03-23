namespace CurseForgeApiLib.Behaivour
{
    public interface IGetModFile
    {
        public Task<string> GetModFile(int modId, int fileId);
    }
}