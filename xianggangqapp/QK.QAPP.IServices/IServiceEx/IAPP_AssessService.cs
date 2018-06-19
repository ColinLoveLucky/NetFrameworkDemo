/***********************
 * 作    者：刘云松
 * 创建时间：‎‎2015-3-20 14:22:07
 * 作    用：提供（车贷）评估相关操作
*****************************/
using QK.QAPP.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QK.QAPP.IServices
{
    public interface IAPP_AssessService
    {
        /// <summary>
        /// 待评估状态
        /// </summary>
        Dictionary<string, string> AssessQueueStatus { get; }

        /// <summary>
        /// 需要评估的车贷logo
        /// </summary>
        List<string> NeedAssessProductLogo { get; }

        /// <summary>
        /// 根据主表的状态查询进件
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        AssessQueueViewFieldList GetAssessListByMainStatus(AssessListSearchPara para);

        /// <summary>
        /// 根据APP_CODE更新评估师
        /// </summary>
        /// <param name="appCode"></param>
        /// <param name="valuatorCode"></param>
        /// <param name="valuatorName"></param>
        /// <returns></returns>
        bool UpdateValuatorByAppCode(string appCode,string valuatorCode, string valuatorName);

        /// <summary>
        /// 查询已批复队列
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        AssessQueueViewFieldList GetApprovedList(AssessListSearchPara para);

        /// <summary>
        /// 查询批复队列数据
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="logo"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        APP_QUEUE_ASSESS GetAssessInfo(long ID, string logo, string status);

        /// <summary>
        /// 查询车况评估详情
        /// </summary>
        /// <param name="appCode"></param>
        /// <returns></returns>
        PAD_CARJUDGE GetCarJudgeInfo(string appCode);

        /// <summary>
        /// 更新队列中某条记录
        /// <para>这里直接使用了update，所以不接受自己创建的对象</para>
        /// </summary>
        /// <param name="appAssess">来自查询结果的评估队列数据</param>
        void UpdateAssessInfo(APP_QUEUE_ASSESS appAssess);

        /// <summary>
        /// 根据id变更APP_QUEUE_ASSESS的assess_status
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="statusType">变更后的状态</param>
        /// <param name="beforeStatus">变更钱的状态</param>
        bool UpdateAssessStatus(long id, AssessStatusType statusType, string beforeStatus);

        /// <summary>
        /// 判断预约时间是否和当前评估师的其他预约时间间隔20分钟
        /// </summary>
        /// <param name="queueAssess"></param>
        /// <returns></returns>
        bool CheckBookTime(APP_QUEUE_ASSESS queueAssess);
    }
}
