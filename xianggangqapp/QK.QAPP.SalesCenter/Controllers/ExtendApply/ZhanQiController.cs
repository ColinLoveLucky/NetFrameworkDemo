using Microsoft.Practices.Unity;
using QK.QAPP.Entity;
using QK.QAPP.Infrastructure;
using QK.QAPP.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QK.QAPP.Global;

namespace QK.QAPP.SalesCenter.Controllers
{
    public class ZhanQiController : Controller
    {
        #region 属性

        [Dependency]
        public IExtendApplyService extendApplyService { get; set; }

        [Dependency]
        public IQFUserService qfuserService { get; set; }

        [Dependency]
        public IAPP_MAINSERVICE AppMainService { get; set; }

        [Dependency]
        public IAPP_CITYSERVICE AppCityService { get; set; }

        [Dependency]
        public IQFProductInfoService ProductInfoService { get; set; }

        [Dependency]
        public ICR_DATA_DICService DicService { get; set; }

        [Dependency]
        public IExtendApplicationService ExtendAppService { get; set; }

        [Dependency]
        public IAPP_EXTEND_CONFIGSERVICE ExtendConfigService { get; set; }

        [Dependency]
        public ILoanApplicationService LoanApplicationService { get; set; }
        #endregion

        #region 展期列表

        public ActionResult ZhanQiList(string menuCode)
        {
            ViewData["menuCode"] = menuCode;
            ViewData["NeedExtendStatus_Extend"] = extendApplyService.NeedExtendStatus_Extend;
            ViewData["Order_ExceptSD_Status"] = extendApplyService.Order_ExceptSD_Status_Car;
            //汽车金融部和个人金融部的展期申请页不同，在此以menuCode做区分
            ViewData["zhanQiApplicationPage"] = menuCode == "GJVEHICLE" ? "GJZhanQiApplication" : "ZhanQiApplication";

            //按钮授权
            string url = Request.Url.PathAndQuery;
            List<APP_Button> listButtons = qfuserService.GetButtonByUrl(url);
            ViewData["Permission_Buttons"] = Newtonsoft.Json.JsonConvert.SerializeObject(listButtons);
            return View("~/Views/ZhanQi/ZhanQiList.cshtml");
        }

        public ActionResult HouseList()
        {
            ViewData["NeedExtendStatus_Extend"] = extendApplyService.NeedExtendStatus_Extend;
            ViewData["Order_ExceptSD_Status"] = GlobalSetting.Order_ExceptSD_Status_House;

            //按钮授权
            string url = Request.Url.PathAndQuery;
            List<APP_Button> listButtons = qfuserService.GetButtonByUrl(url);
            ViewData["Permission_Buttons"] = Newtonsoft.Json.JsonConvert.SerializeObject(listButtons);
            return View("~/Views/ZhanQi/HouseList.cshtml");
        }

        /// <summary>
        /// 车贷展期列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetZhanQiList(string menuCode)
        {
            //menuCode
            //"VEHICLE":车贷部车贷
            //"GJVEHICLE":个金部车贷
            //截至到2016-3-17 两者都是从车贷拆分得到的，除产品名称和logo不一样外，其他都一样，走得流程和状态也一样
            menuCode = string.IsNullOrEmpty(menuCode) ? "VEHICLE" : menuCode;
            var para = new ExtendApplySearchPara
            {
                //车贷非补件状态
                OrderExceptSDStatus = extendApplyService.Order_ExceptSD_Status_Car,
                //是否显示拒贷码
                IsDisplayRefuseFunc = LoanApplicationService.IsDisplayRefuseLoanCar,
                //车贷Logo
                ListLogo = GlobalSetting.LogoGroupForMenu[menuCode],
                //车贷可展条件
                ExtendCondition = extendApplyService.AddExtendConditionCar,
                //可展列表数据权限
                DataPermissionExtend = extendApplyService.AddExtendPermission,
                // 查询APP_AUTH表的MENUCODE，在从不同菜单查询相同logo产品时使用
                // lys 2016-3-30
                InputMenuCode = menuCode
            };

            var ret = GetZhanQiList(para);

            return ret;
        }

        [HttpPost]
        public JsonResult GetHouseList()
        {
            var para = new ExtendApplySearchPara
            {
                //房贷非补件状态
                OrderExceptSDStatus = GlobalSetting.Order_ExceptSD_Status_House,
                //是否显示拒贷码
                IsDisplayRefuseFunc = LoanApplicationService.IsDisplayRefuseLoanHouse,
                //车贷Logo
                ListLogo = GlobalSetting.LogoGroupForMenu["HOUSE"],
                //房贷可展条件
                ExtendCondition = extendApplyService.AddExtendConditionHouse,
                //房贷可展列表数据权限
                DataPermissionExtend = extendApplyService.AddExtendPermissionHouse

            };

            var ret = GetZhanQiList(para);

            return ret;
        }

