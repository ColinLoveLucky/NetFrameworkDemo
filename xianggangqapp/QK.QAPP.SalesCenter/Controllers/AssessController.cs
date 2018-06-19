using Microsoft.Practices.Unity;
using QK.QAPP.Entity;
using QK.QAPP.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QK.QAPP.Infrastructure;
using QK.QAPP.Global;

namespace QK.QAPP.SalesCenter.Controllers
{
    public class AssessController : Controller
    {
        #region 属性

        [Dependency]
        public IQFUserService qfuserService { get; set; }

        [Dependency]
        public IAPP_AssessService appAssessService { get; set; }

        [Dependency]
        public IAPP_CARVALUATORSSERVICE carValuatorServie { get; set; }

        [Dependency]
        public IQFUserService UserHelper { get; set; }

        /// <summary>
        /// 非法访问跳转地址
        /// </summary>
        const string NoAuthorization = "/Home/NoAuthorization";

        #endregion

        //
        // GET: /Assess/
        public ActionResult ApprovedList(string menuCode)
        {
            ViewData["menuCode"] = menuCode;
            //需要评估的车贷logo
            ViewData["NeedAssessProductLogo"] = Newtonsoft.Json.JsonConvert.SerializeObject(appAssessService.NeedAssessProductLogo);

            string url = Request.Url.PathAndQuery;
            List<APP_Button> listButtons = qfuserService.GetButtonByUrl(url);
            //按钮权限
            ViewData["Permission_Buttons"] = Newtonsoft.Json.JsonConvert.SerializeObject(listButtons);
            return View();
        }

        public ActionResult AssessList(string menuCode)
        {
            ViewData["menuCode"] = menuCode;
            ViewData["AssessQueueStatus"] = appAssessService.AssessQueueStatus;

            string url = Request.Url.PathAndQuery;
            List<APP_Button> listButtons = qfuserService.GetButtonByUrl(url);
            ViewData["Permission_Buttons"] = Newtonsoft.Json.JsonConvert.SerializeObject(listButtons);
            return View();
        }

