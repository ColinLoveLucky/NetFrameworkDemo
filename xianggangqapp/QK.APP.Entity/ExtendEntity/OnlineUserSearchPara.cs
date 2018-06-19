using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QK.QAPP.Entity
{
   public class OnlineUserSearchPara
    {
       public int PageIndex { get; set; }
       public int PageSize { get; set; }
       public Dictionary<string, string> Sort { get; set; }
       public string ConnectionId { get; set; }
       public string UserName { get; set; }
       public string UserIp { get; set; }
       public string UserBrowser { get; set; }
       public string UserBrowserVersion { get; set; }
       public string MachineName { get; set; }
    }
}
