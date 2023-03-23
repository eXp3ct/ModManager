namespace Core.Model
{
    public class MinecraftGameVersion
    {
        public int Id { get; set; }
        public int GameVersionId { get; set; }
        public string VersionString { get; set; }
        public string JarDownloadUrl { get; set; }
        public string JsonDownloadUrl { get; set; }
        public bool Approved { get; set; }
        public DateTime DateModified { get; set; }
        public int GameVersionTypeId { get; set; }
    }
}