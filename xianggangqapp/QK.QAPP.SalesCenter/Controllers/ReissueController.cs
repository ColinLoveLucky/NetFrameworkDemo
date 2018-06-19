/***********************
 * 作    者：刘云松
 * 创建时间：‎2014‎-10‎-19 ‏‎18:25:13
 * 作    用：补件页面
*****************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QK.QAPP.Entity;
using QK.QAPP.IServices;
using Microsoft.Practices.Unity;
using QK.QAPP.Global;

namespace QK.QAPP.SalesCenter.Controllers
{
    public class ReissueController : Controller
    {
        #region 属性

        [Dependency]
        public IApplyTableService enterOrderService { get; set; }

        [Dependency]
        public IQFUserService qfuserService { get; set; }

        #endregion

        public ActionResult Index()
        {
            //补件动作持续天数
            ViewData["Order_SD_AbidanceDay"] = GlobalSetting.Order_SD_AbidanceDay;

            //待补件状态
            ViewData["Order_SD_Status_Need"] = GlobalSetting.Order_SD_Status_Need;

            //已补件状态
            string strHad = string.Empty;
            foreach (KeyValuePair<string, string> kvHad in GlobalSetting.Order_SD_Status_Had)
            {
                strHad += kvHad.Key + "|";
            }
            ViewData["Order_SD_Status_Had"] = strHad;

            //补件失效状态
            string strCancel = string.Empty;
            foreach (KeyValuePair<string, string> kvCancel in GlobalSetting.Order_SD_Status_Cancel)
            {
                strCancel += kvCancel.Key + "|";
            }
            ViewData["Order_SD_Status_Cancel"] = strCancel;

            //按钮授权
            string url = Request.Url.PathAndQuery;
            List<APP_Button> listButtons = qfuserService.GetButtonByUrl(url);
            ViewData["Permission_Buttons"] = Newtonsoft.Json.JsonConvert.SerializeObject(listButtons);
            
            return View();
        }

        public ActionResult Ry100()
        {
            //补件动作持续天数
            ViewData["Order_SD_AbidanceDay"] = GlobalSetting.Order_SD_AbidanceDay;

            //待补件状态
            ViewData["Order_SD_Status_Need_Ry100"] = GlobalSetting.Order_SD_Status_Need_Ry100;

            //已补件状态
            string strHad = string.Empty;
            foreach (KeyValuePair<string, string> kvHad in GlobalSetting.Order_SD_Status_Had_Ry100)
            {
                strHad += kvHad.Key + "|";
            }
            ViewData["Order_SD_Status_Had_Ry100"] = strHad;

            //补件失效状态
            string strCancel = string.Empty;
            foreach (KeyValuePair<string, string> kvCancel in GlobalSetting.Order_SD_Status_Cancel_Ry100)
            {
                strCancel += kvCancel.Key + "|";
            }
            ViewData["Order_SD_Status_Cancel_Ry100"] = strCancel;

            //按钮授权
            string url = Request.Url.PathAndQuery;
            List<APP_Button> listButtons = qfuserService.GetButtonByUrl(url);
            ViewData["Permission_Buttons"] = Newtonsoft.Json.JsonConvert.SerializeObject(listButtons);

            return View();
        }
        
        /// <summary>
        /// 极客贷补件管理
        /// </summary>
        /// <returns></returns>
        public ActionResult Geek()
        {
            //补件动作持续天数
            ViewData["Order_SD_AbidanceDay"] = GlobalSetting.Order_SD_AbidanceDay;

            //待补件状态
            ViewData["Order_SD_Status_Need_Geek"] = GlobalSetting.Order_SD_Status_Need_Geek;

            //已补件状态
            string strHad = string.Empty;
            foreach (KeyValuePair<string, string> kvHad in GlobalSetting.Order_SD_Status_Had_Geek)
            {
                strHad += kvHad.Key + "|";
            }
            ViewData["Order_SD_Status_Had_Geek"] = strHad;

            //补件失效状态
            string strCancel = string.Empty;
            foreach (KeyValuePair<string, string> kvCancel in GlobalSetting.Order_SD_Status_Cancel_Geek)
            {
                strCancel += kvCancel.Key + "|";
            }
            ViewData["Order_SD_Status_Cancel_Geek"] = strCancel;

            //按钮授权
            string url = Request.Url.PathAndQuery;
            List<APP_Button> listButtons = qfuserService.GetButtonByUrl(url);
            ViewData["Permission_Buttons"] = Newtonsoft.Json.JsonConvert.SerializeObject(listButtons);

            return View();
        }
        public ActionResult Vehicle(string menuCode)
        {
            ViewData["menuCode"] = menuCode;
            //补件动作持续天数
            ViewData["Order_SD_AbidanceDay_Car"] = GlobalSetting.Order_SD_AbidanceDay_Car;
            //待补件状态
            ViewData["Order_SD_Status_Need_Car"] = GlobalSetting.Order_SD_Status_Need_Car;

            //已补件状态
            string strHad = string.Empty;
            foreach (KeyValuePair<string, string> kvHad in GlobalSetting.Order_SD_Status_Had_Car)
            {
                strHad += kvHad.Key + "|";
            }
            ViewData["Order_SD_Status_Had_Car"] = strHad;

            //按钮授权
            string url = Request.Url.PathAndQuery;
            List<APP_Button> listButtons = qfuserService.GetButtonByUrl(url);
            ViewData["Permission_Buttons"] = Newtonsoft.Json.JsonConvert.SerializeObject(listButtons);

            return View();
        }

        public ActionResult House()
        {
            //房贷补件有效天数
            ViewData["Order_SD_AbidanceDay_House"] = GlobalSetting.Order_SD_AbidanceDay_House;
            //房贷待补件状态
            ViewData["Order_SD_Status_Need_House"] = GlobalSetting.Order_SD_Status_Need_House;

            //房贷已补件状态
            string strHad = String.Empty;
            foreach (KeyValuePair<string, string> kvHad in GlobalSetting.Order_SD_Status_Had_House)
            {
                strHad += kvHad.Key + "|";
            }
            ViewData["Order_SD_Status_Had_House"] = strHad;

            //补件失效状态
            string strCancel = string.Empty;
            foreach (KeyValuePair<string, string> kvCancel in GlobalSetting.Order_SD_Status_Cancel_House)
            {
                strCancel += kvCancel.Key + "|";
            }
            ViewData["Order_SD_Status_Cancel_House"] = strCancel;

            //按钮授权
            string url = Request.Url.PathAndQuery;
            List<APP_Button> listButtons = qfuserService.GetButtonByUrl(url);
            ViewData["Permission_Buttons"] = Newtonsoft.Json.JsonConvert.SerializeObject(listButtons);

            return View();
        }

        [HttpPost]
        public JsonResult GetReissueList()
        {
            EnterListSearchPara para = new EnterListSearchPara();
            //数据权限
            para.AccessableCsac = qfuserService.GetDataPermission();
            para.PageIndex = int.Parse(Request["page"] ?? "0");
            para.PageSize = int.Parse(Request["rows"] ?? "0");
            para.Sort = new Dictionary<string, string>();
            para.Sort.Add(Request["sidx"] ?? string.Empty, Request["sord"] ?? string.Empty);
            //申请单号
            para.AppCode = HttpUtility.UrlDecode(Request["appCode"]);
            //客户姓名
            para.CustomerName = HttpUtility.UrlDecode(Request["customerName"]);
            //客户身份证
            para.CustomerIDCard = HttpUtility.UrlDecode(Request["customerIDCard"]);
            //申请时间开始
            if (Request["applyStart"] != null && Request["applyStart"].ToString() != string.Empty)
            {
                DateTime dtm = DateTime.Now;
                if (DateTime.TryParse(Request["applyStart"].ToString() + " 00:00:00", out dtm))
                    para.ApplyStart = dtm;
            }
            //申请时间结束
            if (Request["applyEnd"] != null && Request["applyEnd"].ToString() != string.Empty)
            {
                DateTime dtm = DateTime.Now;
                if (DateTime.TryParse(Request["applyEnd"].ToString() + " 23:59:59", out dtm))
                    para.ApplyEnd = dtm;
            }
            //进件状态
            if (Request["enterStasus"] != null && Request["enterStasus"].ToString() != string.Empty)
            {
                string strStatus = Request["enterStasus"].ToString();
                string[] aryStatus = strStatus.Split('|');
                foreach (string s in aryStatus)
                {
                    if (string.IsNullOrEmpty(s))
                        continue;
                    para.ListEnterStatus.Add((EnterStatusType)Enum.Parse(typeof(EnterStatusType), s));
                }
            }
            //如果没有选择进件状态，则默认查出所有补件
            else
            {
                foreach (KeyValuePair<string, string> kv in GlobalSetting.Order_SD_Status_Need)
                {
                    para.ListEnterStatus.Add((EnterStatusType)Enum.Parse(typeof(EnterStatusType), kv.Key));
                }
            }
            //客户经理
            para.SaleCode = HttpUtility.UrlDecode(Request["sales"]);
            para.SaleName = HttpUtility.UrlDecode(Request["sales"]);
            //客服
            para.CsacCode = HttpUtility.UrlDecode(Request["csac"]);
            para.CsacName = HttpUtility.UrlDecode(Request["csac"]);
            //是否是Search窗口的模糊查询
            if (Request["fuzzySearch"] != null && Request["fuzzySearch"].ToString() == "1")
            {
                para.FuzzySearch = true;
            }
            //是否是待补件列表的查询
            if (Request["needTag"] != null && Request["needTag"].ToString() == "1")
            {
                para.NeedTag = true;
            }
            else
            {
                para.NeedTag = false;
            }

            para.ListLogo = Global.GlobalSetting.LogoGroupForMenu["CREDIT"];
            
            EnterListViewFiledList ret;
            if (Request["searchSD"] != null && Request["searchSD"].ToString().ToUpper() == "TRUE")
            {
                ret = enterOrderService.GetEnterOrderListBySDStatus(para);
            }
            else
            {
                ret = enterOrderService.GetEnterOrderListByMainStatus(para);
            }
            return Json(ret, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetReissueListCar(string menuCode)
        {
            //menuCode
            //"VEHICLE":车贷部车贷
            //"GJVEHICLE":个金部车贷
            //截至到2016-3-17 两者都是从车贷拆分得到的，除产品名称和logo不一样外，其他都一样，走得流程和状态也一样
            menuCode = string.IsNullOrEmpty(menuCode) ? "VEHICLE" : menuCode;
            EnterListSearchPara para = new EnterListSearchPara();
            //数据权限
            para.AccessableCsac = qfuserService.GetDataPermission();
            para.PageIndex = int.Parse(Request["page"] ?? "0");
            para.PageSize = int.Parse(Request["rows"] ?? "0");
            para.Sort = new Dictionary<string, string>();
            para.Sort.Add(Request["sidx"] ?? string.Empty, Request["sord"] ?? string.Empty);
            //申请单号
            para.AppCode = HttpUtility.UrlDecode(Request["appCode"]);
            //客户姓名
            para.CustomerName = HttpUtility.UrlDecode(Request["customerName"]);
            //客户身份证
            para.CustomerIDCard = HttpUtility.UrlDecode(Request["customerIDCard"]);
            //申请时间开始
            if (Request["applyStart"] != null && Request["applyStart"].ToString() != string.Empty)
            {
                DateTime dtm = DateTime.Now;
                if (DateTime.TryParse(Request["applyStart"].ToString() + " 00:00:00", out dtm))
                    para.ApplyStart = dtm;
            }
            //申请时间结束
            if (Request["applyEnd"] != null && Request["applyEnd"].ToString() != string.Empty)
            {
                DateTime dtm = DateTime.Now;
                if (DateTime.TryParse(Request["applyEnd"].ToString() + " 23:59:59", out dtm))
                    para.ApplyEnd = dtm;
            }
            //进件状态
            if (Request["enterStasus"] != null && Request["enterStasus"].ToString() != string.Empty)
            {
                string strStatus = Request["enterStasus"].ToString();
                string[] aryStatus = strStatus.Split('|');
                foreach (string s in aryStatus)
                {
                    if (string.IsNullOrEmpty(s))
                        continue;
                    para.ListEnterStatus.Add((EnterStatusType)Enum.Parse(typeof(EnterStatusType), s));
                }
            }
            //如果没有选择进件状态，则默认查出所有补件
            else
            {
                foreach (KeyValuePair<string, string> kv in GlobalSetting.Order_SD_Status_Need_Car)
                {
                    para.ListEnterStatus.Add((EnterStatusType)Enum.Parse(typeof(EnterStatusType), kv.Key));
                }
            }
            //客户经理
            para.SaleCode = HttpUtility.UrlDecode(Request["sales"]);
            para.SaleName = HttpUtility.UrlDecode(Request["sales"]);
            //客服
            para.CsacCode = HttpUtility.UrlDecode(Request["csac"]);
            para.CsacName = HttpUtility.UrlDecode(Request["csac"]);
            //是否是Search窗口的模糊查询
            if (Request["fuzzySearch"] != null && Request["fuzzySearch"].ToString() == "1")
            {
                para.FuzzySearch = true;
            }
            //是否是待补件列表的查询
            if (Request["needTag"] != null && Request["needTag"].ToString() == "1")
            {
                para.NeedTag = true;
            }
            else
            {
                para.NeedTag = false;
            }

            para.ListLogo = Global.GlobalSetting.LogoGroupForMenu[menuCode];

            // 查询APP_AUTH表的MENUCODE，在从不同菜单查询相同logo产品时使用
            // lys 2016-3-30
            para.InputMenuCode = menuCode;

            EnterListViewFiledList ret;
            if (Request["searchSD"] != null && Request["searchSD"].ToString().ToUpper() == "TRUE")
            {
                ret = enterOrderService.GetEnterOrderListBySDStatus(para);
            }
            else
            {
                ret = enterOrderService.GetEnterOrderListByMainStatus(para);
            }
            return Json(ret, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetReissueListHouse()
        {
            EnterListSearchPara para = new EnterListSearchPara();
            //数据权限
            para.AccessableCsac = qfuserService.GetDataPermission();
            para.PageIndex = int.Parse(Request["page"] ?? "0");
            para.PageSize = int.Parse(Request["rows"] ?? "0");
            para.Sort = new Dictionary<string, string>();
            para.Sort.Add(Request["sidx"] ?? string.Empty, Request["sord"] ?? string.Empty);
            //申请单号
            para.AppCode = HttpUtility.UrlDecode(Request["appCode"]);
            //客户姓名
            para.CustomerName = HttpUtility.UrlDecode(Request["customerName"]);
            //客户身份证
            para.CustomerIDCard = HttpUtility.UrlDecode(Request["customerIDCard"]);
            //申请时间开始
            if (Request["applyStart"] != null && Request["applyStart"].ToString() != string.Empty)
            {
                DateTime dtm = DateTime.Now;
                if (DateTime.TryParse(Request["applyStart"].ToString() + " 00:00:00", out dtm))
                    para.ApplyStart = dtm;
            }
            //申请时间结束
            if (Request["applyEnd"] != null && Request["applyEnd"].ToString() != string.Empty)
            {
                DateTime dtm = DateTime.Now;
                if (DateTime.TryParse(Request["applyEnd"].ToString() + " 23:59:59", out dtm))
                    para.ApplyEnd = dtm;
            }
            //进件状态
            if (Request["enterStasus"] != null && Request["enterStasus"].ToString() != string.Empty)
            {
                string strStatus = Request["enterStasus"].ToString();
                string[] aryStatus = strStatus.Split('|');
                foreach (string s in aryStatus)
                {
                    if (string.IsNullOrEmpty(s))
                        continue;
                    para.ListEnterStatus.Add((EnterStatusType)Enum.Parse(typeof(EnterStatusType), s));
                }
            }
            //如果没有选择进件状态，则默认查出所有补件
            else
            {
                foreach (KeyValuePair<string, string> kv in GlobalSetting.Order_SD_Status_Need_House)
                {
                    para.ListEnterStatus.Add((EnterStatusType)Enum.Parse(typeof(EnterStatusType), kv.Key));
                }
            }
            //客户经理
            para.SaleCode = HttpUtility.UrlDecode(Request["sales"]);
            para.SaleName = HttpUtility.UrlDecode(Request["sales"]);
            //客服
            para.CsacCode = HttpUtility.UrlDecode(Request["csac"]);
            para.CsacName = HttpUtility.UrlDecode(Request["csac"]);
            //是否是Search窗口的模糊查询
            if (Request["fuzzySearch"] != null && Request["fuzzySearch"].ToString() == "1")
            {
                para.FuzzySearch = true;
            }
            //是否是待补件列表的查询
            if (Request["needTag"] != null && Request["needTag"].ToString() == "1")
            {
                para.NeedTag = true;
            }
            else
            {
                para.NeedTag = false;
            }

            para.ListLogo = Global.GlobalSetting.LogoGroupForMenu["HOUSE"];

            EnterListViewFiledList ret;
            if (Request["searchSD"] != null && Request["searchSD"].ToString().ToUpper() == "TRUE")
            {
                ret = enterOrderService.GetEnterOrderListBySDStatus(para);
            }
            else
            {
                ret = enterOrderService.GetEnterOrderListByMainStatus(para);
            }
            return Json(ret, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetReissueListGeek()
        {
            EnterListSearchPara para = new EnterListSearchPara();
            //数据权限
            para.AccessableCsac = qfuserService.GetDataPermission();
            para.PageIndex = int.Parse(Request["page"] ?? "0");
            para.PageSize = int.Parse(Request["rows"] ?? "0");
            para.Sort = new Dictionary<string, string>();
            para.Sort.Add(Request["sidx"] ?? string.Empty, Request["sord"] ?? string.Empty);
            //申请单号
            para.AppCode = HttpUtility.UrlDecode(Request["appCode"]);
            //客户姓名
            para.CustomerName = HttpUtility.UrlDecode(Request["customerName"]);
            //客户身份证
            para.CustomerIDCard = HttpUtility.UrlDecode(Request["customerIDCard"]);
            //申请时间开始
            if (Request["applyStart"] != null && Request["applyStart"].ToString() != string.Empty)
            {
                DateTime dtm = DateTime.Now;
                if (DateTime.TryParse(Request["applyStart"].ToString() + " 00:00:00", out dtm))
                    para.ApplyStart = dtm;
            }
            //申请时间结束
            if (Request["applyEnd"] != null && Request["applyEnd"].ToString() != string.Empty)
            {
                DateTime dtm = DateTime.Now;
                if (DateTime.TryParse(Request["applyEnd"].ToString() + " 23:59:59", out dtm))
                    para.ApplyEnd = dtm;
            }
            //进件状态
            if (Request["enterStasus"] != null && Request["enterStasus"].ToString() != string.Empty)
            {
                string strStatus = Request["enterStasus"].ToString();
                string[] aryStatus = strStatus.Split('|');
                foreach (string s in aryStatus)
                {
                    if (string.IsNullOrEmpty(s))
                        continue;
                    para.ListEnterStatus.Add((EnterStatusType)Enum.Parse(typeof(EnterStatusType), s));
                }
            }
            //如果没有选择进件状态，则默认查出所有补件
            else
            {
                foreach (KeyValuePair<string, string> kv in GlobalSetting.Order_SD_Status_Need_Geek)
                {
                    para.ListEnterStatus.Add((EnterStatusType)Enum.Parse(typeof(EnterStatusType), kv.Key));
                }
            }
            //客户经理
            para.SaleCode = HttpUtility.UrlDecode(Request["sales"]);
            para.SaleName = HttpUtility.UrlDecode(Request["sales"]);
            //客服
            para.CsacCode = HttpUtility.UrlDecode(Request["csac"]);
            para.CsacName = HttpUtility.UrlDecode(Request["csac"]);
            //是否是Search窗口的模糊查询
            if (Request["fuzzySearch"] != null && Request["fuzzySearch"].ToString() == "1")
            {
                para.FuzzySearch = true;
            }
            //是否是待补件列表的查询
            if (Request["needTag"] != null && Request["needTag"].ToString() == "1")
            {
                para.NeedTag = true;
            }
            else
            {
                para.NeedTag = false;
            }

            para.ListLogo = Global.GlobalSetting.LogoGroupForMenu["GEEK"];

            EnterListViewFiledList ret;
            if (Request["searchSD"] != null && Request["searchSD"].ToString().ToUpper() == "TRUE")
            {
                ret = enterOrderService.GetEnterOrderListBySDStatus(para);
            }
            else
            {
                ret = enterOrderService.GetEnterOrderListByMainStatus(para);
            }
            return Json(ret, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetReissueListRy()
        {
            EnterListSearchPara para = new EnterListSearchPara();
            //数据权限
            para.AccessableCsac = qfuserService.GetDataPermission();
            para.PageIndex = int.Parse(Request["page"] ?? "0");
            para.PageSize = int.Parse(Request["rows"] ?? "0");
            para.Sort = new Dictionary<string, string>();
            para.Sort.Add(Request["sidx"] ?? string.Empty, Request["sord"] ?? string.Empty);
            //申请单号
            para.AppCode = HttpUtility.UrlDecode(Request["appCode"]);
            //客户姓名
            para.CustomerName = HttpUtility.UrlDecode(Request["customerName"]);
            //客户身份证
            para.CustomerIDCard = HttpUtility.UrlDecode(Request["customerIDCard"]);
            //申请时间开始
            if (Request["applyStart"] != null && Request["applyStart"].ToString() != string.Empty)
            {
                DateTime dtm = DateTime.Now;
                if (DateTime.TryParse(Request["applyStart"].ToString() + " 00:00:00", out dtm))
                    para.ApplyStart = dtm;
            }
            //申请时间结束
            if (Request["applyEnd"] != null && Request["applyEnd"].ToString() != string.Empty)
            {
                DateTime dtm = DateTime.Now;
                if (DateTime.TryParse(Request["applyEnd"].ToString() + " 23:59:59", out dtm))
                    para.ApplyEnd = dtm;
            }
            //进件状态
            if (Request["enterStasus"] != null && Request["enterStasus"].ToString() != string.Empty)
            {
                string strStatus = Request["enterStasus"].ToString();
                string[] aryStatus = strStatus.Split('|');
                foreach (string s in aryStatus)
                {
                    if (string.IsNullOrEmpty(s))
                        continue;
                    para.ListEnterStatus.Add((EnterStatusType)Enum.Parse(typeof(EnterStatusType), s));
                }
            }
            //如果没有选择进件状态，则默认查出所有补件
            else
            {
                foreach (KeyValuePair<string, string> kv in GlobalSetting.Order_SD_Status_Need_Ry100)
                {
                    para.ListEnterStatus.Add((EnterStatusType)Enum.Parse(typeof(EnterStatusType), kv.Key));
                }
            }
            //客户经理
            para.SaleCode = HttpUtility.UrlDecode(Request["sales"]);
            para.SaleName = HttpUtility.UrlDecode(Request["sales"]);
            //客服
            para.CsacCode = HttpUtility.UrlDecode(Request["csac"]);
            para.CsacName = HttpUtility.UrlDecode(Request["csac"]);
            //是否是Search窗口的模糊查询
            if (Request["fuzzySearch"] != null && Request["fuzzySearch"].ToString() == "1")
            {
                para.FuzzySearch = true;
            }
            //是否是待补件列表的查询
            if (Request["needTag"] != null && Request["needTag"].ToString() == "1")
            {
                para.NeedTag = true;
            }
            else
            {
                para.NeedTag = false;
            }

            para.ListLogo = Global.GlobalSetting.LogoGroupForMenu["RY100"];

            // 查询APP_AUTH表的MENUCODE，在从不同菜单查询相同logo产品时使用
            // lys 2016-3-30
            para.InputMenuCode = "RY100";

            EnterListViewFiledList ret;
            if (Request["searchSD"] != null && Request["searchSD"].ToString().ToUpper() == "TRUE")
            {
                ret = enterOrderService.GetEnterOrderListBySDStatus(para);
            }
            else
            {
                ret = enterOrderService.GetEnterOrderListByMainStatus(para);
            }
            return Json(ret, JsonRequestBehavior.AllowGet);
        }
    }
}