using Core.Enums;

namespace Core.Model
{
    public class FileDependency
    {
        public int ModId { get; set; }
        public FileRelationType RelationType { get; set; }
    }
}