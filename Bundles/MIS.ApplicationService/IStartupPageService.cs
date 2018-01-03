using OSGi.NET.Core;
using System;
using System.Collections.Generic;
using System.Linq;


namespace MIS.ApplicationService
{
    /// <summary>
    /// 应用程序页面流程服务(用于控制页面启动流程)
    /// </summary>
    public interface IStartupPageService
    {
        String ClassReflection { get; }

        /// <summary>
        /// 
        /// </summary>
        IBundle Owner { get; }
    }
}