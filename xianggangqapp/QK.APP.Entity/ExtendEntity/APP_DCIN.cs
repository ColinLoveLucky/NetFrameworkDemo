/* ==============================================================================
 * 功能描述：APP_DCIN  
 * 创 建 者：leiz
 * 创建日期：2015/3/6 17:35:58
 * ==============================================================================*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QK.QAPP.Entity
{
    public partial class APP_DCIN
    {
        /// <summary>
        /// 主键
        /// </summary>		
        public decimal ID { get; set; }
        /// <summary>
        /// 申请信息外键
        /// </summary>		
        public decimal APP_ID { get; set; }
        /// <summary>
        /// 当前是第几次访问
        /// </summary>		
        public decimal DCIN_NO { get; set; }
        /// <summary>
        /// 系统初次决策
        /// </summary>		
        public string DCIN_FIRST_ES_DECISION { get; set; }
        /// <summary>
        /// 系统上次决策
        /// </summary>		
        public string DCIN_LATEST_ES_DECISION { get; set; }
        /// <summary>
        /// 初审决策人员
        /// </summary>		
        public string DCIN_DECISION_NO_FIRST { get; set; }
        /// <summary>
        /// 决策代码
        /// </summary>		
        public string DCIN_DECISION_CODE { get; set; }
        /// <summary>
        /// 决策描述（*）
        /// </summary>		
        public string DCIN_DECISION_MEMO { get; set; }
        /// <summary>
        /// 决策描述反馈销售（*）
        /// </summary>		
        public string DCIN_DECISION_MEMO_2SALE { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>		
        public DateTime CHANGED_TIME { get; set; }
        /// <summary>
        /// 修改者
        /// </summary>		
        public string CHANGED_USER { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>		
        public DateTime CREATED_TIME { get; set; }
        /// <summary>
        /// 创建者
        /// </summary>		
        public string CREATED_USER { get; set; }
        /// <summary>
        /// 终审总监决策
        /// </summary>		
        public string DCIN_DECISION_SUPER { get; set; }
        /// <summary>
        /// 初审初级决策
        /// </summary>		
        public string DCIN_DECISION1_JUNIOR { get; set; }
        /// <summary>
        /// 初审高级决策
        /// </summary>		
        public string DCIN_DECISION1_SENIOR { get; set; }
        /// <summary>
        /// 终审初级决策
        /// </summary>		
        public string DCIN_DECISION2_JUNIOR { get; set; }
        /// <summary>
        /// 终审高级决策
        /// </summary>		
        public string DCIN_DECISION2_SENIOR { get; set; }
        /// <summary>
        /// 决策人员姓名
        /// </summary>		
        public string DCIN_DECISION_NAME { get; set; }
        /// <summary>
        /// 不同于系统推荐原因
        /// </summary>		
        public string DCIN_DIFF_ES_MEMO { get; set; }
        /// <summary>
        /// 当前人员的决策
        /// </summary>		
        public string DCIN_MANUAL_DECISION { get; set; }
        /// <summary>
        /// DCIN_ACTION
        /// </summary>		
        public string DCIN_ACTION { get; set; }
        /// <summary>
        /// 黑名单
        /// </summary>		
        public string DCIN_BL_DECISION { get; set; }
        /// <summary>
        /// 重复进件
        /// </summary>		
        public string DCIN_RP_DECISION { get; set; }
        /// <summary>
        /// 查重
        /// </summary>		
        public string DCIN_CR_DECISION { get; set; }
        /// <summary>
        /// lg逻辑信息
        /// </summary>		
        public string DCIN_LG_DECISION { get; set; }
        /// <summary>
        /// ol进件大纲
        /// </summary>		
        public string DCIN_OL_DECISION { get; set; }
        /// <summary>
        /// 终审决策人员
        /// </summary>		
        public string DCIN_DECISION_NO_FINAL { get; set; }
        /// <summary>
        /// 反欺诈人员
        /// </summary>		
        public string FRAUD_NO { get; set; }
        /// <summary>
        /// 终审决策
        /// </summary>		
        public string DCIN_FDECISION_MEMO { get; set; }
        /// <summary>
        /// 终审决策
        /// </summary>		
        public string DCIN_FDECISION_MEMO_2SALE { get; set; }
        /// <summary>
        /// 审批人员姓名
        /// </summary>		
        public string APPROVE_NAME { get; set; }
        /// <summary>
        /// 反欺诈人员姓名
        /// </summary>		
        public string FRAUD_NAME { get; set; }
        /// <summary>
        /// 拒贷码信息
        /// </summary>		
        public string DCIN_REFUSE_CODE { get; set; }
        /// <summary>
        /// 规则码信息
        /// </summary>		
        public string DCIN_RULE_CODE { get; set; }
        /// <summary>
        /// 原产品类型
        /// </summary>		
        public string DCIN_OLD_LOGO { get; set; }
    }
}
