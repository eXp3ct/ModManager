namespace CurseForgeApiLib.Behaivour
{
    public interface IGetCategories
    {
        public Task<string> GetCategories(int gameId, int classId = default);
    }
}