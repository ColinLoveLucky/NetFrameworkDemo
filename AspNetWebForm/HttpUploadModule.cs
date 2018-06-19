using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;

namespace AspNetWebForm
{
    public class HttpUploadModule : IHttpModule
    {
        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public void Init(HttpApplication context)
        {
            context.BeginRequest += context_BeginRequest;
        }

        void context_BeginRequest(object sender, EventArgs e)
        {
            HttpApplication app = sender as HttpApplication;

            app.Response.Write("Hello First");

            HttpWorkerRequest request = GetWorkRequest(app.Context);

            Encoding encoding = app.Context.Request.ContentEncoding;

            int bytesRead = 0;

            int read;

            int count = 8192;

            byte[] buffer;

            if (request != null)
            {
                byte[] tempBuff = request.GetPreloadedEntityBody();

                if (tempBuff != null && IsUploadRequest(app.Request))
                {
                    long length = long.Parse(
                        request.GetKnownRequestHeader(HttpWorkerRequest.HeaderContentLength));

                    buffer = new byte[length];

                    count = tempBuff.Length;

                    Buffer.BlockCopy(tempBuff, 0, buffer, bytesRead, count);

                    bytesRead = tempBuff.Length;

                    while (request.IsClientConnected() &&
                        !request.IsEntireEntityBodyIsPreloaded() && bytesRead < length)
                    {
                        if(bytesRead+count>length)
                        {
                            count = (int)(length - bytesRead);
                            tempBuff = new byte[count];
                        }

                        read = request.ReadEntityBody(tempBuff, count);

                        Buffer.BlockCopy(tempBuff, 0, buffer, bytesRead, read);

                        bytesRead += read;

                        if(request.IsClientConnected()&&!request.IsEntireEntityBodyIsPreloaded())
                        {
                          InjectTextParts(request, buffer);
                        }
                      
                    }


                }


            }
        }

        private HttpWorkerRequest GetWorkRequest(HttpContext context)
        {
            IServiceProvider provider = (IServiceProvider)HttpContext.Current;

            return (HttpWorkerRequest)provider.GetService(typeof(HttpWorkerRequest));
        }

        private static bool StringStartsWithAnotherIgnoreCase(string s1, string s2)
        {
            return (string.Compare(s1, 0, s2, 0, s2.Length, true, CultureInfo.InvariantCulture) == 0);
        }

        private bool IsUploadRequest(HttpRequest request)
        {
            return StringStartsWithAnotherIgnoreCase(request.ContentType, "multipart/form-data");
        }

        private void InjectTextParts(HttpWorkerRequest request, byte[] textParts)
        {
            BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.NonPublic;

            Type type = request.GetType();

            while ((type != null) && (type.FullName != "System.Web.Hosting.ISAPIWorkerRequest"))
            {
                type = type.BaseType;
            }

            if (type != null)
            {
                type.GetField("_contentAvailLength", bindingFlags).SetValue(request, textParts.Length);
                type.GetField("_contentTotalLength", bindingFlags).SetValue(request, textParts.Length);
                type.GetField("_preloadedContent", bindingFlags).SetValue(request, textParts);
                type.GetField("_preloadedContentRead", bindingFlags).SetValue(request, true);
            }
        }
    }
}