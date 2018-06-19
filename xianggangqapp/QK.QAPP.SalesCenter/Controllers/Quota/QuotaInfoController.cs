using QK.QAPP.Entity;
using QK.QAPP.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QK.QAPP.SalesCenter.Controllers.Quota
{
    public class QuotaInfoController : Controller
    {
        public IQuotaManageService quotaManageService { get; set; }
        //
        // GET: /QuotaInfo/
        public ActionResult QuotaInfo()
        {
            //获取非工作日日期列表
            //财务要求，T日和T+1日要屏蔽非工作日
            var weekend = quotaManageService.GetWeekend();
            string tDate = DateTime.Now.Date.ToString();
            string t1Date = DateTime.Now.AddDays(1).ToString();
            /*20160507上线时有变更，此功能注释掉*/
            //quotaManageService.GetTiWorkDay(weekend, 0, out tDate);
            //quotaManageService.GetTiWorkDay(weekend, 1, out t1Date);
            Dictionary<string, List<QB_V_AMTLIMIT>> dic = new Dictionary<string, List<QB_V_AMTLIMIT>>() 
            { 
                {"T",quotaManageService.GetQuotaInfo(tDate)},
                {"T1",quotaManageService.GetQuotaInfo(t1Date)}
            };
            ViewBag.QuotaInfo = dic;
            return View("~/Views/Quota/QuotaInfo.cshtml");
        }
    }
}
