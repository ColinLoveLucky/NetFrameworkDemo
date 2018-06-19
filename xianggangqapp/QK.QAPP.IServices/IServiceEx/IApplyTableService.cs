/***********************
 * 作    者：刘云松
 * 创建时间：‎‎2014‎-‎10‎-‎15‎ ‏‎16:40:56
 * 作    用：提供进件、补件的查询与更新
*****************************/
using QK.QAPP.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QK.QAPP.IServices
{
    public interface IApplyTableService
    {
        #region 属性、字段


        #endregion
        /// <summary>
        /// 根据主表的状态查询进件
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        EnterListViewFiledList GetEnterOrderListByMainStatus(EnterListSearchPara para);

        /// <summary>
        /// 根据补件状态查询进件
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        EnterListViewFiledList GetEnterOrderListBySDStatus(EnterListSearchPara para);

         /// <summary>
        /// 根据appId变更app_main的app_status
       /// </summary>
       /// <param name="appId">申请单的ID</param>
       /// <param name="statusType">变更后的状态</param>
       /// <param name="beforeStatus">变跟前的状态</param>
        bool UpdateEnterOrderStatus(long appId, EnterStatusType statusType, string beforeStatus);

        /// <summary>
        /// 根据appId废弃进件并重置原进件HAS_EXTEND
        /// </summary>
        /// <param name="appId">申请单的ID</param>
        /// <param name="beforeStatus">变跟前的状态</param>
        bool DisusedOrderAndResetHasApply(long appId, string beforeStatus);

        /// <summary>
        /// 取得申请单需要补件的项
        /// </summary>
        /// <param name="appId"></param>
        /// <returns></returns>
        List<APP_NR_LIST> GetNRList(long appId);

        /// <summary>
        /// 检查是否需要补件
        /// </summary>
        /// <param name="appId"></param>
        /// <param name="nrId"></param>
        /// <returns></returns>
        bool CheckNrNeedSD(long appId, long nrId);

        /// <summary>
        /// 检查是否需要补件
        /// </summary>
        /// <param name="appId"></param>
        /// <param name="fileType"></param>
        /// <returns></returns>
        bool CheckNrNeedSD(long appId, string fileType);

        /// <summary>
        /// 检查是否需要补件
        /// </summary>
        /// <param name="appId"></param>
        /// <param name="nrId"></param>
        /// <param name="fileType"></param>
        /// <returns></returns>
        bool CheckNrNeedSD(long appId, long nrId, string fileType);

        /// <summary>
        /// 检查需要补件的项是否都已经补
        /// </summary>
        /// <param name="appId"></param>
        /// <returns></returns>
        string CheckSDUploaded(long appId);

        /// <summary>
        /// 更新有效的补件队列的更新事件
        /// </summary>
        /// <param name="appId"></param>
        /// <param name="nrListId"></param>
        /// <param name="clearUpdateTime"></param>
        /// <returns></returns>
        bool UpdateNrListUpdateTime(long appId, long nrListId, bool clearUpdateTime);

        /// <summary>
        /// 更新有效的补件队列的更新事件
        /// </summary>
        /// <param name="appId"></param>
        /// <param name="nrListId"></param>
        /// <param name="clearUpdateTime"></param>
        /// <returns></returns>
        bool UpdateNrListUpdateTime(long appId, string fileType, bool clearUpdateTime);

        /// <summary>
        /// 补件完成更新相关各表状态
        /// </summary>
        /// <param name="appId"></param>
        /// <returns></returns>
        string UpdateSDStatusOK(long appId);

        /// <summary>
        /// 根据进件状态查询申请单数目
        /// </summary>
        /// <param name="lstEnterStatus"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        int GetCountEnterByStatus(List<EnterStatusType> lstEnterStatus, string user);

        /// <summary>
        /// 取得补件提交时间
        /// </summary>
        /// <param name="appId"></param>
        /// <param name="fileType"></param>
        /// <returns></returns>
        DateTime? GetNrDateApply(long appId, string fileType);

        /// <summary>
        /// 车贷补件完成，各表状态更新
        /// </summary>
        /// <param name="appId"></param>
        /// <returns></returns>
        string UpdateSDStatusOKCar(long appId);

        /// <summary>
        /// 房贷补件完成，对各表状态进行更新
        /// </summary>
        /// <param name="appId"></param>
        /// <returns></returns>
        string UpdateSDStatusOKHouse(long appId);
    }
}
