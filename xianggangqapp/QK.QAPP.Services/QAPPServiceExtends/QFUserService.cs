
/*********************
 * 作者：刘成帅
 * 时间：2014/9/3
 * 功能：关于用户以及用户菜单存取的操作
**********************/

using System.Linq.Expressions;
using QK.QAPP.Entity;
using QK.QAPP.Entity.ExtendEntity;
using QK.QAPP.Global;
using QK.QAPP.Infrastructure;
using QK.QAPP.Infrastructure.Log4Net;
using QK.QAPP.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Text;

namespace QK.QAPP.Services
{
    public class QFUserService : IQFUserService
    {
        private static string SESSION_USER = GlobalSetting.AuthSaveKey;
        private static string SESSION_Menu = GlobalSetting.MenuSaveKey;

        #region 属性注入
        //[Dependency]
        IV_ORG_ROLE_USERService ViewService;

        //[Dependency]
        IAPP_MSGBOX_USERSERVICE MsgUserService;

        IAPP_AUTHSERVICE AppAuthService;
        public QFUserService(IV_ORG_ROLE_USERService _V_ORG_ROLE_USERService,
            IAPP_MSGBOX_USERSERVICE _MsgUserService,
            IAPP_AUTHSERVICE _AppAuthService)
        {
            this.ViewService = _V_ORG_ROLE_USERService;
            this.MsgUserService = _MsgUserService;
            this.AppAuthService = _AppAuthService;
        }
        #endregion

        #region 接口实现
        /// <summary>
        /// 用户登录验证
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public string UserLogin(string userName, string password)
        {
            //检查当前是否可以登录
            bool flag = false;
            //配置文件中配置允许登录的账户
            //string[] acountNames = GlobalSetting.AllowAcount.Split(';');
            var acountNames = GlobalSetting.AllowAcount.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries).ToArray();
            if (acountNames.Any() && acountNames.Contains(userName, StringComparer.OrdinalIgnoreCase))
            {
                flag = true;
            }
            if (GlobalSetting.AllowLogin != "OPEN" && !flag)
            {
                Infrastructure.Log4Net.LogWriter.Biz("当前系统未开启登录但是用户进行了登录！");
                return GlobalSetting.TipInfo;//"当前系统正在升级或者维护，您暂时无法登录系统，请稍候再试！";
            }

            string json = "";
            json += "{";
            json += "   \"userName\":\"" + userName + "\",";
            json += "   \"password\":\"" + password + "\"";
            json += "}";

            var restClient = new RestHelper(GlobalSetting.AuthInterfaceURL);
            var user = restClient.Post<QFUser>("CheckUserLogin", json);

            if (user != null && user.Status == DtoMessageStatus.Success && user.ReturnObj != null && user.ReturnObj.Account != null)
            {
                if (GlobalSetting.MultipleLogin != "OPEN")
                {
                    var msgUser = CheckUserIsLogin(user.ReturnObj.Account);
                    if (msgUser != null)
                    {
                        return string.Format("logined,{0},{1}", msgUser.USERIP, msgUser.MACHINENAME);
                    }
                }

                SetUser(user.ReturnObj);
                ReadAndSaveMenu();
                if (GetMenu().Count <= 0)
                {
                    LogoutUser(user.ReturnObj.Account);
                    //取消Session会话
                    HttpContext.Current.Session.Abandon();

                    //删除Forms验证票证
                    FormsAuthentication.SignOut();
                    Infrastructure.Log4Net.LogWriter.Biz("用户没有访问权限，但是用户进行了登录！", userName);
                    return "您没有该系统的访问权限，请联系管理员！";
                }
                return "";
            }
            else
            {
                Infrastructure.Log4Net.LogWriter.Biz("用户登录失败！", userName, user);
                return "登录失败，请检查您的用户名或者密码！";
            }
        }

