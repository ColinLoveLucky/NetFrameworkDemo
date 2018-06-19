using QK.QAPP.Entity.QbEntity;
using QK.QAPP.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QK.QAPP.SalesCenter.Controllers.LabelPact
{
    public class BidHistoryController : Controller
    {
        public IQuotaManageService quotaManageService { get; set; }
        //
        // GET: /BidHistory/
        public ActionResult BidHistory()
        {
            return View("~/Views/LabelPact/BidHistory.cshtml");
        }

        /// <summary>
        /// 获取标的操作历史
        /// </summary>
        /// <returns></returns>
        public JsonResult GetBidHistoryList()
        {
            AmtLimitListPara amtListPara = new AmtLimitListPara();
            //SIT-1130:操作历史目前没有分部门查询
            amtListPara.AMT_DEPT = Request["dept"];
            amtListPara.AMT_USE_DATE = Request["useDate"];
            amtListPara.KEY_VALUE = Request["keyWord"];
            amtListPara.CurrentPage = int.Parse(string.IsNullOrEmpty(Request["page"]) ? "1" : Request["page"].ToString());//如果为空，默认第一页
            amtListPara.Rows = int.Parse(string.IsNullOrEmpty(Request["rows"]) ? "20" : Request["rows"].ToString());//如果为空，默认20条
            amtListPara.AMT_ID = Request["amtId"];
            amtListPara.IsAdjusted = 0;
            string pageIndex = string.IsNullOrEmpty(Request["page"]) ? "1" : Request["page"];
            string pageSize = string.IsNullOrEmpty(Request["rows"]) ? "20" : Request["rows"];
            var result = quotaManageService.GetQuotaHistoryList(amtListPara);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
	}
}