using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QK.QAPP.Entity
{
   public class BidOperateRequest
    {
       /// <summary>
        /// 发标编号，多个之间用|分割
       /// </summary>
        public string BidList { get; set; }
       /// <summary>
       /// 用户
       /// </summary>
        public string UserCode { get; set; }
        /// <summary>
        /// 用户名字
        /// </summary>
        public string UserName { get; set; }
       /// <summary>
       /// 用户列表，多个之间用|分割
       /// </summary>
        public string SubUserCodeList { get; set; }
       /// <summary>
       /// 驳回原因code
       /// </summary>
        public string RejectCode { get; set; }
       /// <summary>
       /// 驳回原因
       /// </summary>
        public string RejectRemark { get; set; }
    }
}
