using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MIS.Foundation.Framework.Http
{
    /// <summary>
    /// HTTP Response 处理委托
    /// </summary>
    /// <typeparam name="T">返回数据模型类型</typeparam>
    /// <param name="resultInfo">返回数据模型</param>
    public delegate void ResponseHandler<T>(ResultInfo<T> resultInfo);
    /// <summary>
    /// HTTP Request请求行为抽象
    /// </summary>
    public interface IHttp
    {
        /// <summary>
        /// 构建HTTP Request请求
        /// </summary>
        /// <typeparam name="T">返回数据模型类型</typeparam>
        /// <param name="method">数据提交方式(POST/GET)</param>
        /// <param name="area">区域</param>
        /// <param name="controller">控制器</param>
        /// <param name="action">方法名称</param>
        /// <param name="parameter">参数</param>
        /// <param name="handler">回调处理委托</param>
        void Request<T>(String method, String area, String controller, String action, String parameter, ResponseHandler<T> handler);

        /// <summary>
        /// 构建HTTP Request请求
        /// </summary>
        /// <typeparam name="T">返回数据模型类型</typeparam>
        /// <param name="method">数据提交方式(POST/GET)</param>
        /// <param name="area">区域</param>
        /// <param name="controller">控制器</param>
        /// <param name="action">方法名称</param>
        /// <param name="parameters">参数</param>
        /// <param name="handler">回调处理委托</param>
        void Request<T>(String method, String area, String controller, String action, List<RequestParams> parameters, ResponseHandler<T> handler);

        /// <summary>
        /// 构建HTTP Request请求
        /// </summary>
        /// <typeparam name="T">返回数据模型类型</typeparam>
        /// <param name="method">数据提交方式(POST/GET)</param>
        /// <param name="url">HTTP Url</param>
        /// <param name="parameter">参数</param>
        /// <param name="handler">回调处理委托</param>
        void Request<T>(String method, String url, String parameter, ResponseHandler<T> handler);

        /// <summary>
        /// 构建HTTP Request请求
        /// </summary>
        /// <typeparam name="T">返回数据模型类型</typeparam>
        /// <param name="method">数据提交方式(POST/GET)</param>
        /// <param name="url">HTTP Url</param>
        /// <param name="parameters">参数</param>
        /// <param name="handler">回调处理委托</param>
        void Request<T>(String method, String url, List<RequestParams> parameters, ResponseHandler<T> handler);

        /// <summary>
        /// 构建HTTP Request请求
        /// </summary>
        /// <typeparam name="T">返回数据模型类型</typeparam>
        /// <param name="method">数据提交方式(POST/GET)</param>
        /// <param name="area">区域</param>
        /// <param name="controller">控制器</param>
        /// <param name="action">方法名称</param>
        /// <param name="parameter">参数</param>
        /// <returns>返回数据内容</returns>
        ResultInfo<T> Request<T>(String method, String area, String controller, String action, String parameter);

        /// <summary>
        /// 构建HTTP Request请求
        /// </summary>
        /// <typeparam name="T">返回数据模型类型</typeparam>
        /// <param name="method">数据提交方式(POST/GET)</param>
        /// <param name="area">区域</param>
        /// <param name="controller">控制器</param>
        /// <param name="action">方法名称</param>
        /// <param name="parameters">参数</param>
        /// <returns>返回数据内容</returns>
        ResultInfo<T> Request<T>(String method, String area, String controller, String action, List<RequestParams> parameters);

        /// <summary>
        /// 构建HTTP Request请求
        /// </summary>
        /// <typeparam name="T">返回数据模型类型</typeparam>
        /// <param name="method">数据提交方式(POST/GET)</param>
        /// <param name="url">HTTP Url</param>
        /// <param name="parameter">参数</param>
        /// <returns>返回数据内容</returns>
        ResultInfo<T> Request<T>(String method, String url, String parameter);

        /// <summary>
        /// 构建HTTP Request请求
        /// </summary>
        /// <typeparam name="T">返回数据模型类型</typeparam>
        /// <param name="method">数据提交方式(POST/GET)</param>
        /// <param name="url">HTTP Url</param>
        /// <param name="parameters">参数</param>
        /// <returns>返回数据内容</returns>
        ResultInfo<T> Request<T>(String method, String url, List<RequestParams> parameters);
    }
}
