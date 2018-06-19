using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QK.QAPP.Entity
{
   public  class BidStatusQueryType
    {
        /// <summary>
        /// 未发标、未挂标状态
        /// </summary>
       public static readonly string Bid_Step_NotSendBid = "Bid_Step_NotSendBid";
        /// <summary>
        /// 在凑标、已挂标状态
        /// </summary>
       public static readonly string Bid_Step_CollectBid = "Bid_Step_CollectBid";
        /// <summary>
        /// 已满标状态
        /// </summary>
       public static readonly string Bid_Step_FullBid = "Bid_Step_FullBid";
        /// <summary>
        /// 已流标状态
        /// </summary>
       public static readonly string Bid_Step_FailBid = "Bid_Step_FailBid";
       /// <summary>
       /// 划标情况
       /// </summary>
       public static readonly string Bid_MarkingState = "Bid_MarkingState";
       /// <summary>
       /// 中航审核结果
       /// </summary>
       public static readonly string Bid_ZH_MarkingState = "Bid_ZH_MarkingState";
       /// <summary>
       /// 协议状态
       /// </summary>
       public static readonly string Bid_AgreementState = "Bid_AgreementState";
       /// <summary>
       /// 中航协议审核结果
       /// </summary>
       public static readonly string Bid_ZH_AgreementState = "Bid_ZH_AgreementState";
       /// <summary>
       /// 协议已上传
       /// </summary>
       public static readonly string BS_XY_YSC = "BS_XY_YSC";
       /// <summary>
       /// 协议已确认
       /// </summary>
       public static readonly string BS_XY_YQR = "BS_XY_YQR";
       /// <summary>
       /// 协议未上传
       /// </summary>
       public static readonly string BS_XY_WSC = "BS_XY_WSC";
       /// <summary>
       /// 协议补录
       /// </summary>
       public static readonly string Bid_Step_Addtion = "Bid_Step_Addtion";
    }
}
