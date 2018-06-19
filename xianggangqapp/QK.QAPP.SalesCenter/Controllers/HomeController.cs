using System.Linq.Expressions;
using Microsoft.Practices.Unity;
using QK.QAPP.Global;
using QK.QAPP.Infrastructure.Data.EFRepository;
using QK.QAPP.Infrastructure.Data.EFRepository.Repository;
using QK.QAPP.Infrastructure.Log4Net;
using QK.QAPP.IServices;
using System.Web.Mvc;
using System.Web.Security;
using QK.QAPP.Entity;
using System.Linq;
using QK.QAPP.Infrastructure;
using System.Diagnostics;
using QK.QAPP.Infrastructure.Cache;
using QK.QAPP.Infrastructure.MessageQueue;
using System.Collections.Generic;
using System;
using QK.QAPP.SalesCenter.Hubs;

namespace QK.QAPP.SalesCenter.Controllers
{

    public class HomeController : Controller
    {
        [Dependency]
        public IQFUserService QFUserHelper { get; set; }
        [Dependency]
        public IAPP_MAINSERVICE RepAppMain { get; set; }
        [Dependency]
        public IAPP_MSGBOXSERVICE MsgBoxService { get; set; }
        [Dependency]
        public IApplyTableService AppTableService { get; set; }

        [Dependency]
        public IV_APPMAINSERVICE VappMainService { get; set; }
        [Dependency]
        public IAPP_REFUSELOANSERVICE AppRefuseLoanService { get; set; }

        public HomeController(IQFUserService _QFUserHelper,
            IAPP_MSGBOXSERVICE _MsgBoxService,
            IApplyTableService _AppTableService,
            IAPP_MAINSERVICE _RepAppMain)
        {
            this.QFUserHelper = _QFUserHelper;
            this.MsgBoxService = _MsgBoxService;
            this.AppTableService = _AppTableService;
            this.RepAppMain = _RepAppMain;
        }
        public ActionResult Index()
        {
            var user = QFUserHelper.GetCurrentUser();
            ViewData["UserAccount"] = user.Account;
            ViewData["Role"] = QFUserHelper.GetMenu();
            //我的所有申请
            ViewBag.AllApplyCount = AppTableService.GetCountEnterByStatus(new List<EnterStatusType>(), user.Account);
            //待补件申请数
            var statusList = GlobalSetting.Order_SD_Status_Need.Select(item => (EnterStatusType)Enum.Parse(typeof(EnterStatusType), item.Key)).ToList();
            statusList.AddRange(GlobalSetting.Order_SD_Status_Need_Car.Select(item => (EnterStatusType)Enum.Parse(typeof(EnterStatusType), item.Key)));
            ViewBag.BuJianApply = AppTableService.GetCountEnterByStatus(statusList, user.Account);
            //废弃
            ViewBag.FeiQiApply = AppTableService.GetCountEnterByStatus(
                new List<EnterStatusType>() { EnterStatusType.DISUSED }, user.Account);
            //已签合同
            ViewBag.HeTongAllApply = AppTableService.GetCountEnterByStatus(
                new List<EnterStatusType>() { EnterStatusType.CONTRACT }, user.Account);
            //系统版本
            ViewData["SystemVersion"] = GlobalSetting.SystemVersion;
            return View();
        }
        /// <summary>
        /// 获取补件通知列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetSupplementList()
        {
            int pageIndex = 0, pageSize = 0;
            //分页、排序参数
            if (Convert.ToString(Request["page"]) != null)
            {
                pageIndex = int.Parse(Convert.ToString(Request["page"]));
            }
            if (Convert.ToString(Request["rows"]) != null)
            {
                pageSize = int.Parse(Convert.ToString(Request["rows"]));
            }
            ViewListByPage<APP_MSGBOX> bjList = new ViewListByPage<APP_MSGBOX>();
            //var list = new List<APP_MSGBOX>();
            var dp = QFUserHelper.GetDataPermission();

            #region 已注释  2014-12-29
            //List<APP_MSGBOX> appMsgbox = new List<APP_MSGBOX>();
            //var vList = new List<V_APPMAIN>();
            //SDAPPRWT:初审待补件|SDENTRYWT:录入待补件  测试用（APPROK  PENDING)
            //var NrEntity = VappMainService.Find(o => dp.Contains(o.CSADNO) && (o.APPSTATUS == "SDENTRYWT" || o.APPSTATUS == "SDAPPRWT")).OrderByDescending(o => o.UPDATETIME);
            //if (NrEntity!=null&&NrEntity.Any())
            //{
            //    var messageList = NrEntity.Skip((pageIndex - 1) * pageSize).Take(pageSize);
            //    vList = messageList.ToList();
            //    vList.ForEach(f =>
            //    {
            //        APP_MSGBOX msgbox = new APP_MSGBOX();
            //        msgbox.USERNAME = f.CSADNO;
            //        msgbox.TITLE = "客户补件通知";
            //        msgbox.CONTENT = string.Format(GlobalSetting.APPLICATION_NR_CONTENT, f.CUSTOMERNAME);
            //        msgbox.PRIORTYLEVEL = MessagePRIORTYLEVEL.HIGHT.ToString();
            //        var cat = "";
            //        GlobalSetting.APP_MSGBOXCATEGORY.TryGetValue(PushMsgType.SupplementStart.ToString(), out cat);
            //        msgbox.CATEGORY = cat;
            //        msgbox.URL = string.Format("/LoanApplication/Application?dformCode={0}&operation=2&appid={1}"
            //                                , f.LOGO
            //                                , f.APPID); ;
            //        msgbox.CREATETITME = f.UPDATETIME;
            //        appMsgbox.Add(msgbox);
            //    });
            //    //去重复名,防止查询数据库重复
            //    foreach (var item in appMsgbox.ToLookup(p => p.USERNAME, p => p))
            //    {
            //        //替换名字
            //        if (item.Key != QFUserHelper.GetCurrentUser().Account)
            //        {
            //            var orgRoleUser = QFUserHelper.GetUserBy(item.Key);
            //            if (orgRoleUser != null)
            //            {
            //                string objectName = orgRoleUser.OBJECTNAME;
            //                foreach (var itemChild in item)
            //                {
            //                    itemChild.USERNAME = objectName;
            //                }
            //            }
            //        }
            //    }
            //}
            #endregion

            var query = MsgBoxService.Find(o => o.STATUS == "UnProcess");
            query = QFUserHelper.QueryJoinUserAuth(query, o => o.APPCODE, a => a.APP_CODE);
            query = query.OrderByDescending(o => o.CREATETITME);

            if (query != null)
            {
                bjList.SetParameters(query, pageIndex, pageSize);
                bjList.ViewList.ForEach(c =>
                {
                    var cat = "";
                    GlobalSetting.APP_MSGBOXCATEGORY.TryGetValue(c.CATEGORY, out cat);
                    c.CATEGORY = cat;
                });

                //去重复名,防止查询数据库重复
                foreach (var item in bjList.ViewList.ToLookup(p => p.USERNAME, p => p))
                {
                    //替换名字
                    if (item.Key != QFUserHelper.GetCurrentUser().Account)
                    {
                        //item.USERNAME = QFUserHelper.GetUserBy(item.USERNAME).OBJECTNAME;
                        var orgRoleUser = QFUserHelper.GetUserBy(item.Key);
                        if (orgRoleUser != null)
                        {
                            string objectName = orgRoleUser.OBJECTNAME;
                            foreach (var itemChild in item)
                            {
                                itemChild.USERNAME = objectName;
                            }
                        }
                    }
                }
            }
            return Json(bjList, JsonRequestBehavior.AllowGet);
        }

