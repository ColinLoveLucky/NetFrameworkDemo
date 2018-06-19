using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Practices.Unity;
using QK.QAPP.Entity;
using QK.QAPP.Global;
using QK.QAPP.Infrastructure;
using QK.QAPP.IServices;

namespace QK.QAPP.SalesCenter.Controllers
{
    public class PreApplicationController : Controller
    {
        #region 属性
        [Dependency]
        public IPRE_APP_MAINSERVICE PreAppMainService { get; set; }
        [Dependency]
        public IAPP_CITYSERVICE AppCityService { get; set; }
        [Dependency]
        public IQFProductInfoService ProductInfoService { get; set; }
        [Dependency]
        public ICR_DATA_DICService DicService { get; set; }
        [Dependency]
        public IPreApplyService PreApplyService { get; set; }
        [Dependency]
        public IAPP_CITY_PRODUCTSERVICE CityProductService { get; set; }
        #endregion

        [HttpGet]
        public ActionResult PreApplication(long preAppId)
        {
            var preAppMain = PreAppMainService.FirstOrDefault(m => m.ID == preAppId);
            var msg = PreApplyService.CheckPermission(preAppMain);
            if (!string.IsNullOrEmpty(msg))
            {
                ViewData["noPermission"] = msg;
            }
            SetViewData(preAppMain, SetViewDataCredit);
            return View();
        }

        [HttpGet]
        public ActionResult PreGJCarApplication(long preAppId)
        {
            ViewBag.CustomerType = GetCustomerType();
            var preAppMain = PreAppMainService.FirstOrDefault(m => m.ID == preAppId);
            var msg = PreApplyService.CheckPermission(preAppMain);
            if (!string.IsNullOrEmpty(msg))
            {
                ViewData["noPermission"] = msg;
            }
            SetViewData(preAppMain, SetViewDataGJCar);
            return View();
        }

        [HttpGet]
        public ActionResult PreCarApplication(long preAppId)
        {
            ViewBag.CustomerType = GetCustomerType();
            var preAppMain = PreAppMainService.FirstOrDefault(m => m.ID == preAppId);
            var msg = PreApplyService.CheckPermission(preAppMain);
            if (!string.IsNullOrEmpty(msg))
            {
                ViewData["noPermission"] = msg;
            }
            SetViewData(preAppMain, SetViewDataCar);
            return View();
        }

        [HttpGet]
        public ActionResult PreGeekApplication(long preAppId)
        {
            var preAppMain = PreAppMainService.FirstOrDefault(m => m.ID == preAppId);
            var msg = PreApplyService.CheckPermission(preAppMain);
            if (!string.IsNullOrEmpty(msg))
            {
                ViewData["noPermission"] = msg;
            }
            SetViewData(preAppMain, SetViewDataGeek);
            return View();
        }

        /// <summary>
        /// 车贷预申请进件提交
        /// </summary>
        /// <param name="formCollection"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult PreGJCarApplication(FormCollection formCollection,string menuCode)
        {
            string isRedirect = string.Empty;
            string resultMsg;
            //验证表单必填字段信息
            var formDic = CheckCarFormData(formCollection, out resultMsg);
            //必填字段信息有误
            if (!string.IsNullOrEmpty(resultMsg))
            {
                Infrastructure.Log4Net.LogWriter.Biz("预申请进件验证：" + resultMsg);
                return Json(new { resultMsg, isRedirect });
            }

            //传入车贷menuCode保存到app_auth表，lys 2016-3-30
            formDic.Add("InputMenuCode", menuCode);

            long preAppId = formDic["preAppId"].ToInt64();
            //预申请进件
            var appMain = PreApplyService.ApplyLoanCar(preAppId, formDic, out resultMsg);
            if (!string.IsNullOrEmpty(resultMsg))
            {
                return Json(new { resultMsg, isRedirect });
            }

            resultMsg = "/LoanApplication/Application?dformCode=" + appMain.LOGO + "&operation=1&appid=" + appMain.ID;
            isRedirect = "true";

            return Json(new { resultMsg, isRedirect });
        }

