using QK.QAPP.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QK.QAPP.SalesCenter.App_Code
{
    /// <summary>
    /// 申请系统对外提供的Api基类(不需要登录)
    /// </summary>
    [AllowAnonymousAttribute]
    public class ApiBaseController : Controller
    {
        #region 请求相关类
        public class ApiRequest : BaseApiRequest
        {
        }

        public class ApiResponse : BaseApiResponse
        {
        }
        #endregion
    }
}