        #region 用户登录
        /// <summary>
        /// 登录页面
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        public ActionResult Login()
        {
            //禁用账户时底部显示提示信息
            if (Global.GlobalSetting.AllowLogin != "OPEN")
            {
                ViewData["TipInfo"] = Global.GlobalSetting.TipInfo;


            }
            return View();
        }
        /// <summary>
        /// 登录 AJAX调用
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public ContentResult CheckUserLogin(string userName, string password)
        {
            var isSucess = QFUserHelper.UserLogin(userName, password);
            if (string.IsNullOrEmpty(isSucess))
            {
                Infrastructure.Log4Net.LogWriter.Biz("用户登录成功", userName);
                //创建身份验证票证，即转换为“已登录状态”
                FormsAuthentication.SetAuthCookie(userName.ToLower(), true);
            }
            return Content(isSucess + "");
        }

        /// <summary>
        /// 注销 AJAX调用
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public ActionResult Logout()
        {
            var user = QFUserHelper.GetCurrentUser();
            if (user != null)
            {
                QFUserHelper.LogoutUser(user.Account);
            }
            //取消Session会话
            Session.Abandon();

            //删除Forms验证票证
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Home");
        }

        [HttpPost]
        [AllowAnonymous]
        public ContentResult LogOutOtherUser(string account)
        {
            var message = "您的账户在其他地方登录<br /><b>如果非本人操作,请按Ctrl+Alt+Delete修改密码！</b>";
            PushMessage.PushToUser(account.ToLower(), message, "LoginInfo");

            QFUserHelper.LogoutUser(account);
            return Content("");
        }
        #endregion

