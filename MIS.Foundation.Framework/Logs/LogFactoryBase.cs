using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MIS.Foundation.Framework
{
    /// <summary>
    /// LogFactory 基类
    /// </summary>
    public abstract class LogFactoryBase : ILogFactory
    {
        protected string ConfigFile { get; private set; }

        protected bool IsSharedConfig { get; private set; }


        protected LogFactoryBase(string configFile)
        {
            if (Path.IsPathRooted(configFile))
            {
                ConfigFile = configFile;
                return;
            }
            var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, configFile);
            if (File.Exists(filePath))
            {
                ConfigFile = filePath;
                return;
            }
            var rootDir = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).FullName;
            filePath = Path.Combine(rootDir, AppDomain.CurrentDomain.FriendlyName + "." + configFile);
            if (File.Exists(filePath))
            {
                ConfigFile = filePath;
                return;
            }
            filePath = Path.Combine(rootDir, configFile);
            if (File.Exists(filePath))
            {
                ConfigFile = filePath;
                IsSharedConfig = true;
                return;
            }
            rootDir = Path.Combine(rootDir, "Config");
            filePath = Path.Combine(rootDir, AppDomain.CurrentDomain.FriendlyName + "." + configFile);
            if (File.Exists(filePath))
            {
                ConfigFile = filePath;
                return;
            }
            filePath = Path.Combine(rootDir, configFile);
            if (File.Exists(filePath))
            {
                ConfigFile = filePath;
                IsSharedConfig = true;
                return;
            }
            ConfigFile = configFile;
        }

        public abstract ILog GetLog(string name);
    }
}
