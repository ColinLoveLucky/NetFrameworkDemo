using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Practices.Unity;
using QK.QAPP.Entity;
using QK.QAPP.IServices;

namespace QK.QAPP.SalesCenter.Controllers
{
    public class CompanyController : Controller
    {
        [Dependency]
        public IAPP_AEOSERVICE AeoService { get; set; }

        public ActionResult CompanyPick()
        {
            return View();
        }

         [HttpPost]
        public JsonResult GetCompanyList(string aeoType, string keyWord)
         {
             int pageIndex = 0, pageSize = 0;
             string type = HttpUtility.UrlDecode(aeoType);
             string key = HttpUtility.UrlDecode(keyWord);
             if (Convert.ToString(Request["page"]) != null)
             {
                 pageIndex = int.Parse(Convert.ToString(Request["page"]));
             }
             if (Convert.ToString(Request["rows"]) != null)
             {
                 pageSize = int.Parse(Convert.ToString(Request["rows"]));
             }
             ViewListByPage<APP_AEO> aeoList = AeoService.GetCompanyList(type, key, pageIndex, pageSize);
             return Json(aeoList, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ListByCategory(string aeoType, string keyWord)
        {
            int pageIndex = 0, pageSize = 0;
            string type = HttpUtility.UrlDecode(aeoType);
            string key = HttpUtility.UrlDecode(keyWord);
            if (Convert.ToString(Request["page"]) != null)
            {
                pageIndex = int.Parse(Convert.ToString(Request["page"]));
            }
            if (Convert.ToString(Request["rows"]) != null)
            {
                pageSize = int.Parse(Convert.ToString(Request["rows"]));
            }
            ViewListByPage<APP_AEO> aeoList = AeoService.ListByCategory(type, key, pageIndex, pageSize);
            return Json(aeoList, JsonRequestBehavior.AllowGet);
        }
    }
}