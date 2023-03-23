namespace CurseForgeApiLib.Behaivour
{
    public interface IGetModFileDownloadUrl
    {
        public Task<string> GetModFileDownloadUrl(int modId, int fileId);
    }
}