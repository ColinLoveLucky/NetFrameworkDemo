/*********************
 * 作者：王瑞
 * 创建时间：2014/09/30
 * 功能：关于贷款申请页面的Controller
**********************/
using Microsoft.Practices.Unity;
using QK.QAPP.Entity;
using QK.QAPP.Infrastructure;
using QK.QAPP.Infrastructure.Cache;
using QK.QAPP.Infrastructure.Log4Net;
using QK.QAPP.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using QK.QAPP.Entity.ExtendEntity;
using QK.QAPP.Global;

namespace QK.QAPP.SalesCenter.Controllers
{
    public class ProductApplicationController : Controller
    {
        #region 属性

        [Dependency]
        public IQFProductInfoService ProductInfoService { get; set; }
        [Dependency]
        public IAPP_MAINSERVICE AppMainService { get; set; }
        [Dependency]
        public IAPP_LOANSERVICE AppLoanService { get; set; }
        [Dependency]
        public IAPP_CUSTOMERSERVICE AppCustomerService { get; set; }
        [Dependency]
        public IQFUserService QFUserService { get; set; }
        [Dependency]
        public IAPP_CITYSERVICE AppCityService { get; set; }
        [Dependency]
        public IAPP_STAFF_ONLYSERVICE AppStaffOnlyService { get; set; }
        [Dependency]
        public IAPP_CARINFOSERVICE AppCarInfoService { get; set; }
        [Dependency]
        public IAPP_AUTHSERVICE AppAuthService { get; set; }
        [Dependency]
        public ICacheProvider CacheService { get; set; }
        [Dependency]
        public IAPP_HOUSESERVICE appHouseService { get; set; }
        [Dependency]
        public ILoanApplicationService loanApplicationService { get; set; }
        [Dependency]
        public IAPP_CITY_PRODUCTSERVICE CityProductService { get; set; }

        public IApplicationService ApplicationService { get; set; }
        #endregion

        private static readonly string formNotEditMsg = "当前表单状态已更改，目前不可编辑，无法保存或提交！";

        #region 信贷申请

        /// <summary>
        /// 展示贷款申请页面
        /// </summary>
        /// <param name="app_id">需要展示数据的APP_ID（如果为空则表示添加数据）</param>
        /// <param name="isEdit">是否可编辑（"true"或"false"）</param>
        /// <returns>ViewResult</returns>
        [LogicalActionFilter(ActionSummary = "贷款申请：页面展示")]
        public ActionResult Application(string app_id, string isEdit)
        {
            ViewBag.LoanPurpose = GetLoanPurpose();
            ViewBag.CustomerType = GetCustomerType();
            ViewBag.Platform = GetPlantform();
            ViewData["CheDaiLogos"] = GetCheDaiLogos();
            if (string.IsNullOrEmpty(app_id))
            {
                return View("Application");
            }

            long appId = app_id.ToLong();
            APP_MAIN appMainEntity = AppMainService.Find(m => m.ID == appId).FirstOrDefault();

            if (appMainEntity == null)
            {
                ViewData["noPermission"] = "数据不存在！";
            }
            else
            {
                string resultMsg = CheckPermission(appMainEntity, isEdit, GlobalSetting.LogoGroupForMenu["CREDIT"]);
                if (!string.IsNullOrEmpty(resultMsg))
                {
                    //ViewData["noPermission"] = resultMsg;
                    return new RedirectResult("/Home/NoAuthorization");
                }

                if (!string.IsNullOrEmpty(isEdit) && isEdit == "true")
                {
                    ViewData["isEdit"] = "true";
                }
                else
                {
                    ViewData["isEdit"] = "false";
                }

                SetEditViewData(appMainEntity);

                /*描述：添加拒贷描述信息，此处根据权限和拒贷申请来判断是否显示拒贷信息，true(是)，false(否)，时间：2015-03-06，,添加人：leiz*/
                ViewData["IsDisplayRefuseLoan"] = loanApplicationService.IsDisplayRefuseLoan(appMainEntity.APP_STATUS);
            }

            return View("ApplicationEdit");

        }

        /// <summary>
        /// 处理贷款信息的新增表单的提交，APP_MAIN对象在方法创建
        /// </summary>
        /// <param name="collection">表单提交的数据</param>
        /// <returns>错误信息</returns>
        [HttpPost]
        [LogicalActionFilter(ActionSummary = "贷款申请(post)：新增贷款申请信息")]
        public ActionResult Application(FormCollection collection, string menuCode)
        {
            string resultMsg;
            string isRedirect = string.Empty;
            APP_MAIN appMainEntity = InitAppMainEntity(collection, out resultMsg);
            if (!string.IsNullOrEmpty(resultMsg))
            {
                return Json(new { resultMsg, isRedirect });
            }
            //如果传入了menuCode，则将menuCode保存到APP_AUTH表，供查询时区分从不同菜单进的相同logo件
            appMainEntity.InputMenuCode = menuCode;
            APP_LOAN appLoanEntity = InitAppLoanEntity(collection, appMainEntity, out resultMsg);
            if (!string.IsNullOrEmpty(resultMsg))
            {
                return Json(new { resultMsg, isRedirect });
            }

            APP_CUSTOMER appCustomerEntity = InitAppCustomerEntity(collection, appMainEntity, out resultMsg);
            if (!string.IsNullOrEmpty(resultMsg))
            {
                return Json(new { resultMsg, isRedirect });
            }

            APP_CARINFO appCarInfoEntity = InitAppCarInfoEntity(collection, appMainEntity, out resultMsg);
            if (!string.IsNullOrEmpty(resultMsg))
            {
                return Json(new { resultMsg, isRedirect });
            }

            APP_STAFF_ONLY appStaffOnlyEntity = InitAppStaffOnlyEntity(collection, appMainEntity, out resultMsg);
            if (!string.IsNullOrEmpty(resultMsg))
            {
                return Json(new { resultMsg, isRedirect });
            }
            APP_AUTH appAuthEntity = InitAppAuthEntity(appMainEntity);

            AppMainService.Add(appMainEntity);
            AppLoanService.Add(appLoanEntity);
            AppCustomerService.Add(appCustomerEntity);
            AppStaffOnlyService.Add(appStaffOnlyEntity);
            AppCarInfoService.Add(appCarInfoEntity);
            AppAuthService.Add(appAuthEntity);


            AppMainService.UnitOfWork.SaveChanges();

            resultMsg = "/LoanApplication/Application?dformCode=" + appMainEntity.LOGO + "&operation=1&appid=" + appMainEntity.ID;
            isRedirect = "true";

            //日志
            Infrastructure.Log4Net.LogWriter.Biz("提交贷款申请（部分1）", appMainEntity.ID + String.Empty, appMainEntity);
            Infrastructure.Log4Net.LogWriter.Biz("提交贷款申请（部分2）", appMainEntity.ID + String.Empty, appLoanEntity);
            Infrastructure.Log4Net.LogWriter.Biz("提交贷款申请（部分3）", appMainEntity.ID + String.Empty, new Dictionary<string, string>
            {
                {"客户姓名",appCustomerEntity.NAME},
                {"身份证号",appCustomerEntity.ID_NO}
            });
            LogWriter.Biz("创建当前用户权限数据", appMainEntity.ID + String.Empty, appAuthEntity);
            return Json(new { resultMsg, isRedirect });
        }