        private JsonResult GetZhanQiList(ExtendApplySearchPara para)
        {
            if(para == null)
                para = new ExtendApplySearchPara();
            //数据权限
            para.AccessableCsac = qfuserService.GetDataPermission();
            para.PageIndex = int.Parse(Request["page"] ?? "1");
            para.PageSize = int.Parse(Request["rows"] ?? "1");
            para.Sort = new Dictionary<string, string>();
            if (!string.IsNullOrEmpty(Request["sidx"]) && !string.IsNullOrEmpty(Request["sord"]))
            {
                para.Sort.Add(Request["sidx"], Request["sord"]);
            }
            //申请单号
            para.AppCode = HttpUtility.UrlDecode(Request["appCode"]);
            //客户姓名
            para.CustomerName = HttpUtility.UrlDecode(Request["customerName"]);
            //客户身份证
            para.CustomerIDCard = HttpUtility.UrlDecode(Request["customerIDCard"]);
            //申请时间开始
            if (Request["applyStart"] != null && Request["applyStart"].ToString() != string.Empty)
            {
                DateTime dtm = DateTime.Now;
                if (DateTime.TryParse(Request["applyStart"].ToString() + " 00:00:00", out dtm))
                    para.ApplyStart = dtm;
            }
            //申请时间结束
            if (Request["applyEnd"] != null && Request["applyEnd"].ToString() != string.Empty)
            {
                DateTime dtm = DateTime.Now;
                if (DateTime.TryParse(Request["applyEnd"].ToString() + " 23:59:59", out dtm))
                    para.ApplyEnd = dtm;
            }
            //客户经理
            para.SaleCode = HttpUtility.UrlDecode(Request["sales"]);
            para.SaleName = HttpUtility.UrlDecode(Request["sales"]);
            //客服
            para.CsacCode = HttpUtility.UrlDecode(Request["csac"]);
            para.CsacName = HttpUtility.UrlDecode(Request["csac"]);
            //是否是Search窗口的模糊查询
            if (Request["fuzzySearch"] != null && Request["fuzzySearch"].ToString() == "1")
            {
                para.FuzzySearch = true;
            }
            //查询创建时间而不是更改时间
            para.NeedTag = false;
            para.ExtendActionGroup = extendApplyService.ActionGroup_Extend;

            //进件状态,如果没有选择进件状态，则默认查出所有补件
            if (Request["enterStasus"] != null && Request["enterStasus"].ToString() != string.Empty)
            {
                string strStatus = Request["enterStasus"].ToString();
                string[] aryStatus = strStatus.Split(',');
                foreach (string s in aryStatus)
                {
                    if (string.IsNullOrEmpty(s))
                        continue;
                    para.ListEnterStatus.Add((EnterStatusType)Enum.Parse(typeof(EnterStatusType), s));
                }
            }

            if (Request["searchToBe"] != null && Request["searchToBe"].ToString().ToUpper() == "TRUE")
            {
                if (para.ListEnterStatus.Count == 0)
                {
                    foreach (KeyValuePair<string, string> kv in extendApplyService.NeedExtendStatus_Extend)
                    {
                        para.ListEnterStatus.Add((EnterStatusType)Enum.Parse(typeof(EnterStatusType), kv.Key));
                    }
                }

                ExtendApplyViewFieldList ret = extendApplyService.ExtendListToBe(para);

                //重写列表状态显示
                RewriteVAppMainExtendStatusName(ret.ListEnter);

                return Json(ret, JsonRequestBehavior.AllowGet);
            }
            else
            {
                if (para.ListEnterStatus.Count == 0)
                {
                    foreach (KeyValuePair<string, string> kv in para.OrderExceptSDStatus)
                    {
                        string[] aryTemp = kv.Key.Split(',');
                        foreach (string s in aryTemp)
                        {
                            if (!string.IsNullOrWhiteSpace(s))
                            {
                                para.ListEnterStatus.Add((EnterStatusType)Enum.Parse(typeof(EnterStatusType), s));
                            }
                        }
                    }
                }
                EnterListViewFiledList ret = extendApplyService.ExtendedList(para);
                //重写列表状态显示
                RewriteVAppMainStatusName(ret.ListEnter, para.OrderExceptSDStatus, para.IsDisplayRefuseFunc);

                return Json(ret, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult GetZhanQiHistory(string appCode)
        {
            var lstAppList = InternalGetHistory(appCode, GlobalSetting.Order_ExceptSD_Status_Car);
            return Json(lstAppList, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetHouseHistory(string appCode)
        {
            var lstAppList = InternalGetHistory(appCode, GlobalSetting.Order_ExceptSD_Status_House);
            return Json(lstAppList, JsonRequestBehavior.AllowGet);
        }

        private List<APP_EXTEND_RELATION> InternalGetHistory(string appCode, Dictionary<string, string> orderExceptSdStatus)
        {
            /*注意：这里查询数据时 APP_EXTEND_RELATION 中
             * 字段 CONTRACT_ID 意义为 订单状态 APPSTATUS
             * 字段 INIT_CONTRACTID 意义为 订单状态名 APPSTATUSNAME
            */
            List<APP_EXTEND_RELATION> lstAppList = extendApplyService.ExtendHistory(appCode, extendApplyService.ActionGroup_Extend[0]);
            //重写列表状态显示
            foreach (var item in lstAppList)
            {
                foreach (KeyValuePair<string, string> kv in orderExceptSdStatus)
                {
                    string[] statuName = kv.Value.Split(',');
                    if (!string.IsNullOrWhiteSpace(kv.Key) && kv.Key.Contains(item.CONTRACT_ID))
                    {
                        string[] statuCode = kv.Key.Split(',');
                        foreach (var status in statuCode)
                        {
                            if (status == item.CONTRACT_ID)
                            {
                                if (!string.IsNullOrWhiteSpace(statuName[1]))
                                {
                                    item.INIT_CONTRACTID = "<span class='label label-" + statuName[1] + "'>" + statuName[0] + "</span>";
                                }
                                else
                                {
                                    item.INIT_CONTRACTID = "<span class='label label-info'>" + statuName[0] + "</span>";
                                }
                            }
                        }

                    }
                }
            }

            return lstAppList;
        }

        /// <summary>
        /// 重写V_APPMAIN中AppStatusName为配置的内容
        /// </summary>
        /// <param name="list"></param>
        /// <param name="orderExceptSDStatus">非补件状态</param>
        /// <param name="isDisplayRefuseFunc">是否显示拒贷权限判断</param>
        private void RewriteVAppMainStatusName(List<V_APPMAIN> list, Dictionary<string, string> orderExceptSDStatus, Func<string, bool> isDisplayRefuseFunc)
        {
            //重写列表状态显示
            foreach (var item in list)
            {
                foreach (KeyValuePair<string, string> kv in orderExceptSDStatus)
                {
                    string[] statuName = kv.Value.Split(',');
                    if (!string.IsNullOrWhiteSpace(kv.Key) && kv.Key.Contains(item.APPSTATUS))
                    {
                        string[] statuCode = kv.Key.Split(',');
                        foreach (var status in statuCode)
                        {
                            if (status == item.APPSTATUS)
                            {
                                if (!string.IsNullOrWhiteSpace(statuName[1]))
                                {
                                    /*item.APPSTATUSNAME = "<span class='label label-" + statuName[1] + "'>" + statuName[0] + "</span>";  //V2*/
                                    /*描述：如果是拒贷申请并且有权限，在办理状态加入下划线<u>标签,记录appid
                                      时间：2015-03-11
                                      修改者：leiz*/
                                    if (isDisplayRefuseFunc(status))
                                    {
                                        item.APPSTATUSNAME = "<span class='label label-" + statuName[1] + " tooltip-" + statuName[1] + " refuseLoanStatus' data-appid=" + item.APPID + "><u>" + statuName[0] + "</u></span>";
                                    }
                                    else
                                    {
                                        item.APPSTATUSNAME = "<span class='label label-" + statuName[1] + "'>" + statuName[0] + "</span>";
                                    }
                                }
                                else
                                {
                                    item.APPSTATUSNAME = "<span class='label label-info'>" + statuName[0] + "</span>";
                                }
                            }
                        }

                    }
                }
            }
        }

        /// <summary>
        /// 重写V_APPMAIN_EXTEND中AppStatusName为配置的内容
        /// </summary>
        /// <param name="list"></param>
        private void RewriteVAppMainExtendStatusName(List<V_APPMAIN_EXTEND> list)
        {
            //重写列表状态显示
            foreach (var item in list)
            {
                foreach (KeyValuePair<string, string> kv in GlobalSetting.Order_ExceptSD_Status_Car)
                {
                    string[] statuName = kv.Value.Split(',');
                    if (!string.IsNullOrWhiteSpace(kv.Key) && kv.Key.Contains(item.AppStatus))
                    {
                        string[] statuCode = kv.Key.Split(',');
                        foreach (var status in statuCode)
                        {
                            if (status == item.AppStatus)
                            {
                                if (!string.IsNullOrWhiteSpace(statuName[1]))
                                {
                                    item.AppStatusName = "<span class='label label-" + statuName[1] + "'>" + statuName[0] + "</span>";
                                }
                                else
                                {
                                    item.AppStatusName = "<span class='label label-info'>" + statuName[0] + "</span>";
                                }
                            }
                        }

                    }
                }
            }
        }

        #endregion

        #region 车贷展期申请

        /// <summary>
        /// 展示展期申请页面
        /// </summary>
        /// <param name="appId">申请单APP_ID（根据操作不同，1为被展单子的ID，2和3为当前单子ID）</param>
        /// <param name="operation">操作（1-申请，2-编辑，3-只读）</param>
        /// <returns></returns>
        public ActionResult ZhanQiApplication(long appId, ENUM_FormOperation operation)
        {
            var appMain = AppMainService.Find(a => a.ID == appId).FirstOrDefault();
            //验证权限
            var msg = ExtendAppService.CheckExtendPermission(appMain, operation, ExtendAppService.CanExtend);
            if (string.IsNullOrEmpty(msg))
            {
                SetViewData(appMain, operation, SetViewDataCar);
                ViewData["IsDisplayRefuseLoan"] = LoanApplicationService.IsDisplayRefuseLoanCar(appMain.APP_STATUS);
                switch (operation)
                {
                    case ENUM_FormOperation.ADD:
                        ViewBag.IsAdd = true;
                        ViewBag.IsEdit = false;
                        ViewBag.IsDisabled = false;
                        break;
                    case ENUM_FormOperation.EDIT:
                        ViewBag.IsAdd = false;
                        ViewBag.IsEdit = true;
                        ViewBag.IsDisabled = true;
                        break;
                    case ENUM_FormOperation.READONLY:
                        ViewBag.IsAdd = false;
                        ViewBag.IsEdit = false;
                        ViewBag.IsDisabled = true;
                        break;
                }
            }
            else
            {
                ViewData["noPermission"] = msg;
            }


            return View("~/Views/ZhanQi/ZhanQiApplication.cshtml");
        }

        /// <summary>
        /// 个人金融部展期申请页面
        /// </summary>
        /// <param name="appId"></param>
        /// <param name="operation"></param>
        /// <returns></returns>
        public ActionResult GJZhanQiApplication(long appId, ENUM_FormOperation operation)
        {
            var appMain = AppMainService.Find(a => a.ID == appId).FirstOrDefault();
            //验证权限
            var msg = ExtendAppService.CheckExtendPermission(appMain, operation, ExtendAppService.CanExtend);
            if (string.IsNullOrEmpty(msg))
            {
                SetViewData(appMain, operation, SetViewDataGJCar);
                ViewData["IsDisplayRefuseLoan"] = LoanApplicationService.IsDisplayRefuseLoanCar(appMain.APP_STATUS);
                switch (operation)
                {
                    case ENUM_FormOperation.ADD:
                        ViewBag.IsAdd = true;
                        ViewBag.IsEdit = false;
                        ViewBag.IsDisabled = false;
                        break;
                    case ENUM_FormOperation.EDIT:
                        ViewBag.IsAdd = false;
                        ViewBag.IsEdit = true;
                        ViewBag.IsDisabled = true;
                        break;
                    case ENUM_FormOperation.READONLY:
                        ViewBag.IsAdd = false;
                        ViewBag.IsEdit = false;
                        ViewBag.IsDisabled = true;
                        break;
                }
            }
            else
            {
                ViewData["noPermission"] = msg;
            }


            return View("~/Views/ZhanQi/GJZhanQiApplication.cshtml");
        }

        /// <summary>
        /// 处理展期申请表单的提交（会复制被展申请单中的信息）
        /// </summary>
        /// <param name="collection"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ZhanQiApplication(FormCollection collection)
        {
            string isRedirect = string.Empty;
            string resultMsg;

            //验证表单必填字段信息
            var formDic = CheckFormData(collection, null, out resultMsg);  //CheckFormDataCar

            //必填字段信息有误
            if (!string.IsNullOrEmpty(resultMsg))
            {
                Infrastructure.Log4Net.LogWriter.Error("展期申请：" + resultMsg);
                return Json(new { resultMsg, isRedirect });
            }

            long appId = formDic["appId"].ToInt64();

            //展期申请
            var appMain = ExtendAppService.ExtendLoanCar(appId, formDic, out resultMsg);

            if (!string.IsNullOrEmpty(resultMsg))
            {
                return Json(new { resultMsg, isRedirect });
            }

            resultMsg = "/LoanApplication/ExtendApplication?dformCode=" + appMain.LOGO + "&operation=1&appid=" + appMain.ID;
            isRedirect = "true";

            return Json(new { resultMsg, isRedirect });
        }
        /// <summary>
        /// 设置车贷展期申请页面的ViewData
        /// </summary>
        /// <param name="appMainEntity">APP_MAIN对象</param>
        /// <param name="operation">操作（1-申请，2-编辑，3-只读）</param>
        private void SetViewDataCar(APP_MAIN appMainEntity, ENUM_FormOperation operation)
        {
            //合作渠道
            ViewBag.Plantform = GetCarPlantform();
            //车贷logos
            ViewBag.CheDaiLogos = GetCheDaiLogos();
            //城市列表
            ViewBag.CityList = GetCityList("VEHICLE");
            //借款用途
            ViewBag.LoanPurpose = GetCarLoanPurpose();
            //原单到手金额 update by ruiwang 2016/05/16
            switch (operation)
            {
                //若为新增，当前appid的进件即为原单
                case ENUM_FormOperation.ADD:
                    var appLoan = appMainEntity.APP_LOAN.FirstOrDefault();
                    if (appLoan != null)
                    {
                        ViewData["OriApplyAmt"] = appLoan.LOAN_AMT;
                    }
                    break;
                //若为编辑，则需要通过当前appid找到原单
                case ENUM_FormOperation.EDIT:
                    ViewData["OriApplyAmt"] = ExtendAppService.GetOriLoanAmt(appMainEntity);
                    break;
            }
            if (appMainEntity != null)
            {
                APP_CARINFO appCarInfoEntity = appMainEntity.APP_CARINFO.FirstOrDefault();
                if (appCarInfoEntity != null)
                {
                    //购车类型Code
                    ViewData["carKindCode"] = appCarInfoEntity.CAR_KIND;
                    //购车类型
                    ViewData["carKindName"] = DicService.GetDICNameByCode(appCarInfoEntity.CAR_KIND);
                    //发票价格
                    ViewData["carSellingPrice"] = appCarInfoEntity.CAR_SELLINGPRICE;
                }

                if (Global.GlobalSetting.CheDaiLogos.Contains(appMainEntity.LOGO))
                {
                    //是否显示车辆信息
                    ViewData["carDisplay"] = "display";
                }

            }

        }

        private void SetViewDataGJCar(APP_MAIN appMainEntity, ENUM_FormOperation operation)
        {
            //合作渠道
            ViewBag.Plantform = GetGJCarPlantform();
            //车贷logos
            ViewBag.CheDaiLogos = GetCheDaiLogos();
            //城市列表
            ViewBag.CityList = GetCityList("GJVEHICLE");
            //借款用途
            ViewBag.LoanPurpose = GetCarLoanPurpose();
            //原单到手金额 update by ruiwang 2016/05/16
            switch (operation)
            {
                //若为新增，当前appid的进件即为原单
                case ENUM_FormOperation.ADD:
                    var appLoan = appMainEntity.APP_LOAN.FirstOrDefault();
                    if (appLoan != null)
                    {
                        ViewData["OriApplyAmt"] = appLoan.LOAN_AMT;
                    }
                    break;
                //若为编辑，则需要通过当前appid找到原单
                case ENUM_FormOperation.EDIT:
                    ViewData["OriApplyAmt"] = ExtendAppService.GetOriLoanAmt(appMainEntity);
                    break;
            }
            if (appMainEntity != null)
            {
                APP_CARINFO appCarInfoEntity = appMainEntity.APP_CARINFO.FirstOrDefault();
                if (appCarInfoEntity != null)
                {
                    //购车类型Code
                    //ViewData["carKindCode"] = appCarInfoEntity.CAR_KIND;
                    //购车类型
                    //ViewData["carKindName"] = DicService.GetDICNameByCode(appCarInfoEntity.CAR_KIND);
                    //发票价格
                    ViewData["carSellingPrice"] = appCarInfoEntity.CAR_SELLINGPRICE;
                }

                if (Global.GlobalSetting.CheDaiLogos.Contains(appMainEntity.LOGO))
                {
                    //是否显示车辆信息
                    ViewData["carDisplay"] = "display";
                }

            }
        }

        /// <summary>
        /// 车贷表单验证
        /// </summary>
        /// <param name="collection"></param>
        /// <returns></returns>
        private string CheckFormDataCar(FormCollection collection)
        {
            string resultMsg = string.Empty;

            if (collection.AllKeys.Contains("carDisplay") && collection["carDisplay"] == "display")
            {
                
            }

            return resultMsg;
        }

        #endregion

        #region 房贷展期

        /// <summary>
        /// 房贷展期申请页面展示
        /// </summary>
        /// <param name="appId">申请单APP_ID（根据操作不同，1为被展单子的ID，2和3为当前单子ID</param>
        /// <param name="operation">操作（1-申请，2-编辑，3-只读）</param>
        /// <returns></returns>
        public ActionResult HouseApplication(long appId, ENUM_FormOperation operation)
        {
            var appMain = AppMainService.Find(a => a.ID == appId).FirstOrDefault();
            //验证权限
            var msg = ExtendAppService.CheckExtendPermission(appMain, operation, ExtendAppService.CanExtendHouse);
            if (string.IsNullOrEmpty(msg))
            {
                SetViewData(appMain, operation, SetViewDataHouse);
                ViewData["IsDisplayRefuseLoan"] = LoanApplicationService.IsDisplayRefuseLoanHouse(appMain.APP_STATUS);
                switch (operation)
                {
                    case ENUM_FormOperation.ADD:
                        ViewBag.IsAdd = true;
                        ViewBag.IsEdit = false;
                        ViewBag.IsDisabled = false;
                        break;
                    case ENUM_FormOperation.EDIT:
                        ViewBag.IsAdd = false;
                        ViewBag.IsEdit = true;
                        ViewBag.IsDisabled = true;
                        break;
                    case ENUM_FormOperation.READONLY:
                        ViewBag.IsAdd = false;
                        ViewBag.IsEdit = false;
                        ViewBag.IsDisabled = true;
                        break;
                }
            }
            else
            {
                ViewData["noPermission"] = msg;
            }


            return View("~/Views/ZhanQi/HouseApplication.cshtml");
        }

        /// <summary>
        /// 房贷展期申请提交
        /// </summary>
        /// <param name="collection"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult HouseApplication(FormCollection collection)
        {
            string isRedirect = string.Empty;
            string resultMsg;

            //验证表单必填字段信息
            var formDic = CheckFormData(collection, CheckFormDataHouse, out resultMsg);

            //必填字段信息有误
            if (!string.IsNullOrEmpty(resultMsg))
            {
                Infrastructure.Log4Net.LogWriter.Error("房贷展期申请：" + resultMsg);
                return Json(new { resultMsg, isRedirect });
            }

            long appId = formDic["appId"].ToInt64();

            //展期申请
            var appMain = ExtendAppService.ExtendLoanHouse(appId, formDic, out resultMsg);

            if (!string.IsNullOrEmpty(resultMsg))
            {
                return Json(new { resultMsg, isRedirect });
            }

            resultMsg = "/LoanApplication/ExtendApplication?dformCode=" + appMain.LOGO + "&operation=1&appid=" + appMain.ID;
            isRedirect = "true";

            return Json(new { resultMsg, isRedirect });
        }

        /// <summary>
        /// 设置房贷展期申请页面ViewData
        /// </summary>
        /// <param name="appMain"></param>
        /// <param name="operation"></param>
        private void SetViewDataHouse(APP_MAIN appMain, ENUM_FormOperation operation)
        {
            //合作渠道
            ViewBag.Plantform = GetPlantformHouse();
            //城市列表
            ViewBag.CityList = GetCityList("HOUSE");
            //评估价值
            APP_HOUSE appHouseInfoEntity = appMain.APP_HOUSE.FirstOrDefault();
            if (appHouseInfoEntity != null)
            {
                ViewData["assessmentPrice"] = appHouseInfoEntity.ASSESSMENT_VALUE;
            }

            //原单合同金额
            switch (operation)
            {
                //若为新增，当前appid的进件即为原单
                case ENUM_FormOperation.ADD:
                    var appLoan = appMain.APP_LOAN.FirstOrDefault();
                    if (appLoan != null)
                    {
                        ViewData["OriContactAmt"] = appLoan.LOAN_AMT_OF_CONTRACT;
                    }
                    break;
                //若为编辑，则需要通过当前appid找到原单
                case ENUM_FormOperation.EDIT:
                    ViewData["OriContactAmt"] = ExtendAppService.GetOriContactAmt(appMain);
                    break;
            }
        }

        /// <summary>
        /// 验证表单必填字段信息等（房贷展期）
        /// </summary>
        /// <param name="collection"></param>
        /// <returns></returns>
        private string CheckFormDataHouse(FormCollection collection)
        {
            string resultMsg = string.Empty;

            if (!collection.AllKeys.Contains("contractValue") || string.IsNullOrEmpty(collection["contractValue"]))
            {
                resultMsg += "请输入正确的合同金额！";
            }
            if (!collection.AllKeys.Contains("assessmentPrice") || string.IsNullOrEmpty(collection["assessmentPrice"]))
            {
                resultMsg += "请输入评估价值！";
            }
            if (!collection.AllKeys.Contains("customerType") || string.IsNullOrEmpty(collection["customerType"]))
            {
                resultMsg += "客户类型不能为空！";
            }

            return resultMsg;
        }

        #endregion

        #region 内部共用方法

        /// <summary>
        /// 验证表单必填字段信息，并将FormCollection转为Dictionary
        /// </summary>
        /// <param name="collection"></param>
        /// <param name="checkFormDataFun">验证表单不同部分</param>
        /// <param name="resultMsg"></param>
        /// <returns>错误信息（如果没错则为空字符串）</returns>
        private Dictionary<string, string> CheckFormData(FormCollection collection, Func<FormCollection, string> checkFormDataFun, out string resultMsg)
        {
            resultMsg = string.Empty;
            if (!collection.AllKeys.Contains("appId") || string.IsNullOrEmpty(collection["appId"]))
            {
                resultMsg += "appId为空！";
            }
            //if (!collection.AllKeys.Contains("logo") || string.IsNullOrEmpty(collection["logo"]))
            //{
            //    resultMsg += "产品logo为空！";
            //}
            //验证表单必填字段信息
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
            if (!collection.AllKeys.Contains("productTerm") || string.IsNullOrEmpty(collection["productTerm"]))
            {
                resultMsg += "请选择申请期限！";
            }
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
            if (!collection.AllKeys.Contains("platform") || string.IsNullOrEmpty(collection["platform"]))
            {
                resultMsg += "合作渠道不能为空！";
            }
            if (checkFormDataFun != null)
            {
                resultMsg += checkFormDataFun(collection);
            }

            //将collection转为dictionary
            var dic = collection.AllKeys.ToDictionary(key => key, key => collection[key]);

            return dic;
        }

        /// <summary>
        /// 各产品共用方法，设置基本的ViewData
        /// </summary>
        /// <param name="appMainEntity"></param>
        /// <param name="operation">操作（1-申请，2-编辑，3-只读）</param>
        /// <param name="setViewDataFun">不同产品的部分在此方法中设置，若没有则为null</param>
        private void SetViewData(APP_MAIN appMainEntity, ENUM_FormOperation operation, Action<APP_MAIN, ENUM_FormOperation> setViewDataFun)
        {
            //客户类型
            ViewBag.CustomerType = GetCustomerType();
            //借款用途
            ViewBag.LoanPurpose = GetLoanPurpose();
            //期数
            ViewBag.TermList = new List<int>();
            //还款方式
            ViewBag.RePayTypeList = new List<DataType>();
            //产品列表
            ViewBag.ProductList = new List<QFPProduct>();

            if (appMainEntity != null)
            {
                ViewData["appId"] = appMainEntity.ID;
                ViewData["logo"] = appMainEntity.LOGO;
                //申请号
                ViewData["appCode"] = appMainEntity.APP_CODE;
                //业务品种
                ViewData["proCode"] = appMainEntity.PRODUCT_CODE;
                ViewData["proName"] = appMainEntity.PRODUCT_NAME;
                //客户类型
                ViewData["customerKind"] = appMainEntity.CUSTOMERTYPE;

                if (!string.IsNullOrEmpty(appMainEntity.APPLY_CITY_CODE))
                {
                    //城市value格式为 "[AREA_CODE],[CITY_CODE]"
                    ViewData["edit_cityCode"] = appMainEntity.APPLY_AREA_CODE + "," + appMainEntity.APPLY_CITY_CODE;
                    var appCity = AppCityService.Find(c => c.CITY_CODE == appMainEntity.APPLY_CITY_CODE).FirstOrDefault();
                    //获取当前产品的展期配置，用于筛选展期产品
                    var extendConfig = ExtendConfigService.FirstOrDefault(e => e.PRODUCT_CODE == appMainEntity.PRODUCT_CODE
                        && e.CITY_CODE == appMainEntity.APPLY_CITY_CODE && GlobalSetting.APPExtendConfig_Extend.Contains(e.ACTION_GROUP));
                    if (appCity != null && extendConfig != null)
                    {
                        //业务开办城市
                        ViewData["edit_city"] = appCity.CITY_NAME;
                        //展期配置中所配置的展期产品
                        List<string> pros = extendConfig.TARGET_PRODUCT_CODE.Split(',').ToList();
                        var tempList = ProductInfoService.GetProductListByLogo(PInfoInterfaceURLAccount.logo.ToString(), appMainEntity.LOGO)
                            .Select(p => p.pProduct);
                        List<QFPProduct> productList = tempList.Where(item => pros.Contains(item.productCode)).ToList();
                        //产品列表
                        ViewBag.ProductList = productList;
                        //还款方式列表
                        ViewBag.RePayTypeList = GetRepyTypeList(appMainEntity.PRODUCT_CODE);

                        if (pros.Contains(appMainEntity.PRODUCT_CODE))
                        {
                            //期数
                            ViewBag.TermList = ProductInfoService.GetQFProductTerm(appMainEntity.PRODUCT_CODE);
                        }

                    }
                }

                APP_CUSTOMER appCustomerEntity = appMainEntity.APP_CUSTOMER.FirstOrDefault();
                if (appCustomerEntity != null)
                {
                    //客户名称
                    ViewData["customerName"] = appCustomerEntity.NAME;
                    //身份证号
                    ViewData["customerCardID"] = appCustomerEntity.ID_NO;
                }

                APP_LOAN appLoanEntity = appMainEntity.APP_LOAN.FirstOrDefault();
                if (appLoanEntity != null)
                {
                    //合同金额
                    ViewData["contractAmt"] = appLoanEntity.LOAN_AMT_OF_CONTRACT;
                    //还款方式Code
                    ViewData["repayTypeCode"] = appLoanEntity.PAYTYPE;
                    //期数
                    ViewData["terms"] = appLoanEntity.TERMS;
                    //申请利率
                    ViewData["rate"] = appLoanEntity.RATE;
                    //罚息利率
                    ViewData["interestRatio"] = appLoanEntity.DEFAULT_INTEREST_RATIO;
                    //申请金额
                    ViewData["applyAmt"] = appLoanEntity.APPLY_AMT;
                    //服务费率
                    ViewData["serviceChargeRatio"] = appLoanEntity.SERVICE_CHARGE_RATIO;
                    //服务费
                    ViewData["serviceCharge"] = appLoanEntity.SERVICE_CHARGE_AMT;
                    //咨询费率
                    ViewData["consultationChargeRatio"] = appLoanEntity.CONSULTATION_CHARGE_RATIO;
                    //咨询费
                    ViewData["consultationCharge"] = appLoanEntity.CONSULTATION_CHARGE_AMT;
                    //借款用途
                    ViewData["loanPur"] = appLoanEntity.LOAN_PURPOSE;
                    //借款用途其他
                    ViewData["memoOfLoanPurposeOther"] = appLoanEntity.MEMO_OF_LOAN_PURPOSE_OTHER;
                    //可接受月还款
                    ViewData["payAmtMonthly"] = appLoanEntity.PAY_AMT_MONTHLY_ACCEPTABLE;
                }

                APP_STAFF_ONLY appSaffOnlyEntity = appMainEntity.APP_STAFF_ONLY.FirstOrDefault();
                if (appSaffOnlyEntity != null)
                {
                    //合作渠道Code
                    ViewData["channelColde"] = appSaffOnlyEntity.CHANNEL_CODE;
                    //合作渠道
                    ViewData["channel"] = appSaffOnlyEntity.CHANNEL_NAME;
                }

                //续展期数
                int periodAmt;
                //剩余可展期数
                var periodRemain = ExtendAppService.GetExtendPeriod(appMainEntity, out periodAmt);
                ViewData["periodRemain"] = periodRemain;
                ViewData["periodAmt"] = periodAmt;

                if (setViewDataFun != null)
                {
                    setViewDataFun(appMainEntity, operation);
                }
            }
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
        /// <returns></returns>
        private List<APP_CITY> GetCityList(string menuGroup)
        {
            var list = AppCityService
                .FilterByMenuGroup(menuGroup)
                .ToList();

            foreach (var item in list)
            {
                item.CITY_CODE = item.AREA_CODE + ',' + item.CITY_CODE;
            }

            return list;
        }

        /// <summary>
        /// 还款方式
        /// </summary>
        /// <param name="productCode">产品Code</param>
        /// <returns></returns>
        private List<DataType> GetRepyTypeList(string productCode)
        {
            return ProductInfoService.GetRepayTypeByProductCode(productCode);
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

        private List<CR_DATA_DIC> GetPlantformHouse()
        {
            List<CR_DATA_DIC> plantformList = new List<CR_DATA_DIC>();

            plantformList = DicService.GetDICByParentCode("CHANNELCOOPERATION_HOUSE").ToList();

            return plantformList;
        }

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

        /// <summary>
        /// 车贷产品logos
        /// </summary>
        /// <returns></returns>
        private string GetCheDaiLogos()
        {
            var strs = Global.GlobalSetting.CheDaiLogos;
            return string.Join(",", strs);
        }

        #endregion

        //public ActionResult GetProductAndRepayTypeExtend(string cityCode)
        //{
        //    cityCode = cityCode.Trim();
        //    //string areaId = "323ea269-5aea-4f60-b6d8-31d3215e208d"; //区域编号
        //    var currentCity = AppCityService.Find(c => c.CITY_CODE == cityCode).FirstOrDefault();

        //    if (currentCity != null)
        //    {
        //        List<string> pros = currentCity.PRODUCT_CODE.Split(',').ToList();

        //        var tempList = (ProductInfoService.GetProductList(PInfoInterfaceURLAccount.orgId.ToString(), currentCity.COMPANY_CODE));
        //        //.TakeWhile(c => pros.Contains(c.productCode.Trim()))  //此处不能用TakeWhile，会导致数据查询不全，详见TakeWhile用法

        //        List<QFPProduct> productList = tempList.Where(item => pros.Contains(item.productCode)).ToList();

        //        var proList = productList.Select(p => new
        //        {
        //            p.productCode,
        //            p.productName
        //        });

        //        var repayType = ProductInfoService.GetRePayTypeList(currentCity.COMPANY_CODE).FirstOrDefault();

        //        if (!proList.Any() && repayType == null)
        //        {
        //            return Content(string.Empty);
        //        }

        //        return Json(new { proList, repayType }, JsonRequestBehavior.AllowGet);
        //    }

        //    return Content(string.Empty);
        //}
    }
}