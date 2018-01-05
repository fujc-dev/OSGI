using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace MIS.Foundation.Framework.Http
{
    internal class HttpBuilder
    {
        public static String BuilderHttpRequestUrl(string host, string area, string controller, string action)
        {
            return string.Format("{0}{1}/{2}/{3}", host, area, controller, action);
        }

        public static String BuilderHttpRequestUrl(string host, string url)
        {
            return string.Format("{0}{1}", host, url);
        }

        public static string BuilderParamesToString(List<RequestParams> parames)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < parames.Count; i++)
            {
                if (i != 0)
                {
                    sb.AppendFormat("&");
                }
                sb.AppendFormat("{0}={1}", parames[i].Key, HttpUtility.UrlEncode(parames[i].Value), Encoding.GetEncoding("gb2312"));
            }
            return sb.ToString();
        }

        public static T JsonToObject<T>(byte[] responseData)
        {
            string returnStr = Encoding.UTF8.GetString(responseData);
            return JsonConvert.DeserializeObject<T>(returnStr);
        }
    }
}
