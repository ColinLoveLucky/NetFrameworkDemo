using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QK.QAPP.Infrastructure.Log4Net;
using QK.QAPP.MvcScaffold.DForm;
using QK.QAPP.Global;
using QK.QAPP.Infrastructure;
using QK.QAPP.IServices;
using QK.QAPP.Entity;
using Microsoft.Practices.Unity;

namespace QK.QAPP.SalesCenter.Controllers
{
    public class LoanApplicationController : Controller
    {

        [Dependency]
        public IApplicationService AppService { get; set; }

        [Dependency]
        public IApplyTableService ApplyService { get; set; }

        [Dependency]
        public IAPP_EXTEND_CONFIGSERVICE AppExtendService { get; set; }

        private static readonly string formNotEditMsg = "当前表单状态已更改，目前不可编辑，无法保存或提交！";
        
        // GET: /LoanApplication/
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// 读取动态表单HTML
        /// </summary>
        /// <param name="dformCode"></param>
        /// <param name="operation"></param>
        /// <returns></returns>
        public ActionResult Application(string dformCode, ENUM_FormOperation operation, string appid)
        {
            var watch = new Stopwatch();
            watch.Start();
            //确认权限
            var dp = AppService.CheckDataPermission(appid.ToLong(), operation);
            if (!string.IsNullOrEmpty(dp))
            {
                ViewBag.CreateCount = "<script>bootbox.alert('" + dp + "',function(){   $(window).unbind('beforeunload');window.location.href='/';});</script>";
                return View();
            }
            watch.Stop();
            LogWriter.Biz("进入动态表单验证权限耗时:" + watch.ElapsedMilliseconds);
            watch.Restart();
            //判断微车车贷信息 以及判断过找的到记录
            APP_MAIN mainEntity = AppService.GetAPPMain(appid.ToLong());
            DFormCreater creater;
            //判断是否是车贷/房贷补件
            if ((GlobalSetting.CheDaiLogos.Contains(mainEntity.LOGO) || GlobalSetting.LogoGroupForMenu["HOUSE"].Contains(mainEntity.LOGO))
                && operation == ENUM_FormOperation.REISSUE)
            {
                //由于部分表单字段可编辑，表单版本为指定后缀（可配置）的表单，并通过operation为ADD创建表单
                creater = new DFormCreater(dformCode, GlobalSetting.DFormVersions[dformCode] + GlobalSetting.DFormVersion_Suffix_NR, ENUM_FormOperation.ADD, mainEntity.CUSTOMERTYPE);
            }
            else
            {
                //对于预申请入口的进件，匹配预申请对应表单
                if (mainEntity.PRE_APP_ID.HasValue 
                    && mainEntity.PRE_APP_ID.Value > 0 
                    && operation == ENUM_FormOperation.ADD)
                {
                    creater = new DFormCreater(dformCode, GlobalSetting.DFormVersions[dformCode] + GlobalSetting.DFormVersion_Suffix_Pre, operation, mainEntity.CUSTOMERTYPE);
                }
                else
                {
                    creater = new DFormCreater(dformCode, GlobalSetting.DFormVersions[dformCode], operation, mainEntity.CUSTOMERTYPE);
                }
            }

            if (mainEntity != null)
            {
                //if (GlobalSetting.CheDaiLogos.Contains(mainEntity.LOGO) && mainEntity.CUSTOMERTYPE == GlobalSetting.CustomerJYCategory)
                //{
                //    var subFrom = creater.SubformList.FirstOrDefault(c => c.Name.Contains(GlobalSetting.CheDaiCustomerCategoryParent));
                //    if (subFrom != null)
                //    {
                //        subFrom.FieldList = subFrom.FieldList.Where(c => c.Field_Group != GlobalSetting.CustomerSXGroup).ToList();
                //    }
                //}
                //else if (GlobalSetting.CheDaiLogos.Contains(mainEntity.LOGO) && mainEntity.CUSTOMERTYPE == GlobalSetting.CustomerSXCategory)
                //{
                //    var subFrom = creater.SubformList.FirstOrDefault(c => c.Name.Contains(GlobalSetting.CheDaiCustomerCategoryParent));
                //    if (subFrom != null)
                //    {
                //        subFrom.FieldList = subFrom.FieldList.Where(c => c.Field_Group != GlobalSetting.CustomerJYGroup).ToList();
                //    }
                //}
                ViewBag.CreateCount = creater.CreateDForm(mainEntity.CUSTOMERTYPE);
            }
            watch.Stop();
            LogWriter.Biz("进入动态表HTML构造耗时:" + watch.ElapsedMilliseconds);
            return View();
        }

