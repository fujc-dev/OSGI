using OSGi.NET.Core;
using System;
using System.Collections.Generic;
using System.Linq;


namespace MIS.ApplicationService
{
    public class BundleActivator : IBundleActivator
    {


        //private IServiceRegistration serviceRegistration;



        public void Start(IBundleContext context)
        {
            //Bundle正在启动

            context.ExtensionChanged += ContextOnExtensionChanged;
        }

        public void Stop(IBundleContext context)
        {
            //serviceRegistration.Unregister();
        }

        private void ContextOnExtensionChanged(object sender, OSGi.NET.Event.ExtensionEventArgs e)
        {
            IBundle bundle = sender as IBundle;
            IBundleContext context = bundle.GetBundleContext();
            IStartupPageService service = new DefaultStartupPageService(bundle);
            context.RegisterService<IStartupPageService>(service);
        }
    }
}
