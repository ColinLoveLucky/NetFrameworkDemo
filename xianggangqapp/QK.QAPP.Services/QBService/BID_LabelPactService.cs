using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QK.QAPP.Entity;
using QK.QAPP.Infrastructure.Cache;
using Microsoft.Practices.Unity;
using QK.QAPP.Global;
using QK.QAPP.Infrastructure.Log4Net;
using QK.QAPP.Infrastructure.MessageQueue;
using QK.QAPP.IServices;
using QK.QAPP.Infrastructure;
using System.Web.Mvc;
using System.Collections.Specialized;
using System.Web;

namespace QK.QAPP.Services
{

    public class BID_LabelPactService : IBID_LabelPactService
    {
        public ICacheProvider CacheService { get; set; }
        #region 获取标的配置信息
        public List<QB_SYS_CONFIG_INFO> GetBIDSysConfigByType(string type)
        {
            Dictionary<string, string> dicKey = new Dictionary<string, string>();
            NameValueCollection collection = new NameValueCollection();
            collection.Add("type", type);
            dicKey = SecuritySignHelper.GetSecurityCollectionWithSign(collection);
            RestApiHelper restClient = new RestApiHelper(GlobalApi.QKBidSysConfigType);
            var keyData = CacheService.GetFromCacheOrProxy<List<QB_SYS_CONFIG_INFO>>(
                "QB_GetBIDSysConfigByType_" + type,
                () => restClient.Get<List<QB_SYS_CONFIG_INFO>>(string.Empty, dicKey));
            return keyData;
        }
        public Dictionary<string, string> GetBIDSysConfigByKey(string key)
        {
            Dictionary<string, string> dicKey = new Dictionary<string, string>();
            NameValueCollection collection = new NameValueCollection();
            collection.Add("sysKey", key);
            dicKey = SecuritySignHelper.GetSecurityCollectionWithSign(collection);
            RestApiHelper restClient = new RestApiHelper(GlobalApi.QKBidSysConfig);
            var keyData = CacheService.GetFromCacheOrProxy<Dictionary<string, string>>(
                "QB_GetBIDSysConfigByKey_" + key,
                () => restClient.Get<Dictionary<string, string>>(string.Empty, dicKey));
            return keyData;
        }
        public string GetBIDSysConfigInfoValue(string key)
        {
            Dictionary<string, string> dicKey = new Dictionary<string, string>();
            NameValueCollection collection = new NameValueCollection();
            collection.Add("key", key);
            dicKey = SecuritySignHelper.GetSecurityCollectionWithSign(collection);
            RestApiHelper restClient = new RestApiHelper(GlobalApi.QKSysConfigInfoValue);
            var keyData = CacheService.GetFromCacheOrProxy<string>(
                "QB_GetBIDSysConfigInfoValue_" + key,
                () => restClient.Get<string>(string.Empty, dicKey));
            return keyData;
        }
        #endregion
        #region  获取标的列表
        public PageData<QB_V_BIDLIST> GetBIDInfoList(BidListQueryPara QueryPara)
        {
            return GetBIDList(QueryPara);
        }
        private PageData<QB_V_BIDLIST> GetBIDList(BidListQueryPara QueryPara)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            NameValueCollection collection = new NameValueCollection();
            RestApiHelper restApiHelper = new RestApiHelper(GlobalApi.QKBid);
            collection = Serializer.ObjToNameValueCollection(QueryPara);
            dic = Serializer.ObjToDictionary(QueryPara);
            var bidData = restApiHelper.Post<PageData<QB_V_BIDLIST>>(restApiHelper.GetUrlParam(SecuritySignHelper.PostSecurityCollectionWithSign(collection)), dic);
            if (bidData == null)
            {
                return new PageData<QB_V_BIDLIST>();
            }
            return bidData;
        }
        #endregion
        #region 发标操作
        public List<BidMatchTip> SendBid(BidOperateRequest operate)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            NameValueCollection collection = new NameValueCollection();
            RestApiHelper restApiHelper = new RestApiHelper(GlobalApi.QKBidHangBid);
            collection = Serializer.ObjToNameValueCollection(operate);
            dic = Serializer.ObjToDictionary(operate);
            var operateData = restApiHelper.Post<List<BidMatchTip>>(restApiHelper.GetUrlParam(SecuritySignHelper.PostSecurityCollectionWithSign(collection)), dic);
            return operateData;
        }
        #endregion
        #region 流标操作
        public List<BidMatchTip> UnDoBid(BidOperateRequest operate)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            NameValueCollection collection = new NameValueCollection();
            RestApiHelper restApiHelper = new RestApiHelper(GlobalApi.QKBidUndoBid);
            collection = Serializer.ObjToNameValueCollection(operate);
            dic = Serializer.ObjToDictionary(operate);
            var operateData = restApiHelper.Post<List<BidMatchTip>>(restApiHelper.GetUrlParam(SecuritySignHelper.PostSecurityCollectionWithSign(collection)), dic);
            return operateData;
        }
        #endregion
        #region 取消挂标操作
        public List<BidMatchTip> CancelHangBid(BidOperateRequest operate)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            NameValueCollection collection = new NameValueCollection();
            RestApiHelper restApiHelper = new RestApiHelper(GlobalApi.QKBidCancelHangBid);
            collection = Serializer.ObjToNameValueCollection(operate);
            dic = Serializer.ObjToDictionary(operate);
            var operateData = restApiHelper.Post<List<BidMatchTip>>(restApiHelper.GetUrlParam(SecuritySignHelper.PostSecurityCollectionWithSign(collection)), dic);
            return operateData;
        }
        public string CancelHangBidJson(BidOperateRequest operate)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            NameValueCollection collection = new NameValueCollection();
            RestApiHelper restApiHelper = new RestApiHelper(GlobalApi.QKBidCancelHangBid);
            collection = Serializer.ObjToNameValueCollection(operate);
            dic = Serializer.ObjToDictionary(operate);
            var operateData = restApiHelper.Post<string>(restApiHelper.GetUrlParam(SecuritySignHelper.PostSecurityCollectionWithSign(collection)), dic);
            return operateData;
        }
        #endregion
        #region 协议确认操作
        public List<BidMatchTip> AgreementConfirm(BidOperateRequest operate)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            NameValueCollection collection = new NameValueCollection();
            RestApiHelper restApiHelper = new RestApiHelper(GlobalApi.QKBidAgreementConfirm);
            collection = Serializer.ObjToNameValueCollection(operate);
            dic = Serializer.ObjToDictionary(operate);
            var operateData = restApiHelper.Post<List<BidMatchTip>>(restApiHelper.GetUrlParam(SecuritySignHelper.PostSecurityCollectionWithSign(collection)), dic);
            return operateData;
        }
        #endregion
        #region 协议上传提交
        public BidMatchTip AgreementUpload(string bidCode)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            NameValueCollection collection = new NameValueCollection();
            collection.Add("bidCode", bidCode);
            dic = SecuritySignHelper.GetSecurityCollectionWithSign(collection);
            var operateData = new RestApiHelper(GlobalApi.QKBidAgreementUpload).Get<BidMatchTip>(string.Empty, dic);
            return operateData;
        }
        #endregion
        #region 协议驳回操作
        public string AgreementReject(BidOperateRequest operate)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            NameValueCollection collection = new NameValueCollection();
            RestApiHelper restApiHelper = new RestApiHelper(GlobalApi.QKBidAgreementReject);
            collection = Serializer.ObjToNameValueCollection(operate);
            dic = Serializer.ObjToDictionary(operate);
            var operateData = restApiHelper.Post<string>(restApiHelper.GetUrlParam(SecuritySignHelper.PostSecurityCollectionWithSign(collection)), dic);
            return operateData;
        }
        #endregion
        #region 获取标的详情
        public QB_V_BID_DETAIL GetBidDetail(string bidCode)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            NameValueCollection collection = new NameValueCollection();
            collection.Add("bidCode", bidCode);
            dic = SecuritySignHelper.GetSecurityCollectionWithSign(collection);
            var bidDetailInfo = new RestApiHelper(GlobalApi.QKBidDetail).Get<QB_V_BID_DETAIL>(string.Empty, dic);
            if (bidDetailInfo == null)
            {
                return new QB_V_BID_DETAIL();
            }
            return bidDetailInfo;
        }
        public QB_V_BID_DETAIL GetBidDetail(string bidCode, string partner, string partnerKey)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            NameValueCollection collection = new NameValueCollection();
            collection.Add("bidCode", bidCode);
            dic = SecuritySignHelper.GetSecurityCollectionWithSign(collection,partner,partnerKey);
            var bidDetailInfo = new RestApiHelper(GlobalApi.QKBidDetail).Get<QB_V_BID_DETAIL>(string.Empty, dic);
            if (bidDetailInfo == null)
            {
                return new QB_V_BID_DETAIL();
            }
            return bidDetailInfo;
        }
        public string GetBidDetailJson(string bidCode)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            NameValueCollection collection = new NameValueCollection();
            collection.Add("bidCode", bidCode);
            dic = SecuritySignHelper.GetSecurityCollectionWithSign(collection);
            var bidDetailInfo = new RestApiHelper(GlobalApi.QKBidDetail).Get<string>(string.Empty, dic);
            if (bidDetailInfo == null)
            {
                return Serializer.ToJson(new QB_V_BID_DETAIL());
            }
            return bidDetailInfo;
        }
        /// <summary>
        /// 获取标的详情json
        /// </summary>
        /// <param name="bidCode"></param>
        /// <returns></returns>
        public string GetBidDetailJson(BidDetailRequest bidDetail)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            NameValueCollection collection = new NameValueCollection();
            RestApiHelper restApiHelper = new RestApiHelper(GlobalApi.QKBidDetail);
            collection = Serializer.ObjToNameValueCollection(bidDetail);
            dic = Serializer.ObjToDictionary(bidDetail);
            var bidDetailInfo = restApiHelper.Post<string>(restApiHelper.GetUrlParam(SecuritySignHelper.PostSecurityCollectionWithSign(collection)), dic);
            if (bidDetailInfo == null)
            {
                return Serializer.ToJson(new QB_V_BID_DETAIL());
            }
            return bidDetailInfo;
        }
        #endregion
        #region 生成标的详情页
        public string CreateBidDetailInfo(string logo, string appid, Dictionary<string, string> bidInfo)
        {
            var bidInfoFiled = this.GetBidInfoFiled(logo);
            var bidInfoFormatUnit = this.GetBidInfoFormatUnit();
            var dformCode = new Dictionary<string, string>() { { "logo", logo }, { "appid", appid } };
            return CreateBidDetailInfoHtml(bidInfo, bidInfoFiled, bidInfoFormatUnit, dformCode);
        }
        /// <summary>
        /// 生成标的详情页html
        /// </summary>
        /// <param name="bidDetailInfoDic"></param>
        /// <param name="bidInfoFiled"></param>
        /// <param name="bidInfoFormatUnit"></param>
        /// <param name="dformCode"></param>
        /// <returns></returns>
        private string CreateBidDetailInfoHtml(Dictionary<string, string> bidDetailInfoDic, Dictionary<string, Dictionary<string, string>> bidInfoFiled, Dictionary<string, string> bidInfoFormatUnit, Dictionary<string, string> dformCode)
        {
            StringBuilder sbHtml = new StringBuilder();

            //业务要求：如果划标类型是T2P,则将风险准备金置为 0 
            if (bidDetailInfoDic.Keys.Contains("BID_DIVIDE_TYPE"))
            {
                if (bidDetailInfoDic["BID_DIVIDE_TYPE"] != null)
                {
                    if (bidDetailInfoDic["BID_DIVIDE_TYPE"].Contains(GlobalSetting.IsShowLoanLossProvision))
                    {
                        bidDetailInfoDic["BID_LOAN_LOSS_PROVISION"] = "0";
                    }
                }
            }
            foreach (var i in bidInfoFiled.Keys)
            {
                sbHtml.AppendLine("<div class=\"form-group\">");
                sbHtml.AppendLine("     <div class=\"clearfix\">");
                foreach (var item in bidInfoFiled[i.ToString()].Keys)
                {
                    sbHtml.AppendLine("      <span class=\"control-label col-xs-12 col-sm-2\">" + bidInfoFiled[i.ToString()][item] + "</span>");

                    if (item == "BID_APP_CODE")
                    {
                        sbHtml.AppendLine("         <div id=\"" + item + "\" class=\"col-xs-12 col-sm-3\">");
                        sbHtml.AppendLine("            <a style='text-decoration:underline;' id=\"" + item + "\" href=\"/LoanApplication/Application?dformCode=" + dformCode["logo"] + "&operation=3&appid=" + dformCode["appid"] + "\"\">");
                        sbHtml.AppendLine(bidDetailInfoDic[item]);
                        sbHtml.AppendLine("            </a>");
                        sbHtml.AppendLine("         </div>");
                    }
                    else
                    {
                        sbHtml.AppendLine("         <div id=\"" + item + "\" class=\"col-xs-12 col-sm-3\">");
                        sbHtml.AppendLine(FormatOper(bidInfoFormatUnit, item, bidDetailInfoDic[item]));
                        sbHtml.AppendLine("         </div>");
                    }
                }
                sbHtml.AppendLine("      </div>");
                sbHtml.AppendLine("</div>");
            }
            return sbHtml.ToString();
        }
        /// <summary>
        /// 获取标的详情页显示字段配置
        /// </summary>
        /// <param name="logo"></param>
        /// <returns></returns>
        private Dictionary<string, Dictionary<string, string>> GetBidInfoFiled(string logo)
        {
            var bidInfoFiled = string.Empty;
            if (!string.IsNullOrEmpty(logo))
            {
                bidInfoFiled = GetBidDetailConfigValue(logo);
            }
            if (string.IsNullOrWhiteSpace(bidInfoFiled))
            {
                bidInfoFiled = this.GetBidDetailConfigValue(BidSysConfigDic.BidInfo_DeflaultFiled);
            }
            if (string.IsNullOrWhiteSpace(bidInfoFiled))
                return new Dictionary<string, Dictionary<string, string>>();
            else
                return Serializer.FromJson<Dictionary<string, Dictionary<string, string>>>(bidInfoFiled);

        }
        /// <summary>
        /// 获取标的详情
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, string> GetBidInfoFormatUnit()
        {
            var bidInfoFormatUnit = this.GetBidDetailConfigValue(BidSysConfigDic.BidInfo_FormatUnit);
            if (string.IsNullOrWhiteSpace(bidInfoFormatUnit))
                return new Dictionary<string, string>();
            else
                return Serializer.FromJson<Dictionary<string, string>>(bidInfoFormatUnit);
        }
        /// <summary>
        /// 格式化单位符号
        /// </summary>
        /// <param name="OperDic"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private string FormatOper(Dictionary<string, string> OperDic, string key, string value)
        {
            value = value == null ? "" : value;
            if (OperDic.Keys.Contains(key))
            {
                float valuef = 0;
                switch (OperDic[key])
                {
                    case "元":
                        if (string.IsNullOrWhiteSpace(value))
                        {
                            return string.Format("{0:N}元", 0);
                        }
                        else if (float.TryParse(value, out valuef))
                        {
                            return string.Format("{0:N}元", Convert.ToDouble(value));
                        }
                        else
                            return string.Format("{0}元", value);
                    case "%":
                        if (string.IsNullOrWhiteSpace(value))
                        {
                            return string.Format("{0:P5}", 0);
                        }
                        else if (float.TryParse(value, out valuef))
                        {
                            return string.Format("{0:P5}", Convert.ToDouble(value));
                        }
                        else
                            return string.Format("{0}%", value);
                    case "‰":
                        if (string.IsNullOrWhiteSpace(value))
                        {
                            return string.Format("{0:F5}‰", 0);
                        }
                        else if (float.TryParse(value, out valuef))
                        {

                            return string.Format("{0:F5}‰", Convert.ToDouble(value) * 1000);
                        }
                        else
                            return string.Format("{0}‰", value);
                    case "月":
                        if (string.IsNullOrWhiteSpace(value))
                        {
                            return string.Format("{0}月", 0);
                        }
                        else
                            return string.Format("{0}月", value);
                    case "日":
                        if (string.IsNullOrWhiteSpace(value))
                        {
                            return string.Format("{0}日", 0);
                        }
                        else
                            return string.Format("{0}日", value);

                    default:
                        return value;
                }
            }
            else
                return value;
        }
        /// <summary>
        /// 获取标的配置Value
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private string GetBidDetailConfigValue(string key)
        {
            List<QB_SYS_CONFIG_INFO> BidDetailConfig = GetBIDSysConfigByType("BidDetailConfig");
            if (BidDetailConfig != null)
            {
                var bidDetailConfigInfo = BidDetailConfig.FirstOrDefault(o => o.SYS_KEY == key);
                if (bidDetailConfigInfo != null)
                {
                    return bidDetailConfigInfo.SYS_VALUE;
                }
            }
            return "";
        }
        #endregion
        /// <summary>
        /// 协议补录推送ams
        /// </summary>
        /// <param name="bidCode"></param>
        /// <param name="?"></param>
        /// <returns></returns>
        public BidMatchTip AgreementAdditionalUpload(string bidCode)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            NameValueCollection collection = new NameValueCollection { { "bidCode", bidCode } };
            dic = collection.GetSecurityCollectionWithSign();
            return new RestApiHelper(GlobalApi.QKBidAgreementAdditionalUpload).Get<BidMatchTip>(string.Empty, dic);
        }
        /// <summary>
        /// 获取信托信息
        /// </summary>
        /// <param name="trustno"></param>
        /// <returns></returns>
        public List<QB_P2P_TRUST> GetP2Ptrust(string TrustNo)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>() { { "TrustNo", TrustNo } };
            return new RestApiHelper(GlobalApi.QKBidGetP2Ptrust).Get<List<QB_P2P_TRUST>>(string.Empty, dic);
        }
        /// <summary>
        /// 回写合同生成时间和生成状态
        /// </summary>
        /// <param name="request"></param>
        /// <param name="QKSetContractState"></param>
        /// <returns></returns>
        public BidMatchTip SetContractState(ContractOperateRequest request, string QKSetContractState)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            RestApiHelper restApiHelper = new RestApiHelper(QKSetContractState);
            dic = Serializer.ObjToDictionary(request);
            var operateData = restApiHelper.Post<BidMatchTip>(string.Empty, dic);
            return operateData;
        }
        /// <summary>
        /// 获取T24产品code
        /// </summary>
        /// <param name="prdCode">产品code</param>
        /// <returns></returns>
        public QB_PRODUCT_MAP GetProductMap(string productCode)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>() { { "productCode", productCode } };
            return new RestApiHelper(GlobalApi.QKLoanGetProductMap).Get<QB_PRODUCT_MAP>(string.Empty, dic);
        }
    }
}
