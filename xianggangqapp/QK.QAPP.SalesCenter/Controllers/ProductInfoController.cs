using QK.QAPP.IServices;
using QK.QAPP.Entity;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QK.QAPP.SalesCenter.Controllers
{
    public class ProductInfoController : Controller
    {
        [Dependency]
        public IQFProductInfoService QFProductHelper { get; set; }
        public ActionResult Index()
        {
            var productList = QFProductHelper.GetProductList(PInfoInterfaceURLAccount.orgId.ToString(), "775d0be3-263e-47c7-ae66-b6b61c0d5eb7");
            var productSelectList = new SelectList(productList, "productCode", "productName");
            ViewData["productList"] = productSelectList;


            var termList = QFProductHelper.GetQFProductTerm("productCodesyd18");
            var termSelectList = new SelectList(termList);
            ViewData["termsList"] = termSelectList;

            var productList1 = QFProductHelper.GetProductList(PInfoInterfaceURLAccount.productCode.ToString(), "productCodexyd18").FirstOrDefault();
            if (productList1 != null)
            {
                ViewData["productLogoName"] = productList1.productName;
                ViewData["productLogoCode"] = productList1.productCode;
                ViewData["consultationChargeRatio"] = productList1.consultationChargeRatio;
            }
            else
            {
                ViewData["productLogoName"] = "";
                ViewData["productLogoCode"] = "";

            }

            List<DataType> rePayTypeList = QFProductHelper.GetRePayTypeList("775d0be3-263e-47c7-ae66-b6b61c0d5eb7");
            var rePayTypeSelectList = new SelectList(rePayTypeList, "dataCode", "dataName");
            ViewData["rePayType"] = rePayTypeSelectList;

            var interest = QFProductHelper.GetInterestList(PInfoInterfaceURLAccount.productCode.ToString(), "productCodexyd18").FirstOrDefault();
            
            if (interest!=null)
            {
                ViewData["serviceChargeRatio"] = interest.serviceChargeRatio.ToString();
               
                ViewData["defaultInterestRatio"] = interest.defaultInterestRatio.ToString();
                ViewData["rateType"] = interest.rateType.ToString();
                ViewData["rate"] = interest.rate.ToString();

            }
            return View();
        }
    }
}