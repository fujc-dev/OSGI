using OSGi.NET.Core.Root;
using OSGi.NET.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace OSGi.NET
{
    public class BundleRuntime : IDisposable
    {

        #region 成员变量
        private static BundleRuntime _BundleRuntime = null;
        private static Object lockObj = "lock";
        #endregion

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

        #region CLR属性
        public IFramework Framework { get; private set; }
        #endregion

        /// <summary>
        /// 启动OSGI.NET核心库
        /// </summary>
        public void Start()
        {
            //创建框架工厂
            var frameworkFactory = new FrameworkFactory();
            //创建框架内核
            this.Framework = frameworkFactory.CreateFramework();
            //初始化框架
            this.Framework.Init();
            //启动框架
            this.Framework.Start();
            //

        }

        /// <summary>
        /// 以泛型方式根据服务约束类型获取服务
        /// </summary>
        /// <typeparam name="T">服务约束类型</typeparam>
        /// <returns>服务实例</returns>
        public Object GetFirstOrDefaultService(String contract)
        {
            IServiceReference serviceReference = this.Framework.GetBundleContext().GetServiceReference(contract);
            return serviceReference.GetService();
        }

        public String GetObjectPropertyValue<T>(T t, string propertyname)
        {
            Type type = typeof(T);

            PropertyInfo property = type.GetProperty(propertyname);

            if (property == null) return string.Empty;

            object o = property.GetValue(t, null);

            if (o == null) return string.Empty;

            return o.ToString();
        }

        #region IDisposable Interface Implement
        public void Dispose()
        {
            //throw new NotImplementedException();
        }
        #endregion
    }
}
