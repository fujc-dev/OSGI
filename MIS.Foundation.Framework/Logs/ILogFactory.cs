using System;
using System.Collections.Generic;
using System.Linq;

namespace MIS.Foundation.Framework
{
    public interface ILogFactory
    {
        ILog GetLog(string name);
    }
}
