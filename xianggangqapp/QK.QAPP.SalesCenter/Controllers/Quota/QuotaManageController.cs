using Microsoft.Practices.Unity;
using Newtonsoft.Json;
using QK.QAPP.Entity;
using QK.QAPP.Entity.QbEntity;
using QK.QAPP.Infrastructure;
using QK.QAPP.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QK.QAPP.SalesCenter.Controllers.Quota
{
    public class QuotaManageController : Controller
    {
        [Dependency]
        public IQuotaManageService quotaManageService { get; set; }
        #region 01 额度管理
        //
        // GET: /QuotaManage/
        public ActionResult Index()
        {
            SetQuotaTypeToView(Request["dept"]);
            return View("~/Views/Quota/QuotaManage.cshtml");
        }

        public JsonResult GetQuotaManageList()
        {
            AmtLimitListPara amtListPara = new AmtLimitListPara();
            amtListPara.AMT_TYPE = Request["amtType"];
            amtListPara.KEY_VALUE = Request["keyWord"];
            amtListPara.AMT_USE_DATE = Request["useDate"];
            amtListPara.AMT_DEPT = Request["dept"];
            amtListPara.CurrentPage = int.Parse(string.IsNullOrEmpty(Request["page"]) ? "1" : Request["page"].ToString());//如果为空，默认第一页
            amtListPara.Rows = int.Parse(string.IsNullOrEmpty(Request["rows"]) ? "20" : Request["rows"].ToString());//如果为空，默认20条
            var result = quotaManageService.GetQuotaManageList(amtListPara);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 02 额度新增
        //新增额度
        public ActionResult QuotaAdd()
        {
            SetQuotaTypeToView(Request["dept"]);
            //获取非工作日日期列表
            var weekend = quotaManageService.GetWeekend();
            //默认T+1排除非工作日
            string T1Date = DateTime.Now.AddDays(1).ToShortDateString();
            quotaManageService.GetTiWorkDay(weekend, 1, out T1Date);
            ViewData["T1Date"] = T1Date;
            //日历插件屏蔽非工作日，数据准备
            string json = "";
            weekend.ForEach(w => { json += w.WEEKEND_DATE.ToShortDateString() + ","; });
            ViewData["NonWorkDayJson"] = json.Remove(json.LastIndexOf(","));//移除最后一个 逗点
            return View("~/Views/Quota/QuotaAdd.cshtml");
        }
       
        /// <summary>
        /// 获取子额度类型
        /// </summary>
        /// <param name="parentCode">父级code</param>
        /// <param name="dept">部门guid</param>
        /// <returns></returns>
        public JsonResult GetQuotaTypeByParentCode(string parentCode, string dept)
        {
            var quotaTypeList = quotaManageService.GetQuotaTypeByParentCode(parentCode).Where(p => p.AMT_DEPT_CODE == dept);
            return Json(quotaTypeList);
        }

        public string SaveQuota(QB_AMT_LIMIT amtLimit)
        {
            string result = quotaManageService.SaveQuota(amtLimit);
            return result;
        }
        #endregion

        #region 03 额度查看、修改
        //查看、修改额度
        public ActionResult QuotaViewOrModify()
        {
            return View("~/Views/Quota/QuotaViewOrModify.cshtml");
        }

        public JsonResult GetQuotaInfoById(string Id)
        {
            var quotaInfo = quotaManageService.GetQuotaInfoById(Id);
            return Json(quotaInfo, JsonRequestBehavior.AllowGet);
        }

        public string ModifyQuota(QB_AMT_LIMIT amtLimit)
        {
            string result = quotaManageService.ModifyQuota(amtLimit);
            return result;  //结果：{ errMsg = errMsg, flag = resultFlag }
        }
        #endregion

        #region 04 额度调整
        //调整额度
        public ActionResult QuotaAdjust()
        {
            string Id = Request["key"];
            var quotaInfo = quotaManageService.GetQuotaInfoById(Id);
            return View("~/Views/Quota/QuotaAdjust.cshtml", quotaInfo);
        }

        public string AdjustQuota(string id, string adjustType, string quotaAmt)
        {
            string result = quotaManageService.AdjustQuota(id, adjustType, quotaAmt);
            return result;
        }
        #endregion

        #region 05 额度删除
        public string DeleteQuota(string id)
        {
            string result = quotaManageService.DeleteQuota(id);
            return result;
        }
        #endregion

        #region 06 额度历史
        public JsonResult GetQuotaHistoryList()
        {
            AmtLimitListPara amtListPara = new AmtLimitListPara();
            //SIT-1130:操作历史目前没有分部门查询
            amtListPara.AMT_DEPT = Request["dept"];
            amtListPara.AMT_USE_DATE = Request["useDate"];
            amtListPara.KEY_VALUE = Request["keyWord"];
            amtListPara.CurrentPage = int.Parse(string.IsNullOrEmpty(Request["page"]) ? "1" : Request["page"].ToString());//如果为空，默认第一页
            amtListPara.Rows = int.Parse(string.IsNullOrEmpty(Request["rows"]) ? "20" : Request["rows"].ToString());//如果为空，默认20条
            amtListPara.AMT_ID = Request["amtId"];
            amtListPara.IsAdjusted = 1;
            string pageIndex = string.IsNullOrEmpty(Request["page"]) ? "1" : Request["page"];
            string pageSize = string.IsNullOrEmpty(Request["rows"]) ? "20" : Request["rows"];
            var result = quotaManageService.GetQuotaHistoryList(amtListPara);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 07 其他方法

        /// <summary>
        /// 将额度类型保存到View--保存一级
        /// </summary>
        public void SetQuotaTypeToView(string dept)
        {
            //根据部门筛选大额度类型
            var quotaTypeListParent = quotaManageService.GetQuotaType().Where(p => p.LEVEL_NO == 1).ToDictionary(k => k.AMT_TYPE, v => v.AMT_NAME);
            var quotaTypeListChild = quotaManageService.GetQuotaType().Where(p => p.LEVEL_NO == 2 && p.AMT_DEPT_CODE == dept).ToList();
            quotaTypeListChild.ForEach(p =>
            {
                if (quotaTypeListParent.ContainsKey(p.PARENT_CODE))
                {
                    p.PARENT_NAME = quotaTypeListParent[p.PARENT_CODE];
                }
            });
            ViewData["QuotaType"] = quotaTypeListChild.Distinct(d => d.PARENT_CODE);
        }

        #endregion
    }
}