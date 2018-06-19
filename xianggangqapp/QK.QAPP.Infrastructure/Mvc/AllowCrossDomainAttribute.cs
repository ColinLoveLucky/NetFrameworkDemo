using System.Web;
using System.Web.Mvc;

namespace QK.QAPP.Infrastructure
{
    public class AllowCrossDomainAttribute : ActionFilterAttribute
    {
        private readonly string _method;
        public AllowCrossDomainAttribute(string method)
        {
            _method = method;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //return;
            var method = filterContext.HttpContext.Request.HttpMethod;
            if (method.Equals("options"))
            {
                filterContext.Result = new ContentResult();
            }
            else
            {
                if (!method.Equals(_method))
                {
                    throw new HttpException(404, "Invalid request method.");
                }
                base.OnActionExecuting(filterContext);
            }
        }
    }

    public class AllowCrossDomainGetAttribute : AllowCrossDomainAttribute
    {
        public AllowCrossDomainGetAttribute()
            : base("get")
        {
        }
    }

    public class AllowCrossDomainPostAttribute : AllowCrossDomainAttribute
    {
        public AllowCrossDomainPostAttribute()
            : base("post")
        {
        }
    }
}