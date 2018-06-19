using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace QK.QAPP.Infrastructure.Mvc
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = true, AllowMultiple = true)]
    public class AdminAttribute : FilterAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationContext filterContext)
        {
            AuthorizationAttribute author = new AuthorizationAttribute();
            var user= author.GetAuthorization(new AuthorizationContext(filterContext));
            if (user != null || user.RoleList.Count > 0)
            {
                var RoleList = user.RoleList;
                if (RoleList.Any(c => c.RoleCode == Global.GlobalSetting.AdminCode))
                {
                }
                else
                {
                    throw new Exception("您没有足够的访问权限");
                }
            }
        }
    }
}
