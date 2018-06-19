using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QK.QAPP.Entity
{
    /// <summary>
    /// 信托机构信息
    /// </summary>
    public class TC_INFO
    {
        /// <summary>
        /// 机构代码
        /// </summary>
        public string ORG_CODE { get; set; }
        /// <summary>
        /// 中文机构全称
        /// </summary>
        public string NAME_ZH { get; set; }
        /// <summary>
        /// 英文机构全称
        /// </summary>
        public string NAME_EN { get; set; }
        /// <summary>
        /// 中文机构简称
        /// </summary>
        public string ABBR_ZH { get; set; }
        /// <summary>
        /// 英文机构简称
        /// </summary>
        public string ABBR_EN { get; set; }
        /// <summary>
        /// 机构类型ID
        /// </summary>
        public string ORG_TYPE { get; set; }
        /// <summary>
        /// 联系人
        /// </summary>
        public string ORG_CONT { get; set; }
        /// <summary>
        /// 法人代表
        /// </summary>
        public string REPR_NAME { get; set; }
        /// <summary>
        /// 公司注册地址
        /// </summary>
        public string REGT_PLAC { get; set; }
        /// <summary>
        /// 公司联系地址
        /// </summary>
        public string ORG_ADDR { get; set; }
        /// <summary>
        /// 公司联系电话
        /// </summary>
        public string CONT_NO { get; set; }
        /// <summary>
        /// 开户行
        /// </summary>
        public string BAND_DEPT { get; set; }
        /// <summary>
        /// 开户名称
        /// </summary>
        public string BAND_DEPT_NAME { get; set; }
        /// <summary>
        /// 账号
        /// </summary>
        public string BAND_DEPT_ACNT { get; set; }
        /// <summary>
        /// 银行预留手机号
        /// </summary>
        public string RESV_MOBL { get; set; }
        /// <summary>
        /// 是否生效
        /// </summary>
        public string IS_ACTV { get; set; }
        /// <summary>
        /// 生效日
        /// </summary>
        public string ACTV_DATE { get; set; }

    }
}
