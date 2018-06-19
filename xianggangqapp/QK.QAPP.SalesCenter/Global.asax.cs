using QK.QAPP.Infrastructure;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using QK.QAPP.Global;
using QK.QAPP.Infrastructure.Data.EFRepository.Repository;
using QK.QAPP.Infrastructure.Log4Net;
using QK.QAPP.SalesCenter.App_Start;
using QK.QAPP.Infrastructure.MessageQueue;
using QK.QAPP.SalesCenter.Hubs;
using QK.QAPP.Entity;
using System.Web;
using QK.QAPP.IServices;

namespace QK.QAPP.SalesCenter
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            //MVC 初始化依赖注入
            //Bootstrapers.Init();
            AutofacConfig.Bootstrapper();

            //SignalR 初始化依赖注入
            AutofacSignalRConfig.Bootstrapper();

            //初始化获取数据库配置文件字典方法
            AutofacConfig.InitGlobalConfig();

            //EF日志跟踪记录todo临时关闭
            TraceComponent.Init();

            //日志初始化
            log4net.Config.XmlConfigurator.Configure();

            //Page页面性能监控
            //ProfilterHepler.Init();  
            //初始化Session
            //RedisSessionStateStoreProvider.Init();

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            StartMQListen();
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

        /// <summary>
        /// 监听消息队列
        /// </summary>
        private void StartMQListen()
        {
            #region 信贷补件

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
                    LogWriter.Error("MQ消息服务器发送失败", ex);
                }
            }

            #endregion

            #region 车贷补件

            if (GlobalSetting.Car_NR_Listener_Enable == "TRUE")
            {
                try
                {
                    MQConsumer Car_NR_Consumer = new MQConsumer(
                                                GlobalSetting.MQMultipleServer,
                                                GlobalSetting.MQUserName,
                                                GlobalSetting.MQUserPassword,
                                                GlobalSetting.MQ_Car_Application_NR
                                             );
                    Car_NR_Consumer.Listener += Car_NR_Consumer_Listener;

                    MQConsumer Car_Entry_NR_Consumer = new MQConsumer(
                        GlobalSetting.MQMultipleServer,
                        GlobalSetting.MQUserName,
                        GlobalSetting.MQUserPassword,
                        GlobalSetting.MQ_Car_Entry_Application_NR);
                    Car_Entry_NR_Consumer.Listener += Car_NR_Consumer_Listener;
                }
                catch (System.Exception ex)
                {
                    //System.Diagnostics.Trace.Write(ex);
                    LogWriter.Error("MQ消息服务器发送失败", ex);
                }
            }

            #endregion

            #region 待评估车贷

            try
            {
                MQConsumer Car_ToBeAssess_Consumer = new MQConsumer(
                                            GlobalSetting.MQMultipleServer,
                                            GlobalSetting.MQUserName,
                                            GlobalSetting.MQUserPassword,
                                            GlobalSetting.MQ_CAR_TO_BE_ASSESSED
                                         );
                Car_ToBeAssess_Consumer.Listener += Car_ToBeAssess_Consumer_Listener;
            }
            catch (System.Exception ex)
            {
                //System.Diagnostics.Trace.Write(ex);
                LogWriter.Error("MQ消息服务器发送失败", ex);
            }

            #endregion

            #region 房贷补件

            if (GlobalSetting.House_NR_Listener_Enable == "TRUE")
            {
                try
                {
                    MQConsumer House_NR_Consumer = new MQConsumer(
                                                GlobalSetting.MQMultipleServer,
                                                GlobalSetting.MQUserName,
                                                GlobalSetting.MQUserPassword,
                                                GlobalSetting.MQ_House_Application_NR
                                             );
                    House_NR_Consumer.Listener += House_NR_Consumer_Listener;
                }
                catch (System.Exception ex)
                {
                    //System.Diagnostics.Trace.Write(ex);
                    LogWriter.Error("MQ消息服务器发送失败-房贷补件", ex);
                }
            }

            #endregion
        }

        #region MQ消息处理

        private void NR_Consumer_Listener(MessageBody message)
        {
            if (message != null && !string.IsNullOrWhiteSpace(message.Text))
            {
                var AppCode = message.Text.Trim();
                //System.Diagnostics.Trace.Write("收到补件消息:" + AppCode);
                LogWriter.Info("收到补件消息:" + AppCode);

                using (EFRepositoryTransaction efRepository = new EFRepositoryTransaction(GlobalSetting.MainDataBasenameOrConnectionString))
                {
                    var VappMainService = efRepository.GetRepository<V_APPMAIN>();
                    var MsgBoxSerivce = efRepository.GetRepository<APP_MSGBOX>();
                    var globalService = efRepository.GetRepository<APP_GLOBALCONFIG>();
                    var app_nr_content_entity = globalService.FirstOrDefault(c => c.KEY == "APPLICATION_NR_CONTENT");
                    var app_nr_content = "";
                    if (app_nr_content_entity != null)
                    {
                        app_nr_content = app_nr_content_entity.VALUE;
                    }
                    //var IAPP_GLOBALCONFIGSERVICE=efRepository
                    var NR_Entity = VappMainService.FirstOrDefault(o => o.APPCODE == AppCode);
                    if (NR_Entity != null)
                    {
                        var ToUser = NR_Entity.CREATEDUSER;
                        var CustomerName = NR_Entity.CUSTOMERNAME;
                        var Url = string.Format("/LoanApplication/Application?dformCode={0}&operation=2&appid={1}"
                                                , NR_Entity.LOGO
                                                , NR_Entity.APPID);
                        var Content = string.Format(app_nr_content, NR_Entity.CUSTOMERNAME);
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
                        efRepository.Commit();

                        var strContent = Serializer.ToJson(app_main_enity);
                        PushMessage.PushToUser(ToUser, strContent, PushMsgType.SupplementStart.ToString());
                    }
                }

            }
        }

        private void Car_NR_Consumer_Listener(MessageBody message)
        {
            if (message != null && !string.IsNullOrWhiteSpace(message.Text))
            {
                var AppCode = message.Text.Trim();
                //System.Diagnostics.Trace.Write("收到补件消息:" + AppCode);
                LogWriter.Info("收到车贷补件/修改资料消息:" + AppCode);

                using (EFRepositoryTransaction efRepository = new EFRepositoryTransaction(GlobalSetting.MainDataBasenameOrConnectionString))
                {
                    var VappMainService = efRepository.GetRepository<V_APPMAIN>();
                    var MsgBoxSerivce = efRepository.GetRepository<APP_MSGBOX>();
                    var globalService = efRepository.GetRepository<APP_GLOBALCONFIG>();
                    var app_nr_content_entity = globalService.FirstOrDefault(c => c.KEY == "CAR_APPLICATION_NR_CONTENT");
                    var app_nr_content = "";
                    if (app_nr_content_entity != null)
                    {
                        app_nr_content = app_nr_content_entity.VALUE;
                    }
                    //var IAPP_GLOBALCONFIGSERVICE=efRepository
                    var NR_Entity = VappMainService.FirstOrDefault(o => o.APPCODE == AppCode);
                    if (NR_Entity != null)
                    {
                        var ToUser = NR_Entity.CREATEDUSER;
                        var CustomerName = NR_Entity.CUSTOMERNAME;
                        var Url = string.Format("/LoanApplication/Application?dformCode={0}&operation=4&appid={1}"
                                                , NR_Entity.LOGO
                                                , NR_Entity.APPID);
                        var Content = string.Format(app_nr_content, NR_Entity.CUSTOMERNAME);
                        var app_main_enity = new
                        {
                            CustomerName = NR_Entity.CUSTOMERNAME,
                            AppCode = NR_Entity.APPCODE,
                            Logo = NR_Entity.LOGO,
                            AppId = NR_Entity.APPID,
                            Title = "车贷补件/修改资料通知",
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
                            CATEGORY = PushMsgType.CarSupplementStart.ToString(),
                            URL = app_main_enity.Url,
                            APPCODE = app_main_enity.AppCode
                        };
                        MsgBoxSerivce.Add(msg);
                        efRepository.Commit();

                        var strContent = Serializer.ToJson(app_main_enity);
                        PushMessage.PushToUser(ToUser, strContent, PushMsgType.CarSupplementStart.ToString());
                    }
                }

            }
        }

        private void Car_ToBeAssess_Consumer_Listener(MessageBody message)
        {
            if (message != null && !string.IsNullOrWhiteSpace(message.Text))
            {
                var appCode = message.Text.Trim();
                //System.Diagnostics.Trace.Write("收到补件消息:" + AppCode);
                LogWriter.Info("收到待评估车贷消息:" + appCode);

                using (EFRepositoryTransaction efRepository = new EFRepositoryTransaction(GlobalSetting.MainDataBasenameOrConnectionString))
                {
                    var VappMainService = efRepository.GetRepository<V_APPMAIN>();
                    var appCustomerService = efRepository.GetRepository<APP_CUSTOMER>();
                    var appCarInfoService = efRepository.GetRepository<APP_CARINFO>();
                    var MsgBoxSerivce = efRepository.GetRepository<APP_MSGBOX>();
                    var queueAssessServie = efRepository.GetRepository<APP_QUEUE_ASSESS>();
                    var globalService = efRepository.GetRepository<APP_GLOBALCONFIG>();
                    var queueService = efRepository.GetRepository<APP_QUEUE>();
                    var app_content_entity = globalService.FirstOrDefault(c => c.KEY == "CAR_TO_BE_ASSESSED_CONTENT");
                    var app_content = "";
                    if (app_content_entity != null)
                    {
                        app_content = app_content_entity.VALUE;
                    }

                    var APP_Entity = VappMainService.FirstOrDefault(o => o.APPCODE == appCode);
                    if (APP_Entity != null)
                    {
                        var APP_Customer = appCustomerService.FirstOrDefault(c => c.APP_ID == APP_Entity.APPID);
                        if (APP_Customer == null)
                        {
                            return;
                        }

                        var APP_Carinfo = appCarInfoService.FirstOrDefault(c => c.APP_ID == APP_Entity.APPID);
                        if (APP_Carinfo == null)
                        {
                            return;
                        }

                        var ToUser = APP_Entity.CREATEDUSER;
                        var customerName = APP_Entity.CUSTOMERNAME;
                        var url = "/Assess/ApprovedList";
                        var content = string.Format(app_content, customerName);
                        //通知内容
                        var app_main_enity = new
                        {
                            CustomerName = customerName,
                            AppCode = appCode,
                            Logo = APP_Entity.LOGO,
                            AppId = APP_Entity.APPID,
                            Title = "车辆待评估通知",
                            Content = content,
                            Url = url
                        };
                        //写消息盒子
                        APP_MSGBOX msg = new APP_MSGBOX()
                        {
                            USERNAME = ToUser,
                            TITLE = app_main_enity.Title,
                            CONTENT = app_main_enity.Content,
                            STATUS = MessageStatus.UnProcess.ToString(),
                            CREATETITME = System.DateTime.Now,
                            PRIORTYLEVEL = MessagePRIORTYLEVEL.HIGHT.ToString(),
                            CATEGORY = PushMsgType.CarToBeAssessed.ToString(),
                            URL = app_main_enity.Url,
                            APPCODE = app_main_enity.AppCode
                        };
                        //若进件状态为评估已分配 CARASSESSDISEDWT ，才会写入评估队列
                        if (APP_Entity.APPSTATUS == EnterStatusType.CARASSESSDISEDWT.ToString())
                        {
                            APP_QUEUE_ASSESS assessInfo = queueAssessServie.FirstOrDefault(q => q.APP_CODE == appCode);
                            APP_QUEUE appQueue = queueService.FirstOrDefault(q => q.APP_CODE == appCode);
                            if (assessInfo == null)
                            {
                                //写评估队列
                                assessInfo = new APP_QUEUE_ASSESS()
                                {
                                    APP_ID = APP_Entity.APPID,
                                    APP_CODE = appCode,
                                    ASSESS_STATUS = AssessStatusType.CarAssessApplyApproved.ToString(),
                                    CHANGED_TIME = System.DateTime.Now,
                                    CHANGED_USER = "MQ Info",
                                    CREATED_TIME = System.DateTime.Now,
                                    CREATED_USER = "MQ Info",
                                    CUSTOMER_NAME = APP_Entity.CUSTOMERNAME,
                                    CUSTOMER_PHONE =
                                        (string.IsNullOrEmpty(APP_Customer.MOBILE1)
                                            ? APP_Customer.MOBILE2
                                            : APP_Customer.MOBILE1),
                                    PRODUCT_LOGO = APP_Entity.LOGO,
                                    SALES_CODE = APP_Entity.SALESNO,
                                    SALES_NAME = APP_Entity.SALESNAME,
                                    SALES_PHONE = "",
                                    VEHICLE_NO = APP_Carinfo.VEHICLE_NO,
                                    VERSION = 1
                                };
                                //评估师信息
                                if (appQueue != null)
                                {
                                    assessInfo.VALUATOR = appQueue.ASSESS_NO;
                                    assessInfo.VALUATOR_NAME = appQueue.ASSESS_NAME;
                                }
                                queueAssessServie.Add(assessInfo);
                            }
                            else
                            {
                                assessInfo.ASSESS_STATUS = AssessStatusType.CarAssessApplyApproved.ToString();
                                assessInfo.CHANGED_TIME = System.DateTime.Now;
                                assessInfo.CHANGED_USER = "MQ Info";
                                assessInfo.CREATED_TIME = System.DateTime.Now;
                                assessInfo.CREATED_USER = "MQ Info";
                                assessInfo.CUSTOMER_NAME = APP_Entity.CUSTOMERNAME;
                                assessInfo.CUSTOMER_PHONE = (string.IsNullOrEmpty(APP_Customer.MOBILE1)
                                    ? APP_Customer.MOBILE2
                                    : APP_Customer.MOBILE1);
                                assessInfo.PRODUCT_LOGO = APP_Entity.LOGO;
                                assessInfo.SALES_CODE = APP_Entity.SALESNO;
                                assessInfo.SALES_NAME = APP_Entity.SALESNAME;
                                assessInfo.SALES_PHONE = "";
                                assessInfo.VEHICLE_NO = APP_Carinfo.VEHICLE_NO;
                                assessInfo.VERSION = 1;
                                assessInfo.CUSTOMER_ARRIVE_TIME = null;
                                assessInfo.CUSTOMER_BOOK_TIME = null;
                                assessInfo.REMARK = string.Empty;

                                //评估师信息
                                if (appQueue != null)
                                {
                                    assessInfo.VALUATOR = appQueue.ASSESS_NO;
                                    assessInfo.VALUATOR_NAME = appQueue.ASSESS_NAME;
                                }


                                queueAssessServie.Update(assessInfo);
                            }
                        }
                        MsgBoxSerivce.Add(msg);
                        efRepository.Commit();

                        var strContent = Serializer.ToJson(app_main_enity);
                        PushMessage.PushToUser(ToUser, strContent, PushMsgType.CarSupplementStart.ToString());
                    }
                }
            }
        }

        private void House_NR_Consumer_Listener(MessageBody message)
        {
            if (message != null && !string.IsNullOrWhiteSpace(message.Text))
            {
                var AppCode = message.Text.Trim();

                LogWriter.Info("收到房贷补件/修改资料消息:" + AppCode);

                using (EFRepositoryTransaction efRepository = new EFRepositoryTransaction(GlobalSetting.MainDataBasenameOrConnectionString))
                {
                    var VappMainService = efRepository.GetRepository<V_APPMAIN>();
                    var MsgBoxSerivce = efRepository.GetRepository<APP_MSGBOX>();
                    var globalService = efRepository.GetRepository<APP_GLOBALCONFIG>();
                    var app_nr_content_entity = globalService.FirstOrDefault(c => c.KEY == "HOUSE_APPLICATION_NR_CONTENT");
                    var app_nr_content = "";
                    if (app_nr_content_entity != null)
                    {
                        app_nr_content = app_nr_content_entity.VALUE;
                    }
                    //var IAPP_GLOBALCONFIGSERVICE=efRepository
                    var NR_Entity = VappMainService.FirstOrDefault(o => o.APPCODE == AppCode);
                    if (NR_Entity != null)
                    {
                        var ToUser = NR_Entity.CREATEDUSER;
                        var CustomerName = NR_Entity.CUSTOMERNAME;
                        var Url = string.Format("/LoanApplication/Application?dformCode={0}&operation=4&appid={1}"
                                                , NR_Entity.LOGO
                                                , NR_Entity.APPID);
                        var Content = string.Format(app_nr_content, CustomerName);
                        var app_main_enity = new
                        {
                            CustomerName = NR_Entity.CUSTOMERNAME,
                            AppCode = NR_Entity.APPCODE,
                            Logo = NR_Entity.LOGO,
                            AppId = NR_Entity.APPID,
                            Title = "房贷补件/修改资料通知",
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
                            CATEGORY = PushMsgType.HouseSupplementStart.ToString(),
                            URL = app_main_enity.Url,
                            APPCODE = app_main_enity.AppCode
                        };
                        MsgBoxSerivce.Add(msg);
                        efRepository.Commit();

                        var strContent = Serializer.ToJson(app_main_enity);
                        PushMessage.PushToUser(ToUser, strContent, PushMsgType.CarSupplementStart.ToString());
                    }
                }

            }
        }
        #endregion
    }
}
