using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.IO;
using System.Xml;

using Mono.Cecil;
using log4net;

using OSGi.NET.Event;
using OSGi.NET.Extension;
using OSGi.NET.Provider;
using OSGi.NET.Utils;
using OSGi.NET.Core.Root;

namespace OSGi.NET.Core
{

    /// <summary>
    /// Bundle表示由内核创建的一个插件
    /// </summary>
    public class Bundle : IBundle, IBase
    {

        /// <summary>
        /// 日志
        /// </summary>
        private static ILog log = LogManager.GetLogger(typeof(Bundle));

        /// <summary>
        /// framwork
        /// </summary>
        private IFramework framework;

        /// <summary>
        /// bundle上下文
        /// </summary>
        private IBundleContext bundleContext;

        /// <summary>
        /// Bundle路径
        /// </summary>
        private string bundleDirectoryPath;

        /// <summary>
        /// Bundle文件名称
        /// </summary>
        private string bundleAssemblyFileName;

        /// <summary>
        /// Bundle 配置信息
        /// </summary>
        private XmlNode manifestData;

        /// <summary>
        /// bundle程序集全名
        /// </summary>
        private string bundleAssemblyFullName;

        /// <summary>
        /// bundle符号名称
        /// </summary>
        private string bundleSymbolicName = "<未加载>";

        /// <summary>
        /// bundle版本
        /// </summary>
        private Version bundleVersion;

        /// <summary>
        /// 最后一次修改时间
        /// </summary>
        private long lastModified;

        /// <summary>
        /// bundle元数据字典
        /// </summary>
        private IDictionary<string, string> metaDataDictionary;

        /// <summary>
        /// 当前bundle引用程序集
        /// </summary>
        private IDictionary<string, Assembly> bundleRefAssemblyDict;

        /// <summary>
        /// 当前bundle引用程序集定义
        /// </summary>
        private IDictionary<string, AssemblyDefinition> bundleRefDefinitionDict;

        /// <summary>
        /// 扩展点
        /// </summary>
        private IList<ExtensionPoint> extensionPoints;

        /// <summary>
        /// 扩展数据
        /// </summary>
        private IList<ExtensionData> extensionDatas;

        /// <summary>
        /// bundle程序集
        /// </summary>
        private Assembly bundleAssembly;

        /// <summary>
        /// bundle启动器类型
        /// </summary>
        private Type activatorClass;

        /// <summary>
        /// bundle启动器实例
        /// </summary>
        private IBundleActivator bundleActivator;

        /// <summary>
        /// bundle依赖bundles集合
        /// </summary>
        private IList<Bundle> requiredBundleList;

        /// <summary>
        /// 状态
        /// </summary>
        private int state = BundleStateConst.INSTALLED;


        /// <summary>
        /// Bundle构造
        /// </summary>
        /// <param name="framework">框架实例</param>
        /// <param name="bundleDirectoryPath">Bundle路径</param>
        /// <param name="bundleConfigData">Bundle配置节点</param>
        public Bundle(IFramework framework, string bundleDirectoryPath, XmlNode bundleConfigData)
        {
            this.framework = framework;
            this.bundleDirectoryPath = bundleDirectoryPath;
            manifestData = bundleConfigData;

            bundleRefDefinitionDict = new Dictionary<string, AssemblyDefinition>();
            bundleRefAssemblyDict = new Dictionary<string, Assembly>();
            extensionDatas = new List<ExtensionData>();
            extensionPoints = new List<ExtensionPoint>();
            bundleContext = new BundleContext(framework, this);

            Init();
        }

        /// <summary>
        /// 初始化Bundle，读取相关信息
        /// </summary>
        private void Init()
        {
            RemoveAllRefAssembly();

            var assemblyName = GetBundleAssemblyFileName();
            bundleAssemblyFileName = Path.Combine(bundleDirectoryPath, string.Format("{0}.dll", assemblyName));
            if (false == File.Exists(bundleAssemblyFileName))
            {
                bundleAssemblyFileName = Path.Combine(bundleDirectoryPath, string.Format("{0}.exe", assemblyName));
            }
            lastModified = File.GetLastWriteTime(bundleAssemblyFileName).Ticks;

            LoadMetaData(assemblyName);

            var frameworkFireEvent = (IFrameworkFireEvent)framework;
            frameworkFireEvent.FireBundleEvent(new BundleEventArgs(BundleEventArgs.INSTALLED, this));
        }


