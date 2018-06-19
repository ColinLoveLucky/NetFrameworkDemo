using Microsoft.Practices.Unity;
using QK.QAPP.Global;
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
using QK.QAPP.SalesTest.Hubs;

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
            var list = new List<APP_MSGBOX>();
            var myDP = QFUserHelper.GetDataPermission();
            var MsgList = MsgBoxService.Find(o => myDP.Contains(o.USERNAME) && o.STATUS == "UnProcess")
                                            .OrderByDescending(o => o.CREATETITME);
            if (MsgList != null && MsgList.Any())
            {
                list = MsgList.ToList();
                list.ForEach(c =>
                {
                    var cat = "";
                    GlobalSetting.APP_MSGBOXCATEGORY.TryGetValue(c.CATEGORY, out cat);
                    c.CATEGORY = cat;
                    //if (c.USERNAME != user.Account)
                    //{
                    //    var orgRoleUser = QFUserHelper.GetUserBy(c.USERNAME);
                    //    if (orgRoleUser!=null)
                    //    {
                    //        c.USERNAME = orgRoleUser.OBJECTNAME;

                    //    }                        
                    //}

                });

                foreach (var item in list.ToLookup(p => p.USERNAME, p => p))
                {
                    //替换名字
                    if (item.Key != user.Account)
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
            
            ViewData["UserAccount"] = user.Account;
            ViewData["Role"] = QFUserHelper.GetMenu();



            //我的所有申请
            ViewBag.AllApplyCount = AppTableService.GetCountEnterByStatus(new List<EnterStatusType>(), user.Account);
            //待补件申请数
            var statusList = GlobalSetting.Order_SD_Status_Need.Select(item => (EnterStatusType) Enum.Parse(typeof (EnterStatusType), item.Key)).ToList();
            //车贷待补件
            statusList.AddRange(GlobalSetting.Order_SD_Status_Need_Car.Select(item => (EnterStatusType)Enum.Parse(typeof(EnterStatusType), item.Key)));
            ViewBag.BuJianApply = AppTableService.GetCountEnterByStatus(statusList, user.Account);
            //废弃
            ViewBag.FeiQiApply = AppTableService.GetCountEnterByStatus(
                new List<EnterStatusType>() { EnterStatusType.DISUSED }, user.Account);
            //已签合同
            ViewBag.HeTongAllApply = AppTableService.GetCountEnterByStatus(
                new List<EnterStatusType>() { EnterStatusType.CONTRACT }, user.Account);

           


            //throw new Exception("测试错误");
            return View(list);
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
        [AllowAnonymous]
        public ActionResult CheckUserLogin(string userName, string password)
        {
            var isSucess = QFUserHelper.UserLogin(userName, password);
            if (string.IsNullOrEmpty(isSucess))
            {
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
            PushMessage.PushToUser(account, message, "LoginInfo");

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
            var imgurl = GlobalSetting.UserImageUrl.Replace("{account}", user.Account);

            ViewBag.UserImage = GlobalSetting.UserImageUrl.Replace("{account}", user.Account);


            ViewBag.City = user.City == null ? "" : user.City.CITY_NAME;
            ViewBag.Company = user.CompanyName;
            ViewBag.Deparment = user.DepartmentName;
            ViewBag.RoleList = user.RoleList.Select(c => c.RoleName).JoinString(",");
           



            var list = new List<APP_MSGBOX>();
            //获取当前权限可以获取的信息
            var myDP = QFUserHelper.GetDataPermission();
            var MsgList = MsgBoxService.Find(o => myDP.Contains(o.USERNAME) && o.STATUS == "UnProcess")
                                            .OrderByDescending(o => o.CREATETITME).ToList();
            if (MsgList != null && MsgList.Any())
            {
                list = MsgList;

                //去重复名,防止查询数据库重复
                foreach (var item in list.Take(5).ToLookup(p => p.USERNAME, p => p))
                {
                    //替换名字
                    if (item.Key != user.Account)
                    {
                        //item.USERNAME = QFUserHelper.GetUserBy(item.USERNAME).OBJECTNAME;
                        var OrgRoleUser = QFUserHelper.GetUserBy(item.Key);
                        if (OrgRoleUser != null)
                        {
                            string objectName= OrgRoleUser.OBJECTNAME;
                            foreach (var itemChild in item)
                            {
                                itemChild.USERNAME = objectName;
                            }
                        }
                    }
                }
            }
            return PartialView(list);
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
        #endregion

        [AllowAnonymous]
        public ActionResult NoAuthorization()
        {
            return View();
        }
    }
};