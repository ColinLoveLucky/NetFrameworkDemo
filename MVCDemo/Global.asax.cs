using MVCDemo.Infranstructure;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure.Interception;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace MVCDemo
{
    public class MvcApplication : System.Web.HttpApplication
    {
	
		private static int num = 0;
		private static int initNum = 0;
        protected void Application_Start()
        {
			num++;
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            BundleTable.EnableOptimizations = true;

            DbInterception.Add(new DbInterceptorTransientErrors());
            DbInterception.Add(new DbInterceptorLogging());
        }

		public override void Init()
		{
			base.Init();
			initNum++;

		}
	}
}
