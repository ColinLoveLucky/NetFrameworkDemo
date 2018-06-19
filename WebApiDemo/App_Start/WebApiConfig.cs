
using Microsoft.Owin.Security.OAuth;
using System.Web.Http;
using System.Web.Http.Routing;
using WebApiDemo.Controllers;
using WebApiDemo.Filters;

namespace WebApiDemo
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            //// Web API configuration and services
            //// Web API routes
            config.SuppressDefaultHostAuthentication();
            var constraintResolver = new DefaultInlineConstraintResolver();
            constraintResolver.ConstraintMap.Add("nonzero", typeof(NonZeroConstraint));
            config.MapHttpAttributeRoutes(constraintResolver);
            //config.Routes.MapHttpRoute(
            //    name: "DefaultApi",
            //    routeTemplate: "api/{controller}/{action}/{id}",
            //    defaults: new { id = RouteParameter.Optional }
            //);
            //config.Filters.Add(new AuthorizeAttribute());
            ////ODataConventionModelBuilder builder = new ODataConventionModelBuilder();
            ////builder.EntitySet<Author>("Author");
            ////config.Routes.MapODataRoute("odata", "odata", builder.GetEdmModel());
            // Web API configuration and services
            // Configure Web API to use only bearer token authentication.
            config.Filters.Add(new HostAuthenticationFilter(OAuthDefaults.AuthenticationType));
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
            // Enforce HTTPS
          //  config.Filters.Add(new RequireHttpsAttribute());
        }
    }
}
