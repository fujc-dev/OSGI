using MIS.ClientUI.Core;
using OSGi.NET.Event;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MIS.ClientUI
{
    /// <summary>
    /// 启动向导(使用此类的公共方法操作主界面)
    /// </summary>
    public class Bootstrap
    {
        /// <summary>
        /// 存储其他插件扩展到Shell中的扩展数据(后续会使用此数据将其他插件绘制到Shell程序的界面)
        /// </summary>
        //private static Dictionary<String, IShellResolveService> mContainers = new Dictionary<string, IShellResolveService>();
        private static List<IShellResolveService> mContainers = new List<IShellResolveService>();

        public static void AddShellResolveService(IShellResolveService service)
        {
            mContainers.Add(service);
        }

        public static List<IShellResolveService> GetService() { return mContainers; }



    }
}