        /// <summary>
        /// 处理贷款信息的编辑保存
        /// </summary>
        /// <param name="collection">表单提交的数据</param>
        /// <returns>错误信息</returns>
        [LogicalActionFilter(ActionSummary = "贷款申请：保存编辑后的贷款申请信息")]
        public string ApplicationEdit(FormCollection collection)
        {
            string resultMsg = string.Empty;
            if (collection.AllKeys.Contains("appId") && !string.IsNullOrEmpty(collection["appId"]))
            {
                long appId = collection["appId"].ToLong();
                var appLoanEntity = AppLoanService.Find(l => l.APP_ID == appId).FirstOrDefault();
                if (appLoanEntity != null)
                {
                    if(!ApplicationService.CheckIsAllowEdit(appLoanEntity.APP_MAIN.APP_STATUS))
                    {
                        resultMsg = "当前表单状态不可编辑，无法保存！";
                        return resultMsg;
                    }
                    if (collection.AllKeys.Contains("applyAmount") && !string.IsNullOrEmpty(collection["applyAmount"]))
                    {
                        appLoanEntity.APPLY_AMT = collection["applyAmount"].ToDecimal();
                        //计算合同金额，前期咨询费，借款服务费
                        CalculateContractAmt(appLoanEntity);
                    }
                    else
                    {
                        resultMsg = "请输入正确的申请金额！";
                        return resultMsg;  //提示错误后直接返回，不保存下面内容 update by 张浩 on 2016-03-30
                    }

                    if (collection.AllKeys.Contains("payAmtMonthly") && !string.IsNullOrEmpty(collection["payAmtMonthly"]))
                    {
                        decimal? payAmotMonthly = collection["payAmtMonthly"].ToDecimal();
                        appLoanEntity.PAY_AMT_MONTHLY_ACCEPTABLE = payAmotMonthly;
                    }
                    else if (collection["payAmtMonthly"] == string.Empty)
                    {
                        appLoanEntity.PAY_AMT_MONTHLY_ACCEPTABLE = 0;
                    }
                    var appCarinfoEntity = appLoanEntity.APP_MAIN.APP_CARINFO.FirstOrDefault();
                    if (appCarinfoEntity != null)
                    {
                        if (collection["carSellingPrice"].IsDecimal())
                        {
                            appCarinfoEntity.CAR_SELLINGPRICE = collection["carSellingPrice"].ToDecimal();
                        }
                    }


                    AppLoanService.Update(appLoanEntity);
                    AppLoanService.UnitOfWork.SaveChanges();

                    //日志
                    Infrastructure.Log4Net.LogWriter.Biz("修改贷款申请金额及月还款", appId + String.Empty, new Dictionary<string, string>()
                    {
                        {"申请金额", appLoanEntity.APPLY_AMT + String.Empty},
                        {"可接受月还款", appLoanEntity.PAY_AMT_MONTHLY_ACCEPTABLE + String.Empty},
                        {"合同金额", appLoanEntity.LOAN_AMT_OF_CONTRACT + String.Empty},
                        {"借款咨询费(含风险金)", appLoanEntity.CONSULTATION_CHARGE_AMT + String.Empty},
                        {"借款服务费", appLoanEntity.SERVICE_CHARGE_AMT + String.Empty},
                        {"开票价格", appCarinfoEntity != null ? appCarinfoEntity.CAR_SELLINGPRICE + String.Empty : "未知"}
                    });
                }
            }
            else
            {
                resultMsg = "发生异常：appID为Null！";
            }

            return resultMsg;
        }

        #endregion

        #region 车贷申请（个金部）

        /// <summary>
        /// 展示贷款申请页面
        /// </summary>
        /// <param name="app_id">需要展示数据的APP_ID（如果为空则表示添加数据）</param>
        /// <param name="isEdit">是否可编辑（"true"或"false"）</param>
        /// <returns>ViewResult</returns>
        [LogicalActionFilter(ActionSummary = "贷款申请：页面展示")]
        public ActionResult GJCarApplication(string app_id, string isEdit)
        {
            ViewBag.LoanPurpose = GetCarLoanPurpose();
            ViewBag.CustomerType = GetCustomerType();
            ViewBag.Platform = GetGJCarPlantform();
            if (string.IsNullOrEmpty(app_id))
            {
                return View();
            }

            long appId = app_id.ToLong();
            APP_MAIN appMainEntity = AppMainService.Find(m => m.ID == appId).FirstOrDefault();

            if (appMainEntity == null)
            {
                ViewData["noPermission"] = "数据不存在！";
            }
            else
            {
                string resultMsg = CheckPermission(appMainEntity, isEdit, GlobalSetting.LogoGroupForMenu["GJVEHICLE"]);
                if (!string.IsNullOrEmpty(resultMsg))
                {
                    //ViewData["noPermission"] = resultMsg;
                    return new RedirectResult("/Home/NoAuthorization");
                }

                if (!string.IsNullOrEmpty(isEdit) && isEdit == "true")
                {
                    ViewData["isEdit"] = "true";
                }
                else
                {
                    ViewData["isEdit"] = "false";
                }

                SetEditViewData(appMainEntity);

                ViewData["IsDisplayRefuseLoan"] = loanApplicationService.IsDisplayRefuseLoanCar(appMainEntity.APP_STATUS);
            }

            return View();

        }

        #endregion

        #region 车贷申请(创新部)
        /// <summary>
        /// 展示贷款申请页面
        /// </summary>
        /// <param name="app_id">需要展示数据的APP_ID（如果为空则表示添加数据）</param>
        /// <param name="isEdit">是否可编辑（"true"或"false"）</param>
        /// <returns>ViewResult</returns>
        [LogicalActionFilter(ActionSummary = "贷款申请：页面展示")]
        public ActionResult CXCarApplication(string app_id, string isEdit)
        {
            ViewBag.LoanPurpose = GetCarLoanPurpose();
            ViewBag.CustomerType = GetCustomerType();
            ViewBag.Platform = GetCXCarPlantform("CHANNELCOOPERATION_CXVEHICLE");
            if (string.IsNullOrEmpty(app_id))
            {
                return View();
            }
            long appId = app_id.ToLong();
            APP_MAIN appMainEntity = AppMainService.Find(m => m.ID == appId).FirstOrDefault();
            if (appMainEntity == null)
            {
                ViewData["noPermission"] = "数据不存在！";
            }
            else
            {
                string resultMsg = CheckPermission(appMainEntity, isEdit, GlobalSetting.LogoGroupForMenu["GJVEHICLE"]);
                if (!string.IsNullOrEmpty(resultMsg))
                {
                    //ViewData["noPermission"] = resultMsg;
                    return new RedirectResult("/Home/NoAuthorization");
                }
                if (!string.IsNullOrEmpty(isEdit) && isEdit == "true")
                {
                    ViewData["isEdit"] = "true";
                }
                else
                {
                    ViewData["isEdit"] = "false";
                }
                SetEditViewData(appMainEntity);
                ViewData["IsDisplayRefuseLoan"] = loanApplicationService.IsDisplayRefuseLoanCar(appMainEntity.APP_STATUS);
            }

            return View();
        }

        #endregion

        #region 车贷申请（车贷部）

        /// <summary>
        /// 展示贷款申请页面
        /// </summary>
        /// <param name="app_id">需要展示数据的APP_ID（如果为空则表示添加数据）</param>
        /// <param name="isEdit">是否可编辑（"true"或"false"）</param>
        /// <returns>ViewResult</returns>
        [LogicalActionFilter(ActionSummary = "贷款申请：页面展示")]
        public ActionResult CarApplication(string app_id, string isEdit)
        {
            ViewBag.LoanPurpose = GetCarLoanPurpose();
            ViewBag.CustomerType = GetCustomerType();
            ViewBag.Platform = GetCarPlantform();
            if (string.IsNullOrEmpty(app_id))
            {
                return View();
            }

            long appId = app_id.ToLong();
            APP_MAIN appMainEntity = AppMainService.Find(m => m.ID == appId).FirstOrDefault();

            if (appMainEntity == null)
            {
                ViewData["noPermission"] = "数据不存在！";
            }
            else
            {
                string resultMsg = CheckPermission(appMainEntity, isEdit, GlobalSetting.LogoGroupForMenu["VEHICLE"]);
                if (!string.IsNullOrEmpty(resultMsg))
                {
                    //ViewData["noPermission"] = resultMsg;
                    return new RedirectResult("/Home/NoAuthorization");
                }

                if (!string.IsNullOrEmpty(isEdit) && isEdit == "true")
                {
                    ViewData["isEdit"] = "true";
                }
                else
                {
                    ViewData["isEdit"] = "false";
                }

                SetEditViewData(appMainEntity);

                ViewData["IsDisplayRefuseLoan"] = loanApplicationService.IsDisplayRefuseLoanCar(appMainEntity.APP_STATUS);
            }

            return View();

        }

        #endregion

        #region 房贷申请

