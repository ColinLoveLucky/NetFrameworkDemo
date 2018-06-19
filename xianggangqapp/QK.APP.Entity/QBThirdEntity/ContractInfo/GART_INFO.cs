using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QK.QAPP.Entity
{
    /// <summary>
    /// 担保保证人信息
    /// </summary>
    public class GART_INFO
    {
        /// <summary>
        /// 保证人名字
        /// </summary>
        public string USER_NAME { get; set; }

        /// <summary>
        /// 保证人证件类型
        /// </summary>
        public string CERT_TYPE { get; set; }
        /// <summary>
        /// 保证人证件号码
        /// </summary>
        public string CERT_NO { get; set; }
        /// <summary>
        /// 保证人邮箱
        /// </summary>
        public string USER_MAIL { get; set; }
       
        /// <summary>
        /// 保证人(甲方)名称
        /// </summary>
        public string ORG_CONT { get; set; }
        /// <summary>
        /// 保证人(甲方)地址
        /// </summary>
        public string USER_ADDR { get; set; }
        /// <summary>
        /// 保证人(甲方)法人代表
        /// </summary>
        public string REPR_NAME { get; set; }
        /// <summary>
        /// 保证人(甲方)联系电话
        /// </summary>
        public string RESV_MOBL { get; set; }
        /// <summary>
        /// 保证人(甲方)传真
        /// </summary>
        public string FAX { get; set; }
        /// <summary>
        /// 保证人(甲方)邮编
        /// </summary>
        public string ZIDCODE { get; set; }

    }
}
