using QK.QAPP.Entity;
using QK.QAPP.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QK.QAPP.SalesCenter.Controllers.Quota
{
    public class RaisingPlanController : Controller
    {
        public IRaisingPlanService raisingPlanService { get; set; }
        #region 01 募集计划管理-历史
        //
        // GET: /RaisingPlan/
        public ActionResult RaisingPlan()
        {
            return View("~/Views/Quota/RaisingPlan.cshtml");
        }

        public JsonResult GetRaisingPlanList()
        {
            ListViewBase para = new ListViewBase();
            para.KEY_VALUE = Request["keyWord"];
            para.CurrentPage = int.Parse(string.IsNullOrEmpty(Request["page"]) ? "1" : Request["page"].ToString());//如果为空，默认第一页
            para.Rows = int.Parse(string.IsNullOrEmpty(Request["rows"]) ? "20" : Request["rows"].ToString());//如果为空，默认20条
            var result = raisingPlanService.GetRaisingPlanList(para);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetRaisingPlanHistory()
        {
            RaisePlanListPara para = new RaisePlanListPara();
            para.KEY_VALUE = Request["keyWord"];
            para.RaiseFundNo = Request["raiseFundNo"];
            //根据主键查询。。。。。。。。。
            para.CurrentPage = int.Parse(string.IsNullOrEmpty(Request["page"]) ? "1" : Request["page"].ToString());//如果为空，默认第一页
            para.Rows = int.Parse(string.IsNullOrEmpty(Request["rows"]) ? "20" : Request["rows"].ToString());//如果为空，默认20条
            var result = raisingPlanService.GetRaisePlanHistory(para);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion 

        #region 02 募集计划详情
        // 
        // GET: /RaisingPlanDetail/
        public ActionResult RaisingPlanDetail()
        {
            string id = Request["id"];
            QB_RAISEPLAN result = raisingPlanService.GetRaisingPlanById(id);
            return View("~/Views/Quota/RaisingPlanDetail.cshtml",result);
        }

        #endregion 

    }
}