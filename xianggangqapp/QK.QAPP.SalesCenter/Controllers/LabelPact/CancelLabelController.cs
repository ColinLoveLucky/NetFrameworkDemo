using Microsoft.Practices.Unity;
using QK.QAPP.Entity;
using QK.QAPP.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QK.QAPP.Infrastructure;

namespace QK.QAPP.SalesCenter.Controllers.LabelPact
{
    public class CancelLabelController : Controller
    {
        public IBID_LabelPactService bidLabelPactService { get; set; }
        public IQFUserService QFUserHelper { get; set; }
        public IQuotaBidDicConfigService bidDicConfigService { get; set; }
        //
        // GET: /CancelLabel/
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult CancelLabelManage()
        {
            var bidMarkState = bidLabelPactService.GetBIDSysConfigByKey(BidStatusQueryType.Bid_MarkingState);
            var bidZHMarkState = bidLabelPactService.GetBIDSysConfigByKey(BidStatusQueryType.Bid_ZH_MarkingState);
            var bidUndoReason = bidDicConfigService.GetDicByType(BidSysConfigDic.BidUndoReason);
            ViewData[BidStatusQueryType.Bid_MarkingState] = bidMarkState == null ? new Dictionary<string, string>() : bidMarkState;
            ViewData[BidStatusQueryType.Bid_ZH_MarkingState] = bidZHMarkState == null ? new Dictionary<string, string>() : bidZHMarkState;
            ViewData[BidSysConfigDic.BidUndoReason] = bidUndoReason == null ? new Dictionary<string, string>() : bidUndoReason;
            ViewBag.SBid_Step_NotSendBid = BidStatusQueryType.Bid_Step_NotSendBid;
            ViewBag.SBid_Step_FailBid = BidStatusQueryType.Bid_Step_FailBid;
            ViewBag.SBid_Step_FullBid = BidStatusQueryType.Bid_Step_FullBid;
            return View("~/Views/LabelPact/Manage/CancellationLabelManage.cshtml");
        }
        /// <summary>
        /// 进行流标列表
        /// </summary>
        /// <returns></returns>
        public JsonResult CancelLabelList()
        {
            /*
             * fuzzySearch为0或1，查询列表按条件搜索框进行精确查询或模糊查询；
             * fuzzySearch为null，查询列表按默认的tab加载列表即（未发标列表、在凑标列表、已满标列表）进行查询；
             * GlobalSearch = 0为精确查询；
             * GlobalSearch = 1为模糊查询；
             */
            BidListQueryPara queryPara = new BidListQueryPara();
            queryPara.CurrentPage = int.Parse(Request["page"] ?? "0");
            queryPara.PageSize = int.Parse(Request["rows"] ?? "0");
            queryPara.GlobalSearch = 0;//默认精确查询
            //用户权限
            QFUserAuth userAuth = QFUserHelper.GetUserAuth();
            queryPara.IsSelectAll = userAuth.IsSelectAll;
            queryPara.AccountList = Serializer.ListToString(userAuth.AccountList, ",");
            queryPara.ParentIdList = userAuth.ParentIdList.JoinString(",");
            queryPara.CompanyList = userAuth.CompanyList.JoinString(",");
            if (Request["bidStep"] != null)
            {
                queryPara.BidStep = Request["bidStep"].ToString();
            }
            if (Request["fuzzySearch"] != null)
            {
                if (Request["fuzzySearch"].ToString() == "0")
                {
                    queryPara.GlobalSearch = 0;
                    queryPara.BidAppCode = HttpUtility.UrlDecode(Request["bidAppCode"]);
                    queryPara.BidCode = HttpUtility.UrlDecode(Request["bidCode"]);
                    queryPara.BidContractNo = HttpUtility.UrlDecode(Request["bidContractNo"]);
                    queryPara.BidCustomerName = HttpUtility.UrlDecode(Request["bidCustomerName"]);
                    queryPara.BidCustomerManager = HttpUtility.UrlDecode(Request["bidCustomerNamager"]);
                    queryPara.BidServiceName = HttpUtility.UrlDecode(Request["bidServiceName"]);
                    queryPara.BidState = HttpUtility.UrlDecode(Request["bidState"]);
                    queryPara.BidThirdState = HttpUtility.UrlDecode(Request["bidZHState"]);
                    queryPara.BidAgreementState = HttpUtility.UrlDecode(Request["bidAgreementState"]);
                    queryPara.BidOccurType = HttpUtility.UrlDecode(Request["bidOccurType"]);
                    queryPara.BidDivideType = HttpUtility.UrlDecode(Request["bidDivideType"]);

                }
                else
                {
                    queryPara.GlobalSearch = 1;
                    queryPara.SearchMsg = HttpUtility.UrlDecode(Request["searchPara"]);
                }
            }
            PageData<QB_V_BIDLIST> bidlist = bidLabelPactService.GetBIDInfoList(queryPara);
            return Json(bidlist, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 撤销发标操作（即流标操作）
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult CancelSendBid(string bidCode,string unDoResonCode,string unDoReson)
        {
            if (string.IsNullOrEmpty(bidCode))
                return Json(false);
            var user = QFUserHelper.GetCurrentUser();
            var dp = QFUserHelper.GetDataPermission();
            BidOperateRequest bidOperate = new BidOperateRequest();
            bidOperate.BidList = bidCode;
            bidOperate.UserCode = user.Account;
            bidOperate.UserName = user.RealName;
            bidOperate.SubUserCodeList = Serializer.ListToString(dp, ",");
            bidOperate.RejectCode = unDoResonCode;
            bidOperate.RejectRemark = unDoReson;
            var resultMsg = bidLabelPactService.UnDoBid(bidOperate);
            Infrastructure.Log4Net.LogWriter.Biz("流标操作", bidCode, resultMsg);
            string msg = string.Empty;
            resultMsg.ForEach(f =>
            {
                msg += f.BidCode + ":" + f.TipMsg + ";  ";

            });
            return Json(msg);
        }
	}
}