using System;
using System.Collections.Generic;
using System.Linq;

namespace MIS.Foundation.Framework.Queues
{
    /// <summary>
    /// 消息通知委托。
    /// </summary>
    /// <param name="Message">消息对象。</param>
    public delegate void MessageNotifyHandler(object Message);
}