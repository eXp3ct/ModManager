namespace CurseForgeApiLib.Behaivour
{
    public interface ICurseCategoriesService
    {
        public Task<string> GetCategories(int gameId, int classId = default);
    }
}