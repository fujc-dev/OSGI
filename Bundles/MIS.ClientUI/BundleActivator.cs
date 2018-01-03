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
        public void Start(IBundleContext context)
        {
            context.ExtensionChanged += ContextOnExtensionChanged;
        }



        public void Stop(IBundleContext context)
        {
        }



        private void ContextOnExtensionChanged(object sender, OSGi.NET.Event.ExtensionEventArgs e)
        {
            var stateStr = string.Empty;
            if (e.GetState() == ExtensionEventArgs.LOAD)
            {
                stateStr = "Load";
            }
            else
            {
                if (e.GetState() == ExtensionEventArgs.UNLOAD)
                {
                    stateStr = "UnLoad";
                }
            }
            var extensionStr = new StringBuilder();
            foreach (var xmlNode in e.GetExtensionData().ExtensionList)
            {
                extensionStr.Append(xmlNode.InnerXml);
            }
            MessageBox.Show(string.Format("{0} {1} {2} Extension {3}", ((IBundle)sender).GetSymbolicName(), stateStr, e.GetExtensionPoint().Name, extensionStr));
        }
    }
}
