using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QK.QAPP.IServices;
using QK.QAPP.Entity;
using Microsoft.Practices.Unity;
using QK.QAPP.Infrastructure.Cache;
using QK.QAPP.Infrastructure;
using QK.QAPP.Infrastructure.Log4Net;


namespace QK.QAPP.Services
{
           
    public class LoanApplicationService : ILoanApplicationService
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
        #endregion

        #region 融誉100

        public APP_MAIN RyLoanApplication(Dictionary<string, string> formDic, out string resultMsg)
        {
            resultMsg = string.Empty;
            return CreateCreditLoan(formDic, out resultMsg);
        }
        
        #endregion

        #region 极客贷

        /// <summary>
        /// 极客贷申请
        /// </summary>
        /// <param name="formDic"></param>
        /// <param name="resultMsg"></param>
        /// <returns></returns>
        public APP_MAIN GeekLoanApplication(Dictionary<string, string> formDic, out string resultMsg)
        {
            resultMsg = string.Empty;
            return CreateCreditLoan(formDic, out resultMsg);
        }

        #endregion

        /// <summary>
        /// 创建信贷申请
        /// </summary>
        /// <param name="formDic"></param>
        /// <param name="resultMsg"></param>
        /// <returns></returns>
        private APP_MAIN CreateCreditLoan(Dictionary<string, string> formDic, out string resultMsg)
        {
            resultMsg = string.Empty;
            if (formDic == null || formDic.Keys.Count == 0)
            {
                resultMsg += "贷款申请表单为空！";
                return null;
            }
            var appMainEntity = InitAppMainEntity(formDic, out resultMsg);
            var appLoanEntity = InitAppLoanEntity(formDic, appMainEntity, out resultMsg);
            var appCustomerEntity = InitAppCustomerEntity(formDic, appMainEntity, out resultMsg);
            var appStaffOnlyEntity = InitAppStaffOnlyEntity(formDic, appMainEntity, out resultMsg);
            var appAuthEntity = InitAppAuthEntity(appMainEntity, appStaffOnlyEntity);

            AppMainService.Add(appMainEntity);
            AppLoanService.Add(appLoanEntity);
            AppCustomerService.Add(appCustomerEntity);
            AppStaffOnlyService.Add(appStaffOnlyEntity);
            AppAuthService.Add(appAuthEntity);

            AppMainService.UnitOfWork.SaveChanges();

            //日志
            Infrastructure.Log4Net.LogWriter.Biz("提交贷款申请（部分1）", appMainEntity.ID + String.Empty, appMainEntity);
            Infrastructure.Log4Net.LogWriter.Biz("提交贷款申请（部分2）", appMainEntity.ID + String.Empty, appLoanEntity);
            Infrastructure.Log4Net.LogWriter.Biz("提交贷款申请（部分3）", appMainEntity.ID + String.Empty, new Dictionary<string, string>
            {
                {"客户姓名",appCustomerEntity.NAME},
                {"身份证号",appCustomerEntity.ID_NO}
            });
            LogWriter.Biz("创建当前用户权限数据", appMainEntity.ID + String.Empty, appAuthEntity);
            return appMainEntity;
        }

