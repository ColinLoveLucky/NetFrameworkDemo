using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QK.QAPP.Entity
{
   public class CA_INFO
    {
       /// <summary>
        /// RA_CODE：RA（Registration Authority， 数字证书注册机构）的代码，默认值FDD
       /// </summary>
       public string RA_CODE { get; set; }
       public _CA_USER CA_USER { get; set; }
       public class _CA_USER{
            /// <summary>
            /// 用户姓名
            /// </summary>
          public string USER_NAME{get;set;}
            /// <summary>
          /// 用户身份证号
            /// </summary>
           public string NATIONAL_ID{get;set;}
            /// <summary>
           /// 用户手机号
            /// </summary>
           public string MOBILE_NO { get; set; }
           /// <summary>
           /// 用户邮箱
           /// </summary>
           public string EMAIL_ID { get; set; }
        }

    }
}