        /// <summary>
        /// 车贷预申请进件提交
        /// </summary>
        /// <param name="formCollection"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult PreCarApplication(FormCollection formCollection, string menuCode)
        {
            string isRedirect = string.Empty;
            string resultMsg;
            //验证表单必填字段信息
            var formDic = CheckCarFormData(formCollection, out resultMsg);
            //必填字段信息有误
            if (!string.IsNullOrEmpty(resultMsg))
            {
                Infrastructure.Log4Net.LogWriter.Biz("预申请进件验证：" + resultMsg);
                return Json(new { resultMsg, isRedirect });
            }

            //传入车贷menuCode保存到app_auth表，lys 2016-3-30
            formDic.Add("InputMenuCode", menuCode);

            long preAppId = formDic["preAppId"].ToInt64();
            //预申请进件
            var appMain = PreApplyService.ApplyLoanCar(preAppId, formDic, out resultMsg);
            if (!string.IsNullOrEmpty(resultMsg))
            {
                return Json(new { resultMsg, isRedirect });
            }

            resultMsg = "/LoanApplication/Application?dformCode=" + appMain.LOGO + "&operation=1&appid=" + appMain.ID;
            isRedirect = "true";

            return Json(new { resultMsg, isRedirect });
        }

        /// <summary>
        /// 信贷预申请进件提交
        /// </summary>
        /// <param name="formCollection"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult PreApplication(FormCollection formCollection)
        {
            string isRedirect = string.Empty;
            string resultMsg;

            //验证表单必填字段信息
            var formDic = CheckFormData(formCollection, out resultMsg);

            //必填字段信息有误
            if (!string.IsNullOrEmpty(resultMsg))
            {
                Infrastructure.Log4Net.LogWriter.Biz("预申请进件验证：" + resultMsg);
                return Json(new { resultMsg, isRedirect });
            }

            long preAppId = formDic["preAppId"].ToInt64();

            //预申请进件
            var appMain = PreApplyService.ApplyLoan(preAppId, formDic, out resultMsg);
            if (!string.IsNullOrEmpty(resultMsg))
            {
                return Json(new { resultMsg, isRedirect });
            }

            resultMsg = "/LoanApplication/Application?dformCode=" + appMain.LOGO + "&operation=1&appid=" + appMain.ID;
            isRedirect = "true";

            return Json(new { resultMsg, isRedirect });
        }

        /// <summary>
        /// 极客贷预申请进件提交
        /// </summary>
        /// <param name="formCollection"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult PreGeekApplication(FormCollection formCollection)
        {
            string isRedirect = string.Empty;
            string resultMsg;

            //验证表单必填字段信息
            var formDic = CheckFormData(formCollection, out resultMsg);

            //必填字段信息有误
            if (!string.IsNullOrEmpty(resultMsg))
            {
                Infrastructure.Log4Net.LogWriter.Biz("预申请进件验证：" + resultMsg);
                return Json(new { resultMsg, isRedirect });
            }

            long preAppId = formDic["preAppId"].ToInt64();

            //预申请进件
            var appMain = PreApplyService.ApplyLoan(preAppId, formDic, out resultMsg);
            if (!string.IsNullOrEmpty(resultMsg))
            {
                return Json(new { resultMsg, isRedirect });
            }

            resultMsg = "/LoanApplication/Application?dformCode=" + appMain.LOGO + "&operation=1&appid=" + appMain.ID;
            isRedirect = "true";

            return Json(new { resultMsg, isRedirect });
        }

        private void SetViewDataCredit(PRE_APP_MAIN preAppMain)
        {
            //城市列表
            ViewBag.CityList = GetCityList("CREDIT");
            //产品
            SetProductByLogoGroupMenu(preAppMain, "CREDIT");
            //合作渠道
            ViewBag.Plantform = GetPlantform();
        }

        private void SetViewDataGJCar(PRE_APP_MAIN preAppMain)
        {
            //城市列表
            ViewBag.CityList = GetCityList("GJVEHICLE");
            //产品
            SetProductByLogoGroupMenu(preAppMain, "GJVEHICLE");
            //合作渠道
            ViewBag.Plantform = GetCarPlantform("CHANNELCOOPERATION_GJVEHICLE");
        }

        private void SetViewDataCar(PRE_APP_MAIN preAppMain)
        {
            //城市列表
            ViewBag.CityList = GetCityList("VEHICLE");
            //产品
            SetProductByLogoGroupMenu(preAppMain, "VEHICLE");
            //合作渠道
            ViewBag.Plantform = GetCarPlantform("CHANNELCOOPERATION_VEHICLE");
        }

        private void SetViewDataGeek(PRE_APP_MAIN preAppMain)
        {
            ViewBag.CityList = GetCityList("GEEK");
            SetProductByLogoGroupMenu(preAppMain, "GEEK");
            //合作渠道
            ViewBag.Plantform = GetPlantform();
        }

