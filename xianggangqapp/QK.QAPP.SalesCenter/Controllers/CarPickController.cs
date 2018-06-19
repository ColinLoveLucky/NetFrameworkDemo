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
    public class CarPickController : Controller
    {
        [Dependency]
        public IAPP_CARPRICESERVICE CarPriceService { get; set; }

        /// <summary>
        /// 车辆选择页面（iframe）
        /// </summary>
        /// <returns></returns>
        public ActionResult CarPick()
        {
            return View();
        }

        /// <summary>
        /// 分页获取车辆列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetCarList()
        {
            CarListSearchPara searchPara = new CarListSearchPara();
            int pageIndex = 0, pageSize = 0;
            searchPara.CarBrand = HttpUtility.UrlDecode(Request["carBrand"]);
            searchPara.CarSeries = HttpUtility.UrlDecode(Request["carSeries"]);
            searchPara.CarStyle = HttpUtility.UrlDecode(Request["carStyle"]);

            pageIndex = int.Parse(Request["page"] ?? "1");
            pageSize = int.Parse(Request["rows"] ?? "1");

            ViewListByPage<APP_CARPRICE> aeoList = CarPriceService.GetCarList(searchPara, pageIndex, pageSize);
            return Json(aeoList, JsonRequestBehavior.AllowGet);
        }
    }
}