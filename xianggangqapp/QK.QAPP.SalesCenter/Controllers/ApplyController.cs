using Microsoft.Practices.Unity;
using QK.QAPP.IServices;
using QK.QAPP.Entity;
using QK.QAPP.Global;
using QK.QAPP.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;

namespace QK.QAPP.SalesCenter.Controllers
{
    /// <summary>
    /// 贷款申请
    /// </summary>
    public class ApplyController : Controller
    {
        [Dependency]
        public IApplyTableService EnterOrderService { get; set; }

        [Dependency]
        public IQFUserService QfUserService { get; set; }

        [Dependency]
        public ILoanApplicationService LoanAppService { get; set; }

        // GET: /Apply/
        public ActionResult Index()
        {
            ViewData["Order_ExceptSD_Status"] = GlobalSetting.Order_ExceptSD_Status;
            //获取本菜单的按钮权限
            string menuUrl = null;
            if (Request.Url != null)
            {
                menuUrl = Request.Url.PathAndQuery;
            }
            ViewData["Permission_Buttons_listObj"] = QfUserService.GetButtonByUrl(menuUrl);
            ViewData["Permission_Buttons"] = Newtonsoft.Json.JsonConvert.SerializeObject(QfUserService.GetButtonByUrl(menuUrl));
            return View();
        }
        /// <summary>
        /// 极客贷申请管理
        /// </summary>
        /// <returns></returns>
        public ActionResult Geek()
        {
            ViewData["Order_ExceptSD_Status_Geek"] = GlobalSetting.Order_ExceptSD_Status_Geek;
            //获取本菜单的按钮权限
            string menuUrl = null;
            if (Request.Url != null)
            {
                menuUrl = Request.Url.PathAndQuery;
            }
            ViewData["Permission_Buttons_listObj"] = QfUserService.GetButtonByUrl(menuUrl);
            ViewData["Permission_Buttons"] = Newtonsoft.Json.JsonConvert.SerializeObject(QfUserService.GetButtonByUrl(menuUrl));
            return View();
        }

        public ActionResult Ry100()
        {
            ViewData["Order_ExceptSD_Status_Ry100"] = GlobalSetting.Order_ExceptSD_Status_Ry100;
            string menuUrl = null;
            if (Request.Url != null)
            {
                menuUrl = Request.Url.PathAndQuery;
            }
            ViewData["Permission_Buttons_listObj"] = QfUserService.GetButtonByUrl(menuUrl);
            ViewData["Permission_Buttons"] = Newtonsoft.Json.JsonConvert.SerializeObject(QfUserService.GetButtonByUrl(menuUrl));
            return View();
        }

        // GET: /Apply/
        public ActionResult Vehicle(string menuCode)
        {
            ViewData["menuCode"] = menuCode;

            ViewBag.Url = menuCode == "CXVEHICLE" ? "CXCarApplication" : "GJCarApplication";

            ViewData["Order_ExceptSD_Status"] = GlobalSetting.Order_ExceptSD_Status_Car;
            //获取本菜单的按钮权限
            string menuUrl = null;
            if (Request.Url != null)
            {
                menuUrl = Request.Url.PathAndQuery;
            }
            ViewData["Permission_Buttons_listObj"] = QfUserService.GetButtonByUrl(menuUrl);
            ViewData["Permission_Buttons"] = Newtonsoft.Json.JsonConvert.SerializeObject(QfUserService.GetButtonByUrl(menuUrl));
            return View();
        }

        //房贷申请管理页面
        public ActionResult House()
        {
            ViewData["Order_ExceptSD_Status"] = GlobalSetting.Order_ExceptSD_Status_House;
            //获取本菜单的按钮权限
            string menuUrl = null;
            if (Request.Url != null)
            {
                menuUrl = Request.Url.PathAndQuery;
            }
            ViewData["Permission_Buttons_listObj"] = QfUserService.GetButtonByUrl(menuUrl);
            ViewData["Permission_Buttons"] = Newtonsoft.Json.JsonConvert.SerializeObject(QfUserService.GetButtonByUrl(menuUrl));
            return View();
        }

