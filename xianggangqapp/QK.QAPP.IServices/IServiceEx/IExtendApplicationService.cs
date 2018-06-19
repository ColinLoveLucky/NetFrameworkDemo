/***********************
 * 作    者：ruiwang
 * 创建时间：2015/3/12 9:19:54
 * 作    用：提供展期、循环贷业务中相关对象的操作
*****************************/

using QK.QAPP.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QK.QAPP.IServices
{
    public interface IExtendApplicationService
    {
        /// <summary>
        /// 展期申请（车贷）
        /// </summary>
        /// <param name="appIdOld">原申请单ID</param>
        /// <param name="formDic">表单信息</param>
        /// <param name="resultMsg">提示信息</param>
        /// <returns>新APP_MAIN对象</returns>
        APP_MAIN ExtendLoanCar(long appIdOld, Dictionary<string, string> formDic, out string resultMsg);

        /// <summary>
        /// 展期申请（房贷）
        /// </summary>
        /// <param name="appIdOld">原申请单ID</param>
        /// <param name="formDic">表单信息</param>
        /// <param name="resultMsg">提示信息</param>
        /// <returns></returns>
        APP_MAIN ExtendLoanHouse(long appIdOld, Dictionary<string, string> formDic, out string resultMsg);

        /// <summary>
        /// 验证申请是否有操作权限（展期）
        /// </summary>
        /// <param name="appMainEntity">APP_MAIN对象</param>
        /// <param name="operation">操作</param>
        /// <param name="canExtendFunc">是否可展方法</param>
        /// <returns>消息（如果为空则表示有权限）</returns>
        string CheckExtendPermission(APP_MAIN appMainEntity, ENUM_FormOperation operation, Func<APP_MAIN, bool> canExtendFunc);

        /// <summary>
        /// 查询剩余可展期数及续展期数
        /// </summary>
        /// <param name="appMain">APP_MAIN对象</param>
        /// <param name="periodAmt">续展期数</param>
        /// <returns>剩余可展期数</returns>
        int GetExtendPeriod(APP_MAIN appMain, out int periodAmt);

        /// <summary>
        /// 获取原单的申请金额
        /// </summary>
        /// <param name="appMain">当前进件appMain</param>
        /// <returns></returns>
        decimal? GetOriApplyAmt(APP_MAIN appMain);

        /// <summary>
        /// Check某个单子是否可以做展期申请（车贷）
        /// </summary>
        /// <param name="appMain"></param>
        /// <returns></returns>
        bool CanExtend(APP_MAIN appMain);

        /// <summary>
        /// Check是否可以做展期申请（房贷）
        /// </summary>
        /// <param name="appMain"></param>
        /// <returns></returns>
        bool CanExtendHouse(APP_MAIN appMain);

        /// <summary>
        /// 获取原单合同金额
        /// </summary>
        /// <param name="appMain">当前进件</param>
        /// <returns></returns>
        decimal? GetOriContactAmt(APP_MAIN appMain);

        /// <summary>
        /// 获取原单到手金额
        /// </summary>
        /// <param name="appMain">当前进件</param>
        /// <returns></returns>
        decimal? GetOriLoanAmt(APP_MAIN appMain);
    }
}
