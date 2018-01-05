using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MIS.Foundation.Framework.Queues
{
    /// <summary>
    /// 消息交换中心。
    /// </summary>
    public class MessageExchange : IMessageBus, IDisposable
    {
        private static MessageExchange _instanceMessageExchange;
        private static object lockStr = String.Empty;
        private List<MessageExchange.Subscriber> _subscriberList; //存储订阅者相关所有的主题以及消息通知

        private MessageExchange()
        {
            lockStr = "MIS.Foundation.Framework.MessageExchange";
            this._subscriberList = new List<MessageExchange.Subscriber>();
        }
        /// <summary>
        /// MessageExchange唯一实例。
        /// </summary>
        public static MessageExchange Singleton
        {
            get
            {
                if (MessageExchange._instanceMessageExchange == null)
                {
                    lock (MessageExchange.lockStr)
                    {
                        if (MessageExchange._instanceMessageExchange == null) MessageExchange._instanceMessageExchange = new MessageExchange();
                    }
                }
                return MessageExchange._instanceMessageExchange;
            }
        }

        #region IDisposable Interface Imp
        public void Dispose()
        {

        }
        #endregion

        #region IMessageBus Interface Imp

        public void Publish(string topic, object message)
        {
            this.Publish(topic, message, false);
        }

        public void Publish(string topic, object message, bool async)
        {
            if (string.IsNullOrEmpty(topic))
            {
                return;
            }
            List<MessageExchange.Subscriber> list = (
                from p in MessageExchange.Singleton._subscriberList
                where p.GetTopic() == topic
                select p).ToList<MessageExchange.Subscriber>();
            foreach (MessageExchange.Subscriber current in list)
            {
                try
                {
                    current.GetMessageNotifyHandler()(message);
                }
                catch
                {
                }
            }
        }

        public void Subscribe(object subscriber, string topic, MessageNotifyHandler notifyHandler)
        {
            if (string.IsNullOrEmpty(topic))
            {
                return;
            }
            if (subscriber == null)
            {
                return;
            }
            if (notifyHandler == null)
            {
                return;
            }
            int num = (
                from p in MessageExchange.Singleton._subscriberList
                where p.GetSubscriber() == subscriber && p.GetTopic() == topic
                select p).Count<MessageExchange.Subscriber>();
            if (num < 1)
            {
                lock (MessageExchange.Singleton._subscriberList)
                {
                    MessageExchange.Subscriber @class = new MessageExchange.Subscriber();
                    @class.SetSubscriber(subscriber);
                    @class.SetTopic(topic);
                    @class.SetMessageNotifyHandler(notifyHandler);
                    MessageExchange.Singleton._subscriberList.Add(@class);
                }
            }
        }

        public void Unsubscribe(object subscriber)
        {
            this.Unsubscribe(subscriber, String.Empty);
        }

        public void Unsubscribe(object subscriber, string topic)
        {
            IEnumerable<MessageExchange.Subscriber> source =
                 from p in MessageExchange.Singleton._subscriberList
                 where p.GetSubscriber() == subscriber
                 select p;
            if (!string.IsNullOrEmpty(topic))
            {
                source =
                    from p in source
                    where p.GetTopic() == topic
                    select p;
            }
            List<MessageExchange.Subscriber> list = source.ToList<MessageExchange.Subscriber>();
            if (list.Count > 0)
            {
                lock (MessageExchange.Singleton._subscriberList)
                {
                    foreach (MessageExchange.Subscriber current in list)
                    {
                        MessageExchange.Singleton._subscriberList.Remove(current);
                    }
                }
            }
        }


        public List<string> GetSubscribers(string topic)
        {
            throw new NotImplementedException();
        }
        #endregion

        private class Subscriber
        {
            private object _subscriber;

            private string _topic;

            private MessageNotifyHandler _messageNotifyHandler;

            public object GetSubscriber()
            {
                return this._subscriber;
            }

            public void SetSubscriber(object object_1)
            {
                this._subscriber = object_1;
            }

            public string GetTopic()
            {
                return this._topic;
            }

            public void SetTopic(string topic)
            {
                this._topic = topic;
            }

            public MessageNotifyHandler GetMessageNotifyHandler()
            {
                return this._messageNotifyHandler;
            }

            public void SetMessageNotifyHandler(MessageNotifyHandler messageNotifyHandler)
            {
                this._messageNotifyHandler = messageNotifyHandler;
            }
        }

    }
}
