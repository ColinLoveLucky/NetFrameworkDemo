using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVCDemo.Controllers
{
    public class PartialViewController : Controller
    {
        //
        // GET: /PartialView/
        public ActionResult Index()
        {
            return View();
        }

        [ChildActionOnly]
        public PartialViewResult ChildAction(DateTime time)
        {
            string greetings = string.Empty;
            if (time.Hour > 18)
            {
                greetings = "Good evening. Now is " + time.ToString("HH:mm:ss");
            }
            else if (time.Hour > 12)
            {
            }
            else
            {
                greetings = "Good morning. Now is " + time.ToString("HH:mm:ss");
            }
            return PartialView("ChildAction", greetings);
        }  
	}
}