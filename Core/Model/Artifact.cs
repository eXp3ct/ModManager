using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Model
{
    public class Artifact
    {
        public string Name { get; set; }
        public long Size { get; set; }
        public DateTime CreatedOn { get; set; }
        public long Downloads { get; set; }
        public bool Selected { get; set; }
    }
}
