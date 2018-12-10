using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UpdateEngine.Enums
{
    public enum UpdateEnvironment
    {
        Development,
        Test,
        Stage,
        Production,
        Unknow
    }

    public enum UpdateStatus
    {
        NeverApplied,
        Applied,
        Rollbacked,
        Unknow
    }
}
