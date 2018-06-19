/***********************
 * 作    者：ruiwang
 * 创建时间：2015/3/12 9:19:54
 * 作    用：提供展期、循环贷业务中相关对象的操作
*****************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QK.QAPP.Global;
using QK.QAPP.Infrastructure;
using QK.QAPP.IServices;
using QK.QAPP.Entity;
using Microsoft.Practices.Unity;
using QK.QAPP.Infrastructure.Data.EFRepository.Repository;

namespace QK.QAPP.Services
{
    public class ExtendApplicationService : RepositoryBaseSql, IExtendApplicationService
    {
        #region 属性注入

        [Dependency]
        public IAPP_MAINSERVICE AppMainService { get; set; }
        [Dependency]
        public IAPP_AUTHSERVICE AppAuthService { get; set; }
        [Dependency]
        public IAPP_BANKCARDSERVICE AppBankCardService { get; set; }
        [Dependency]
        public IAPP_CARINFOSERVICE AppCarInfoService { get; set; }
        [Dependency]
        public IAPP_CONTACTSERVICE AppContactService { get; set; }
        [Dependency]
        public IAPP_CUSTOMERSERVICE AppCustomerService { get; set; }
        [Dependency]
        public IAPP_JOBSERVICE AppJobService { get; set; }
        [Dependency]
        public IAPP_LOANSERVICE AppLoanService { get; set; }
        [Dependency]
        public IAPP_STAFF_ONLYSERVICE AppStaffOnlyService { get; set; }
        [Dependency]
        public IFL_BIZSERVICE FlBizService { get; set; }
        [Dependency]
        public IFL_LISTSERVICE FlListService { get; set; }
        [Dependency]
        public IQFProductInfoService ProductInfoService { get; set; }
        [Dependency]
        public IAPP_APPLY_SEQUENCESERVICE ApplySequenceService { get; set; }
        [Dependency]
        public IQFUserService UserService { get; set; }
        [Dependency]
        public IAPP_EXTEND_CONFIGSERVICE AppExtendConfigService { get; set; }
        [Dependency]
        public IAPP_EXTEND_RELATIONSERVICE AppExtendRelaService { get; set; }
        [Dependency]
        public ILoanApplicationService LoanAppService { get; set; }
        [Dependency]
        public IAPP_HOUSESERVICE AppHouseService { get; set; }
        [Dependency]
        public IExtendApplyService ExtendApplyService { get; set; }
        #endregion

        public ExtendApplicationService(IUnitOfWork unitOfWork) : base(unitOfWork) { }

        /// <summary>
        /// 查询剩余可展期数及续展期数
        /// </summary>
        /// <param name="appMain">APP_MAIN对象</param>
        /// <param name="periodAmt">续展期数</param>
        /// <returns>剩余可展期数</returns>
        public int GetExtendPeriod(APP_MAIN appMain, out int periodAmt)
        {
            //默认续展期数为1
            periodAmt = 1;
            //剩余可展期数
            int periodRemain = 0;

            //进件展期信息
            var appExtendRela = AppExtendRelaService.FirstOrDefault(e =>
                e.ACTION_GROUP == GlobalSetting.APPExtendConfig_Extend.FirstOrDefault()
                && e.APP_CODE == appMain.APP_CODE);
            //展期产品配置
            var appExtendConfig = AppExtendConfigService.FirstOrDefault(e =>
                e.ACTION_GROUP == GlobalSetting.APPExtendConfig_Extend.FirstOrDefault()
                && e.CITY_CODE == appMain.APPLY_CITY_CODE
                && e.PRODUCT_CODE == appMain.PRODUCT_CODE);

            if (appExtendConfig != null && appExtendConfig.PERIOD_AMOUNT.HasValue && appExtendConfig.PERIOD_AMOUNT_TOTAL.HasValue)
            {
                periodAmt = appExtendConfig.PERIOD_AMOUNT.Value;
                if (appExtendRela == null)
                {
                    //第一次展期，剩余可展期数为总可展期数
                    periodRemain = appExtendConfig.PERIOD_AMOUNT_TOTAL.Value;
                }
                else if (appExtendRela.PERIOD_AMOUNT_USED.HasValue)
                {
                    //有展过的信息，剩余可展期数 = 总可展期数 - 已展期数
                    periodRemain = appExtendConfig.PERIOD_AMOUNT_TOTAL.Value - appExtendRela.PERIOD_AMOUNT_USED.Value;
                }
            }

            if (periodRemain < 0)
            {
                periodRemain = 0;
            }

            return periodRemain;
        }

        #region 展期申请

        public APP_MAIN ExtendLoanCar(long appIdOld, Dictionary<string, string> formDic, out string resultMsg)
        {
            return ExtendLoan(appIdOld, formDic, CanExtend, CreateLoanFromExist, out resultMsg);
        }

        public APP_MAIN ExtendLoanHouse(long appIdOld, Dictionary<string, string> formDic, out string resultMsg)
        {
            return ExtendLoan(appIdOld, formDic, CanExtendHouse, CreateLoanHouse, out resultMsg);
        }

        /// <summary>
        /// 展期申请
        /// </summary>
        /// <param name="appIdOld">原申请单ID</param>
        /// <param name="formDic">表单信息</param>
        /// <param name="canExtend">check当前进件是否可以做展期</param>
        /// <param name="createLoanFunc">创建申请</param>
        /// <param name="resultMsg">提示信息</param>
        /// <returns>新APP_MAIN对象</returns>
        private APP_MAIN ExtendLoan(long appIdOld, Dictionary<string, string> formDic,
            Func<APP_MAIN, bool> canExtend,
            Func<ExtendLoanPara, APP_MAIN> createLoanFunc, out string resultMsg)
        {
            var para = new ExtendLoanPara();

            para.AppMainOld = AppMainService.Find(a => a.ID == appIdOld).FirstOrDefault();

            if (para.AppMainOld != null)
            {
                //先Check是否可申请展期
                resultMsg = CheckExtendPermission(para.AppMainOld, ENUM_FormOperation.ADD, canExtend);
                if (!string.IsNullOrEmpty(resultMsg))
                {
                    return null;
                }
                //申请号中的展期次数
                int? opeType;

                //如果可以展，再看期数是否超过可展期数
                //进件展期信息
                var appExtendRela = AppExtendRelaService.FirstOrDefault(e =>
                    e.ACTION_GROUP == GlobalSetting.APPExtendConfig_Extend.FirstOrDefault()
                    && e.APP_CODE == para.AppMainOld.APP_CODE);
                //展期产品配置
                var appExtendConfig = AppExtendConfigService.FirstOrDefault(e =>
                    e.ACTION_GROUP == GlobalSetting.APPExtendConfig_Extend.FirstOrDefault()
                    && e.CITY_CODE == para.AppMainOld.APPLY_CITY_CODE
                    && e.PRODUCT_CODE == para.AppMainOld.PRODUCT_CODE);
                if (appExtendConfig != null)
                {
                    //续展期数
                    para.PeriodAmt = appExtendConfig.PERIOD_AMOUNT;
                    if (appExtendRela == null)
                    {
                        //表示没有展过，可以展
                        //已展期数
                        para.PeriodAmtUsed = appExtendConfig.PERIOD_AMOUNT;
                        //已展次数
                        para.TimesNumUsed = 1;
                        //申请号中的展期次数
                        opeType = para.TimesNumUsed + 1;
                    }
                    else
                    {
                        //如果已展期数大于等于可展总期数，则不能展
                        if (appExtendRela.PERIOD_AMOUNT_USED >= appExtendConfig.PERIOD_AMOUNT_TOTAL)
                        {
                            resultMsg = "已展期数已达到可展总期数，无法提交展期申请！";
                            Infrastructure.Log4Net.LogWriter.Biz("已展期数已达到可展总期数，无法提交展期申请！", para.AppMainOld.ID + String.Empty);
                            return null;
                        }
                        else
                        {
                            //计算已展期数
                            para.PeriodAmtUsed = appExtendRela.PERIOD_AMOUNT_USED + appExtendConfig.PERIOD_AMOUNT;
                            //计算已展次数
                            para.TimesNumUsed = appExtendRela.TIMES_NUMBER_USED + 1;
                            //申请号中的展期次数
                            opeType = para.TimesNumUsed + 1;
                        }
                    }
                }
                else
                {
                    resultMsg = "未找到展期配置信息！";
                    Infrastructure.Log4Net.LogWriter.Error("未找到APP_EXTEND_CONFIG对象:city_code为"
                        + para.AppMainOld.APPLY_CITY_CODE + "，product_code为" + para.AppMainOld.PRODUCT_CODE);
                    return null;
                }

                if (!opeType.HasValue || !para.PeriodAmt.HasValue || !para.PeriodAmtUsed.HasValue)
                {
                    resultMsg = "申请展期出错！";
                    Infrastructure.Log4Net.LogWriter.Error("期数计算错误！");
                }

                para.OpeType = opeType.ToString();
                para.FormDic = formDic;
                para.ActionGroup = GlobalSetting.APPExtendConfig_Extend.FirstOrDefault();

                return createLoanFunc(para);
            }
            else
            {
                resultMsg = "申请展期出错！";
                Infrastructure.Log4Net.LogWriter.Error("未找到ID为:" + appIdOld + "的APP_MIAN对象！");
                return null;
            }
        }

        /// <summary>
        /// 根据已有的申请单创建并保存新的申请单（车贷）
        /// </summary>
        /// <param name="para">参数</param>
        /// <returns>新创建的APP_MAIN实体</returns>
        private APP_MAIN CreateLoanFromExist(ExtendLoanPara para)
        {
            if (para != null && para.AppMainOld != null)
            {
                //初始化APP_MAIN相关的数据对象
                var appMain = InitAppMain(para.OpeType, para.AppMainOld, para.FormDic);
                var appAuth = InitAppAuth(appMain);
                var appBankcard = InitAppBankcard(appMain, para.AppMainOld);
                var appCarInfo = InitAppCarInfo(appMain, para.AppMainOld, para.FormDic);
                var appContactList = InitAppContact(appMain, para.AppMainOld);
                var appCustomer = InitAppCustomer(appMain, para.AppMainOld);
                var appJob = InitAppJob(appMain, para.AppMainOld);
                var appLoan = InitAppLoan(appMain, para.AppMainOld, para.FormDic);
                var appStaffOnly = InitStaffOnly(appMain, para.AppMainOld, para.FormDic);
                var appExtendRelation = InitExtendRelation(appMain, para);

                //写入展期状态
                para.AppMainOld.HAS_EXTEND = "Y";

                AppMainService.Update(para.AppMainOld);
                AppMainService.Add(appMain);
                AppAuthService.Add(appAuth);
                AppBankCardService.Add(appBankcard);
                AppCarInfoService.Add(appCarInfo);
                AppContactService.AddMultiple(appContactList);
                AppCustomerService.Add(appCustomer);
                AppJobService.Add(appJob);
                AppLoanService.Add(appLoan);
                AppStaffOnlyService.Add(appStaffOnly);
                AppExtendRelaService.Add(appExtendRelation);

                var flBizOld = FlBizService.Find(b => b.BIZ_CODE == para.AppMainOld.APP_CODE).FirstOrDefault();
                if (flBizOld != null)
                {
                    //初始化文件相关的对象
                    var flBiz = InitFlBiz(appMain, para.AppMainOld, flBizOld);
                    var flList = InitFlList(flBiz, flBizOld);

                    FlBizService.Add(flBiz);
                    FlListService.AddMultiple(flList);
                }
                else
                {
                    Infrastructure.Log4Net.LogWriter.Error("未找到BIZ_CODE为:" + para.AppMainOld.APP_CODE + "的FL_BIZ对象！");
                }

                try
                {
                    AppMainService.UnitOfWork.SaveChanges();
                }
                catch (Exception e)
                {
                    //若SaveChange出错，记录日志
                    Infrastructure.Log4Net.LogWriter.Error("ExtendApplicationService中SaveChange出错！", e);
                    throw;
                }

                //日志
                Infrastructure.Log4Net.LogWriter.Biz("更新展期状态为Y", para.AppMainOld.ID + String.Empty, para.AppMainOld);
                Infrastructure.Log4Net.LogWriter.Biz("复制并创建【APP_MAIN】，原ID为：" + para.AppMainOld.ID, appMain.ID + String.Empty, appMain);
                Infrastructure.Log4Net.LogWriter.Biz("创建【APP_AUTH】", appAuth.APP_ID + String.Empty, appAuth);
                Infrastructure.Log4Net.LogWriter.Biz("复制并创建【APP_BANKCARD】", appBankcard.APP_ID + String.Empty, appBankcard);
                Infrastructure.Log4Net.LogWriter.Biz("复制并创建【APP_CARINFO】", appCarInfo.APP_ID + String.Empty, appCarInfo);
                foreach (var appContact in appContactList)
                {
                    Infrastructure.Log4Net.LogWriter.Biz("复制并创建【APP_CONTACT】", appContact.APP_ID + String.Empty, appContact);
                }
                Infrastructure.Log4Net.LogWriter.Biz("复制并创建【APP_CUSTOMER】", appCustomer.APP_ID + String.Empty, appCustomer);
                Infrastructure.Log4Net.LogWriter.Biz("复制并创建【APP_JOB】", appJob.APP_ID + String.Empty, appJob);
                Infrastructure.Log4Net.LogWriter.Biz("复制并创建【APP_LOAN】", appLoan.APP_ID + String.Empty, appLoan);
                Infrastructure.Log4Net.LogWriter.Biz("复制并创建【APP_STAFF_ONLY】", appStaffOnly.APP_ID + String.Empty, appStaffOnly);
                Infrastructure.Log4Net.LogWriter.Biz("创建【APP_EXTEND_RELATION】", appMain.ID + String.Empty, appExtendRelation);
                Infrastructure.Log4Net.LogWriter.Biz("页面表单信息", appMain.ID + String.Empty, para.FormDic);

                return appMain;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 根据已有的申请单创建并保存新的申请单（房贷）
        /// </summary>
        /// <param name="para">参数</param>
        /// <returns></returns>
        private APP_MAIN CreateLoanHouse(ExtendLoanPara para)
        {
            if (para != null && para.AppMainOld != null)
            {
                //初始化APP_MAIN相关的数据对象
                var appMain = InitAppMain(para.OpeType, para.AppMainOld, para.FormDic);
                var appAuth = InitAppAuth(appMain);
                var appBankcard = InitAppBankcard(appMain, para.AppMainOld);
                var appContactList = InitAppContact(appMain, para.AppMainOld);
                var appCustomer = InitAppCustomer(appMain, para.AppMainOld);
                var appJob = InitAppJob(appMain, para.AppMainOld);
                var appLoan = InitAppLoanHouse(appMain, para.AppMainOld, para.FormDic);
                var appStaffOnly = InitStaffOnly(appMain, para.AppMainOld, para.FormDic);
                var appHouse = InitAppHouse(appMain, para.AppMainOld, para.FormDic);
                var appExtendRelation = InitExtendRelation(appMain, para);

                //写入展期状态
                para.AppMainOld.HAS_EXTEND = "Y";

                AppMainService.Update(para.AppMainOld);
                AppMainService.Add(appMain);
                AppAuthService.Add(appAuth);
                AppBankCardService.Add(appBankcard);
                AppContactService.AddMultiple(appContactList);
                AppCustomerService.Add(appCustomer);
                AppJobService.Add(appJob);
                AppLoanService.Add(appLoan);
                AppStaffOnlyService.Add(appStaffOnly);
                AppHouseService.Add(appHouse);
                AppExtendRelaService.Add(appExtendRelation);

                try
                {
                    AppMainService.UnitOfWork.SaveChanges();
                }
                catch (Exception e)
                {
                    //若SaveChange出错，记录日志
                    Infrastructure.Log4Net.LogWriter.Error("ExtendApplicationService中SaveChange出错！", e);
                    throw;
                }

                //日志
                Infrastructure.Log4Net.LogWriter.Biz("更新展期状态为Y", para.AppMainOld.ID + String.Empty, para.AppMainOld);
                Infrastructure.Log4Net.LogWriter.Biz("复制并创建【APP_MAIN】，原ID为：" + para.AppMainOld.ID, appMain.ID + String.Empty, appMain);
                Infrastructure.Log4Net.LogWriter.Biz("创建【APP_AUTH】", appAuth.APP_ID + String.Empty, appAuth);
                Infrastructure.Log4Net.LogWriter.Biz("复制并创建【APP_BANKCARD】", appBankcard.APP_ID + String.Empty, appBankcard);
                foreach (var appContact in appContactList)
                {
                    Infrastructure.Log4Net.LogWriter.Biz("复制并创建【APP_CONTACT】", appContact.APP_ID + String.Empty, appContact);
                }
                Infrastructure.Log4Net.LogWriter.Biz("复制并创建【APP_CUSTOMER】", appCustomer.APP_ID + String.Empty, appCustomer);
                Infrastructure.Log4Net.LogWriter.Biz("复制并创建【APP_JOB】", appJob.APP_ID + String.Empty, appJob);
                Infrastructure.Log4Net.LogWriter.Biz("复制并创建【APP_LOAN】", appLoan.APP_ID + String.Empty, appLoan);
                Infrastructure.Log4Net.LogWriter.Biz("复制并创建【APP_STAFF_ONLY】", appStaffOnly.APP_ID + String.Empty, appStaffOnly);
                Infrastructure.Log4Net.LogWriter.Biz("复制并创建【APP_HOUSE】", appHouse.APP_ID + String.Empty, appHouse);
                Infrastructure.Log4Net.LogWriter.Biz("创建【APP_EXTEND_RELATION】", appMain.ID + String.Empty, appExtendRelation);
                Infrastructure.Log4Net.LogWriter.Biz("页面表单信息", appMain.ID + String.Empty, para.FormDic);

                return appMain;
            }
            else
            {
                return null;
            }
        }

        #endregion

        #region 初始化数据对象

        /// <summary>
        /// 初始化APP_MAIN对象（会重新生成申请号，重置申请状态等）
        /// </summary>
        /// <param name="opeType">申请号中的期数</param>
        /// <param name="appMainOld">原APP_MAIN对象</param>
        /// <param name="formDic">表单信息</param>
        /// <returns>新APP_MAIN对象</returns>
        private APP_MAIN InitAppMain(string opeType, APP_MAIN appMainOld, Dictionary<string, string> formDic)
        {
            string productRegularId = string.Empty; //申请号中产品编码

            APP_MAIN entity = new APP_MAIN(true);
            //复制对象信息
            if (CloneObject(appMainOld, entity))
            {
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
                //城市
                if (formDic.ContainsKey("applyCity"))
                {
                    string[] citys = formDic["applyCity"].Trim().Split(',');
                    entity.APPLY_AREA_CODE = citys[0];//城市区号
                    entity.APPLY_CITY_CODE = citys[1];//城市编码
                }
                //生成申请号
                if (!string.IsNullOrEmpty(entity.LOGO) && !string.IsNullOrEmpty(entity.APPLY_CITY_CODE))
                {
                    entity.APP_CODE = ApplySequenceService.GetApplyNumberExtend(entity.LOGO,
                        string.IsNullOrEmpty(productRegularId) ? "00" : productRegularId,
                        entity.APPLY_CITY_CODE, opeType);
                }
                else
                {
                    Infrastructure.Log4Net.LogWriter.Error("LOGO:" + entity.LOGO + ";APPLY_CITY_CODE:" + entity.APPLY_CITY_CODE + " 为空，无法生成申请编号！");
                    throw new Exception("LOGO:" + entity.LOGO + ";APPLY_CITY_CODE:" + entity.APPLY_CITY_CODE + " 为空，无法生成申请编号！");
                }

                //客户类型
                if (GlobalSetting.CheDaiLogos.Contains(entity.LOGO) ||
                    GlobalSetting.HouseLogos.Contains(entity.LOGO))
                {
                    if (formDic.ContainsKey("customerType"))
                    {
                        entity.CUSTOMERTYPE = formDic["customerType"];
                    }
                }

                //申请状态
                entity.APP_STATUS = EnterStatusType.PENDING.ToString();
                entity.CREATED_USER = UserService.GetCurrentUser().Account;
                entity.CREATED_TIME = DateTime.Now;
                entity.CHANGED_USER = entity.CREATED_USER;
                entity.CHANGED_TIME = entity.CREATED_TIME;

            }
            else
            {
                Infrastructure.Log4Net.LogWriter.Error("复制【APP_MAIN】对象出错！");
            }

            return entity;
        }

        /// <summary>
        /// 初始化APP_AUTH对象（不会复制原单的权限，使用当前用户的权限）
        /// </summary>
        /// <param name="appMain">新APP_MAIN对象</param>
        /// <returns>新APP_AUTH对象</returns>
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

        /// <summary>
        /// 初始化APP_BANKCARD对象
        /// </summary>
        /// <param name="appMain">新APP_MAIN对象</param>
        /// <param name="appMainOld">原APP_MAIN对象</param>
        /// <returns>新APP_BANKCARD对象</returns>
        private APP_BANKCARD InitAppBankcard(APP_MAIN appMain, APP_MAIN appMainOld)
        {
            var oldEntity = AppBankCardService.Find(b => b.APP_ID == appMainOld.ID).FirstOrDefault();
            var entity = new APP_BANKCARD();
            //复制对象信息
            if (CloneObject(oldEntity, entity))
            {
                entity.APP_ID = appMain.ID;
            }
            else
            {
                Infrastructure.Log4Net.LogWriter.Error("复制【APP_BANKCARD】对象出错！");
            }

            return entity;
        }

        /// <summary>
        /// 初始化APP_CARINFO对象
        /// </summary>
        /// <param name="appMain">新APP_MAIN对象</param>
        /// <param name="appMainOld">原APP_MAIN对象</param>
        /// <param name="formDic">表单信息</param>
        /// <returns>新APP_CARINFO对象</returns>
        private APP_CARINFO InitAppCarInfo(APP_MAIN appMain, APP_MAIN appMainOld, Dictionary<string, string> formDic)
        {
            var oldEntity = AppCarInfoService.Find(c => c.APP_ID == appMainOld.ID).FirstOrDefault();
            var entity = new APP_CARINFO();
            //复制对象信息
            if (CloneObject(oldEntity, entity))
            {
                entity.APP_ID = appMain.ID;
                if (GlobalSetting.CheDaiLogos.Contains(appMain.LOGO))
                {
                    if (formDic.ContainsKey("carType"))
                    {
                        entity.CAR_KIND = formDic["carType"];
                    }
                    //车辆开票价格
                    if (formDic.ContainsKey("carSellingPrice") && formDic["carSellingPrice"].IsDecimal())
                    {
                        entity.CAR_SELLINGPRICE = formDic["carSellingPrice"].Trim().ToDecimal();
                    }
                }
            }
            else
            {
                Infrastructure.Log4Net.LogWriter.Error("复制【APP_CARINFO】对象出错！");
            }

            return entity;
        }

        /// <summary>
        /// 初始化APP_CONTACT对象
        /// </summary>
        /// <param name="appMain">新APP_MAIN对象</param>
        /// <param name="appMainOld">原APP_MAIN对象</param>
        /// <returns>新APP_CONTACT对象的集合</returns>
        private List<APP_CONTACT> InitAppContact(APP_MAIN appMain, APP_MAIN appMainOld)
        {
            var oldEntities = AppContactService.Find(c => c.APP_ID == appMainOld.ID);
            var entityList = new List<APP_CONTACT>();
            foreach (var item in oldEntities)
            {
                var entity = new APP_CONTACT();
                //复制对象信息
                if (CloneObject(item, entity))
                {
                    entity.APP_ID = appMain.ID;
                    entityList.Add(entity);
                }
                else
                {
                    Infrastructure.Log4Net.LogWriter.Error("复制【APP_CONTACT】对象出错！");
                }
            }

            return entityList;
        }

        /// <summary>
        /// 初始化APP_CUSTOMER对象
        /// </summary>
        /// <param name="appMain">新APP_MAIN对象</param>
        /// <param name="appMainOld">原APP_MAIN对象</param>
        /// <returns>新APP_CUSTOMER对象</returns>
        private APP_CUSTOMER InitAppCustomer(APP_MAIN appMain, APP_MAIN appMainOld)
        {
            var oldEntity = AppCustomerService.Find(c => c.APP_ID == appMainOld.ID).FirstOrDefault();
            var entity = new APP_CUSTOMER();
            //复制对象信息
            if (CloneObject(oldEntity, entity))
            {
                entity.APP_ID = appMain.ID;
            }
            else
            {
                Infrastructure.Log4Net.LogWriter.Error("复制【APP_CUSTOMER】对象出错！");
            }

            return entity;
        }

        /// <summary>
        /// 初始化APP_JOB对象
        /// </summary>
        /// <param name="appMain">新APP_MAIN对象</param>
        /// <param name="appMainOld">原APP_MAIN对象</param>
        /// <returns>新APP_JOB对象</returns>
        private APP_JOB InitAppJob(APP_MAIN appMain, APP_MAIN appMainOld)
        {
            var oldEntity = AppJobService.Find(j => j.APP_ID == appMainOld.ID).FirstOrDefault();
            var entity = new APP_JOB();
            //复制对象信息
            if (CloneObject(oldEntity, entity))
            {
                entity.APP_ID = appMain.ID;
            }
            else
            {
                Infrastructure.Log4Net.LogWriter.Error("复制【APP_JOB】对象出错！");
            }

            return entity;
        }

        /// <summary>
        /// 初始化APP_LOAN对象
        /// </summary>
        /// <param name="appMain">新APP_MAIN对象</param>
        /// <param name="appMainOld">原APP_MAIN对象</param>
        /// <param name="formDic">表单信息</param>
        /// <returns>新APP_LOAN对象</returns>
        private APP_LOAN InitAppLoan(APP_MAIN appMain, APP_MAIN appMainOld, Dictionary<string, string> formDic)
        {
            var oldEntity = AppLoanService.Find(l => l.APP_ID == appMainOld.ID).FirstOrDefault();
            var entity = new APP_LOAN();
            //复制对象信息
            if (CloneObject(oldEntity, entity))
            {
                entity.APP_ID = appMain.ID;
                //实际贷款金额置空
                entity.LOAN_AMT = null;

                //申请金额
                if (formDic.ContainsKey("applyAmount"))
                {
                    entity.APPLY_AMT = formDic["applyAmount"].Trim().ToDecimal();
                }
                //可接受月还款
                if (formDic.ContainsKey("payAmtMonthly"))
                {
                    entity.PAY_AMT_MONTHLY_ACCEPTABLE = string.IsNullOrEmpty(formDic["payAmtMonthly"]) ? 0 : formDic["payAmtMonthly"].Trim().ToDecimal();

                }
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
                var product = ProductInfoService.GetProductListByProductCode(PInfoInterfaceURLAccount.productCode.ToString(), appMain.PRODUCT_CODE);
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

                //还款方式
                if (formDic.ContainsKey("repaymentType"))
                {
                    entity.PAYTYPE = formDic["repaymentType"];
                }
                //计算合同金额，前期咨询费，借款服务费
                LoanAppService.CalculateContractAmt(entity);
            }
            else
            {
                Infrastructure.Log4Net.LogWriter.Error("复制【APP_LOAN】对象出错！");
            }

            return entity;
        }

        /// <summary>
        /// 初始化APP_LOAN（房贷）
        /// </summary>
        /// <param name="appMain"></param>
        /// <param name="appMainOld"></param>
        /// <param name="formDic"></param>
        /// <returns></returns>
        private APP_LOAN InitAppLoanHouse(APP_MAIN appMain, APP_MAIN appMainOld, Dictionary<string, string> formDic)
        {
            var oldEntity = AppLoanService.Find(l => l.APP_ID == appMainOld.ID).FirstOrDefault();
            var entity = new APP_LOAN();
            //复制对象信息
            if (CloneObject(oldEntity, entity))
            {
                entity.APP_ID = appMain.ID;
                //实际贷款金额置空
                entity.LOAN_AMT = null;

                //合同金额
                if (formDic.ContainsKey("contractValue"))
                {
                    entity.LOAN_AMT_OF_CONTRACT = (formDic["contractValue"].Trim()).ToDecimal();
                }
                //可接受月还款
                if (formDic.ContainsKey("payAmtMonthly"))
                {
                    entity.PAY_AMT_MONTHLY_ACCEPTABLE = string.IsNullOrEmpty(formDic["payAmtMonthly"]) ? 0 : formDic["payAmtMonthly"].Trim().ToDecimal();

                }
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
                var product = ProductInfoService.GetProductListByProductCode(PInfoInterfaceURLAccount.productCode.ToString(), appMain.PRODUCT_CODE);
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

                //还款方式
                if (formDic.ContainsKey("repaymentType"))
                {
                    entity.PAYTYPE = formDic["repaymentType"];
                }
                //计算合同金额，前期咨询费，借款服务费
                LoanAppService.CalculateContractHouse(entity);
            }
            else
            {
                Infrastructure.Log4Net.LogWriter.Error("复制【APP_LOAN】对象出错！");
            }

            return entity;
        }

        /// <summary>
        /// 初始化StaffOnly对象
        /// </summary>
        /// <param name="appMain">新APP_MAIN对象</param>
        /// <param name="appMainOld">原APP_MAIN对象</param>
        /// <param name="formDic">表单信息</param>
        /// <returns>新APP_STAFF_ONLY对象</returns>
        private APP_STAFF_ONLY InitStaffOnly(APP_MAIN appMain, APP_MAIN appMainOld, Dictionary<string, string> formDic)
        {
            var oldEntity = AppStaffOnlyService.Find(s => s.APP_ID == appMainOld.ID).FirstOrDefault();
            var entity = new APP_STAFF_ONLY();
            //复制对象信息
            if (CloneObject(oldEntity, entity))
            {
                entity.APP_ID = appMain.ID;
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
            }
            else
            {
                Infrastructure.Log4Net.LogWriter.Error("复制【APP_STAFF_ONLY】对象出错！");
            }

            return entity;
        }

        /// <summary>
        /// 初始化APP_HOUSE
        /// </summary>
        /// <param name="appMain">新APP_MAIN对象</param>
        /// <param name="appMainOld">原APP_MAIN对象</param>
        /// <param name="formDic">表单信息</param>
        /// <returns></returns>
        private APP_HOUSE InitAppHouse(APP_MAIN appMain, APP_MAIN appMainOld, Dictionary<string, string> formDic)
        {
            var oldEntity = AppHouseService.Find(h => h.APP_ID == appMainOld.ID).FirstOrDefault();
            var entity = new APP_HOUSE();
            //复制对象
            if (CloneObject(oldEntity, entity))
            {
                entity.APP_ID = appMain.ID;
                //评估价值
                if (formDic.ContainsKey("assessmentPrice"))
                {
                    entity.ASSESSMENT_VALUE = formDic["assessmentPrice"].Trim().ToDecimal();
                }
            }

            return entity;
        }

        /// <summary>
        /// 初始化FL_BIZ对象
        /// </summary>
        /// <param name="appMain">新APP_MAIN对象</param>
        /// <param name="appMainOld">原APP_MAIN对象</param>
        /// <param name="flBizOld">原FL_BIZ的ID</param>
        /// <returns>新FL_BIZ对象</returns>
        private FL_BIZ InitFlBiz(APP_MAIN appMain, APP_MAIN appMainOld, FL_BIZ flBizOld)
        {
            var entity = new FL_BIZ(true);
            //复制对象信息
            if (CloneObject(flBizOld, entity))
            {
                entity.BIZ_CODE = appMain.APP_CODE;
                entity.CREATED_USER = UserService.GetCurrentUser().Code;
                entity.CREATED_TIME = DateTime.Now;
                entity.CHANGED_USER = entity.CREATED_USER;
                entity.CHANGED_TIME = entity.CREATED_TIME;
            }
            else
            {
                Infrastructure.Log4Net.LogWriter.Error("复制【FL_BIZ】对象出错！");
            }

            return entity;
        }

        /// <summary>
        /// 初始化FL_LIST对象
        /// </summary>
        /// <param name="flBiz">新FL_BIZ对象</param>
        /// <param name="flBizOld">原FL_BIZ对象</param>
        /// <returns>新FL_LIST对象的集合</returns>
        private List<FL_LIST> InitFlList(FL_BIZ flBiz, FL_BIZ flBizOld)
        {
            //车辆评估图片类型
            string[] assessType = new[] { "CarPhotoCondition", "CarPhotoMatiral" };
            //只复制未删除的文件列表信息，且不包含车辆评估图片
            var entityListOld = FlListService.Find(l => l.FL_ID == flBizOld.ID && l.STATUS == "Y" && !assessType.Contains(l.FL_TYPE));
            var entityList = new List<FL_LIST>();
            var currentUserCode = UserService.GetCurrentUser().Code;
            foreach (var item in entityListOld)
            {
                var entity = new FL_LIST();
                //复制对象信息
                if (CloneObject(item, entity))
                {
                    entity.FL_ID = flBiz.ID;
                    entity.CREATED_USER = currentUserCode;
                    entity.CREATED_TIME = DateTime.Now;
                    entity.CHANGED_USER = entity.CREATED_USER;
                    entity.CHANGED_TIME = entity.CREATED_TIME;
                    entityList.Add(entity);
                }
                else
                {
                    Infrastructure.Log4Net.LogWriter.Error("复制【FL_LIST】对象出错！");
                }
            }

            return entityList;
        }

        /// <summary>
        /// 初始化APP_EXTEND_RELATION对象
        /// </summary>
        /// <param name="appMain">新APP_MAIN对象</param>
        /// <param name="para">展期参数</param>
        /// <returns></returns>
        private APP_EXTEND_RELATION InitExtendRelation(APP_MAIN appMain, ExtendLoanPara para)
        {
            var extendRelation = new APP_EXTEND_RELATION();

            //展期单号
            extendRelation.APP_CODE = appMain.APP_CODE;
            //被展单号
            extendRelation.PARENT_APP_CODE = para.AppMainOld.APP_CODE;
            //操作
            extendRelation.ACTION_GROUP = para.ActionGroup;
            //续展期数
            extendRelation.PERIOD_AMOUNT = para.PeriodAmt;
            //已展期数
            extendRelation.PERIOD_AMOUNT_USED = para.PeriodAmtUsed;
            //已展次数
            extendRelation.TIMES_NUMBER_USED = para.TimesNumUsed;
            //状态
            extendRelation.PERIOD_STATUS = ExtendStatusType.CarExtendNotClear.ToString();
            //创建者
            extendRelation.CREATED_USER = UserService.GetCurrentUser().Account;
            //创建时间
            extendRelation.CREATED_TIME = DateTime.Now;
            //修改者
            extendRelation.CHANGED_USER = extendRelation.CREATED_USER;
            //修改时间
            extendRelation.CHANGED_TIME = DateTime.Now;

            var extendOld = AppExtendRelaService.FirstOrDefault(e => e.APP_CODE == para.AppMainOld.APP_CODE);
            if (extendOld == null)
            {
                //没有做过展期，初始单号为appMainOld.APP_CODE
                extendRelation.INIT_APP_CODE = para.AppMainOld.APP_CODE;
            }
            else
            {
                //已经做过展期，初始单号取展期信息表中的初始单号extendOld.INIT_APP_CODE
                extendRelation.INIT_APP_CODE = extendOld.INIT_APP_CODE;
            }


            return extendRelation;
        }

        /// <summary>
        /// 复制对象属性值(限同类型对象间，不复制名为ID的属性、BasicEntity中的属性、接口类型的属性)
        /// </summary>
        /// <param name="sourceObj">原对象</param>
        /// <param name="targetObj">新对象</param>
        /// <returns>true表示成功</returns>
        private bool CloneObject(object sourceObj, object targetObj)
        {
            bool flag = false;
            if (sourceObj != null && targetObj != null)
            {
                Type type = targetObj.GetType();
                var properties = type.GetProperties();
                foreach (var p in properties)
                {
                    if (p.CanRead && p.CanWrite && !p.PropertyType.IsInterface
                        && p.PropertyType.BaseType.Name != "BasicEntity"
                        && p.Name != "ID")
                    {
                        p.SetValue(targetObj, p.GetValue(sourceObj, null), null);
                    }
                }
                flag = true;
            }
            return flag;
        }

        #endregion

        #region 展期申请权限验证

        /// <summary>
        /// 验证申请是否有操作权限（展期）
        /// </summary>
        /// <param name="appMainEntity">APP_MAIN对象</param>
        /// <param name="operation">操作</param>
        /// <param name="canExtendFunc">是否可展方法</param>
        /// <returns>消息（如果为空则表示有权限）</returns>
        public string CheckExtendPermission(APP_MAIN appMainEntity, ENUM_FormOperation operation, Func<APP_MAIN, bool> canExtendFunc)
        {
            if (appMainEntity != null)
            {
                //var staffOnly = appMainEntity.APP_STAFF_ONLY.FirstOrDefault();
                switch (operation)
                {
                    //如果是展期申请，数据权限车贷和房贷有区分
                    case ENUM_FormOperation.ADD:
                        if (!canExtendFunc(appMainEntity))
                        {
                            Infrastructure.Log4Net.LogWriter.Biz("越权请求，在申请不能展期的情况下请求展期", appMainEntity.ID + "", appMainEntity);
                            return "该申请现阶段不提供展期申请！";
                        }
                        break;
                    //如果是编辑，数据权限按当前用户来
                    case ENUM_FormOperation.EDIT:
                        if (appMainEntity.APP_STATUS != EnterStatusType.PENDING.ToString())
                        {
                            Infrastructure.Log4Net.LogWriter.Biz("越权请求编辑展期申请", appMainEntity.ID + "", appMainEntity);
                            return "该申请现阶段不提供编辑！";
                        }

                        if (!UserService.CheckDataPermission(appMainEntity.ID))
                        {
                            Infrastructure.Log4Net.LogWriter.Biz(string.Format("越权请求，当前用户无进件（appId:{0}）的数据权限", appMainEntity.ID), appMainEntity.ID + "", appMainEntity);
                            return "非法访问，您无法访问该申请！";
                        }
                        break;
                    case ENUM_FormOperation.READONLY:
                        break;
                }

                return String.Empty;
            }

            return "所请求的数据不存在！";
        }

        /// <summary>
        /// Check某个单子是否可以做展期申请（车贷）
        /// </summary>
        /// <param name="appMain">申请实体</param>
        /// <returns></returns>
        public bool CanExtend(APP_MAIN appMain)
        {
            string tempSql = String.Empty;
            List<object> paras = new List<object>();
            var extendActions = GlobalSetting.APPExtendConfig_Extend;
            var extendStatus = GlobalSetting.NeedExtendStatus_Extend;
            //准备用于检查权限的变量
            QFUserAuth currentAuth = UserService.GetUserAuth();

            StringBuilder sql = new StringBuilder();
            sql.Append("SELECT ROWNUM ID,AE.* FROM V_APPMAIN_EXTEND AE ");

            //最大权限则不需要检查权限
            if (!currentAuth.IsSelectAll)
            {
                sql.Append(" LEFT JOIN APP_AUTH AA ");
                sql.Append(" ON AE.AppId = AA.APP_ID ");
            }

            sql.Append(" WHERE AE.APPID = :appId AND AE.EXTENDACTION in (");

            paras.Add(appMain.ID);

            //ExtendActions参数
            for (int i = 0; i < extendActions.Count; i++)
            {
                if (!string.IsNullOrEmpty(extendActions[i]))
                {
                    tempSql += string.Format(":ExtendAction_{0},", i);
                    paras.Add(extendActions[i]);
                }
            }
            sql.Append(string.IsNullOrEmpty(tempSql) ? "''" : tempSql.TrimEnd(','));

            sql.Append(") AND AE.APPSTATUS IN (");

            tempSql = String.Empty;
            int j = 0;
            //ExtendStatus参数
            foreach (var item in extendStatus)
            {
                j++;
                if (!string.IsNullOrEmpty(item.Key))
                {
                    tempSql += string.Format(":AppStatus_{0},", j);
                    paras.Add(item.Key);
                }
            }
            sql.Append(string.IsNullOrEmpty(tempSql) ? "''" : tempSql.TrimEnd(',')).Append(") ");

            //在系统时间不超过还款日前5个工作日的条件，只比较年月日部分
            //sql.Append(") AND to_date(to_char(sysdate,'YYYYMMDD'),'yyyy/mm/dd')+(select count(0) from app.app_main_sysdisused_weekend amsw where amsw.weekend_date between sysdate and sysdate+" + GlobalSetting.BackLoanDay + ")+" + GlobalSetting.BackLoanDay + " < to_date(to_char(AE.Back_loan_time,'YYYYMMDD'),'yyyy/mm/dd') ");
            //sql.Append(" AND ceil(to_date(to_char(AE.Back_loan_time,'YYYYMMDD'),'yyyy/mm/dd')-sysdate)-(select count(0)-1 from app.app_main_sysdisused_weekend amsw where amsw.weekend_date between sysdate and to_date(to_char(AE.Back_loan_time,'YYYYMMDD'),'yyyy/mm/dd')) >= ").Append(GlobalSetting.BackLoanDay);
            //展期状态为Y标识已经做过展期，不能再申请了
            //sql.Append("AND AE.has_extend is null ");

            ExtendApplyService.AddExtendConditionCar(sql);

            if (!currentAuth.IsSelectAll)
                ExtendApplyService.AddExtendPermission(currentAuth, sql, paras);

            var result = this.SqlQuery<V_APPMAIN_EXTEND>(sql.ToString(), paras.ToArray());

            return result.Any();
        }

        /// <summary>
        /// Check是否可以做展期申请（房贷）
        /// </summary>
        /// <param name="appMain"></param>
        /// <returns></returns>
        public bool CanExtendHouse(APP_MAIN appMain)
        {
            string tempSql = String.Empty;
            List<object> paras = new List<object>();
            var extendActions = GlobalSetting.APPExtendConfig_Extend;
            var extendStatus = GlobalSetting.NeedExtendStatus_Extend;
            //准备用于检查权限的变量
            QFUserAuth currentAuth = UserService.GetUserAuth();

            StringBuilder sql = new StringBuilder();
            sql.Append("SELECT ROWNUM ID,AE.* FROM V_APPMAIN_EXTEND AE");

            //最大权限则不需要检查权限
            if (!currentAuth.IsSelectAll)
            {
                sql.Append(" LEFT JOIN APP_AUTH AA ");
                sql.Append(" ON AE.AppId = AA.APP_ID ");
            }

            sql.Append(" WHERE AE.APPID = :appId AND AE.EXTENDACTION in (");

            paras.Add(appMain.ID);

            //ExtendActions参数
            for (int i = 0; i < extendActions.Count; i++)
            {
                if (!string.IsNullOrEmpty(extendActions[i]))
                {
                    tempSql += string.Format(":ExtendAction_{0},", i);
                    paras.Add(extendActions[i]);
                }
            }
            sql.Append(string.IsNullOrEmpty(tempSql) ? "''" : tempSql.TrimEnd(','));

            sql.Append(")  AND AE.APPSTATUS IN (");

            tempSql = String.Empty;
            int j = 0;
            //ExtendStatus参数
            foreach (var item in extendStatus)
            {
                j++;
                if (!string.IsNullOrEmpty(item.Key))
                {
                    tempSql += string.Format(":AppStatus_{0},", j);
                    paras.Add(item.Key);
                }
            }
            sql.Append(string.IsNullOrEmpty(tempSql) ? "''" : tempSql.TrimEnd(',')).Append(") ");

            //在系统时间位于第2个还款日之后的第一个工作日至到期日前一天之间，
            //sql.Append(" AND floor(sysdate-add_months(to_date(to_char(AE.Back_loan_time,'YYYYMMDD'),'yyyy/mm/dd'), -1))-(select count(0)-1 from app.app_main_sysdisused_weekend amsw where amsw.weekend_date between add_months(to_date(to_char(AE.Back_loan_time,'YYYYMMDD'),'yyyy/mm/dd'),-1) and sysdate)>0 ");
            //sql.Append(" AND sysdate + ").Append(GlobalSetting.BackLoanDayHouse).Append(" <to_date(to_char(AE.Back_loan_time,'YYYYMMDD'),'yyyy/mm/dd') ");

            //当前合同状态为正常
            //sql.Append(" AND AE.overdue_status = '").Append(OverdueStatus.NotOverdue).Append("' ");

            //展期状态为Y标识已经做过展期，不能再申请了
            //sql.Append(" AND AE.has_extend is null ");

            //房贷可展期条件
            ExtendApplyService.AddExtendConditionHouse(sql);

            if (!currentAuth.IsSelectAll)
                ExtendApplyService.AddExtendPermissionHouse(currentAuth, sql, paras);

            var result = this.SqlQuery<V_APPMAIN_EXTEND>(sql.ToString(), paras.ToArray());

            return result.Any();
        }
        #endregion

        #region 获取原单的申请金额，合同金额，到手金额

        /// <summary>
        /// 获取原单的申请金额
        /// </summary>
        /// <param name="appMain">当前进件appMain</param>
        /// <returns></returns>
        public decimal? GetOriApplyAmt(APP_MAIN appMain)
        {
            var query =
                from al in AppLoanService.GetAll()
                join am in AppMainService.GetAll() on al.APP_ID equals am.ID
                join r in AppExtendRelaService.Find(r => r.APP_CODE == appMain.APP_CODE) on am.APP_CODE equals r.PARENT_APP_CODE
                select al;

            var oriAppLoan = query.FirstOrDefault();
            if (oriAppLoan != null)
            {
                return oriAppLoan.APPLY_AMT;
            }

            return null;
        }

        /// <summary>
        /// 获取原单合同金额
        /// </summary>
        /// <param name="appMain">当前进件</param>
        /// <returns></returns>
        public decimal? GetOriContactAmt(APP_MAIN appMain)
        {
            var query =
                from al in AppLoanService.GetAll()
                join am in AppMainService.GetAll() on al.APP_ID equals am.ID
                join r in AppExtendRelaService.Find(r => r.APP_CODE == appMain.APP_CODE) on am.APP_CODE equals r.PARENT_APP_CODE
                select al;

            var oriAppLoan = query.FirstOrDefault();
            if (oriAppLoan != null)
            {
                return oriAppLoan.LOAN_AMT_OF_CONTRACT;
            }

            return null;
        }

        public decimal? GetOriLoanAmt(APP_MAIN appMain)
        {
            var query =
                from al in AppLoanService.GetAll()
                join am in AppMainService.GetAll() on al.APP_ID equals am.ID
                join r in AppExtendRelaService.Find(r => r.APP_CODE == appMain.APP_CODE) on am.APP_CODE equals r.PARENT_APP_CODE
                select al;
            var oriAppLoan = query.FirstOrDefault();
            if (oriAppLoan != null)
            {
                return oriAppLoan.LOAN_AMT;
            }

            return null;
        }
        #endregion

    }
}