        /// <summary>
        /// 读取元数据信息
        /// </summary>
        private void LoadMetaData(string assemblyName)
        {
            log.Debug(string.Format("模块读取元数据信息！[{0}]", assemblyName));

            metaDataDictionary = new Dictionary<string, string>();

            var domaininfo = new AppDomainSetup();
            domaininfo.ApplicationBase = System.Environment.CurrentDirectory;
            var adevidence = AppDomain.CurrentDomain.Evidence;
            var tmpAppDomain = AppDomain.CreateDomain(string.Format("Bundle[{0}] AppDomain", assemblyName), adevidence, domaininfo);
            var assemblyResolver = tmpAppDomain.CreateInstanceAndUnwrap(typeof(Bundle).Assembly.FullName, typeof(AssemblyResolver).FullName) as AssemblyResolver;
            assemblyResolver.Init((File.ReadAllBytes(bundleAssemblyFileName)), GetBundleLibsDirectoryName(), BundleConfigProvider.GetShareLibsDirectory());

            bundleAssemblyFullName = assemblyResolver.GetAssemblyFullName();
            bundleSymbolicName = assemblyResolver.GetAssemblyName();
            metaDataDictionary.Add(BundleConst.BUNDLE_MANIFEST_SYMBOLIC_NAME, bundleSymbolicName);
            bundleVersion = assemblyResolver.GetVersion();
            metaDataDictionary.Add(BundleConst.BUNDLE_MANIFEST_VERSION, bundleVersion.ToString());
            metaDataDictionary.Add(BundleConst.BUNDLE_MANIFEST_NAME, assemblyResolver.GetAssemblyTitle());
            metaDataDictionary.Add(BundleConst.BUNDLE_MANIFEST_VENDOR, assemblyResolver.GetVendor());
            metaDataDictionary.Add(BundleConst.BUNDLE_MANIFEST_REQUIRE_BUNDLE, assemblyResolver.GetAssemblyRequiredAssembly());
            assemblyResolver = null;

            AppDomain.Unload(tmpAppDomain);
        }


        /// <summary>
        /// 清除当前Bundle引用程序集
        /// </summary>
        private void RemoveAllRefAssembly()
        {
            foreach (string assemblyName in bundleRefAssemblyDict.Keys)
            {
                BundleAssemblyProvider.RemoveAssembly(assemblyName);
            }
            bundleRefAssemblyDict.Clear();
            bundleRefDefinitionDict.Clear();
        }

        /// <summary>
        /// 获取当前Bundle目录
        /// </summary>
        /// <returns></returns>
        private string GetBundleFolderName()
        {
            return new DirectoryInfo(bundleDirectoryPath).Name;
        }

        /// <summary>
        /// 启动
        /// </summary>
        public void Start()
        {
            log.Debug(string.Format("模块{0}启动开始！", GetBundleAssemblyFileName()));

            if (state == BundleStateConst.INSTALLED)
            {
                try
                {
                    Resolve();
                }
                catch (Exception ex)
                {
                    state = BundleStateConst.INSTALLED;
                    log.Debug(string.Format("模块{0}启动失败！", GetBundleAssemblyFileName()));
                    throw ex;
                }
            }
            if (state == BundleStateConst.RESOLVED)
            {
                state = BundleStateConst.STARTING;

                try
                {
                    var frameworkFireEvent = (IFrameworkFireEvent)framework;
                    frameworkFireEvent.FireBundleEvent(new BundleEventArgs(BundleEventArgs.STARTING, this));

                    if (activatorClass != null)
                    {
                        log.Debug("激活器启动！");
                        bundleActivator = Activator.CreateInstance(activatorClass) as IBundleActivator;
                        bundleActivator.Start(bundleContext);

                        log.Debug("加载扩展点数据！");
                        LoadExtensions();
                    }
                    state = BundleStateConst.ACTIVE;

                    frameworkFireEvent.FireBundleEvent(new BundleEventArgs(BundleEventArgs.STARTED, this));
                }
                catch (Exception ex)
                {
                    state = BundleStateConst.RESOLVED;
                    log.Debug(string.Format("模块{0}启动失败！", GetBundleAssemblyFileName()));
                    throw ex.InnerException;
                }
            }

            log.Debug(string.Format("模块{0}启动结束！", GetBundleAssemblyFileName()));
        }

