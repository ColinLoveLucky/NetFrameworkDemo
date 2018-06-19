using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using QK.QAPP.Entity;
using QK.QAPP.IServices;

namespace QK.QAPP.SalesCenter.Controllers
{
    public class QuantController : Controller
    {
        public IAPP_QUANTSERVICE AppQuantService { get; set; }
        // GET: Quant
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public string QuantImport()
        {
            var resultObj = AppQuantService.ImportData(Request);
            return JsonConvert.SerializeObject(resultObj);
        }

        [HttpPost]
        public JsonResult Handle()
        {
            var resultObj = AppQuantService.HandleQuantData();
            return Json(resultObj);
        }

        public JsonResult HandleInfo()
        {
            var resultObj = AppQuantService.HandleInfo();
            return Json(resultObj, JsonRequestBehavior.AllowGet);
        }
    }
}