        [HttpPost]
        public JsonResult GetAssessList(string menuCode)
        {
            //menuCode
            //"VEHICLE":车贷部车贷
            //"GJVEHICLE":个金部车贷
            //截至到2016-3-17 两者都是从车贷拆分得到的，除产品名称和logo不一样外，其他都一样，走得流程和状态也一样
            menuCode = string.IsNullOrEmpty(menuCode) ? "VEHICLE" : menuCode;
            AssessListSearchPara para = new AssessListSearchPara();
            //数据权限
            para.AccessableCsac = qfuserService.GetDataPermission();
            para.PageIndex = int.Parse(Request["page"] ?? "1");
            para.PageSize = int.Parse(Request["rows"] ?? "1");
            para.Sort = new Dictionary<string, string>();
            if (!string.IsNullOrEmpty(Request["sidx"]) && !string.IsNullOrEmpty(Request["sord"]))
            {
                para.Sort.Add(Request["sidx"], Request["sord"]);
            }
            else
            {
                para.Sort.Add("ASSESS_STATUS", "DESC");
            }
            //申请单号
            para.AppCode = HttpUtility.UrlDecode(Request["appCode"]);
            //客户姓名
            para.CustomerName = HttpUtility.UrlDecode(Request["customerName"]);
            //客户经理
            para.SaleCode = HttpUtility.UrlDecode(Request["sales"]);
            para.SaleName = HttpUtility.UrlDecode(Request["sales"]);
            //是否是Search窗口的模糊查询
            if (Request["fuzzySearch"] != null && Request["fuzzySearch"].ToString() == "1")
            {
                para.FuzzySearch = true;
            }
            //评估状态,如果没有选择评估状态，则默认查出所以
            if (Request["assessStasus"] != null && Request["assessStasus"].ToString() != string.Empty)
            {
                string strStatus = Request["assessStasus"].ToString();
                string[] aryStatus = strStatus.Split('|');
                foreach (string s in aryStatus)
                {
                    if (string.IsNullOrEmpty(s))
                        continue;
                    para.ListAssessStatus.Add((AssessStatusType)Enum.Parse(typeof(AssessStatusType), s));
                }
            }
            if (para.ListAssessStatus.Count == 0)
            {
                foreach (KeyValuePair<string, string> kv in appAssessService.AssessQueueStatus)
                {
                    para.ListAssessStatus.Add((AssessStatusType)Enum.Parse(typeof(AssessStatusType), kv.Key));
                }
            }
            //车贷Logo
            para.ListLogo = GlobalSetting.LogoGroupForMenu[menuCode];

            // 查询APP_AUTH表的MENUCODE，在从不同菜单查询相同logo产品时使用
            // lys 2016-3-30
            para.InputMenuCode = menuCode;

            AssessQueueViewFieldList ret = appAssessService.GetAssessListByMainStatus(para);
            return Json(ret, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 保存评估师
        /// </summary>
        /// <param name="appCode">APP_Code</param>
        /// <param name="appCode">评估师账号</param>
        /// <param name="valuatorName">评估师名字</param>
        [HttpPost]
        public string SaveValuator(string appCode, string valuatorCode, string valuatorName)
        {
            string error = string.Empty;
            try
            {
                appAssessService.UpdateValuatorByAppCode(appCode, valuatorCode.Trim(), valuatorName.Trim());
                Infrastructure.Log4Net.LogWriter.Biz("分配车辆评估的评估师", string.Empty,
                    new Dictionary<string, string>() 
                    {
                        { "appCode", appCode.ToString() }, 
                        { "valuatorCode", valuatorCode }, 
                        { "valuatorName", valuatorName } 
                    });
            }
            catch (Exception ex)
            {
                error = "抱歉，分配评估师出错！";
                Infrastructure.Log4Net.LogWriter.Error("分配评估师出错", ex);
            }
            return error;
        }

        public JsonResult GetApprovedList(string menuCode)
        {
            //menuCode
            //"VEHICLE":车贷部车贷
            //"GJVEHICLE":个金部车贷
            //截至到2016-3-17 两者都是从车贷拆分得到的，除产品名称和logo不一样外，其他都一样，走得流程和状态也一样
            menuCode = string.IsNullOrEmpty(menuCode) ? "VEHICLE" : menuCode;
            AssessListSearchPara para = new AssessListSearchPara();
            //数据权限
            para.AccessableCsac = qfuserService.GetDataPermission();
            para.PageIndex = int.Parse(Request["page"] ?? "1");
            para.PageSize = int.Parse(Request["rows"] ?? "1");
            para.Sort = new Dictionary<string, string>();
            if (!string.IsNullOrEmpty(Request["sidx"]) && !string.IsNullOrEmpty(Request["sord"]))
            {
                para.Sort.Add(Request["sidx"], Request["sord"]);
            }
            //申请单号
            para.AppCode = HttpUtility.UrlDecode(Request["appCode"]);
            //客户姓名
            para.CustomerName = HttpUtility.UrlDecode(Request["customerName"]);
            //客户经理
            para.SaleCode = HttpUtility.UrlDecode(Request["sales"]);
            para.SaleName = HttpUtility.UrlDecode(Request["sales"]);
            //是否是Search窗口的模糊查询
            if (Request["fuzzySearch"] != null && Request["fuzzySearch"].ToString() == "1")
            {
                para.FuzzySearch = true;
            }
            para.ListAssessStatus.Add(AssessStatusType.CarAssessApplyApproved);
            if (Request["needAssess"] != null && Request["needAssess"].ToString() != "")
            {
                para.NeedAssess = (Request["needAssess"].ToString() == "Y");
            }
            para.ListLogo = GlobalSetting.LogoGroupForMenu[menuCode];

            // 查询APP_AUTH表的MENUCODE，在从不同菜单查询相同logo产品时使用
            // lys 2016-3-30
            para.InputMenuCode = menuCode;

            AssessQueueViewFieldList ret = appAssessService.GetApprovedList(para);
            return Json(ret, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AssessInfoEdit(long id, string logo)
        {
            string strStatus = string.Empty;
            string edit = Request["isEdit"] ?? "";
            switch (edit)
            {
                case "1":
                    strStatus = AssessStatusType.CarAssessApplyApproved.ToString();
                    break;
                case "2":
                    strStatus = AssessStatusType.CarAssessToBeAssess.ToString();
                    string currentCityCode = UserHelper.GetCurrentUser().City.CITY_CODE;
                    ViewData["valuators"] = carValuatorServie.Find(v => v.CITY_CODE == currentCityCode).ToList();
                    break;
                case "3":
                    break;
                default:
                    break;
            }
            ViewData["IsEdit"] = edit;
            ViewData["needAssess"] = (appAssessService.NeedAssessProductLogo.Contains(logo));
            APP_QUEUE_ASSESS ret = appAssessService.GetAssessInfo(id, logo, strStatus);
            if (ret == null)
            {
                //写日志
                Infrastructure.Log4Net.LogWriter.Biz("访问车辆评估信息时传入的参数有误", string.Empty, new Dictionary<string, string>() { { "id", id.ToString() }, { "logo", logo } });
                return new RedirectResult(NoAuthorization);
            }
            return View(ret);
        }

        public string EditAssessInfo()
        {
            string logo = Request["logo"];
            long id = long.Parse(Request["id"]);
            string appID = Request["appId"];
            APP_QUEUE_ASSESS ret = appAssessService.GetAssessInfo(id, logo, AssessStatusType.CarAssessApplyApproved.ToString());
            if (ret == null)
            {
                //写日志
                Infrastructure.Log4Net.LogWriter.Biz("访问车辆评估信息时传入的参数有误", string.Empty, new Dictionary<string, string>() { { "id", id.ToString() }, { "logo", logo } });
                return "无权编辑评估信息";
            }
            if (appAssessService.NeedAssessProductLogo.Contains(logo))
            {
                string bookTime = Request["bookTime"];
                DateTime dt = DateTime.Now;
                if (DateTime.TryParse(bookTime, out dt))
                {
                    ret.CUSTOMER_BOOK_TIME = dt;
                }
                else
                {
                    ret.CUSTOMER_BOOK_TIME = null;
                }
                
                string arriveTime = Request["arriveTime"];
                if (DateTime.TryParse(arriveTime, out dt))
                {
                    ret.CUSTOMER_ARRIVE_TIME = dt;
                }
                else
                {
                    ret.CUSTOMER_ARRIVE_TIME = null;
                }

                if (!appAssessService.CheckBookTime(ret))
                    return "抱歉，所选预约时间与当前评估师其他预约时间间隔小于20分钟，请另选时间进行预约！";

                if (Request["submit"] == "1")
                {
                    if (ret.CUSTOMER_BOOK_TIME == null)
                    {
                        //写日志
                        Infrastructure.Log4Net.LogWriter.Biz("提交车辆评估信息时传入的预约评车时间有误", appID, Request["bookTime"]);
                        return "预约评车时间";
                    }
                    if (ret.CUSTOMER_ARRIVE_TIME == null)
                    {
                        //写日志
                        Infrastructure.Log4Net.LogWriter.Biz("提交评估信息时传入的实际到店评车时间有误", appID, Request["arriveTime"]);
                        return "实际到店评车时间有误";
                    }
                    ret.ASSESS_STATUS = AssessStatusType.CarAssessToBeAssess.ToString();
                }
            }
            else
            {
                /**
                 *2015-06-26：只有需评估的车贷才会进评估队列，所以此else走不到了
                 */
                if (Request["submit"] == "1")
                {
                    ret.ASSESS_STATUS = AssessStatusType.CarAssessGPS.ToString();
                }
            }
            ret.REMARK = Request["remark"];
            ret.CHANGED_TIME = DateTime.Now;
            ret.CHANGED_USER = UserHelper.GetCurrentUser().Account;


            try
            {
                appAssessService.UpdateAssessInfo(ret);
                Infrastructure.Log4Net.LogWriter.Biz("保存/提交车辆评估信息", appID, ret);
            }
            catch (Exception ex)
            {
                Infrastructure.Log4Net.LogWriter.Error("保存/提交车辆评估信息出错", ex);
                return "保存/提交出错！";
            }
            return "";
        }

        /// <summary>
        /// 犹豫中的客户拒绝
        /// </summary>
        /// <param name="id"></param>
        [HttpPost]
        public JsonResult HesitateRefuse(long id)
        {
            bool error = appAssessService.UpdateAssessStatus(id, AssessStatusType.CarAssessCustomerReject, AssessStatusType.CarAssessCustomerHesitate.ToString());
            return Json(error);
        }

        /// <summary>
        /// 评估已提交
        /// </summary>
        /// <param name="id"></param>
        [HttpPost]
        public JsonResult HesitateAgree(long id)
        {
            bool error = appAssessService.UpdateAssessStatus(id, AssessStatusType.CarAssessSubmitted, AssessStatusType.CarAssessCustomerHesitate.ToString());
            return Json(error);
        }

        /// <summary>
        /// 车况评估详情
        /// </summary>
        /// <param name="appCode"></param>
        /// <param name="logo"></param>
        /// <returns></returns>
        public ActionResult CarAssessInfo(string appCode)
        {

            PAD_CARJUDGE ret = appAssessService.GetCarJudgeInfo(appCode);
            if (ret == null)
            {
                //写日志
                Infrastructure.Log4Net.LogWriter.Biz("访问车辆评估详情时传入的参数有误", string.Empty, new Dictionary<string, string>() { { "appCode", appCode.ToString() } });
                return new RedirectResult(NoAuthorization);
            }
            return View(ret);
        }
    }
}