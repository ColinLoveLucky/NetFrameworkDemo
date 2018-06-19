/***************************************************************************
 * 描述：借款人信息 
 * 创建/修改时间：20160310
 * 创建/修改人：net team
 ***************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QK.QAPP.Entity
{
    public class BOWR_INFO
    {
        /// <summary>
        /// 姓名
        /// </summary>
        public string USER_NAME { get; set; }
        /// <summary>
        /// 借款人夸客平台账号
        /// </summary>
        public string QF_USERID { get; set; }
        /// <summary>
        /// 证件类型
        /// </summary>
        public string CERT_TYPE { get; set; }
        /// <summary>
        /// 证件号码
        /// </summary>
        public string CERT_NO { get; set; }
        /// <summary>
        /// 银行预留手机号/移动电话
        /// </summary>
        public string RESV_MOBL { get; set; }
        /// <summary>
        /// 固定电话
        /// </summary>
        public string FIX_MOBL { get; set; }
        /// <summary>
        /// 通讯地址
        /// </summary>
        public string USER_ADDR { get; set; }
        /// <summary>
        /// 户籍地址
        /// </summary>
        public string USER_HOU_ADDR { get; set; }
        /// <summary>
        /// 邮箱
        /// </summary>
        public string USER_MAIL { get; set; }
        /// <summary>
        /// 户名=用户姓名
        /// </summary>
        public string BAND_DEPT_NAME { get; set; }
        /// <summary>
        /// 银行账号
        /// </summary>
        public string BAND_DEPT_ACNT { get; set; }
        /// <summary>
        /// 开户行
        /// </summary>
        public string BAND_DEPT { get; set; }
        /// <summary>
        /// 开户行支行
        /// </summary>
        public string BAND_DEPT_SUB { get; set; }
        /// <summary>
        /// 传真
        /// </summary>
        public string FAX { get; set; }
        /// <summary>
        /// 邮编
        /// </summary>
        public string ZIDCODE { get; set; }
        /// <summary>
        /// 借款人房产地址
        /// </summary>
        public string FADDR { get; set; }
        /// <summary>
        /// 产权号
        /// </summary>
        public string FPR { get; set; }
        /// <summary>
        /// 房屋面积
        /// </summary>
        public string FAREA { get; set; }
    }
}
