using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enums
{
    public enum FileRelationType
    {
        EmbeddedLibrary = 1,
        OptionalDependency = 2,
        RequiredDependency = 3,
        Tool = 4,
        Incompatible = 5,
        Include = 6
    }
}
