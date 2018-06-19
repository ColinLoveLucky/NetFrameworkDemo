using Autofac;
using Autofac.Integration.Mvc;
using MVC5AutoFac.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Compilation;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace MVC5AutoFac
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            var builder = new ContainerBuilder();
            //注册IControl接口
            builder.RegisterControllers(Assembly.GetExecutingAssembly());
            builder.RegisterControllers(Assembly.GetExecutingAssembly()).PropertiesAutowired();
            //注册IControl里面构造函数，属性的依赖对象
            var baseType = typeof(IDependency);
            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly()).
                Where(t => baseType.IsAssignableFrom(t) && t != baseType).AsImplementedInterfaces().
                InstancePerLifetimeScope();
            //注册IModelBinder
            builder.RegisterModelBinders(Assembly.GetExecutingAssembly());
            builder.RegisterModelBinderProvider();

            //注册abastractModule
            //HttpContextBase
            //HttpRequestBase
            //HttpResponseBase
            //HttpServerUtilityBase
            //HttpSessionStateBase
            //HttpApplicationStateBase
            //HttpBrowserCapabilitiesBase
            //HttpFileCollectionBase
            //RequestContext
            //HttpCachePolicyBase
            //VirtualPathProvider
            //UrlHelper
            builder.RegisterModule<AutofacWebTypesModule>();

            //Filter 的注入
            builder.RegisterFilterProvider();

            //动态加载目录dll
            // BuildManager.AddReferencedAssembly(asm);

            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}