        /// <summary>
        /// 从接口读取菜单并且将菜单存入SESSION
        /// </summary>
        public void ReadAndSaveMenu()
        {
            GetMenu();
            var user = GetCurrentUser();
            if (user != null)
            {
                var prp = new Dictionary<string, string>();
                prp.Add("UserId", user.UserId + "");
                prp.Add("code", GlobalSetting.RootMenuCode);
                var restClient = new RestHelper(GlobalSetting.AuthInterfaceURL);
                var message = restClient.Get<List<APP_Menu>>("GetUserPermission", prp);
                if (message != null && message.Status == DtoMessageStatus.Success && message.ReturnObj != null)
                {
                    SetMenu(message.ReturnObj);
                }
            }
        }

        /// <summary>
        /// 获取当前用户的菜单
        /// </summary>
        public List<APP_Menu> GetMenu()
        {
            List<APP_Menu> menu = new List<APP_Menu>();
            var user = GetCurrentUser();
            if (user != null)
            {

                try
                {
                    HttpContext rq = HttpContext.Current;
                    menu = (List<APP_Menu>)rq.Session[SESSION_Menu];
                }
                catch (Exception)
                {
                    //throw new Exception(MessageHelper.MSG0031);
                }
                return menu;
            }
            else
            {
                return new List<APP_Menu>();
            }
        }


        public void SetUser(QFUser user)
        {
            //添加进件城市
            var cityService = Ioc.GetService<IAPP_CITYSERVICE>();
            //user.City = cityService.FirstOrDefault(c => c.COMPANY_CODE == user.CompanyId);--V2
            user.City = cityService.FilterByPlatform(
                GlobalSetting.UsingPlatformForCityProduct_QAPP
            ).FirstOrDefault(c => c.COMPANY_CODE == user.CompanyId);
            if (user.City == null)
            {
                user.City = new APP_CITY();
            }
            HttpContext rq = HttpContext.Current;
            rq.Session[SESSION_USER] = user;
        }
        public QFUser GetCurrentUser()
        {
            QFUser su = null;
            try
            {
                HttpContext rq = HttpContext.Current;
                su = (QFUser)rq.Session[SESSION_USER];

            }
            catch (Exception)
            {
                //throw new Exception(MessageHelper.MSG0031);
            }
            return su;
        }
        public void SetMenu(List<APP_Menu> menu)
        {
            HttpContext rq = HttpContext.Current;
            rq.Session[SESSION_Menu] = menu;
        }

        /// <summary>
        /// 获取数据权限
        /// </summary>
        /// <returns></returns>
        public List<string> GetDataPermission()
        {
            return GetCurrentUser().DataPermission;
        }

        /// <summary>
        /// 获取数据权限更新版
        /// </summary>
        /// <returns></returns>
        public QFUserAuth GetUserAuth()
        {
            return GetCurrentUser().DataPermissionOrg;
        }

        /// <summary>
        /// 检查数据权限
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public bool CheckDataPermission(string account)
        {
            var dp = GetDataPermission();
            if (dp != null)
            {
                return dp.Any(c => c == account);
            }
            return false;
        }

        /// <summary>
        /// 检查数据权限（通过AppAuth）
        /// </summary>
        /// <param name="appId"></param>
        /// <returns></returns>
        public bool CheckDataPermission(long appId)
        {
            var appAuth = AppAuthService.FirstOrDefault(c => c.APP_ID == appId);
            var userAuth = GetUserAuth();
            if (appAuth != null && userAuth != null)
            {
                if (userAuth.IsSelectAll)
                    return true;
                if (userAuth.AccountList != null && userAuth.AccountList.Contains(appAuth.ACCOUNT, StringComparer.OrdinalIgnoreCase))
                    return true;
                if (userAuth.CompanyList != null && userAuth.CompanyList.Contains(appAuth.COMPANY, StringComparer.OrdinalIgnoreCase))
                    return true;
                if (userAuth.ParentIdList != null && userAuth.ParentIdList.Contains(appAuth.PARENT_ORGANIZATION, StringComparer.OrdinalIgnoreCase))
                    return true;
            }
            return false;
        }


