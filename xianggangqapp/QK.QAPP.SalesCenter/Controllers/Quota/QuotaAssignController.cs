using QK.QAPP.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QK.QAPP.Entity;

namespace QK.QAPP.SalesCenter.Controllers.Quota
{
    public class QuotaAssignController : Controller
    {
        public IQuotaAssignService quotaAssignService { get; set; }
        public IQuotaManageService quotaManageService { get; set; }

        #region 01 额度分配管理
        //
        // GET: /QuotaAssign/
        public ActionResult QuotaAssign()
        {
            ViewBag.District = quotaAssignService.GetDistrict();
            return View("~/Views/Quota/QuotaAssign.cshtml");
        }

        public JsonResult GetQuotaAssignList()
        {
            AmtAssignListPara assignPara = new AmtAssignListPara();
            assignPara.Area = Request["area"];
            assignPara.UseDate = Request["useDate"];
            assignPara.KEY_VALUE = Request["keyWord"];
            assignPara.CurrentPage = int.Parse(string.IsNullOrEmpty(Request["page"]) ? "1" : Request["page"].ToString());//如果为空，默认第一页
            assignPara.Rows = int.Parse(string.IsNullOrEmpty(Request["rows"]) ? "20" : Request["rows"].ToString());//如果为空，默认20条
            var result = quotaAssignService.GetQuotaAssignList(assignPara);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region 02 新增分配
        public ActionResult QuotaAssignAdd()
        {
            //获取非工作日日期列表
            var weekend = quotaManageService.GetWeekend();
            //默认T+1排除非工作日
            string T1Date = DateTime.Now.AddDays(1).ToShortDateString();
            quotaManageService.GetTiWorkDay(weekend, 1, out T1Date);
            ViewData["T1Date"] = T1Date;
            //日历插件屏蔽非工作日，数据准备
            string json = "";
            weekend.ForEach(w => { json += w.WEEKEND_DATE.ToShortDateString() + ","; });
            ViewData["NonWorkDayJson"] = json.Remove(json.LastIndexOf(","));//移除最后一个 逗点

            ViewBag.District = quotaAssignService.GetDistrict();
            ViewBag.GlobalAvailableAmt = quotaAssignService.GetGlobalAvailableAmt(string.Empty).ToString("N");
            return View("~/Views/Quota/QuotaAssignAdd.cshtml");
        }
        public string AddQuotaAssign(QB_AMT_LIMIT_ASSIGN amtAssign)
        {
            var result = quotaAssignService.AddQuotaAssign(amtAssign);
            return result;
        }

        public ActionResult GetGlobalAvailableAmt(string dateTime)
        {
            var amt = quotaAssignService.GetGlobalAvailableAmt(dateTime).ToString("N");
            return Content(amt);
        }
        #endregion

        #region 03 调整分配
        public ActionResult QuotaAssignAdjust()
        {
            string id = Request["id"];
            var quotaAssign = quotaAssignService.GetAssignQuotaById(id);
            ViewBag.QuotaAssignInfo = quotaAssign;
            ViewBag.GlobalAvailableAmt = quotaAssignService.GetGlobalAvailableAmt(quotaAssign.AMT_ASSIGN_USE_DATE).ToString("N");
            return View("~/Views/Quota/QuotaAssignAdjust.cshtml");
        }

        public string AdjustQuotaAssign(string id, string adjustType, string assignAmt)
        {
            var result = quotaAssignService.AdjustQuotaAssign(id, adjustType, assignAmt);
            return result;
        }
        #endregion
    }
}