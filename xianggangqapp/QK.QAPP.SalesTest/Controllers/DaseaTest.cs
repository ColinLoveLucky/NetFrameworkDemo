
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Razor.Parser;
using Glimpse.Mvc.Message;
using Microsoft.Practices.Unity;
using QK.QAPP.Entity;
using QK.QAPP.Global;
using QK.QAPP.Infrastructure;
using QK.QAPP.IServices;
using QK.QAPP.MvcScaffold.DForm;

namespace QK.QAPP.SalesTest.Controllers
{
    public class DaseaTestController : Controller
    {

        [Dependency]
        public IApplicationService AppService { get; set; }

        [Dependency]
        public IApplyTableService ApplyService { get; set; }

        public static List<SaveCustomerTime> saveCustomerTimes;
        private static int count = 0;
        static DaseaTestController()
        {
            saveCustomerTimes = new List<SaveCustomerTime>();
        }

        private static object lockObj = new object();


        //
        // GET: /LoanApplication/
        [AllowAnonymous]
        public ActionResult Index()
        {
        
            string id = Request["id"];
            string read = Request["read"];
            int plind = 0;
            int.TryParse(id, out plind);
            lock (lockObj)
            {
                count++;
            }
            ViewBag.Message = null;
            if (plind > 0 && count > 1)
            {
                WcdjksqdTest(count);
            }

            if (!string.IsNullOrEmpty(read))
            {
                ViewBag.Message = saveCustomerTimes;
            }

            return View();
        }

        [AllowAnonymous]
        public ActionResult Test()
        {
            string plinCountStr = Request["count"];
            int plinCount = 0;
            int.TryParse(plinCountStr, out plinCount);

            if (plinCount > 0)
            {
                Task[] tasks = new Task[plinCount];
                for (int i = 0; i < plinCount; i++)
                {
                    tasks[i] = Task.Factory.StartNew(obj =>
                    {
                        WebRequest request = WebRequest.Create("http://172.16.11.57:2014/daseatest/index?id=1");
                        WebResponse response = request.GetResponse();
                    }, i
                );
                }
                Task.WaitAll(tasks);
            }

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
            //确认权限
            var dp = AppService.CheckDataPermission(appid.ToLong(), operation);
            if (!string.IsNullOrEmpty(dp))
            {
                ViewBag.CreateCount = "<script>bootbox.alert('" + dp + "',function(){   $(window).unbind('beforeunload');window.location.href='/';});</script>";
                return View();
            }
            DFormCreater creater = new DFormCreater(dformCode, GlobalSetting.DFormVersions[dformCode], operation);
            //判断微车车贷信息 以及判断过找的到记录
            APP_MAIN mainEntity = AppService.GetAPPMain(appid.ToLong());
            if (mainEntity != null)
            {
                if (GlobalSetting.CheDaiLogos.Contains(mainEntity.LOGO) && mainEntity.CUSTOMERTYPE == GlobalSetting.CustomerJYCategory)
                {
                    var subFrom = creater.SubformList.Where(c => c.Name.Contains(GlobalSetting.CheDaiCustomerCategoryParent)).FirstOrDefault();
                    if (subFrom != null)
                    {
                        subFrom.FieldList = subFrom.FieldList.Where(c => c.Field_Group != GlobalSetting.CustomerSXGroup).ToList();
                    }
                    ViewBag.CreateCount = creater.CreateDForm(GlobalSetting.CustomerJYCategory);
                }
                else if (GlobalSetting.CheDaiLogos.Contains(mainEntity.LOGO) && mainEntity.CUSTOMERTYPE == GlobalSetting.CustomerSXCategory)
                {
                    var subFrom = creater.SubformList.Where(c => c.Name.Contains(GlobalSetting.CheDaiCustomerCategoryParent)).FirstOrDefault();
                    if (subFrom != null)
                    {
                        subFrom.FieldList = subFrom.FieldList.Where(c => c.Field_Group != GlobalSetting.CustomerJYGroup).ToList();
                    }
                    ViewBag.CreateCount = creater.CreateDForm(GlobalSetting.CustomerSXCategory);
                }
                else
                {
                    ViewBag.CreateCount = creater.CreateDForm();
                }

            }

            return View();
        }



