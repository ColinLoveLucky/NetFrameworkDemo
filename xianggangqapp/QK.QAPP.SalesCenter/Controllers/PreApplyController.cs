using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QK.QAPP.IServices;
using QK.QAPP.Entity;
using QK.QAPP.Global;
using QK.QAPP.Infrastructure;
using Microsoft.Practices.Unity;

namespace QK.QAPP.SalesCenter.Controllers
{
    public class PreApplyController : Controller
    {
        #region

        [Dependency]
        public IQFUserService QfUserService { get; set; }

        [Dependency]
        public IPreApplyService PreApplyService { get; set; }

        #endregion
        public ActionResult Index()
        {
            //获取本菜单的按钮权限
            string menuUrl = null;
            if (Request.Url != null)
            {
                menuUrl = Request.Url.PathAndQuery;
            }
            ViewData["Permission_Buttons"] = Newtonsoft.Json.JsonConvert.SerializeObject(QfUserService.GetButtonByUrl(menuUrl));
            return View();
        }
        public ActionResult PreCar(string menuCode)
        {
            ViewData["menuCode"] = menuCode;
            //获取本菜单的按钮权限
            string menuUrl = null;
            if (Request.Url != null)
            {
                menuUrl = Request.Url.PathAndQuery;
            }
            ViewData["Permission_Buttons"] = Newtonsoft.Json.JsonConvert.SerializeObject(QfUserService.GetButtonByUrl(menuUrl));
            return View();
        }
        public ActionResult PreGeek()
        {
            string menuUrl = null;
            if (Request.Url != null)
            {
                menuUrl = Request.Url.PathAndQuery;
            }
            ViewData["Permission_Buttons"] = Newtonsoft.Json.JsonConvert.SerializeObject(QfUserService.GetButtonByUrl(menuUrl));
            return View();
        }

        /// <summary>
        /// 弃用
        /// </summary>
        /// <returns></returns>
        public JsonResult GetPreApplyList()
        {
            PreEnterListSearchPara para = new PreEnterListSearchPara();
            //分页、排序参数
            if (Convert.ToString(Request["page"]) != null)
            {
                para.PageIndex = int.Parse(Convert.ToString(Request["page"]));
            }
            if (Convert.ToString(Request["rows"]) != null)
            {
                para.PageSize = int.Parse(Convert.ToString(Request["rows"]));
            }
            para.Sort = new Dictionary<string, string>();
            para.Sort.Add(Request["sidx"] ?? string.Empty, Request["sord"] ?? string.Empty);

            //申请单号
            para.PreAppCode = HttpUtility.UrlDecode(Request["preAppCode"]);
            //客户姓名
            para.CustomerName = HttpUtility.UrlDecode(Request["customerName"]);
            //客户经理
            para.SaleCode = HttpUtility.UrlDecode(Request["sales"]);
            para.SaleName = HttpUtility.UrlDecode(Request["sales"]);
            para.CustomerMobile = HttpUtility.UrlDecode(Request["customerMobile"]);
            //是否是Search窗口的模糊查询
            if (Request["fuzzySearch"] != null && Request["fuzzySearch"].ToString() == "1")
            {
                para.FuzzySearch = true;
            }
            para.ListEnterStatus.Add(PreEnterStatusType.PRE_APPROK);

            PreEnterListViewFiledList result = PreApplyService.GetPreApplyList(para);

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 获取信贷预申请列表
        /// </summary>
        /// <returns></returns>
        public JsonResult GetPreCreditApplyList()
        {
            PreEnterListViewFiledList result = PreApplyService.GetPreApplyByList(InitPreEnterListSearchPara(Global.GlobalSetting.LogoGroupForMenu["CREDIT"]));
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 获取车贷预申请列表
        /// </summary>
        /// <returns></returns>
        public JsonResult GetPreCarApplyList(string menuCode)
        {
            //menuCode
            //"VEHICLE":车贷部车贷
            //"GJVEHICLE":个金部车贷
            //截至到2016-3-17 两者都是从车贷拆分得到的，除产品名称和logo不一样外，其他都一样，走得流程和状态也一样
            menuCode = string.IsNullOrEmpty(menuCode) ? "VEHICLE" : menuCode;
            PreEnterListViewFiledList result = PreApplyService.GetPreApplyByList(InitPreEnterListSearchPara(Global.GlobalSetting.LogoGroupForMenu[menuCode]));
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 极客贷申请列表
        /// </summary>
        /// <returns></returns>
        public JsonResult GetPreGeekApplyList()
        {
            PreEnterListViewFiledList result = PreApplyService.GetPreApplyByList(InitPreEnterListSearchPara(Global.GlobalSetting.LogoGroupForMenu["GEEK"]));
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 初始化基础查询条件
        /// </summary>
        /// <returns></returns>
        private PreEnterListSearchPara InitPreEnterListSearchPara(List<string> logo)
        {
            PreEnterListSearchPara para = new PreEnterListSearchPara();
            //分页、排序参数
            if (Convert.ToString(Request["page"]) != null)
            {
                para.PageIndex = int.Parse(Convert.ToString(Request["page"]));
            }
            if (Convert.ToString(Request["rows"]) != null)
            {
                para.PageSize = int.Parse(Convert.ToString(Request["rows"]));
            }
            para.Sort = new Dictionary<string, string>();
            para.Sort.Add(Request["sidx"] ?? string.Empty, Request["sord"] ?? string.Empty);

            //申请单号
            para.PreAppCode = HttpUtility.UrlDecode(Request["preAppCode"]);
            //客户姓名
            para.CustomerName = HttpUtility.UrlDecode(Request["customerName"]);
            //客户经理
            para.SaleCode = HttpUtility.UrlDecode(Request["sales"]);
            para.SaleName = HttpUtility.UrlDecode(Request["sales"]);
            para.CustomerMobile = HttpUtility.UrlDecode(Request["customerMobile"]);
            //是否是Search窗口的模糊查询
            if (Request["fuzzySearch"] != null && Request["fuzzySearch"].ToString() == "1")
            {
                para.FuzzySearch = true;
            }
            para.ListEnterStatus.Add(PreEnterStatusType.PRE_APPROK);
            if (logo != null)
            {
                para.ListLogo = logo;
            }
            return para;
        }
    }
}