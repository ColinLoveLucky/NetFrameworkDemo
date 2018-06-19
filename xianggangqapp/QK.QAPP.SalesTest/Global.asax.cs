using QK.QAPP.Infrastructure;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using QK.QAPP.Global;
using QK.QAPP.SalesTest.App_Start;
using QK.QAPP.Infrastructure.MessageQueue;
using QK.QAPP.SalesTest.Hubs;
using QK.QAPP.Entity;
using System.Web;
using QK.QAPP.IServices;
using QK.QAPP.SalesTest;
using StackExchange.Redis;
using RedisSessionProvider.Config;
using System.Collections.Generic;

namespace QK.QAPP.SalesTest
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            //初始化依赖注入
            // Bootstrapers.Init();
            AutofacConfig.Bootstrapper();

            AutofacConfig.InitGlobalConfig();

            //EF日志跟踪记录todo临时关闭
            TraceComponent.Init();
            //Page页面性能监控
            //ProfilterHepler.Init();  
            //初始化Session
            //RedisSessionStateStoreProvider.Init();

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            if (GlobalSetting.NR_Listener_Enable == "TRUE")
            {
                try
                {
                    MQConsumer NR_Consumer = new MQConsumer(
                                                GlobalSetting.MQMultipleServer,
                                                GlobalSetting.MQUserName,
                                                GlobalSetting.MQUserPassword,
                                                GlobalSetting.MQApplication_NR
                                             );
                    NR_Consumer.Listener += NR_Consumer_Listener;
                }
                catch (System.Exception ex)
                {
                    //System.Diagnostics.Trace.Write(ex);
                }
            }

            ////全局设置 写入
            //var globalConfigService = Ioc.GetService<IAPP_GLOBALCONFIGSERVICE>();
            //Global.GlobalSetting.GlobalSettingDic = globalConfigService.InitGlobalSetting();
        }

        protected void Application_BeginRequest()
        {

        }

        protected void Application_EndRequest()
        {
            var context = new HttpContextWrapper(Context);
            if (context.Request.IsAjaxRequest() && context.Response.StatusCode == 302)
            {
                Context.Response.Clear();
                Context.Response.Write("授权过期,请重新登录");
                Context.Response.StatusCode = 401;
            }
        }



        private void NR_Consumer_Listener(MessageBody message)
        {
            //根据传输ID获取补件信息，然后根据模板推送消息通知            
            if (message != null && !string.IsNullOrWhiteSpace(message.Text))
            {
                var AppCode = message.Text.Trim();
                System.Diagnostics.Trace.Write("收到补件消息:" + AppCode);
                IV_APPMAINSERVICE VappMainService = Ioc.GetService<IV_APPMAINSERVICE>();
                IAPP_MSGBOXSERVICE MsgBoxSerivce = Ioc.GetService<IAPP_MSGBOXSERVICE>();
                var NR_Entity = VappMainService.FirstOrDefault(o => o.APPCODE == AppCode);
                if (NR_Entity != null)
                {
                    var ToUser = NR_Entity.CREATEDUSER;
                    var CustomerName = NR_Entity.CUSTOMERNAME;
                    var Url = string.Format("/LoanApplication/Application?dformCode={0}&operation=2&appid={1}"
                                            , NR_Entity.LOGO
                                            , NR_Entity.APPID);
                    var Content = string.Format(GlobalSetting.APPLICATION_NR_CONTENT, NR_Entity.CUSTOMERNAME);
                    var app_main_enity = new
                    {
                        CustomerName = NR_Entity.CUSTOMERNAME,
                        AppCode = NR_Entity.APPCODE,
                        Logo = NR_Entity.LOGO,
                        AppId = NR_Entity.APPID,
                        Title = "客户补件通知",
                        Content = Content,
                        Url = Url
                    };

                    APP_MSGBOX msg = new APP_MSGBOX()
                    {
                        USERNAME = ToUser,
                        TITLE = app_main_enity.Title,
                        CONTENT = app_main_enity.Content,
                        STATUS = MessageStatus.UnProcess.ToString(),
                        CREATETITME = System.DateTime.Now,
                        PRIORTYLEVEL = MessagePRIORTYLEVEL.HIGHT.ToString(),
                        CATEGORY = PushMsgType.SupplementStart.ToString(),
                        URL = app_main_enity.Url,
                        APPCODE = app_main_enity.AppCode
                    };
                    MsgBoxSerivce.Add(msg);
                    MsgBoxSerivce.UnitOfWork.SaveChanges();

                    var strContent = Serializer.ToJson(app_main_enity);
                    PushMessage.PushToUser(ToUser, strContent, PushMsgType.SupplementStart.ToString());
                }
            }
        }

    }
}
