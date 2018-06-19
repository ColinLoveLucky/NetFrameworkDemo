/*********************
 * 作者：刘成帅
 * 时间：2014/9/9
 * 功能：用于从数据库中读取动态表单的一些方法
**********************/

using System.Text;
using System.Web.UI.WebControls.Expressions;
using Microsoft.Practices.Unity;
using QK.QAPP.Entity;
using QK.QAPP.Global;
using QK.QAPP.Infrastructure;
using QK.QAPP.Infrastructure.Cache;
using QK.QAPP.Infrastructure.Mvc;
using QK.QAPP.IServices;
using QK.QAPP.MvcScaffold.DForm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using QK.QAPP.Infrastructure.Log4Net;
using QK.QAPP.Entity.QbEntity;

namespace QK.QAPP.SalesCenter.Controllers
{

    public class SystemConfigController : Controller
    {
        #region 属性

        [Dependency]
        public IQFUserService UserService { get; set; }
        [Dependency]
        public IAPP_CITYSERVICE CityService { get; set; }
        [Dependency]
        public IQFProductInfoService ProductInfoService { get; set; }
        [Dependency]
        public IAPP_APPLY_SEQUENCESERVICE ApplySeqService { get; set; }
        [Dependency]
        public IAPP_GLOBALCONFIGSERVICE GlobalConfigService { get; set; }
        [Dependency]
        public IAPP_EXTEND_CONFIGSERVICE ExtendConfigService { get; set; }
        [Dependency]
        public IAPP_FORBIDEN_AREASERVICE ForbidenService { get; set; }
        [Dependency]
        public ICacheProvider CacheService { get; set; }
        [Dependency]
        public IAPP_CARPRICESERVICE CarPriceService { get; set; }

        [Dependency]
        public IQuotaManageService quotaManageService { get; set; }

        [Dependency]
        public IQBBidJobConfigService IBidJobConfigService { get; set; }

        [Dependency]
        public IQBBidJobAmtService IQBBidJobAmtService { get; set; }

        [Dependency]
        public IQuotaBidDicConfigService IQuotaBidDicConfigService { get; set; }

        [Dependency]
        public IQBAutoJobLogService IQBAutoJobLogService { get; set; }

        [Dependency]
        public IAPP_CITY_PRODUCTSERVICE CityProductService { get; set; }
        #endregion
        public ActionResult DFormCopy()
        {
            return View();
        }

        [HttpPost]
        [LogicalActionFilterAttribute(ActionSummary = "动态表单复制")]
        public string DFormCopy(string productCode, string productVersion, string newCode, string newVersion)
        {
            var dformHandler = new DFormHandler();
            if (dformHandler.DFormCopy(productCode, productVersion, newCode, newVersion))
            {
                return "表单复制成功，新表单CODE为：" + newCode + "；VERSION为：" + newVersion;
            }

            return "出现未知错误！";
        }
        #region 服务器缓存清理


        [LogicalActionFilterAttribute(ActionSummary = "缓存清理")]
        public ActionResult ClearCache()
        {
            ViewData["GetALLKey"] = CacheService.GetALLKey();
            return View();
        }
        [HttpPost]
        [LogicalActionFilterAttribute(ActionSummary = "缓存清理")]
        public string ClearCache(string groupName)
        {
            //日志
            Infrastructure.Log4Net.LogWriter.Biz("缓存清理", groupName);

            return CacheService.RemoveStartWith(groupName) + "";
        }

        #endregion

