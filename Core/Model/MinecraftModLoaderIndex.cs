using CurseForgeApiLib.Enums;

namespace Core.Model
{
    public class MinecraftModLoaderIndex
    {
        public string Name { get; set; }
        public string GameVersion { get; set; }
        public bool Latest { get; set; }
        public bool Recommended { get; set; }
        public DateTime DateModified { get; set; }
        public ModLoaderType Type { get; set; }
    }
}