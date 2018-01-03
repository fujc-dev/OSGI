using OSGi.NET;
using OSGi.NET.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace MIS.Shell
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            base.Startup += App_Startup;
            base.Exit += App_Exit;
        }

        private void App_Startup(object sender, StartupEventArgs e)
        {
            using (var bundleRuntime = new BundleRuntime())
            {
                bundleRuntime.Start();
                var pageFlowService = bundleRuntime.GetFirstOrDefaultService("MIS.ApplicationService.IStartupPageService");
                var type = pageFlowService.GetType();
                var ClassReflection = type.GetProperty("ClassReflection");
                var Owner = type.GetProperty("Owner");
                var o1 = (String)ClassReflection.GetValue(pageFlowService, null);
                var bundle = (Bundle)Owner.GetValue(pageFlowService, null);
                var app = Application.Current;
                app.ShutdownMode = ShutdownMode.OnLastWindowClose;
                app.MainWindow = bundle.LoadClass(o1) as Window;
                app.MainWindow.Show();
            }
        }

        private void App_Exit(object sender, ExitEventArgs e)
        {
        }
    }
}
