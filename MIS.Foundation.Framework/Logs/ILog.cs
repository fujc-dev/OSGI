using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZHGL.Utils
{
    public interface ILog
    {
        bool IsDebugEnabled { get; }  // 是否启用Debug日志
        bool IsErrorEnabled { get; }    //是否启用Error日志
        bool IsFatalEnabled { get; }    //是否启用Fatal日志
        bool IsInfoEnabled { get; }     //是否启用Info日志
        bool IsWarnEnabled { get; }   //是否启用Warn日志

        void Debug(object message);
        void Debug(object message, Exception exception);

        void Error(object message);
        void Error(object message, Exception exception);

        void Fatal(object message);
        void Fatal(object message, Exception exception);

        void Warn(object message);
        void Warn(object message, Exception exception);

        void Info(object message);
        void Info(object message, Exception exception);
    }
}