        public ActionResult RyApplication(string dformCode, ENUM_FormOperation operation, string appid)
        {
            var watch = new Stopwatch();
            watch.Start();
            //确认权限
            var dp = AppService.CheckDataPermission(appid.ToLong(), operation);
            if (!string.IsNullOrEmpty(dp))
            {
                ViewBag.CreateCount = "<script>bootbox.alert('" + dp + "',function(){   $(window).unbind('beforeunload');window.location.href='/';});</script>";
                return View("Application");
            }
            watch.Stop();
            LogWriter.Biz("进入动态表单验证权限耗时:" + watch.ElapsedMilliseconds);
            watch.Restart();
            //判断微车车贷信息 以及判断过找的到记录
            APP_MAIN mainEntity = AppService.GetAPPMain(appid.ToLong());
            DFormCreater creater;
            string dformVersion = string.Empty;

            //优先使用融誉100的表单配置，如未找到相应产品的表单，则使用默认配置
            if (GlobalSetting.RyDFormVersions.ContainsKey(dformCode))
            {
                dformVersion = GlobalSetting.RyDFormVersions[dformCode];
            }
            else
            {
                dformVersion = GlobalSetting.DFormVersions[dformCode];
            }

            creater = new DFormCreater(dformCode, dformVersion, operation, mainEntity.CUSTOMERTYPE);


            if (mainEntity != null)
            {
                ViewBag.CreateCount = creater.CreateDForm(mainEntity.CUSTOMERTYPE);
            }
            watch.Stop();
            LogWriter.Biz("进入动态表HTML构造耗时:" + watch.ElapsedMilliseconds);
            return View("Application");
        }

        /// <summary>
        ///  读取展期动态表单HTML
        /// </summary>
        /// <param name="operation"></param>
        /// <param name="appid"></param>
        /// <returns></returns>
        public ActionResult ExtendApplication(ENUM_FormOperation operation, string appid)
        {
            var watch = new Stopwatch();
            watch.Start();
            //确认权限
            var dp = AppService.CheckDataPermission(appid.ToLong(), operation);
            if (!string.IsNullOrEmpty(dp))
            {
                ViewBag.CreateCount = "<script>bootbox.alert('" + dp + "',function(){   $(window).unbind('beforeunload');window.location.href='/';});</script>";
                return View("Application");
            }
            watch.Stop();
            LogWriter.Biz("进入动态表单验证权限耗时:" + watch.ElapsedMilliseconds);
            watch.Restart();
            APP_MAIN mainEntity = AppService.GetAPPMain(appid.ToLong());
            //读取展期动态表单html
            if (mainEntity != null)
            {
                string logo = mainEntity.LOGO;
                //string productCode = mainEntity.PRODUCT_CODE.ToString();
                string cityCode = mainEntity.APPLY_CITY_CODE.ToString();
                if (!string.IsNullOrEmpty(logo) && !string.IsNullOrEmpty(cityCode))
                {
                    //根据city_code和target_logo和action_group确定配置
                    APP_EXTEND_CONFIG extendConfig = AppExtendService.Find(a => a.TARGET_LOGO == logo && a.CITY_CODE == cityCode
                        && GlobalSetting.APPExtendConfig_Extend.Contains(a.ACTION_GROUP)).FirstOrDefault();
                    if (extendConfig != null)
                    {
                        string targetDFormCode = extendConfig.TARGET_LOGO.ToString();
                        string targetDformVersion = extendConfig.TARGET_DFORM_VERSION.ToString();
                        DFormCreater creater = new DFormCreater(targetDFormCode, targetDformVersion, operation, mainEntity.CUSTOMERTYPE);

                        //if (GlobalSetting.CheDaiLogos.Contains(mainEntity.LOGO) && mainEntity.CUSTOMERTYPE == GlobalSetting.CustomerJYCategory)
                        //{
                        //    var subFrom = creater.SubformList.FirstOrDefault(c => c.Name.Contains(GlobalSetting.CheDaiCustomerCategoryParent));
                        //    if (subFrom != null)
                        //    {
                        //        subFrom.FieldList = subFrom.FieldList.Where(c => c.Field_Group != GlobalSetting.CustomerSXGroup).ToList();
                        //    }
                        //}
                        //else if (GlobalSetting.CheDaiLogos.Contains(mainEntity.LOGO) && mainEntity.CUSTOMERTYPE == GlobalSetting.CustomerSXCategory)
                        //{
                        //    var subFrom = creater.SubformList.FirstOrDefault(c => c.Name.Contains(GlobalSetting.CheDaiCustomerCategoryParent));
                        //    if (subFrom != null)
                        //    {
                        //        subFrom.FieldList = subFrom.FieldList.Where(c => c.Field_Group != GlobalSetting.CustomerJYGroup).ToList();
                        //    }
                        //}

                        ViewBag.CreateCount = creater.CreateDForm(mainEntity.CUSTOMERTYPE);
                    }
                }
            }
            watch.Stop();
            LogWriter.Biz("进入动态表HTML构造耗时:" + watch.ElapsedMilliseconds);
            return View("Application");
        }

