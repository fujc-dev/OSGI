using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using log4net;

using OSGi.NET.Utils;

namespace OSGi.NET.Provider
{
    /// <summary>
    /// Bundle关联程序集提供程序
    /// </summary>
    public static class BundleAssemblyProvider
    {
        private static ILog log = LogManager.GetLogger(typeof(BundleAssemblyProvider));

        /// <summary>
        /// 所有Bundle引用程序集
        /// </summary>
        private static readonly IDictionary<string, Assembly> AllBundleRefAssemblyDict;

        /// <summary>
        /// 通过反射加载的程序集
        /// </summary>
        private static readonly IDictionary<string, Assembly> AllShareRefAssemblyDict;

        /// <summary>
        /// 静态构造
        /// </summary>
        static BundleAssemblyProvider()
        {
            AllBundleRefAssemblyDict = new Dictionary<string, Assembly>();
            AllShareRefAssemblyDict = new Dictionary<string, Assembly>();
            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomainAssemblyResolve;
        }


        /// <summary>
        /// 当前域加载程序集触发事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        private static Assembly CurrentDomainAssemblyResolve(object sender, ResolveEventArgs args)
        {
            var assembly = GetAssembly(args.Name);
            if (assembly == null)
            {
                if (!args.Name.Contains(".resources")
                    && !args.Name.Contains(".XmlSerializers")
                )
                {
                    log.Error(string.Format("程序集[{0}]未加载！", args.Name));
                }
            }
            return assembly;
        }

        /// <summary>
        /// 加入到程序集管理清单
        /// </summary>
        /// <param name="assemblyName">程序集名称</param>
        /// <param name="assembly">程序集</param>
        internal static void AddAssembly(string assemblyName, Assembly assembly)
        {
            if (AllBundleRefAssemblyDict.ContainsKey(assemblyName))
            {
                return;
            }
            AllBundleRefAssemblyDict.Add(assemblyName, assembly);
        }

        /// <summary>
        /// 从程序集管理清单移除
        /// </summary>
        /// <param name="assemblyName">程序集</param>
        internal static void RemoveAssembly(string assemblyName)
        {
            if (AllBundleRefAssemblyDict.ContainsKey(assemblyName))
            {
                AllBundleRefAssemblyDict.Remove(assemblyName);
            }
        }


        /// <summary>
        /// 添加程序集至共享程序集管理清单
        /// </summary>
        /// <param name="assemblyName">程序集名称</param>
        /// <param name="assembly">程序集</param>
        internal static void AddShareAssembly(string assemblyName, Assembly assembly)
        {
            if (AllShareRefAssemblyDict.ContainsKey(assemblyName))
            {
                return;
            }
            AllShareRefAssemblyDict.Add(assemblyName, assembly);
        }

        /// <summary>
        /// 检测程序集是否属于共享程序集
        /// </summary>
        /// <param name="assemblyFullName">程序集全名称</param>
        /// <returns>是否存在</returns>
        internal static bool CheckHasShareLib(string assemblyFullName)
        {
            var flag = false;
            var assemblyName = new AssemblyName(assemblyFullName);
            foreach (var assemblyKey in AllShareRefAssemblyDict.Keys)
            {
                var resovleAssemblyName = new AssemblyName(assemblyKey);
                if (assemblyName.Name == resovleAssemblyName.Name
                    && assemblyName.Version == resovleAssemblyName.Version)
                {
                    flag = true;
                    break;
                }
            }
            return flag;
        }


        /// <summary>
        /// 返回共享程序集
        /// </summary>
        /// <param name="assemblyFullName">程序集全名称</param>
        /// <returns>共享程序集</returns>
        internal static Assembly GetShareAssembly(string assemblyFullName)
        {
            Assembly assembly = null;
            var assemblyName = new AssemblyName(assemblyFullName);
            foreach (var assemblyKey in AllShareRefAssemblyDict.Keys)
            {
                var resovleAssemblyName = new AssemblyName(assemblyKey);
                if (assemblyName.Name == resovleAssemblyName.Name
                    && assemblyName.Version == resovleAssemblyName.Version)
                {
                    assembly = AllShareRefAssemblyDict[assemblyKey];
                    break;
                }
            }
            return assembly;
        }

        /// <summary>
        /// 检测程序集是否存在同名Bundle Lib程序集
        /// </summary>
        /// <param name="assemblyFullName">程序集全名称</param>
        /// <returns>是否存在</returns>
        internal static bool CheckHasBundleLib(string assemblyFullName)
        {
            var flag = false;
            var assemblyName = new AssemblyName(assemblyFullName);
            foreach (var assemblyKey in AllBundleRefAssemblyDict.Keys)
            {
                var resovleAssemblyName = new AssemblyName(assemblyKey);
                if (assemblyName.Name == resovleAssemblyName.Name
                    && assemblyName.Version == resovleAssemblyName.Version)
                {
                    flag = true;
                    break;
                }
            }
            return flag;
        }


        /// <summary>
        /// 返回Bundle Lib程序集
        /// </summary>
        /// <param name="assemblyFullName">程序集全名称</param>
        /// <returns>Bundle Lib程序集</returns>
        internal static Assembly GetBundleLibAssembly(string assemblyFullName)
        {
            Assembly assembly = null;
            var assemblyName = new AssemblyName(assemblyFullName);
            foreach (var assemblyKey in AllBundleRefAssemblyDict.Keys)
            {
                var resovleAssemblyName = new AssemblyName(assemblyKey);
                if (assemblyName.Name == resovleAssemblyName.Name
                    && assemblyName.Version == resovleAssemblyName.Version)
                {
                    assembly = AllBundleRefAssemblyDict[assemblyKey];
                    break;
                }
            }
            return assembly;
        }

        /// <summary>
        /// 返回GAC程序集
        /// </summary>
        /// <param name="assemblyFullName"></param>
        /// <returns></returns>
        internal static Assembly GetGacAssembly(string assemblyFullName)
        {
            try
            {
                var assemblyName = new AssemblyName(assemblyFullName);
                if (BundleUtils.IsAssemblyBelongsFcl(assemblyName.Name) &&
                    !assemblyName.Name.Contains(".resources") &&
                    !assemblyName.Name.Contains(".XmlSerializers"))
                {
                    log.Debug(string.Format("加载.NET FrameWork框架程集:{0}", assemblyFullName));
                    return Assembly.Load(assemblyFullName);
                }
                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// 获取程序集
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        internal static Assembly GetAssembly(string name)
        {
            var resovleAssemblyName = new AssemblyName(name);

            Assembly assembly = null;

            if (CheckHasShareLib(resovleAssemblyName.FullName))
            {
                assembly = GetShareAssembly(resovleAssemblyName.FullName);
            }
            else
            {
                assembly = GetBundleLibAssembly(resovleAssemblyName.FullName);
            }
            return assembly ?? GetGacAssembly(resovleAssemblyName.FullName);
        }

        /// <summary>
        /// 获取所有已管理的程序集
        /// </summary>
        /// <returns>管理的程序集</returns>
        public static IList<Assembly> GetManagedAssemblies()
        {
            var assemblies = AllBundleRefAssemblyDict.Select(assemblyKeyValue => assemblyKeyValue.Value).ToList();
            assemblies.AddRange(AllShareRefAssemblyDict.Select(assemblyKeyValue => assemblyKeyValue.Value));

            return assemblies;
        }
    }
}
