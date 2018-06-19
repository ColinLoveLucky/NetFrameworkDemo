using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QK.QAPP.SalesCenter.Controllers.LabelPact
{
    public class ContractController : Controller
    {
        //
        // GET: /Contract/
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Contracted()
        {
            return View("~/Views/LabelPact/Query/Contracted.cshtml");
        }

        public ActionResult Contracting()
        {
            return View("~/Views/LabelPact/Query/Contracting.cshtml");
        }

        public ActionResult ContractingDetail()
        {
            return View("~/Views/LabelPact/Query/ContractingDetail.cshtml");
        }

        public ActionResult ContractedDetail()
        {
            return View("~/Views/LabelPact/Query/ContractedDetail.cshtml");
        }

        public ActionResult FeePreview()
        {
            return View("~/Views/LabelPact/Query/FeePreview.cshtml");
        }
	}
}