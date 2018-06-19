using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QK.QAPP.Entity;
using QK.QAPP.Infrastructure.Cache;

namespace QK.QAPP.IServices
{
    public interface  IBID_LabelPactService
    {
        /// <summary>
        /// 标的查询列表
        /// </summary>
        /// <param name="url">接口url带签名参数</param>
        /// <param name="item">post参数对象</param>
        /// <returns></returns>
        PageData<QB_V_BIDLIST> GetBIDInfoList(BidListQueryPara QueryPara);
        /// <summary>
        /// 挂标操作
        /// </summary>
        /// <param name="operate"></param>
        /// <returns></returns>
        List<BidMatchTip> SendBid(BidOperateRequest operate);
        /// <summary>
        /// 流标操作
        /// </summary>
        /// <param name="operate"></param>
        /// <returns></returns>
        List<BidMatchTip> UnDoBid(BidOperateRequest operate);
        /// <summary>
        /// 取消挂标操作
        /// </summary>
        /// <param name="operate"></param>
        /// <returns></returns>
        List<BidMatchTip> CancelHangBid(BidOperateRequest operate);
        /// <summary>
        /// 取消挂标操作
        /// </summary>
        /// <param name="operate"></param>
        /// <returns></returns>
        string CancelHangBidJson(BidOperateRequest operate);
        /// <summary>
        /// 协议确认
        /// </summary>
        /// <param name="bidCode"></param>
        /// <returns></returns>
        List<BidMatchTip> AgreementConfirm(BidOperateRequest operate);
        /// <summary>
        /// 协议上传提交
        /// </summary>
        /// <param name="bidCode"></param>
        /// <returns></returns>
        BidMatchTip AgreementUpload(string bidCode);
        /// <summary>
        /// 协议驳回
        /// </summary>
        /// <param name="bidCode"></param>
        /// <param name="rejectCode"></param>
        /// <param name="rejectRemark"></param>
        /// <returns></returns>
        string AgreementReject(BidOperateRequest operate);
        /// <summary>
        /// 获取标的详情
        /// </summary>
        /// <param name="bidCode">标的编号</param>
        /// <returns></returns>
        QB_V_BID_DETAIL GetBidDetail(string bidCode);
        /// <summary>
        /// 获取标的详情(解决异步线程调用报错的问题)
        /// </summary>
        /// <param name="bidCode">标的编号</param>
        /// <param name="partner">签名私钥</param>
        /// <param name="partnerKey">签名公钥</param>
        /// <returns></returns>
        QB_V_BID_DETAIL GetBidDetail(string bidCode, string partner, string partnerKey);
        /// <summary>
        /// 获取标的详情json
        /// </summary>
        /// <param name="bidCode"></param>
        /// <returns></returns>
        string GetBidDetailJson(string bidCode);
        /// <summary>
        /// 获取标的详情json
        /// </summary>
        /// <param name="bidCode"></param>
        /// <returns></returns>
        string GetBidDetailJson(BidDetailRequest bidDetail);
        /// <summary>
        /// 获取标的系统配置状态
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        Dictionary<string, string> GetBIDSysConfigByKey(string key);
        /// <summary>
        /// 获取系统配置信息
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        string GetBIDSysConfigInfoValue(string key);
        /// <summary>
        /// 生成标的详情html
        /// </summary>
        /// <param name="logo"></param>
        /// <param name="appid"></param>
        /// <param name="bidCode"></param>
        /// <returns></returns>
        string CreateBidDetailInfo(string logo, string appid, Dictionary<string,string> bidInfo);
        /// <summary>
        /// 根据系统配置类型获取配置信息
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        List<QB_SYS_CONFIG_INFO> GetBIDSysConfigByType(string type);
         /// <summary>
        /// 协议补录推送ams
        /// </summary>
        /// <param name="bidCode"></param>
        /// <param name="?"></param>
        /// <returns></returns>
         BidMatchTip AgreementAdditionalUpload(string bidCode);
        /// <summary>
        /// 获取信托信息
        /// </summary>
        /// <param name="trustno"></param>
        /// <returns></returns>
         List<QB_P2P_TRUST> GetP2Ptrust(string TrustNo);
         /// <summary>
        /// 回写合同生成时间和生成状态
        /// </summary>
        /// <param name="request"></param>
        /// <param name="QKSetContractState"></param>
        /// <returns></returns>
         BidMatchTip SetContractState(ContractOperateRequest request, string QKSetContractState);
        /// <summary>
        /// 获取T24产品code
        /// </summary>
        /// <param name="prdCode">产品code</param>
        /// <returns></returns>
         QB_PRODUCT_MAP GetProductMap(string productCode);
    }
}
