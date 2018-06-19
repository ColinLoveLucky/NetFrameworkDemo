using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Security.Permissions;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AspNetWebForm
{
    [PrincipalPermission(SecurityAction.Demand, Name = "QUARK\\XianggangZhang")]
    public partial class login : System.Web.UI.Page
    {
        private UsersResposity _userResposity;
        public login()
        {
            _userResposity = new UsersResposity();
        }
        protected void Page_Load(object sender, EventArgs e)
        {
           // PrincipalPermission
           // CodeAccessPermission
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (_userResposity.ValidaterUserIsValid(txtName.Text, txtPassword.Text))
            {
               
               var rolesName= _userResposity.FindUser().
                   Where(x => x.Name.ToLower() == txtName.Text).
                   Select(x => x.Roles).First().Select(x => x.Name);
                FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1,
                txtName.Text, DateTime.Now, DateTime.Now.AddSeconds(40), false, string.Join(",", rolesName));

                string encryTicket = FormsAuthentication.Encrypt(ticket);
                HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName);
                //把加密后的票据信息放入cookie
                cookie.Value = encryTicket;
                //把cookie添加到响应流中
                Response.Cookies.Add(cookie);
                Response.Redirect(FormsAuthentication.GetRedirectUrl(txtName.Text, false), true);
            }

            
        }
    }
}