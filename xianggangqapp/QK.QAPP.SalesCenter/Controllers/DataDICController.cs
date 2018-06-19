using QK.QAPP.Infrastructure;
using QK.QAPP.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QK.QAPP.SalesCenter.Controllers
{
    public class DataDICController : Controller
    {
        ICR_DATA_DICService dicService = Ioc.GetService<ICR_DATA_DICService>();

        /// <summary>
        /// 通过 DATA_CODE 查询数据字典
        /// </summary>
        /// <param name="DATA_CODE"></param>
        /// <returns></returns>
        public JsonResult GetDICByParentCode(string DATA_CODE)
        {
            var dicList = dicService.GetDICByParentCode(DATA_CODE).Select(c => new
            {
                c.DATA_CODE,
                c.DATA_NAME
            }).ToList();

            return Json(dicList, JsonRequestBehavior.AllowGet);

        }

        /// <summary>
        /// 通过 PARENT_ID 查询数据字典
        /// </summary>
        /// <param name="PARENT_ID"></param>
        /// <returns></returns>
        public JsonResult GetDICByParentID(long PARENT_ID)
        {
            var dicList = dicService.GeDICListByParentID(PARENT_ID).Select(c => new
            {
                c.DATA_CODE,
                c.DATA_NAME
            }).ToList();

            return Json(dicList, JsonRequestBehavior.AllowGet);
        }
	}
}