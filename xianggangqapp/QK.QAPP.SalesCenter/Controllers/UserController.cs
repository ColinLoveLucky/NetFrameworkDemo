using Microsoft.Practices.Unity;
using QK.QAPP.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QK.QAPP.SalesCenter.Controllers
{
    public class UserController : Controller
    {
        [Dependency]
        public IStaffPickService StaffPickService { get; set; }

        //
        // GET: /User/
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult StaffPick()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetStaffUnit(string parent, string roleid, string companyId)
        {
            return Json(StaffPickService.GetUnit(parent, roleid, companyId), JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult SeachStaff(string keyWord, string companyid, string roleid)
        {
            var kw = HttpUtility.UrlDecode(keyWord);
            return Json(StaffPickService.GetStaffByKeyWord(keyWord, roleid, companyid), JsonRequestBehavior.AllowGet);
        }
    }
}