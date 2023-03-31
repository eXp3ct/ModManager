using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Model
{
    public class DependencyFound
    {
        public string DependencyString { get; set; }
        public List<Mod> DependencyMods { get; set; }
    }
}
