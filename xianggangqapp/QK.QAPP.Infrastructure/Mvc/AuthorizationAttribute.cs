using QK.QAPP.Entity;
using QK.QAPP.Global;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace QK.QAPP.Infrastructure
{
    /// <summary>
    /// 表示需要用户登录才可以使用的特性
    /// <para>如果不需要处理用户登录，则请指定AllowAnonymousAttribute属性</para>
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = true, AllowMultiple = true)]
    public class AuthorizationAttribute : FilterAttribute, IAuthorizationFilter
    {
        private String _AuthUrl = String.Empty;
        /// <summary>
        /// 获取或者设置一个值，改值表示登录地址
        /// <para>如果web.config中未定义AuthUrl的值，则默认为/Home/Login</para>
        /// </summary>
        public String AuthUrl
        {
            get { return _AuthUrl.Trim(); }
            set
            {
                if (String.IsNullOrEmpty(value))
                {
                    throw new ArgumentNullException("用于验证用户登录信息的登录地址不能为空！");
                }
                else
                {
                    _AuthUrl = value.Trim();
                }
            }
        }

        private String _AuthSaveKey = String.Empty;
        /// <summary>
        /// 获取或者设置一个值，改值表示登录用来保存登陆信息的键名
        /// <para>如果web.config中未定义AuthSaveKey的值，则默认为LoginedUser</para>
        /// </summary>
        public String AuthSaveKey
        {
            get { return _AuthSaveKey.Trim(); }
            set
            {
                if (String.IsNullOrEmpty(value))
                {
                    throw new ArgumentNullException("用于保存登陆信息的键名不能为空！");
                }
                else
                {
                    this._AuthSaveKey = value.Trim();
                }
            }
        }

        private string _MenuSaveKey = string.Empty;
        /// <summary>
        /// 获取或者设置一个值，该值表示登录用户保存菜单列表的键名
        /// </summary>
        public string MenuSaveKey
        {
            get { return _MenuSaveKey.Trim(); }
            set
            {
                if (String.IsNullOrEmpty(value))
                {
                    throw new ArgumentNullException("该用户没有任何模块权限！");
                }
                else
                {
                    this._MenuSaveKey = value.Trim();
                }
            }
        }

        private String _AuthSaveType = String.Empty;
        /// <summary>
        /// 获取或者设置一个值，该值表示用来保存登陆信息的方式
        /// <para>如果web.config中未定义AuthSaveType的值，则默认为Session保存</para>
        /// </summary>
        public String AuthSaveType
        {
            get { return _AuthSaveType.Trim().ToUpper(); }
            set
            {
                if (String.IsNullOrEmpty(value))
                {
                    throw new ArgumentNullException("用于保存登陆信息的方式不能为空，只能为【Cookie】或者【Session】！");
                }
                else
                {
                    _AuthSaveType = value.Trim();
                }
            }
        }


        /// <summary>
        /// 默认构造函数
        /// </summary>
        public AuthorizationAttribute()
        {
            String authUrl = GlobalSetting.AuthUrl;
            String menuSavekey = GlobalSetting.MenuSaveKey;
            String authSaveKey = GlobalSetting.AuthSaveKey;
            String authSaveType = GlobalSetting.AuthSaveType;

            if (String.IsNullOrEmpty(authUrl))
            {
                this._AuthUrl = "/Home/Login";
            }
            else
            {
                this._AuthUrl = authUrl;
            }

            if (String.IsNullOrEmpty(authSaveKey))
            {
                this._AuthSaveKey = "SESSION_USER";
            }
            else
            {
                this._AuthSaveKey = authSaveKey;
            }

            if (String.IsNullOrEmpty(menuSavekey))
            {
                this._MenuSaveKey = "SESSION_MENU";
            }
            else
            {
                this._MenuSaveKey = menuSavekey;
            }

            if (String.IsNullOrEmpty(authSaveType))
            {
                this._AuthSaveType = "Session";
            }
            else
            {
                this._AuthSaveType = authSaveType;
            }
        }
        /// <summary>
        /// 构造函数重载
        /// </summary>
        /// <param name="loginUrl">表示没有登录跳转的登录地址</param>
        public AuthorizationAttribute(String authUrl)
            : this()
        {
            this._AuthUrl = authUrl;
        }
        /// <summary>
        /// 构造函数重载
        /// </summary>
        /// <param name="loginUrl">表示没有登录跳转的登录地址</param>
        /// <param name="authSaveKey">表示登录用来保存登陆信息的键名</param>
        public AuthorizationAttribute(String authUrl, String authSaveKey)
            : this(authUrl)
        {
            this._AuthSaveKey = authSaveKey;
            this._AuthSaveType = "Session";
        }

        /// <summary>
        /// 构造函数重载
        /// </summary>
        /// <param name="authUrl">表示没有登录跳转的登录地址</param>
        /// <param name="authSaveKey">表示登录用来保存登陆信息的键名</param>
        /// <param name="menuSaveKey">表示登录用户保存菜单权限的键名</param>
        public AuthorizationAttribute(String authUrl, String authSaveKey, String menuSaveKey)
            : this(authUrl, authSaveKey)
        {
            this._MenuSaveKey = menuSaveKey;
        }

        /// <summary>
        /// 构造函数重载
        /// </summary>
        /// <param name="authUrl">表示没有登录跳转的登录地址</param>
        /// <param name="saveKey">表示登录用来保存登陆信息的键名</param>
        /// <param name="saveType">表示登录用来保存登陆信息的方式</param>
        public AuthorizationAttribute(String authUrl, String saveKey, String menuSaveKey, String authSaveType)
            : this(authUrl, saveKey, menuSaveKey)
        {
            this._AuthSaveType = authSaveType;
        }


        /// <summary>
        /// 处理用户登录
        /// </summary>
        /// <param name="filterContext"></param>
        public void OnAuthorization(AuthorizationContext filterContext)
        {
            if (filterContext.HttpContext == null)
            {
                throw new Exception("此特性只适合于Web应用程序使用！");
            }
            else
            {
                var curReqURL = filterContext.HttpContext.Request.Url;
                string backURl = System.Web.HttpUtility.UrlEncode(curReqURL.ToString());
                switch (AuthSaveType)
                {
                    case "SESSION":
                        if (filterContext.HttpContext.Session == null)
                        {
                            throw new Exception("服务器Session不可用！");
                        }
                        else if (!filterContext.ActionDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true)
&& !filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true))
                        {
                            QFUser user = (QFUser)filterContext.HttpContext.Session[_AuthSaveKey];
                            if (user == null)
                            {
                                filterContext.Result = new RedirectResult(_AuthUrl + "?backUrl=" + backURl);
                            }
                            else
                            {
                                string curAbsoPath = curReqURL.AbsolutePath.ToString();
                                if (curAbsoPath == "/" || filterContext.HttpContext.Request.Headers["X-Requested-With"] == "XMLHttpRequest") break;
                                List<APP_Menu> menu = (List<APP_Menu>)filterContext.HttpContext.Session[_MenuSaveKey];
                                List<APP_Button> button = new List<APP_Button>();
                                bool result = false;
                                if (menu.Count > 0)
                                {
                                    string[] curPAQ = HandleAccount(curReqURL.PathAndQuery.ToString().ToLower());
                                    foreach (var item in menu)
                                    {
                                        if (item.NavigateUrl != null)
                                        {
                                            string[] menuNav = HandleAccount(item.NavigateUrl.ToLower());
                                            if (curPAQ.Count() == menuNav.Count())
                                            {
                                                int i = 0;
                                                foreach (var curPAQitem in curPAQ)
                                                {
                                                    foreach (var menuNavitem in menuNav)
                                                    {
                                                        if (curPAQitem.Contains(menuNavitem))
                                                        {
                                                            i += 1;
                                                        }
                                                    }                                                                                                                                                                                                                                               
                                                }
                                                if (i == curPAQ.Count())
                                                {
                                                    result = true;
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                }
                                else 
                                {
                                    Log4Net.LogWriter.Biz("用户已登录，但是没有任何模块权限", user.Account, "URL地址："+curReqURL);
                                }
                                if (!result)
                                {
                                    Log4Net.LogWriter.Biz("用户已登录，但是没有此权限", user.Account, "URL地址：" + curReqURL);
                                    filterContext.Result = new RedirectResult("/Home/NoAuthorization");
                                }
                            }
                        }
                        break;
                    case "COOKIE":
                        if (!filterContext.ActionDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true)
&& !filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true))
                        {
                            if (filterContext.HttpContext.Request.Cookies[_AuthSaveKey] == null)
                            {
                                filterContext.Result = new RedirectResult(_AuthUrl + "?backUrl=" + backURl);
                            }
                        }
                        break;
                    default:
                        throw new ArgumentNullException("用于保存登陆信息的方式不能为空，只能为【Cookie】或者【Session】！");
                }
            }
        }

        /// <summary>
        /// 处理用户登录
        /// </summary>
        /// <param name="filterContext"></param>
        public QK.QAPP.Entity.QFUser GetAuthorization(AuthorizationContext filterContext)
        {
            if (filterContext.HttpContext == null)
            {
                throw new Exception("此特性只适合于Web应用程序使用！");
            }
            else
            {
                string backURl = System.Web.HttpUtility.UrlEncode(filterContext.HttpContext.Request.Url.ToString());
                switch (AuthSaveType)
                {
                    case "SESSION":
                        if (filterContext.HttpContext.Session == null)
                        {
                            throw new Exception("服务器Session不可用！");
                        }
                        else if (filterContext.HttpContext.Session[_AuthSaveKey] == null)
                        {
                            filterContext.Result = new RedirectResult(_AuthUrl + "?backUrl=" + backURl);
                        }
                        else
                        {
                            return filterContext.HttpContext.Session[_AuthSaveKey] as QAPP.Entity.QFUser;
                        }
                        break;
                    case "COOKIE":
                        if (filterContext.HttpContext.Request.Cookies[_AuthSaveKey] == null)
                        {
                            filterContext.Result = new RedirectResult(_AuthUrl + "?backUrl=" + backURl);
                        }
                        break;
                    default:
                        throw new ArgumentNullException("用于保存登陆信息的方式不能为空，只能为【Cookie】或者【Session】！");
                }
            }
            return null;
        }

        //处理传进来的url
        private string[] HandleAccount(string account)
        {
            if (account.Contains("?"))
            {
                string atr = account.Replace('?', '&');
                string[] ret = atr.Split('&');
                return ret;
            }
            else
            {
                string[] ret = account.Split('?');
                return ret;
            }
        }
    }

}
