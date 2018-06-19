using QK.QAPP.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QK.QAPP.IServices;
using QK.QAPP.Infrastructure;
using Microsoft.Practices.Unity;

namespace QK.QAPP.SalesCenter.Controllers
{
    
    public class QuotaPropertyController : Controller
    {
        [Dependency]
        public IQuotaManageService QuotaManageService { get; set; }
        [Dependency]
        public IQuotaBidDicConfigService QuotaBidDicConfigService { get; set; }

        //
        // GET: /QuotaProperty/
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// 额度类型属性配置列表
        /// </summary>
        /// <returns></returns>
        public ActionResult QuotaPropertyConfig()
        {
            List<QB_V_AMT_ATTR_DETAIL> QuotaPropertyConfigList = new List<QB_V_AMT_ATTR_DETAIL>();
            QuotaPropertyConfigList = QuotaManageService.GetQuotaAttributeList();
            ViewBag.QuotaProperty = QuotaPropertyConfigList;
            return View("~/Views/SystemConfig/QuotaPropertyConfig.cshtml");
        }
        /// <summary>
        /// 编辑配置信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult QuotaPropertyConfigEdit(string id,string isAdd)
        {
            ViewData["add"] = isAdd;
            ViewData["QuotaType"] = QuotaBidDicConfigService.GetQbDicByType("QuotaType");
            ViewData["AMTConfirmType"] = QuotaBidDicConfigService.GetQbDicByType("AMTConfirmType");
            ViewData["AMTModifyType"] = QuotaBidDicConfigService.GetQbDicByType("AMTModifyType");
            ViewData["AMTDelCondition"] = QuotaBidDicConfigService.GetQbDicByType("AMTDelCondition");
            ViewData["AMTAdjustType"] = QuotaBidDicConfigService.GetQbDicByType("AMTAdjustType");
            ViewData["Depart"] = QuotaBidDicConfigService.GetOrgList();
            QB_AMT_LIMIT_ATTRIBUTE quota = new QB_AMT_LIMIT_ATTRIBUTE();
            if (isAdd == "1")
            {
                 quota = new QB_AMT_LIMIT_ATTRIBUTE();
                 quota = QuotaManageService.GetQuotaAttributeById(id);
            }
            return View("~/Views/SystemConfig/QuotaPropertyConfigEdit.cshtml", quota);
        }
        /// <summary>
        /// 保存配置信息
        /// </summary>
        /// <param name="quota"></param>
        /// <returns></returns>
        [HttpPost]
        public string QuotaPropertyConfigSave(QB_AMT_LIMIT_ATTRIBUTE quota)
        {
            string msg = "";
            if(quota.ID.ToString()=="0")
            {
                msg = QuotaManageService.SaveQuotaAttribute(quota);
            }
            else
            {
                msg = QuotaManageService.UpdateQuotaAttribute(quota.ID.ToString(),quota);
            }
            return msg;
        }
        /// <summary>
        /// 删除配置信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public string QuotaPropertyConfigDel(string id)
        {
            string msg = "";
            if(!string.IsNullOrEmpty(id))
            {
                msg = QuotaManageService.DeleteQuotaAttribute(id);
            }
            return msg;
        }
      
	}
}