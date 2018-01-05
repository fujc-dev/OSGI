using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MIS.ClientUI.Core
{
    /// <summary>
    /// 扩展数目异常，一般情况下
    /// </summary>
    public class ExtensionPointNumberException : Exception
    {
        public ExtensionPointNumberException(String message) : base(message) { }
    }
}