        /// <summary>
        /// 贷款信息编辑（信贷）
        /// </summary>
        /// <param name="formDic"></param>
        /// <param name="resultMsg"></param>
        /// <returns></returns>
        public APP_LOAN CreditAppLoanEdit(Dictionary<string, string> formDic, out string resultMsg)
        {
            resultMsg = String.Empty;
            if (formDic.Keys.Contains("appId") && !string.IsNullOrEmpty(formDic["appId"]))
            {
                long appId = formDic["appId"].ToLong();
                var appLoanEntity = AppLoanService.Find(l => l.APP_ID == appId).FirstOrDefault();
                if (appLoanEntity != null)
                {
                    if (formDic.Keys.Contains("applyAmount") && !string.IsNullOrEmpty(formDic["applyAmount"]))
                    {
                        appLoanEntity.APPLY_AMT = formDic["applyAmount"].ToDecimal();
                        //计算合同金额，前期咨询费，借款服务费
                        CalculateContractAmt(appLoanEntity);

                    }
                    else
                    {
                        resultMsg = "请输入正确的申请金额！";
                    }

                    if (formDic.Keys.Contains("payAmtMonthly") && !string.IsNullOrEmpty(formDic["payAmtMonthly"]))
                    {
                        decimal? payAmotMonthly = formDic["payAmtMonthly"].ToDecimal();
                        appLoanEntity.PAY_AMT_MONTHLY_ACCEPTABLE = payAmotMonthly;
                    }
                    else if (formDic["payAmtMonthly"] == string.Empty)
                    {
                        appLoanEntity.PAY_AMT_MONTHLY_ACCEPTABLE = 0;
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
                        {"借款服务费", appLoanEntity.SERVICE_CHARGE_AMT + String.Empty}
                    });

                    return appLoanEntity;
                }
                else
                {
                    resultMsg = "未取到当前进件的贷款信息！";
                }
            }
            else
            {
                resultMsg = "发生异常：appID为Null！";
            }

