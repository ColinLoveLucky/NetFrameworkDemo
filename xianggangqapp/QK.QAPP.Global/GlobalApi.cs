/***********************
 * 作    者：.net team
 * 创建时间：‎20160105
 * 作    用：统一配置标的额度相关接口
*****************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QK.QAPP.Global
{
    public class GlobalApi
    {
        /// <summary>
        /// 标的额度接口服务地址
        /// </summary>
        public static string QBApiHost = GlobalSetting.QBApiHost;
        
        #region 01 额度接口相关
        /// <summary>
        /// 获取额度列表接口
        /// </summary>
        public static string GetAmtList
        {
            get
            {
                return QBApiHost + "/api/QKAmtLimit/GetAmtList";
            }
        }
        /// <summary>
        /// 获取单条额度信息接口
        /// </summary>
        public static string GetAmtListById
        {
            get
            {
                return QBApiHost + "/api/QKAmtLimit/Get";
            }
        }
        /// <summary>
        /// 额度新增接口
        /// </summary>
        public static string QuotaAdd
        {
            get
            {
                return QBApiHost + "/api/QKAmtLimit/Post";
            }
        }
        /// <summary>
        /// 额度批量复核接口
        /// </summary>
        public static string MultiReCheck 
        {
            get
            {
                return QBApiHost + "/api/QKAmtLimit/MultiReCheck ";
            }
        }
        /// <summary>
        /// 额度类型（包含额度属性）接口
        /// </summary>
        public static string QKAmtLimitAttr
        {
            get
            {
                return QBApiHost + "/api/QKAmtLimitAttr/GetAmtAttribute";
            }
        }
        /// <summary>
        /// 额度类型属性获取接口
        /// </summary>
        public static string QKAmtLimitAttrConfig
        {
            get
            {
                return QBApiHost + "/api/QKAmtLimitAttr/Get";
            }
        }
        /// <summary>
        /// 额度类型属性新增接口
        /// </summary>
        public static string QKAmtLimitAttrAdd
        {
            get
            {
                return QBApiHost + "/api/QKAmtLimitAttr/Post";
            }
        }
        /// <summary>
        /// 额度类型属性更新接口
        /// </summary>
        public static string QKAmtLimitAttrUpdate
        {
            get
            {
                return QBApiHost + "/api/QKAmtLimitAttr/Put";
            }
        }

        /// <summary>
        /// 额度类型属性删除接口
        /// </summary>
        public static string QKAmtLimitAttrDelete
        {
            get
            {
                return QBApiHost + "/api/QKAmtLimitAttr/Delete";
            }
        }
        /// <summary>
        /// 额度修改接口
        /// </summary>
        public static string QuotaModify
        {
            get
            {
                return QBApiHost + "/api/QKAmtLimit/Put";
            }
        }
        /// <summary>
        /// 额度调整接口
        /// </summary>
        public static string QuotaAdjust
        {
            get
            {
                return QBApiHost + "/api/QKAmtLimit/Adjust";
            }
        }

        /// <summary>
        /// 额度删除接口
        /// </summary>
        public static string QuotaDelete
        {
            get
            {
                return QBApiHost + "/api/QKAmtLimit/Delete";
            }
        }

        /// <summary>
        /// 获取额度历史接口
        /// </summary>
        public static string GetAmtOPHistory
        {
            get
            {
                return QBApiHost + "/api/QKAmtLimit/GetAmtOPHistory";
            }
        }
        /// <summary>
        /// 额度复核
        /// </summary>
        public static string QuotaCheck
        {
            get
            {
                return QBApiHost + "/api/QKAmtLimit/ReCheck";
            }
        }

        /// <summary>
        /// 获取募集计划列表
        /// </summary>
        public static string GetRaisePlanList
        {
            get
            {
                return QBApiHost + "/api/QKRaisePlan/GetRaisePlanList";
            }
        }
        /// <summary>
        /// 获取单条募集计划  
        /// </summary>
        public static string GetRaisePlanById
        {
            get
            {
                return QBApiHost + "/api/QKRaisePlan/Get";
            }
        }

        /// <summary>
        /// 获取募集计划历史
        /// </summary>
        public static string GetRaisePlanHistory
        {
            get
            {
                return QBApiHost + "/api/QKRaisePlan/GetRaisePlanHis";
            }
        }

        /// <summary>
        /// 额度分配->获取全国剩余可用额度
        /// </summary>
        public static string GetGlobalAvailableAmt
        {
            get
            {
                return QBApiHost + "/api/QKAmtLimitAssign/GetGlobalAvailableAmt";
            }
        }

        /// <summary>
        /// 额度分配->获取额度分配列表
        /// </summary>
        public static string GetQuotaAssignList
        {
            get {
                return QBApiHost + "/api/QKAmtLimitAssign/GetAmtAssignList";
            }
        }
        /// <summary>
        /// 额度分配->获取单条额度分配信息
        /// </summary>
        public static string GetQuotaAssignById
        {
            get {
                return QBApiHost + "/api/QKAmtLimitAssign/Get";
            }
        }
        /// <summary>
        /// 额度分配->新增额度分配
        /// </summary>
        public static string AddQuotaAssign
        {
            get
            {
                return QBApiHost + "/api/QKAmtLimitAssign/Post";
            }
        }
        /// <summary>
        /// 额度分配->调整额度分配
        /// </summary>
        public static string AdjustQuotaAssign
        {
            get {
                return QBApiHost + "/api/QKAmtLimitAssign/Put";
            }
        }
        /// <summary>
        /// 额度信息总览接口
        /// </summary>
        public static string GetQuotaInfoView
        {
            get {
                return QBApiHost + "/api/QKAmtLimit/GetAmtListSummary";
            }
        }

        /// <summary>
        /// 获取额度使用情况
        /// </summary>
        public static string GetAmtUseSummary
        {
            get {
                return QBApiHost + "/api/QKAmtLimitAssign/GetAmtUseSummary";
            }
        }
        #endregion
        /// <summary>
        /// 根据字典类型获取列表
        /// </summary>
        public static string QKDictionary
        {
            get
            {
                return QBApiHost + "/api/QKDictionary/Get";
            }
        }
        #region 02 标的相关接口
        /// <summary>
        /// 标的接口
        /// </summary>
        public static string QKBid
        {
            get
            {
                return QBApiHost + "/Api/QKBid/BidList";
            }
        }
        /// <summary>
        /// 挂标接口
        /// </summary>
        public static string QKBidHangBid
        {
            get
            {
                return QBApiHost + "/Api/QKBid/HangBid";
            }
        }
        /// <summary>
        /// 流标标接口
        /// </summary>
        public static string QKBidUndoBid
        {
            get
            {
                return QBApiHost + "/Api/QKBid/UndoBid";
            }
        }
        /// <summary>
        /// 取消挂标接口
        /// </summary>
        public static string QKBidCancelHangBid
        {
            get
            {
                return QBApiHost + "/Api/QKBid/CancelHangBid";
            }
        }
        /// <summary>
        /// 协议确认接口
        /// </summary>
        public static string QKBidAgreementConfirm
        {
            get
            {
                return QBApiHost + "/Api/QKBid/AgreementConfirm";
            }
        }
        
         /// <summary>
        /// 协议补录调用ams接口
        /// </summary>
        public static string QKBidAgreementAdditionalUpload
        {
            get
            {
                return QBApiHost + "/Api/QKBid/AgreementAdditionalUpload";
            }
        }
        /// <summary>
        /// 获取信托信息接口
        /// </summary>
        public static string QKBidGetP2Ptrust
        {
            get
            {
                return QBApiHost + "/Api/QKBid/GetP2Ptrust";
            }
        }
        /// <summary>
        /// 获取T24产品信息接口
        /// </summary>
        public static string QKLoanGetProductMap
        {
            get
            {
                return QBApiHost + "/Api/QKLoan/GetProductMap";
            }
        }
        /// <summary>
        /// 协议上传提交接口
        /// </summary>
        public static string QKBidAgreementUpload
        {
            get
            {
                return QBApiHost + "/Api/QKBid/AgreementUpload";
            }
        }
        /// <summary>
        /// 协议驳回接口
        /// </summary>
        public static string QKBidAgreementReject
        {
            get
            {
                return QBApiHost + "/Api/QKBid/AgreementReject";
            }
        }
        /// <summary>
        /// 获取标的信息
        /// </summary>
        public static string QKBidDetail
        {
            get
            {
                return QBApiHost + "/Api/QKBid/GetBidDetail";
            }
        }
        /// <summary>
        /// 标的系统配置
        /// </summary>
        public static string QKBidSysConfig
        {
            get
            {
                return QBApiHost + "/Api/QKSysConfig/GetSysConfigInfo";
            }
        }
        /// <summary>
        /// 标的详情配置
        /// </summary>
        public static string QKBidSysConfigType
        {
            get
            {
                return QBApiHost + "/Api/QKSysConfig/GetSysConfigByType";
            }
        }
        /// <summary>
        /// 协议确认历史
        /// </summary>
        public static string QKAgreementHistoryList
        {
            get
            {
                return QBApiHost + "/Api/QkBid/AgreementHistoryList";
            }
        }
         /// <summary>
        /// 协议确认历史添加
        /// </summary>
        public static string QKAgreementAddHistory
        {
            get
            {
                return QBApiHost + "/Api/QkBid/AgreementAddHistory";
            }
        }
        /// <summary>
        /// 回写固定还款日到标的
        /// </summary>
        public static string QKSetRepayMentDay
        {
            get
            {
                return QBApiHost + "/api/QkBid/SetRepayMentDay";
            }
        }
        
         /// <summary>
        /// 回写签约时间到标的
        /// </summary>
        public static string QKSetBidSignedTime
        {
            get
            {
                return QBApiHost + "/api/QkBid/SetBidSignedTime";
            }
        }
        
        /// <summary>
        /// 回写合同生成时间和标示
        /// </summary>
        public static string QKSetContractState
        {
            get
            {
                return QBApiHost + "/api/QkBid/SetContractState";
            }
        }
        /// <summary>
        /// 自动任务执行接口
        /// </summary>
        public static string QKAutoJobConfig
        {
            get
            {
                return QBApiHost + "/Api/AutoJobConfig";
            }
        }

        /// <summary>
        /// 自动任务配置创建接口
        /// </summary>
        public static string QKCreateOrUpdateJobConfigInfo
        {
            get
            {
                return QBApiHost + "/Api/AutoJobConfig/CreateOrUpdateJobConfigInfo";
            }
        }
        
        /// <summary>
        /// 自动任务配置接口
        /// </summary>
        public static string QKAutoJob
        {
            get
            {
                return QBApiHost + "/Api/AutoJob";
            }
        }

        /// <summary>
        /// 自动任务配置接口
        /// </summary>
        public static string QKAutoJobLog
        {
            get
            {
                return QBApiHost + "/Api/AutoJobLog/GetAutoJobLogList";
            }
        }

        /// <summary>
        /// 自动任务额度配置接口
        /// </summary>
        public static string QKAutoAmtJob
        {
            get
            {
                return QBApiHost + "/Api/AutoJobAmt";
            }
        }

        /// <summary>
        /// 删除自动任务主配置
        /// </summary>
        public static string QKDeleteJobAmtInfo
        {
            get
            {
                return QBApiHost + "/Api/AutoJobAmt/JobAmtInfoDelete";
            }
        }

        /// <summary>
        /// 保存自动任务配置
        /// </summary>
        public static string QKCreateOrUpdateJobAmtInfo
        {
            get
            {
                return QBApiHost + "/Api/AutoJobAmt/CreateOrUpdateJobAmtInfo";
            }
        }
        #endregion

        /// <summary>
        /// 系统配置信息
        /// </summary>
        public static string QKSysConfigInfoValue
        {
            get
            {
                return QBApiHost + "/Api/QKSysConfig/GetSysConfigInfoValue";
            }
        }

        /// <summary>
        /// 获取标的系统配置
        /// </summary>
        public static string QKSysConfigInfoList
        {
            get { return QBApiHost + "/Api/QKSysConfig/GetAll"; }
        }

        /// <summary>
        /// 通过Id获取标的系统配置
        /// </summary>
        public static string GetQKSysConfigById
        {
            get { return QBApiHost + "/Api/QKSysConfig/GetQB_SYS_CONFIG_INFO"; }
        }

        public static string QKSysConfigUpdate
        {
            get { return QBApiHost + "/Api/QKSysConfig/PutQB_SYS_CONFIG_INFO"; }
        }

        public static string QKSysConfigIsExistKey
        {
            get { return QBApiHost + "/Api/QKSysConfig/IsExistKey"; }
        }

        public static string QKSysConfigAdd
        {
            get { return QBApiHost + "/Api/QKSysConfig/PostQB_SYS_CONFIG_INFO"; }
        }

        public static string QKSysConfigDelete
        {
            get { return QBApiHost + "/Api/QKSysConfig/DeleteQB_SYS_CONFIG_INFO"; }
        }

        /// <summary>
        /// 获取失败合约列表
        /// </summary>
        public static string QKGetFailContractList
        {
            get { return QBApiHost + "/Api/QKLoan/DepositsFailure"; }
        }

        /// <summary>
        /// 将失败的合约重新发送
        /// </summary>
        public static string QKDepositsArrangementUpdate
        {
            get { return QBApiHost + "/Api/QKLoan/DepositsArrangementUpdate"; }
        }
    }
}
