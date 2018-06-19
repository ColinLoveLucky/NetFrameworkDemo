using System;
using Microsoft.AspNet.SignalR;
using QK.QAPP.Infrastructure;
using QK.QAPP.IServices;

namespace QK.QAPP.SalesTest.Hubs
{
    /// <summary>
    /// 实时消息HUB类
    /// </summary>
    public class SupplementHub : Hub
    {
        private IAPP_MSGBOX_USERSERVICE MsgBoxUserSvr = Ioc.GetService<IAPP_MSGBOX_USERSERVICE>();

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
                System.Diagnostics.Trace.Write(ex, "error");
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
                System.Diagnostics.Trace.Write(ex, "error");
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
                MsgBoxUserSvr.AddConnection(Context.User.Identity.Name, Context.ConnectionId);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.Write(ex, "error");
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
                MsgBoxUserSvr.RemoveConnection(Context.ConnectionId);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.Write(ex, "error");
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
                if (Context.User != null)
                {
                    MsgBoxUserSvr.AddConnection(Context.User.Identity.Name, Context.ConnectionId);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.Write(ex, "error");
            }
            return base.OnReconnected();
        }
    }
}