        /// <summary>
        /// 查找所有按钮权限
        /// </summary>
        /// <param name="menu_Url"></param>
        /// <returns></returns>
        public List<APP_Button> GetButtonByUrl(string menu_Url)
        {
            List<APP_Button> ret = new List<APP_Button>();
            var menuList = GetMenu();
            if (menuList != null && menuList.Count() > 0)
            {
                var menu = GetMenu().Where(c => menu_Url == c.NavigateUrl);
                foreach (var item in menu)
                {
                    ret.AddRange(item.Button);
                }
            }

            return ret;
        }
        /// <summary>
        /// 获取按钮权限
        /// </summary>
        /// <param name="menu_Id"></param>
        /// <returns></returns>
        public List<APP_Button> GetButtonById(string menu_Id)
        {

            List<APP_Button> ret = new List<APP_Button>();
            var menuList = GetMenu();
            if (menuList != null && menuList.Count() > 0)
            {
                var menu = GetMenu().Where(c => c.MenuId == menu_Id);

                foreach (var item in menu)
                {
                    ret.AddRange(item.Button);
                }
            }
            return ret;
        }
        /// <summary>
        /// 检查按钮是否存在权限
        /// </summary>
        /// <param name="menu_Url"></param>
        /// <param name="ctr_Id"></param>
        /// <returns></returns>
        public bool CheckButton(string menu_Url, string ctr_Id)
        {
            return GetButtonByUrl(menu_Url).Any(c => c.Control_ID == ctr_Id);
        }

        /// <summary>
        /// 通过Account获取用户对象
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public V_ORG_ROLE_USER GetUserBy(string account)
        {
            V_ORG_ROLE_USER user = ViewService.SqlQuery<V_ORG_ROLE_USER>("SELECT * FROM V_ORG_ROLE_USER WHERE OBJECTVALUE=:account", account).FirstOrDefault();
            return user;
        }


        /// <summary>
        /// 登所有用户
        /// </summary>
        /// <param name="userName"></param>
        public void LogoutUser(string userName)
        {
            var items = MsgUserService.Find(c => c.USERNAME == userName && c.ENABLE == 1);
            if (items != null && items.Count() > 0)
            {
                foreach (var item in items)
                {
                    item.ENABLE = 0;
                    MsgUserService.Update(item);
                }
                MsgUserService.UnitOfWork.SaveChanges();
            }
            Infrastructure.Log4Net.LogWriter.Biz("用户退出！", userName);
        }

        /// <summary>
        /// 登出所有用户
        /// </summary>
        public void LogoutAllUser()
        {
            var items = MsgUserService.Find(c => c.ENABLE == 1);
            if (items != null && items.Count() > 0)
            {
                foreach (var item in items)
                {
                    item.ENABLE = 0;
                    MsgUserService.Update(item);
                }
                MsgUserService.UnitOfWork.SaveChanges();
            }
            Infrastructure.Log4Net.LogWriter.Biz("登出用户");
        }
        /// <summary>
        /// 获取当前在线用户（数据绑定）
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        public OnlineUserViewField GetOnlineUserList(OnlineUserSearchPara para)
        {
            OnlineUserViewField onlineUserList = new OnlineUserViewField();
            string where = @"from app_msgbox_user mu
                            right join v_org_role_user ru
                            on mu.username = ru.OBJECTVALUE
                            where mu.enable = '1' and (ru.OBJECTNAME like :theValue or ru.COMPANYNAME like :theValue or ru.DEPARTNAME like :theValue or ru.ROLENAME like :theValue )
                    ";
            string value = "%" + para.UserName + "%";
            var sql = @"SELECT TEMP.DISPLAYNAME,
                               TEMP.COMPANYNAME,
                               TEMP.DEPARTNAME,
                               TEMP.ROLENAME,
                               TEMP.ID,
                               TEMP.CONNECTIONID,
                               TEMP.USERNAME,
                               TEMP.CREATETIME,
                               TEMP.USERIP,
                               TEMP.USERBROWSER,
                               TEMP.USERBROWSERVERSION,
                               TEMP.LASTUPDATETIME,
                               TEMP.ENABLE,
                               TEMP.MACHINENAME
                          FROM ( select ru.OBJECTNAME  as DISPLAYNAME,
                                       ru.COMPANYNAME,
                                       ru.DEPARTNAME,
                                       ru.ROLENAME,
                                       mu.*,
                                       rownum         RowNumber " + where + @") TEMP
                         WHERE TEMP.ROWNUMBER BETWEEN {0} AND {1}";
            string sqlCount = "SELECT COUNT(0) TOTALCOUNT " + where;
            sql = string.Format(sql, (para.PageIndex - 1) * para.PageSize + 1, para.PageIndex * para.PageSize);
            var ie = ViewService.SqlQuery<OnlineUserTable>(sql, value);
            List<OnlineUserTable> list = ie.ToList();
            onlineUserList.OnlineUserList = list;
            var pg = ViewService.SqlQuery<OnlineUserTotal>(sqlCount, value).FirstOrDefault();
            if (pg != null)
            {
                /*记录在线用户总记录数 修改时间：2015-02-09  修改人：leiz*/
                onlineUserList.TotalRecords = pg.TOTALCOUNT;
                onlineUserList.TotalPages = Math.Ceiling(pg.TOTALCOUNT.ToDouble() / para.PageSize).ToInt32();
            }

            onlineUserList.Rows = para.PageIndex;
            return onlineUserList;
        }

