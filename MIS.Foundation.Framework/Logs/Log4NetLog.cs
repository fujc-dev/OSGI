using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MIS.Foundation.Framework
{
    /// <summary>
    /// Log4NetLog
    /// </summary>
    public class Log4NetLog : ILog
    {
        private log4net.ILog m_Log;

        public Log4NetLog(log4net.ILog log)
        {
            if (log == null)
                throw new ArgumentNullException("log");

            m_Log = log;
        }

        public bool IsDebugEnabled
        {
            get { return m_Log.IsDebugEnabled; }
        }

        public bool IsErrorEnabled
        {
            get { return m_Log.IsErrorEnabled; }
        }

        public bool IsFatalEnabled
        {
            get { return m_Log.IsFatalEnabled; }
        }

        public bool IsInfoEnabled
        {
            get { return m_Log.IsInfoEnabled; }
        }

        public bool IsWarnEnabled
        {
            get { return m_Log.IsWarnEnabled; }
        }


        public void Debug(object message)
        {
            m_Log.Debug(message);
        }

        public void Debug(object message, Exception exception)
        {
            m_Log.Debug(message, exception);
        }

        public void Error(object message)
        {
            m_Log.Error(message);
        }

        public void Error(object message, Exception exception)
        {
            m_Log.Error(message, exception);
        }

        public void Fatal(object message)
        {
            m_Log.Fatal(message);
        }

        public void Fatal(object message, Exception exception)
        {
            m_Log.Debug(message, exception);
        }

        public void Warn(object message)
        {
            m_Log.Warn(message);
        }

        public void Warn(object message, Exception exception)
        {
            m_Log.Warn(message, exception);
        }

        public void Info(object message)
        {
            m_Log.Info(message);
        }

        public void Info(object message, Exception exception)
        {
            m_Log.Info(message, exception);
        }
    }
}
