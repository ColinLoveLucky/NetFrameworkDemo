/***********************
 * 作    者：刘云松
 * 创建时间：‎‎2015-3-20 14:22:07
 * 作    用：评估状态枚举
*****************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QK.QAPP.Entity
{
    public enum AssessStatusType
    {
        /// <summary>
        /// 已批复
        /// </summary>
        CarAssessApplyApproved,

        /// <summary>
        /// 待评估
        /// </summary>
        CarAssessToBeAssess,

        /// <summary>
        /// 评估中
        /// </summary>
        CarAssessAssessing,

        /// <summary>
        /// 客户拒绝
        /// </summary>
        CarAssessCustomerReject,

        /// <summary>
        /// 客户犹豫
        /// </summary>
        CarAssessCustomerHesitate,

        /// <summary>
        /// 评估已提交
        /// </summary>
        CarAssessSubmitted,

        /// <summary>
        /// GPS未安装
        /// </summary>
        CarAssessGPS,

        /// <summary>
        /// GPS已安装
        /// </summary>
        CarAssessGPSed
    }
}
