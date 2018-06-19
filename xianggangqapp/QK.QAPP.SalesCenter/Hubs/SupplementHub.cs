using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Autofac;
using Microsoft.AspNet.SignalR;
using QK.QAPP.Global;
using QK.QAPP.Infrastructure.Cache;
using QK.QAPP.Infrastructure.Data.EFRepository.Repository;
using QK.QAPP.Infrastructure.Log4Net;
using QK.QAPP.IServices;
using QK.QAPP.Entity;
using Microsoft.Practices.Unity;
using QK.QAPP.Infrastructure;
using System.Threading;

namespace QK.QAPP.SalesCenter.Hubs
{
    /// <summary>
    /// 实时消息HUB类
    /// </summary>
    public class SupplementHub : BaseHub
    {

        //IAPP_MSGBOX_USERSERVICE msgBoxUserSvr;
        public SupplementHub(ILifetimeScope lifetimeScope)
            : base(lifetimeScope)
        {
            //msgBoxUserSvr = _hubLifetimeScope.Resolve<IAPP_MSGBOX_USERSERVICE>();
        }

        /// <summary>
        /// 推送消息到客户端
        /// </summary>
        /// <param name="user">用户</param>
        /// <param name="msg">消息</param>
        public void SendMsg(string user, string msg, string categroy)
        {
            try
            {
                PushMessage.Push(user, msg, categroy);
            }
            catch (Exception ex)
            {
                //System.Diagnostics.Trace.Write(ex, "error");
                LogWriter.Warn("消息发送异常（SendMsg）", ex);
            }
        }

        /// <summary>
        /// 推送消息给所有人
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="category"></param>
        public void SendMsgAll(string user, string msg, string category)
        {
            try
            {
                PushMessage.PushToUser(user, msg, category);
            }
            catch (Exception ex)
            {
                //System.Diagnostics.Trace.Write(ex, "error");
                LogWriter.Warn("消息发送异常（SendMsgAll）", ex);
            }
        }

        /// <summary>
        /// 连接后处理
        /// </summary>
        /// <returns></returns>
        public override System.Threading.Tasks.Task OnConnected()
        {
            try
            {
                if (Context.User != null && !string.IsNullOrWhiteSpace(Context.ConnectionId))
                {
                    //不再使用Ioc注入；修改日期20150206
                    using (EFRepositoryTransaction efRepository = new EFRepositoryTransaction(GlobalSetting.MainDataBasenameOrConnectionString))
                    {
                        var msgBoxUserSvr = efRepository.GetRepository<APP_MSGBOX_USER>();
                        var helper = new AppMessageUserHelper(msgBoxUserSvr);

                        helper.AddConnection(Context.User.Identity.Name, Context.ConnectionId);
                        //msgBoxUserSvr.UnitOfWork.SaveChanges();  在上面方法里面已经有了SAVECHANGE
                    }
                }
            }
            catch (Exception ex)
            {
                //System.Diagnostics.Trace.Write(ex, "error");
                LogWriter.Error("消息发送异常（OnConnected）", ex);
            }
            return base.OnConnected();
        }

