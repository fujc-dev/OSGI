using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MIS.Foundation.Framework.Http
{
    /// <summary>
    /// GET/POST请求参数
    /// </summary>
    public class RequestParams
    {
        public RequestParams() { }
        public RequestParams(String key, String value)
        {
            this.Key = key;
            this.Value = value;
        }
        /// <summary>
        /// 参数键
        /// </summary>
        public String Key { get; set; }
        /// <summary>
        /// 参数值
        /// </summary>
        public String Value { get; set; }
    }
}