        /// <summary>
        /// 初始化扩展信息
        /// </summary>
        private void LoadExtensions()
        {
            log.Debug("扩展点加载开始！");

            try
            {
                extensionPoints.Clear();
                extensionDatas.Clear();

                var extensionPointNames = BundleConfigProvider.GetBundleConfigExtensionPoints(manifestData);

                extensionPoints = extensionPointNames.Select(name => new ExtensionPoint()
                {
                    Name = name,
                    Owner = this
                }).ToList();

                var extensionDataDic = BundleConfigProvider.GetBundleConfigExtensionDatas(manifestData);

                extensionDatas = extensionDataDic.Select(item => new ExtensionData()
                {
                    Name = item.Key,
                    Owner = this,
                    ExtensionList = item.Value
                }).ToList();

                foreach (IBundle bundle in framework.GetBundles())
                {
                    var bundlePoints = bundle.GetExtensionPoints();
                    foreach (ExtensionPoint extensionPoint in bundlePoints)
                    {
                        var extensionData = extensionDatas.FirstOrDefault(item => item.Name == extensionPoint.Name);
                        if (extensionData != null)
                        {
                            log.Debug(string.Format("扩展点{0}加载！", extensionPoint.Name));

                            var eventArgs = new ExtensionEventArgs(ExtensionEventArgs.LOAD, extensionPoint, extensionData);

                            var fireContext = (IContextFireEvent)bundle.GetBundleContext();
                            fireContext.FireExtensionChanged(this, eventArgs);


                            var frameworkFireEvent = (IFrameworkFireEvent)framework;
                            frameworkFireEvent.FireExtensionEvent(eventArgs);
                        }
                    }
                }

                foreach (ExtensionPoint extensionPoint in extensionPoints)
                {
                    foreach (IBundle bundle in framework.GetBundles())
                    {
                        if (bundle.GetSymbolicName().Equals("MIS.ApplicationService"))
                        {
                            continue;
                        }
                        var bundleExtensionDatas = bundle.GetExtensionDatas();
                        var extensionData = bundleExtensionDatas.FirstOrDefault(item => item.Name == extensionPoint.Name);
                        if (extensionData != null)
                        {
                            log.Debug(string.Format("扩展点{0}加载！", extensionPoint.Name));

                            var eventArgs = new ExtensionEventArgs(ExtensionEventArgs.LOAD, extensionPoint, extensionData);

                            var fireContext = (IContextFireEvent)GetBundleContext();
                            fireContext.FireExtensionChanged(bundle, eventArgs);

                            var frameworkFireEvent = (IFrameworkFireEvent)framework;
                            frameworkFireEvent.FireExtensionEvent(eventArgs);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("加载Bundle扩展点配置信息出错！", ex);
                throw ex;
            }

            log.Debug("扩展点加载结束！");
        }

        /// <summary>
        /// 装载Bundle
        /// </summary>
        public void Resolve()
        {
            log.Debug(string.Format("模块{0}加载开始！", GetBundleAssemblyFileName()));

            LoadRequiredBundle();

            LoadAssemblys();

            LoadActivator();

            state = BundleStateConst.RESOLVED;

            var frameworkFireEvent = (IFrameworkFireEvent)framework;
            frameworkFireEvent.FireBundleEvent(new BundleEventArgs(BundleEventArgs.RESOLVED, this));

            log.Debug(string.Format("模块{0}加载结束！", GetBundleAssemblyFileName()));
        }

        /// <summary>
        /// 1.加载依赖Bundle
        /// </summary>
        private void LoadRequiredBundle()
        {
            log.Debug("加载相关依赖模块！");

            requiredBundleList = new List<Bundle>();
            var requireBundles = GetRequireBundleList();
            foreach (string tmpStr in requireBundles)
            {
                var requireBundleString = tmpStr.Trim();
                if (string.IsNullOrEmpty(requireBundleString))
                {
                    continue;
                }
                var requireBundleName = (string )null;
                var requireBundleVersionString = (string )null;
                ParseRequireBundleVersion(requireBundleString, out requireBundleName, out requireBundleVersionString);

                Bundle matchBundle = null;
                var matchBundleList = new List<Bundle>();
                foreach (IBundle bundle in GetBundleContext().GetBundles())
                {
                    var tmpBundle = bundle as Bundle;

                    if (Equals(bundle) || tmpBundle == null)
                    {
                        continue;
                    }
                    if (requireBundleName.Equals(tmpBundle.GetSymbolicName()))
                    {
                        if (string.IsNullOrEmpty(requireBundleVersionString))
                        {
                            matchBundle = tmpBundle;
                            break;
                        }
                        else
                        {
                            matchBundleList.Add(tmpBundle);
                        }
                    }
                }
                if (matchBundle == null && !string.IsNullOrEmpty(requireBundleVersionString))
                {
                    var requireBundleVersion = new Version(requireBundleVersionString);

                    matchBundleList.Sort(new Comparison<Bundle>(delegate(Bundle x, Bundle y)
                    {
                        return x.GetVersion().CompareTo(y.GetVersion());
                    }));
                    foreach (Bundle tmpBundle in matchBundleList)
                    {
                        if (tmpBundle.GetVersion().CompareTo(requireBundleVersion) >= 0)
                        {
                            matchBundle = tmpBundle;
                            break;
                        }
                    }
                }
                if (matchBundle == null)
                {
                    continue;
                }
                if (matchBundle.GetState() != BundleStateConst.RESOLVED
                    && matchBundle.GetState() != BundleStateConst.ACTIVE)
                {
                    matchBundle.Resolve();
                }
                requiredBundleList.Add(matchBundle);
            }
        }




        /// <summary>
        /// 2.加载所需的所有程序集
        /// </summary>
        private void LoadAssemblys()
        {
            log.Debug("模块加载自身程序集！");

            var bundleAssemblyDefinition = AssemblyDefinition.ReadAssembly(bundleAssemblyFileName);
            var bundleAssemblyNameDefinition = bundleAssemblyDefinition.Name;

            BundleUtils.RemoveAssemblyStrongName(bundleAssemblyNameDefinition);

            foreach (ModuleDefinition moduleDefinition in bundleAssemblyDefinition.Modules)
            {
                foreach (AssemblyNameReference assemblyNameReference in moduleDefinition.AssemblyReferences)
                {
                    var assemblyName = assemblyNameReference.Name;

                    if (BundleUtils.IsAssemblyBelongsFcl(assemblyName))
                    {
                        continue;
                    }

                    var bundle = GetBundleFromRequiredBundles(assemblyName);
                    if (bundle != null)
                    {
                        var requiredBundleAssemblyName = bundle.bundleAssembly.GetName();

                        assemblyNameReference.Name = requiredBundleAssemblyName.Name;
                        assemblyNameReference.Version = requiredBundleAssemblyName.Version;
                        BundleUtils.RemoveAssemblyStrongName(assemblyNameReference);

                        log.Debug(string.Format("模块关联引用模块[{0}]！", assemblyName));

                        continue;
                    }

                    if (BundleAssemblyProvider.CheckHasShareLib(assemblyNameReference.FullName))
                    {
                        var shareAssembly = BundleAssemblyProvider.GetShareAssembly(assemblyNameReference.FullName);

                        assemblyNameReference.Name = shareAssembly.GetName().Name;
                        assemblyNameReference.Version = shareAssembly.GetName().Version;

                        log.Debug(string.Format("模块关联共享库程序集[{0}]！", assemblyName));

                        continue;
                    }

                    var bundleRefAssembly = GetBundleRefAssembly(assemblyNameReference.FullName);
                    if (bundleRefAssembly != null)
                    {
                        assemblyNameReference.Name = bundleRefAssembly.GetName().Name;
                        assemblyNameReference.Version = bundleRefAssembly.GetName().Version;
                        BundleUtils.RemoveAssemblyStrongName(assemblyNameReference);

                        log.Debug(string.Format("模块关联自身库程序集[{0}]！", assemblyName));

                        continue;
                    }

                    var newAssemblyDefinition = LoadAssemblyFromLibDir(assemblyName);
                    if (newAssemblyDefinition != null)
                    {
                        assemblyNameReference.Name = newAssemblyDefinition.Name.Name;
                        assemblyNameReference.Version = newAssemblyDefinition.Name.Version;
                        BundleUtils.RemoveAssemblyStrongName(assemblyNameReference);

                        log.Debug(string.Format("模块关联自身库程序集[{0}]！", assemblyName));

                        continue;
                    }

                    throw new IOException(string.Format("{0}不能解析依赖的程序集[{1}]", ToString(), assemblyName));
                }
                moduleDefinition.Attributes &= ~ModuleAttributes.StrongNameSigned;
            }
            var ms = new MemoryStream();
            bundleAssemblyDefinition.Write(ms);

            if (BundleConfigProvider.OSGi_NET_IS_DEBUG_MODE)
            {
                bundleAssembly = Assembly.LoadFrom(bundleAssemblyFileName);
            }
            else
            {
                bundleAssembly = Assembly.Load(ms.ToArray());
            }
            bundleAssembly.GetTypes();
            AddRefAssembly(bundleAssemblyNameDefinition.FullName, bundleAssembly, bundleAssemblyDefinition);
        }

        /// <summary>
        /// 3.读取Activator
        /// </summary>
        private void LoadActivator()
        {
            log.Debug("模块搜索激活器！");

            activatorClass = null;
            foreach (Type type in bundleAssembly.GetTypes())
            {
                if (typeof(IBundleActivator).IsAssignableFrom(type))
                {
                    activatorClass = type;
                    metaDataDictionary.Add(BundleConst.BUNDLE_MANIFEST_ACTIVATOR, activatorClass.FullName);
                    break;
                }
            }
        }


        /// <summary>
        /// 从LIB目录加载程序集并返回新的程序集名称
        /// </summary>
        /// <param name="assemblyName">程序集名称</param>
        /// <returns>程序集定义</returns>
        private AssemblyDefinition LoadAssemblyFromLibDir(string assemblyName)
        {
            if (BundleUtils.IsAssemblyBelongsFcl(assemblyName))
            {
                return null;
            }
            var libsDirPath = GetBundleLibsDirectoryName();
            if (!Directory.Exists(libsDirPath))
            {
                Directory.CreateDirectory(libsDirPath);
            }
            var files = Directory.GetFiles(libsDirPath, assemblyName + ".dll", SearchOption.AllDirectories);

            if (files.Length == 0)
            {
                return null;
            }
            var assemblyFileName = files[0];

            var readerParameter = new ReaderParameters()
            {
                AssemblyResolver = new CustomAssemblyResolver(
            resolverAssemblyFullName =>
            {
                var resolverAssemblyName = new AssemblyName(resolverAssemblyFullName);
                var resolverDefinition = GetBundleRefDefinition(resolverAssemblyFullName);
                if (resolverDefinition == null)
                {
                    return LoadAssemblyFromLibDir(resolverAssemblyName.Name);
                }
                return resolverDefinition;
            })
            };

            var assemblyDefinition = AssemblyDefinition.ReadAssembly(assemblyFileName, readerParameter);
            var assemblyNameDefinition = assemblyDefinition.Name;

            BundleUtils.RemoveAssemblyStrongName(assemblyNameDefinition);

            MarkAssembleyVersion(assemblyNameDefinition);

            foreach (ModuleDefinition moduleDefinition in assemblyDefinition.Modules)
            {
                foreach (AssemblyNameReference refAssemblyNameReference in moduleDefinition.AssemblyReferences)
                {
                    var refAssemblyName = refAssemblyNameReference.Name;
                    if (BundleAssemblyProvider.CheckHasShareLib(refAssemblyNameReference.FullName))
                    {
                        var shareAssembly = BundleAssemblyProvider.GetShareAssembly(refAssemblyNameReference.FullName);
                        refAssemblyNameReference.Name = shareAssembly.GetName().Name;
                        refAssemblyNameReference.Version = shareAssembly.GetName().Version;

                        log.Debug(string.Format("模块关联共享库程序集[{0}]！", assemblyName));

                        continue;
                    }

                    var bundleRefAssembly = GetBundleRefAssembly(refAssemblyNameReference.FullName);
                    if (bundleRefAssembly != null)
                    {
                        refAssemblyNameReference.Name = bundleRefAssembly.GetName().Name;
                        refAssemblyNameReference.Version = bundleRefAssembly.GetName().Version;
                        BundleUtils.RemoveAssemblyStrongName(refAssemblyNameReference);

                        log.Debug(string.Format("模块关联自身库程序集[{0}]！", assemblyName));

                        continue;
                    }

                    var newRefAssemblyDefinition = LoadAssemblyFromLibDir(refAssemblyName);
                    if (newRefAssemblyDefinition != null)
                    {
                        refAssemblyNameReference.Name = newRefAssemblyDefinition.Name.Name;
                        refAssemblyNameReference.Version = newRefAssemblyDefinition.Name.Version;
                        BundleUtils.RemoveAssemblyStrongName(refAssemblyNameReference);

                        log.Debug(string.Format("模块关联自身库程序集[{0}]！", assemblyName));

                        continue;
                    }
                }
                moduleDefinition.Attributes &= ~ModuleAttributes.StrongNameSigned;
            }
            var ms = new MemoryStream();
            assemblyDefinition.Write(ms);

            Assembly assembly;
            if (BundleConfigProvider.OSGi_NET_IS_DEBUG_MODE)
            {
                assembly = Assembly.LoadFrom(assemblyFileName);
            }
            else
            {
                assembly = Assembly.Load(ms.ToArray());
            }

            if (BundleConfigProvider.OSGi_NET_ALLTYPES_LOAD)
            {
                assembly.GetTypes();
            }
            log.Debug(string.Format("模块加载依赖程序集[{0}]！", assemblyName));

            AddRefAssembly(assemblyNameDefinition.FullName, assembly, assemblyDefinition);

            return assemblyDefinition;
        }



        /// <summary>
        /// 重名程序集生成唯一版本号
        /// </summary>
        /// <param name="assemblyNameReference">程序集Name引用</param>
        /// <returns>新版本</returns>
        private Version MarkAssembleyVersion(AssemblyNameReference assemblyNameReference)
        {
            if (BundleAssemblyProvider.CheckHasBundleLib(assemblyNameReference.FullName))
            {
                var version = assemblyNameReference.Version;
                var fixVersion = new Random().Next(0, 100);
                var newVersion = new Version(version.Major, version.Minor, version.Build, fixVersion);
                assemblyNameReference.Version = newVersion;

                if (BundleAssemblyProvider.CheckHasBundleLib(assemblyNameReference.FullName))
                {
                    MarkAssembleyVersion(assemblyNameReference);
                }
            }
            return assemblyNameReference.Version;
        }


        /// <summary>
        /// 获取依赖Bundle信息
        /// </summary>
        /// <returns></returns>
        private List<string> GetRequireBundleList()
        {
            var requireBundleStr = metaDataDictionary[BundleConst.BUNDLE_MANIFEST_REQUIRE_BUNDLE];
            return requireBundleStr.Split(new Char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList();
        }

        /// <summary>
        /// 获取依赖Bundle及版本
        /// </summary>
        /// <param name="requireBundleString"></param>
        /// <param name="requireBundleName"></param>
        /// <param name="requireBundleVersionString"></param>
        private void ParseRequireBundleVersion(string requireBundleString, out string requireBundleName, out string requireBundleVersionString)
        {
            var requireBundleStringArray = requireBundleString.Split(new Char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            requireBundleName = requireBundleStringArray[0].Trim();
            requireBundleVersionString = string.Empty;
            IDictionary<string, string> otherDict = new Dictionary<string, string>();
            for (var i = 1; i < requireBundleStringArray.Length; i++)
            {
                var requireBundleStringPart = requireBundleStringArray[i];
                var requireBundleStringPartStringArray = requireBundleStringPart.Split(new Char[] { '=' }, StringSplitOptions.RemoveEmptyEntries);
                otherDict.Add(requireBundleStringPartStringArray[0].Trim(), requireBundleStringPartStringArray[1].Trim());
            }
            if (otherDict.ContainsKey(BundleConst.BUNDLE_MANIFEST_REQUIRED_BUNDLE_VERSION))
            {
                requireBundleVersionString = otherDict[BundleConst.BUNDLE_MANIFEST_REQUIRED_BUNDLE_VERSION];
                requireBundleVersionString = requireBundleVersionString.Replace("\"", string.Empty);
            }
        }


        /// <summary>
        /// 根据符号名称从已装载的Bundle列表获取依赖Bundle
        /// </summary>
        /// <param name="bundleName"></param>
        /// <returns></returns>
        private Bundle GetBundleFromRequiredBundles(string bundleName)
        {
            foreach (Bundle bundle in requiredBundleList)
            {
                if (bundle.GetSymbolicName().Equals(bundleName))
                {
                    return bundle;
                }
            }
            return null;
        }

        /// <summary>
        /// 得到Bundle依赖库的目录名称
        /// </summary>
        /// <returns></returns>
        private string GetBundleLibsDirectoryName()
        {
            return Path.Combine(bundleDirectoryPath, BundleConst.BUNDLE_LIBS_DIRECTORY_NAME);
        }


        /// <summary>
        /// 添加引用程序集
        /// </summary>
        /// <param name="assemblyFullName"></param>
        /// <param name="assemblyDefinition"></param>
        /// <param name="assembly"></param>
        private void AddRefAssembly(string assemblyFullName, Assembly assembly, AssemblyDefinition assemblyDefinition)
        {
            if (!bundleRefAssemblyDict.ContainsKey(assemblyFullName))
            {
                bundleRefAssemblyDict.Add(assemblyFullName, assembly);
                bundleRefDefinitionDict.Add(assemblyFullName, assemblyDefinition);
            }
            BundleAssemblyProvider.AddAssembly(assemblyFullName, assembly);
        }

        /// <summary>
        /// 获取当前Bundle已经引用的程序集
        /// </summary>
        /// <param name="assemblyFullName"></param>
        private Assembly GetBundleRefAssembly(string assemblyFullName)
        {
            var refAssemblyName = new AssemblyName(assemblyFullName);
            var bundleRefAssembly = bundleRefAssemblyDict.FirstOrDefault(
            keyValuePair =>
            {
                var bundleRefAssemblyName = new AssemblyName(keyValuePair.Key);
                if (bundleRefAssemblyName.Name == refAssemblyName.Name
                                  && bundleRefAssemblyName.Version == refAssemblyName.Version)
                {
                    return true;
                }
                return false;
            });
            return bundleRefAssembly.Value;
        }

        /// <summary>
        /// 获取当前Bundle已经引用的程序集定义
        /// </summary>
        /// <param name="assemblyFullName"></param>
        private AssemblyDefinition GetBundleRefDefinition(string assemblyFullName)
        {
            var refAssemblyName = new AssemblyName(assemblyFullName);
            var bundleRefDefinition = bundleRefDefinitionDict.FirstOrDefault(
            keyValuePair =>
            {
                var bundleRefAssemblyName = new AssemblyName(keyValuePair.Key);
                if (bundleRefAssemblyName.Name == refAssemblyName.Name
                                  && bundleRefAssemblyName.Version == refAssemblyName.Version)
                {
                    return true;
                }
                return false;
            });
            return bundleRefDefinition.Value;
        }


        /// <summary>
        /// 停止
        /// </summary>
        public void Stop()
        {
            log.Debug(string.Format("模块{0}停止开始！", GetBundleAssemblyFileName()));

            if (state == BundleStateConst.ACTIVE)
            {
                state = BundleStateConst.STOPPING;
                if (bundleActivator != null)
                {
                    var frameworkFireEvent = (IFrameworkFireEvent)framework;
                    frameworkFireEvent.FireBundleEvent(new BundleEventArgs(BundleEventArgs.STOPPING, this));

                    log.Debug("卸载扩展点数据！");
                    UnLoadExtensions();

                    log.Debug("激活器停止");
                    bundleActivator.Stop(bundleContext);

                    ((BundleContext)bundleContext).Stop();

                    frameworkFireEvent.FireBundleEvent(new BundleEventArgs(BundleEventArgs.STOPPED, this));

                    bundleActivator = null;
                }
                state = BundleStateConst.RESOLVED;
            }

            log.Debug(string.Format("模块{0}停止结束！", GetBundleAssemblyFileName()));
        }

        /// <summary>
        /// 卸载扩展点数据
        /// </summary>
        private void UnLoadExtensions()
        {
            log.Debug("扩展点卸载开始");

            foreach (IBundle bundle in framework.GetBundles())
            {
                var bundlePoints = bundle.GetExtensionPoints();
                foreach (ExtensionPoint extensionPoint in bundlePoints)
                {
                    var extensionData = extensionDatas.FirstOrDefault(item => item.Name == extensionPoint.Name);
                    if (extensionData != null)
                    {
                        log.Debug(string.Format("扩展点{0}卸载！", extensionPoint.Name));

                        var fireContext = (IContextFireEvent)extensionPoint.Owner.GetBundleContext();
                        fireContext.FireExtensionChanged(this, new ExtensionEventArgs(ExtensionEventArgs.UNLOAD, extensionPoint, extensionData));
                    }
                }
            }

            extensionDatas.Clear();
            extensionPoints.Clear();

            log.Debug("扩展点卸载停止");
        }


        /// <summary>
        /// 卸载组件
        /// </summary>
        public void UnInstall()
        {
            log.Debug("模块卸载开始！");

            ((IFrameworkInstaller)framework).UnInstall(this);
            RemoveAllRefAssembly();
            BundleConfigProvider.RemoveBundleConfig(GetBundleFolderName());

            log.Debug("模块卸载完成！");
        }

        /// <summary>
        /// 指定路径更新当前Bundle
        /// </summary>
        /// <param name="zipFile">更新的Bundle路径</param>
        public void Update(string zipFile)
        {
            log.Debug("模块更新开始！");

            var preState = state;
            if (File.Exists(zipFile))
            {
                var tmpBundleDirectoryPath = string.Format("{0}_{1}", GetBundleFolderName(), Guid.NewGuid().ToString());
                BundleUtils.ExtractBundleFile(tmpBundleDirectoryPath, zipFile);
                var xmlNode = BundleConfigProvider.ReadBundleConfig(tmpBundleDirectoryPath);
                try
                {
                    var tmpassemblyName = BundleConfigProvider.GetBundleConfigAssemblyName(xmlNode);
                    var assemblyName = GetBundleAssemblyFileName();
                    if (!assemblyName.Equals(tmpassemblyName))
                    {
                        throw new Exception(string.Format("要更新的插件[{0}]与输入流中的插件[{1}]不匹配！", assemblyName, tmpassemblyName));
                    }
                }
                catch (Exception ex)
                {
                    Directory.Delete(tmpBundleDirectoryPath, true);
                    throw ex;
                }
                ((IFrameworkInstaller)framework).Delete(this);
                Directory.Move(tmpBundleDirectoryPath, bundleDirectoryPath);

                manifestData = xmlNode;

                BundleConfigProvider.SyncBundleConfig(GetBundleFolderName(), xmlNode);
            }
            if (preState == BundleStateConst.INSTALLED)
            {
                Init();
            }
            else
            {
                if (preState == BundleStateConst.RESOLVED)
                {
                    Init();
                    Resolve();
                }
                else
                {
                    if (preState == BundleStateConst.ACTIVE)
                    {
                        Stop();
                        Init();
                        Resolve();
                        Start();
                    }
                }
            }
            log.Debug("模块更新完成！");
        }

        /// <summary>
        /// 获取当前Bundle上下文对象
        /// </summary>
        /// <returns>Bundle上下文对象</returns>
        public IBundleContext GetBundleContext()
        {
            return bundleContext;
        }


        /// <summary>
        /// 获取当前Bundle状态
        /// </summary>
        /// <returns>Bundle状态</returns>
        public int GetState()
        {
            return state;
        }

        /// <summary>
        /// 获取当前Bundle版本信息
        /// </summary>
        /// <returns>Bundle版本信息</returns>
        public Version GetVersion()
        {
            return bundleVersion;
        }


        /// <summary>
        /// 获取当前Bundle符号名称
        /// </summary>
        /// <returns>Bundle符号名称</returns>
        public string GetSymbolicName()
        {
            return bundleSymbolicName;
        }

        /// <summary>
        /// 获取当前Bundle程序集全名
        /// </summary>
        /// <returns>Bundle程序集全名</returns>
        public string GetBundleAssemblyFullName()
        {
            return bundleAssemblyFullName;
        }

        /// <summary>
        /// 获取当前Bundle程序集清单数据
        /// </summary>
        /// <returns>Bundle程序清单数据</returns>
        public IDictionary<string, string> GetManifest()
        {
            return new Dictionary<string, string>(metaDataDictionary);
        }

        /// <summary>
        /// 获取当前Bundle目录
        /// </summary>
        /// <returns>Bundle目录</returns>
        public string GetBundleDirectoryPath()
        {
            return bundleDirectoryPath;
        }

        /// <summary>
        /// 获取当前Bundle程序集文件名称
        /// </summary>
        /// <returns>Bundle程序集文件名称</returns>
        public string GetBundleAssemblyFileName()
        {
            return BundleConfigProvider.GetBundleConfigAssemblyName(manifestData);
        }

        /// <summary>
        /// 获取当前Bundle启动级别
        /// </summary>
        /// <returns>Bundle启动级别</returns>
        public int GetBundleStartLevel()
        {
            return BundleConfigProvider.GetBundleConfigStartLevel(manifestData);
        }

        /// <summary>
        /// 获取模块清单数据
        /// </summary>
        /// <returns>清单数据节点</returns>
        public XmlNode GetBundleManifestData()
        {
            return manifestData.Clone();
        }

        /// <summary>
        /// 获取当前Bundle扩展点
        /// </summary>
        /// <returns>扩展点列表</returns>
        public IList<ExtensionPoint> GetExtensionPoints()
        {
            var tmpExtensionPoints = new ExtensionPoint[extensionPoints.Count];
            extensionPoints.CopyTo(tmpExtensionPoints, 0);
            return tmpExtensionPoints.ToList();
        }

        /// <summary>
        /// 获取当前Bundle扩展的扩展数据
        /// </summary>
        /// <returns>扩展数据列表</returns>
        public IList<ExtensionData> GetExtensionDatas()
        {
            var tmpExtensionDatas = new ExtensionData[extensionDatas.Count];
            extensionDatas.CopyTo(tmpExtensionDatas, 0);
            return tmpExtensionDatas.ToList();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(System.Object obj)
        {
            var bundle = obj as Bundle;

            if (bundle == null)
            {
                return false;
            }

            return bundleAssemblyFullName == bundle.bundleAssemblyFullName
                && bundleVersion == bundle.bundleVersion;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return string.Format("{0}^{1}",
                bundleAssemblyFullName, bundleVersion.ToString()).GetHashCode();
        }

        public override string ToString()
        {
            return string.Format("Bundle[AssemblyName:{0},SymbolicName:{1},Version:{2}]", bundleAssemblyFullName, bundleSymbolicName, bundleVersion);
        }


        public Object LoadClass(string typeName)
        {
            var _assembly = Assembly.LoadFile(bundleAssemblyFileName);
            return _assembly.CreateInstance(typeName);
        }
    }
}