        private void SetViewData(PRE_APP_MAIN preAppMainEntity, Action<PRE_APP_MAIN> setViewDataFun)
        {
            //借款用途
            ViewBag.LoanPurpose = (setViewDataFun.Method.Name == "SetViewDataCar" || setViewDataFun.Method.Name == "SetViewDataGJCar")
                ? GetCarLoanPurpose() : GetLoanPurpose();
            
            //期数
            ViewBag.TermList = new List<int>();
            //还款方式
            ViewBag.RePayTypeList = new List<DataType>();
            //产品列表
            ViewBag.ProductList = new List<QFPProduct>();

            if (preAppMainEntity != null)
            {
                //预申请单子ID
                ViewData["preAppId"] = preAppMainEntity.ID;
                //业务品种
                ViewData["proCode"] = preAppMainEntity.PRODUCT_CODE;
                ViewData["proName"] = preAppMainEntity.PRODUCT_NAME;
                ViewData["customerKind"] = preAppMainEntity.CUSTOMERTYPE;
                ViewData["logo"] = preAppMainEntity.LOGO;
                if (!string.IsNullOrEmpty(preAppMainEntity.APPLY_CITY_CODE))
                {
                    ViewData["edit_cityCode"] = preAppMainEntity.APPLY_CITY_CODE;
                    var appCity = AppCityService.FilterByPlatform(GlobalSetting.UsingPlatformForCityProduct_QAPP)
                        .FirstOrDefault(c => c.CITY_CODE == preAppMainEntity.APPLY_CITY_CODE);
                    if (appCity != null)
                    {
                        ViewData["edit_city"] = appCity.CITY_NAME;
                        //业务开办城市
                        //List<string> pros = appCity.PRODUCT_CODE.Split(',').ToList();
                        /*
                        var tempList = ProductInfoService.GetProductAllList(PInfoInterfaceURLAccount.productList.ToString()).Select(p => p.pProduct);
                        List<QFPProduct> productList = tempList.Where(item => pros.Contains(item.productCode)).ToList();
                        //产品列表
                        ViewBag.ProductList = productList;
                         * */
                        //还款方式列表
                        //ViewBag.RePayTypeList = GetRepyTypeList(preAppMainEntity.PRODUCT_CODE);

                        //if (pros.Contains(preAppMainEntity.PRODUCT_CODE))
                        //{
                        //    //期数
                        //    ViewBag.TermList = ProductInfoService.GetQFProductTerm(preAppMainEntity.PRODUCT_CODE);
                        //}
                    }
                }

                var preAppCustomerEntity = preAppMainEntity.PRE_APP_CUSTOMER.FirstOrDefault();
                if (preAppCustomerEntity != null)
                {
                    //客户名称
                    ViewData["customerName"] = preAppCustomerEntity.NAME;
                    //身份证号
                    ViewData["customerCardID"] = preAppCustomerEntity.ID_NO;
                }

                var preAppLoanEntity = preAppMainEntity.PRE_APP_LOAN.FirstOrDefault();
                if (preAppLoanEntity != null)
                {
                    //期数
                    ViewData["terms"] = preAppLoanEntity.TERMS;
                    //申请金额
                    ViewData["applyAmt"] = preAppLoanEntity.APPLY_AMT;

                    //借款用途
                    ViewData["loanPur"] = preAppLoanEntity.LOAN_PURPOSE;
                    //借款用途其他
                    ViewData["memoOfLoanPurposeOther"] = preAppLoanEntity.MEMO_OF_LOAN_PURPOSE_OTHER;
                }

                if (setViewDataFun != null)
                {
                    setViewDataFun(preAppMainEntity);
                }

            }
        }
        /// <summary>
        /// 根据业务开办城市和菜单加载产品列表ProductList
        /// </summary>
        /// <param name="preAppMainEntity"></param>
        /// <param name="LogoGroupMenu"></param>
        private void SetProductByLogoGroupMenu(PRE_APP_MAIN preAppMainEntity, string LogoGroupMenu)
        {
            if (preAppMainEntity != null)
            {
                if (!string.IsNullOrEmpty(preAppMainEntity.APPLY_CITY_CODE))
                {
                    //取城市菜单-产品配置
                    var cityProduct = CityProductService.FindByCityCodeAndMenuGroup(preAppMainEntity.APPLY_CITY_CODE, LogoGroupMenu);

                    if (cityProduct != null)
                    {
                        //业务开办城市
                        List<string> pros = cityProduct.PRODUCT_CODE.Split(',').ToList();
                        var tempList = ProductInfoService.GetProductAllList(PInfoInterfaceURLAccount.productList.ToString());
                        List<QFPProduct> productList = tempList
                            .Where(item => pros.Contains(item.pProduct.productCode))
                            .Select(p => p.pProduct)
                            .ToList();
                        //产品列表
                        ViewBag.ProductList = productList;
                        //还款方式列表
                        ViewBag.RePayTypeList = GetRepyTypeList(preAppMainEntity.PRODUCT_CODE);

                        if (pros.Contains(preAppMainEntity.PRODUCT_CODE))
                        {
                            //期数
                            ViewBag.TermList = ProductInfoService.GetQFProductTerm(preAppMainEntity.PRODUCT_CODE);
                        }
                    }
                }
            }
        }
        /// <summary>
        /// 验证表单必填字段信息，并将FormCollection转为Dictionary
        /// </summary>
        /// <param name="collection"></param>
        /// <returns>错误信息（如果没错则为空字符串）</returns>
        private Dictionary<string, string> CheckFormData(FormCollection collection, out string resultMsg)
        {
            resultMsg = string.Empty;
            if (!collection.AllKeys.Contains("preAppId") || string.IsNullOrEmpty(collection["preAppId"]))
            {
                resultMsg += "preAppId为空！";
            }
            //验证表单必填字段信息
            if (!collection.AllKeys.Contains("customerName") || string.IsNullOrEmpty(collection["customerName"]))
            {
                resultMsg += "客户姓名不能为空！";
            }
            if (!collection.AllKeys.Contains("customerIDCard") || string.IsNullOrEmpty(collection["customerIDCard"]))
            {
                resultMsg += "客户身份证不能为空！";
            }
            if (!collection.AllKeys.Contains("productCode") || string.IsNullOrEmpty(collection["productCode"]))
            {
                resultMsg += "业务品种不能为空！";
            }
            if (!collection.AllKeys.Contains("applyCity") || string.IsNullOrEmpty(collection["applyCity"]))
            {
                resultMsg += "业务开办城市不能为空！";
            }
            if (!collection.AllKeys.Contains("applyAmount") || string.IsNullOrEmpty(collection["applyAmount"]))
            {
                resultMsg += "请输入正确的申请金额！";
            }
            if (!collection.AllKeys.Contains("loanPurpose") || string.IsNullOrEmpty(collection["loanPurpose"]))
            {
                resultMsg += "请选择借款用途！";
            }
            else
            {
                if (collection["loanPurpose"].Trim() == "LoanPurposeOther")
                {
                    if (!collection.AllKeys.Contains("memoOfLoanPurposeOther")
                        || string.IsNullOrEmpty(collection["memoOfLoanPurposeOther"]))
                    {
                        resultMsg += "请输入借款用途其他信息！";
                    }
                }
            }
            if (!collection.AllKeys.Contains("productTerm") || string.IsNullOrEmpty(collection["productTerm"]))
            {
                resultMsg += "请选择申请期限！";
            }
            if (!collection.AllKeys.Contains("platform") || string.IsNullOrEmpty(collection["platform"]))
            {
                resultMsg += "合作渠道不能为空！";
            }
            //将collection转为dictionary
            var dic = collection.AllKeys.ToDictionary(key => key, key => collection[key]);

            return dic;
        }
        /// <summary>
        /// 验证表单必填字段信息，并将FormCollection转为Dictionary
        /// </summary>
        /// <param name="collection"></param>
        /// <returns>错误信息（如果没错则为空字符串）</returns>
        private Dictionary<string, string> CheckCarFormData(FormCollection collection, out string resultMsg)
        {
            resultMsg = string.Empty;
            if (!collection.AllKeys.Contains("preAppId") || string.IsNullOrEmpty(collection["preAppId"]))
            {
                resultMsg += "preAppId为空！";
            }
            //验证表单必填字段信息
            if (!collection.AllKeys.Contains("customerName") || string.IsNullOrEmpty(collection["customerName"]))
            {
                resultMsg += "客户姓名不能为空！";
            }
            if (!collection.AllKeys.Contains("customerIDCard") || string.IsNullOrEmpty(collection["customerIDCard"]))
            {
                resultMsg += "客户身份证不能为空！";
            }
            if (!collection.AllKeys.Contains("productCode") || string.IsNullOrEmpty(collection["productCode"]))
            {
                resultMsg += "业务品种不能为空！";
            }
            if (!collection.AllKeys.Contains("applyCity") || string.IsNullOrEmpty(collection["applyCity"]))
            {
                resultMsg += "业务开办城市不能为空！";
            }
            if (!collection.AllKeys.Contains("applyAmount") || string.IsNullOrEmpty(collection["applyAmount"]))
            {
                resultMsg += "请输入正确的资金需求！";
            }
            //借款用途
            if (collection.AllKeys.Contains("loanPurpose") && !string.IsNullOrEmpty(collection["loanPurpose"]))
            {
                if (collection["loanPurpose"].Trim() == "LoanPurposeOther")
                {
                    if (!collection.AllKeys.Contains("memoOfLoanPurposeOther")
                        || string.IsNullOrEmpty(collection["memoOfLoanPurposeOther"]))
                    {
                        resultMsg += "请输入借款用途其他信息！";
                    }
                }
            }
            else
            {
                var product = ProductInfoService.GetProductListByProductCode(PInfoInterfaceURLAccount.productCode.ToString(),
                    collection["productCode"]);
                if (!GlobalSetting.NoNeed_LoanPurpose_Product.Split(',').Contains(product != null ? product.pLogo.logo : string.Empty))  //不需要填借款用途产品不用提示
                {
                    resultMsg += "请选择借款用途！";
                }
            }
            //if (!collection.AllKeys.Contains("customerType") || string.IsNullOrEmpty(collection["customerType"]))
            //{
            //    resultMsg += "请选择客户类型！";
            //}
            if (!collection.AllKeys.Contains("productTerm") || string.IsNullOrEmpty(collection["productTerm"]))
            {
                resultMsg += "请选择申请期限！";
            }
            if (!collection.AllKeys.Contains("plantform") || string.IsNullOrEmpty(collection["plantform"]))
            {
                resultMsg += "合作渠道不能为空！";
            }
            //if (!collection.AllKeys.Contains("carType") || string.IsNullOrEmpty(collection["carType"]))
            //{
            //    resultMsg += "购车种类不能为空";
            //}
            //将collection转为dictionary
            var dic = collection.AllKeys.ToDictionary(key => key, key => collection[key]);

            return dic;
        }

