using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QK.QAPP.Global;
using QK.QAPP.IServices;
using QK.QAPP.Entity;
using QK.QAPP.Infrastructure;
using Microsoft.Practices.Unity;
using QK.QAPP.Infrastructure.Data.EFRepository.Repository;

namespace QK.QAPP.Services
{
    public class PreApplyService : RepositoryBaseSql, IPreApplyService
    {
        #region 属性、字段

        [Dependency]
        public IV_PRE_APPMAINSERVICE preMainService
        {
            get;
            set;
        }

        [Dependency]
        public IQFUserService UserService { get; set; }

        [Dependency]
        public IQFProductInfoService ProductInfoService { get; set; }

        [Dependency]
        public IAPP_APPLY_SEQUENCESERVICE ApplySequenceService { get; set; }

        [Dependency]
        public IAPP_MAINSERVICE AppMainService { get; set; }

        [Dependency]
        public IAPP_AUTHSERVICE AppAuthService { get; set; }

        [Dependency]
        public IAPP_BANKCARDSERVICE AppBankCardService { get; set; }

        [Dependency]
        public IAPP_CUSTOMERSERVICE AppCustomerService { get; set; }

        [Dependency]
        public IAPP_JOBSERVICE AppJobService { get; set; }
        [Dependency]
        public IAPP_CARINFOSERVICE AppCarInfoService { get; set; }
        [Dependency]
        public IAPP_LOANSERVICE AppLoanService { get; set; }

        [Dependency]
        public IAPP_STAFF_ONLYSERVICE AppStaffOnlyService { get; set; }

        [Dependency]
        public IPRE_APP_MAINSERVICE PreAppMainService { get; set; }
        [Dependency]
        public IQFUserService QFUserService { get; set; }

        [Dependency]
        public IMobileHistoryService MobileHistoryService { get; set; }

        public IGenesisService GenesisService { get; set; }
        #endregion

