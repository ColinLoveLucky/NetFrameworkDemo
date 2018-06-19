using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVCDemo.Controllers
{
    public class RemoteQuartzController : Controller
    {
        // GET: RemoteQuartz
        public ActionResult Index()
        {
            return View();
        }

        public string ReturnRemote()
        {
            NameValueCollection properties = new NameValueCollection
            {
                {"quartz.scheduler.proxy","true" },
                {"quartz.scheduler.proxy.address","tcp://localhost:556/QuartzScheduler" }
            };

            var schedulerFactory = new StdSchedulerFactory(properties);
            var scheduler = schedulerFactory.GetScheduler();

            JobKey key = new JobKey("myJob", "group1");
            var isExists = scheduler.CheckExists(key);

            scheduler.PauseAll();

            return "Hello World";
        }
    }
}