        /// <summary>
        /// 还款方式
        /// </summary>
        /// <param name="productCode"></param>
        /// <returns></returns>
        private List<DataType> GetRepyTypeList(string productCode)
        {
            return ProductInfoService.GetRepayTypeByProductCode(productCode);
        }

        /// <summary>
        /// 借款用途
        /// </summary>
        /// <returns>字典集合</returns>
        private List<CR_DATA_DIC> GetLoanPurpose()
        {
            List<CR_DATA_DIC> loanPurposeList = new List<CR_DATA_DIC>();

            loanPurposeList = DicService.GetDICByParentCode("BORROW_USE").ToList();

            return loanPurposeList;
        }

        /// <summary>
        /// 车贷借款用途(GPS)
        /// </summary>
        /// <returns>字典集合</returns>
        private List<CR_DATA_DIC> GetCarLoanPurpose()
        {
            List<CR_DATA_DIC> loanPurposeList = new List<CR_DATA_DIC>();

            loanPurposeList = DicService.GetDICByParentCode("CarBORROW_USE").ToList();

            return loanPurposeList;
        }

        /// <summary>
        /// 城市列表
        /// </summary>
        /// <param name="menuGroup">菜单</param>
        /// <returns></returns>
        private List<APP_CITY> GetCityList(string menuGroup)
        {
            return AppCityService
                .FilterByMenuGroup(menuGroup)
                .ToList();
            //return AppCityService.FilterByPlatform(Global.GlobalSetting.UsingPlatformForCityProduct_QAPP).Where(c => c.ENABLE == 1).ToList();
        }

