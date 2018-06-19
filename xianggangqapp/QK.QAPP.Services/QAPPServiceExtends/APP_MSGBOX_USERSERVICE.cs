using QK.QAPP.Entity;
using QK.QAPP.Infrastructure.Data.EFRepository.Repository;
using QK.QAPP.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QK.QAPP.Services
{
    public partial class APP_MSGBOX_USERSERVICE : RepositoryBase<APP_MSGBOX_USER>, IAPP_MSGBOX_USERSERVICE
    {
        public void AddConnection(string UserName, string ConnectionId)
        {
            var rq = System.Web.HttpContext.Current.Request;
            var hostName = rq.UserHostAddress; ;
             

            if (!string.IsNullOrWhiteSpace(UserName))
            {
                var Check = this.FirstOrDefault(o => o.USERNAME == UserName);
                if (Check != null)
                {
                    Check.CONNECTIONID = ConnectionId;
                    Check.LASTUPDATETIME = DateTime.Now;
                    Check.USERBROWSER = rq.UserAgent;
                    Check.USERBROWSERVERSION = rq.Browser.Version;
                    Check.MACHINENAME = hostName;
                    Check.ENABLE = 1;
                    this.Update(Check);
                    this.UnitOfWork.SaveChanges();
                }
                else
                {
                    APP_MSGBOX_USER CurItem = new APP_MSGBOX_USER()
                    {
                        USERNAME = (UserName+"").ToLower(),
                        CONNECTIONID = ConnectionId,
                        CREATETIME = DateTime.Now,
                        USERIP = rq.UserHostAddress,
                        USERBROWSER = rq.UserAgent,
                        USERBROWSERVERSION = rq.Browser.Version,
                        MACHINENAME = hostName,
                        ENABLE = 1
                    };
                    this.Add(CurItem);
                    this.UnitOfWork.SaveChanges();
                }
            }
        }

        public void RemoveConnection(string ConnectionId)
        {
            var item = this.FirstOrDefault(o => o.CONNECTIONID == ConnectionId);
            if (item != null)
            {
                item.ENABLE = 0;
                this.Update(item);
                //this.Delete(item);
                this.UnitOfWork.SaveChanges();
            }
        }
    }
}