        #region 申请表单测试

        /// <summary>
        /// 薪易贷借款申请单
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public JsonResult XydjksqdTest(int plinCount)
        {






            //申请人信息 1 /LoanApplication/SaveCustomerBasic /LoanApplication/GetCustomerBasic 1   
            // 

            //联系人资料 1 /LoanApplication/EditContacts /LoanApplication/GetContacts 2   
            // 

            //银行信息 1 /LoanApplication/SaveBankCard /LoanApplication/GetBankCard 3   
            // 

            //员工专用栏 1 /LoanApplication/SaveStaffOnly /LoanApplication/GetStaffOnly 4 

            return Json(plinCount, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// 商易贷借款申请单
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public JsonResult SydjksqdTest(int plinCount)
        {
            //申请人信息 1 /LoanApplication/SaveCustomerBasic /LoanApplication/GetCustomerBasic 1   
            // 

            //联系人资料 1 /LoanApplication/EditContacts /LoanApplication/GetContacts 2   
            // 

            //银行信息 1 /LoanApplication/SaveBankCard /LoanApplication/GetBankCard 3   
            // 

            //员工专用栏 1 /LoanApplication/SaveStaffOnly /LoanApplication/GetStaffOnly 4 

            return Json(plinCount, JsonRequestBehavior.AllowGet);
        }


        FormCollection createFormCollection(int order)
        {
            FormCollection form = new FormCollection();
            //////<----->
            //////APP_CUSTOMER 个人基本信息 主键 PrimaryKey
            //form.Add("APP_Customer_ID", "");

            ////APP_CUSTOMER 个人基本信息 外键 PrimaryKey
            form.Add("APP_Customer_APPID", "5471");

            ////APP_CUSTOMER 个人基本信息 姓名 Text
            form.Add("CustomerName", "魏翔单元测试xyd" + order);

            ////APP_CUSTOMER 个人基本信息 身份证号码 Text
            form.Add("CustomerIDNumber", "330702197108020000");

            ////APP_CUSTOMER 个人基本信息 出生日期 DatePicker
            form.Add("CustomerBirthday", "1992-08-01");

            ////APP_CUSTOMER 个人基本信息 性别 Select
            form.Add("CustomerGender", "genderM");

            ////APP_CUSTOMER 个人基本信息 婚姻状态 Select
            form.Add("MarrageStatus", "marrageSingle");

            ////APP_CUSTOMER 个人基本信息 婚姻状态其他 Other
            form.Add("MarrageStatusOther", "");

            ////APP_CUSTOMER 个人基本信息 教育程度 Select
            form.Add("Education", "educationA");

            ////APP_CUSTOMER 个人基本信息 本市房产情况 Select
            form.Add("HasLocalHouse", "hasLocalHouseNone");

            ////APP_CUSTOMER 个人基本信息 住房月还款 Number
            form.Add("MortgageAMTByMonth", "1000");

            ////APP_CUSTOMER 个人基本信息 名下是否有车 Select
            form.Add("HasCar", "CONSTANTS_YES");

            ////APP_CUSTOMER 个人基本信息 自有车辆品牌 Text
            form.Add("BrandOwned", "宝马");

            ////APP_CUSTOMER 个人基本信息 自有车辆型号 Text
            form.Add("ModelOwned", "X6");

            ////APP_CUSTOMER 个人基本信息 居住情况 Select
            form.Add("ResidentStatus", "OwnResStatusB");

            ////APP_CUSTOMER 个人基本信息 居中情况其他 Other
            form.Add("ResidentStatusOther", "");

            ////APP_CUSTOMER 个人基本信息 现居住地址 Adress
            form.Add("NowAdress", "");

            ////APP_CUSTOMER 个人基本信息 邮编 Text
            form.Add("PostCode", "475400");

            ////APP_CUSTOMER 个人基本信息 在此城市居住年限 Number
            form.Add("InLocalYear", "3");

            ////APP_CUSTOMER 个人基本信息 居住地电话 TelNumber
            form.Add("ResidentTel", "15838338233");

            ////APP_CUSTOMER 个人基本信息 户主与本人关系 Text
            form.Add("RelationShipOfResident", "朋友");

            ////APP_CUSTOMER 个人基本信息 手机号码1 TelNumber
            form.Add("Mobile1", "15838338233");

            ////APP_CUSTOMER 个人基本信息 手机号码2 TelNumber
            form.Add("Mobile2", "15838338233");

            ////APP_CUSTOMER 个人基本信息 户籍地址 Adress
            form.Add("REGISTERAdress", "");

            ////APP_CUSTOMER 个人基本信息 信用卡最高额度 Number
            form.Add("MAX_CREDITLIMIT_OF_CCC", "100");

            ////APP_CUSTOMER 个人基本信息 本人持有信用卡数量 Number
            form.Add("NUMS_OF_CCC", "10");

            ////APP_CUSTOMER 个人基本信息 邮箱 Text
            form.Add("EMAIL", "leiz@qq.com");

            ////APP_CUSTOMER 个人基本信息 QQ Text
            form.Add("QQ", "756826958");

            ////APP_JOB 职业信息 现职公司全称 Text
            form.Add("COM_NAME", "上海夸客金融");

            ////APP_JOB 职业信息 部门 Text
            form.Add("COM_DEPT", "销售部门");

            ////APP_JOB 职业信息 职位 Select
            form.Add("COM_POSITION", "销售专员");

            ////APP_JOB 职业信息 职位其他 Other
            form.Add("COM_POSITION_OTHER", "");

            ////APP_JOB 职业信息 公司地址 Adress
            form.Add("COM_ADDRESS", "");

            ////APP_JOB 职业信息 公司类型 Select
            form.Add("COM_TYPE", "comTypeCC");

            //// 职业信息 公司类型其他 Other
            form.Add("COM_TYPE_OTHER", "");

            ////APP_JOB 职业信息 公司固定电话 TelNumber
            form.Add("COM_TEL_NO", "15838338233");

            ////APP_JOB 职业信息 入司时间 DatePicker
            form.Add("DATE_JOIN", "2011-11-01");

            ////APP_JOB 职业信息 主键 PrimaryKey
            form.Add("APP_JOB_ID", "");

            ////APP_JOB 职业信息 外键 PrimaryKey
            form.Add("APP_JOB_APPID", "5471");

            ////APP_CUSTOMER 收支情况 个人年度总收入 Number
            form.Add("INCOME_ANNUAL", "10");

            ////APP_CUSTOMER 收支情况 每月工作收入 Number
            form.Add("INCOME_MONTHLY_FROM_JOB", "6000");

            ////APP_CUSTOMER 收支情况 每月其他收入 Number
            form.Add("INCOME_MONTHLY_FROM_OTHER", "2000");

            ////APP_CUSTOMER 收支情况 每月其他收入来源 Text
            form.Add("MEMO_OF_INCOME_FROM_OTHER", "");

            ////APP_CUSTOMER 收支情况 目前需要供养人数 Number
            form.Add("NUMS_OF_PROVIDE", "3");

            form.Add("subformID", "26002");

            return form;
        }

      
        /// <summary>
        /// 微车贷借款申请单
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public void WcdjksqdTest(int plinCount)
        {
            saveCustomerTimes.Add(SaveCustomerBasic(createFormCollection(plinCount)));
        
            //var taskArray = new Task<SaveCustomerTime>[plinCount];
            //var forms = new FormCollection[plinCount];

            //for (int i = 0; i < plinCount; i++)
            //{
            //    forms[i] = createFormCollection(i);
            //}

            //for (int i = 0; i < plinCount; i++)
            //{
            //    int i1 = i;
            //    taskArray[i] = Task.Factory.StartNew<SaveCustomerTime>(obj =>
            //    {
            //      var data=  SaveCustomerBasic(forms[i1]);
            //      data.Order = i1;
            //      return data;
            //    }, i1);
            //}

            //Task.WaitAll(taskArray);
            //var dataCollection =new List<SaveCustomerTime>();
            //for (int i = 0; i < taskArray.Length; i++)
            //{
            //    dataCollection.Add(taskArray[i].Result);
            //}

            //dataCollection.Sort();


            ////申请人信息 1 /LoanApplication/SaveCustomerBasic /LoanApplication/GetCustomerBasic 1   
            //// 

            ////联系人资料 1 /LoanApplication/EditContacts /LoanApplication/GetContacts 2   
            //// 

            ////所购车辆信息 1 /LoanApplication/SaveCarInfo /LoanApplication/GetCarInfo 3   
            //// 

            ////银行信息 1 /LoanApplication/SaveBankCard /LoanApplication/GetBankCard 4   
            //// 

            ////员工专用栏 1 /LoanApplication/SaveStaffOnly /LoanApplication/GetStaffOnly 5 

            //return dataCollection;
        }

        #endregion


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


        public class SaveCustomerTime : IComparable
        {
            public int Order { get; set; }

            public long MapTime { get; set; }

            public long UpdateTime { get; set; }

            public long TotalTime
            {
                get { return this.MapTime + this.UpdateTime; }
            }

            public int CompareTo(object obj)
            {
                return this.Order > ((SaveCustomerTime)obj).Order ? 0 : 1;
            }
        }

        /// <summary>
        /// 保存基本信息
        /// </summary>
        /// <param name="form"></param>
        [HttpPost]
        [LogicalActionFilter(ActionSummary = "保存申请表单的个人基本信息")]
        public SaveCustomerTime SaveCustomerBasic(FormCollection form)
        {
            SaveCustomerTime saveCustomer = new SaveCustomerTime();
            Stopwatch watch = new Stopwatch();
            saveCustomer.Order = count;
            watch.Start();
            string customerError = "";
            string customerJobError = "";
            var customer = DFormHelper.InitObjVaidate<APP_CUSTOMER>(form, out customerError);
            var customerJob = DFormHelper.InitObjVaidate<APP_JOB>(form, out customerJobError);
            watch.Stop();
            saveCustomer.MapTime = watch.ElapsedMilliseconds;

            watch.Reset();
            watch.Start();
            string msg = AppService.SaveCustomerBasic(customer, customerJob) + customerError + customerJobError;
            watch.Stop();
            saveCustomer.UpdateTime = watch.ElapsedMilliseconds;

            return saveCustomer;

        }
        /// <summary>
        /// 保存银行卡信息
        /// </summary>
        /// <param name="form"></param>
        [HttpPost]
        public string SaveBankCard(FormCollection form)
        {
            string customerError = "";
            var customer = DFormHelper.InitObjVaidate<APP_BANKCARD>(form, out customerError);
            return AppService.SaveCustomerBankCard(customer) + customerError;
        }
        /// <summary>
        /// 保存员工专用栏信息
        /// </summary>
        /// <param name="form"></param>
        [HttpPost]
        public string SaveStaffOnly(FormCollection form)
        {
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
        /// 保存备注
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        public string SaveMemo(FormCollection form)
        {
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
            string carInfoError = "";
            var carInfo = DFormHelper.InitObjVaidate<APP_CARINFO>(form, out carInfoError);
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
        #endregion

        [HttpPost]
        public string Submit(long appId)
        {
            return AppService.SubmitLoan(appId);
        }
        #endregion

    }
}