        #region 动态表单Action
        /// <summary>
        /// 获取用户基本信息
        /// </summary>
        /// <param name="appid"></param>
        /// <param name="subformID"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetCustomerBasic(long appid, long subformID)
        {

            var basic = AppService.GetUserBasic(appid);
            var job = AppService.GetUserJob(appid);
            var jsonDic = DFormHelper.GetFormJson<APP_CUSTOMER>(basic, subformID);
            jsonDic.AddRange(DFormHelper.GetFormJson<APP_JOB>(job, subformID));
            return Json(jsonDic, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 获取用户银行信息
        /// </summary>
        /// <param name="appid"></param>
        /// <param name="subformID"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetBankCard(long appid, long subformID)
        {

            var obj = AppService.GetCustomerBankCard(appid);
            var jsonDic = DFormHelper.GetFormJson<APP_BANKCARD>(obj, subformID);
            return Json(jsonDic, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 获取员工专用栏
        /// </summary>
        /// <param name="appid"></param>
        /// <param name="subformID"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetStaffOnly(long appid, long subformID)
        {

            var obj = AppService.GetStaffOnly(appid);
            var jsonDic = DFormHelper.GetFormJson<APP_STAFF_ONLY>(obj, subformID);
            return Json(jsonDic, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取员工专用栏（包含APP_MAIN中征信渠道）
        /// </summary>
        /// <param name="appid"></param>
        /// <param name="subformID"></param>
        /// <returns></returns>
        public JsonResult GetStaffOnlyAndChannel(long appid, long subformID)
        {
            var obj = AppService.GetStaffOnly(appid);
            var main = obj.APP_MAIN;
            var jsonDic = DFormHelper.GetFormJson<APP_STAFF_ONLY>(obj, subformID);
            jsonDic.AddRange(DFormHelper.GetFormJson<APP_MAIN>(main, subformID));
            return Json(jsonDic, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取购车信息
        /// </summary>
        /// <param name="appid"></param>
        /// <param name="subformID"></param>
        /// <returns></returns>
        public JsonResult GetCarInfo(long appid, long subformID)
        {
            var obj = AppService.GetCarInfo(appid);
            var jsonDic = DFormHelper.GetFormJson<APP_CARINFO>(obj, subformID);
            return Json(jsonDic, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 保存基本信息
        /// </summary>
        /// <param name="form"></param>
        [HttpPost]
        [LogicalActionFilter(ActionSummary = "保存申请表单的个人基本信息")]
        public string SaveCustomerBasic(FormCollection form)
        {
            if (!AppService.CheckIsAllowEdit(form["appid"].ToLong()))  //验证表单是否处于可编辑状态
            {
                return formNotEditMsg;
            }
            string customerError = "";
            string customerJobError = "";
            var customer = DFormHelper.InitObjVaidate<APP_CUSTOMER>(form, out customerError);
            var customerJob = DFormHelper.InitObjVaidate<APP_JOB>(form, out customerJobError);
            return AppService.SaveCustomerBasic(customer, customerJob) + customerError + customerJobError;
        }
        /// <summary>
        /// 保存展期基本信息
        /// </summary>
        /// <param name="form"></param>
        [HttpPost]
        [LogicalActionFilter(ActionSummary = "保存展期申请表单的个人基本信息")]
        public string SaveExtendCustomerBasic(FormCollection form)
        {
            if (!AppService.CheckIsAllowEdit(form["appid"].ToLong()))  //验证表单是否处于可编辑状态
            {
                return formNotEditMsg;
            }
            string customerError = "";
            string customerJobError = "";
            APP_CUSTOMER customer = new APP_CUSTOMER();
            APP_JOB customerJob = new APP_JOB();
            if (!string.IsNullOrEmpty(form["appid"]))
            {
                var appid = form["appid"].ToLong();
                customer = AppService.GetUserBasic(appid);
                customerJob = AppService.GetUserJob(appid);
            }
            customer = DFormHelper.InitObjVaidate<APP_CUSTOMER>(form, customer, out customerError);
            customerJob = DFormHelper.InitObjVaidate<APP_JOB>(form, customerJob, out customerJobError);
            return AppService.SaveCustomerBasic(customer, customerJob) + customerError + customerJobError;
        }

        /// <summary>
        /// 保存银行卡信息
        /// </summary>
        /// <param name="form"></param>
        [HttpPost]
        public string SaveBankCard(FormCollection form)
        {
            if (!AppService.CheckIsAllowEdit(form["appid"].ToLong()))  //验证表单是否处于可编辑状态
            {
                return formNotEditMsg;
            }
            string customerError = "";
            var customer = DFormHelper.InitObjVaidate<APP_BANKCARD>(form, out customerError);
            return AppService.SaveCustomerBankCard(customer) + customerError;
        }

        /// <summary>
        /// 保存银行卡信息（不进行中金验证，只保存银行名称）
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        [HttpPost]
        public string SaveBankCarkNoCode(FormCollection form)
        {
            if (!AppService.CheckIsAllowEdit(form["appid"].ToLong()))  //验证表单是否处于可编辑状态
            {
                return formNotEditMsg;
            }
            string error = string.Empty;
            var bankCard = DFormHelper.InitObjVaidate<APP_BANKCARD>(form, out error);
            return AppService.SaveBankCardNoCode(bankCard) + error;
        }

        /// <summary>
        /// 保存展期银行卡信息
        /// </summary>
        /// <param name="form"></param>
        [HttpPost]
        public string SaveExtendBankCard(FormCollection form)
        {
            if (!AppService.CheckIsAllowEdit(form["appid"].ToLong()))  //验证表单是否处于可编辑状态
            {
                return formNotEditMsg;
            }
            string customerError = "";
            APP_BANKCARD customerBankCard = new APP_BANKCARD();
            if (!string.IsNullOrEmpty(form["appid"]))
            {
                var appid = form["appid"].ToLong();
                customerBankCard = AppService.GetCustomerBankCard(appid);
            }

            customerBankCard = DFormHelper.InitObjVaidate<APP_BANKCARD>(form, customerBankCard, out customerError);
            return AppService.SaveCustomerBankCard(customerBankCard) + customerError;
        }

        /// <summary>
        /// 保存员工专用栏信息
        /// </summary>
        /// <param name="form"></param>
        [HttpPost]
        public string SaveStaffOnly(FormCollection form)
        {
            if (!AppService.CheckIsAllowEdit(form["appid"].ToLong()))  //验证表单是否处于可编辑状态
            {
                return formNotEditMsg;
            }
            string customerError = "";
            string mainError = "";
            APP_STAFF_ONLY customer = new APP_STAFF_ONLY();
            APP_MAIN main = new APP_MAIN();

            if (!string.IsNullOrEmpty(form["appid"]))
            {
                var appid = form["appid"].ToLong();
                customer = AppService.GetStaffOnly(appid);
                main = AppService.GetAPPMain(appid);
            }

            customer = DFormHelper.InitObjVaidate<APP_STAFF_ONLY>(form, customer, out customerError);
            main = DFormHelper.InitObjVaidate<APP_MAIN>(form, main, out mainError);
            //return AppService.SaveStaffOnly(customer) + customerError;
            return AppService.SaveStaffOnly(customer, main) + customerError + mainError;
        }

        /// <summary>
        /// 保存备注
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        public string SaveMemo(FormCollection form)
        {
            if (!AppService.CheckIsAllowEdit(form["appid"].ToLong()))  //验证表单是否处于可编辑状态
            {
                return formNotEditMsg;
            }
            string customerError = "";
            APP_STAFF_ONLY customer = new APP_STAFF_ONLY();

            if (!string.IsNullOrEmpty(form["appid"]))
            {
                var appid = form["appid"].ToLong();
                customer = AppService.GetStaffOnly(appid);
            }

            customer = DFormHelper.InitObjVaidate<APP_STAFF_ONLY>(form, customer, out customerError);
            return AppService.SaveStaffOnly(customer) + customerError;
        }

        /// <summary>
        /// 保存购车信息
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        [HttpPost]
        public string SaveCarInfo(FormCollection form)
        {
            if (!AppService.CheckIsAllowEdit(form["appid"].ToLong()))  //验证表单是否处于可编辑状态
            {
                return formNotEditMsg;
            }
            string carInfoError = "";
            APP_CARINFO carInfo = new APP_CARINFO();

            if (!string.IsNullOrEmpty(form["appid"]))
            {
                var appid = form["appid"].ToLong();
                carInfo = AppService.GetCarInfo(appid);
            }

            carInfo = DFormHelper.InitObjVaidate<APP_CARINFO>(form, carInfo, out carInfoError);
            return AppService.SaveCarInfo(carInfo) + carInfoError;
        }

        #region 联系人处理
        /// <summary>
        /// 删除联系人
        /// </summary>
        /// <param name="APP_MAIN_Id"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetContacts(long appid, long subformID)
        {
            var list = AppService.GetContacts(appid);
            Dictionary<string, Object> str = new Dictionary<string, Object>();
            foreach (var item in list)
            {
                var jsonDic = DFormHelper.GetFormJson<APP_CONTACT>(item, subformID);
                foreach (var dic in jsonDic.Where(c => c.Key.StartsWith(item.CONTACT_PROPERTY)))
                {
                    str.Add(dic.Key, dic.Value);
                }
            }

            return Json(str, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 添加联系人
        /// </summary>
        /// <param name="form"></param>
        [HttpPost]
        public string EditContacts(FormCollection form)
        {
            if (!AppService.CheckIsAllowEdit(form["appid"].ToLong()))  //验证表单是否处于可编辑状态
            {
                return formNotEditMsg;
            }
            List<string> contactsTypes = form.AllKeys.Where(c => c.Contains("-")).Select(c => c.Split('-').FirstOrDefault()).Distinct().ToList();
            String error = "";
            //分组保存
            foreach (var contactsType in contactsTypes)
            {
                FormCollection f = new FormCollection();
                f.Add("subformID", form["subformID"]);
                foreach (var item in form.AllKeys.Where(c => c.StartsWith(contactsType)))
                {
                    f.Add(item, form[item]);
                }
                string thisError;
                //构造实体
                var app_con = DFormHelper.InitObjVaidate<APP_CONTACT>(f, out thisError);
                app_con.CONTACT_PROPERTY = contactsType;
                error += thisError;
                AppService.EditContacts(app_con);

            }

            return "";
        }

        /// <summary>
        /// 添加展期联系人
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        [HttpPost]
        public string EditExtendContacts(FormCollection form)
        {
            if (!AppService.CheckIsAllowEdit(form["appid"].ToLong()))  //验证表单是否处于可编辑状态
            {
                return formNotEditMsg;
            }
            List<string> contactsTypes = form.AllKeys.Where(c => c.Contains("-")).Select(c => c.Split('-').FirstOrDefault()).Distinct().ToList();
            String error = "";
            List<APP_CONTACT> contectList = new List<APP_CONTACT>();
            if (!string.IsNullOrEmpty(form["appid"]))
            {
                var appid = form["appid"].ToLong();
                contectList = AppService.GetContacts(appid);
            }
            //分组保存
            foreach (var contactsType in contactsTypes)
            {
                FormCollection f = new FormCollection();
                f.Add("subformID", form["subformID"]);
                foreach (var item in form.AllKeys.Where(c => c.StartsWith(contactsType)))
                {
                    f.Add(item, form[item]);
                }
                string thisError;
                //构造实体
                var contect = contectList.FirstOrDefault(c => c.CONTACT_PROPERTY == contactsType) ?? new APP_CONTACT();
                //APP_CONTACT contect = new APP_CONTACT();

                //foreach (var appContect in contectList)
                //{
                //    if (appContect.CONTACT_PROPERTY == contactsType)
                //    {
                //        contect = appContect;
                //    }
                //}

                contect = DFormHelper.InitObjVaidate<APP_CONTACT>(f, contect, out thisError);
                contect.CONTACT_PROPERTY = contactsType;
                error += thisError;
                AppService.EditContacts(contect);
            }
            return "";
        }
        #endregion

        /// <summary>
        /// 读取抵押房产资料
        /// </summary>
        /// <param name="appid"></param>
        /// <param name="subformID"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetHouse(long appid, long subformID)
        {
            var obj = AppService.GetHouse(appid);
            var jsonDic = DFormHelper.GetFormJson(obj, subformID);
            return Json(jsonDic, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 保存抵押房产资料
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        [HttpPost]
        public string SaveHouse(FormCollection form)
        {
            if (!AppService.CheckIsAllowEdit(form["appid"].ToLong()))  //验证表单是否处于可编辑状态
            {
                return formNotEditMsg;
            }
            string houseError = String.Empty;
            APP_HOUSE house = new APP_HOUSE();

            if (!string.IsNullOrEmpty(form["appid"]))
            {
                var appid = form["appid"].ToLong();
                house = AppService.GetHouse(appid);
            }

            house = DFormHelper.InitObjVaidate(form, house, out houseError);
            return AppService.SaveHouse(house) + houseError;
        }

        /// <summary>
        /// 读取房屋权利人信息
        /// </summary>
        /// <param name="appid"></param>
        /// <param name="subformID"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetObligees(long appid, long subformID)
        {
            var list = AppService.GetObligees(appid);
            Dictionary<string, Object> str = new Dictionary<string, object>();
            foreach (var item in list)
            {
                var jsonDic = DFormHelper.GetFormJson(item, subformID);
                foreach (var dic in jsonDic.Where(c => c.Key.StartsWith(item.CONTACT_PROPERTY)))
                {
                    str.Add(dic.Key, dic.Value);
                }
            }

            return Json(str, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 保存房屋权利人信息
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        [HttpPost]
        public string SaveObligees(FormCollection form)
        {
            if (!AppService.CheckIsAllowEdit(form["appid"].ToLong()))  //验证表单是否处于可编辑状态
            {
                return formNotEditMsg;
            }
            List<string> obligeeTypes = form.AllKeys.Where(c => c.Contains("-")).Select(c => c.Split('-').FirstOrDefault()).Distinct().ToList();
            string error = string.Empty;
            //分组保存
            foreach (var obligeeType in obligeeTypes)
            {
                FormCollection f = new FormCollection();
                f.Add("subformID", form["subformID"]);
                foreach (var item in form.AllKeys.Where(c => c.StartsWith(obligeeType)))
                {
                    f.Add(item, form[item]);
                }
                string thisError;
                var entity = DFormHelper.InitObjVaidate<APP_CONTACT>(f, out thisError);
                entity.CONTACT_PROPERTY = obligeeType;
                error += thisError;
                AppService.SaveObligees(entity);
            }
            return string.Empty;
        }

        [HttpPost]
        public string Submit(long appId)
        {
            if (!AppService.CheckIsAllowEdit(appId))  //验证表单是否处于可编辑状态
            {
                return formNotEditMsg;
            }
            return AppService.SubmitLoan(appId);
        }
        #endregion
    }
}