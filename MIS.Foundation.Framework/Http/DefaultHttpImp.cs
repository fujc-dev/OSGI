using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MIS.Foundation.Framework.Http
{
    /// <summary>
    /// WEB Client方式模拟HTTP请求
    /// </summary>
    public class DefaultHttpImp : IHttp
    {
        private String mHost = "http://localhost:8888/";

        public DefaultHttpImp(String host)
        {
            this.mHost = host;
        }

        public void Request<T>(string method, string area, string controller, string action, string parameter, ResponseHandler<T> handler)
        {
            //构建HTTP Request Url
            var url = HttpBuilder.BuilderHttpRequestUrl(this.mHost, area, controller, action);
            using (var webClient = new BaseWebClient())
            {
                ResultInfo<T> info = new ResultInfo<T>();
                try
                {
                    if (method.Equals(MethodState.POST))
                    {
                        //构建提交参数
                        byte[] postData = Encoding.UTF8.GetBytes(parameter);
                        byte[] responseData = webClient.UploadData(url, "POST", postData);
                        info.Data = HttpBuilder.JsonToObject<T>(responseData);
                    }
                    else
                    {
                        byte[] responseData = webClient.DownloadData(String.Format("{0}?{1}", url, parameter));
                        info.Data = HttpBuilder.JsonToObject<T>(responseData);
                    }
                    //Convert Json To Data Model

                    //info.State = ResultState.Success;
                }
                catch (Exception e)
                {
                    info.Message.Msg = e.Message;
                    info.Message.State = ResultState.Failure;
                }
                finally
                {
                    if (handler != null) handler(info);
                }
            }
        }

        public void Request<T>(string method, string area, string controller, string action, List<RequestParams> parameters, ResponseHandler<T> handler)
        {
            this.Request<T>(method, area, controller, action, HttpBuilder.BuilderParamesToString(parameters), handler);
        }

        public ResultInfo<T> Request<T>(string method, string area, string controller, string action, string parameter)
        {
            //构建HTTP Request Url
            var url = HttpBuilder.BuilderHttpRequestUrl(this.mHost, area, controller, action);
            using (var webClient = new BaseWebClient())
            {
                ResultInfo<T> info = new ResultInfo<T>();
                try
                {
                    if (method.Equals(MethodState.POST))
                    {
                        //构建提交参数
                        byte[] postData = Encoding.UTF8.GetBytes(parameter);
                        byte[] responseData = webClient.UploadData(url, method, postData);
                        //Convert Json To Data Model
                        info.Data = HttpBuilder.JsonToObject<T>(responseData);
                    }
                    else
                    {
                        byte[] responseData = webClient.DownloadData(String.Format("{0}?{1}", url, parameter));
                        info.Data = HttpBuilder.JsonToObject<T>(responseData);
                    }
                    //info.Message.State = ResultState.Success; 成功可忽略
                }
                catch (Exception e)
                {
                    info.Message.Msg = e.Message;
                    info.Message.State = ResultState.Failure;
                }
                return info;
            }
        }

        public ResultInfo<T> Request<T>(string method, string area, string controller, string action, List<RequestParams> parameters)
        {
            return this.Request<T>(method, area, controller, action, HttpBuilder.BuilderParamesToString(parameters));
        }

        public void Request<T>(string method, string url, string parameter, ResponseHandler<T> handler)
        {
            String _url = HttpBuilder.BuilderHttpRequestUrl(this.mHost, url);
            using (var webClient = new BaseWebClient())
            {
                ResultInfo<T> info = new ResultInfo<T>();
                try
                {
                    if (method.Equals(MethodState.POST))
                    {
                        //构建提交参数
                        byte[] postData = Encoding.UTF8.GetBytes(parameter);
                        byte[] responseData = webClient.UploadData(_url, "POST", postData);
                        info.Data = HttpBuilder.JsonToObject<T>(responseData);
                    }
                    else
                    {
                        byte[] responseData = webClient.DownloadData(String.Format("{0}?{1}", _url, parameter));
                        info.Data = HttpBuilder.JsonToObject<T>(responseData);
                    }
                    //Convert Json To Data Model

                    //info.State = ResultState.Success;
                }
                catch (Exception e)
                {
                    info.Message.Msg = e.Message;
                    info.Message.State = ResultState.Failure;
                }
                finally
                {
                    if (handler != null) handler(info);
                }
            }
        }

        public void Request<T>(string method, string url, List<RequestParams> parameters, ResponseHandler<T> handler)
        {
            this.Request<T>(method, url, HttpBuilder.BuilderParamesToString(parameters), handler);
        }

        public ResultInfo<T> Request<T>(string method, string url, string parameter)
        {
            String _url = HttpBuilder.BuilderHttpRequestUrl(this.mHost, url);
            using (var webClient = new BaseWebClient())
            {
                ResultInfo<T> info = new ResultInfo<T>();
                try
                {
                    if (method.Equals(MethodState.POST))
                    {
                        //构建提交参数
                        byte[] postData = Encoding.UTF8.GetBytes(parameter);
                        byte[] responseData = webClient.UploadData(new Uri(_url), method, postData);
                        info.Data = HttpBuilder.JsonToObject<T>(responseData);
                        //Convert Json To Data Model
                    }
                    else
                    {
                        byte[] responseData = webClient.DownloadData(String.Format("{0}?{1}", _url, parameter));
                        info.Data = HttpBuilder.JsonToObject<T>(responseData);
                    }
                    //info.Message.State = ResultState.Success; 成功可忽略
                }
                catch (Exception e)
                {
                    info.Message.Msg = e.Message;
                    info.Message.State = ResultState.Failure;
                }
                return info;
            }
        }

        public ResultInfo<T> Request<T>(string method, string url, List<RequestParams> parameters)
        {
            return this.Request<T>(method, url, HttpBuilder.BuilderParamesToString(parameters));
        }
    }
}
