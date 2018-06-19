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
    public class CarAgencyController : Controller
    {
        [Dependency]
        public IAPP_CARAGENCYSERVICE CarAgencyService{get;set;}
        public ActionResult CarAgencyPicker()
        {
            return View();
        }

        /// <summary>
        /// 获取汽车经销商
        /// </summary>
        /// <param name="keyWord">查询关键字</param>
        /// <param name="product">产品logo</param>
        /// <returns></returns>
        public JsonResult GetCarAgencyList(string keyWord,string product)
        {
            int pageIndex = 0, pageSize = 0;
            string key = HttpUtility.UrlDecode(keyWord);
            if (Convert.ToString(Request["page"]) != null)
            {
                pageIndex = int.Parse(Convert.ToString(Request["page"]));
            }
            if (Convert.ToString(Request["rows"]) != null)
            {
                pageSize = int.Parse(Convert.ToString(Request["rows"]));
            }
            ViewListByPage<APP_CARAGENCY> carAgencyList = CarAgencyService.GetCarAgencyList(product,keyWord, pageIndex, pageSize);
            return Json(carAgencyList, JsonRequestBehavior.AllowGet);
        }
    }
}