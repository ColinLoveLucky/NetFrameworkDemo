using QK.QAPP.Global;
using QK.QAPP.Infrastructure;
using QK.QAPP.Infrastructure.WebApi;
using QK.QAPP.IServices;
using QK.QAPP.SalesCenter.App_Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Mvc;

namespace QK.QAPP.SalesCenter.Controllers.WebApi
{
    public class MessageController : ApiBaseController
    {
        public IV_APPMAINSERVICE MainService { get; set; }

        public ICR_DATA_DICService CrDataDicService { get; set; }

        [HttpPost]
        public JsonResult AuditMessage(string appCode,string appStatus)
        {
            JsonResult result = new JsonResult();
            ApiResponse response = new ApiResponse();
            #region 参数验证
            if (string.IsNullOrEmpty(appCode))
            {
                response.BizErrorMsg = "申请编号不能为空！";
                result.Data = response;
                return result;
            }
            if (string.IsNullOrEmpty(appStatus))
            {
                response.BizErrorMsg = "进件状态不能为空！";
                result.Data = response;
                return result;
            }
            var main=MainService.FirstOrDefault(a => a.APPCODE == appCode);
            if(main==null)
            {
                response.BizErrorMsg = "没有对应的申请进件！";
                result.Data = response;
                return result;
            }
            string receiveUser = main.CSADNO;
            #endregion
            string content = string.Format(GlobalSetting.AuditMessageContent, main.CSADNAME, appCode, CrDataDicService.GetDICNameByCode(appStatus));
            string category = "AuditMessage";
            try
            {
                QAPP.SalesCenter.Hubs.PushMessage.PushToUser(receiveUser, content, category); //发送消息
            }
            catch(Exception ex)
            {
                response.ErrCode = ErrorCode.SystemError;
                response.ErrMsg = "系统错误,发送消息失败！";
                Infrastructure.Log4Net.LogWriter.Error(response.GetErrorMessage(), ex);
            }
            //日志
            Infrastructure.Log4Net.LogWriter.Biz("消息推送", category, "消息内容：" + content);
            result.Data = response;
            return result;
        }
    }
}