        /// <summary>
        /// 合作渠道
        /// </summary>
        /// <returns>字典集合</returns>
        private List<CR_DATA_DIC> GetPlantform()
        {
            List<CR_DATA_DIC> plantformList = new List<CR_DATA_DIC>();

            plantformList = DicService.GetDICByParentCode("CHANNELCOOPERATION").ToList();

            return plantformList;
        }
        /// <summary>
        /// 客户类型
        /// </summary>
        /// <returns>字典集合</returns>
        private List<CR_DATA_DIC> GetCustomerType()
        {
            List<CR_DATA_DIC> customerList = new List<CR_DATA_DIC>();
            customerList = DicService.GetDICByParentCode("CUSTOMER").ToList();
            return customerList;
        }

        /// <summary>
        /// 车贷Logo
        /// </summary>
        /// <returns></returns>
        private string GetCheDaiLogos()
        {
            var strs = Global.GlobalSetting.CheDaiLogos;
            return string.Join(",", strs);
        }

        /// <summary>
        /// 合作渠道
        /// </summary>
        /// <param name="channelCode">合作渠道Code</param>
        /// <returns></returns>
        private List<CR_DATA_DIC> GetCarPlantform(string channelCode)
        {
            List<CR_DATA_DIC> plantformList = new List<CR_DATA_DIC>();

            plantformList = DicService.GetDICByParentCode(channelCode).ToList();

            return plantformList;
        }
    }
}