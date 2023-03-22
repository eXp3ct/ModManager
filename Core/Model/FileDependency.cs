using Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Model
{
    public class FileDependency
    {
        public int ModId { get; set; }
        public FileRelationType RelationType { get; set; }
    }
}
