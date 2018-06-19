using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Linq;
using System.Management;
using System.Security.Principal;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AspNetWebForm
{
    public partial class WindowsAuthentication : System.Web.UI.Page
    {
        private UsersResposity _userResposity;
        public WindowsAuthentication()
        {
            _userResposity = new UsersResposity();

            throw new Exception("Message Exception");
        }
        protected void Page_Load(object sender, EventArgs e)
        {
        }
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (_userResposity.ValidaterUserIsValid(txtName.Text, txtPassword.Text))
            {
                string ldapPath = "LDAP://" + UserHelper.GetDomainName();
                string domainAndUsername = Environment.UserDomainName + "\\" + txtName.Text;
                DirectoryEntry entry = new DirectoryEntry(ldapPath, domainAndUsername, txtPassword.Text);
                DirectorySearcher search = new DirectorySearcher(entry);
                try
                {
                    SearchResult result = search.FindOne();
                }
                catch (Exception ex)
                {
                    // 如果用户名或者密码不正确，也会抛出异常。
                 //   MessageBox.Show(ex.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
                WindowsPrincipal winPrincipal = (WindowsPrincipal)HttpContext.Current.User;
                HttpContext.Current.Response.Write(string.Format("HttpContext.Current.User.Identity: {0}, {1}\r\n",
                        winPrincipal.Identity.AuthenticationType, winPrincipal.Identity.Name));
                WindowsPrincipal winPrincipal2 = (WindowsPrincipal)Thread.CurrentPrincipal;
                HttpContext.Current.Response.Write(string.Format("Thread.CurrentPrincipal.Identity: {0}, {1}\r\n",
                        winPrincipal2.Identity.AuthenticationType, winPrincipal2.Identity.Name));
                WindowsIdentity winId = WindowsIdentity.GetCurrent();
                HttpContext.Current.Response.Write(string.Format("WindowsIdentity.GetCurrent(): {0}, {1}",
                        winId.AuthenticationType, winId.Name));

                
            }
        }
    }
    public sealed class UserInfo
    {
        public string GivenName;
        public string FullName;
        public string Email;
    }

    public static class UserHelper
    {
        /// <summary>
        /// 活动目录中的搜索路径，也可根据实际情况来修改这个值。
        /// </summary>
        public static string DirectoryPath = "LDAP://" + GetDomainName();
        /// <summary>
        /// 获取与指定HttpContext相关的用户信息
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static UserInfo GetCurrentUserInfo(HttpContext context)
        {
            string loginName = GetUserLoginName(context);
            if (string.IsNullOrEmpty(loginName))
                return null;
            return GetUserInfoByLoginName(loginName);
        }
        /// <summary>
        /// 根据指定的HttpContext对象，获取登录名。
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static string GetUserLoginName(HttpContext context)
        {
            if (context == null)
                return null;
            if (context.Request.IsAuthenticated == false)
                return null;
            string userName = context.User.Identity.Name;

            // 此时userName的格式为：UserDomainName\LoginName
            // 我们只需要后面的LoginName就可以了。
            string[] array = userName.Split(new char[] { '\\' }, StringSplitOptions.RemoveEmptyEntries);
            if (array.Length == 2)
                return array[1];
            return null;
        }
        /// <summary>
        /// 根据登录名查询活动目录，获取用户信息。
        /// </summary>
        /// <param name="loginName"></param>
        /// <returns></returns>
        public static UserInfo GetUserInfoByLoginName(string loginName)
        {
            if (string.IsNullOrEmpty(loginName))
                return null;

            // 下面的代码将根据登录名查询用户在AD中的信息。
            // 为了提高性能，可以在此处增加一个缓存容器(Dictionary or Hashtable)。

            try
            {
                DirectoryEntry entry = new DirectoryEntry(DirectoryPath);
                DirectorySearcher search = new DirectorySearcher(entry);
                search.Filter = "(SAMAccountName=" + loginName + ")";

                search.PropertiesToLoad.Add("givenName");
                search.PropertiesToLoad.Add("cn");
                search.PropertiesToLoad.Add("mail");
                // 如果还需要从AD中获取其它的用户信息，请参考ActiveDirectoryDEMO
                SearchResult result = search.FindOne();

                if (result != null)
                {
                    UserInfo info = new UserInfo();
                    info.GivenName = result.Properties["givenName"][0].ToString();
                    info.FullName = result.Properties["cn"][0].ToString();
                    info.Email = result.Properties["mail"][0].ToString();
                    return info;
                }
            }
            catch
            {
                // 如果需要记录异常，请在此处添加代码。
            }
            return null;
        }
        public static string GetDomainName()
        {
            // 注意：这段代码需要在Windows XP及较新版本的操作系统中才能正常运行。
            SelectQuery query = new SelectQuery("Win32_ComputerSystem");
            using (ManagementObjectSearcher searcher = new ManagementObjectSearcher(query))
            {
                foreach (ManagementObject mo in searcher.Get())
                {
                    if ((bool)mo["partofdomain"])
                        return mo["domain"].ToString();
                }
            }
            return null;
        }

    }
}