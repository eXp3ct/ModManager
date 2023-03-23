using Core.Model;

namespace CurseForgeApiLib.Behaivour
{
    public interface IGetFiles
    {
        public Task<string> GetFiles(List<int> files);
    }
}