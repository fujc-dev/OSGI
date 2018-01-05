using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MIS.Foundation.Framework.Http.Config;

namespace MIS.Foundation.Framework.Http
{
    /// <summary>
    /// HTTP工厂类，构建所需要的HTTP相关配置
    /// </summary>
    public class HttpFactory
    {
        /// <summary>
        /// 默认构造函数，请求参考 <see cref="ZHGLSystem.Service.DefaultHttpImp"/> 类
        /// </summary>
        public HttpFactory()  //String host
        {

        }

        public IHttp GetInitialization()
        {
            HttpConfigurationSection httpSection = (HttpConfigurationSection)ConfigurationManager.GetSection("httpConfig");
            String host = httpSection.ZhglHttp.Host;  //配置
            //可扩展其他模拟HTTP 请求，HttpClient、HttpWebRequest 等
            return new DefaultHttpImp(host);
        }

        public IHttp ZTGetInitialization()
        {
            HttpConfigurationSection httpSection = (HttpConfigurationSection)ConfigurationManager.GetSection("httpConfig");
            String host = httpSection.ZTHttp.Host;  //配置
            //可扩展其他模拟HTTP 请求，HttpClient、HttpWebRequest 等
            return new DefaultHttpImp(host);
        }
    }
}
