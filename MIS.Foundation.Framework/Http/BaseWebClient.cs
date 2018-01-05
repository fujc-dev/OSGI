using System;
using System.Net;

namespace MIS.Foundation.Framework.Http
{
    /// <summary>
    /// WebClient派生类
    /// </summary>
    internal class BaseWebClient : WebClient
    {
        public BaseWebClient()
        {
            this.Headers.Add("Content-Type", "application/x-www-form-urlencoded;charset=utf-8");
        }
    }
}