        [HttpPost]
        public JsonResult GetApplyTableListRy()
        {
            EnterListViewFiledList ret = SetApplyList("RY100", GlobalSetting.Order_ExceptSD_Status_Ry100, LoanAppService.IsDisplayRefuseLoanRy);
            return Json(ret, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetApplyTableList()
        {
            EnterListSearchPara para = new EnterListSearchPara();
            //数据权限
            para.AccessableCsac = QfUserService.GetDataPermission();
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
            para.AppCode = HttpUtility.UrlDecode(Request["appCode"]);
            //客户姓名
            para.CustomerName = HttpUtility.UrlDecode(Request["customerName"]);
            //客户身份证号
            para.CustomerIDCard = HttpUtility.UrlDecode(Request["customerIDCard"]);
            //申请时间开始
            if (Request["applyStart"] != null && Convert.ToString(Request["applyStart"]) != string.Empty)
            {
                string applyStart = Convert.ToString(Request["applyStart"]) + " 00:00:00";
                para.ApplyStart = applyStart.ToDateTime();
            }
            //申请时间结束
            if (Request["applyEnd"] != null && Convert.ToString(Request["applyEnd"]) != string.Empty)
            {
                string applyEnd = Convert.ToString(Request["applyEnd"]) + " 23:59:59";
                para.ApplyEnd = applyEnd.ToDateTime();
            }
            //进件状态
            if (Request["enterStasus"] != null && Convert.ToString(Request["enterStasus"]) != string.Empty)
            {
                string[] aryTemp = Convert.ToString(Request["enterStasus"]).Split(',');
                foreach (string s in aryTemp)
                {
                    if (!string.IsNullOrWhiteSpace(s))
                    {
                        para.ListEnterStatus.Add((EnterStatusType)Enum.Parse(typeof(EnterStatusType), s));
                    }
                }

            }
            //如果没有选择进件状态，则默认查出所有进件状态
            else
            {
                foreach (KeyValuePair<string, string> kv in GlobalSetting.Order_ExceptSD_Status)
                {
                    string[] aryTemp = kv.Key.Split(',');
                    foreach (string s in aryTemp)
                    {
                        if (!string.IsNullOrWhiteSpace(s))
                        {
                            para.ListEnterStatus.Add((EnterStatusType)Enum.Parse(typeof(EnterStatusType), s));
                        }
                    }
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
            para.ListLogo = Global.GlobalSetting.LogoGroupForMenu["CREDIT"];

            EnterListViewFiledList ret = EnterOrderService.GetEnterOrderListByMainStatus(para);
            foreach (var item in ret.ListEnter)
            {
                foreach (KeyValuePair<string, string> kv in GlobalSetting.Order_ExceptSD_Status)
                {
                    string[] statuName = kv.Value.Split(',');
                    if (!string.IsNullOrWhiteSpace(kv.Key) && kv.Key.Contains(item.APPSTATUS))
                    {
                        string[] statuCode = kv.Key.Split(',');
                        foreach (var status in statuCode)
                        {
                            if (status == item.APPSTATUS)
                            {
                                if (!string.IsNullOrWhiteSpace(statuName[1]))
                                {
                                    /*item.APPSTATUSNAME = "<span class='label label-" + statuName[1] + "'>" + statuName[0] + "</span>";  //V2*/
                                    /*描述：如果是拒贷申请并且有权限，在办理状态加入下划线<u>标签,记录appid
                                      时间：2015-03-11
                                      修改者：leiz*/
                                    if (LoanAppService.IsDisplayRefuseLoan(status))
                                    {
                                        item.APPSTATUSNAME = "<span class='label label-" + statuName[1] + " tooltip-" + statuName[1] + " refuseLoanStatus' data-appid=" + item.APPID + "><u>" + statuName[0] + "</u></span>";
                                    }
                                    else
                                    {
                                        item.APPSTATUSNAME = "<span class='label label-" + statuName[1] + "'>" + statuName[0] + "</span>";
                                    }
                                }
                                else
                                {
                                    item.APPSTATUSNAME = "<span class='label label-info'>" + statuName[0] + "</span>";
                                }
                            }
                        }

                    }
                }
            }
            return Json(ret, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 极客贷申请管理
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetApplyTableListGeek()
        {
            EnterListViewFiledList ret = SetApplyList("GEEK", GlobalSetting.Order_ExceptSD_Status_Geek, LoanAppService.IsDisplayRefuseLoanGeek);
            //foreach (var item in ret.ListEnter)
            //{
            //    foreach (KeyValuePair<string, string> kv in GlobalSetting.Order_ExceptSD_Status_Geek)
            //    {
            //        string[] statuName = kv.Value.Split(',');
            //        if (!string.IsNullOrWhiteSpace(kv.Key) && kv.Key.Contains(item.APPSTATUS))
            //        {
            //            string[] statuCode = kv.Key.Split(',');
            //            foreach (var status in statuCode)
            //            {
            //                if (status == item.APPSTATUS)
            //                {
            //                    if (!string.IsNullOrWhiteSpace(statuName[1]))
            //                    {
            //                        /*item.APPSTATUSNAME = "<span class='label label-" + statuName[1] + "'>" + statuName[0] + "</span>";  //V2*/
            //                        /*描述：如果是拒贷申请并且有权限，在办理状态加入下划线<u>标签,记录appid
            //                          时间：2015-03-11
            //                          修改者：leiz*/
            //                        if (LoanAppService.IsDisplayRefuseLoanGeek(status))
            //                        {
            //                            item.APPSTATUSNAME = "<span class='label label-" + statuName[1] + " tooltip-" + statuName[1] + " refuseLoanStatus' data-appid=" + item.APPID + "><u>" + statuName[0] + "</u></span>";
            //                        }
            //                        else
            //                        {
            //                            item.APPSTATUSNAME = "<span class='label label-" + statuName[1] + "'>" + statuName[0] + "</span>";
            //                        }
            //                    }
            //                    else
            //                    {
            //                        item.APPSTATUSNAME = "<span class='label label-info'>" + statuName[0] + "</span>";
            //                    }
            //                }
            //            }

            //        }
            //    }
            //}
            return Json(ret, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 极客贷申请管理列表
        /// </summary>
        /// <param name="applyType"></param>
        /// <param name="orderExceptSdStatus"></param>
        /// <returns></returns>
        public EnterListViewFiledList SetApplyListGeek(string applyType, Dictionary<string, string> orderExceptSdStatus)
        {

            EnterListSearchPara para = new EnterListSearchPara();
            //数据权限
            para.AccessableCsac = QfUserService.GetDataPermission();
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
            para.AppCode = HttpUtility.UrlDecode(Request["appCode"]);
            //客户姓名
            para.CustomerName = HttpUtility.UrlDecode(Request["customerName"]);
            //客户身份证号
            para.CustomerIDCard = HttpUtility.UrlDecode(Request["customerIDCard"]);
            //申请时间开始
            if (Request["applyStart"] != null && Convert.ToString(Request["applyStart"]) != string.Empty)
            {
                string applyStart = Convert.ToString(Request["applyStart"]) + " 00:00:00";
                para.ApplyStart = applyStart.ToDateTime();
            }
            //申请时间结束
            if (Request["applyEnd"] != null && Convert.ToString(Request["applyEnd"]) != string.Empty)
            {
                string applyEnd = Convert.ToString(Request["applyEnd"]) + " 23:59:59";
                para.ApplyEnd = applyEnd.ToDateTime();
            }
            //进件状态
            if (Request["enterStasus"] != null && Convert.ToString(Request["enterStasus"]) != string.Empty)
            {
                string[] aryTemp = Convert.ToString(Request["enterStasus"]).Split(',');
                foreach (string s in aryTemp)
                {
                    if (!string.IsNullOrWhiteSpace(s))
                    {
                        para.ListEnterStatus.Add((EnterStatusType)Enum.Parse(typeof(EnterStatusType), s));
                    }
                }

            }
            //如果没有选择进件状态，则默认查出所有进件状态
            else
            {
                foreach (KeyValuePair<string, string> kv in orderExceptSdStatus)
                {
                    string[] aryTemp = kv.Key.Split(',');
                    foreach (string s in aryTemp)
                    {
                        if (!string.IsNullOrWhiteSpace(s))
                        {
                            para.ListEnterStatus.Add((EnterStatusType)Enum.Parse(typeof(EnterStatusType), s));
                        }
                    }
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
            para.ListLogo = Global.GlobalSetting.LogoGroupForMenu[applyType];

            EnterListViewFiledList ret = EnterOrderService.GetEnterOrderListByMainStatus(para);
            return ret;
        }
        /// <summary>
        /// 房贷申请列表获取
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetApplyTableListHouse()
        {
            EnterListViewFiledList ret = SetApplyList("HOUSE", GlobalSetting.Order_ExceptSD_Status_House, LoanAppService.IsDisplayRefuseLoanHouse);
            return Json(ret, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取申请管理列表数据
        /// </summary>
        /// <param name="applyType"></param>
        /// <param name="orderExceptSdStatus"></param>
        /// <param name="isDisplayRefuseFunc">是否显示拒贷码</param>
        /// <returns></returns>
        private EnterListViewFiledList SetApplyList(string applyType, Dictionary<string, string> orderExceptSdStatus,Func<string,bool> isDisplayRefuseFunc)
        {
            EnterListSearchPara para = new EnterListSearchPara();
            //数据权限
            para.AccessableCsac = QfUserService.GetDataPermission();
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
            para.AppCode = HttpUtility.UrlDecode(Request["appCode"]);
            //客户姓名
            para.CustomerName = HttpUtility.UrlDecode(Request["customerName"]);
            //客户身份证号
            para.CustomerIDCard = HttpUtility.UrlDecode(Request["customerIDCard"]);
            //申请时间开始
            if (Request["applyStart"] != null && Convert.ToString(Request["applyStart"]) != string.Empty)
            {
                string applyStart = Convert.ToString(Request["applyStart"]) + " 00:00:00";
                para.ApplyStart = applyStart.ToDateTime();
            }
            //申请时间结束
            if (Request["applyEnd"] != null && Convert.ToString(Request["applyEnd"]) != string.Empty)
            {
                string applyEnd = Convert.ToString(Request["applyEnd"]) + " 23:59:59";
                para.ApplyEnd = applyEnd.ToDateTime();
            }
            //进件状态
            if (Request["enterStasus"] != null && Convert.ToString(Request["enterStasus"]) != string.Empty)
            {
                string[] aryTemp = Convert.ToString(Request["enterStasus"]).Split(',');
                foreach (string s in aryTemp)
                {
                    if (!string.IsNullOrWhiteSpace(s))
                    {
                        para.ListEnterStatus.Add((EnterStatusType)Enum.Parse(typeof(EnterStatusType), s));
                    }
                }

            }
            //如果没有选择进件状态，则默认查出所有进件状态
            else
            {
                foreach (KeyValuePair<string, string> kv in orderExceptSdStatus)
                {
                    string[] aryTemp = kv.Key.Split(',');
                    foreach (string s in aryTemp)
                    {
                        if (!string.IsNullOrWhiteSpace(s))
                        {
                            para.ListEnterStatus.Add((EnterStatusType)Enum.Parse(typeof(EnterStatusType), s));
                        }
                    }
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
            para.ListLogo = Global.GlobalSetting.LogoGroupForMenu[applyType];

            EnterListViewFiledList ret = EnterOrderService.GetEnterOrderListByMainStatus(para);
            foreach (var item in ret.ListEnter)
            {
                foreach (KeyValuePair<string, string> kv in orderExceptSdStatus)
                {
                    string[] statuName = kv.Value.Split(',');
                    if (!string.IsNullOrWhiteSpace(kv.Key) && kv.Key.Contains(item.APPSTATUS))
                    {
                        string[] statuCode = kv.Key.Split(',');
                        foreach (var status in statuCode)
                        {
                            if (status == item.APPSTATUS)
                            {
                                if (!string.IsNullOrWhiteSpace(statuName[1]))
                                {
                                    /*item.APPSTATUSNAME = "<span class='label label-" + statuName[1] + "'>" + statuName[0] + "</span>";  //V2*/
                                    /*描述：如果是拒贷申请并且有权限，在办理状态加入下划线<u>标签,记录appid
                                      时间：2015-03-11
                                      修改者：leiz*/
                                    if (isDisplayRefuseFunc(status))
                                    {
                                        item.APPSTATUSNAME = "<span class='label label-" + statuName[1] + " tooltip-" + statuName[1] + " refuseLoanStatus' data-appid=" + item.APPID + "><u>" + statuName[0] + "</u></span>";
                                    }
                                    else
                                    {
                                        item.APPSTATUSNAME = "<span class='label label-" + statuName[1] + "'>" + statuName[0] + "</span>";
                                    }
                                }
                                else
                                {
                                    item.APPSTATUSNAME = "<span class='label label-info'>" + statuName[0] + "</span>";
                                }
                            }
                        }

                    }
                }
            }
            return ret;
        }

        public JsonResult GetApplyTableListCar(string menuCode)
        {
            //menuCode
            //"VEHICLE":车贷部车贷
            //"GJVEHICLE":个金部车贷
            //截至到2016-3-17 两者都是从车贷拆分得到的，除产品名称和logo不一样外，其他都一样，走得流程和状态也一样
            menuCode = string.IsNullOrEmpty(menuCode) ? "VEHICLE" : menuCode;

            EnterListSearchPara para = new EnterListSearchPara();
            //数据权限
            para.AccessableCsac = QfUserService.GetDataPermission();
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
            para.AppCode = HttpUtility.UrlDecode(Request["appCode"]);
            //客户姓名
            para.CustomerName = HttpUtility.UrlDecode(Request["customerName"]);
            //客户身份证号
            para.CustomerIDCard = HttpUtility.UrlDecode(Request["customerIDCard"]);
            //申请时间开始
            if (Request["applyStart"] != null && Convert.ToString(Request["applyStart"]) != string.Empty)
            {
                string applyStart = Convert.ToString(Request["applyStart"]) + " 00:00:00";
                para.ApplyStart = applyStart.ToDateTime();
            }
            //申请时间结束
            if (Request["applyEnd"] != null && Convert.ToString(Request["applyEnd"]) != string.Empty)
            {
                string applyEnd = Convert.ToString(Request["applyEnd"]) + " 23:59:59";
                para.ApplyEnd = applyEnd.ToDateTime();
            }
            //进件状态
            if (Request["enterStasus"] != null && Convert.ToString(Request["enterStasus"]) != string.Empty)
            {
                string[] aryTemp = Convert.ToString(Request["enterStasus"]).Split(',');
                foreach (string s in aryTemp)
                {
                    if (!string.IsNullOrWhiteSpace(s))
                    {
                        para.ListEnterStatus.Add((EnterStatusType)Enum.Parse(typeof(EnterStatusType), s));
                    }
                }
            }
            //如果没有选择进件状态，则默认查出所有进件状态
            else
            {
                foreach (KeyValuePair<string, string> kv in GlobalSetting.Order_ExceptSD_Status_Car)
                {
                    string[] aryTemp = kv.Key.Split(',');
                    foreach(string s in aryTemp)
                    {
                        if (!string.IsNullOrWhiteSpace(s))
                        {
                            para.ListEnterStatus.Add((EnterStatusType)Enum.Parse(typeof(EnterStatusType),s));
                        }
                    }
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
            para.ListLogo = Global.GlobalSetting.LogoGroupForMenu[menuCode];

            // 查询APP_AUTH表的MENUCODE，在从不同菜单查询相同logo产品时使用
            // lys 2016-3-30
            para.InputMenuCode = menuCode;

            EnterListViewFiledList ret = EnterOrderService.GetEnterOrderListByMainStatus(para);
            foreach (var item in ret.ListEnter)
            {
                foreach (KeyValuePair<string, string> kv in GlobalSetting.Order_ExceptSD_Status_Car)
                {
                    string[] statuName = kv.Value.Split(',');
                    if (!string.IsNullOrWhiteSpace(kv.Key) && kv.Key.Contains(item.APPSTATUS))
                    {
                        string[] statuCode = kv.Key.Split(',');
                        foreach (var status in statuCode)
                        {
                            if (status == item.APPSTATUS)
                            {
                                if (!string.IsNullOrWhiteSpace(statuName[1]))
                                {
                                    /*item.APPSTATUSNAME = "<span class='label label-" + statuName[1] + "'>" + statuName[0] + "</span>";  //V2*/
                                    /*描述：如果是拒贷申请并且有权限，在办理状态加入下划线<u>标签,记录appid
                                      时间：2015-03-11
                                      修改者：leiz*/
                                    if (LoanAppService.IsDisplayRefuseLoanCar(status))
                                    {
                                        item.APPSTATUSNAME = "<span class='label label-" + statuName[1] + " tooltip-" + statuName[1] + " refuseLoanStatus' data-appid=" + item.APPID + "><u>" + statuName[0] + "</u></span>";
                                    }
                                    else
                                    {
                                        item.APPSTATUSNAME = "<span class='label label-" + statuName[1] + "'>" + statuName[0] + "</span>";
                                    }
                                }
                                else
                                {
                                    item.APPSTATUSNAME = "<span class='label label-info'>" + statuName[0] + "</span>";
                                }
                            }
                        }

                    }
                }
            }
            return Json(ret, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 废弃申请未提交的进件
        /// </summary>
        /// <param name="appId"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult DiscardEnterOrder(long appId)
        {
            EnterOrderService.UpdateEnterOrderStatus(appId, EnterStatusType.DISUSED, EnterStatusType.PENDING.ToString());
            //日志
            Infrastructure.Log4Net.LogWriter.Biz("废弃进件", appId);
            return Json(true);
        }

        /// <summary>
        /// 废弃申请未提交进件（展期），会更改原单中的 Has_Apply 状态
        /// </summary>
        /// <param name="appId"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult DiscardEnterOrderExtend(long appId)
        {
            EnterOrderService.DisusedOrderAndResetHasApply(appId, EnterStatusType.PENDING.ToString());
            //日志
            Infrastructure.Log4Net.LogWriter.Biz("展期废弃进件", appId);
            return Json(true);
        }
    }
}