        /// <summary>
        /// 获取用户权限数据
        /// </summary>
        /// <param name="userId">用户GUID</param>
        /// <returns></returns>
        public QFAuthInfo GetUserAuthInfo(string userId)
        {
            string authInterfaceUrl = GlobalSetting.AuthInterfaceURL;
            var rest = new RestHelper(authInterfaceUrl);
            var para = new Dictionary<string, string>();
            para.Add("userid", userId);
            var dtoMessage = rest.Get<QFAuthInfo>("FindSecondCompany", para);
            if (dtoMessage != null && dtoMessage.Status == DtoMessageStatus.Success && dtoMessage.ReturnObj != null)
            {
                return dtoMessage.ReturnObj;
            }
            else
            {
                LogWriter.Error("权限数据获取失败[" + userId + "]");
                return new QFAuthInfo();
            }

        }

        /// <summary>
        /// 联接APP_AUTH表进行查询MenuCode
        /// </summary>
        /// <typeparam name="T">需要查询数据的类型</typeparam>
        /// <typeparam name="TKey">联接键的类型</typeparam>
        /// <param name="query">传入的Query</param>
        /// <param name="outerKeySelect">类型T所提供的联接键</param>
        /// <param name="innerKeySelect">APP_AUTH所提供的联接键</param>
        /// <param name="menuCode">用于查询匹配APP_AUTH表中MenuCode字段</param>
        /// <returns>处理后的Query</returns>
        public IQueryable<T> QueryJoinMenuAuthOnly<T, TKey>(IQueryable<T> query, Expression<Func<T, TKey>> outerKeySelect, Expression<Func<APP_AUTH, TKey>> innerKeySelect, string menuCode = null)
        {
            var appAuthService = Ioc.GetService<IAPP_AUTHSERVICE>();

            if (!string.IsNullOrEmpty(menuCode))
            {
                query = query.Join(appAuthService.Find(o => o.MENUCODE == menuCode || o.MENUCODE == null), outerKeySelect, innerKeySelect, (e, o) => e);
            }
            return query;
        }

