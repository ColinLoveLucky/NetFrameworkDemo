using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using QK.QAPP.IServices;
using QK.QAPP.Entity;

namespace QK.QAPP.SalesCenter.Controllers.Quota
{
    public class QuotaUsageController : Controller
    {
        public IQuotaUsageService quotaUsageService { get; set; }
        public IQuotaAssignService quotaAssignService { get; set; }
        //
        // GET: /QuotaUsage/
        public ActionResult QuotaUsage()
        {
            ViewBag.District = quotaAssignService.GetDistrict();
            return View("~/Views/Quota/QuotaUsage.cshtml");
        }

        public string GetQuotaUsage()
        {
            AmtAssignListPara para = new AmtAssignListPara() {  Area = Request["area"] };
            return quotaUsageService.GetAmtUseSummary(para);
        }
    }
}