            return null;
        }

        

        /// <summary>
        /// 房贷申请
        /// </summary>
        /// <param name="formDic">房贷申请表单</param>
        /// <param name="resultMsg">错误信息</param>
        /// <returns></returns>
        public APP_MAIN HouseLoanApplication(Dictionary<string, string> formDic, out string resultMsg)
        {
            resultMsg = string.Empty;
            return CreateHouseLoan(formDic,out resultMsg);
        }
        /// <summary>
        /// 创建房贷申请
        /// </summary>
        /// <param name="formDic">房贷申请表单</param>
        /// <param name="resultMsg">错误信息</param>
        /// <returns></returns>
        public APP_MAIN CreateHouseLoan(Dictionary<string, string> formDic, out string resultMsg)
        {
            resultMsg = string.Empty;
            if (formDic.Keys.Count==0||formDic==null)
            {
                resultMsg += "贷款申请表单为空！";
                return null;
            }
            APP_MAIN appMainEntity = InitAppMainEntity(formDic, out resultMsg);
            APP_LOAN appLoanEntity = InitAppLoanEntityHouse(formDic, appMainEntity, out resultMsg);
            APP_CUSTOMER appCustomerEntity = InitAppCustomerEntity(formDic, appMainEntity, out resultMsg);
            APP_STAFF_ONLY appStaffOnlyEntity = InitAppStaffOnlyEntity(formDic, appMainEntity, out resultMsg);
            APP_HOUSE appHouseInfo = InitAppHouseEntity(formDic, appMainEntity, out resultMsg);            //房贷评估价值保存
            APP_AUTH appAuthEntity = InitAppAuthEntity(appMainEntity, appStaffOnlyEntity);

            AppMainService.Add(appMainEntity);
            AppLoanService.Add(appLoanEntity);
            AppCustomerService.Add(appCustomerEntity);
            AppStaffOnlyService.Add(appStaffOnlyEntity);
            AppAuthService.Add(appAuthEntity);
            appHouseService.Add(appHouseInfo);

            AppMainService.UnitOfWork.SaveChanges();

            //日志
            Infrastructure.Log4Net.LogWriter.Biz("提交贷款申请（部分1）", appMainEntity.ID + String.Empty, appMainEntity);
            Infrastructure.Log4Net.LogWriter.Biz("提交贷款申请（部分2）", appMainEntity.ID + String.Empty, appLoanEntity);
            Infrastructure.Log4Net.LogWriter.Biz("提交贷款申请（部分3）", appMainEntity.ID + String.Empty, new Dictionary<string, string>
            {
                {"客户姓名",appCustomerEntity.NAME},
                {"身份证号",appCustomerEntity.ID_NO}
            });
            LogWriter.Biz("创建当前用户权限数据", appMainEntity.ID + String.Empty, appAuthEntity);
            return appMainEntity;
        }
        /// <summary>
        /// 创建并初始化APP_MAIN对象
        /// </summary>
        /// <param name="formDic">请求表单数据</param>
        /// <param name="resultMsg">错误信息（输出参数）</param>
        /// <returns>APP_MAIN对象</returns>
        private APP_MAIN InitAppMainEntity(Dictionary<string, string> formDic, out string resultMsg)
        {
            APP_MAIN entity = new APP_MAIN(true);
            var applyNumberService = Ioc.GetService<IAPP_APPLY_SEQUENCESERVICE>();
            var currentUser = QFUserService.GetCurrentUser();
            resultMsg = string.Empty;
            string productRegularId = string.Empty; //申请号中产品编码
            string fit4CustomerType = string.Empty; //客户类型

            //--申请主表--
            if (formDic.ContainsKey("productCode"))
            {
                entity.PRODUCT_CODE = formDic["productCode"].Trim();
                entity.PRODUCT_NAME = string.Empty;
                entity.PROD_VERSION = string.Empty;
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
            if (formDic.ContainsKey("applyCity"))
            {
                string[] citys = formDic["applyCity"].Trim().Split(',');
                entity.APPLY_AREA_CODE = citys[0];//城市区号
                entity.APPLY_CITY_CODE = citys[1];//城市编码
            }
            //客户类型
            if (Global.GlobalSetting.HouseLogos.Contains(entity.LOGO))
            {
                if (formDic.ContainsKey("customerType"))
                {
                    entity.CUSTOMERTYPE = formDic["customerType"].Trim();
                }        
            }
            else
            {
                entity.CUSTOMERTYPE = fit4CustomerType;
            }
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

            //menuCode处理，lys 2016-3-30
            if (formDic.ContainsKey("InputMenuCode"))
            {
                entity.InputMenuCode = formDic["InputMenuCode"].Trim();
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
        /// <param name="formDic">请求表单数据</param>
        /// <param name="appMain">APP_MAIN对象</param>
        /// <param name="resultMsg">错误信息（输出参数）</param>
        /// <returns>APP_LOAN对象</returns>
        private APP_LOAN InitAppLoanEntity(Dictionary<string, string> formDic, APP_MAIN appMain, out string resultMsg)
        {
            APP_LOAN entity = new APP_LOAN();
            var product = ProductInfoService.GetProductListByProductCode(PInfoInterfaceURLAccount.productCode.ToString(), appMain.PRODUCT_CODE);
            resultMsg = string.Empty;

            //--货款信息--
            entity.APP_ID = appMain.ID;
            //申请金额
            if (formDic.ContainsKey("applyAmount") && !string.IsNullOrEmpty(formDic["applyAmount"]))
            {
                entity.APPLY_AMT = (formDic["applyAmount"].Trim()).ToDecimal();
            }
            //可接受月还款
            if (formDic.ContainsKey("payAmtMonthly") && !string.IsNullOrEmpty(formDic["payAmtMonthly"]))
            {
                entity.PAY_AMT_MONTHLY_ACCEPTABLE = formDic["payAmtMonthly"].Trim().ToDecimal();
            }

            //借款用途
            if (formDic.ContainsKey("loanPurpose"))
            {
                entity.LOAN_PURPOSE = formDic["loanPurpose"].Trim();
                if (entity.LOAN_PURPOSE == "LoanPurposeOther")
                {
                    if (formDic.ContainsKey("memoOfLoanPurposeOther"))
                    {
                        entity.MEMO_OF_LOAN_PURPOSE_OTHER = formDic["memoOfLoanPurposeOther"].Trim();
                    }
                }
            }
            //期限
            if (formDic.ContainsKey("productTerm") && !string.IsNullOrEmpty(formDic["productTerm"]))
            {
                entity.TERMS = (formDic["productTerm"].Trim()).ToInt16();
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
            if (formDic.ContainsKey("repaymentType") )
            {
                entity.PAYTYPE = formDic["repaymentType"].Trim();
            }

            return entity;
        }
        /// <summary>
        /// 创建并初始化APP_LOAN对象
        /// </summary>
        /// <param name="formDic">请求表单数据</param>
        /// <param name="appMain">APP_MAIN对象</param>
        /// <param name="resultMsg">错误信息（输出参数）</param>
        /// <returns>APP_LOAN对象</returns>
        private APP_LOAN InitAppLoanEntityHouse(Dictionary<string, string> formDic, APP_MAIN appMain, out string resultMsg)
        {
            APP_LOAN entity = new APP_LOAN();
            var product = ProductInfoService.GetProductListByProductCode(PInfoInterfaceURLAccount.productCode.ToString(), appMain.PRODUCT_CODE);
            resultMsg = string.Empty;

            //--货款信息--
            entity.APP_ID = appMain.ID;
            //合同金额
            if (formDic.ContainsKey("contractValue") && !string.IsNullOrEmpty(formDic["contractValue"]))
            {
                entity.LOAN_AMT_OF_CONTRACT = (formDic["contractValue"].Trim()).ToDecimal();
            }
            //可接受月还款
            if (formDic.ContainsKey("payAmtMonthly") && !string.IsNullOrEmpty(formDic["payAmtMonthly"]))
            {
                entity.PAY_AMT_MONTHLY_ACCEPTABLE = formDic["payAmtMonthly"].Trim().ToDecimal();
            }

            //借款用途
            if (formDic.ContainsKey("loanPurpose"))
            {
                entity.LOAN_PURPOSE = formDic["loanPurpose"].Trim();
                if (entity.LOAN_PURPOSE == "LoanPurposeOther")
                {
                    if (formDic.ContainsKey("memoOfLoanPurposeOther"))
                    {
                        entity.MEMO_OF_LOAN_PURPOSE_OTHER = formDic["memoOfLoanPurposeOther"].Trim();
                    }
                }
            }
            //期限
            if (formDic.ContainsKey("productTerm") && !string.IsNullOrEmpty(formDic["productTerm"]))
            {
                entity.TERMS = (formDic["productTerm"].Trim()).ToInt16();
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
            CalculateContractHouse(entity);

            //还款方式
            if (formDic.ContainsKey("repaymentType"))
            {
                entity.PAYTYPE = formDic["repaymentType"].Trim();
            }

            return entity;
        }
        /// <summary>
        /// 创建并初始化APP_CUSTOMER对象
        /// </summary>
        /// <param name="formDic">请求表单数据</param>
        /// <param name="appMain">APP_MAIN对象</param>
        /// <param name="resultMsg">错误信息（输出参数）</param>
        /// <returns>APP_CUSTOMER对象</returns>
        private APP_CUSTOMER InitAppCustomerEntity(Dictionary<string, string> formDic, APP_MAIN appMain, out string resultMsg)
        {
            APP_CUSTOMER entity = new APP_CUSTOMER();
            resultMsg = string.Empty;

            //--客户信息--
            entity.APP_ID = appMain.ID;
            //姓名
            if (formDic.ContainsKey("customerName"))
            {
                entity.NAME = formDic["customerName"].Trim();
            }
            //ID
            if (formDic.ContainsKey("customerIDCard"))
            {
                entity.ID_NO = formDic["customerIDCard"].Trim();
            }          
            return entity;
        }
        /// <summary>
        /// 创建并初始化APP_CARINFO对象
        /// </summary>
        /// <param name="formDic">请求表单数据</param>
        /// <param name="appMain">APP_MAIN对象</param>
        /// <param name="resultMsg">错误信息（输出参数）</param>
        /// <returns>APP_CARINFO对象</returns>
        private APP_CARINFO InitAppCarInfoEntity(Dictionary<string, string> formDic, APP_MAIN appMain, out string resultMsg)
        {
            resultMsg = string.Empty;
            APP_CARINFO entity = new APP_CARINFO();
            entity.APP_ID = appMain.ID;

            if (Global.GlobalSetting.CheDaiLogos.Contains(appMain.LOGO))
            {
                if (formDic.ContainsKey("carSellingPrice") &&
                    formDic["carSellingPrice"].IsDecimal())
                {
                    //entity.CAR_KIND = formDic["carType"].Trim();
                    entity.CAR_SELLINGPRICE = formDic["carSellingPrice"].ToDecimal();
                }
            }
            return entity;
        }
        /// <summary>
        /// 创建并初始化APP_STAFF_ONLY对象
        /// </summary>
        /// <param name="formDic">请求表单数据</param>
        /// <param name="appMain">APP_MAIN对象</param>
        /// <returns>APP_STAFF_ONLY对象</returns>
        private APP_STAFF_ONLY InitAppStaffOnlyEntity(Dictionary<string, string> formDic, APP_MAIN appMain, out string resultMsg)
        {
            IStaffPickService staffPickService = Ioc.GetService<IStaffPickService>();
            APP_STAFF_ONLY entity = new APP_STAFF_ONLY();
            resultMsg = string.Empty;

            entity.APP_ID = appMain.ID;

            if (formDic.ContainsKey("platform"))
            {
                entity.CHANNEL_CODE = formDic["platform"].Trim();
                entity.CHANNEL_NAME = Ioc.GetService<ICR_DATA_DICService>().GetDICByCode(entity.CHANNEL_CODE).DATA_NAME;
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
        /// <param name="formDic">请求表单数据</param>
        /// <param name="appMain">APP_MAIN对象</param>
        /// <returns>APP_HOUSE对象</returns>
        private APP_HOUSE InitAppHouseEntity(Dictionary<string, string> formDic, APP_MAIN appMain, out string resultMsg)
        {
            APP_HOUSE entity = new APP_HOUSE();
            resultMsg = string.Empty;
            if (formDic.ContainsKey("assessmentPrice") && !string.IsNullOrEmpty(formDic["assessmentPrice"]))
            {
                entity.ASSESSMENT_VALUE = (formDic["assessmentPrice"].Trim()).ToDecimal();
                entity.APP_ID = appMain.ID;
            }
            return entity;
        }
        /// <summary>
        /// 创建并初始化APP_AUTH对象
        /// </summary>
        /// <param name="appMain">APP_MAIN对象</param>
        /// <returns>APP_AUTH对象</returns>
        private APP_AUTH InitAppAuthEntity(APP_MAIN appMain, APP_STAFF_ONLY staffOnly)
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

                //营业部信息
                var org = QFUserService.GetBusinessDep(entity.PARENT_ORGANIZATION);
                if (org != null)
                {
                    entity.BUSINESS_DEPARTMENT = org.ORGANIZATIONID;
                    staffOnly.BUSINESS_DEPARTMENT = org.FULLNAME;
                }
            }
            else
            {
                LogWriter.Error("获取当前用户权限数据失败！");
            }
            return entity;

        }
        public APP_LOAN HouseAppLoanEdit(Dictionary<string, string> formDic, out string resultMsg)
        {
            
            resultMsg = String.Empty;
            if (formDic.Keys.Contains("appId") && !string.IsNullOrEmpty(formDic["appId"]))
            {
                long appId = formDic["appId"].ToLong();
                var appLoanEntity = AppLoanService.Find(l => l.APP_ID == appId).FirstOrDefault();
                if (appLoanEntity != null)
                {
                    if (formDic.Keys.Contains("contractValue") && !string.IsNullOrEmpty(formDic["contractValue"]))
                    {
                        //合同金额
                        appLoanEntity.LOAN_AMT_OF_CONTRACT = Convert.ToDecimal(formDic["contractValue"]);
                        //计算申请金额，前期咨询费，借款服务费
                        CalculateContractHouse(appLoanEntity);
                    }
                    else
                    {
                        resultMsg = "合同金额输入有误！";
                    }

                    //可接受月还款
                    if (formDic.ContainsKey("payAmtMonthly") && !String.IsNullOrEmpty(formDic["payAmtMonthly"]))
                    {
                        decimal? payAmotMonthly = Convert.ToDecimal(formDic["payAmtMonthly"]);
                        appLoanEntity.PAY_AMT_MONTHLY_ACCEPTABLE = payAmotMonthly;
                    }
                    else if (String.IsNullOrEmpty(formDic["payAmtMonthly"]))
                    {
                        appLoanEntity.PAY_AMT_MONTHLY_ACCEPTABLE = 0;
                    }

                    //房贷评估价值保存
                    var appHouseEntity = appHouseService.Find(h => h.APP_ID == appId).FirstOrDefault();
                    if (appHouseEntity != null)
                    {
                        appHouseEntity.ASSESSMENT_VALUE = formDic["assessmentPrice"].ToDecimal();
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
                        {"评估价值", appHouseEntity.ASSESSMENT_VALUE + String.Empty},
                    });
                }
                else
                {
                    resultMsg = "未取到当前进件的贷款信息！";
                }
            }
            else
            {
                resultMsg = "发生异常：appID为Null！";
            }

            return null;
            
        }

        /// <summary>
        /// 房贷计算申请金额，前期咨询费，借款服务费
        /// </summary>
        /// <param name="entity">APP_LOAN实体</param>
        /// <returns>申请金额</returns>
        public void CalculateContractHouse(APP_LOAN entity)
        {
            if (entity.LOAN_AMT_OF_CONTRACT.HasValue && entity.CONSULTATION_CHARGE_RATIO.HasValue && entity.SERVICE_CHARGE_RATIO.HasValue)
            {

                //申请金额 = 合同金额（到手金额）*（1 – 前期咨询费费率）【计算结果保留百位】
                entity.APPLY_AMT = entity.LOAN_AMT_OF_CONTRACT * (1 - entity.CONSULTATION_CHARGE_RATIO);
                entity.APPLY_AMT = AccurateHundredsDigit(entity.APPLY_AMT);
                
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
        /// <summary>
        /// 计算合同金额，前期咨询费，借款服务费
        /// </summary>
        /// <param name="entity">APP_LOAN实体</param>
        /// <returns>合同金额</returns>
        public void CalculateContractAmt(APP_LOAN entity)
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

        #region  是否是拒贷判断&&是否能看到拒贷信息权限判断
        /// <summary>
        /// 描述：添加拒贷描述信息，此处根据权限和拒贷申请来判断是否显示拒贷信息，true(是)，false(否)
        /// 时间：2015-03-10
        /// 添加人：leiz
        /// </summary>
        /// <param name="status">申请状态</param>
        /// <returns>true/false</returns>
        public bool IsDisplayRefuseLoan(string status)
        {
            bool isDisplayRefuseLoanInfo = false;
            //是否是拒贷判断
            bool isRefuseLoan = (
                status == EnterStatusType.FFRAUDOK.ToString() ||
                status == EnterStatusType.FRAUDOK.ToString() ||
                status == EnterStatusType.SYSCANCEL.ToString() ||
                status == EnterStatusType.FAPPRDEC.ToString()
                );
            if (isRefuseLoan)
            {
                //是否能看到拒贷信息权限判断
                isDisplayRefuseLoanInfo = QFUserService.GetMenu().Any(p => p.NavigateUrl == "/Home/RefuseLoanDiscription");
            }
            return isDisplayRefuseLoanInfo;
        }

        /// <summary>
        /// 车贷拒贷
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        public bool IsDisplayRefuseLoanCar(string status)
        {
            bool isDisplayRefuseLoanInfo = false;
            //是否是拒贷判断
            bool isRefuseLoan = (
                //FFRAUDOK,FAPPRDEC,FRAUDOK,APPRCANCEL,APPRREJECT,RCAPPRREJ,RCAPPRCANCEL,PLMAAPPRREJ,PLMAAPPRCANCEL,PLMSAPPRCANCEL
                status == EnterStatusType.FFRAUDOK.ToString() ||
                status == EnterStatusType.FAPPRDEC.ToString() ||
                status == EnterStatusType.FRAUDOK.ToString() ||
                status == EnterStatusType.APPRCANCEL.ToString() ||
                status == EnterStatusType.APPRREJECT.ToString() ||
                status == EnterStatusType.RCAPPRREJ.ToString() ||
                status == EnterStatusType.RCAPPRCANCEL.ToString() ||
                status == EnterStatusType.PLMAAPPRREJ.ToString() ||
                status == EnterStatusType.PLMAAPPRCANCEL.ToString() ||
                status == EnterStatusType.PLMSAPPRCANCEL.ToString()
                );
            if (isRefuseLoan)
            {
                //是否能看到拒贷信息权限判断
                isDisplayRefuseLoanInfo = QFUserService.GetMenu().Any(p => p.NavigateUrl == "/Home/RefuseLoanDiscriptionCar");
            }
            return isDisplayRefuseLoanInfo;
        }

        /// <summary>
        /// 极客贷拒贷
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        public bool IsDisplayRefuseLoanGeek(string status)
        {
            bool isDisplayRefuseLoanInfo = false;
            //是否是拒贷判断
            bool isRefuseLoan = (
                status == EnterStatusType.FFRAUDOK.ToString() ||
                status == EnterStatusType.FRAUDOK.ToString() ||
                status == EnterStatusType.SYSCANCEL.ToString() ||
                status == EnterStatusType.FAPPRDEC.ToString()
                );
            if (isRefuseLoan)
            {
                //是否能看到拒贷信息权限判断
                isDisplayRefuseLoanInfo = QFUserService.GetMenu().Any(p => p.NavigateUrl == "/Home/RefuseLoanDiscriptionGeek");
            }
            return isDisplayRefuseLoanInfo;
        }

        public bool IsDisplayRefuseLoanHouse(string status)
        {
            bool isDisplayRefuseLoanInfo = false;
            
            bool isRefuseLoan = (
                status == EnterStatusType.HOUSEAPPRDEC.ToString() ||
                status == EnterStatusType.HOUSEAPPRCANCLE.ToString() ||
                status == EnterStatusType.HOUSEFAPPRDEC.ToString() ||
                status == EnterStatusType.HOUSEFAPPRCANCLE.ToString()
                );

            if (isRefuseLoan)
            {
                //是否能看到拒贷信息权限判断
                isDisplayRefuseLoanInfo = QFUserService.GetMenu().Any(p => p.NavigateUrl == "/Home/RefuseLoanDiscriptionHouse");
            }
            return isDisplayRefuseLoanInfo;
        }

        public bool IsDisplayRefuseLoanRy(string status)
        {
            bool isDisplayRefuseLoanInfo = false;
            //是否是拒贷判断
            bool isRefuseLoan = (
                status == EnterStatusType.FFRAUDOK.ToString() ||
                status == EnterStatusType.FRAUDOK.ToString() ||
                status == EnterStatusType.SYSCANCEL.ToString() ||
                status == EnterStatusType.FAPPRDEC.ToString()
                );
            if (isRefuseLoan)
            {
                //是否能看到拒贷信息权限判断
                isDisplayRefuseLoanInfo = QFUserService.GetMenu().Any(p => p.NavigateUrl == "/Home/RefuseLoanDiscriptionRy");
            }
            return isDisplayRefuseLoanInfo;
        }
        #endregion

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
    }
}