        public PreApplyService(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {

        }

        public PreEnterListViewFiledList GetPreApplyList(PreEnterListSearchPara para)
        {
            //返回值
            PreEnterListViewFiledList enterList = new PreEnterListViewFiledList();

            //将查询条件的进件状态转换由枚举转换成字符串
            List<string> lstStatus = new List<string>();
            foreach (PreEnterStatusType s in para.ListEnterStatus)
            {
                lstStatus.Add(s.ToString());
            }
            DateTime dtm = DateTime.Now.AddDays(Global.GlobalSetting.PreQappForApplyDays * -1).Date;
            IQueryable<V_PRE_APPMAIN> query = null;
            if (para.FuzzySearch)
            {
                query = preMainService.Find(a =>
                    (
                        (a.APPCODE.IndexOf(para.PreAppCode) > -1 || string.IsNullOrEmpty(para.PreAppCode))
                        ||
                        (a.CUSTOMERNAME.IndexOf(para.CustomerName) > -1 || string.IsNullOrEmpty(para.CustomerName))
                        )
                    && lstStatus.Contains(a.APPSTATUS)
                    && a.HAS_APPLY == 0
                    && a.CREATEDTIME > dtm);
            }
            else
            {
                query = preMainService.Find(a =>
                        (a.APPCODE.IndexOf(para.PreAppCode) > -1 || string.IsNullOrEmpty(para.PreAppCode))
                        && (a.CUSTOMERNAME.IndexOf(para.CustomerName) > -1 || string.IsNullOrEmpty(para.CustomerName))
                        && (
                            (a.SALESCODE.IndexOf(para.SaleCode) > -1 || string.IsNullOrEmpty(para.SaleCode))
                            ||
                            (a.SALESNAME.IndexOf(para.SaleName) > -1 || string.IsNullOrEmpty(para.SaleName))
                            )
                        && (a.CUSTOMERMOBILE1.IndexOf(para.CustomerMobile) > -1 || a.CUSTOMERMOBILE2.IndexOf(para.CustomerMobile) > -1
                            || string.IsNullOrEmpty(para.CustomerMobile))
                        && lstStatus.Contains(a.APPSTATUS)
                        && a.HAS_APPLY == 0
                        && a.CREATEDTIME > dtm);
            }

            if (query != null)
            {
                query = UserService.QueryJoinUserAuthPre(query, a => a.APPID, o => o.APP_ID)
                    .OrderBy(a => a.SORTING)
                    .ThenByDescending(a => a.CREATEDTIME);
            }

            //排序条件
            if (para.Sort.Count > 0)
            {
                foreach (KeyValuePair<string, string> kv in para.Sort)
                {
                    if (kv.Key != "" && kv.Value != "")
                    {
                        query = query.Sort(kv.Key, kv.Value);
                    }
                }
            }
            enterList.SetParameters(query, para);
            return enterList;
        }
        /// <summary>
        /// 获取预申请列表
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        public PreEnterListViewFiledList GetPreApplyByList(PreEnterListSearchPara para)
        {
            //返回值
            PreEnterListViewFiledList enterList = new PreEnterListViewFiledList();
            //将查询条件的LOGO转换为字符串
            List<string> listLogo = new List<string>();
            foreach (string s in para.ListLogo)
            {
                listLogo.Add(s.ToString());
            }
            IQueryable<V_PRE_APPMAIN> query = GetPreApply(para);
            if (query != null)
            {
                query = query.Where(a => (listLogo.Contains(a.LOGO)));
            }
            enterList.SetParameters(query, para);
            return enterList;
        }
        private IQueryable<V_PRE_APPMAIN> GetPreApply(PreEnterListSearchPara para)
        {
            //将查询条件的进件状态转换由枚举转换成字符串
            List<string> lstStatus = new List<string>();
            foreach (PreEnterStatusType s in para.ListEnterStatus)
            {
                lstStatus.Add(s.ToString());
            }
            DateTime dtm = DateTime.Now.AddDays(Global.GlobalSetting.PreQappForApplyDays * -1).Date;
            IQueryable<V_PRE_APPMAIN> query = null;
            if (para.FuzzySearch)
            {
                query = preMainService.Find(a =>
                    (
                        (a.APPCODE.IndexOf(para.PreAppCode) > -1 || string.IsNullOrEmpty(para.PreAppCode))
                        ||
                        (a.CUSTOMERNAME.IndexOf(para.CustomerName) > -1 || string.IsNullOrEmpty(para.CustomerName))
                        )
                    && lstStatus.Contains(a.APPSTATUS)
                    && a.HAS_APPLY == 0
                    && a.CREATEDTIME > dtm);
            }
            else
            {
                query = preMainService.Find(a =>
                        (a.APPCODE.IndexOf(para.PreAppCode) > -1 || string.IsNullOrEmpty(para.PreAppCode))
                        && (a.CUSTOMERNAME.IndexOf(para.CustomerName) > -1 || string.IsNullOrEmpty(para.CustomerName))
                        && (
                            (a.SALESCODE.IndexOf(para.SaleCode) > -1 || string.IsNullOrEmpty(para.SaleCode))
                            ||
                            (a.SALESNAME.IndexOf(para.SaleName) > -1 || string.IsNullOrEmpty(para.SaleName))
                            )
                        && (a.CUSTOMERMOBILE1.IndexOf(para.CustomerMobile) > -1 || a.CUSTOMERMOBILE2.IndexOf(para.CustomerMobile) > -1
                            || string.IsNullOrEmpty(para.CustomerMobile))
                        && lstStatus.Contains(a.APPSTATUS)
                        && a.HAS_APPLY == 0
                        && a.CREATEDTIME > dtm);
            }

            if (query != null)
            {
                query = UserService.QueryJoinUserAuthPre(query, a => a.APPID, o => o.APP_ID)
                    .OrderBy(a => a.SORTING)
                    .ThenByDescending(a => a.CREATEDTIME);
            }

            //排序条件
            if (para.Sort.Count > 0)
            {
                foreach (KeyValuePair<string, string> kv in para.Sort)
                {
                    if (kv.Key != "" && kv.Value != "")
                    {
                        query = query.Sort(kv.Key, kv.Value);
                    }
                }
            }
            return query;
        }

        public APP_MAIN ApplyLoan(long preAppId, Dictionary<string, string> formDic, out string resultMsg)
        {
            resultMsg = string.Empty;
            var preAppMain = PreAppMainService.FirstOrDefault(m => m.ID == preAppId);
            if (preAppMain == null)
            {
                resultMsg = "处理申请出错！";
                Infrastructure.Log4Net.LogWriter.Error("未找到ID为:" + preAppId + "的PRE_APP_MIAN对象！");
                return null;
            }
            //check权限
            resultMsg = CheckPermission(preAppMain);
            if (!string.IsNullOrEmpty(resultMsg))
            {
                return null;
            }
            resultMsg = CheckProcess(preAppMain.ID);
            if (!string.IsNullOrEmpty(resultMsg))
            {
                return null;
            }

            return CreateLoanFromPre(preAppMain, formDic);
        }
        /// <summary>
        /// 从车贷预申请进件
        /// </summary>
        /// <param name="preAppId">预申请id</param>
        /// <param name="formDic"></param>
        /// <param name="resultMsg"></param>
        /// <returns></returns>
        public APP_MAIN ApplyLoanCar(long preAppId, Dictionary<string, string> formDic, out string resultMsg)
        {
            resultMsg = string.Empty;
            var preAppMain = PreAppMainService.FirstOrDefault(m => m.ID == preAppId);
            if (preAppMain == null)
            {
                resultMsg = "处理申请出错！";
                Infrastructure.Log4Net.LogWriter.Error("未找到ID为:" + preAppId + "的PRE_APP_MIAN对象！");
                return null;
            }
            //check权限
            resultMsg = CheckPermission(preAppMain);
            if (!string.IsNullOrEmpty(resultMsg))
            {
                return null;
            }
            resultMsg = CheckProcess(preAppMain.ID);
            if (!string.IsNullOrEmpty(resultMsg))
            {
                return null;
            }

            return CreateLoanFromPreCar(preAppMain, formDic);
        }
        /// <summary>
        /// 创建保存车贷进件信息
        /// </summary>
        /// <param name="preAppMain"></param>
        /// <param name="formDic"></param>
        /// <returns></returns>
        private APP_MAIN CreateLoanFromPreCar(PRE_APP_MAIN preAppMain, Dictionary<string, string> formDic)
        {
            if (preAppMain != null)
            {
                //初始化APP_MAIN相关的数据对象
                var appMain = InitAppMainCar(preAppMain, formDic);
                var appAuth = InitAppAuth(appMain);
                var appCustomer = InitAppCustomer(appMain, preAppMain, formDic);
                var appLoan = InitAppLoan(appMain, preAppMain, formDic);
                var appStaffOnly = InitStaffOnlyCar(appMain, preAppMain, formDic);
                var appCarInfo = InitAppCarInfo(appMain, preAppMain, formDic);
                var appJob = InitAppJob(appMain, preAppMain);
                var appBankcard = InitAppBankcard(appMain, preAppMain);
                //表示此预申请单已经在QAPP申请
                preAppMain.HAS_APPLY = 1;
                //处理完成
                preAppMain.PROCESSING = 0;
                preAppMain.CHANGED_TIME = DateTime.Now;

                AppMainService.Add(appMain);
                AppAuthService.Add(appAuth);
                AppCustomerService.Add(appCustomer);
                AppLoanService.Add(appLoan);
                AppStaffOnlyService.Add(appStaffOnly);
                AppCarInfoService.Add(appCarInfo);
                AppJobService.Add(appJob);
                AppBankCardService.Add(appBankcard);
                PreAppMainService.Update(preAppMain);

                try
                {
                    AppMainService.UnitOfWork.SaveChanges();
                }
                catch (Exception e)
                {
                    //若SaveChange出错，记录日志
                    Infrastructure.Log4Net.LogWriter.Error("PreApplyService中SaveChange出错！", e);
                    throw;
                }

                //日志
                Infrastructure.Log4Net.LogWriter.Biz("创建【APP_MAIN】（从预申请信息进件），preAppId为：" + preAppMain.ID, appMain.ID + String.Empty, appMain);
                Infrastructure.Log4Net.LogWriter.Biz("创建【APP_AUTH】", appAuth.APP_ID + String.Empty, appAuth);
                Infrastructure.Log4Net.LogWriter.Biz("创建【APP_CARINFO】", appCarInfo.APP_ID + String.Empty, appCarInfo);
                Infrastructure.Log4Net.LogWriter.Biz("创建【APP_CUSTOMER】", appCustomer.APP_ID + String.Empty, appCustomer);
                Infrastructure.Log4Net.LogWriter.Biz("创建【APP_LOAN】", appLoan.APP_ID + String.Empty, appLoan);
                Infrastructure.Log4Net.LogWriter.Biz("创建【APP_STAFF_ONLY】", appStaffOnly.APP_ID + String.Empty, appStaffOnly);
                Infrastructure.Log4Net.LogWriter.Biz("创建【APP_JOB】", appJob.APP_ID + String.Empty, appJob);
                Infrastructure.Log4Net.LogWriter.Biz("创建【APP_BANKCARD】", appBankcard.APP_ID + String.Empty, appBankcard);

                return appMain;
            }
            else
            {
                return null;
            }
        }
        private APP_MAIN CreateLoanFromPre(PRE_APP_MAIN preAppMain, Dictionary<string, string> formDic)
        {
            if (preAppMain != null)
            {
                //初始化APP_MAIN相关的数据对象
                var appMain = InitAppMain(preAppMain, formDic);
                var appAuth = InitAppAuth(appMain);
                var appCustomer = InitAppCustomer(appMain, preAppMain, formDic);
                var appLoan = InitAppLoan(appMain, preAppMain, formDic);
                var appStaffOnly = InitStaffOnly(appMain, preAppMain, formDic);
                var appJob = InitAppJob(appMain, preAppMain);
                var appBankcard = InitAppBankcard(appMain, preAppMain);
                //表示此预申请单已经在QAPP申请
                preAppMain.HAS_APPLY = 1;
                //处理完成
                preAppMain.PROCESSING = 0;
                preAppMain.CHANGED_TIME = DateTime.Now;

                AppMainService.Add(appMain);
                AppAuthService.Add(appAuth);
                AppCustomerService.Add(appCustomer);
                AppLoanService.Add(appLoan);
                AppStaffOnlyService.Add(appStaffOnly);
                AppJobService.Add(appJob);
                AppBankCardService.Add(appBankcard);
                PreAppMainService.Update(preAppMain);

                try
                {
                    AppMainService.UnitOfWork.SaveChanges();
                }
                catch (Exception e)
                {
                    //若SaveChange出错，记录日志
                    Infrastructure.Log4Net.LogWriter.Error("PreApplyService中SaveChange出错！", e);
                    throw;
                }

                //日志
                Infrastructure.Log4Net.LogWriter.Biz("创建【APP_MAIN】（从预申请信息进件），preAppId为：" + preAppMain.ID, appMain.ID + String.Empty, appMain);
                Infrastructure.Log4Net.LogWriter.Biz("创建【APP_AUTH】", appAuth.APP_ID + String.Empty, appAuth);
                Infrastructure.Log4Net.LogWriter.Biz("创建【APP_BANKCARD】", appBankcard.APP_ID + String.Empty, appBankcard);
                Infrastructure.Log4Net.LogWriter.Biz("创建【APP_CUSTOMER】", appCustomer.APP_ID + String.Empty, appCustomer);
                Infrastructure.Log4Net.LogWriter.Biz("创建【APP_JOB】", appJob.APP_ID + String.Empty, appJob);
                Infrastructure.Log4Net.LogWriter.Biz("创建【APP_LOAN】", appLoan.APP_ID + String.Empty, appLoan);
                Infrastructure.Log4Net.LogWriter.Biz("创建【APP_STAFF_ONLY】", appStaffOnly.APP_ID + String.Empty, appStaffOnly);

                return appMain;
            }
            else
            {
                return null;
            }
        }

        private APP_BANKCARD InitAppBankcard(APP_MAIN appMain, PRE_APP_MAIN preAppMain)
        {
            var entity = new APP_BANKCARD();
            var preAppBankcard = preAppMain.PRE_APP_BANKCARD.FirstOrDefault();
            entity.APP_ID = appMain.ID;
            if (!CloneObjectByMapping(preAppBankcard, entity, GlobalSetting.BankcardToPreBankcardMapping))
            {
                Infrastructure.Log4Net.LogWriter.Error("从PRE_APP_BANKCARD中未找到相关数据！");
            }

            return entity;
        }

        private APP_JOB InitAppJob(APP_MAIN appMain, PRE_APP_MAIN preAppMain)
        {
            var entity = new APP_JOB();
            var preAppJob = preAppMain.PRE_APP_JOB.FirstOrDefault();
            entity.APP_ID = appMain.ID;
            if (!CloneObjectByMapping(preAppJob, entity, GlobalSetting.JobToPreJobMapping))
            {
                Infrastructure.Log4Net.LogWriter.Error("从PRE_APP_JOB中未找到相关数据！");
            }

            return entity;
        }

        private APP_STAFF_ONLY InitStaffOnly(APP_MAIN appMain, PRE_APP_MAIN preAppMain, Dictionary<string, string> formDic)
        {
            var entity = new APP_STAFF_ONLY();
            var preAppStaffOnly = preAppMain.PRE_APP_STAFF_ONLY.FirstOrDefault();
            entity.APP_ID = appMain.ID;
            if (!CloneObjectByMapping(preAppStaffOnly, entity, GlobalSetting.StaffOnlyToPreStaffOnlyMapping))
            {
                Infrastructure.Log4Net.LogWriter.Error("从PRE_APP_STAFF_ONLY中未找到相关数据！");
            }
            //合作渠道Code
            if (formDic.ContainsKey("platform"))
            {
                entity.CHANNEL_CODE = formDic["platform"];
                //合作渠道名称
                entity.CHANNEL_NAME = Ioc.GetService<ICR_DATA_DICService>()
                    .GetDICByCode(entity.CHANNEL_CODE).DATA_NAME;
            }
            var currentUser = UserService.GetCurrentUser();
            //员工账户
            entity.CSAD_CODE = currentUser.Account;
            //员工姓名（姓名加工号后六位）
            entity.CSAD_NAME = Ioc.GetService<IStaffPickService>()
                .GetUserDisplayName(currentUser.RealName, currentUser.Code);

            return entity;
        }
        private APP_STAFF_ONLY InitStaffOnlyCar(APP_MAIN appMain, PRE_APP_MAIN preAppMain, Dictionary<string, string> formDic)
        {
            var entity = new APP_STAFF_ONLY();
            var preAppStaffOnly = preAppMain.PRE_APP_STAFF_ONLY.FirstOrDefault();
            entity.APP_ID = appMain.ID;
            if (!CloneObjectByMapping(preAppStaffOnly, entity, GlobalSetting.StaffOnlyToPreStaffOnlyMapping))
            {
                Infrastructure.Log4Net.LogWriter.Error("从PRE_APP_STAFF_ONLY中未找到相关数据！");
            }
            //合作渠道Code
            if (formDic.ContainsKey("plantform"))
            {
                entity.CHANNEL_CODE = formDic["plantform"];
                //合作渠道名称
                entity.CHANNEL_NAME = Ioc.GetService<ICR_DATA_DICService>()
                    .GetDICByCode(entity.CHANNEL_CODE).DATA_NAME;
            }
            var currentUser = UserService.GetCurrentUser();
            if (currentUser != null)
            {
                //员工账户
                entity.CSAD_CODE = currentUser.Account;
                //员工姓名（姓名加工号后六位）
                entity.CSAD_NAME = Ioc.GetService<IStaffPickService>()
                    .GetUserDisplayName(currentUser.RealName, currentUser.Code);
            }
            else
            {

                Infrastructure.Log4Net.LogWriter.Error("初始化APP_STAFF_ONLY对象出错,currentUser为Null");
                return entity;
            }
            return entity;
        }
        private APP_LOAN InitAppLoan(APP_MAIN appMain, PRE_APP_MAIN preAppMain, Dictionary<string, string> formDic)
        {
            var entity = new APP_LOAN();
            var preAppLoan = preAppMain.PRE_APP_LOAN.FirstOrDefault();
            entity.APP_ID = appMain.ID;
            if (!CloneObjectByMapping(preAppLoan, entity, GlobalSetting.LoanToPreLoanMapping))
            {
                Infrastructure.Log4Net.LogWriter.Error("从PRE_APP_LOAN中未找到相关数据！");
            }
            //申请金额
            if (formDic.ContainsKey("applyAmount"))
            {
                entity.APPLY_AMT = formDic["applyAmount"].Trim().ToDecimal();
            }
            //可接受月还款
            if (formDic.ContainsKey("payAmtMonthly") && !string.IsNullOrEmpty(formDic["payAmtMonthly"]))
            {
                entity.PAY_AMT_MONTHLY_ACCEPTABLE = formDic["payAmtMonthly"].Trim().ToDecimal();
            }
            //若为微车贷，则贷款用途默认为消费 CarLoanPurposeBuy
            if (appMain.LOGO == "productCodeMiniCarLoan")
                entity.LOAN_PURPOSE = "CarLoanPurposeBuy";
            //借款用途
            if (formDic.ContainsKey("loanPurpose"))
            {
                entity.LOAN_PURPOSE = formDic["loanPurpose"];
            }
            //借款用途其他
            if (formDic.ContainsKey("memoOfLoanPurposeOther"))
            {
                entity.MEMO_OF_LOAN_PURPOSE_OTHER = formDic["memoOfLoanPurposeOther"];
            }
            //期限
            if (formDic.ContainsKey("productTerm"))
            {
                entity.TERMS = formDic["productTerm"].Trim().ToInt16();
            }
            //还款方式
            if (formDic.ContainsKey("repaymentType"))
            {
                entity.PAYTYPE = formDic["repaymentType"];
            }
            //咨询费率
            var product = ProductInfoService.GetProductListByProductCode(PInfoInterfaceURLAccount.productCode.ToString(), appMain.PRODUCT_CODE);
            if (product != null)
            {
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
            //var chargeList = ProductInfoService.GetInterestList(PInfoInterfaceURLAccount.productCode.ToString(), appMain.PRODUCT_CODE).FirstOrDefault();
            //if (chargeList != null)
            //{
            //    //罚息比率
            //    entity.DEFAULT_INTEREST_RATIO = chargeList.defaultInterestRatio;
            //    //服务费率
            //    entity.SERVICE_CHARGE_RATIO = chargeList.serviceChargeRatio;
            //    //利率类型
            //    entity.RATE_TYPE = chargeList.rateType;
            //    //利率
            //    entity.RATE = chargeList.rate;
            //}
            //计算合同金额，前期咨询费，借款服务费
            CalculateContractAmt(entity);

            return entity;
        }

        private APP_CUSTOMER InitAppCustomer(APP_MAIN appMain, PRE_APP_MAIN preAppMain, Dictionary<string, string> formDic)
        {
            var entity = new APP_CUSTOMER();
            var preAppCustomer = preAppMain.PRE_APP_CUSTOMER.FirstOrDefault();
            entity.APP_ID = appMain.ID;
            if (!CloneObjectByMapping(preAppCustomer, entity, GlobalSetting.CustomerToPreCustomerMapping))
            {
                Infrastructure.Log4Net.LogWriter.Error("从PRE_APP_CUSTOMER中未找到相关数据！");
            }
            //客户姓名
            if (formDic.ContainsKey("customerName"))
            {
                entity.NAME = formDic["customerName"];
            }

            //客户身份证
            if (formDic.ContainsKey("customerIDCard"))
            {
                entity.ID_NO = formDic["customerIDCard"];
            }

            return entity;
        }
        private APP_CARINFO InitAppCarInfo(APP_MAIN appMain, PRE_APP_MAIN preAppMain, Dictionary<string, string> formDic)
        {
            var entity = new APP_CARINFO();
            var preAppCarInfo = preAppMain.PRE_APP_CARINFO.FirstOrDefault();
            entity.APP_ID = appMain.ID;
            if (!CloneObjectByMapping(preAppCarInfo, entity, GlobalSetting.CustomerToPreCustomerMapping))
            {
                Infrastructure.Log4Net.LogWriter.Error("从PRE_APP_CUSTOMER中未找到相关数据！");
            }
            //if (Global.GlobalSetting.CheDaiLogos.Contains(appMain.LOGO))
            //{
            if (formDic.ContainsKey("carSellingPrice") &&
                formDic["carSellingPrice"].IsDecimal())
            {
                entity.CAR_SELLINGPRICE = formDic["carSellingPrice"].ToDecimal();
            }
            if (formDic.ContainsKey("carType"))
            {
                entity.CAR_KIND = formDic["carType"].Trim();
            }
            //}
            return entity;
        }
        private APP_AUTH InitAppAuth(APP_MAIN appMain)
        {
            var currentUser = UserService.GetCurrentUser();
            var entity = new APP_AUTH();

            entity.APP_ID = appMain.ID;
            entity.APP_CODE = appMain.APP_CODE;
            entity.MENUCODE = appMain.InputMenuCode;
            if (currentUser != null)
            {
                entity.ACCOUNT = currentUser.Account;

                var authInfo = UserService.GetUserAuthInfo(currentUser.UserId);
                entity.PARENT_ORGANIZATION = authInfo.ParentOrganization;
                entity.COMPANY = authInfo.Company;
                entity.AREA = authInfo.Area;
                entity.HEADQUARTER = authInfo.Headquarters;
            }
            else
            {
                Infrastructure.Log4Net.LogWriter.Error("获取当前用户权限数据失败！");
            }


            return entity;
        }

        private APP_MAIN InitAppMain(PRE_APP_MAIN preAppMain, Dictionary<string, string> formDic)
        {
            string productRegularId = string.Empty; //申请号中产品编码

            var entity = new APP_MAIN(true);

            //产品Code
            if (formDic.ContainsKey("productCode"))
            {
                entity.PRODUCT_CODE = formDic["productCode"];
            }
            var product = ProductInfoService.GetProductListByProductCode(PInfoInterfaceURLAccount.productCode.ToString(), entity.PRODUCT_CODE);
            if (product != null)
            {
                //产品编码
                productRegularId = product.pProduct.productRegularId;
                //客户类型
                entity.CUSTOMERTYPE = product.pProduct.fit4customerType;
                //产品名称
                entity.PRODUCT_NAME = product.pProduct.productName;
                //产品版本
                entity.PROD_VERSION = product.pProduct.prodVersion;
                //产品logo
                entity.LOGO = product.pLogo.logo;
            }
            if (formDic.ContainsKey("applyCity"))
            {
                string[] cities = formDic["applyCity"].Trim()
                    .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                if (cities.Length > 1)
                {
                    //城市区号
                    entity.APPLY_AREA_CODE = cities[0];
                    //城市编码
                    entity.APPLY_CITY_CODE = cities[1];
                }
            }
            //产品logo
            //var pLogo = ProductInfoService.GetLogoList(PInfoInterfaceURLAccount.productCode.ToString(), entity.PRODUCT_CODE)
            //    .FirstOrDefault();
            //if (pLogo != null)
            //{
            //    entity.LOGO = pLogo.logo;
            //}
            //生成申请号
            if (!string.IsNullOrEmpty(entity.LOGO) && !string.IsNullOrEmpty(entity.APPLY_CITY_CODE))
            {
                entity.APP_CODE = ApplySequenceService.GetApplyNumber(entity.LOGO,
                    string.IsNullOrEmpty(productRegularId) ? "00" : productRegularId,
                    entity.APPLY_CITY_CODE);
            }
            else
            {
                Infrastructure.Log4Net.LogWriter.Error("LOGO:" + entity.LOGO + ";APPLY_CITY_CODE:" + entity.APPLY_CITY_CODE + " 为空，无法生成申请编号！");
                throw new Exception("LOGO:" + entity.LOGO + ";APPLY_CITY_CODE:" + entity.APPLY_CITY_CODE + " 为空，无法生成申请编号！");
            }
            //申请状态
            entity.APP_STATUS = EnterStatusType.PENDING.ToString();
            //创建者
            entity.CREATED_USER = UserService.GetCurrentUser().Account;
            //创建时间
            entity.CREATED_TIME = DateTime.Now;
            //预申请单ID
            if (preAppMain != null)
            {
                entity.PRE_APP_ID = preAppMain.ID;

                //设置通话详单状态
                SetMobileHistoryStatus(entity, preAppMain);
                //设置公积金，Pboc，网银认证状态
                SetGenesisStatus(entity, preAppMain);
            }

            return entity;
        }
        private APP_MAIN InitAppMainCar(PRE_APP_MAIN preAppMain, Dictionary<string, string> formDic)
        {
            APP_MAIN entity = new APP_MAIN(true);
            var applyNumberService = Ioc.GetService<IAPP_APPLY_SEQUENCESERVICE>();
            var currentUser = QFUserService.GetCurrentUser();
            string productRegularId = string.Empty; //申请号中产品编码
            string fit4CustomerType = string.Empty; //客户类型

            //--申请主表--
            if (formDic.ContainsKey("productCode"))
            {
                entity.PRODUCT_CODE = formDic["productCode"].Trim();
                entity.PRODUCT_NAME = string.Empty;
                entity.PROD_VERSION = string.Empty;
            }
            else
            {
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

            if (formDic.ContainsKey("applyCity"))
            {
                /*entity.APPLY_CITY_CODE = collection["applyCity"].Trim();--V2*/
                string[] citys = formDic["applyCity"].Trim().Split(',');
                entity.APPLY_AREA_CODE = citys[0];//城市区号
                entity.APPLY_CITY_CODE = citys[1];//城市编码
            }
            else
            {
                return entity;
            }

            //客户类型
            //if (Global.GlobalSetting.CheDaiLogos.Contains(entity.LOGO))
            //{
            //    if (formDic.ContainsKey("customerType"))
            //    {
            //        entity.CUSTOMERTYPE = formDic["customerType"].Trim();
            //    }
            //    else
            //    {
            //        return entity;
            //    }
            //}
            //else
            //{
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

            //预申请单ID
            if (preAppMain != null)
            {
                entity.PRE_APP_ID = preAppMain.ID;

                //设置通话详单状态
                SetMobileHistoryStatus(entity, preAppMain);
                //设置公积金，Pboc，网银认证状态
                SetGenesisStatus(entity, preAppMain);
            }

            return entity;
        }

        #region 计算合同金额等

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

        #endregion

        private void SetMobileHistoryStatus(APP_MAIN appMain, PRE_APP_MAIN preAppMain)
        {
            var status = MobileHistoryService.GetStatusFormApi(preAppMain.APP_CODE);
            appMain.MOBILE_HISTORY_STATUS = status;
            return;
        }

        private void SetGenesisStatus(APP_MAIN appMain, PRE_APP_MAIN preAppMain)
        {
            var bankStatus = GenesisService.GetNetbankStatusFromApi(preAppMain.APP_CODE);
            appMain.NETBANK_STATUS = bankStatus;
            var fundStatus = GenesisService.GetFundStatusFromApi(preAppMain.APP_CODE);
            appMain.FUND_STATUS = fundStatus;
            var pbocStatus = GenesisService.GetPbocStatusFromApi(preAppMain.APP_CODE);
            appMain.PBOC_STATUS = pbocStatus;
            return;
        }

        private bool CloneObjectByMapping(object sourceObj, object targetObj, Dictionary<string, string> mappingDic)
        {
            bool flag = false;
            if (sourceObj != null && targetObj != null)
            {
                Type targetType = targetObj.GetType();
                Type sourceType = sourceObj.GetType();

                var properties = targetType.GetProperties();
                foreach (var tp in properties)
                {
                    if (mappingDic.ContainsKey(tp.Name) && tp.CanWrite)
                    {
                        var sp = sourceType.GetProperty(mappingDic[tp.Name]);
                        if (sp.CanRead)
                        {
                            if (tp.PropertyType.FullName.IndexOf("Int16") > -1
                                && !string.IsNullOrEmpty(sp.GetValue(sourceObj, null) == null ? string.Empty : sp.GetValue(sourceObj, null).ToString()))
                            {
                                tp.SetValue(targetObj, short.Parse(sp.GetValue(sourceObj, null).ToString()), null);
                            }
                            else
                            {
                                tp.SetValue(targetObj, sp.GetValue(sourceObj, null), null);
                            }
                        }
                    }
                }
                flag = true;
            }
            return flag;
        }

        public string CheckPermission(PRE_APP_MAIN preAppMain)
        {
            string msg = string.Empty;
            if (preAppMain == null)
            {
                return "所请求的数据不存在！";
            }

            ////已经申请过
            //if (preAppMain.HAS_APPLY == 1 ||
            //    //申请状态为非通过
            //    preAppMain.APP_STATUS != PreEnterStatusType.PRE_APPROK.ToString() ||
            //    //超过30天
            //    (preAppMain.CREATED_TIME.HasValue && preAppMain.CREATED_TIME.Value.Date.AddDays(GlobalSetting.PreQappForApplyDays) < DateTime.Now.Date))
            //{
            //    Infrastructure.Log4Net.LogWriter.Biz("来晚啦！此预申请不久前已被他人进件，无法重复进件！", preAppMain.ID + "", preAppMain);
            //    return "所请求的数据不符合要求，无法完成该申请！";
            //}

            if (preAppMain.HAS_APPLY == 1)
            {
                msg = "来晚啦！此预申请不久前已被他人进件，无法重复进件！";
                Infrastructure.Log4Net.LogWriter.Biz(msg, preAppMain.ID + "", preAppMain);
                return msg;
            }
            //若正在处理中，且未超过处理时限
            if (preAppMain.PROCESSING == 1 &&
                DateTime.Now.Subtract(preAppMain.CHANGED_TIME ?? DateTime.Now).TotalSeconds < GlobalSetting.PreProcessingTime)
            {
                msg = "此预申请进件正在处理中，请稍后再试！";
                Infrastructure.Log4Net.LogWriter.Biz(msg, preAppMain.ID + "", preAppMain);
                return msg;
            }

            if (preAppMain.APP_STATUS != PreEnterStatusType.PRE_APPROK.ToString())
            {
                msg = "此预申请进件状态不是预申请通过状态，无法完成该申请！";
                Infrastructure.Log4Net.LogWriter.Biz(msg, preAppMain.ID + "", preAppMain);
                return msg;
            }
            if (preAppMain.CREATED_TIME.HasValue &&
                preAppMain.CREATED_TIME.Value.Date.AddDays(GlobalSetting.PreQappForApplyDays) < DateTime.Now.Date)
            {
                msg = "此预申请进件已经超过有效期，因此无法完成该申请！";
                Infrastructure.Log4Net.LogWriter.Biz(msg, preAppMain.ID + "", preAppMain);
                return msg;
            }


            if (UserService.CheckDataPermissionByPreAuth(preAppMain.ID))
            {
                return String.Empty;
            }
            else
            {
                Infrastructure.Log4Net.LogWriter.Biz("越权请求，当前用户无法操作该申请", preAppMain.ID + "", preAppMain);
                return "非法访问，您无法提交该申请！";
            }
        }

        private string CheckProcess(long preAppId)
        {
            if (SetProcessing(preAppId) > 0)
                return String.Empty;

            return "此预申请进件正在处理中，请稍后再试！";
        }

        private int SetProcessing(long preAppId)
        {
            int seconds = GlobalSetting.PreProcessingTime;
            string sql = "update pre_app_main am set am.processing = 1,am.changed_time = sysdate where am.id = :preAppId and (am.processing = 0 or (am.processing = 1 and to_number(sysdate - cast(am.changed_time as date)) * 24 * 60 * 60 > :seconds))";
            List<object> paras = new List<object>();
            paras.Add(preAppId);
            paras.Add(seconds);
            return ExecuteSqlCommand(sql, paras.ToArray());
        }
    }
}