        #region 动态表单读取
        /// <summary>
        /// 获取FormBuilder列表（只获取不同的Code）
        /// </summary>
        /// <returns></returns>
        [LogicalActionFilterAttribute(ActionSummary = "动态表单Code")]
        public JsonResult GetDFormCodes()
        {
            var formBuilderService = Ioc.GetService<IAPP_DFORM_FORMBUILDERSERVICE>();
            var formBuilderList = formBuilderService.GetAll().Select(b => b.CODE).Distinct();

            return Json(formBuilderList, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取某产品的版本列表
        /// </summary>
        /// <param name="code"></param> 
        /// <returns></returns>
        [LogicalActionFilterAttribute(ActionSummary = "获取某产品的版本列表")]
        public JsonResult GetDFormVersions(string code)
        {
            var formBuilderService = Ioc.GetService<IAPP_DFORM_FORMBUILDERSERVICE>();
            var versionList = formBuilderService.Find(b => b.CODE == code).Select(b => b.VERSION);
            var dicVers = Global.GlobalSetting.DFormVersions;

            return Json(new { versionList, dicVers }, JsonRequestBehavior.AllowGet);
        }
        #endregion



        #region 动态表单列表
        /// <summary>
        /// 获取FormBuilder列表（全）
        /// </summary>
        /// <returns></returns>
        [LogicalActionFilterAttribute(ActionSummary = "获取动态表单主表列表（全）")]
        public JsonResult GetDFormBuilderList()
        {
            var formBuilderService = Ioc.GetService<IAPP_DFORM_FORMBUILDERSERVICE>();
            var formBuilderList = formBuilderService.GetAll().Select(b => new
            {
                b.ID,
                b.NAME,
                b.CODE,
                b.VERSION
            }).OrderBy(b => b.CODE);

            var dicVers = Global.GlobalSetting.DFormVersions;

            return Json(new { formBuilderList, dicVers }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取DFormInfo列表
        /// </summary>
        /// <returns></returns>
        [LogicalActionFilterAttribute(ActionSummary = "获取动态表单子表单列表")]
        public JsonResult GetDFormInfoList(long FB_ID)
        {
            var formInfoService = Ioc.GetService<IAPP_DFORM_FORMINFOSERVICE>();

            var dFormInfoList = formInfoService.Find(i => i.FB_ID == FB_ID).Select(i => new
            {
                i.ID,
                i.FB_ID,
                i.NAME,
                i.READONLY,
                i.ACTION_EDIT,
                i.ACTION_READ,
                i.Sort,
                i.ADDMORE,
                i.ADDMOREKEYWORD
            }).OrderBy(i => i.Sort);

            //if (dFormInfoList.Any())
            //{
            //    return Json(dFormInfoList, JsonRequestBehavior.AllowGet);
            //}
            //return Json(string.Empty);

            return dFormInfoList.Any() ? Json(dFormInfoList, JsonRequestBehavior.AllowGet) : Json(string.Empty);
        }

        /// <summary>
        /// 获取DFormField列表
        /// </summary>
        /// <param name="formInfoID"></param>
        /// <returns></returns>
        [LogicalActionFilterAttribute(ActionSummary = "获取动态表字段列表")]
        public JsonResult GetDFormFieldList(long formInfoID)
        {
            var formFieldService = Ioc.GetService<IAPP_DFORM_FORMFIELDSERVICE>();

            var fieldList = formFieldService.Find(f => f.FORMINFO_ID == formInfoID)
                .OrderBy(f => f.FIELD_GROUP)
                .ThenBy(f => f.FIELD_SORT)
                .Select(f => new
            {
                f.ID,
                f.FORMINFO_ID,
                f.FIELD_KEY,
                f.FIELD_DISPLAYNAME,
                f.FIELD_TYPE,
                f.FIELD_ROWSPAN,
                f.MAPPER_TABLE,
                f.MAPPER_TABLEFIELD,
                f.FIELD_GROUP,
                f.FIELD_SORT,
                f.FIELD_REQUIRED
            });

            //if (fieldList != null)
            //{
            //    return Json(fieldList, JsonRequestBehavior.AllowGet);
            //}
            //else
            //{
            //    return Json(string.Empty);
            //}

            return fieldList.Any() ? Json(fieldList, JsonRequestBehavior.AllowGet) : Json(string.Empty);
        }

        #endregion

        #region 动态表单添加
        [LogicalActionFilterAttribute(ActionSummary = "添加字段")]
        public JsonResult AddDFormField(long formInfoID)
        {
            var formFieldService = Ioc.GetService<IAPP_DFORM_FORMFIELDSERVICE>();

            var formFieldEntity = new Entity.APP_DFORM_FORMFIELD { FORMINFO_ID = formInfoID };

            formFieldService.Add(formFieldEntity);

            formFieldService.UnitOfWork.SaveChanges();

            //日志
            Infrastructure.Log4Net.LogWriter.Biz("动态表单字段添加", formFieldEntity);

            return Json(new { formFieldEntity.ID, formFieldEntity.FORMINFO_ID }, JsonRequestBehavior.AllowGet);
        }
        #endregion


        #region 动态表单字段的删除
        [LogicalActionFilterAttribute(ActionSummary = "动态表单字段删除Ajax")]
        public string DeleteDFormField(long id)
        {
            var formFieldService = Ioc.GetService<IAPP_DFORM_FORMFIELDSERVICE>();
            var formAttrService = Ioc.GetService<IAPP_DFORM_FIELDATTRIBUTESERVICE>();

            var fieldEntity = formFieldService.Find(f => f.ID == id).FirstOrDefault();
            var attrList = formAttrService.Find(a => a.FORMFIELD_ID == id);

            formAttrService.DeleteMultiple(attrList.ToList());
            formFieldService.Delete(fieldEntity);

            formFieldService.UnitOfWork.SaveChanges();

            //日志
            Infrastructure.Log4Net.LogWriter.Biz("动态表单字段（以及相关的属性）删除", fieldEntity);

            return "删除成功！";
        }

        #endregion

        #region 动态表单编辑
        [LogicalActionFilterAttribute(ActionSummary = "动态表单字段编辑页面")]
        public ActionResult DFormFormEdit()
        {
            return View();
        }
        [LogicalActionFilterAttribute(ActionSummary = "动态表单字段编辑页面Ajax")]
        public ActionResult DFormFieldEdit(long id)
        {
            var service = Ioc.GetService<DFormHandler>();
            ViewData["FormList"] = service.GetAllSubForm();
            ViewData["TypeList"] = service.GetALLFieldType();
            ViewData["EntityList"] = service.GetALLEntityType();
            return View();
        }
        /// <summary>
        /// 获取
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        [LogicalActionFilterAttribute(ActionSummary = "获取字段列表Ajax")]
        [HttpGet]
        public JsonResult DFormFiledList(string tableName)
        {
            var service = Ioc.GetService<DFormHandler>();
            return Json(service.GetFieldName(tableName), JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 获取属性名
        /// </summary>
        /// <param name="typeName"></param>
        /// <returns></returns>
        [LogicalActionFilterAttribute(ActionSummary = "获取动态表单字段Ajax")]
        [HttpGet]
        public JsonResult GetTypeFiledList(string typeName)
        {
            var service = Ioc.GetService<DFormHandler>();
            return Json(service.GetFieldAttr(typeName), JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        [LogicalActionFilterAttribute(ActionSummary = "获取动态表单字段属性Ajax")]
        public ContentResult GetFieldEntity(long fieldID)
        {
            var service = Ioc.GetService<DFormHandler>();
            var entity = service.GetFieldEntity(fieldID);
            return Content(entity);
        }
        [HttpGet]
        [LogicalActionFilterAttribute(ActionSummary = "获取表单字段实体Ajax")]
        public JsonResult GetFieldTypeEntity(long fieldID)
        {
            var service = Ioc.GetService<DFormHandler>();
            var entity = service.GetFieldTypeAttr(fieldID);
            return Json(entity, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [LogicalActionFilterAttribute(ActionSummary = "更新动态表单字段基本属性Ajax")]
        public ContentResult UpdateFieldEntity(APP_DFORM_FORMFIELD entity)
        {
            var service = Ioc.GetService<DFormHandler>();

            return Content(service.SaveFieldBaseInfo(entity) + "");
        }
        [HttpPost]
        [LogicalActionFilterAttribute(ActionSummary = "更新动态表单字段特性属性Ajax")]
        public ContentResult UpdateFieldAttrEntity(FormCollection form)
        {
            var service = Ioc.GetService<DFormHandler>();

            return Content(service.SaveFieldAttrInfo(form) + "");
        }
        #endregion

        #region DFormBuilder的删除
        [LogicalActionFilterAttribute(ActionSummary = "动态表单删除Ajax")]
        public string DeleteDFormBuilder(long id)
        {
            var formBuilderService = Ioc.GetService<IAPP_DFORM_FORMBUILDERSERVICE>();
            var formInfoService = Ioc.GetService<IAPP_DFORM_FORMINFOSERVICE>();
            var formFieldService = Ioc.GetService<IAPP_DFORM_FORMFIELDSERVICE>();
            var formAttrService = Ioc.GetService<IAPP_DFORM_FIELDATTRIBUTESERVICE>();


            //查询
            var builder = formBuilderService.Find(b => b.ID == id);

            var infoList = formInfoService.Find(i => i.FB_ID == id);
            var infoIdList = infoList.Select(i => i.ID).ToArray();

            var fieldList = formFieldService.Find(f => f.FORMINFO_ID.HasValue && infoIdList.Contains(f.FORMINFO_ID.Value));
            var fieldIdList = fieldList.Select(f => f.ID).ToArray();

            var attrList = formAttrService.Find(a => a.FORMFIELD_ID.HasValue && fieldIdList.Contains(a.FORMFIELD_ID.Value));

            //删除
            formAttrService.DeleteMultiple(attrList.ToList());
            formFieldService.DeleteMultiple(fieldList.ToList());
            formInfoService.DeleteMultiple(infoList.ToList());
            formBuilderService.Delete(builder.FirstOrDefault());

            //formAttrService.UnitOfWork.SaveChanges();
            //formFieldService.UnitOfWork.SaveChanges();
            //formInfoService.UnitOfWork.SaveChanges();
            formBuilderService.UnitOfWork.SaveChanges();

            //日志
            Infrastructure.Log4Net.LogWriter.Biz("删除动态表单", builder.FirstOrDefault());

            return "删除成功！";
        }

        #endregion

        #region DFormBuilder编辑

        /// <summary>
        /// 编辑动态表单Builder
        /// </summary>
        /// <param name="id">DFormBuilder的ID</param>
        /// <returns></returns>
        [LogicalActionFilterAttribute(ActionSummary = "动态表单DFormBuilder编辑页面")]
        public ActionResult DFormBuilderEdit(long id)
        {
            var formBuilderService = Ioc.GetService<IAPP_DFORM_FORMBUILDERSERVICE>();

            var builder = formBuilderService.Find(b => b.ID == id).FirstOrDefault();

            if (builder != null)
            {
                ViewBag.DFormBuilder = builder;
            }

            return View();
        }

        [LogicalActionFilterAttribute(ActionSummary = "动态表单DFormBuilder编辑Ajax")]
        public ContentResult UpdateBuilderEntity(APP_DFORM_FORMBUILDER entity)
        {
            var formBuilderService = Ioc.GetService<IAPP_DFORM_FORMBUILDERSERVICE>();
            var formHandler = Ioc.GetService<DFormHandler>();

            var builder = formBuilderService.Find(b => b.ID == entity.ID).FirstOrDefault();

            if (builder != null)
            {
                builder.NAME = entity.NAME;
                builder.VERSION = entity.VERSION;
                builder.CODE = entity.CODE;
                builder.CHANGED_USER = UserService.GetCurrentUser().Account;
                builder.CHANGED_TIME = DateTime.Now;

                return Content(formHandler.SaveBuilderInfo(builder));
            }
            //日志
            Infrastructure.Log4Net.LogWriter.Biz("编辑DFormBuilder出错，没有找到数据！", entity);
            return Content("编辑表单出错！");
        }

        #endregion

        #region DFormInfo添加

        /// <summary>
        /// 添加动态表单-子表单
        /// </summary>
        /// <param name="formBuilderID">DFormBuilder的ID</param>
        /// <returns></returns>
        public JsonResult AddDFormInfo(long formBuilderID)
        {
            var formInfoService = Ioc.GetService<IAPP_DFORM_FORMINFOSERVICE>();

            var formInfoEntity = new APP_DFORM_FORMINFO
            {
                FB_ID = formBuilderID,
                CREATED_USER = UserService.GetCurrentUser().Code,
                CREATED_TIME = DateTime.Now
            };

            formInfoService.Add(formInfoEntity);
            formInfoService.UnitOfWork.SaveChanges();

            //日志
            Infrastructure.Log4Net.LogWriter.Biz("动态表单子表单添加", formInfoEntity);

            return Json(new { formInfoEntity.ID, formInfoEntity.FB_ID }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region DFormInfo编辑

        /// <summary>
        /// 编辑动态表单-子表单页面
        /// </summary>
        /// <param name="id">子表单ID</param>
        /// <returns></returns>
        public ActionResult DFormInfoEdit(long id)
        {
            return View();
        }

        /// <summary>
        /// 获取动态表单-子表单实体信息
        /// </summary>
        /// <param name="infoId">子表单ID</param>
        /// <returns></returns>
        [HttpGet]
        public ContentResult GetFormInfoEntity(long infoId)
        {
            var service = Ioc.GetService<DFormHandler>();
            var entity = service.GetFormInfoEntity(infoId);
            return Content(entity);
        }

        /// <summary>
        /// 保存子表单
        /// </summary>
        /// <param name="entity">子表单对象</param>
        /// <returns></returns>
        public ContentResult UpdateInfoEntity(APP_DFORM_FORMINFO entity)
        {
            var formInfoService = Ioc.GetService<IAPP_DFORM_FORMINFOSERVICE>();
            var formHandler = Ioc.GetService<DFormHandler>();

            var infoEntity = formInfoService.Find(i => i.ID == entity.ID).FirstOrDefault();
            if (infoEntity != null)
            {
                infoEntity.NAME = entity.NAME;
                infoEntity.VERSION = entity.VERSION;
                infoEntity.READONLY = entity.READONLY;
                infoEntity.ACTION_EDIT = entity.ACTION_EDIT;
                infoEntity.ACTION_READ = entity.ACTION_READ;
                infoEntity.CHANGED_USER = UserService.GetCurrentUser().Account;
                infoEntity.CHANGED_TIME = DateTime.Now;
                infoEntity.Sort = entity.Sort;
                infoEntity.ADDMORE = entity.ADDMORE;
                infoEntity.ADDMOREKEYWORD = entity.ADDMOREKEYWORD;

                return Content(formHandler.SaveFormInfo(infoEntity));
            }
            //日志，保存出错
            Infrastructure.Log4Net.LogWriter.Biz("编辑DFormInfo出错，没有找到数据！", entity);
            return Content("编辑表单（DFomrInfo）出错！");

        }

        #endregion

        #region DFromInfo删除

        /// <summary>
        /// 删除动态表单-子表单以及下面的字段和属性
        /// </summary>
        /// <param name="id">子表单ID</param>
        /// <returns></returns>
        public string DeleteDFormInfo(long id)
        {
            var forminfoService = Ioc.GetService<IAPP_DFORM_FORMINFOSERVICE>();
            var formFieldService = Ioc.GetService<IAPP_DFORM_FORMFIELDSERVICE>();
            var formAttrService = Ioc.GetService<IAPP_DFORM_FIELDATTRIBUTESERVICE>();

            //查询子表单
            var infoEntity = forminfoService.Find(i => i.ID == id);
            //查询子表单下的字段
            var fieldList = formFieldService.Find(f => f.FORMINFO_ID == id);
            var fieldIdList = fieldList.Select(f => f.ID).ToArray();
            //查询字段下的属性
            var attrList = formAttrService.Find(a => a.FORMFIELD_ID.HasValue && fieldIdList.Contains(a.FORMFIELD_ID.Value));

            //进行删除
            formAttrService.DeleteMultiple(attrList.ToList());
            formFieldService.DeleteMultiple(fieldList.ToList());
            forminfoService.Delete(infoEntity.FirstOrDefault());

            forminfoService.UnitOfWork.SaveChanges();

            //日志
            Infrastructure.Log4Net.LogWriter.Biz("删除动态表单子表单", id + String.Empty, infoEntity.FirstOrDefault());

            return "删除成功！";
        }

        #endregion

        #region 强制登出所有用户‘
        public ActionResult UserManager()
        {
            return View();
        }
        #endregion

        #region 消息推送

        public ActionResult PushMessage()
        {
            return View();
        }

        [HttpPost]

        public void PushMessageAjax(string user, string content, bool allUser)
        {
            string category = allUser ? "SYSMSG2ALL" : "SYSMSG2SOMEBODY";
            char[] c = { ';' };
            string[] users = user.Split(c, StringSplitOptions.RemoveEmptyEntries);
            if (!allUser)
            {
                for (int i = 0; i < users.Length; i++)
                {
                    QAPP.SalesCenter.Hubs.PushMessage.PushToUser(users[i], Server.UrlDecode(content), category);
                }

                //日志
                Infrastructure.Log4Net.LogWriter.Biz("消息推送", category, new { users, content });
            }
            else
            {
                QAPP.SalesCenter.Hubs.PushMessage.PushToUser(user, Server.UrlDecode(content), category);

                //日志
                Infrastructure.Log4Net.LogWriter.Biz("消息推送", category, "消息内容：" + content);
            }

        }
        #endregion

        #region 在线用户
        public ActionResult OnlineUser()
        {
            return View();
        }

        public JsonResult GetOnlinUser()
        {
            //System.Threading.Thread.Sleep(2000);
            OnlineUserSearchPara para = new OnlineUserSearchPara
            {
                //分页、排序参数
                PageIndex = Request["page"].ToInt(),
                PageSize = int.Parse(Request["rows"].ToString()),
                Sort =
                    new Dictionary<string, string> { { Request["sidx"] ?? string.Empty, Request["sord"] ?? string.Empty } },
                //搜索关键字
                UserName = HttpUtility.UrlDecode(Request["userName"] + ""),
                UserIp = HttpUtility.UrlDecode(Request["userIp"] + ""),
                UserBrowser = HttpUtility.UrlDecode(Request["userBrowser"] + ""),
                UserBrowserVersion = HttpUtility.UrlDecode(Request["userBrowserVersion"] + ""),
                MachineName = HttpUtility.UrlDecode(Request["machineName"] + "")
            };
            OnlineUserViewField ret = UserService.GetOnlineUserList(para);
            return Json(ret, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 城市及产品配置

        /// <summary>
        /// 展示城市及产品配置页面
        /// </summary>
        /// <returns>ViewResult</returns>
        [LogicalActionFilter(ActionSummary = "系统配置：城市及产品配置页面")]
        public ActionResult CityProductConfig()
        {
            ViewData["UsingPlatformForCityProduct"] = Global.GlobalSetting.UsingPlatformForCityProduct;
            return View();
        }

        /// <summary>
        /// 获取城市列表
        /// </summary>
        /// <returns>JSON数据</returns>
        [LogicalActionFilter(ActionSummary = "系统配置：获取城市列表")]
        public JsonResult GetCityList(string platform)
        {
            /*var cityList = CityService.GetAll()
                .OrderBy(c => c.ID)
                .ToList();--V2*/
            var cityList = CityService.FilterByPlatform(platform).OrderBy(c => c.ID).ToList();
            foreach (var item in cityList)
            {
                if (!string.IsNullOrEmpty(item.PRODUCT_CODE))
                    item.PRODUCT_CODE = ProductCodeToName(item.PRODUCT_CODE);

                if (!string.IsNullOrEmpty(item.COMPANY_CODE))
                    item.COMPANY_CODE = CompanyCodeToName(item.COMPANY_CODE);

                if (!string.IsNullOrEmpty(item.CITY_CODE))
                {
                    string[] c = item.CITY_CODE.Split('_');
                    item.CITY_CODE = c[c.Length - 1];
                }
            }
            return Json(cityList, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 将产品编号转为相应的名称便于显示
        /// </summary>
        /// <param name="codes">产品编号，中间以逗号隔开</param>
        /// <returns></returns>
        private string ProductCodeToName(string codes)
        {
            List<string> resultList = new List<string>();
            string[] strs = codes.Split(',');
            var dic = GetAllProductDic();
            if (dic.Count > 0)
            {
                resultList.AddRange(strs.Select(item => dic.ContainsKey(item) ? dic[item] : "未知产品代码"));
            }
            return resultList.JoinString(",");
        }

        /// <summary>
        /// 将区域编号转为名称便于显示
        /// </summary>
        /// <param name="codes">区域编号</param>
        /// <returns></returns>
        private string CompanyCodeToName(string codes)
        {
            var result = string.Empty;
            var orgList = CityService.GetOrgRoleList();
            var temp = orgList.Find(o => o.OBJECTID == codes);
            if (temp != null)
            {
                result = temp.COMPANYNAME + "->" + temp.OBJECTNAME;
            }
            return result;
        }

        /// <summary>
        /// 获取所有产品，Dictionary(string,string)
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, string> GetAllProductDic()
        {
            return CacheService.GetFromCacheOrProxy("QAPP_GetAllProductDic", GetProductDic);
        }

        /// <summary>
        /// 获取所有logo，Dictionary(string,string)
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, string> GetAllLogoDic()
        {
            return CacheService.GetFromCacheOrProxy("QAPP_GetAllLogoDic", GetLogoDic);
        }

        public Dictionary<string, string> GetProductDic()
        {
            //var cityList = CityService.GetAll();
            //foreach (var item in cityList)
            //{
            //    var tempList = ProductInfoService.GetProductList(PInfoInterfaceURLAccount.orgId.ToString(), item.COMPANY_CODE); --V2
            //List<string> comCodeList = CityService.GetAll().Select<APP_CITY, string>(c => c.COMPANY_CODE).Distinct().ToList();
            //foreach (var item in comCodeList)
            //{
            //    var tempList = ProductInfoService.GetProductList(PInfoInterfaceURLAccount.orgId.ToString(), item);
            //    foreach (var p in tempList)
            //    {
            //        if (!productDic.ContainsKey(p.productCode))
            //        {
            //            productDic.Add(p.productCode, p.productName);
            //        }
            //    }
            //}
            var tempList = ProductInfoService.GetProductAllList(PInfoInterfaceURLAccount.productList.ToString())
                .Select(c => c.pProduct)
                .ToDictionary(item => item.productCode, item => item.productName);
            return tempList;
        }


        public Dictionary<string, string> GetLogoDic()
        {
            //Dictionary<string, string> logoDic = new Dictionary<string, string>();
            //var cityList = CityService.GetAll();
            //foreach (var item in cityList)
            //{
            //    var tempList = ProductInfoService.GetLogoList(PInfoInterfaceURLAccount.orgId.ToString(), item.COMPANY_CODE);--V2
            //List<string> logoList = CityService.GetAll().Select<APP_CITY, string>(c => c.COMPANY_CODE).Distinct().ToList();
            //foreach (var item in logoList)
            //{
            //    var tempList = ProductInfoService.GetLogoList(PInfoInterfaceURLAccount.orgId.ToString(), item);
            //    foreach (var l in tempList)
            //    {
            //        if (!logoDic.ContainsKey(l.logo))
            //        {
            //            logoDic.Add(l.logo, l.logoName);
            //        }
            //    }
            //}

            var logoDic = ProductInfoService.GetProductAllList(PInfoInterfaceURLAccount.productList.ToString())
                .Select(c => c.pLogo)
                .ToDictionary(item => item.logo, item => item.logoName);

            return logoDic;
        }

        /// <summary>
        /// 添加/编辑城市
        /// </summary>
        /// <param name="id"></param>
        /// <param name="platform">平台</param>
        /// <returns></returns>
        public ActionResult CityEdit(string platform, long id = -1)
        {
            if (id < 0)
            {
                //添加
                ViewData["operation"] = "add";
            }
            else
            {
                //编辑
                ViewData["operation"] = "edit";
            }
            ViewData["UsingPlatformForCityProduct"] = Global.GlobalSetting.UsingPlatformForCityProduct;
            ViewData["CurrentPlatform"] = platform;
            ViewBag.orgRoleList = CityService.GetOrgRoleList();
            ViewBag.productDic = GetAllProductDic();
            return View();
        }

        /// <summary>
        /// 获取城市信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public JsonResult GetCityEntity(long id)
        {
            var editCity = CityService.Find(c => c.ID == id).FirstOrDefault();
            if (editCity != null && !string.IsNullOrEmpty(editCity.CITY_CODE))
            {
                string[] c = editCity.CITY_CODE.Split('_');
                editCity.CITY_CODE = c[c.Length - 1];
            }
            return Json(editCity, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 添加/编辑 保存
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="PRODUCT_CODE"></param>
        /// <returns></returns>
        public ContentResult UpdateOrAddCity(APP_CITY entity, string[] PRODUCT_CODE, string OLD_CITY_CODE)
        {
            string msg = string.Empty;

            if (PRODUCT_CODE != null && PRODUCT_CODE.Length > 0)
                entity.PRODUCT_CODE = PRODUCT_CODE.JoinString(",");

            //验证
            if (string.IsNullOrEmpty(entity.CITY_NAME))
            {
                msg += "【名称】不能为空！";
            }
            else if (entity.CITY_NAME.Length > 50)
            {
                msg += "【名称】长度过长！";
            }
            if (string.IsNullOrEmpty(entity.COMPANY_CODE))
            {
                msg += "【区域】不能为空！";
            }
            if (string.IsNullOrEmpty(entity.AREA_CODE))
            {
                msg += "【区号】不能为空！";
            }
            entity.AREA_CODE = entity.AREA_CODE.Trim();
            if (string.IsNullOrEmpty(entity.CITY_CODE))
            {
                msg += "【城市编码】不能为空！";
            }
            entity.CITY_CODE = entity.CITY_CODE.Trim();
            if (entity.PLATFORM != "QAPP")
            {
                entity.CITY_CODE = string.Format("{0}_{1}", entity.PLATFORM, entity.CITY_CODE);
            }
            /*else
            {
                Regex regex = new Regex("^0[0-9]{2,3}$");
                if (!regex.IsMatch(entity.CITY_CODE))
                {
                    msg += "【区号】格式不正确！";
                }
            }
            var count = CityService.Count(c => c.CITY_CODE == entity.CITY_CODE || c.ID == entity.ID);--V2*/
            var count = CityService.FilterByPlatform(entity.PLATFORM).Count(c => c.CITY_CODE == entity.CITY_CODE || c.ID == entity.ID);
            if ((entity.ID < 0 && count > 0) || (entity.ID >= 0 && count > 1))
            {
                msg += "已存在相同【城市编码】的城市，请核实后再填写！";
            }

            if (!string.IsNullOrEmpty(msg))
            {
                return Content(msg);
            }

            if (entity.ID < 0)
            {
                //添加
                entity.CREATE_TIME = DateTime.Now;
                entity.ID = 0;

                CityService.Add(entity);
                //日志
                Infrastructure.Log4Net.LogWriter.Biz("添加城市", entity);
            }
            else
            {
                //编辑
                var newEntity = CityService.Find(c => c.ID == entity.ID).FirstOrDefault();

                if (entity != null && newEntity != null)
                {
                    newEntity.CITY_NAME = entity.CITY_NAME;
                    newEntity.COMPANY_CODE = entity.COMPANY_CODE;
                    newEntity.AREA_CITY = entity.AREA_CITY;
                    newEntity.AREA_PROVINCE = entity.AREA_PROVINCE;
                    newEntity.ENABLE = entity.ENABLE;
                    newEntity.UPDATE_TIME = DateTime.Now;
                    newEntity.CITY_CODE = entity.CITY_CODE;
                    newEntity.AREA_CODE = entity.AREA_CODE;
                    newEntity.PRODUCT_CODE = entity.PRODUCT_CODE;

                    CityService.Update(newEntity);

                    //日志
                    Infrastructure.Log4Net.LogWriter.Biz("编辑城市信息", newEntity);
                }

                /*如果城市编码有更改，则更新申请号配置表(APP_APPLY_SEQUENCE)中的城市编码
                if (entity != null && entity.CITY_CODE != OLD_CITY_CODE)
                {
                    ApplySeqService.UpdateCitycodeByCityCode(OLD_CITY_CODE, entity.CITY_CODE);

                    //日志
                    Infrastructure.Log4Net.LogWriter.Biz("更新申请号配置表区号", new { oldCode = OLD_CITY_CODE, newCode = entity.CITY_CODE });
                }--V2*/
                //如果城市编码有更改，则更新申请号配置表(APP_APPLY_SEQUENCE)以及APP_CITY_PRODUCT中的城市编码
                if (newEntity != null && newEntity.CITY_CODE != OLD_CITY_CODE)
                {
                    //申请系统
                    if (newEntity.PLATFORM == Global.GlobalSetting.UsingPlatformForCityProduct_QAPP)
                    {
                        ApplySeqService.UpdateCityCodeByCityCode(OLD_CITY_CODE, entity.CITY_CODE);

                        CityProductService.UpdateCityCodeByCityCode(OLD_CITY_CODE, entity.CITY_CODE);

                        //日志
                        Infrastructure.Log4Net.LogWriter.Biz("更新申请号配置表城市编码", new { oldCode = OLD_CITY_CODE, newCode = entity.CITY_CODE });
                    }
                    //更改非申请系统的配置，需要更新对应系统的单号申请规则
                    else
                    {
                        // TO BE UPDATED
                    }
                }
            }

            CityService.UnitOfWork.SaveChanges();



            return Content(msg);
        }

        /// <summary>
        /// 删除城市及申请号配置表的相关信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ContentResult DeleteCity(long id)
        {
            string msg = string.Empty;
            var city = CityService.Find(c => c.ID == id).FirstOrDefault();
            if (city != null)
            {
                CityProductService.DeleteByCityCode(city.CITY_CODE);

                ApplySeqService.DeleteByCityCode(city.CITY_CODE);

                CityService.Delete(city);

                CityService.UnitOfWork.SaveChanges();

                //日志
                Infrastructure.Log4Net.LogWriter.Biz("删除城市（及相关的申请号配置）", city);
            }
            else
            {
                msg += "没有找到数据！";
            }

            return Content(msg);
        }

        #endregion

        #region GLOBALCONFIG
        /// <summary>
        /// GLOBALCONFIG列表
        /// </summary>
        /// <returns></returns>
        public ActionResult GlobalConfigList()
        {
            var list = GlobalConfigService
                .Find(c => c.PLATFORM == GlobalSetting.Platform)
                .OrderBy(c => c.SORT).ToList();
            return View(list);
        }
        /// <summary>
        /// GLOBALCONFIG编辑
        /// </summary>
        /// <returns></returns>
        public ActionResult GlobalConfigEdit(string id)
        {

            var model = new APP_GLOBALCONFIG();
            if (!string.IsNullOrEmpty(id) && id != "0")
            {
                var intId = id.ToInt64();
                model = GlobalConfigService.Find(c => c.ID == intId).FirstOrDefault();
            }
            if (model != null)
            {
                return View(model);
            }
            else
            {
                model = new APP_GLOBALCONFIG();
                model.CATEGORY = "SystemConfig";
                return View(new APP_GLOBALCONFIG());
            }

        }

        /// <summary>
        /// 编辑GlobalConfig 异步
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public string EditGlobalConfig(APP_GLOBALCONFIG entity)
        {
            try
            {
                if (entity != null)
                {
                    entity.PLATFORM = GlobalSetting.Platform;
                    if (entity.ID > 0)
                    {
                        GlobalConfigService.Update(entity);
                        //日志
                        Infrastructure.Log4Net.LogWriter.Biz("修改系统配置", entity.ID + string.Empty, entity);
                    }
                    else
                    {
                        if (GlobalConfigService.Find(c => c.KEY == entity.KEY).Any())
                        {
                            return "您输入的键已经存在!";
                        }
                        GlobalConfigService.Add(entity);
                        //日志
                        Infrastructure.Log4Net.LogWriter.Biz("添加系统配置", entity.ID + string.Empty, entity);
                    }
                    GlobalConfigService.UnitOfWork.SaveChanges();
                    GlobalConfigService.ClearCacheByKey(entity.KEY);

                }
                else
                {
                    return "实体获取出错。";
                }
                return "";
            }
            catch (Exception ex)
            {

                return ex.ToString();
            }

        }

        [HttpPost]
        public string DeleteGlobalConfig(int id)
        {
            try
            {
                var intId = id.ToInt64();
                var model = GlobalConfigService.Find(c => c.ID == intId).FirstOrDefault();
                if (model != null)
                {
                    string error = GlobalConfigService.Delete(model) ? "" : "删除失败";
                    GlobalConfigService.UnitOfWork.SaveChanges();

                    //日志
                    Infrastructure.Log4Net.LogWriter.Biz("删除系统配置", model.ID + string.Empty, model);

                    return error;
                }
                return "";
            }
            catch (Exception ex)
            {

                return ex.ToString();
            }


        }

        /// <summary>
        /// 根据Key获取GlobalSetting中的值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetGlobalSettingByKey(string key)
        {
            object resultObj = new object();
            if (key == null)
                key = String.Empty;
            var type = typeof(GlobalSetting);
            var prop = type.GetProperty(key);
            if (prop != null && prop.CanRead)
            {
                resultObj = prop.GetValue(null, null);
            }

            return Json(resultObj, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region 展期配置
        public ActionResult ExtendConfig()
        {
            return View();
        }

        public JsonResult GetProductByLoGo(string logo)
        {
            var tempList = ProductInfoService.GetProductList(PInfoInterfaceURLAccount.logo.ToString(), logo).ToList();


            return Json(tempList, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetExtendConfigList()
        {
            int pageIndex = 0, pageSize = 0;
            //分页、排序参数
            if (Convert.ToString(Request["page"]) != null)
            {
                pageIndex = int.Parse(Convert.ToString(Request["page"]));
            }
            if (Convert.ToString(Request["rows"]) != null)
            {
                pageSize = int.Parse(Convert.ToString(Request["rows"]));
            }
            string sortBy = Request["sidx"] ?? string.Empty;
            string sortDirection = Request["sord"] ?? string.Empty;
            var extendConfigList = new ViewListByPage<APP_EXTEND_CONFIG>();

            var query = ExtendConfigService.GetAll().OrderBy(p => p.ID);
            if (query != null)
            {
                if (!string.IsNullOrEmpty(sortBy) && !string.IsNullOrEmpty(sortDirection))
                {
                    extendConfigList.SetParameters(query.Sort(sortBy, sortDirection), pageIndex, pageSize);//手动排序
                }
                else
                {
                    extendConfigList.SetParameters(query, pageIndex, pageSize);//不手动排序
                }
                foreach (var item in extendConfigList.ViewList)
                {
                    var result = CityService.FirstOrDefault(p => p.CITY_CODE == item.CITY_CODE);
                    item.PRODUCT_CODE = ProductCodeToName(item.PRODUCT_CODE);
                    item.CITY_CODE = result == null ? "未知城市" : result.CITY_NAME;
                }
            }
            return Json(extendConfigList, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ExtendConfigEdit(int? id)
        {
            ViewBag.OperateType = id > 0 ? "edit" : "add";
            ViewBag.ActionGroup = GlobalSetting.APPExtendConfig_Extend.Concat(GlobalSetting.APPExtendConfig_Circle).ToList();
            ViewBag.ProductInfo = GetAllProductDic();
            ViewBag.City = CityService.FilterByPlatform(GlobalSetting.UsingPlatformForCityProduct_QAPP);
            return View();
        }

        public JsonResult GetExtendConfigEntiy(int id)
        {
            var extendConfigList = ExtendConfigService.Find(p => p.ID == id).FirstOrDefault();
            var usingDicVers = Global.GlobalSetting.DFormVersions;
            return Json(new { extendConfigList, usingDicVers }, JsonRequestBehavior.AllowGet);
        }

        public string AddOrUpdateExtendConfig(APP_EXTEND_CONFIG extendConfigEntity, string[] TARGET_PRODUCT_CODE)
        {
            extendConfigEntity.TARGET_PRODUCT_CODE = TARGET_PRODUCT_CODE.JoinString(",");
            StringBuilder sb = new StringBuilder("");
            if (string.IsNullOrEmpty(extendConfigEntity.ACTION_GROUP))
            {
                sb.Append("请选择操作类型!</br>");
            }
            if (string.IsNullOrEmpty(extendConfigEntity.PRODUCT_CODE))
            {
                sb.Append("请选择产品!</br>");
            }
            if (string.IsNullOrEmpty(extendConfigEntity.CITY_CODE))
            {
                sb.Append("请填写城市!</br>");
            }
            if (string.IsNullOrEmpty(extendConfigEntity.TARGET_LOGO))
            {
                sb.Append("请选择目标表单!</br>");
            }
            if (string.IsNullOrEmpty(extendConfigEntity.TARGET_PRODUCT_CODE))
            {
                sb.Append("请选择目标产品!</br>");
            }
            if (string.IsNullOrEmpty(extendConfigEntity.TARGET_DFORM_VERSION))
            {
                sb.Append("请选择目标表单版本!</br>");
            }
            if (string.IsNullOrEmpty(extendConfigEntity.PERIOD_AMOUNT_TOTAL.ToString()))
            {
                sb.Append("请输入可续展总期数!</br>");
            }
            if (string.IsNullOrEmpty(extendConfigEntity.PERIOD_AMOUNT.ToString()))
            {
                sb.Append("请输入每次续展期数!</br>");
            }

            if (sb.ToString() != "")
            {
                return sb.ToString();
            }
            if (extendConfigEntity.ID > 0)
            {
                //根据 操作action_group，原产品product_code，城市city_code三者判断是否有重复配置
                var query =
              ExtendConfigService.Count(
                  p =>
                      p.CITY_CODE == extendConfigEntity.CITY_CODE && p.ACTION_GROUP == extendConfigEntity.ACTION_GROUP &&
                      p.PRODUCT_CODE == extendConfigEntity.PRODUCT_CODE && p.ID != extendConfigEntity.ID);
                if (query > 0)
                {
                    sb.Append("已存在该条配置，请重新选填!</br>");
                    return sb.ToString();
                }
                var entity = ExtendConfigService.Find(p => p.ID == extendConfigEntity.ID).FirstOrDefault();
                if (entity != null)
                {
                    entity.ACTION_GROUP = extendConfigEntity.ACTION_GROUP;
                    entity.PRODUCT_CODE = extendConfigEntity.PRODUCT_CODE;
                    entity.CITY_CODE = extendConfigEntity.CITY_CODE;
                    entity.TARGET_PRODUCT_CODE = extendConfigEntity.TARGET_PRODUCT_CODE;
                    entity.TARGET_DFORM_VERSION = extendConfigEntity.TARGET_DFORM_VERSION;
                    entity.PERIOD_AMOUNT_TOTAL = extendConfigEntity.PERIOD_AMOUNT_TOTAL;
                    entity.PERIOD_AMOUNT = extendConfigEntity.PERIOD_AMOUNT;
                    entity.TARGET_LOGO = extendConfigEntity.TARGET_LOGO;
                    //以下字段暂定
                    entity.SETTLEMENT_TYPE = extendConfigEntity.SETTLEMENT_TYPE;
                    entity.SETTLEMENT_AMOUNT = extendConfigEntity.SETTLEMENT_AMOUNT;
                    entity.SERVICE_CHARGE_TYPE = extendConfigEntity.SERVICE_CHARGE_TYPE;
                    entity.SERVICE_CHARGE = extendConfigEntity.SERVICE_CHARGE;
                    entity.CONSULT_CHARGE_TYPE = extendConfigEntity.CONSULT_CHARGE_TYPE;
                    entity.CONSULT_CHARGE = extendConfigEntity.CONSULT_CHARGE;
                    ExtendConfigService.Update(entity);
                    Infrastructure.Log4Net.LogWriter.Biz("更新展期配置信息", entity);
                }
            }
            else
            {
                var query =
               ExtendConfigService.Count(
                   p =>
                       p.CITY_CODE == extendConfigEntity.CITY_CODE && p.ACTION_GROUP == extendConfigEntity.ACTION_GROUP &&
                       p.PRODUCT_CODE == extendConfigEntity.PRODUCT_CODE);
                if (query > 0)
                {
                    sb.Append("已存在该条配置，请重新选填!</br>");
                    return sb.ToString();
                }
                ExtendConfigService.Add(extendConfigEntity);
                Infrastructure.Log4Net.LogWriter.Biz("新增展期配置信息", extendConfigEntity);
            }
            ExtendConfigService.UnitOfWork.SaveChanges();
            return sb.ToString();
        }

        public string DeleteExtendConfig(int id)
        {
            string msg = string.Empty;
            var result = ExtendConfigService.Find(p => p.ID == id).FirstOrDefault();
            if (result != null)
            {
                ExtendConfigService.Delete(result);
                ExtendConfigService.UnitOfWork.SaveChanges();
                Infrastructure.Log4Net.LogWriter.Biz("删除展期配置信息", result);
            }
            else
            {
                msg += "数据没找到,无法删除";
            }
            return msg;
        }

        #endregion

        #region 黑名单配置
        public ActionResult ForbidenConfig()
        {
            return View();
        }

        public JsonResult GetForbidenConfigList()
        {
            int pageIndex = 0, pageSize = 0;
            //分页、排序参数
            if (Convert.ToString(Request["page"]) != null)
            {
                pageIndex = int.Parse(Convert.ToString(Request["page"]));
            }
            if (Convert.ToString(Request["rows"]) != null)
            {
                pageSize = int.Parse(Convert.ToString(Request["rows"]));
            }
            string sortBy = Request["sidx"] ?? string.Empty;
            string sortDirection = Request["sord"] ?? string.Empty;
            var forbidenconfigList = new ViewListByPage<APP_FORBIDEN_AREA>();

            var query = ForbidenService.GetAll().OrderBy(p => p.ID);
            if (query != null)
            {
                if (!string.IsNullOrEmpty(sortBy) && !string.IsNullOrEmpty(sortDirection))
                {
                    forbidenconfigList.SetParameters(query.Sort(sortBy, sortDirection), pageIndex, pageSize);//手动排序
                }
                else
                {
                    forbidenconfigList.SetParameters(query, pageIndex, pageSize);//不手动排序
                }
                foreach (var item in forbidenconfigList.ViewList)
                {
                    item.JUST_FOR_CITY = item.JUST_FOR_CITY == "0" ? "false" : "true";
                    var result = CityService.FirstOrDefault(p => p.CITY_CODE == item.CITY_CODE);
                    item.CITY_CODE = result == null ? "未知城市" : result.CITY_NAME;
                }
            }
            return Json(forbidenconfigList, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ForbidenConfigEdit(int? id)
        {
            ViewBag.OperateType = id != null ? "edit" : "add";
            ViewBag.City = CityService.FilterByPlatform(GlobalSetting.UsingPlatformForCityProduct_QAPP);
            return View();
        }

        public JsonResult ForbidenConfigEntiy(int id)
        {
            var ForbidenConfigList = ForbidenService.Find(p => p.ID == id).FirstOrDefault();
            var usingDicVers = Global.GlobalSetting.DFormVersions;
            return Json(new { ForbidenConfigList, usingDicVers }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetCompanyCode(string id)
        {
            var city = CityService.Find(e => e.CITY_CODE == id).FirstOrDefault();
            var plogo = ProductInfoService.GetLogoList(PInfoInterfaceURLAccount.orgId.ToString(), city.COMPANY_CODE).Distinct(c => c.logo).ToList();

            return Json(plogo, JsonRequestBehavior.AllowGet);
        }

        public string AddOrUpdateForbidenConfig()
        {
            APP_FORBIDEN_AREA ForbidenConfigEntity = JsonConvert.DeserializeObject<APP_FORBIDEN_AREA>(Request["st"]);
            var currentUser = UserService.GetCurrentUser();
            StringBuilder sb = new StringBuilder("");
            if (string.IsNullOrEmpty(ForbidenConfigEntity.CITY_CODE))
            {
                sb.Append("请填写城市!</br>");
            }
            if (string.IsNullOrEmpty(ForbidenConfigEntity.FORBIDEN_ID_START.ToString()))
            {
                sb.Append("请输入身份证后六位!</br>");
            }

            if (ForbidenConfigEntity.ID > 0)
            {
                var entity = ForbidenService.Find(p => p.ID == ForbidenConfigEntity.ID).FirstOrDefault();
                if (entity != null)
                {
                    entity.CITY_CODE = ForbidenConfigEntity.CITY_CODE;
                    entity.FORBIDEN_AREA = ForbidenConfigEntity.FORBIDEN_AREA;
                    entity.FORBIDEN_ID_START = ForbidenConfigEntity.FORBIDEN_ID_START;
                    entity.PRODUCT_LOGO = ForbidenConfigEntity.PRODUCT_LOGO;
                    entity.JUST_FOR_CITY = ForbidenConfigEntity.JUST_FOR_CITY;
                    entity.CHANGED_TIME = DateTime.Now;
                    entity.CHANGED_USER = currentUser.Account;
                    entity.PROVINCE_CODE = ForbidenConfigEntity.PROVINCE_CODE;
                    entity.PRPVINCE_CITY = ForbidenConfigEntity.PRPVINCE_CITY;
                    Infrastructure.Log4Net.LogWriter.Biz("更新黑名单配置信息", entity);
                }
            }
            else
            {
                ForbidenConfigEntity.CREATED_TIME = DateTime.Now;
                ForbidenConfigEntity.CREATED_USER = currentUser.Account;
                ForbidenConfigEntity.CHANGED_TIME = DateTime.Now;
                ForbidenConfigEntity.CHANGED_USER = currentUser.Account;
                ForbidenService.Add(ForbidenConfigEntity);
                Infrastructure.Log4Net.LogWriter.Biz("新增黑名单配置信息", ForbidenConfigEntity);
            }
            ForbidenService.UnitOfWork.SaveChanges();
            return sb.ToString();
        }

        public string DeleteForbidenConfig(int id)
        {
            string msg = string.Empty;
            var result = ForbidenService.Find(p => p.ID == id).FirstOrDefault();
            if (result != null)
            {
                ForbidenService.Delete(result);
                ForbidenService.UnitOfWork.SaveChanges();
                Infrastructure.Log4Net.LogWriter.Biz("删除黑名单配置信息", result);
            }
            else
            {
                msg += "数据没找到,无法删除";
            }
            return msg;
        }

        #endregion

        #region 车辆数据导入

        public ActionResult CarInfoImport()
        {
            var entity = CarPriceService.GetAll().OrderByDescending(c => c.ID).FirstOrDefault();
            if (entity != null)
            {
                ViewData["lastImportTime"] = entity.CREATED_TIME;
            }
            return View();
        }

        [HttpPost]
        public string CarInfoFileUpload()
        {
            string resultMsg = String.Empty;

            resultMsg = CarPriceService.DeleteAll();
            if (!string.IsNullOrEmpty(resultMsg))
            {
                return resultMsg;
            }

            resultMsg = CarPriceService.ImportFromExcel(Request);

            return resultMsg;
        }

        #endregion

        #region 消息队列信息发送


        public ActionResult PushMQMessage()
        {
            return View();
        }

        [HttpPost]
        public string PushMQMessageAjax(string message, string queueName)
        {
            string error = string.Empty;

            if (string.IsNullOrWhiteSpace(message))
            {
                error = "消息为空！";
                return error;
            }

            if (string.IsNullOrWhiteSpace(queueName))
            {
                error = "Queue Name 为空！";
                return error;
            }

            //using (
            //    QK.QAPP.Infrastructure.MessageQueue.MQProducer MQ_Producer =
            //        new QK.QAPP.Infrastructure.MessageQueue.MQProducer(GlobalSetting.MQMultipleServer,
            //            GlobalSetting.MQUserName,
            //            GlobalSetting.MQUserPassword,
            //            queueName))
            //{
            //    if (!MQ_Producer.Publish(message, out error))
            //    {
            //        Infrastructure.Log4Net.LogWriter.Error(error);
            //        error = "消息发送失败！";
            //    }
            //}
            if (!Infrastructure.MessageQueue.MQHelper.Publish(
                GlobalSetting.MQMultipleServer,
                GlobalSetting.MQUserName,
                GlobalSetting.MQUserPassword,
                queueName,
                message,out error))
            {
                LogWriter.Error(error);
                error = "消息发送失败！";
            }

            return error;
        }

        #endregion

        #region 城市产品配置（申请）

        public ActionResult CityProductConfigQapp()
        {
            return View();
        }

        public ActionResult CityEditQapp(string platform, long id = -1)
        {
            if (id < 0)
            {
                //添加
                ViewData["operation"] = "add";
            }
            else
            {
                //编辑
                ViewData["operation"] = "edit";
            }
            ViewData["UsingPlatformForCityProduct"] = Global.GlobalSetting.UsingPlatformForCityProduct;
            ViewData["CurrentPlatform"] = platform;
            ViewBag.orgRoleList = CityService.GetOrgRoleList();
            //ViewBag.productDic = GetAllProductDic();
            return View();
        }

        public ActionResult ProductConfig(long cityId)
        {
            ViewBag.MenuGroups = GlobalSetting.LogoGroupForMenu;
            ViewBag.productDic = GetAllProductDic();
            ViewBag.AppCity = CityService.FirstOrDefault(c => c.ID == cityId);

            return View();
        }

        public JsonResult GetCityProduct(string cityCode, string menuGroup)
        {
            var cityProduct = CityProductService
                .FirstOrDefault(c => c.CITY_CODE == cityCode && c.MENU_GROUP == menuGroup);
            return Json(cityProduct, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ContentResult UpdateOrAddCityProduct(APP_CITY_PRODUCT entity, string[] PRODUCT_CODE)
        {
            string msg = string.Empty;

            if (PRODUCT_CODE != null && PRODUCT_CODE.Length > 0)
                entity.PRODUCT_CODE = PRODUCT_CODE.JoinString(",");

            //验证
            if (string.IsNullOrEmpty(entity.CITY_CODE))
            {
                msg += "【城市编码】不能为空！";
            }
            if (string.IsNullOrEmpty(entity.MENU_GROUP))
            {
                msg += "【菜单】不能为空！";
            }
            var newEntity = CityProductService
                .FirstOrDefault(c => c.CITY_CODE == entity.CITY_CODE && c.MENU_GROUP == entity.MENU_GROUP);

            if (newEntity != null)
            {
                //Update
                newEntity.ENABLE = entity.ENABLE;
                newEntity.PRODUCT_CODE = entity.PRODUCT_CODE;
                newEntity.CHANGED_TIME = DateTime.Now;

                CityProductService.Update(newEntity);
            }
            else
            {
                //Add
                entity.ID = 0;
                entity.CREATED_TIME = DateTime.Now;
                entity.CHANGED_TIME = entity.CREATED_TIME;

                CityProductService.Add(entity);
            }
            CityProductService.UnitOfWork.SaveChanges();

            return Content(msg);
        }

        #endregion

        #region 标的job配置
        /// <summary>
        /// 标的job配置列表取得
        /// </summary>
        /// <returns></returns>
        public ActionResult BidJobConfigList()
        {
            // 取得所有额度信息
            ViewBag.AmtListInfo = GetAllAmtType();
            // 取得所有任务信息
            ViewBag.JobTypeInfo = GetJobType();

            List<QB_JOB_CONFIG_INFO> QbJobConfigINFO = new List<QB_JOB_CONFIG_INFO>();
            QbJobConfigINFO = IBidJobConfigService.GetBidJobConfigList();

            // 检查任务是否正在运行
            if (QbJobConfigINFO != null && QbJobConfigINFO.Count > 0)
            {
                IBidJobConfigService.CheckJobRun(QbJobConfigINFO);
            }
            return View(QbJobConfigINFO);
        }

        /// <summary>
        /// 标的job配置编辑
        /// </summary>
        /// <returns></returns>
        public ActionResult BidJobConfigEdit(string id)
        {
            var model = new QB_JOB_CONFIG_INFO();
            // 编辑的场合
            if (!string.IsNullOrEmpty(id) && id != "0")
            {
                model = IBidJobConfigService.GetBidJobConfigEditInfo(id);
            }
            Dictionary<string, string> listAmt = new Dictionary<string, string>();
            // 编辑的场合
            if (model != null && !string.IsNullOrEmpty(model.JOB_TYPE))
            {
                listAmt = GetAmtByJobTypeMethod(model.JOB_TYPE);
            }
            // 取得所有额度信息
            ViewBag.AmtListInfo = listAmt;
            // 取得所有任务信息
            ViewBag.JobTypeInfo = GetJobType();

            return View(model);

        }
        /// <summary>
        /// 所有任务类型
        /// </summary>
        /// <returns>字典集合</returns>
        private Dictionary<string, string> GetJobType(bool bRemoveDelJob = false)
        {
            Dictionary<string, string> listJobType = new Dictionary<string, string>();

            listJobType = IQuotaBidDicConfigService.GetQbDicByType("BidAuotJobType");
            if (bRemoveDelJob)
            {
                //排除自动流标
                listJobType.Remove("AutoDelGBJob");
            }
            return listJobType;
        }

        /// <summary>
        /// 所有额度类型
        /// </summary>
        /// <returns>字典集合</returns>
        private List<QB_V_AMT_ATTRIBUTE> GetAllAmtType()
        {
            //根据部门筛选大额度类型
            var quotaTypeListParent = quotaManageService.GetQuotaType().Where(p => p.LEVEL_NO == 1).ToDictionary(k => k.AMT_TYPE, v => v.AMT_NAME);
            var quotaTypeListChild = quotaManageService.GetQuotaType().Where(p => p.LEVEL_NO == 2).ToList();
            quotaTypeListChild.ForEach(p =>
            {
                if (quotaTypeListParent.ContainsKey(p.PARENT_CODE))
                {
                    p.AMT_NAME = quotaTypeListParent[p.PARENT_CODE] + "-" + p.AMT_NAME;
                }
            });
            return quotaTypeListChild;
        }

        /// <summary>
        /// 新增或者编辑标的job配置
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public string EditJobConfig(QB_JOB_CONFIG_INFO entity)
        {
            try
            {
                return IBidJobConfigService.CreateOrEditJobConfig(entity);
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }

        [HttpPost]
        public string DeleteJobConfig(string id)
        {
            try
            {
                IBidJobConfigService.DeleteJobConfigInfo(id);
                return "";
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }

        [HttpPost]
        public ActionResult JobOperateConfig(string type, string id)
        {
            var message = "";
            if (type == "1")
            {
                message = "启动自动任务";
            }
            else
            {
                message = "关闭自动任务";
            }

            try
            {
                var result = IBidJobConfigService.JobOperateConfig(type, id);
                if (result)
                {
                    return Json(new { flag = "ok", Msg = message + "成功" });
                }
                else
                {
                    return Json(new { flag = "error", Msg = message + "失败" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { flag = "error", Msg = message + "失败：" + ex.Message });
            }
        }

        /// <summary>
        /// 根据任务类型取得可用额度
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetAmtByJobType(string jobtype)
        {
            try
            {
                Dictionary<string, string> dicAmt = GetAmtByJobTypeMethod(jobtype);
                return Json(dicAmt, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex.ToString(), JsonRequestBehavior.AllowGet);
            }
        }

        private Dictionary<string, string> GetAmtByJobTypeMethod(string jobtype)
        {
            Dictionary<string, string> dicAmt = new Dictionary<string, string>();
            List<string> listAmt = new List<string>();
            // 编辑的场合
            if (!string.IsNullOrEmpty(jobtype))
            {
                listAmt = IBidJobConfigService.GetAmtByJobType(jobtype);
                List<QB_V_AMT_ATTRIBUTE> listAllAmtType = GetAllAmtType();
                QB_V_AMT_ATTRIBUTE qbVlist = null;
                foreach (string str in listAmt)
                {
                    qbVlist = listAllAmtType.Find(c => c.AMT_TYPE == str);
                    if (qbVlist != null)
                    {
                        dicAmt.Add(str, qbVlist.AMT_NAME);
                    }
                }
            }
            return dicAmt;
        }
        #endregion

        #region 额度任务主配置
        /// <summary>
        /// 额度任务主配置
        /// </summary>
        /// <returns></returns>
        public ActionResult BidJobMainConfigList()
        {
            // 取得所有额度信息
            ViewBag.AmtListInfo = GetAllAmtType();
            // 取得所有任务信息
            ViewBag.JobTypeInfo = GetJobType(true);

            List<JobAmtInfo> listQB_JOB_AMT_INFO = new List<JobAmtInfo>();
            // 调用服务返回额度和任务关系对象
            listQB_JOB_AMT_INFO = IQBBidJobAmtService.GetJobAmtInfoList();
            return View(listQB_JOB_AMT_INFO);
        }

        /// <summary>
        /// 额度任务配置编辑
        /// </summary>
        /// <returns></returns>
        public ActionResult BidJobMainConfigEdit(string amttype)
        {
            // 取得所有额度信息
            ViewBag.AmtListInfo = GetAllAmtType();
            // 取得所有任务信息
            ViewBag.JobTypeInfo = GetJobType(true);

            var model = new JobAmtInfo();
            // 编辑的场合
            if (!string.IsNullOrEmpty(amttype) && amttype != "0")
            {
                model = IQBBidJobAmtService.GetBidJobEditInfoByType(amttype);
            }
            return View(model);
        }

        /// <summary>
        /// 新增或者编辑额度任务配置
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public string EditJobAmt(JobAmtInfo entity)
        {
            try
            {
                IQBBidJobAmtService.CreateOrEditJobAmt(entity);
                return "";
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }

        /// <summary>
        /// 根据额度类型类型取得
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetJobByAmtType(string amttype)
        {
            try
            {
                var model = new JobAmtInfo();
                // 编辑的场合
                if (!string.IsNullOrEmpty(amttype))
                {
                    model = IQBBidJobAmtService.GetBidJobEditInfoByType(amttype);
                }
                return Json(model, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex.ToString(), JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public string DeleteJobAmtInfo(string amttype)
        {
            try
            {
                IQBBidJobAmtService.DeleteJobAmtInfo(amttype);
                return "删除成功";
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }
        #endregion

        #region 自动任务日志
        /// <summary>
        /// 自动任务日志信息取得
        /// </summary>
        /// <returns></returns>
        public ActionResult BidAutoJobLog()
        {
            // 取得所有额度信息
            ViewBag.AmtListInfo = GetAllAmtType();
            // 取得所有任务信息
            ViewBag.JobTypeInfo = GetJobType();

            return View("~/Views/SystemConfig/BidAutoJobLog.cshtml");
        }

        /// <summary>
        /// 取得自动任务日志列表
        /// </summary>
        /// <returns></returns>
        public JsonResult GetBidAutoJobLogList()
        {
            AutoJobLogListPara logPara = new AutoJobLogListPara();
            logPara.LogType = Request["logtype"];
            logPara.JobType = Request["jobtype"];
            logPara.AmtType = Request["amttype"];
            logPara.UseDate = Request["useDate"];
            logPara.CurrentPage = int.Parse(string.IsNullOrEmpty(Request["page"]) ? "1" : Request["page"].ToString());//如果为空，默认第一页
            logPara.Rows = int.Parse(string.IsNullOrEmpty(Request["rows"]) ? "20" : Request["rows"].ToString());//如果为空，默认20条
            var result = IQBAutoJobLogService.GetAutoJobLogList(logPara);
            // 取得所有额度信息
            List<QB_V_AMT_ATTRIBUTE> listAmt = GetAllAmtType();
            // 取得所有任务信息
            Dictionary<string, string> dicJob = GetJobType();
            List<QB_AUTO_JOB_LOG> qbLog;
            if (result != null && result.DataList != null && result.DataList.Count > 0)
            {
                qbLog = result.DataList;
                foreach (QB_AUTO_JOB_LOG model in qbLog)
                {
                    foreach (QB_V_AMT_ATTRIBUTE amtModel in listAmt)
                    {
                        if (model.AMT_TYPE == amtModel.AMT_TYPE)
                        {
                            model.AMT_TYPE_NAME = amtModel.AMT_NAME;
                            break;
                        }
                    }

                    foreach (KeyValuePair<string, string> jobModel in dicJob)
                    {
                        if (model.JOB_TYPE == jobModel.Key)
                        {
                            model.JOB_TYPE_NAME = jobModel.Value;
                            break;
                        }
                    }
                }
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        #endregion
    }
}