namespace MIS.ApplicationService
{
    using OSGi.NET.Core;
    using System;
    using System.Collections.Generic;

    public class DefaultStartupPageService : IStartupPageService
    {
        /// <summary>
        /// 启动项扩展点
        /// </summary>
        private const String MIS_APPLICATION_SERVICE_STARTUP = "MIS.ApplicationService.Startup";
        private IBundle bundle;
        public DefaultStartupPageService(IBundle bundle)
        {
            this.bundle = bundle;
            HandleStartupPage();
        }

        private void HandleStartupPage()
        {
            var extensions = bundle.GetExtensionDatas();
            foreach (var item in extensions)
            {
                if (item.Name.Equals(MIS_APPLICATION_SERVICE_STARTUP))
                {
                    foreach (var ex in item.ExtensionList)
                    {
                        _ClassReflection = ex.FirstChild.Attributes["Value"].Value;
                    }
                }
            }
        }

        private string _ClassReflection = string.Empty;
        public string ClassReflection
        {
            get
            {
                return _ClassReflection;
            }
        }




        public IBundle Owner
        {
            get
            {
                return bundle;
            }
        }
    }
}