        /// <summary>
        /// 失去连接后处理
        /// </summary>
        /// <returns></returns>
        public override System.Threading.Tasks.Task OnDisconnected(bool stopCalled)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(Context.ConnectionId))
                {
                    //不再使用Ioc注入；修改日期20150206
                    using (EFRepositoryTransaction efRepository = new EFRepositoryTransaction(GlobalSetting.MainDataBasenameOrConnectionString))
                    {
                        var msgBoxUserSvr = efRepository.GetRepository<APP_MSGBOX_USER>();
                        var helper = new AppMessageUserHelper(msgBoxUserSvr);
                        helper.RemoveConnection(Context.ConnectionId);
                        //msgBoxUserSvr.UnitOfWork.SaveChanges();  在上面方法里面已经有了SAVECHANGE
                    }
                }
            }
            catch (Exception ex)
            {
                //System.Diagnostics.Trace.Write(ex, "error");
                LogWriter.Warn("消息发送异常（OnDisconnected）", ex);
            }
            return base.OnDisconnected(stopCalled);
        }

        /// <summary>
        /// 断开后重新连接
        /// </summary>
        /// <returns></returns>
        public override System.Threading.Tasks.Task OnReconnected()
        {
            try
            {
                if (Context != null && Context.User != null)
                {
                    if (Context.User.Identity != null)
                    {
                        using (EFRepositoryTransaction efRepository = new EFRepositoryTransaction(GlobalSetting.MainDataBasenameOrConnectionString))
                        {
                            var msgBoxUserSvr = efRepository.GetRepository<APP_MSGBOX_USER>();
                            var helper = new AppMessageUserHelper(msgBoxUserSvr);
                            helper.AddConnection(Context.User.Identity.Name, Context.ConnectionId);
                            //msgBoxUserSvr.UnitOfWork.SaveChanges();  在上面方法里面已经有了SAVECHANGE
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                //System.Diagnostics.Trace.Write(ex, "error");
                LogWriter.Warn("消息发送异常（OnReconnected）", ex);
            }
            return base.OnReconnected();
        }


    }


    public class AppMessageUserHelper
    {
        public IRepositoryBase<APP_MSGBOX_USER> Repository { get; set; }

        //SESSION_USER
        private string SessionUser
        {
            get
            {
                return Global.GlobalSetting.AuthSaveKey;
            }
        }

        public AppMessageUserHelper(IRepositoryBase<APP_MSGBOX_USER> repository)
        {
            this.Repository = repository;
        }

        public void AddConnection(string UserName, string ConnectionId)
        {
            if (HttpContext.Current == null)
            {
                return;
            }
            var rq = HttpContext.Current.Request;
            var clientMachineIp = "0.0.0.0";    //客户端IP
            var userName = "unknown user";      //用户姓名
            var userId = "unknown ID";          //用户账户
            var clientMachineName = "unknown machine";  //客户端机器名

            try
            {
                //计算客户端IP
                if (rq.ServerVariables["HTTP_X_FORWARDED_FOR"] != null)
                {
                    clientMachineIp = rq.ServerVariables["HTTP_X_FORWARDED_FOR"].ToString().Split(',')[0].Split(':')[0].Trim();
                }
                else if (rq.ServerVariables["REMOTE_ADDR"] != null)
                {
                    clientMachineIp = rq.ServerVariables["REMOTE_ADDR"].ToString();
                }
                else
                {
                    clientMachineIp = rq.UserHostAddress;
                }

                //计算客户端机器名
                clientMachineName = "unknown machine";
                clientMachineName = clientMachineIp;

                //计算用户姓名和用户ID
                userName = "unknown user";
                userId = clientMachineName;
                try
                {
                    var user = HttpContext.Current.Session[SessionUser] as QFUser;
                    if (user != null)
                    {
                        userName = user.RealName;
                        userId = user.Account;
                    }
                }
                catch { }
            }
            catch
            {
                userName = "未知用户姓名";
                userId = "未知用户账户";
                clientMachineIp = "未知客户端IP";
            }

            if (!string.IsNullOrWhiteSpace(UserName))
            {
                var Check = Repository.FirstOrDefault(o => o.USERNAME == UserName);
                if (Check != null)
                {
                    Check.CONNECTIONID = ConnectionId;
                    Check.LASTUPDATETIME = DateTime.Now;
                    Check.USERIP = clientMachineIp;
                    Check.USERBROWSER = rq.UserAgent;
                    Check.USERBROWSERVERSION = rq.Browser.Version;
                    Check.MACHINENAME = clientMachineName;
                    Check.ENABLE = 1;
                    Repository.Update(Check);
                    Repository.UnitOfWork.SaveChanges();
                }
                else
                {
                    APP_MSGBOX_USER CurItem = new APP_MSGBOX_USER()
                    {
                        USERNAME = UserName,
                        CONNECTIONID = ConnectionId,
                        CREATETIME = DateTime.Now,
                        USERIP = clientMachineIp,
                        USERBROWSER = rq.UserAgent,
                        USERBROWSERVERSION = rq.Browser.Version,
                        MACHINENAME = clientMachineName,
                        ENABLE = 1
                    };
                    Repository.Add(CurItem);
                    Repository.UnitOfWork.SaveChanges();
                }
            }
        }

        public void RemoveConnection(string ConnectionId)
        {
            var item = Repository.FirstOrDefault(o => o.CONNECTIONID == ConnectionId);
            if (item != null)
            {
                item.ENABLE = 0;
                Repository.Update(item);
                //this.Delete(item);
                Repository.UnitOfWork.SaveChanges();
            }
        }

    }
}