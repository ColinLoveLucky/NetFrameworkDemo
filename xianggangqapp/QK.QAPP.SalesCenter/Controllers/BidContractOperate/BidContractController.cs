using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QK.QAPP.Entity;
using QK.QAPP.Entity.ExtendEntity;
using QK.QAPP.Global;
using QK.QAPP.Infrastructure;
using QK.QAPP.IServices;

namespace QK.QAPP.SalesCenter.Controllers.BidContractOperate
{
    public class BidContractController : Controller
    {
        private IBID_ContractService BID_ContractService;

        public BidContractController(IBID_ContractService _BID_ContractService)
        {
            BID_ContractService = _BID_ContractService;
        }

        // GET: BidContract
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GetList()
        {
            var para = new BidContractSearchPara()
            {
                CurrentPage = int.Parse(Request["page"] ?? "0"),
                PageSize = int.Parse(Request["rows"] ?? "0")
            };

            if (Request["fuzzySearch"] != null && Request["fuzzySearch"] == "0")
            {
                para.BidIdentifier = HttpUtility.UrlDecode(Request["bidCode"]);
                para.ArrangementId = HttpUtility.UrlDecode(Request["txtPactshowNo"]);
            }

            var result = BID_ContractService.GetFailContractList(para);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public string RePost(string bidcode, string contractNo)
        {
            var para = new ArrangementActivityItfDTO
            {
                BidIdentifier = bidcode,
                IsNew = false,
                ArrangementId = contractNo
            };

            return BID_ContractService.RePost(para);
        }
    }
}