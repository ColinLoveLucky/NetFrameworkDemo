using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QK.QAPP.Infrastructure
{
    /// <summary>
    /// api系统反馈基类
    /// </summary>
    public abstract class BaseApiResponse
    {
        #region 系统级别错误验证
        /// <summary>
        /// 系统级别的参数 -- 错误码
        /// </summary>
        public string ErrCode { get; set; }

        /// <summary>
        /// 系统级别的参数 -- 错误信息
        /// </summary>
        public string ErrMsg { get; set; }

        /// <summary>
        /// 系统级别的方法 -- 获取失败信息
        /// </summary>
        /// <returns></returns>
        public string GetErrorMessage()
        {
            return ErrMsg;
        }

        /// <summary>
        ///  系统级别的方法 - 请求是否成功
        /// </summary>
        /// <returns></returns>
        public bool IsSuccess()
        {
            return ErrCode == null;
        }

        public string GetErrCode()
        {
            return ErrCode ?? string.Empty;
        }
        #endregion

        #region 业务级别错误验证
        /// <summary>
        /// 业务错误信息
        /// </summary>
        public string BizErrorMsg { get; set; }

        /// <summary>
        /// 业务是否成功
        /// </summary>
        public bool IsBizSuccess
        {
            get { return string.IsNullOrEmpty(BizErrorMsg); }
        }
        #endregion
    }
}