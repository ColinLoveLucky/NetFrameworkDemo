/**********************************************************************
 * 时间：20160219
 * 创建人：leizhao
 * 描述：封装额度信息总览
 **********************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QK.QAPP.Entity
{
    public class QB_AMT_INFO
    {
        #region 一、 个人金融部额度信息总览
        #region 01 P2P额度
        /// <summary>
        /// 直投挂标总额度
        /// </summary>
        public decimal GJ_ZT_TOTAL_AMT { get; set; }
        /// <summary>
        /// 直投挂标可用额度
        /// </summary>
        public decimal GJ_ZT_TOTAL_AMT_USABLE { get; set; }

        /// <summary>
        /// 理财挂标总额度
        /// </summary>
        public decimal GJ_LC_TOTAL_AMT { get; set; }
        /// <summary>
        /// 理财挂标可用额度
        /// </summary>
        public decimal GJ_LC_TOTAL_AMT_USABLE { get; set; }
        #endregion

        #region 02 T2P额度
        /// <summary>
        /// T2P外贸抵押车贷额度
        /// </summary>
        public decimal GJ_T2P_WM_DYCD_AMT { get; set; }
        /// <summary>
        /// T2P外贸抵押车贷可用额度
        /// </summary>
        public decimal GJ_T2P_WM_DYCD_AMT_USABLE { get; set; }

        /// <summary>
        /// T2P中航抵押车贷额度
        /// </summary>
        public decimal GJ_T2P_ZH_DYCD_AMT { get; set; }
        /// <summary>
        /// T2P中航抵押车贷可用额度
        /// </summary>
        public decimal GJ_T2P_ZH_DYCD_AMT_USABLE { get; set; }
        /// <summary>
        /// T2P外贸其他额度
        /// </summary>
        public decimal GJ_T2P_WM_OTHER_AMT { get; set; }
        /// <summary>
        /// T2P外贸其他可用额度
        /// </summary>
        public decimal GJ_T2P_WM_OTHER_AMT_USABLE { get; set; }
        /// <summary>
        /// T2P中航其他额度
        /// </summary>
        public decimal GJ_T2P_ZH_OTHER_AMT { get; set; }
        /// <summary>
        /// T2P中航其他可用额度
        /// </summary>
        public decimal GJ_T2P_ZH_OTHER_AMT_USABLE { get; set; }

        #endregion
        #endregion

        #region 二、房贷管理部额度信息总览
        #region 01 P2P额度
        /// <summary>
        /// 理财挂标总额度
        /// </summary>
        public decimal FD_LC_TOTAL_AMT { get; set; }
        /// <summary>
        /// 理财挂标可用额度
        /// </summary>
        public decimal FD_LC_TOTAL_AMT_USABLE { get; set; }
        #endregion

        #region 02 T2P额度
        /// <summary>
        /// T2P外贸房贷额度
        /// </summary>
        public decimal FD_T2P_WM_FD_AMT { get; set; }
        /// <summary>
        /// T2P外贸房贷可用额度
        /// </summary>
        public decimal FD_T2P_WM_FD_AMT_USABLE { get; set; }
        /// <summary>
        /// T2P中航房贷额度
        /// </summary>
        public decimal FD_T2P_ZH_FD_AMT { get; set; }
        /// <summary>
        /// T2P中航房贷可用额度
        /// </summary>
        public decimal FD_T2P_ZH_FD_AMT_USABLE { get; set; }
        #endregion
        #endregion

        #region 三、创新业务部额度信息总览
        #region 01 P2P额度
        /// <summary>
        /// 理财挂标总额度
        /// </summary>
        public decimal CX_LC_TOTAL_AMT { get; set; }
        /// <summary>
        /// 理财挂标可用额度
        /// </summary>
        public decimal CX_LC_TOTAL_AMT_USABLE { get; set; }
        #endregion

        #region 02 消费信贷额度
        /// <summary>
        /// 消费信贷放款额度
        /// </summary>
        public decimal CX_P2P_XFXD_XFXDFK_AMT { get; set; }
        /// <summary>
        /// 消费信贷放款可用额度
        /// </summary>
        public decimal CX_P2P_XFXD_XFXDFK_AMT_USABLE { get; set; }
        #endregion
        #endregion
    }
}
