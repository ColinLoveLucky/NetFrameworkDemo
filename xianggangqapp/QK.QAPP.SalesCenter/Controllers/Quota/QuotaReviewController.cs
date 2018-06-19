using Microsoft.Practices.Unity;
using QK.QAPP.Entity.QbEntity;
using QK.QAPP.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QK.QAPP.SalesCenter.Controllers.Quota
{
    public class QuotaReviewController : Controller
    {
        [Dependency]
        public IQuotaManageService quotaManageService { get; set; }
        //
        // GET: /QuotaReview/
        public ActionResult QuotaReview()
        {
            ViewData["QuotaType"] = quotaManageService.GetQuotaType().Where(p => p.LEVEL_NO == 1).ToDictionary(k => k.AMT_TYPE, v => v.AMT_NAME);
            return View("~/Views/Quota/QuotaReview.cshtml");
        }
        /// <summary>
        /// 获取复核列表
        /// </summary>
        /// <returns></returns>
        public JsonResult GetQuotaListForReCheck()
        {
            AmtLimitListPara amtListPara = new AmtLimitListPara();
            amtListPara.AMT_TYPE = Request["amtType"];
            amtListPara.KEY_VALUE = Request["keyWord"];
            amtListPara.AMT_USE_DATE = Request["useDate"];
            amtListPara.IsAdjusted = 1;
            amtListPara.CurrentPage = int.Parse(string.IsNullOrEmpty(Request["page"]) ? "1" : Request["page"].ToString());//如果为空，默认第一页
            amtListPara.Rows = int.Parse(string.IsNullOrEmpty(Request["rows"]) ? "20" : Request["rows"].ToString());//如果为空，默认20条
            var result = quotaManageService.GetQuotaManageList(amtListPara);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 复核操作
        /// </summary>
        /// <param name="id">额度主键</param>
        /// <returns></returns>
        public string ReCheck(string id)
        {
            var result = quotaManageService.QuotaReCheck(id);
            return result;
        }

        /// <summary>
        /// 复核操作（批量）
        /// </summary>
        /// <param name="idList"></param>
        /// <returns></returns>
        [HttpPost]
        public string ReCheckBatch(string idList)
        {
            var result = quotaManageService.QuotaReCheckBatch(idList);
            return result;
        }
    }
}