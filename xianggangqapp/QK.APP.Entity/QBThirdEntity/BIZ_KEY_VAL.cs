using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QK.QAPP.Entity
{
    /// <summary>
    /// 业务和合同模板的关联关系。
    ///接入系统需要提前和合同管理平台约定“业务和合同模板的关联关系”
    /// </summary>
   public class BIZ_KEY_VAL
    {
       public string BIZ_KEY { get; set; }
       public string BIZ_VAL { get; set; }
    }
}
