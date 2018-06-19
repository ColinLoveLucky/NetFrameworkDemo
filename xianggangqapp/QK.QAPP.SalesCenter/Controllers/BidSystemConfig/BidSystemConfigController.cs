using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QK.QAPP.Entity;
using QK.QAPP.Entity.ExtendEntity;
using QK.QAPP.Entity.QbEntity;
using QK.QAPP.Infrastructure;
using QK.QAPP.IServices.IQBService;

namespace QK.QAPP.SalesCenter.Controllers
{
    public class BidSystemConfigController : Controller
    {
        private IBidSystemConfigService bidSysConfigService;

        public BidSystemConfigController(IBidSystemConfigService service)
        {
            bidSysConfigService = service;
        }

        /// <summary>
        /// 标的系统配置列表
        /// </summary>
        /// <returns></returns>
        public ActionResult BidSysConfigList()
        {
            var result = bidSysConfigService.GetBidSystemConfigList().OrderBy(m => m.ID).ToList();
            return View(result);
        }

        /// <summary>
        /// 标的系统配置修改
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult BidSysConfigEdit(string id)
        {

            var model = new BidSystemConfigInfo();
            if (!string.IsNullOrEmpty(id) && id != "0")
            {
                model = bidSysConfigService.GetInfoById(id);
            }
            if (model != null)
            {
                return View(model);
            }

            return View(new BidSystemConfigInfo());
        }

        /// <summary>
        /// 标的系统配置修改
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        public string BidSysConfig(BidSystemConfigInfo entity)
        {
            try
            {
                if (entity != null)
                {
                    if (entity.ID > 0)
                    {
                        bidSysConfigService.Update(entity);
                        //日志
                        Infrastructure.Log4Net.LogWriter.Biz("修改标的系统配置", entity.ID + string.Empty, entity);
                    }
                    else
                    {
                        if (bidSysConfigService.IsExistKey(entity.SYS_KEY))
                        {
                            return "您输入的键已经存在!";
                        }
                        bidSysConfigService.Add(entity);
                        //日志
                        Infrastructure.Log4Net.LogWriter.Biz("添加标的系统配置", entity.ID + string.Empty, entity);
                    }

                    //bidSysConfigService.ClearCacheByKey(entity.KEY);
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
        public string DeleteBidSysConfig(string id)
        {
            try
            {
                var model = bidSysConfigService.GetInfoById(id);
                if (model != null)
                {
                    string error = bidSysConfigService.Delete(id) ? "" : "删除失败";

                    //日志
                    Infrastructure.Log4Net.LogWriter.Biz("删除标的系统配置", model.ID + string.Empty, model);

                    return error;
                }
                return "";
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }
    }
}