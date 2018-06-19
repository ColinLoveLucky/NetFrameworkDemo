using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNet.SignalR;
using QK.QAPP.Infrastructure;
using QK.QAPP.IServices;

namespace QK.QAPP.SalesTest.Hubs
{
    public class PushMessage
    {
        private static IAPP_MSGBOX_USERSERVICE MsgBoxSvr = Ioc.GetService<IAPP_MSGBOX_USERSERVICE>();

        /// <summary>
        /// 消息推送
        /// </summary>
        /// <param name="User">用户</param>
        /// <param name="Msg">消息内容</param>
        /// <param name="Category">消息类型</param>
        public static void Push(string User, string Msg,string Category)
        {
            IHubContext hubContext = 
                GlobalHost.ConnectionManager.GetHubContext<SupplementHub>();
            if (!string.IsNullOrWhiteSpace(User))
            {
                var FindConnection = MsgBoxSvr.Find(o => o.USERNAME == User && o.ENABLE==1);
                if (FindConnection != null && FindConnection.Count()>0)
                {
                    List<string> UserLists = new List<string>();
                    foreach (var item in FindConnection)
                    {
                        UserLists.Add(item.CONNECTIONID);
                    }
                    hubContext.Clients.Clients(UserLists).ToWeb(User,Msg,Category);
                }
            }       
        }

        /// <summary>
        /// 发送消息给所有人
        /// </summary>
        /// <param name="Msg"></param>
        /// <param name="Category"></param>
        public static void PushToUser(string User,string Msg, string Category)
        {
            IHubContext hubContext =
                GlobalHost.ConnectionManager.GetHubContext<SupplementHub>();            
            hubContext.Clients.All.ToWebAll(User,Msg,Category);            
        }

    }
}