        #region 部分视图
        /// <summary>
        /// 用户信息部分视图
        /// </summary>
        /// <returns></returns>
        public PartialViewResult UserMessagePartial()
        {
            var user = QFUserHelper.GetCurrentUser();
            ViewBag.UserAccount = user.Account;
            ViewBag.UserName = user.RealName;
            ViewBag.Title = user.TitleName;

            //图片获取
            //var imgurl = GlobalSetting.UserImageUrl.Replace("{account}", user.Account);
            ViewBag.UserImage = GlobalSetting.UserImageUrl.Replace("{account}", user.Account);
            ViewBag.City = user.City == null ? "" : user.City.CITY_NAME;
            ViewBag.Company = user.CompanyName;
            ViewBag.Deparment = user.DepartmentName;
            ViewBag.RoleList = user.RoleList.Select(c => c.RoleName).JoinString(",");

            //var list = new List<APP_MSGBOX>();
            ////获取当前权限可以获取的信息
            //var myDP = QFUserHelper.GetDataPermission();
            //var MsgList = MsgBoxService.Find(o => myDP.Contains(o.USERNAME) && o.STATUS == "UnProcess")
            //                                .OrderByDescending(o => o.CREATETITME).ToList();
            //ViewData["MsgCount"] = MsgList.Count;
            //if (MsgList != null && MsgList.Any())
            //{
            //    list = MsgList.Take(5).ToList();
            //    foreach (var item in list)
            //    {
            //        //替换名字
            //        if (item.USERNAME != user.Account)
            //        {
            //            //item.USERNAME = QFUserHelper.GetUserBy(item.USERNAME).OBJECTNAME;
            //            var OrgRoleUser = QFUserHelper.GetUserBy(item.USERNAME);
            //            if (OrgRoleUser != null)
            //            {
            //                item.USERNAME = OrgRoleUser.OBJECTNAME;
            //            }
            //        }
            //    }
            //}

            return PartialView();
        }
        /// <summary>
        /// 获取消息通知列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetUserMessageList()
        {
            var user = QFUserHelper.GetCurrentUser();
            var list = new List<APP_MSGBOX>();
            var count = 0;

            var query = MsgBoxService.Find(m => m.STATUS == "UnProcess");
            query = QFUserHelper.QueryJoinUserAuth(query, m => m.APPCODE, o => o.APP_CODE)
                .OrderByDescending(m => m.CREATETITME);
            count = query.Count();

            if (query != null)
            {
                list = query.Take(5).ToList();
                foreach (var item in list)
                {
                    //替换名字
                    if (item.USERNAME != user.Account)
                    {
                        //item.USERNAME = QFUserHelper.GetUserBy(item.USERNAME).OBJECTNAME;
                        var OrgRoleUser = QFUserHelper.GetUserBy(item.USERNAME);
                        if (OrgRoleUser != null)
                        {
                            item.USERNAME = OrgRoleUser.OBJECTNAME;
                        }
                    }
                }
            }
            return Json(new { list, count }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 菜单部分视图
        /// </summary>
        /// <returns></returns>
        public PartialViewResult NavigationPartial()
        {
            var menuList = QFUserHelper.GetMenu();
            if (menuList == null)
            {

                RedirectToAction("logout", "home");
                return PartialView(new List<APP_Menu>());
            }
            menuList = menuList.Where(c => c.AllowDisplay == 1).ToList();
            return PartialView(menuList);
        }

        [HttpPost]
        public ContentResult LogoutUser(string account, string msg)
        {
            var message = string.Format("您的账户被管理员登出<br />管理员留言:<b>" + msg + "</b>");
            PushMessage.PushToUser(account, message, "LoginInfo");

            QFUserHelper.LogoutUser(account);
            return Content("");
        }

        /// <summary>
        /// 强制登出所有用户
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        [HttpPost]
        public ContentResult LogoutAllUser(string msg)
        {
            var message = string.Format("您的账户被管理员登出<br />管理员留言:<b>" + msg + "</b>");
            PushMessage.PushToUser(string.Empty, message, "LogoutAll");

            QFUserHelper.LogoutAllUser();
            return Content("");
        }

        #region 添加拒贷信息=>20150306
        /// <summary>
        /// 描述：添加拒贷信息
        /// 时间：2015-03-06
        /// 添加人：leiz
        /// </summary>
        /// <returns></returns>
        public PartialViewResult RefuseLoanDiscription()
        {
            return PartialView();
        }

        public PartialViewResult RefuseLoanDiscriptionCar()
        {
            return PartialView("RefuseLoanDiscription");
        }

        public PartialViewResult RefuseLoanDiscriptionGeek()
        {
            return PartialView("RefuseLoanDiscription");
        }

        public PartialViewResult RefuseLoanDiscriptionHouse()
        {
            return PartialView("RefuseLoanDiscription");
        }

        public PartialViewResult RefuseLoanDiscriptionRy()
        {
            return PartialView("RefuseLoanDiscription");
        }

        //以下添加方法获取该申请单的拒贷描述
        /// <summary>
        /// 获取1级拒件描述信息
        /// </summary>
        /// <returns></returns>
        public string GetRefsueLoanInfo()
        {
            string appid = Request["appid"];
            string info = AppRefuseLoanService.GetRefuseLoanInfo(appid);
            return info;
        }

        #endregion
        #endregion

        [AllowAnonymous]
        public ActionResult NoAuthorization()
        {
            return View();
        }
    }
};