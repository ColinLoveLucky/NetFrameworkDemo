/***********************
 * 作    者：刘云松
 * 创建时间：‎‎2015-3-9 15:41:22
 * 作    用：提供展期、循环贷等扩展申请单服务
*****************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QK.QAPP.Entity;

namespace QK.QAPP.IServices
{
    public interface IExtendApplyService
    {
        #region 属性

        /// <summary>
        /// 待补件状态
        /// </summary>
        Dictionary<string, string> NeedExtendStatus_Extend { get; }

        /// <summary>
        /// 除了补件之外的所有进件状态
        /// </summary>
        Dictionary<string, string> Order_ExceptSD_Status_Car { get; }

        /// <summary>
        /// 扩展申请单的方式：展期？循环贷？
        /// <para>取GlobalSetting.APPExtendConfig_</para>
        /// </summary>
        List<string> ActionGroup_Extend { get; }

        #endregion

        /// <summary>
        /// 待扩展的申请单列表
        /// </summary>
        /// <param name="searchCondition"></param>
        /// <returns></returns>
        ExtendApplyViewFieldList ExtendListToBe(ExtendApplySearchPara searchCondition);

        /// <summary>
        /// 扩展历史
        /// </summary>
        /// <param name="appCode"></param>
        /// <param name="actionGroup"></param>
        /// <returns></returns>
        List<APP_EXTEND_RELATION> ExtendHistory(string appCode, string actionGroup);

        /// <summary>
        /// 已扩展过的申请单
        /// </summary>
        /// <param name="searchCondition"></param>
        /// <returns></returns>
        EnterListViewFiledList ExtendedList(ExtendApplySearchPara searchCondition);

        /// <summary>
        /// 车贷可展条件SQL（其中 V_APPMAIN_EXTEND 为 AE）
        /// </summary>
        /// <param name="strSQL"></param>
        void AddExtendConditionCar(StringBuilder strSQL);

        /// <summary>
        /// 房贷可展条件SQL（其中 V_APPMAIN_EXTEND 为 AE）
        /// </summary>
        /// <param name="strSQL"></param>
        void AddExtendConditionHouse(StringBuilder strSQL);

        /// <summary>
        /// 可展列表数据权限（用户权限）
        /// </summary>
        /// <param name="currentAuth"></param>
        /// <param name="strSQL"></param>
        /// <param name="lstSqlPara"></param>
        void AddExtendPermission(QFUserAuth currentAuth, StringBuilder strSQL, List<object> lstSqlPara);

        /// <summary>
        /// 可展列表数据权限（房贷，可看当前城市的所有进件）
        /// </summary>
        /// <param name="currentAuth"></param>
        /// <param name="strSQL"></param>
        /// <param name="lstSqlPara"></param>
        void AddExtendPermissionHouse(QFUserAuth currentAuth, StringBuilder strSQL, List<object> lstSqlPara);
    }
}
