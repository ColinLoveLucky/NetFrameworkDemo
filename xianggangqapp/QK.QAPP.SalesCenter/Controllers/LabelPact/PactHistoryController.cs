using QK.QAPP.Entity;
using QK.QAPP.Infrastructure;
using QK.QAPP.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QK.QAPP.SalesCenter.Controllers.LabelPact
{

    public class PactHistoryController : Controller
    {
        public IQFUserService QFUserHelper { get; set; }
        public IBID_PactHistoryService bidPactHistoryService { get; set; }
        //
        // GET: /PactHistory/
        public ActionResult Index()
        {
            return View("~/Views/LabelPact/Manage/PactHistoryList.cshtml");
        }
        /// <summary>
        /// 获取协议确认历史列表
        /// </summary>
        /// <returns></returns>
        public JsonResult GetPactHistoryList()
        {
            BidListQueryPara queryPara = new BidListQueryPara();
            var user = QFUserHelper.GetCurrentUser();
            queryPara.CurrentPage = int.Parse(Request["page"] ?? "0");
            queryPara.PageSize = int.Parse(Request["rows"] ?? "0");
            queryPara.SearchMsg = HttpUtility.UrlDecode(Request["searchPara"]);
            PageData<QB_CONTRACT_HISTORY> bidlist = bidPactHistoryService.AgreementHistoryList(queryPara);
            return Json(bidlist, JsonRequestBehavior.AllowGet);
        }

	}
}