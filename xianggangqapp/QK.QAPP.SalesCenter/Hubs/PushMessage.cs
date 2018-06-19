using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using QK.QAPP.Entity;
using QK.QAPP.Global;
using QK.QAPP.Infrastructure.Data.EFRepository.Repository;
using QK.QAPP.Infrastructure.Log4Net;
using QK.QAPP.IServices;
using QK.QAPP.Infrastructure;

namespace QK.QAPP.SalesCenter.Hubs
{
    public class PushMessage
    {
        //private static IAPP_MSGBOX_USERSERVICE MsgBoxSvr = Ioc.GetService<IAPP_MSGBOX_USERSERVICE>();

        /// <summary>
        /// 消息推送
        /// </summary>
        /// <param name="User">用户</param>
        /// <param name="Msg">消息内容</param>
        /// <param name="Category">消息类型</param>
        public static void Push(string User, string Msg, string Category)
        {
            IHubContext hubContext = GlobalHost.ConnectionManager.GetHubContext<SupplementHub>();

            //不再使用Ioc注入；修改日期20150206
            using (EFRepositoryTransaction efRepository = new EFRepositoryTransaction(GlobalSetting.MainDataBasenameOrConnectionString))
            {
                var MsgBoxSvr = efRepository.GetRepository<APP_MSGBOX_USER>();
                if (!string.IsNullOrWhiteSpace(User))
                {
                    var FindConnection = MsgBoxSvr.Find(o => o.USERNAME == User && o.ENABLE == 1);
                    if (FindConnection != null && FindConnection.Count() > 0)
                    {
                        List<string> UserLists = new List<string>();
                        foreach (var item in FindConnection)
                        {
                            UserLists.Add(item.CONNECTIONID);
                        }
                        hubContext.Clients.Clients(UserLists).ToWeb(User, Msg, Category);
                    }
                }
            }
        }

        /// <summary>
        /// 发送消息给所有人
        /// </summary>
        /// <param name="Msg"></param>
        /// <param name="Category"></param>
        public static void PushToUser(string User, string Msg, string Category)
        {
            try
            {
                IHubContext hubContext =
                    GlobalHost.ConnectionManager.GetHubContext<SupplementHub>();
                hubContext.Clients.All.ToWebAll(User, Msg, Category);
            }
            catch (Exception ex)
            {
                LogWriter.Error("消息发送异常（PushToUser）", ex);
            }
        }

    }
}