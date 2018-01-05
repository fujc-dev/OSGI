using MIS.ClientUI.Core;
using OSGi.NET.Core;
using OSGi.NET.Event;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace MIS.ClientUI
{
    /// <summary>
    /// 主窗体插件
    /// </summary>
    public class BundleActivator : IBundleActivator
    {
        /// <summary>
        /// 当前Bundle上下文实例
        /// </summary>
        public static IBundleContext Instance;
        public void Start(IBundleContext context)
        {
            Instance = context;
            context.ExtensionChanged += ContextOnExtensionChanged;
        }

        public void Stop(IBundleContext context)
        {
        }

        /// <summary>
        /// 当其他模块实现了扩展点时，会收到消息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ContextOnExtensionChanged(object sender, OSGi.NET.Event.ExtensionEventArgs e)
        {
            var bundle = sender as IBundle;
            IShellResolveService resolveService = new DefaultShellResolveService(bundle, e.GetExtensionData());
            Bootstrap.AddShellResolveService(resolveService);
        }
    }
}
