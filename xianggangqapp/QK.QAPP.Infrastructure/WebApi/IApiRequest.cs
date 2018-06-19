/***********************
 * 作    者：Shawn 
 * 创建时间：‎‎2016年5月9日  10:42:35
*****************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QK.QAPP.Infrastructure
{
    /// <summary>
    /// Api 请求接口
    /// </summary>
    public interface IApiRequest
    {
        /// <summary>
        ///  获取接口地址
        /// </summary>
        /// <returns></returns>
        string GetApiName();

        /// <summary>
        /// 系统参数签名
        /// </summary>
        /// <param name="appKey"></param>
        /// <param name="appSecret"></param>
        void SignRequest(string appKey, string appSecret);

        /// <summary>
        /// 获取系统参数字典
        /// </summary>
        /// <returns></returns>
        Dictionary<string, string> GetSysParamDic();

        /// <summary>
        /// 获取加密签名
        /// </summary>
        /// <returns></returns>
        string GetSign();
    }
}
