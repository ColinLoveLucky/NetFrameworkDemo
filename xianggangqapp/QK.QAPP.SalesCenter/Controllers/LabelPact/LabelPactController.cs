using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using QK.QAPP.Global;
using QK.QAPP.Entity;
using QK.QAPP.IServices;
using Microsoft.Practices.Unity;
using QK.QAPP.Infrastructure;

namespace QK.QAPP.SalesCenter.Controllers
{
    public class Bid
    {

        public string BID_CODE { get; set; }

    }
    public class LabelPactController : Controller
    {
        [Dependency]
        public IBID_LabelPactService bidLabelPactService { get; set; }
        [Dependency]
        public IQFUserService QFUserHelper { get; set; }
        //
        // GET: /LabelPact/
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// 标的管理
        /// </summary>
        /// <returns></returns>
        public ActionResult LabelManage()
        {
            var dicBMS = bidLabelPactService.GetBIDSysConfigByKey(BidStatusQueryType.Bid_MarkingState);//划标情况
            var dicBZHMS = bidLabelPactService.GetBIDSysConfigByKey(BidStatusQueryType.Bid_ZH_MarkingState);//中航审核结果
            var dicBAS = bidLabelPactService.GetBIDSysConfigByKey(BidStatusQueryType.Bid_AgreementState);//协议状态
            var dicBZHAS = bidLabelPactService.GetBIDSysConfigByKey(BidStatusQueryType.Bid_ZH_AgreementState);//中航协议审核结果
            ViewData[BidStatusQueryType.Bid_MarkingState] = dicBMS == null ? new Dictionary<string, string>() : dicBMS;
            ViewData[BidStatusQueryType.Bid_ZH_MarkingState] = dicBZHMS == null ? new Dictionary<string, string>() : dicBZHMS;
            ViewData[BidStatusQueryType.Bid_AgreementState] = dicBAS == null ? new Dictionary<string, string>() : dicBAS;
            ViewData[BidStatusQueryType.Bid_ZH_AgreementState] = dicBZHAS == null ? new Dictionary<string, string>() : dicBZHAS;

            ViewBag.SBid_Step_NotSendBid = BidStatusQueryType.Bid_Step_NotSendBid;
            ViewBag.SBid_Step_CollectBid = BidStatusQueryType.Bid_Step_CollectBid;
            ViewBag.SBid_Step_FullBid = BidStatusQueryType.Bid_Step_FullBid;
            return View("~/Views/LabelPact/Manage/LabelManage.cshtml");
        }

        /// <summary>
        /// 发标操作
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult SendBid(string bidCode)
        {
            if (string.IsNullOrEmpty(bidCode))
                return Json(false);
            var user = QFUserHelper.GetCurrentUser();
            var dp = QFUserHelper.GetDataPermission();
            BidOperateRequest bidOperate = new BidOperateRequest();
            bidOperate.BidList = bidCode;
            bidOperate.UserCode = user.Account;
            bidOperate.UserName = user.RealName;
            string msg = string.Empty;
            var resultMsg = bidLabelPactService.SendBid(bidOperate);
            if (resultMsg != null)
            {
                resultMsg.ForEach(f =>
                {
                    msg += f.BidCode + ":" + f.TipMsg + ";  ";

                });
            }
            else
            {
                msg = "挂标失败接口请求异常！";
            }
            Infrastructure.Log4Net.LogWriter.Biz("挂标操作", "挂标操作", resultMsg);
            return Json(msg);

        }
        /// <summary>
        /// 未发标列表
        /// </summary>
        /// <returns></returns>
        public JsonResult GetSendList()
        {
            /*
             * fuzzySearch为0或1，查询列表按条件搜索框进行精确查询或模糊查询；
             * fuzzySearch为null，查询列表按默认的tab加载列表即（未发标列表、在凑标列表、已满标列表）进行查询；
             * GlobalSearch = 0为精确查询；
             * GlobalSearch = 1为模糊查询；
             */
            BidListQueryPara queryPara = new BidListQueryPara();
            /*数据权限参数 start */
            QFUserAuth userAuth = QFUserHelper.GetUserAuth();
            queryPara.IsSelectAll = userAuth.IsSelectAll;
            queryPara.AccountList = Serializer.ListToString(userAuth.AccountList, ",");
            queryPara.ParentIdList = userAuth.ParentIdList.JoinString(",");
            queryPara.CompanyList = userAuth.CompanyList.JoinString(",");
            /*数据权限参数 end */
            queryPara.CurrentPage = int.Parse(Request["page"] ?? "0");
            queryPara.PageSize = int.Parse(Request["rows"] ?? "0");
            queryPara.GlobalSearch = 0;//默认精确查询
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
                    queryPara.BidCustomerManagerName = HttpUtility.UrlDecode(Request["bidCustomerNamagerName"]);
                    queryPara.BidCustomerManager = HttpUtility.UrlDecode(Request["bidCustomerMamager"]);
                    queryPara.BidServiceName = HttpUtility.UrlDecode(Request["bidServiceName"]);
                    queryPara.BidState = HttpUtility.UrlDecode(Request["bidState"]);
                    queryPara.BidThirdState = HttpUtility.UrlDecode(Request["bidZHState"]);
                    queryPara.BidAgreementState = HttpUtility.UrlDecode(Request["bidAgreementState"]);
                    queryPara.BidDivideType = HttpUtility.UrlDecode(Request["bidDivideType"]);
                    queryPara.BidOccurType = HttpUtility.UrlDecode(Request["bidOccurType"]);

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
        /// 标的详情信息
        /// </summary>
        /// <returns></returns>
        public ActionResult LabelDetail(string bidCode, string logo, string appid)
        {
            if (string.IsNullOrWhiteSpace(bidCode))
            {
                Infrastructure.Log4Net.LogWriter.Biz("标的详情非法访问，bidCode为空！");
                return new RedirectResult("/Home/NoAuthorization");
            }
            BidDetailRequest bidDetailR = new BidDetailRequest();
            bidDetailR.BidIdentifier = bidCode;
            #region 验证数据权限  return new RedirectResult("/Home/NoAuthorization");
            //权限
            QFUserAuth userAuth = QFUserHelper.GetUserAuth();
            bidDetailR.IsSelectAll = userAuth.IsSelectAll;
            bidDetailR.AccountList = Serializer.ListToString(userAuth.AccountList, ",");
            bidDetailR.ParentIdList = userAuth.ParentIdList.JoinString(",");
            bidDetailR.CompanyList = userAuth.CompanyList.JoinString(",");
            //var bidInfo = Serializer.FromJson<Dictionary<string, string>>(bidLabelPactService.GetBidDetailJson(bidCode));
            var bidInfo = Serializer.FromJson<Dictionary<string, string>>(bidLabelPactService.GetBidDetailJson(bidDetailR));
            if (bidInfo == null)
            {
                Infrastructure.Log4Net.LogWriter.Biz("标的详情非法访问，未找到相关数据！");
                return new RedirectResult("/Home/NoAuthorization");
            }
            if (bidInfo["BID_BUS_LOGO"] != logo || bidInfo["BID_APP_ID"] != appid)
            {
                Infrastructure.Log4Net.LogWriter.Biz("标的详情非法访问，未找到BID_BUS_LOGO,BID_APP_ID相关数据！");
                return new RedirectResult("/Home/NoAuthorization");
            }
            #endregion
            ViewBag.CreateBidInfo = bidLabelPactService.CreateBidDetailInfo(logo, appid, bidInfo);
            return View("~/Views/LabelPact/Manage/LabelDetail.cshtml");
        }
    }
}