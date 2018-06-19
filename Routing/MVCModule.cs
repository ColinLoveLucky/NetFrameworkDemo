using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Routing
{
    public class MVCModule : IHttpModule
    {
        private static int i = 0;
        public void Dispose()
        {
           // throw new NotImplementedException();
        }

        public int CountWhiteSpace(string text)
        {
            Contract.Requires<ArgumentNullException>(string.IsNullOrEmpty(text), "text");
            Contract.Ensures(Contract.Result<int>() > 0);
            return text.Count(char.IsWhiteSpace);
        }

        public void Init(HttpApplication context)
        {
           // if(!context.Request.AppRelativeCurrentExecutionFilePath.Contains(".ico"))
           // {
                context.BeginRequest += context_BeginRequest;

                context.AuthenticateRequest += context_AuthenticateRequest;

                context.PostAuthenticateRequest += context_PostAuthenticateRequest;

                context.AuthorizeRequest += context_AuthorizeRequest;

                context.PostAuthorizeRequest += context_PostAuthorizeRequest;

                context.ResolveRequestCache += context_ResolveRequestCache;

                context.PostResolveRequestCache += context_PostResolveRequestCache;

                context.MapRequestHandler += context_MapRequestHandler;

                context.PostMapRequestHandler += context_PostMapRequestHandler;

                context.AcquireRequestState += context_AcquireRequestState;

                context.PostAcquireRequestState += context_PostAcquireRequestState;

                context.PreRequestHandlerExecute += context_PreRequestHandlerExecute;

                context.PostRequestHandlerExecute += context_PostRequestHandlerExecute;

                context.ReleaseRequestState += context_ReleaseRequestState;

                context.PostReleaseRequestState += context_PostReleaseRequestState;

                context.PostUpdateRequestCache += context_PostUpdateRequestCache;

                context.PreSendRequestHeaders += context_PreSendRequestHeaders;

                context.PreSendRequestContent += context_PreSendRequestContent;

                context.RequestCompleted += context_RequestCompleted;
         //   }
           
        }

        void context_RequestCompleted(object sender, EventArgs e)
        {
         //   HttpContext.Current.Response.Write("请求结束时发生&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp");
        }

        void context_PreSendRequestContent(object sender, EventArgs e)
        {
           
            HttpContext.Current.Response.Write("向客户端发送之前发生&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp");
        }

        private void context_ResolveRequestCache(object sender, EventArgs e)
        {
            HttpContext.Current.Response.Write("从缓存模块提供服务时发生&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp");
        }

        void context_PreSendRequestHeaders(object sender, EventArgs e)
        {
            HttpContext.Current.Response.Write("向客户端发送Http头之前发生&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp");
        }

        private void context_ReleaseRequestState(object sender, EventArgs e)
        {
            HttpContext.Current.Response.Write("执行完所有事件时发生&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp");
        }

        void context_PreRequestHandlerExecute(object sender, EventArgs e)
        {
            HttpContext.Current.Response.Write("开始执行之前处理请求发生&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp");
        }

        void context_PostRequestHandlerExecute(object sender, EventArgs e)
        {
            HttpContext.Current.Response.Write("处理请求执行完毕时发生&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp");
        }

        void context_PostReleaseRequestState(object sender, EventArgs e)
        {
            HttpContext.Current.Response.Write("已完成所有的请求处理程序并请求状态以保存时发生&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp");
        }

        void context_PostAuthorizeRequest(object sender, EventArgs e)
        {
            HttpContext.Current.Response.Write("已建立用户授权时发生&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp");
        }

        void context_PostAuthenticateRequest(object sender, EventArgs e)
        {
            HttpContext.Current.Response.Write("安全模块建立用户标识&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp");
        }

        void context_MapRequestHandler(object sender, EventArgs e)
        {
            HttpContext.Current.Response.Write("选择了响应的请求处理程序&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp");
        }

        void context_AcquireRequestState(object sender, EventArgs e)
        {
            HttpContext.Current.Response.Write("获得当前请求的状态&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp");
        }

        private void context_PostAcquireRequestState(object sender, EventArgs e)
        {
            HttpContext.Current.Response.Write("已经获得当前请求的状态&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp");
        }

        void context_PostUpdateRequestCache(object sender, EventArgs e)
        {

            HttpContext.Current.Response.Write("更新缓存&nbsp&nbsp&nbsp");
        }

        void context_AuthorizeRequest(object sender, EventArgs e)
        {
            HttpContext.Current.Response.Write("授权验证&nbsp&nbsp&nbsp");
        }

        void context_BeginRequest(object sender, EventArgs e)
        {
            //string path = HttpContext.Current.Server.MapPath("log.txt");
            //using (var stream = new StreamWriter(path))
            //{
            //    stream.WriteLine(DateTime.Now.ToString());
            //}

            HttpContext.Current.Response.Write("开始请求&nbsp&nbsp&nbsp&nbsp开始请求时间" +DateTime.Now.ToString());
        }

        void context_AuthenticateRequest(object sender, EventArgs e)
        {
            HttpContext.Current.Response.Write("身份认证&nbsp&nbsp&nbsp");
        }

        void context_PostMapRequestHandler(object sender, EventArgs e)
        {

            //var url = HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath;

            //string path =@"D:\FrameWork4.0TestDemo\WebForm\FileWrite.txt";

            //using( var  file = new StreamWriter(path,true))
            //{
            // //   file.WriteLine(url);

            //    file.WriteLine(url);

            //    file.Close();
            //}

            HttpContext.Current.Response.Write("映射到响应时间是发生&nbsp&nbsp&nbsp");


        }

        void context_PostResolveRequestCache(object sender, EventArgs e)
        {

            //var httpContext = HttpContext.Current;

            //var routeData = RouteTable.Routes.GetRouteData();
            //if (routeData == null)
            //    throw new Exception("null");
            //var handler = routeData.RouteHandler.GetRouteHandler();

            //// handler.ProcessRequest(httpContext);

            //httpContext.RemapHandler(handler);

          //  HttpContext.Current.Response.Write(@"解析缓存&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp");

        }

    }
}
