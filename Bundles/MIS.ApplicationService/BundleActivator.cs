using OSGi.NET.Core;
using System;
using System.Collections.Generic;
using System.Linq;


namespace MIS.ApplicationService
{
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
            var bundle = sender as IBundle;
            var context = bundle.GetBundleContext();
            IStartupPageService service = new DefaultStartupPageService(bundle);
            context.RegisterService<IStartupPageService>(service);
        }
    }
}
