using System;
using System.Collections.Generic;
using OSGi.NET.Extension;

namespace OSGi.NET.Core.Root
{
    /// <summary>
    /// 妗嗘灦鍐呮牳鏋勯€犲伐鍘?    /// </summary>
    public interface IFrameworkFactory
    {
        /// <summary>
        /// 鍒涘缓涓€涓鏋跺唴鏍稿疄渚?        /// </summary>
        /// <returns>妗嗘灦鍐呮牳瀹炰緥</returns>
        IFramework CreateFramework();

        /// <summary>
        /// 鍒涘缓涓€涓鏋跺唴鏍稿疄渚?鎵╁睍鐐规敮鎸?        /// </summary>
        /// <returns>妗嗘灦鍐呮牳瀹炰緥</returns>
        IFramework CreateFramework(IList<ExtensionPoint> extensionPoints);

        /// <summary>
        /// 鍒涘缓涓€涓鏋跺唴鏍稿疄渚?鎵╁睍鏁版嵁鏀寔
        /// </summary>
        /// <returns>妗嗘灦鍐呮牳瀹炰緥</returns>
        IFramework CreateFramework(IList<ExtensionData> extensionDatas);
    }
}
