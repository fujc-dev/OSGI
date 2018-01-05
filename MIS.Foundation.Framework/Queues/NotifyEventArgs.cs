using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace MIS.Foundation.Framework.Queues
{
    /// <summary>
    /// 应用通知事件参数。
    /// </summary>
    [Serializable]
    public class NotifyEventArgs : EventArgs
    {
        /// <summary>
        /// 主题。
        /// </summary>
        public string Topic
        {
            get;
            internal set;
        }
        /// <summary>
        /// 消息。
        /// </summary>
        public object Message
        {
            get;
            internal set;
        }
        /// <summary>
        /// 初始化NotifyEnentArgs事件参数。
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public NotifyEventArgs()
        {

        }
        /// <summary>
        /// 初始化NotifyEnentArgs事件参数。
        /// </summary>
        /// <param name="topic">主题。</param>
        /// <param name="message">消息。</param>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public NotifyEventArgs(string topic, object message)
        {
            this.Topic = topic;
            this.Message = message;
        }
    }
}
