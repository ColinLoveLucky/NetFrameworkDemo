using System.Linq.Expressions;
using QK.QAPP.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using QK.QAPP.Entity.ExtendEntity;

namespace QK.QAPP.IServices
{
    public interface IQFUserService
    {
        QFUser GetCurrentUser();
        List<APP_Menu> GetMenu();
        void ReadAndSaveMenu();
        void SetMenu(List<APP_Menu> menu);
        void SetUser(QFUser user);
        string UserLogin(string userName, string password);
        List<string> GetDataPermission();

        /// <summary>
        /// 获取数据权限更新版
        /// </summary>
        /// <returns></returns>
        QFUserAuth GetUserAuth();

        /// <summary>
        /// 检查数据权限（通过Account）
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        bool CheckDataPermission(string account);

        /// <summary>
        /// 检查数据权限（通过AppAuth）
        /// </summary>
        /// <param name="appId"></param>
        /// <returns></returns>
        bool CheckDataPermission(long appId);
        List<APP_Button> GetButtonByUrl(string menu_Url);
        List<APP_Button> GetButtonById(string menu_Id);
        bool CheckButton(string menu_Url, string ctr_Id);
        V_ORG_ROLE_USER GetUserBy(string account);

        void LogoutUser(string account);
        void LogoutAllUser();
        OnlineUserViewField GetOnlineUserList(OnlineUserSearchPara para);

        QFAuthInfo GetUserAuthInfo(string userId);

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
        IQueryable<T> QueryJoinMenuAuthOnly<T, TKey>(IQueryable<T> query, Expression<Func<T, TKey>> outerKeySelect, Expression<Func<APP_AUTH, TKey>> innerKeySelect, string menuCode = null);

        /// <summary>
        /// 联接APP_AUTH表进行查询
        /// </summary>
        /// <typeparam name="T">需要查询的数据类型</typeparam>
        /// <typeparam name="TKey">联接键的类型</typeparam>
        /// <param name="query">传入的Query</param>
        /// <param name="outerKeySelect">类型T所提供的联接键</param>
        /// <param name="innerKeySelect">APP_AUTH所提供的联接键</param>
        /// <param name="menuCode">用于查询匹配APP_AUTH表中MenuCode字段</param>
        /// <returns>处理后的Query</returns>
        IQueryable<T> QueryJoinUserAuth<T, TKey>(IQueryable<T> query, Expression<Func<T, TKey>> outerKeySelect, Expression<Func<APP_AUTH, TKey>> innerKeySelect, string menuCode = null);

        /// <summary>
        /// 返回App_Auth表查询条件（过滤用户权限)
        /// </summary>
        /// <param name="app_AuthTbName">App_Auth表的别名</param>
        /// <param name="menuCode">用于查询匹配APP_AUTH表中MenuCode字段</param>
        /// <returns></returns>
        string GetUserAuthWhereStr(string app_AuthTbName, string menuCode = null);

        /// <summary>
        /// 联接PRE_APP_AUTH表进行查询
        /// </summary>
        /// <typeparam name="T">需要查询数据的类型</typeparam>
        /// <typeparam name="TKey">联接键的类型</typeparam>
        /// <param name="query">传入的Query</param>
        /// <param name="outerKeySelect">类型T所提供的联接键</param>
        /// <param name="innerKeySelect">APP_AUTH所提供的联接键</param>
        /// <returns>处理后的Query</returns>
        IQueryable<T> QueryJoinUserAuthPre<T, TKey>(IQueryable<T> query, Expression<Func<T, TKey>> outerKeySelect, Expression<Func<PRE_APP_AUTH, TKey>> innerKeySelect);

        /// <summary>
        /// 检查数据权限（通过APP_AUTH）
        /// </summary>
        /// <param name="appId"></param>
        /// <returns></returns>
        bool CheckDataPermissionByAuth(long appId);
        /// <summary>
        /// 检查数据权限（通过PRE_APP_AUTH）
        /// </summary>
        /// <param name="preAppId"></param>
        /// <returns></returns>
        bool CheckDataPermissionByPreAuth(long preAppId);

        /// <summary>
        /// 取当前机构的父级机构信息（营业部）
        /// </summary>
        /// <param name="parentOrg">客服直属上级机构Id</param>
        /// <returns></returns>
        RmsOrganization GetBusinessDep(string parentOrg);
    }
}
