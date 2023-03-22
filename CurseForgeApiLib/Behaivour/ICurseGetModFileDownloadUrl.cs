namespace CurseForgeApiLib.Behaivour
{
    public interface ICurseGetModFileDownloadUrl
    {
        public Task<string> GetModFileDownloadUrl(int modId, int fileId);
    }
}