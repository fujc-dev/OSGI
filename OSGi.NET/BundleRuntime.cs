using OSGi.NET.Core.Root;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OSGi.NET
{
    public class BundleRuntime : IDisposable
    {
        private static BundleRuntime _BundleRuntime = null;
        private static Object lockObj = "lock";

        public static BundleRuntime Instance
        {
            get
            {
                lock (lockObj)
                {
                    if (_BundleRuntime == null)
                    {
                        _BundleRuntime = new BundleRuntime();
                    }
                    return _BundleRuntime;
                }
            }
        }

        public IFramework Framework { get; private set; }

        /// <summary>
        /// 启动OSGI.NET核心库
        /// </summary>
        public void Start()
        {
            var frameworkFactory = new FrameworkFactory();
            Framework = frameworkFactory.CreateFramework();
            Framework.Init();
            Framework.Start();
        }

        /// <summary>
        /// 以泛型方式根据服务约束类型获取服务
        /// </summary>
        /// <typeparam name="T">服务约束类型</typeparam>
        /// <returns>服务实例</returns>
        public Object GetFirstOrDefaultService(String contract)
        {
            var serviceReference = Framework.GetBundleContext().GetServiceReference(contract);
            return serviceReference.GetService();
        }

        public String GetObjectPropertyValue<T>(T t, string propertyname)
        {
            var type = typeof(T);

            var property = type.GetProperty(propertyname);

            if (property == null)
            {
                return string.Empty;
            }
            var o = property.GetValue(t, null);

            if (o == null)
            {
                return string.Empty;
            }
            return o.ToString();
        }

        public void Dispose()
        {
        }
    }
}
