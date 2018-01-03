using System;
using System.Collections.Generic;
using System.Linq;

namespace OSGi.NET
{
    public interface IBase
    {

        /// <summary>
        /// 加载指定Bundle下类实例
        /// </summary>
        /// <param name="typeName">类型名称</param>
        /// <returns></returns>
        Object LoadClass(String typeName);
    }
}
