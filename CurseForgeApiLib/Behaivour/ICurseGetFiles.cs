using Core.Model;

namespace CurseForgeApiLib.Behaivour
{
    public interface ICurseGetFiles
    {
        public Task<string> GetFiles(List<int> files);
    }
}