        /// <summary>
        /// 展示贷款申请页面
        /// </summary>
        /// <param name="app_id">需要展示数据的APP_ID（如果为空则表示添加数据）</param>
        /// <param name="isEdit">是否可编辑（"true"或"false"）</param>
        /// <returns>ViewResult</returns>
        [LogicalActionFilter(ActionSummary = "贷款申请：页面展示")]
        public ActionResult HouseApplication(string app_id, string isEdit)
        {
            ViewBag.LoanPurpose = GetLoanPurpose();
            ViewBag.CustomerType = GetCustomerType();
            //ViewBag.Platform = GetPlantform();
            ViewBag.Platform = GetHousePlantform();

            ViewData["CheDaiLogos"] = GetCheDaiLogos();
            if (string.IsNullOrEmpty(app_id))
            {
                return View();
            }

            long appId = app_id.ToLong();
            APP_MAIN appMainEntity = AppMainService.Find(m => m.ID == appId).FirstOrDefault();

            if (appMainEntity == null)
            {
                ViewData["noPermission"] = "数据不存在！";
            }
            else
            {
                string resultMsg = CheckPermission(appMainEntity, isEdit, GlobalSetting.LogoGroupForMenu["HOUSE"]);
                if (!string.IsNullOrEmpty(resultMsg))
                {
                    //ViewData["noPermission"] = resultMsg;
                    return new RedirectResult("/Home/NoAuthorization");
                }

                if (!string.IsNullOrEmpty(isEdit) && isEdit == "true")
                {
                    ViewData["isEdit"] = "true";
                }
                else
                {
                    ViewData["isEdit"] = "false";
                }

                SetEditViewData(appMainEntity);

                ViewData["IsDisplayRefuseLoan"] = loanApplicationService.IsDisplayRefuseLoanHouse(appMainEntity.APP_STATUS);
            }

            return View();

        }

        /// <summary>
        /// 处理房贷信息的新增表单的提交，APP_MAIN对象在方法创建
        /// </summary>
        /// <param name="collection">表单提交的数据</param>
        /// <returns>错误信息</returns>
        [HttpPost]
        [LogicalActionFilter(ActionSummary = "房贷申请(post)：新增贷款申请信息")]
        public ActionResult HouseApplication(FormCollection collection)
        {
            string resultMsg;
            string isRedirect = string.Empty;
            var formDic = CheckHouseLoanFormData(collection, out resultMsg);
            if (!string.IsNullOrEmpty(resultMsg))
            {
                return Json(new { resultMsg, isRedirect });
            }
            var appMainEntity = loanApplicationService.HouseLoanApplication(formDic, out resultMsg);
            if (!string.IsNullOrEmpty(resultMsg))
            {
                return Json(new { resultMsg, isRedirect });
            }
            resultMsg = "/LoanApplication/Application?dformCode=" + appMainEntity.LOGO + "&operation=1&appid=" + appMainEntity.ID;
            isRedirect = "true";
            return Json(new { resultMsg, isRedirect });
        }

        /// <summary>
        /// 处理贷款信息的编辑保存(房贷重构版)
        /// </summary>
        /// <param name="collection">表单提交的数据</param>
        /// <returns>错误信息</returns>
        [HttpPost]
        [LogicalActionFilter(ActionSummary = "房贷申请：保存编辑后的贷款申请信息")]
        public string HouseApplicationEdit(FormCollection collection)
        {
            if (!ApplicationService.CheckIsAllowEdit(collection["appid"].ToLong()))  //验证表单是否处于可编辑状态
            {
                return formNotEditMsg;
            }
            string resultMsg;

            var formDic = collection.AllKeys.ToDictionary(key => key, key => collection[key]);
            //保存房贷修改
            var appLoan = loanApplicationService.HouseAppLoanEdit(formDic, out resultMsg);
            if (!string.IsNullOrEmpty(resultMsg))
            {
                return resultMsg;
            }


            return resultMsg;
        }

