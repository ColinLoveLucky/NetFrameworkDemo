using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QK.QAPP.Entity
{
   public class ContractCreateResponse
    {
       /// <summary>
        /// SUCCESS / ERROR
        /// 是否必须:Y
       /// </summary>
       public string RESULT { get; set;}
       /// <summary>
       /// 10001 – 参数不正确
      ///10002 – 报文解析出错
       ///是否必须:N
       /// </summary>
       public string CODE { get; set; }
       /// <summary>
       /// 文件下载地址，RESULT= "SUCCESS "时必须
       /// 是否必须:N
       /// </summary>
       public string DOWNLOAD_URL { get; set; }
       /// <summary>
       /// 文件查看地址，RESULT= "SUCCESS "时必须
       /// 是否必须:N
       /// </summary>
       public string VIEW_URL { get; set; }
    }
}
