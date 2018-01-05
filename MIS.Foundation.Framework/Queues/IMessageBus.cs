using System;
using System.Collections.Generic;
using System.Linq;

namespace MIS.Foundation.Framework.Queues
{
    /// <summary>
    /// 消息总线接口定义。
    /// </summary>
    public interface IMessageBus : IDisposable
    {
        /// <summary>
        /// 发布一条消息到总线。
        /// </summary>
        /// <param name="topic">主题。</param>
        /// <param name="message">发布的消息。</param>
        void Publish(string topic, object message);

        /// <summary>
        /// 发布一条消息到总线。
        /// </summary>
        /// <param name="topic">主题。</param>
        /// <param name="message">发布的消息。</param>
        /// <param name="async">是否异步发布。</param>
        void Publish(string topic, object message, bool async);

        /// <summary>
        /// 订阅消息。
        /// </summary>
        /// <param name="subscriber">订阅者。</param>
        /// <param name="topic">主题。</param>
        /// <param name="notifyHandler">订阅通知。</param>
        void Subscribe(object subscriber, string topic, MessageNotifyHandler notifyHandler);

        /// <summary>
        /// 退订消息。
        /// </summary>
        /// <param name="subscriber">订阅者。</param>
        void Unsubscribe(object subscriber);

        /// <summary>
        /// 退订消息。
        /// </summary>
        /// <param name="subscriber">订阅者。</param>
        /// <param name="topic">主题。</param>
        void Unsubscribe(object subscriber, string topic);

        /// <summary>
        /// 根据主题求出在经订阅者名称清单(friendName)。
        /// </summary>
        /// <param name="topic">主题。</param>
        List<string> GetSubscribers(string topic);
    }
}