        /// <summary>
        /// 验证表单必填字段信息，并将FormCollection转为Dictionary
        /// </summary>
        /// <param name="collection"></param>
        /// <returns>错误信息（如果没错则为空字符串）</returns>
        private Dictionary<string, string> CheckHouseLoanFormData(FormCollection collection, out string resultMsg)
        {
            resultMsg = string.Empty;
            //--申请主表--
            if (!collection.AllKeys.Contains("productCode") || string.IsNullOrEmpty(collection["productCode"]))
            {
                resultMsg += "业务品种不能为空！";
            }
            //申请金额
            if (!collection.AllKeys.Contains("contractValue") || string.IsNullOrEmpty(collection["contractValue"]))
            {
                resultMsg += "请输入正确的合同金额！";
            }
            //可接受月还款
            if (!collection.AllKeys.Contains("payAmtMonthly") || string.IsNullOrEmpty(collection["payAmtMonthly"]))
            {
                //是否提示信息
            }
            //借款用途
            if (!collection.AllKeys.Contains("loanPurpose") || string.IsNullOrEmpty(collection["loanPurpose"]))
            {
                resultMsg = "请选择借款用途！";

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
            //期限
            if (!collection.AllKeys.Contains("productTerm") || string.IsNullOrEmpty(collection["productTerm"]))
            {
                resultMsg += "请选择申请期限！";
            }

            //还款方式
            if (!collection.AllKeys.Contains("repaymentType") || string.IsNullOrEmpty(collection["repaymentType"]))
            {
                //是否提示信息
            }
            //姓名
            if (!collection.AllKeys.Contains("customerName") || string.IsNullOrEmpty(collection["customerName"]))
            {
                resultMsg = "客户姓名不能为空！";
            }
            //ID
            if (!collection.AllKeys.Contains("customerIDCard") || string.IsNullOrEmpty(collection["customerIDCard"]))
            {
                resultMsg = "身份证号码不能为空！";
            }

            if (!collection.AllKeys.Contains("platform") || string.IsNullOrEmpty(collection["platform"]))
            {
                resultMsg += "合作渠道不能为空！";
            }
            if (!collection.AllKeys.Contains("assessmentPrice") || string.IsNullOrEmpty(collection["assessmentPrice"]))
            {
                //是否提示信息
            }
            //将collection转为dictionary
            var dic = collection.AllKeys.ToDictionary(key => key, key => collection[key]);

            return dic;
        }

        #endregion

        #region 极客贷申请

        /// <summary>
        /// 极客贷展示贷款申请页面
        /// </summary>
        /// <param name="app_id">需要展示数据的APP_ID（如果为空则表示添加数据）</param>
        /// <param name="isEdit">是否可编辑（"true"或"false"）</param>
        /// <returns>ViewResult</returns>
        [LogicalActionFilter(ActionSummary = "贷款申请：页面展示")]
        public ActionResult GeekApplication(string app_id, string isEdit)
        {
            ViewBag.LoanPurpose = GetLoanPurpose();
            ViewBag.Platform = GetPlantform();
            if (string.IsNullOrEmpty(app_id))
            {
                return View();
            }

            long appId = app_id.ToLong();
            APP_MAIN appMainEntity = AppMainService.Find(m => m.ID == appId).FirstOrDefault();

            if (appMainEntity == null)
            {
                ViewData["noPermission"] = "数据不存在！";
            }
            else
            {
                string resultMsg = CheckPermission(appMainEntity, isEdit, GlobalSetting.LogoGroupForMenu["GEEK"]);
                if (!string.IsNullOrEmpty(resultMsg))
                {
                    //ViewData["noPermission"] = resultMsg;
                    return new RedirectResult("/Home/NoAuthorization");
                }

                if (!string.IsNullOrEmpty(isEdit) && isEdit == "true")
                {
                    ViewData["isEdit"] = "true";
                }
                else
                {
                    ViewData["isEdit"] = "false";
                }

                SetEditViewData(appMainEntity);
                //是否显示拒贷信息
                ViewData["IsDisplayRefuseLoan"] = loanApplicationService.IsDisplayRefuseLoanGeek(appMainEntity.APP_STATUS);
            }

            return View();

        }

        /// <summary>
        /// 处理极客贷申请提交
        /// </summary>
        /// <param name="collection"></param>
        /// <returns></returns>
        [HttpPost]
        [LogicalActionFilter(ActionSummary = "极客贷(post)：新增贷款申请信息")]
        public ActionResult GeekApplication(FormCollection collection)
        {
            string resultMsg;
            string isRedirect = String.Empty;
            ;
            var formDic = CheckLoanFormData(collection, out resultMsg);
            if (!string.IsNullOrEmpty(resultMsg))
            {
                return Json(new { resultMsg, isRedirect });
            }
            var appMainEntity = loanApplicationService.GeekLoanApplication(formDic, out resultMsg);
            if (!string.IsNullOrEmpty(resultMsg))
            {
                return Json(new { resultMsg, isRedirect });
            }
            resultMsg = "/LoanApplication/Application?dformCode=" + appMainEntity.LOGO + "&operation=1&appid=" + appMainEntity.ID;
            isRedirect = "true";
            return Json(new { resultMsg, isRedirect });
        }

        /// <summary>
        /// 贷款信息编辑（极客贷）
        /// </summary>
        /// <param name="collection"></param>
        /// <returns></returns>
        [HttpPost]
        public string GeekApplicationEdit(FormCollection collection)
        {
            if (!ApplicationService.CheckIsAllowEdit(collection["appid"].ToLong()))  //验证表单是否处于可编辑状态
            {
                return formNotEditMsg;
            }
            string resultMsg;

            var formDic = collection.AllKeys.ToDictionary(key => key, key => collection[key]);
            //保存修改
            var appLoan = loanApplicationService.CreditAppLoanEdit(formDic, out resultMsg);
            if (!string.IsNullOrEmpty(resultMsg))
            {
                return resultMsg;
            }

            return resultMsg;
        }

        /// <summary>
        /// 验证表单字段信息
        /// </summary>
        /// <param name="collection"></param>
        /// <param name="resultMsg"></param>
        /// <returns></returns>
        private Dictionary<string, string> CheckLoanFormData(FormCollection collection, out string resultMsg)
        {
            resultMsg = string.Empty;
            //--申请主表--
            if (!collection.AllKeys.Contains("productCode") || string.IsNullOrEmpty(collection["productCode"]))
            {
                resultMsg += "业务品种不能为空！";
            }
            if (!collection.AllKeys.Contains("applyCity") || string.IsNullOrEmpty(collection["applyCity"]))
            {
                resultMsg = "业务开办城市不能为空！";
            }
            //申请金额
            if (!collection.AllKeys.Contains("contractValue") || string.IsNullOrEmpty(collection["contractValue"]))
            {
                resultMsg += "请输入正确的合同金额！";
            }
            //可接受月还款
            if (!collection.AllKeys.Contains("payAmtMonthly") || string.IsNullOrEmpty(collection["payAmtMonthly"]))
            {
                //是否提示信息
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
            
            //期限
            if (!collection.AllKeys.Contains("productTerm") || string.IsNullOrEmpty(collection["productTerm"]))
            {
                resultMsg += "请选择申请期限！";
            }

            //还款方式
            if (!collection.AllKeys.Contains("repaymentType") || string.IsNullOrEmpty(collection["repaymentType"]))
            {
                //是否提示信息
            }
            //姓名
            if (!collection.AllKeys.Contains("customerName") || string.IsNullOrEmpty(collection["customerName"]))
            {
                resultMsg = "客户姓名不能为空！";
            }
            //ID
            if (!collection.AllKeys.Contains("customerIDCard") || string.IsNullOrEmpty(collection["customerIDCard"]))
            {
                resultMsg = "身份证号码不能为空！";
            }

            if (!collection.AllKeys.Contains("platform") || string.IsNullOrEmpty(collection["platform"]))
            {
                resultMsg += "合作渠道不能为空！";
            }
            //将collection转为dictionary
            var dic = collection.AllKeys.ToDictionary(key => key, key => collection[key]);

            return dic;
        }

        #endregion

        #region 融誉100申请

        public ActionResult RyApplication(string app_id, string isEdit)
        {
            ViewBag.LoanPurpose = GetLoanPurpose();
            ViewBag.Platform = GetRyPlantform();
            if (string.IsNullOrEmpty(app_id))
            {
                return View();
            }
            long appId = app_id.ToLong();
            APP_MAIN appMainEntity = AppMainService.Find(a => a.ID == appId).FirstOrDefault();
            if (appMainEntity == null)
            {
                ViewData["noPermission"] = "数据不存在！";
            }
            else
            {
                string resultMsg = CheckPermission(appMainEntity, isEdit, GlobalSetting.LogoGroupForMenu["RY100"]);
                if (!string.IsNullOrEmpty(resultMsg))
                {
                    //ViewData["noPermission"] = resultMsg;
                    return new RedirectResult("/Home/NoAuthorization");
                }

                if (!string.IsNullOrEmpty(isEdit) && isEdit == "true")
                {
                    ViewData["isEdit"] = "true";
                }
                else
                {
                    ViewData["isEdit"] = "false";
                }

                SetEditViewData(appMainEntity);

                /*描述：添加拒贷描述信息*/
                ViewData["IsDisplayRefuseLoan"] = loanApplicationService.IsDisplayRefuseLoanRy(appMainEntity.APP_STATUS);
            }
            return View();
        }

        [HttpPost]
        public ActionResult RyApplication(FormCollection collection, string menuCode)
        {
            string resultMsg;
            string isRedirect = String.Empty;
            
            var formDic = CheckLoanFormData(collection, out resultMsg);
            if (!string.IsNullOrEmpty(resultMsg))
            {
                return Json(new { resultMsg, isRedirect });
            }

            //传入车贷menuCode保存到app_auth表，lys 2016-3-30
            formDic.Add("InputMenuCode", menuCode);

            var appMainEntity = loanApplicationService.RyLoanApplication(formDic, out resultMsg);
            if (!string.IsNullOrEmpty(resultMsg))
            {
                return Json(new { resultMsg, isRedirect });
            }
            resultMsg = "/LoanApplication/RyApplication?dformCode=" + appMainEntity.LOGO + "&operation=1&appid=" + appMainEntity.ID;
            isRedirect = "true";
            return Json(new { resultMsg, isRedirect });
        }

        [HttpPost]
        public string RyApplicationEdit(FormCollection collection)
        {
            if (!ApplicationService.CheckIsAllowEdit(collection["appid"].ToLong()))  //验证表单是否处于可编辑状态
            {
                return formNotEditMsg;
            }
            string resultMsg;

            var formDic = collection.AllKeys.ToDictionary(key => key, key => collection[key]);
            //保存修改
            var appLoan = loanApplicationService.CreditAppLoanEdit(formDic, out resultMsg);
            if (!string.IsNullOrEmpty(resultMsg))
            {
                return resultMsg;
            }

            return resultMsg;
        }
        #endregion

        #region 异步 Action

        /// <summary>
        /// 根据区域编号返回产品信息及还款方式
        /// 修改人：leiz
        /// 修改时间：20150403
        /// </summary>
        /// <param name="cityCode">城市编码</param>
        /// <returns>JSON数据（如果未找到数据，则返回空字符串）</returns>
        [LogicalActionFilter(ActionSummary = "贷款申请：查询产品信息及还款方式")]
        public ActionResult GetProductAndRepayType(string cityCode, string type)
        {
            cityCode = cityCode.Trim();

            //取城市菜单-产品配置
            var cityProduct = CityProductService.FindByCityCodeAndMenuGroup(cityCode, type);

            if (cityProduct != null)
            {
                List<string> pros = cityProduct.PRODUCT_CODE.Split(',').ToList();
                List<QFPProduct> productList = new List<QFPProduct>();

                productList = CacheService.GetFromCacheOrProxy<List<QFPProduct>>("QAPP_QFProductInfoList_" + cityCode + "_" + (String.IsNullOrEmpty(type) ? "ALL" : type), () =>
                {
                    var tempList = (ProductInfoService.GetProductAllList(PInfoInterfaceURLAccount.productList.ToString()));

                    var entityList = tempList
                        .Where(item => pros.Contains(item.pProduct.productCode))
                        .Select(o => o.pProduct)
                        .ToList();
                    return entityList;
                });

                var proList = productList.Select(p => new
                {
                    p.productCode,
                    p.productName
                });

                //var repayType = ProductInfoService.GetRepayTypeByProductCode(currentCity.COMPANY_CODE).FirstOrDefault();

                if (!proList.Any())
                {
                    return Content(string.Empty);
                }

                return Json(new { proList }, JsonRequestBehavior.AllowGet);
            }

            return Content(string.Empty);
        }

        /// <summary>
        /// 获取购车种类
        /// </summary>
        /// <param name="logo">产品logo</param>
        /// <returns>JSON数据</returns>
        [LogicalActionFilter(ActionSummary = "贷款申请：获取购车种类")]
        public ActionResult GetCarKinds(string logo)
        {
            var dicService = Ioc.GetService<ICR_DATA_DICService>();
            var relationDic = Global.GlobalSetting.LogoAndCarKindRelation;
            var carKinds = new List<CR_DATA_DIC>();
            if (relationDic.ContainsKey(logo))
            {
                carKinds = dicService.GetDICByParentCode(relationDic[logo]);
            }
            return Json(carKinds.Select(c => new
            {
                c.DATA_CODE,
                c.DATA_NAME
            }), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 根据产品编号获取 [期数] [费率及类型] [Logo]
        /// </summary>
        /// <param name="proCode">产品编号</param>
        /// <returns>JSON数据</returns>
        [LogicalActionFilter(ActionSummary = "贷款申请：获取产品期数，费率，类型，Logo")]
        public ActionResult GetProductInfo(string proCode)
        {
            proCode = proCode.Trim();

            //期数
            var termList = ProductInfoService.GetQFProductTerm(proCode);

            //费率
            QFPInterest chargeList = new QFPInterest();
            // = ProductInfoService.GetInterestList(PInfoInterfaceURLAccount.productCode.ToString(), proCode).FirstOrDefault();

            //logo
            string logoStr = string.Empty;
            //var pLogo = ProductInfoService.GetLogoList(PInfoInterfaceURLAccount.productCode.ToString(), proCode).FirstOrDefault();
            //if (pLogo != null)
            //{
            //    logoStr = pLogo.logo;
            //}

            //咨询费
            decimal consultationChargeRatio = 0;
            //var product = ProductInfoService.GetProductList(PInfoInterfaceURLAccount.productCode.ToString(), proCode).FirstOrDefault();
            //if (product != null)
            //{
            //    consultationChargeRatio = product.consultationChargeRatio;
            //}
            //还款方式
            List<DataType> repayType = new List<DataType>();

            var p = ProductInfoService.GetProductListByProductCode(PInfoInterfaceURLAccount.productCode.ToString(), proCode);
            if (p != null)
            {
                chargeList = p.pInterest;
                logoStr = p.pLogo.logo;
                consultationChargeRatio = p.pProduct.consultationChargeRatio;
                repayType = ProductInfoService.GetRepayTypeFromProInfo(p);
            }

            //还款方式
            //var repayType = ProductInfoService.GetRepayTypeByProductCode(proCode).FirstOrDefault();
            #region 根据配置显示申请金额的标题
            //修改人：张浩 date: 2016-04-12
            string loanAmountTitle = string.Empty;  //申请金额字段显示的标题名
            var dictLoanAmountTitle=GlobalSetting.CarLoan_LoanAmountTitle;
            if (dictLoanAmountTitle.ContainsKey(logoStr))   //如果选择的产品存在配置中
            {
                loanAmountTitle = dictLoanAmountTitle[logoStr];
            }
            #endregion

            return Json(new
            {
                termList,
                chargeList,
                logoStr,
                consultationChargeRatio,
                repayType,
                loanAmountTitle
            }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 根据产品编号获取[申请金额标题] [Logo]
        /// </summary>
        /// <param name="proCode">产品编号</param>
        /// <returns>JSON数据</returns>
        [LogicalActionFilter(ActionSummary = "贷款申请：获取产品Logo、申请金额标题")]
        public ActionResult GetInfoByProductCode(string proCode)
        {
            proCode = proCode.Trim();
            //logo
            string logoStr = string.Empty;
            var p = ProductInfoService.GetProductListByProductCode(PInfoInterfaceURLAccount.productCode.ToString(), proCode);
            if (p != null)
            {
                logoStr = p.pLogo.logo;
            }

            #region 根据配置显示申请金额的标题
            //修改人：张浩 date: 2016-04-12
            string loanAmountTitle = string.Empty;  //申请金额字段显示的标题名
            var dictLoanAmountTitle = GlobalSetting.CarLoan_LoanAmountTitle;
            if (dictLoanAmountTitle.ContainsKey(logoStr))   //如果选择的产品存在配置中
            {
                loanAmountTitle = dictLoanAmountTitle[logoStr];
            }
            #endregion

            return Json(new
            {
                logoStr,
                loanAmountTitle
            }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 城市信息
        /// 修改人：ruiwang
        /// 修改时间：20151225
        /// </summary>
        /// <param name="menuGroup">比如信贷为CREDIT</param>
        /// <returns>JSON数据（包含当前用户所在城市）</returns>
        [LogicalActionFilter(ActionSummary = "贷款申请：获取城市信息")]
        public ActionResult GetCities(string menuGroup)
        {
            //var cityList = AppCityService
            //    .FilterByPlantformAndMenu(Global.GlobalSetting.UsingPlatformForCityProduct_QAPP, menuGroup)
            //    .Where(c => c.ENABLE == 1)
            //    .Select(c => new
            //    {
            //        CityCode = c.AREA_CODE + "," + c.CITY_CODE,
            //        CityName = c.CITY_NAME
            //    });

            var cityList = AppCityService.FilterByMenuGroup(menuGroup)
                .Select(c => new
                {
                    CityCode = c.AREA_CODE + "," + c.CITY_CODE,
                    CityName = c.CITY_NAME
                });

            string currentCityCode = string.Empty;

            var currentUser = QFUserService.GetCurrentUser();
            if (currentUser.City != null)
            {
                currentCityCode = currentUser.City.CITY_CODE;
            }

            return Json(new { cityList, currentCityCode }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取申请金额
        /// </summary>
        /// <param name="appId">外键值APP_ID</param>
        /// <returns>JSON数据</returns>
        [LogicalActionFilter(ActionSummary = "贷款申请：获取申请金额")]
        public JsonResult GetApplyAmount(long appId)
        {
            decimal? amount = -1;
            var appLoan = AppLoanService.Find(l => l.APP_ID == appId).FirstOrDefault();
            if (appLoan != null)
            {
                amount = appLoan.APPLY_AMT;
            }

            return Json(amount, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region 创建对象APP_MIAN,APP_LOAN,APP_CUSTOMER,APP_STAFF_ONLY,APP_AUTH

        /// <summary>
        /// 创建并初始化APP_MAIN对象
        /// </summary>
        /// <param name="collection">请求表单数据</param>
        /// <param name="resultMsg">错误信息（输出参数）</param>
        /// <returns>APP_MAIN对象</returns>
        private APP_MAIN InitAppMainEntity(FormCollection collection, out string resultMsg)
        {
            APP_MAIN entity = new APP_MAIN(true);
            var applyNumberService = Ioc.GetService<IAPP_APPLY_SEQUENCESERVICE>();
            var currentUser = QFUserService.GetCurrentUser();
            resultMsg = string.Empty;
            string productRegularId = string.Empty; //申请号中产品编码
            string fit4CustomerType = string.Empty; //客户类型

            //--申请主表--
            if (collection.AllKeys.Contains("productCode") && !string.IsNullOrEmpty(collection["productCode"]))
            {
                entity.PRODUCT_CODE = collection["productCode"].Trim();

                entity.PRODUCT_NAME = string.Empty;
                entity.PROD_VERSION = string.Empty;
            }
            else
            {
                resultMsg = "业务品种不能为空！";
                return entity;
            }

            var product = ProductInfoService.GetProductListByProductCode(PInfoInterfaceURLAccount.productCode.ToString(),
                    entity.PRODUCT_CODE);

            if (product != null)
            {
                productRegularId = product.pProduct.productRegularId;
                fit4CustomerType = product.pProduct.fit4customerType;
                entity.PRODUCT_NAME = product.pProduct.productName;
                //产品版本
                entity.PROD_VERSION = product.pProduct.prodVersion;
                //产品类型
                entity.LOGO = product.pLogo.logo;
            }

            //从接口取数据，不从页面取
            //if (collection.AllKeys.Contains("productName") && !string.IsNullOrEmpty(collection["productCode"]))
            //{
            //    entity.PRODUCT_NAME =  collection["productName"].Trim();
            //}

            if (collection.AllKeys.Contains("applyCity") && !string.IsNullOrEmpty(collection["applyCity"]))
            {
                /*entity.APPLY_CITY_CODE = collection["applyCity"].Trim();--V2*/
                string[] citys = collection["applyCity"].Trim().Split(',');
                entity.APPLY_AREA_CODE = citys[0];//城市区号
                entity.APPLY_CITY_CODE = citys[1];//城市编码
            }
            else
            {
                resultMsg = "业务开办城市不能为空！";
                return entity;
            }
            //产品类型
            //var pLogo = ProductInfoService.GetLogoList(PInfoInterfaceURLAccount.productCode.ToString(), entity.PRODUCT_CODE).FirstOrDefault();
            //if (pLogo != null)
            //{
            //    entity.LOGO = pLogo.logo;
            //}
            
            //客户类型
            //if (Global.GlobalSetting.CheDaiLogos.Contains(entity.LOGO))
            //{
            //    if (collection.AllKeys.Contains("customerType") && !string.IsNullOrEmpty(collection["customerType"]))
            //    {
            //        entity.CUSTOMERTYPE = collection["customerType"].Trim();
            //    }
            //    else
            //    {
            //        resultMsg = "客户类型不能为空！";
            //        return entity;
            //    }
            //}
            //else
            //{
            //#1779 客户类型不在从页面获取，依照产品接口中的值
            entity.CUSTOMERTYPE = fit4CustomerType;
            //}

            //申请编号
            if (!string.IsNullOrEmpty(entity.LOGO) && !string.IsNullOrEmpty(entity.APPLY_CITY_CODE))
            {
                entity.APP_CODE = applyNumberService.GetApplyNumber(entity.LOGO,
                    string.IsNullOrEmpty(productRegularId) ? "00" : productRegularId,
                    entity.APPLY_CITY_CODE);
            }
            else
            {
                throw new Exception("LOGO:" + entity.LOGO + ";APPLY_CITY_CODE:" + entity.APPLY_CITY_CODE + " 为空，无法生成申请编号！");
            }
            //进件状态
            entity.APP_STATUS = EnterStatusType.PENDING.ToString();
            //创建者
            entity.CREATED_USER = currentUser.Account;
            //创建时间
            entity.CREATED_TIME = DateTime.Now;

            return entity;
        }

        /// <summary>
        /// 创建并初始化APP_LOAN对象
        /// </summary>
        /// <param name="collection">请求表单数据</param>
        /// <param name="appMain">APP_MAIN对象</param>
        /// <param name="resultMsg">错误信息（输出参数）</param>
        /// <returns>APP_LOAN对象</returns>
        private APP_LOAN InitAppLoanEntity(FormCollection collection, APP_MAIN appMain, out string resultMsg)
        {
            APP_LOAN entity = new APP_LOAN();
            //var chargeList = ProductInfoService.GetInterestList(PInfoInterfaceURLAccount.productCode.ToString(), appMain.PRODUCT_CODE).FirstOrDefault();
            //var product = ProductInfoService.GetProductList(PInfoInterfaceURLAccount.productCode.ToString(), appMain.PRODUCT_CODE).FirstOrDefault();
            var product = ProductInfoService.GetProductListByProductCode(PInfoInterfaceURLAccount.productCode.ToString(), appMain.PRODUCT_CODE);
            resultMsg = string.Empty;

            //--货款信息--
            entity.APP_ID = appMain.ID;
            //申请金额
            if (collection.AllKeys.Contains("applyAmount") && !string.IsNullOrEmpty(collection["applyAmount"]))
            {
                entity.APPLY_AMT = (collection["applyAmount"].Trim()).ToDecimal();
            }
            else
            {
                resultMsg = "请输入正确的申请金额！";
                return entity;
            }
            //可接受月还款
            if (collection.AllKeys.Contains("payAmtMonthly") && !string.IsNullOrEmpty(collection["payAmtMonthly"]))
            {
                entity.PAY_AMT_MONTHLY_ACCEPTABLE = collection["payAmtMonthly"].Trim().ToDecimal();
            }
            //若为微车贷，则贷款用途默认为消费 CarLoanPurposeBuy
            if (appMain.LOGO == "productCodeMiniCarLoan")
                entity.LOAN_PURPOSE = "CarLoanPurposeBuy";
            //借款用途
            if (collection.AllKeys.Contains("loanPurpose") && !string.IsNullOrEmpty(collection["loanPurpose"]))
            {
                entity.LOAN_PURPOSE = collection["loanPurpose"].Trim();
                if (entity.LOAN_PURPOSE == "LoanPurposeOther")
                {
                    if (collection.AllKeys.Contains("memoOfLoanPurposeOther") && !string.IsNullOrEmpty(collection["memoOfLoanPurposeOther"]))
                    {
                        entity.MEMO_OF_LOAN_PURPOSE_OTHER = collection["memoOfLoanPurposeOther"].Trim();
                    }
                    else
                    {
                        resultMsg = "请输入借款用途其他信息！";
                        return entity;
                    }
                }
            }
            else
            {
                if (!GlobalSetting.NoNeed_LoanPurpose_Product.Split(',').Contains(appMain.LOGO))  //不需要填借款用途产品不用提示
                {
                    resultMsg = "请选择借款用途！";
                    return entity;
                }
            }
            //期限
            if (collection.AllKeys.Contains("productTerm") && !string.IsNullOrEmpty(collection["productTerm"]))
            {
                entity.TERMS = (collection["productTerm"].Trim()).ToInt16();
            }
            else
            {
                resultMsg = "请选择申请期限！";
                return entity;
            }

            if (product != null)
            {
                //咨询费率
                entity.CONSULTATION_CHARGE_RATIO = product.pProduct.consultationChargeRatio;
                //罚息比率
                entity.DEFAULT_INTEREST_RATIO = product.pInterest.defaultInterestRatio;
                //服务费率
                entity.SERVICE_CHARGE_RATIO = product.pInterest.serviceChargeRatio;
                //利率类型
                entity.RATE_TYPE = product.pInterest.rateType;
                //利率
                entity.RATE = product.pInterest.rate;
            }

            //计算合同金额，前期咨询费，借款服务费
            CalculateContractAmt(entity);

            //还款方式
            if (collection.AllKeys.Contains("repaymentType") && !string.IsNullOrEmpty(collection["repaymentType"]))
            {
                entity.PAYTYPE = collection["repaymentType"].Trim();
            }

            return entity;
        }

        /// <summary>
        /// 计算合同金额，前期咨询费，借款服务费
        /// </summary>
        /// <param name="entity">APP_LOAN实体</param>
        /// <returns>合同金额</returns>
        private void CalculateContractAmt(APP_LOAN entity)
        {
            if (entity.APPLY_AMT.HasValue && entity.CONSULTATION_CHARGE_RATIO.HasValue && entity.SERVICE_CHARGE_RATIO.HasValue)
            {

                //合同金额 = 申请金额（到手金额）/（1 – 前期咨询费费率）【计算结果保留百位】
                entity.LOAN_AMT_OF_CONTRACT = entity.APPLY_AMT / (1 - entity.CONSULTATION_CHARGE_RATIO);
                entity.LOAN_AMT_OF_CONTRACT = AccurateHundredsDigit(entity.LOAN_AMT_OF_CONTRACT);


                //前期咨询费（咨询费）= 合同金额 - 申请金额
                entity.CONSULTATION_CHARGE_AMT = entity.LOAN_AMT_OF_CONTRACT - entity.APPLY_AMT;
                if (entity.CONSULTATION_CHARGE_AMT < 0)
                {
                    entity.CONSULTATION_CHARGE_AMT = 0;
                }

                //借款服务费（月管理费）= 合同金额 * 服务费费率
                entity.SERVICE_CHARGE_AMT = entity.LOAN_AMT_OF_CONTRACT * entity.SERVICE_CHARGE_RATIO;
            }
            else
            {
                Infrastructure.Log4Net.LogWriter.Error("合同金额计算出错！");
            }
        }

        private void CalculateContractAmtHouse(APP_LOAN entity, APP_MAIN appMain)
        {
            //判断如果是房贷申请
            if (Global.GlobalSetting.LogoGroupForMenu["HOUSE"].Contains(appMain.LOGO))
            {
                //TODO:计算房贷合同金额
            }
        }

        /// <summary>
        /// 结果保留到百位（四舍五入）
        /// </summary>
        /// <param name="amount">输入金额</param>
        /// <returns>保留百位后的金额</returns>
        private decimal? AccurateHundredsDigit(decimal? amount)
        {
            if (amount.HasValue)
            {
                amount = amount / 100;
                amount = Math.Round(amount.Value, MidpointRounding.AwayFromZero);
                return amount * 100;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 创建并初始化APP_CUSTOMER对象
        /// </summary>
        /// <param name="collection">请求表单数据</param>
        /// <param name="appMain">APP_MAIN对象</param>
        /// <param name="resultMsg">错误信息（输出参数）</param>
        /// <returns>APP_CUSTOMER对象</returns>
        private APP_CUSTOMER InitAppCustomerEntity(FormCollection collection, APP_MAIN appMain, out string resultMsg)
        {
            APP_CUSTOMER entity = new APP_CUSTOMER();
            resultMsg = string.Empty;

            //--客户信息--
            entity.APP_ID = appMain.ID;
            //姓名
            if (collection.AllKeys.Contains("customerName") && !string.IsNullOrEmpty(collection["customerName"]))
            {
                entity.NAME = collection["customerName"].Trim();
            }
            else
            {
                resultMsg = "客户姓名不能为空！";
                return entity;
            }
            //ID
            if (collection.AllKeys.Contains("customerIDCard") && !string.IsNullOrEmpty(collection["customerIDCard"]))
            {


                entity.ID_NO = collection["customerIDCard"].Trim();
            }
            else
            {
                resultMsg = "身份证号码不能为空！";
                return entity;
            }

            return entity;
        }

        /// <summary>
        /// 创建并初始化APP_STAFF_ONLY对象
        /// </summary>
        /// <param name="collection">请求表单数据</param>
        /// <param name="appMain">APP_MAIN对象</param>
        /// <returns>APP_STAFF_ONLY对象</returns>
        private APP_STAFF_ONLY InitAppStaffOnlyEntity(FormCollection collection, APP_MAIN appMain, out string resultMsg)
        {
            IStaffPickService staffPickService = Ioc.GetService<IStaffPickService>();
            APP_STAFF_ONLY entity = new APP_STAFF_ONLY();
            resultMsg = string.Empty;

            entity.APP_ID = appMain.ID;

            if (collection.AllKeys.Contains("platform") && !string.IsNullOrEmpty(collection["platform"]))
            {
                entity.CHANNEL_CODE = collection["platform"].Trim();
                entity.CHANNEL_NAME = Ioc.GetService<ICR_DATA_DICService>().GetDICByCode(entity.CHANNEL_CODE).DATA_NAME;
            }
            else
            {
                resultMsg = "合作渠道不能为空！";
                return entity;
            }
            var currentUser = QFUserService.GetCurrentUser();

            if (currentUser != null)
            {
                entity.CSAD_CODE = currentUser.Account;
                entity.CSAD_NAME = staffPickService.GetUserDisplayName(currentUser.RealName, currentUser.Code);
            }
            else
            {
                resultMsg = "初始化APP_STAFF_ONLY对象出错";
                Infrastructure.Log4Net.LogWriter.Error("初始化APP_STAFF_ONLY对象出错,currentUser为Null");
                return entity;
            }

            return entity;
        }

        /// <summary>
        /// 创建并初始化APP_HOUSE对象
        /// </summary>
        /// <param name="collection">请求表单数据</param>
        /// <param name="appMain">APP_MAIN对象</param>
        /// <returns>APP_HOUSE对象</returns>
        private APP_HOUSE InitAppHouseEntity(FormCollection collection, APP_MAIN appMain, out string resultMsg)
        {
            APP_HOUSE entity = new APP_HOUSE();
            entity.APP_ID = appMain.ID;
            resultMsg = string.Empty;
            if (collection.AllKeys.Contains("assessmentPrice") && !string.IsNullOrEmpty(collection["assessmentPrice"]))
            {
                entity.ASSESSMENT_VALUE = (collection["assessmentPrice"].Trim()).ToDecimal();
            }
            return entity;
        }


        /// <summary>
        /// 创建并初始化APP_CARINFO对象
        /// </summary>
        /// <param name="collection">请求表单数据</param>
        /// <param name="appMain">APP_MAIN对象</param>
        /// <param name="resultMsg">错误信息（输出参数）</param>
        /// <returns>APP_CARINFO对象</returns>
        private APP_CARINFO InitAppCarInfoEntity(FormCollection collection, APP_MAIN appMain, out string resultMsg)
        {
            resultMsg = string.Empty;
            APP_CARINFO entity = new APP_CARINFO();
            entity.APP_ID = appMain.ID;

            //if (Global.GlobalSetting.CheDaiLogos.Contains(appMain.LOGO))
            //{
                //if (collection.AllKeys.Contains("carType") && !string.IsNullOrEmpty(collection["carType"]))
                //{
                //    entity.CAR_KIND = collection["carType"].Trim();
                //    if (collection["carSellingPrice"].IsDecimal())
                //    {
                //        entity.CAR_SELLINGPRICE = collection["carSellingPrice"].ToDecimal();
                //    }
                //}
                //else
                //{
                //    resultMsg = "请选择购车种类！";
                //    return entity;
                //}
            if (collection.AllKeys.Contains("carSellingPrice")
                && !string.IsNullOrEmpty(collection["carSellingPrice"]))
            {
                if (collection["carSellingPrice"].IsDecimal())
                {
                    entity.CAR_SELLINGPRICE = collection["carSellingPrice"].ToDecimal();
                }
            }
            //}

            return entity;
        }

        /// <summary>
        /// 创建并初始化APP_AUTH对象
        /// </summary>
        /// <param name="appMain">APP_MAIN对象</param>
        /// <returns>APP_AUTH对象</returns>
        private APP_AUTH InitAppAuthEntity(APP_MAIN appMain)
        {
            var currentUser = QFUserService.GetCurrentUser();

            APP_AUTH entity = new APP_AUTH();
            entity.APP_ID = appMain.ID;
            entity.APP_CODE = appMain.APP_CODE;
            entity.MENUCODE = appMain.InputMenuCode;
            if (currentUser != null)
            {
                entity.ACCOUNT = currentUser.Account;

                var authInfo = QFUserService.GetUserAuthInfo(currentUser.UserId);
                entity.PARENT_ORGANIZATION = authInfo.ParentOrganization;
                entity.COMPANY = authInfo.Company;
                entity.AREA = authInfo.Area;
                entity.HEADQUARTER = authInfo.Headquarters;
            }
            else
            {
                LogWriter.Error("获取当前用户权限数据失败！");
            }

            return entity;
        }

        #endregion

        #region private 方法

        /// <summary>
        /// 设置贷款申请编辑页面的ViewData
        /// </summary>
        /// <param name="appMainEntity">APP_MAIN对象</param>
        private void SetEditViewData(APP_MAIN appMainEntity)
        {
            if (appMainEntity != null)
            {
                ViewData["appId"] = appMainEntity.ID;
                ViewData["logo"] = appMainEntity.LOGO;
                ViewData["proCode"] = appMainEntity.PRODUCT_CODE;
                ViewData["proName"] = appMainEntity.PRODUCT_NAME;
                ViewData["customerKind"] = appMainEntity.CUSTOMERTYPE;

                //描述：原APPLY_CITY_CODE改为城市编码，新增字段APPLY_AREA_CODE改为城市区号，
                //      由于上线时历史数据会更新，所以此处获取城市信息不变。
                //时间：20150403
                //修改人：leiz
                if (!string.IsNullOrEmpty(appMainEntity.APPLY_CITY_CODE))
                {
                    ViewData["edit_cityCode"] = appMainEntity.APPLY_CITY_CODE;
                    /*var appCity = AppCityService.Find(c => c.CITY_CODE == appMainEntity.APPLY_CITY_CODE).FirstOrDefault();--V2*/
                    APP_CITY appCity = AppCityService.FilterByPlatform(
                        Global.GlobalSetting.UsingPlatformForCityProduct_QAPP
                    ).Where(c => c.CITY_CODE == appMainEntity.APPLY_CITY_CODE).FirstOrDefault();
                    if (appCity != null)
                    {
                        ViewData["edit_city"] = appCity.CITY_NAME;
                    }
                }

                //APP_LOAN appLoanEntity = AppLoanService.Find(l => l.APP_ID == appMainEntity.ID).FirstOrDefault();
                APP_LOAN appLoanEntity = appMainEntity.APP_LOAN.FirstOrDefault();
                if (appLoanEntity != null)
                {
                    ViewData["contractAmt"] = appLoanEntity.LOAN_AMT_OF_CONTRACT;
                    ViewData["repayTypeCode"] = appLoanEntity.PAYTYPE;
                    ViewData["terms"] = appLoanEntity.TERMS;
                    ViewData["rate"] = appLoanEntity.RATE;
                    ViewData["interestRatio"] = appLoanEntity.DEFAULT_INTEREST_RATIO;
                    ViewData["applyAmt"] = appLoanEntity.APPLY_AMT;
                    ViewData["serviceChargeRatio"] = appLoanEntity.SERVICE_CHARGE_RATIO;
                    ViewData["serviceCharge"] = appLoanEntity.SERVICE_CHARGE_AMT;
                    ViewData["consultationChargeRatio"] = appLoanEntity.CONSULTATION_CHARGE_RATIO;
                    ViewData["consultationCharge"] = appLoanEntity.CONSULTATION_CHARGE_AMT;
                    ViewData["loanPur"] = appLoanEntity.LOAN_PURPOSE;
                    ViewData["memoOfLoanPurposeOther"] = appLoanEntity.MEMO_OF_LOAN_PURPOSE_OTHER;
                    ViewData["payAmtMonthly"] = appLoanEntity.PAY_AMT_MONTHLY_ACCEPTABLE;

                    var rePayTypes = ProductInfoService.GetRepayTypeByProductCode(appMainEntity.PRODUCT_CODE);
                    if (rePayTypes != null)
                    {
                        var rePayType = rePayTypes.Find(c => c.dataCode == appLoanEntity.PAYTYPE);
                        if (rePayType != null)
                        {
                            ViewData["repayType"] = rePayType.dataName;
                        }
                    }
                }
                //APP_CUSTOMER appCustomerEntity = AppCustomerService.Find(c => c.APP_ID == appMainEntity.ID).FirstOrDefault();
                APP_CUSTOMER appCustomerEntity = appMainEntity.APP_CUSTOMER.FirstOrDefault();
                if (appCustomerEntity != null)
                {
                    ViewData["customerName"] = appCustomerEntity.NAME;
                    ViewData["customerCardID"] = appCustomerEntity.ID_NO;
                }

                //APP_STAFF_ONLY appSaffOnlyEntity = AppStaffOnlyService.Find(s => s.APP_ID == appMainEntity.ID).FirstOrDefault();
                APP_STAFF_ONLY appSaffOnlyEntity = appMainEntity.APP_STAFF_ONLY.FirstOrDefault();
                if (appSaffOnlyEntity != null)
                {
                    ViewData["channel"] = appSaffOnlyEntity.CHANNEL_CODE + " " + appSaffOnlyEntity.CHANNEL_NAME;
                    ViewData["channelColde"] = appSaffOnlyEntity.CHANNEL_CODE;
                }

                APP_CARINFO appCarInfoEntity = appMainEntity.APP_CARINFO.FirstOrDefault();
                if(appCarInfoEntity != null)
                {
                    ViewData["carKindCode"] = appCarInfoEntity.CAR_KIND;
                    ViewData["car_sellingprice"] = appCarInfoEntity.CAR_SELLINGPRICE;
                    ICR_DATA_DICService dicService = Ioc.GetService<ICR_DATA_DICService>();
                    ViewData["carKindName"] = dicService.GetDICNameByCode(appCarInfoEntity.CAR_KIND);
                }

                //评估价值
                APP_HOUSE appHouseInfoEntity = appMainEntity.APP_HOUSE.FirstOrDefault();
                if (appHouseInfoEntity != null)
                {
                    ViewData["assessmentPrice"] = appHouseInfoEntity.ASSESSMENT_VALUE;
                }

                if (Global.GlobalSetting.CheDaiLogos.Contains(appMainEntity.LOGO))
                {
                    ViewData["carDisplay"] = "display";
                }

            }

        }

        /// <summary>
        /// 验证记录所在状态是否有操作权限
        /// </summary>
        /// <param name="appMainEntity">APP_MAIN对象</param>
        /// <param name="operation">操作</param>
        /// <param name="logos">可请求的Logo列表</param>
        /// <returns>消息（如果为空则表示有权限）</returns>
        private string CheckPermission(APP_MAIN appMainEntity, string operation, List<string> logos)
        {
            if (appMainEntity != null)
            {
                //var staffOnly = AppStaffOnlyService.FirstOrDefault(c => c.APP_ID == appMainEntity.ID);
                //switch (operation)
                //{
                //    case ENUM_FormOperation.EDIT:
                //        if (appMainEntity.APP_STATUS != EnterStatusType.PENDING.ToString())
                //        {
                //            return "该申请现阶段不提供编辑！";
                //        }
                //        break;
                //    default:
                //        break;
                //}
                if (!logos.Contains(appMainEntity.LOGO))
                    return "无法请求其他产品数据！";

                if (appMainEntity.APP_STATUS != EnterStatusType.PENDING.ToString())
                {
                    if (operation == "true")
                    {
                        return "该申请现阶段不提供编辑！";
                    }

                    return QFUserService.CheckDataPermission(appMainEntity.ID) ? string.Empty : "您没有权限这么做！";
                }

                return string.Empty;
            }

            return "所请求的数据不存在！";
        }

        /// <summary>
        /// 车贷Logo（车贷部）
        /// </summary>
        /// <returns></returns>
        private string GetCheDaiLogos()
        {
            var strs = Global.GlobalSetting.CheDaiLogos;
            return string.Join(",", strs);
        }

        /// <summary>
        /// 车贷Logo（个金部）
        /// </summary>
        /// <returns></returns>
        private string GetGJCheDaiLogos()
        {
            var strs = Global.GlobalSetting.GJCheDaiLogos;
            return string.Join(",", strs);
        }

        /// <summary>
        /// 贷款用途
        /// </summary>
        /// <returns>字典集合</returns>
        private List<CR_DATA_DIC> GetLoanPurpose()
        {
            ICR_DATA_DICService dicService = Ioc.GetService<ICR_DATA_DICService>();
            List<CR_DATA_DIC> loanPurposeList = new List<CR_DATA_DIC>();
            loanPurposeList = dicService.GetDICByParentCode("BORROW_USE").ToList();
            return loanPurposeList;
        }

        /// <summary>
        /// 车贷贷款用途(GPS)
        /// </summary>
        /// <returns>字典集合</returns>
        private List<CR_DATA_DIC> GetCarLoanPurpose()
        {
            ICR_DATA_DICService dicService = Ioc.GetService<ICR_DATA_DICService>();

            List<CR_DATA_DIC> loanPurposeList = new List<CR_DATA_DIC>();

            loanPurposeList = dicService.GetDICByParentCode("CarBORROW_USE").ToList();

            return loanPurposeList;
        }

        /// <summary>
        /// 客户类型
        /// </summary>
        /// <returns>字典集合</returns>
        private List<CR_DATA_DIC> GetCustomerType()
        {
            ICR_DATA_DICService dicService = Ioc.GetService<ICR_DATA_DICService>();
            List<CR_DATA_DIC> customerList = new List<CR_DATA_DIC>();
            customerList = dicService.GetDICByParentCode("CUSTOMER").ToList();
            return customerList;
        }

        /// <summary>
        /// 合作渠道
        /// </summary>
        /// <returns>字典集合</returns>
        private List<CR_DATA_DIC> GetPlantform()
        {
            ICR_DATA_DICService dicService = Ioc.GetService<ICR_DATA_DICService>();
            List<CR_DATA_DIC> plantformList = new List<CR_DATA_DIC>();
            plantformList = dicService.GetDICByParentCode("CHANNELCOOPERATION").ToList(); // CHANNELCOOPERATION:CR_DATA_DIC表data_code
            return plantformList;
        }

        /// <summary>
        /// 房贷合作渠道
        /// </summary>
        /// <returns>字典集合</returns>
        private List<CR_DATA_DIC> GetHousePlantform()
        {
            ICR_DATA_DICService dicService = Ioc.GetService<ICR_DATA_DICService>();
            List<CR_DATA_DIC> plantformList = new List<CR_DATA_DIC>();
            plantformList = dicService.GetDICByParentCode("CHANNELCOOPERATION_HOUSE").ToList(); // CHANNELCOOPERATION:CR_DATA_DIC表data_code
            return plantformList;
        }

        /// <summary>
        /// 车贷（个金）合作渠道
        /// </summary>
        /// <returns></returns>
        private List<CR_DATA_DIC> GetGJCarPlantform()
        {
            ICR_DATA_DICService dicService = Ioc.GetService<ICR_DATA_DICService>();
            List<CR_DATA_DIC> plantformList = new List<CR_DATA_DIC>();
            plantformList = dicService.GetDICByParentCode("CHANNELCOOPERATION_GJVEHICLE").ToList();
            return plantformList;
        }

        #region 车贷（创新）合作渠道
        /// <summary>
        /// 车贷（创新）合作渠道。
        /// add by shawn 2016年5月26日11:36:11
        /// </summary>
        /// <returns></returns>
        private List<CR_DATA_DIC> GetCXCarPlantform(string dataCode)
        {
            ICR_DATA_DICService dicService = Ioc.GetService<ICR_DATA_DICService>();
            List<CR_DATA_DIC> plantformList = new List<CR_DATA_DIC>();
            plantformList = dicService.GetDICByParentCode(dataCode).ToList();
            return plantformList;
        }
        #endregion

        /// <summary>
        /// 车贷合作渠道
        /// </summary>
        /// <returns></returns>
        private List<CR_DATA_DIC> GetCarPlantform()
        {
            ICR_DATA_DICService dicService = Ioc.GetService<ICR_DATA_DICService>();

            List<CR_DATA_DIC> plantformList = new List<CR_DATA_DIC>();

            plantformList = dicService.GetDICByParentCode("CHANNELCOOPERATION_VEHICLE").ToList();

            return plantformList;
        } 

        /// <summary>
        /// 融誉100合作渠道
        /// </summary>
        /// <returns></returns>
        private List<CR_DATA_DIC> GetRyPlantform()
        {
            ICR_DATA_DICService dicService = Ioc.GetService<ICR_DATA_DICService>();
            List<CR_DATA_DIC> list = new List<CR_DATA_DIC>();
            list = dicService.GetDICByParentCode("CHANNELCOOPERATION_RY").ToList();
            return list;
        }

        #endregion
    }
}