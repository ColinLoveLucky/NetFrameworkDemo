using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Permissions;
using System.Security.Principal;
using System.Threading;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AspNetWebForm
{
    /// <summary>
    ///  //PermissionSet
    //HttpContext
    //HttpWorkerRequest
    // HttpRuntime
    // HttpApplication app=
    // ASPNET_ISAPI.dll
    // HttpApplicationFactory
    //  HttpRuntime.NamedPermissionSet
    // PermissionSet
    // IPrincipal
    //ClaimsPrincipal
    //GenericPrincipal
    // WindowsPrincipal
    // IIdentity
    // ClaimsIdentity
    // FormsIdentity
    // GenericIdentity
    // WindowsIdentity
    // Membership
    // PassportAuthenticateModule
    // WindowsAuthenticateModule
    // FormsAuthenticateModule
    //DefaultAuthenticateModule
    // FileAuthorizationModule
    // UrlAuthorizationModule
    // FormsAuthentication
    // FileIOPermission
    //PrincipalPermission
    /// </summary>
    public partial class LoginSuccess : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Page.User.Identity.IsAuthenticated)
            {
                labName.Text = Page.User.Identity.Name;
                HttpCookie cookie = Request.Cookies[FormsAuthentication.FormsCookieName];
                string encryptedTicket = cookie.Value;
                FormsAuthenticationTicket tickets = FormsAuthentication.Decrypt(encryptedTicket);
                labRoles.Text = tickets.UserData;
                FormsIdentity identity = new FormsIdentity(tickets);
                GenericPrincipal user = new GenericPrincipal(identity, tickets.UserData.Split(','));
                HttpContext.Current.User = user;
                if (HttpContext.Current.User.IsInRole("Admin"))
                {
                    labIsTrue.Text = HttpContext.Current.User.IsInRole("Admin").ToString();
                }
            }
            //PassportIdentity
            //  FormsAuthentication.SetAuthCookie()
           // WindowsIdentity
            //IPrincipal
            // IIdentity

            var permission = new PrincipalPermission("QUARK\\XianggangZhang", null);
            //将当前线程的IPrincipal设置成自定义的GenericPrincipal
            Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity("QUARK\\XianggangZhang"), null);

            //   RoleProvider
        }
    }
}