        /// <summary>
        /// 联接APP_AUTH表进行查询权限（含MenuCode查询）
        /// </summary>
        /// <typeparam name="T">需要查询数据的类型</typeparam>
        /// <typeparam name="TKey">联接键的类型</typeparam>
        /// <param name="query">传入的Query</param>
        /// <param name="outerKeySelect">类型T所提供的联接键</param>
        /// <param name="innerKeySelect">APP_AUTH所提供的联接键</param>
        /// <param name="menuCode">用于查询匹配APP_AUTH表中MenuCode字段</param>
        /// <returns>处理后的Query</returns>
        public IQueryable<T> QueryJoinUserAuth<T, TKey>(IQueryable<T> query, Expression<Func<T, TKey>> outerKeySelect, Expression<Func<APP_AUTH, TKey>> innerKeySelect, string menuCode = null)
        {
            var appAuthService = Ioc.GetService<IAPP_AUTHSERVICE>();
            var dp = GetUserAuth();
            if (dp.IsSelectAll)
            {
                if (!string.IsNullOrEmpty(menuCode))
                {
                    query = query.Join(appAuthService.Find(o => o.MENUCODE == menuCode || o.MENUCODE == null), outerKeySelect, innerKeySelect, (e, o) => e);
                }
                return query;
            }
            else
            {
                if (dp.AccountList == null)
                    dp.AccountList = new List<string>();
                if (dp.CompanyList == null)
                    dp.CompanyList = new List<string>();
                if (dp.ParentIdList == null)
                    dp.ParentIdList = new List<string>();

                if (!string.IsNullOrEmpty(menuCode))
                {
                    query = query.Join(appAuthService.Find(o =>
                        (dp.AccountList.Contains(o.ACCOUNT) || dp.CompanyList.Contains(o.COMPANY) || dp.ParentIdList.Contains(o.PARENT_ORGANIZATION)) && (o.MENUCODE == menuCode || o.MENUCODE == null))
                        , outerKeySelect
                        , innerKeySelect
                        , (e, o) => e
                    );
                }
                else
                {
                    query = query.Join(appAuthService.Find(o => dp.AccountList.Contains(o.ACCOUNT) || dp.CompanyList.Contains(o.COMPANY) || dp.ParentIdList.Contains(o.PARENT_ORGANIZATION))
                        , outerKeySelect
                        , innerKeySelect
                        , (e, o) => e
                    );
                }

                return query;
            }
        }

        /// <summary>
        /// 返回App_Auth表查询条件（过滤用户权限)
        /// </summary>
        /// <param name="app_AuthTbName">App_Auth表的别名</param>
        /// <param name="menuCode">用于查询匹配APP_AUTH表中MenuCode字段</param>
        /// <returns></returns>
        public string GetUserAuthWhereStr(string app_AuthTbName, string menuCode = null)
        {
            StringBuilder sb = new StringBuilder();
            var dp = GetUserAuth();
            if (!dp.IsSelectAll)
            {
                if (dp.AccountList == null)
                    dp.AccountList = new List<string>();
                if (dp.CompanyList == null)
                    dp.CompanyList = new List<string>();
                if (dp.ParentIdList == null)
                    dp.ParentIdList = new List<string>();
                sb.Append("and (");
                if (dp.AccountList.Any())
                {
                    sb.Append("(");
                    string accountStr = string.Empty;
                    dp.AccountList.ForEach(a => accountStr +=app_AuthTbName + ".ACCOUNT='" + a + "' or ");
                    sb.Append(accountStr.Substring(0, accountStr.Length - 4)+")");
                }
                if (dp.CompanyList.Any())
                {
                    if(sb.Length>5)
                    {
                        sb.Append(" or ");
                    }
                    sb.Append("(");
                    string companyStr = string.Empty;
                    dp.CompanyList.ForEach(a => companyStr +=app_AuthTbName + ".COMPANY='" + a + "' or ");
                    sb.Append(companyStr.Substring(0, companyStr.Length - 4) + ")");
                }
                if (dp.ParentIdList.Any())
                {
                    if (sb.Length > 5)
                    {
                        sb.Append(" or ");
                    }
                    sb.Append("(");
                    string parentidStr = string.Empty;
                    dp.ParentIdList.ForEach(a => parentidStr +=app_AuthTbName + ".PARENT_ORGANIZATION='" + a + "' or ");
                    sb.Append(parentidStr.Substring(0, parentidStr.Length - 4) + ")");
                }
                if (sb.Length > 5)
                {
                    sb.Append(") ");
                }
            }
            if (!string.IsNullOrEmpty(menuCode))
            {
                sb.Append("and (" + app_AuthTbName + ".MENUCODE='" + menuCode + "' or " + app_AuthTbName + ".MENUCODE is null) ");
            }
            return "1=1 "+sb.ToString();
        }

