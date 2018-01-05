using MIS.UI.Framework.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace MIS.ClientUI.Core
{
    /// <summary>
    /// 插件外壳解析服务
    /// </summary>
    public interface IShellResolveService
    {
        /// <summary>
        /// 创建导航模块
        /// </summary>
        /// <returns></returns>
        NavigationUIItem CreateNavigationUIItem();

        void SetShellResolveEventHandler(ShellResolveEventHandler handler);

        void SetShellResolveFreamEventHandler(ShellResolveFreamEventHandler handler);
    }
}
