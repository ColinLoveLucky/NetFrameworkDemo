
using System.Net.Http;
using System.Security.Principal;
using System.Threading;
using System.Web;
using System.Web.Http;

namespace WebApiDemo.Controllers
{
    [Authorize]
    public class ValuesController : ApiController
    {
        //public HttpResponseMessage Get(int id)
        //{
        //    return null;
        //}
        //[Authorize]
        //public HttpResponseMessage Post()
        //{
        //    return null;
        //}
        //[AllowAnonymous]
        //public HttpResponseMessage Get()
        //{
        //    if (User.IsInRole("Administrators"))
        //    {
        //    }
        //    return null;
        //}
        //private void SetPrincipal(IPrincipal principal)
        //{
        //    Thread.CurrentPrincipal = principal;
        //    if (HttpContext.Current != null)
        //        HttpContext.Current.User = principal;
        //}

        public string Get()
        {
            var userName = this.RequestContext.Principal.Identity.Name;
            return string.Format("Hello, {0}.", userName);
        }
    }


}