        /// <summary>
        /// 联接PRE_APP_AUTH表进行查询
        /// </summary>
        /// <typeparam name="T">需要查询数据的类型</typeparam>
        /// <typeparam name="TKey">联接键的类型</typeparam>
        /// <param name="query">传入的Query</param>
        /// <param name="outerKeySelect">类型T所提供的联接键</param>
        /// <param name="innerKeySelect">APP_AUTH所提供的联接键</param>
        /// <returns>处理后的Query</returns>
        public IQueryable<T> QueryJoinUserAuthPre<T, TKey>(IQueryable<T> query, Expression<Func<T, TKey>> outerKeySelect, Expression<Func<PRE_APP_AUTH, TKey>> innerKeySelect)
        {
            var appAuthService = Ioc.GetService<IPRE_APP_AUTHSERVICE>();
            var dp = GetUserAuth();
            if (dp.IsSelectAll)
            {
                return query;
            }
            if (dp.AccountList == null)
                dp.AccountList = new List<string>();
            if (dp.CompanyList == null)
                dp.CompanyList = new List<string>();
            if (dp.ParentIdList == null)
                dp.ParentIdList = new List<string>();

            query = query.Join(appAuthService.Find(o => dp.AccountList.Contains(o.ACCOUNT) || dp.CompanyList.Contains(o.COMPANY) || dp.ParentIdList.Contains(o.PARENT_ORGANIZATION))
               , outerKeySelect
               , innerKeySelect
               , (e, o) => e
           );

            return query;
        }

        /// <summary>
        /// 检查数据权限（通过APP_AUTH）
        /// </summary>
        /// <param name="appId"></param>
        /// <returns></returns>
        public bool CheckDataPermissionByAuth(long appId)
        {
            var appAuthService = Ioc.GetService<IAPP_AUTHSERVICE>();
            var appAuth = appAuthService.FirstOrDefault(a => a.APP_ID == appId);
            var dp = GetUserAuth();
            if (appAuth != null)
            {
                if (dp.IsSelectAll)
                    return true;
                if (dp.AccountList != null && dp.AccountList.Contains(appAuth.ACCOUNT))
                    return true;
                if (dp.CompanyList != null && dp.CompanyList.Contains(appAuth.COMPANY))
                    return true;
                if (dp.ParentIdList != null && dp.ParentIdList.Contains(appAuth.PARENT_ORGANIZATION))
                    return true;
                return false;
            }
            return false;
        }

        /// <summary>
        /// 检查数据权限（通过PRE_APP_AUTH）
        /// </summary>
        /// <param name="preAppId"></param>
        /// <returns></returns>
        public bool CheckDataPermissionByPreAuth(long preAppId)
        {
            var appAuthService = Ioc.GetService<IPRE_APP_AUTHSERVICE>();
            var appAuth = appAuthService.FirstOrDefault(a => a.APP_ID == preAppId);
            var dp = GetUserAuth();
            if (appAuth != null)
            {
                if (dp.IsSelectAll)
                    return true;
                if (dp.AccountList != null && dp.AccountList.Contains(appAuth.ACCOUNT))
                    return true;
                if (dp.CompanyList != null && dp.CompanyList.Contains(appAuth.COMPANY))
                    return true;
                if (dp.ParentIdList != null && dp.ParentIdList.Contains(appAuth.PARENT_ORGANIZATION))
                    return true;
                return false;
            }
            return false;
        }

        /// <summary>
        /// 取当前机构的父级机构信息（营业部）
        /// </summary>
        /// <param name="parentOrg">客服直属上级机构Id</param>
        /// <returns></returns>
        public RmsOrganization GetBusinessDep(string parentOrg)
        {
            string sql = "select r.organizationid,r.parentid,r.code,r.shortname,r.fullname,r.category from rms.rms_organization r inner join rms.rms_organization o on r.organizationid = o.parentid where o.organizationid = :orgId";

            var org = ViewService.SqlQuery<RmsOrganization>(sql, parentOrg).FirstOrDefault();

            if (org == null)
            {
                LogWriter.Warn(string.Format("未找到当前机构（{0}）的上级机构", parentOrg));
            }

            return org;
        }
        #endregion

        #region 私有函数
        /// <summary>
        /// 判断用户是否已经登录
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        private APP_MSGBOX_USER CheckUserIsLogin(string userName)
        {

            var loginUser = MsgUserService.FirstOrDefault(c => c.USERNAME == userName && c.ENABLE == 1);
            return loginUser;

        }
        #endregion

    }
}
