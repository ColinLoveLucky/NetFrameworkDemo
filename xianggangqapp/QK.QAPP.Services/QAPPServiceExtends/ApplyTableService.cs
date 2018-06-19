/***********************
 * 作    者：刘云松
 * 创建时间：‎‎2014‎-‎10‎-‎15‎ ‏‎16:40:56
 * 作    用：提供进件、补件的查询与更新
 * 
 * 修 改 人：刘云松
 * 修改时间：2014-12-19 10:05:47
 * 修改目的：补件完成，将APP_QUEUE.QUEUE_ORDER设置为1
*****************************/
using Microsoft.Practices.Unity;
using QK.QAPP.Entity;
using QK.QAPP.Global;
using QK.QAPP.Infrastructure;
using QK.QAPP.Infrastructure.Cache;
using QK.QAPP.Infrastructure.Data.EFRepository.Repository;
using QK.QAPP.Infrastructure.Sql;
using QK.QAPP.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace QK.QAPP.Services
{
    public class ApplyTableService : IApplyTableService
    {
        #region 属性

        [Dependency]
        public IV_APPMAINSERVICE VappmainService { get; set; }

        [Dependency]
        public IAPP_MAINSERVICE AppmainService { get; set; }

        [Dependency]
        public IAPP_QUEUESERVICE AppqueueService { get; set; }

        [Dependency]
        public IAPP_NR_LISTSERVICE NrListService { get; set; }

        [Dependency]
        public ICR_DATA_DICService DataDicService { get; set; }


        [Dependency]
        public IQFUserService UserService { get; set; }

        [Dependency]
        public IAPPQueueLogService QueueService { get; set; }

        [Dependency]
        public IAPP_EXTEND_RELATIONSERVICE ExtendRelationService { get; set; }

        #endregion

        /// <summary>
        /// 根据主表的状态查询进件
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        public EnterListViewFiledList GetEnterOrderListByMainStatus(EnterListSearchPara para)
        {
            return GetEnterOrderListByStatus(para, 1);
        }

        /// <summary>
        /// 根据补件状态查询进件
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        public EnterListViewFiledList GetEnterOrderListBySDStatus(EnterListSearchPara para)
        {
            return GetEnterOrderListByStatus(para, 2);
        }

        /// <summary>
        /// 根据进件状态或补件状态查询进件
        /// </summary>
        /// <param name="para">查询参数</param>
        /// <param name="statusType">1.进件 2.补件</param>
        /// <returns></returns>
        private EnterListViewFiledList GetEnterOrderListByStatus(EnterListSearchPara para,int statusType)
        {
            //返回值
            EnterListViewFiledList enterList = new EnterListViewFiledList();
            var execsqlCount = string.Empty;
            var execsqlData = string.Empty;
            enterList.ListEnter = new List<V_APPMAIN>();
            //未授权访问任何资料
            if (para.AccessableCsac.Count == 0)
            {
                return enterList;
            }

            //将查询条件的进件状态转换由枚举转换成字符串
            List<string> lstStatus = para.ListEnterStatus.Select(a => a.ToString()).ToList();

            string selectStr = "a.APPID,a.APPCODE,a.LOGO,a.PRODUCTCODE,a.PRODUCTNAME,a.CUSTOMERNAME,a.CUSTOMERIDCARD,a.APPLYAMT,a.LOANAMTOFCONTRACT,a.EXTENDAMTOFCONTRACT,a.SALESNAME,a.SALESNO,a.CSADNAME,a.CSADNO,a.BUSINESSDEPARTMENT,a.APPSTATUS,a.SORTING,a.APPNRSDSTATUS,a.APPAPPR1SDSTATUS,a.APPSTATUSNAME,a.CREATEDUSER,a.CREATEDTIME,a.UPDATETIME";    //要查询的字段
            string resultStr = selectStr.Replace("a.", ""); //最终显示的字段
            string fromStr = "app.V_APPMAIN a inner join app.APP_AUTH b on a.APPID=b.APP_ID"; //数据源
            string sortStr = "a.SORTING asc,a.CREATEDTIME desc"; //初始排序字段
            string whereStr = string.Empty;
            StringBuilder whereBuilder = new StringBuilder();
            if (!para.FuzzySearch)  //如果是高级查询
            {
                #region 高级查询条件
                if (!string.IsNullOrEmpty(para.AppCode))
                {
                    whereBuilder.Append("and instr(a.APPCODE,'" + para.AppCode + "')>0 ");
                }
                if (!string.IsNullOrEmpty(para.CustomerName))
                {
                    whereBuilder.Append("and instr(a.CUSTOMERNAME,'" + para.CustomerName + "')>0 ");
                }
                if (!string.IsNullOrEmpty(para.CustomerIDCard))
                {
                    whereBuilder.Append("and instr(a.CUSTOMERIDCARD,'" + para.CustomerIDCard + "')>0 ");
                }
                if (para.ApplyStart != null)
                {
                    whereBuilder.Append("and a.CREATEDTIME>=to_date('" + para.ApplyStart.Value + "','YYYY-MM-DD HH24:MI:SS') ");
                }
                if (para.ApplyEnd != null)
                {
                    whereBuilder.Append("and a.CREATEDTIME<=to_date('" + para.ApplyEnd.Value + "','YYYY-MM-DD HH24:MI:SS') ");
                }
                if (!string.IsNullOrEmpty(para.SaleCode) && !string.IsNullOrEmpty(para.SaleName))
                {
                    whereBuilder.Append("and (instr(a.SALESNO,'" + para.SaleCode + "')>0 ");
                    whereBuilder.Append("or instr(a.SALESNAME,'" + para.SaleName + "')>0) ");
                }
                if (!string.IsNullOrEmpty(para.CsacCode) && !string.IsNullOrEmpty(para.CsacName))
                {
                    whereBuilder.Append("and (instr(a.CSADNO,'" + para.CsacCode + "')>0 ");
                    whereBuilder.Append("or instr(a.CSADNAME,'" + para.CsacName + "')>0) ");
                }
                #endregion
            }
            else  //如果是简单查询
            {
                if (!string.IsNullOrEmpty(para.AppCode) && !string.IsNullOrEmpty(para.CustomerName))
                {
                    whereBuilder.Append("and (instr(a.APPCODE,'" + para.AppCode + "')>0 ");
                    whereBuilder.Append("or instr(a.CUSTOMERNAME,'" + para.CustomerName + "')>0) ");
                }
            }
            #region 通用查询条件
            if (lstStatus.Any())    //筛选进件状态或补件状态
            {
                whereBuilder.Append("and (");
                string statusStr = string.Empty;
                if (statusType == 1)  //进件
                {
                    lstStatus.ForEach(a => statusStr += "a.APPSTATUS='" + a + "' or ");
                }
                else if (statusType == 2) //补件
                {
                    lstStatus.ForEach(a => statusStr += "a.APPNRSDSTATUS='" + a + "' or a.APPAPPR1SDSTATUS='"+a+"' or ");
                }
                whereBuilder.Append(statusStr.Substring(0, statusStr.Length - 4) + ") ");
            }
            if (para.ListLogo.Any())    //筛选产品Logo    
            {
                whereBuilder.Append("and (");
                string logoStr = string.Empty;
                para.ListLogo.ForEach(a => logoStr += "a.LOGO='" + a + "' or ");
                whereBuilder.Append(logoStr.Substring(0, logoStr.Length - 4) + ") ");
            }
            #endregion

            if (statusType==1&&!para.NeedTag)  //进件需要排除扩展申请
            {
                whereBuilder.Append("and not exists(select 1 from app.APP_EXTEND_RELATION where APP_CODE=a.APPCODE) ");
            }
            string userAuthWhereStr = UserService.GetUserAuthWhereStr("b", para.InputMenuCode);
            if (!string.IsNullOrEmpty(userAuthWhereStr))
            {
                whereBuilder.Append("and (" + userAuthWhereStr + ") ");  //筛选用户权限
            }
 
            whereStr = whereBuilder.ToString(); 
            #region 自定义排序条件
            if (para.Sort.Count > 0 && para.Sort.Any(a=>a.Key!=""&&a.Value!=""))  //点击表格标题排序
            {
                sortStr = string.Empty;
                foreach (KeyValuePair<string, string> kv in para.Sort)
                {
                    sortStr += kv.Key + " " + kv.Value + ",";
                }
                sortStr = sortStr.Substring(0, sortStr.Length - 1);
            }
            #endregion
            execsqlCount = PagingHelper.GetPagingCountSql(fromStr, whereStr);   //取数据总数sql
            execsqlData = PagingHelper.GetPagingDataSql(selectStr, resultStr, fromStr, sortStr, whereStr, para.PageIndex, para.PageSize);  //取分页数据sql
            var dataCount = VappmainService.SqlQuery(typeof(int), execsqlCount).Cast<int>().FirstOrDefault();  //数据总数
            var dataList = VappmainService.SqlQuery(execsqlData).ToList();    //分页数据
            enterList.SetParameters(dataCount, dataList, para);
            return enterList;
        }

        /// <summary>
        /// 根据appId变更app_main的app_status
        /// </summary>
        /// <param name="appId">申请单的ID</param>
        /// <param name="statusType">变更后的状态</param>
        /// <param name="beforeStatus">变跟前的状态</param>
        public bool UpdateEnterOrderStatus(long appId, EnterStatusType statusType, string beforeStatus)
        {
            bool error = true;
            APP_MAIN entity = AppmainService.FirstOrDefault(a => a.ID == appId && a.APP_STATUS == beforeStatus);
            if (entity != null)
            {
                entity.APP_STATUS = statusType.ToString();
                entity.CHANGED_TIME = DateTime.Now;
                entity.CHANGED_USER = UserService.GetCurrentUser().Account;
                AppmainService.Update(entity);
                AppmainService.UnitOfWork.SaveChanges();
                Infrastructure.Log4Net.LogWriter.Biz("变更此[ID]的进件状态（主表）", appId.ToString(), statusType);
            }
            else
            {
                error = false;
                Infrastructure.Log4Net.LogWriter.Biz("查询APP_Main时传入的参数有误", string.Empty, new Dictionary<string, string>() { { "appId", appId.ToString() } });
            }
            return error;
        }

        /// <summary>
        /// 根据appId废弃进件并重置原进件HAS_EXTEND
        /// </summary>
        /// <param name="appId">申请单的ID</param>
        /// <param name="beforeStatus">变跟前的状态</param>
        public bool DisusedOrderAndResetHasApply(long appId, string beforeStatus)
        {
            bool error = true;
            APP_MAIN entity = AppmainService.FirstOrDefault(a => a.ID == appId && a.APP_STATUS == beforeStatus);
            if (entity != null)
            {
                //获取展期关系
                APP_EXTEND_RELATION relation = ExtendRelationService.FirstOrDefault(r => r.APP_CODE == entity.APP_CODE
                    && r.ACTION_GROUP == GlobalSetting.APPExtendConfig_Extend.FirstOrDefault());
                if (relation != null)
                {
                    //获取原进件
                    APP_MAIN parentEntity = AppmainService.FirstOrDefault(a => a.APP_CODE == relation.PARENT_APP_CODE);
                    if (parentEntity != null)
                    {
                        //置HAS_EXTEND为空，此进件接下来可以继续做展期
                        parentEntity.HAS_EXTEND = null;
                        parentEntity.CHANGED_TIME = DateTime.Now;
                        parentEntity.CHANGED_USER = UserService.GetCurrentUser().Account;
                        AppmainService.Update(parentEntity);
                    }
                    else
                    {
                        Infrastructure.Log4Net.LogWriter.Biz("警告！展期进件[ID]废弃时未找到原进件", appId.ToString());
                    }
                }
                else
                {
                    Infrastructure.Log4Net.LogWriter.Biz("警告！展期进件[ID]废弃时未找到展期关系", appId.ToString());
                }

                //废弃进件
                entity.APP_STATUS = EnterStatusType.DISUSED.ToString();
                entity.CHANGED_TIME = DateTime.Now;
                entity.CHANGED_USER = UserService.GetCurrentUser().Account;
                AppmainService.Update(entity);
                AppmainService.UnitOfWork.SaveChanges();
                Infrastructure.Log4Net.LogWriter.Biz("废弃此[ID]的进件状态（主表），并重置HAS_EXTEND状态", appId.ToString(), entity.APP_STATUS);
            }
            else
            {
                error = false;
                Infrastructure.Log4Net.LogWriter.Biz("查询APP_Main时传入的参数有误", string.Empty, new Dictionary<string, string>() { { "appId", appId.ToString() } });
            }
            return error;
        }

        /// <summary>
        /// 取得申请单需要补件的项
        /// </summary>
        /// <param name="appId"></param>
        /// <returns></returns>
        public List<APP_NR_LIST> GetNRList(long appId)
        {
            //需要补件的状态
            List<string> lstStatus = Global.GlobalSetting.Order_SD_Status_Need.Keys.ToList();
            //只读取还在补件期限内，并且没有补过的件
            //var nrList = NrListService.Find(n => n.APP_ID == appId
            //    && lstStatus.Contains(n.NR_STATUS)
            //    && n.NR_DATE_SUBMIT_FIRST == null
            //);
            //增加查询条件 nr_seq
            var nrList =
                from n in NrListService.Find(n => n.APP_ID == appId
                    && lstStatus.Contains(n.NR_STATUS)
                    && n.NR_DATE_SUBMIT_FIRST == null)
                join a in AppmainService.Find(a => a.ID == appId) on n.APP_ID equals a.ID
                where n.NR_SEQ == a.NR_SEQ
                select n;

            if (nrList != null)
                return nrList.ToList();
            else return null;
        }

        /// <summary>
        /// 检查是否需要补件
        /// </summary>
        /// <param name="appId"></param>
        /// <param name="nrId"></param>
        /// <returns></returns>
        public bool CheckNrNeedSD(long appId, long nrId)
        {
            //需要补件的状态
            List<string> lstStatus = Global.GlobalSetting.Order_SD_Status_Need.Keys.ToList();
            //只读取还在补件期限内，并且没有补过的件
            //var nrList = NrListService.Find(n => n.APP_ID == appId
            //    && lstStatus.Contains(n.NR_STATUS)
            //    && n.ID == nrId
            //    && n.NR_DATE_SUBMIT_FIRST == null
            //);
            //增加查询条件 nr_seq
            var nrList =
                from n in NrListService.Find(n => n.APP_ID == appId
                    && lstStatus.Contains(n.NR_STATUS)
                    && n.ID == nrId
                    && n.NR_DATE_SUBMIT_FIRST == null)
                join a in AppmainService.Find(a => a.ID == appId) on n.APP_ID equals a.ID
                where n.NR_SEQ == a.NR_SEQ
                select n;


            if (nrList == null)
                return false;
            else
                return nrList.Any();
        }

        /// <summary>
        /// 检查是否需要补件
        /// </summary>
        /// <param name="appId"></param>
        /// <param name="fileType"></param>
        /// <returns></returns>
        public bool CheckNrNeedSD(long appId, string fileType)
        {
            //需要补件的状态
            List<string> lstStatus = Global.GlobalSetting.Order_SD_Status_Need.Keys.ToList();
            //只读取还在补件期限内，并且没有补过的件
            //var nrList = NrListService.Find(n => n.APP_ID == appId
            //    && lstStatus.Contains(n.NR_STATUS)
            //    && n.NR_CODE == fileType
            //    && n.NR_DATE_SUBMIT_FIRST == null
            //);
            //增加查询条件 nr_seq
            var nrList =
                from n in NrListService.Find(n => n.APP_ID == appId
                    && lstStatus.Contains(n.NR_STATUS)
                    && n.NR_CODE == fileType
                    && n.NR_DATE_SUBMIT_FIRST == null)
                join a in AppmainService.Find(a => a.ID == appId) on n.APP_ID equals a.ID
                where n.NR_SEQ == a.NR_SEQ
                select n;

            if (nrList == null)
                return false;
            else
                return nrList.ToList().Count > 0;
        }

        /// <summary>
        /// 检查是否需要补件
        /// </summary>
        /// <param name="appId"></param>
        /// <param name="nrId"></param>
        /// <param name="fileType"></param>
        /// <returns></returns>
        public bool CheckNrNeedSD(long appId, long nrId, string fileType)
        {
            //需要补件的状态
            List<string> lstStatus = Global.GlobalSetting.Order_SD_Status_Need.Keys.ToList();
            //只读取还在补件期限内，并且没有补过的件
            //var nrList = NrListService.Find(n => n.APP_ID == appId
            //    && lstStatus.Contains(n.NR_STATUS)
            //    && n.ID == nrId
            //    && n.NR_CODE == fileType
            //    && n.NR_DATE_SUBMIT_FIRST == null
            //);
            //增加查询条件 nr_seq
            var nrList =
                from n in NrListService.Find(n => n.APP_ID == appId
                    && lstStatus.Contains(n.NR_STATUS)
                    && n.ID == nrId && n.NR_CODE == fileType
                    && n.NR_DATE_SUBMIT_FIRST == null)
                join a in AppmainService.Find(a => a.ID == appId) on n.APP_ID equals a.ID
                where n.NR_SEQ == a.NR_SEQ
                select n;
            if (nrList == null)
                return false;
            else
                return nrList.Any();
        }

        /// <summary>
        /// 检查需要补件的项是否都已经补
        /// </summary>
        /// <param name="appId"></param>
        /// <returns></returns>
        public string CheckSDUploaded(long appId)
        {
            List<APP_NR_LIST> nrList = GetNRList(appId);
            if (nrList == null)
            {
                return string.Empty;
            }
            var list = nrList.Where(n => n.NR_DATE_UPDATE == null);
            if (list != null && list.Any())
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("还有{0}项补件未完成，请补充：", list.Count().ToString());
                foreach (APP_NR_LIST anl in list)
                {
                    sb.AppendFormat("<br />{0}", DataDicService.GetDICNameByCode(anl.NR_CODE));
                }
                return sb.ToString();
            }
            return string.Empty;
        }

        /// <summary>
        /// 更新有效的补件队列的更新事件
        /// </summary>
        /// <param name="appId"></param>
        /// <param name="nrListId"></param>
        /// <param name="clearUpdateTime"></param>
        /// <returns></returns>
        public bool UpdateNrListUpdateTime(long appId, long nrListId, bool clearUpdateTime)
        {
            try
            {
                //var query = NrListService.Find(n => n.ID == nrListId && n.APP_ID == appId);
                //增加查询条件 nr_seq
                var query =
                    from n in NrListService.Find(n => n.ID == nrListId && n.APP_ID == appId)
                    join a in AppmainService.Find(m => m.ID == appId) on n.APP_ID equals a.ID
                    where n.NR_SEQ == a.NR_SEQ
                    select n;
                if (query != null && query.Any())
                {
                    APP_NR_LIST anl = query.First();
                    if (clearUpdateTime)
                    {
                        anl.NR_DATE_UPDATE = null;
                        NrListService.Update(anl);
                        NrListService.UnitOfWork.SaveChanges();
                        Infrastructure.Log4Net.LogWriter.Biz("删除了补件队列中此[ID]对应的补件需求的已补文件。", appId.ToString(), nrListId);
                    }
                    else
                    {
                        anl.NR_DATE_UPDATE = DateTime.Now;
                        NrListService.Update(anl);
                        NrListService.UnitOfWork.SaveChanges();
                        Infrastructure.Log4Net.LogWriter.Biz("补件队列中此[ID]对应的补件需求已开始补件。", appId.ToString(), nrListId);
                    }
                }
            }
            catch
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 更改有效的补件队列的更新时间
        /// </summary>
        /// <param name="appId"></param>
        /// <param name="nrListId"></param>
        /// <param name="clearUpdateTime"></param>
        /// <returns></returns>
        public bool UpdateNrListUpdateTime(long appId, string fileType, bool clearUpdateTime)
        {
            try
            {
                //var query = NrListService.Find(n => n.NR_CODE == fileType && n.APP_ID == appId && n.NR_DATE_SUBMIT_FIRST == null);
                //增加查询条件 nr_seq
                var query =
                    from n in NrListService.Find(n => n.NR_CODE == fileType && n.APP_ID == appId && n.NR_DATE_SUBMIT_FIRST == null)
                    join a in AppmainService.Find(m => m.ID == appId) on n.APP_ID equals a.ID
                    where n.NR_SEQ == a.NR_SEQ
                    select n;
                if (query != null && query.Any())
                {
                    APP_NR_LIST anl = query.First();
                    if (clearUpdateTime)
                    {
                        anl.NR_DATE_UPDATE = null;
                        NrListService.Update(anl);
                        NrListService.UnitOfWork.SaveChanges();
                        Infrastructure.Log4Net.LogWriter.Biz("删除了补件队列中此[类型]对应的补件需求的已补文件。", appId.ToString(), fileType);
                    }
                    else
                    {
                        anl.NR_DATE_UPDATE = DateTime.Now;
                        NrListService.Update(anl);
                        NrListService.UnitOfWork.SaveChanges();
                        Infrastructure.Log4Net.LogWriter.Biz("补件队列中此[类型]对应的补件需求已开始补件。", appId.ToString(), fileType);
                    }
                }
            }
            catch
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 补件完成更新相关各表状态
        /// </summary>
        /// <param name="appId"></param>
        /// <returns></returns>
        public string UpdateSDStatusOK(long appId)
        {
            string err = string.Empty;
            try
            {
                err = "更新进件状态时出错！补件提交失败！";
                string strStatus = string.Empty;
                APP_MAIN main = AppmainService.First(a => a.ID == appId);
                if (main != null)
                {
                    main.CHANGED_USER = UserService.GetCurrentUser().Account;
                    main.CHANGED_TIME = DateTime.Now;
                    switch ((EnterStatusType)Enum.Parse(typeof(EnterStatusType), main.APP_STATUS))
                    {
                        case EnterStatusType.SDENTRYWT:
                        case EnterStatusType.SDENTRYING:
                            {
                                strStatus = main.APP_NR_SD_STATUS = main.APP_STATUS = EnterStatusType.SDENTRYOK.ToString();
                                main.STATUS_PHASE = ApproveFlagType.PHASE_ENTRY_SD_OK.ToString();
                            }
                            break;
                        case EnterStatusType.SDAPPRWT:
                        case EnterStatusType.SDAPPRING:
                            {
                                strStatus = main.APP_APPR1_SD_STATUS = main.APP_STATUS = EnterStatusType.SDAPPROK.ToString();
                                main.STATUS_PHASE = ApproveFlagType.PHASE_ENTRY_APPR_OK.ToString();
                            }
                            break;
                        default: break;
                    }
                    AppmainService.Update(main);
                    Infrastructure.Log4Net.LogWriter.Biz(string.Format("此[ID]的进件补件完毕[{0}]，更新主表状态。", strStatus), appId.ToString());

                    err = "更新补件序列状态时出错！补件提交失败！";
                    //只更新还在补件期限内，并且没有补过的件
                    //增加查询条件 nr_seq
                    var query =
                        from n in NrListService.Find(n => n.APP_ID == appId
                            && n.NR_DATE_SUBMIT_FIRST == null)
                        join a in AppmainService.Find(a => a.ID == appId) on n.APP_ID equals a.ID
                        where n.NR_SEQ == a.NR_SEQ
                        select n;
                    //List<APP_NR_LIST> queryNr = NrListService.Find(n => n.APP_ID == appId
                    //    && n.NR_DATE_SUBMIT_FIRST == null).ToList();
                    List<APP_NR_LIST> queryNr = query.ToList();

                    if (queryNr.Count() > 0)
                    {
                        foreach (APP_NR_LIST anl in queryNr)
                        {
                            anl.NR_DATE_SUBMIT_FIRST = DateTime.Now;
                        }
                        NrListService.UpdateMultiple(queryNr);
                    }
                    //提交事务
                    AppmainService.UnitOfWork.SaveChanges();
                    Infrastructure.Log4Net.LogWriter.Biz(string.Format("此[ID]的进件补件完毕[{0}]，更新补件队列。", strStatus), appId.ToString());
                }

                //补件历史状态不加入事务
                err = "更新补件历史状态时出错！但补件提交成功！";
                var queueQuery = AppqueueService.Find(q => q.APP_ID == appId);
                if (queueQuery != null && queueQuery.Count() > 0)
                {
                    APP_QUEUE queue = queueQuery.First();
                    if (queue != null)
                    {
                        //补件完成，将QUEUE_ORDER设置为1。Modified by Liuys
                        queue.QUEUE_ORDER = 1;

                        /***准备写APP_QUEUE_LOG****/
                        APPQueueLog appQueueLog = new APPQueueLog(true)
                        {
                            AGE = queue.AGE,
                            APP_CODE = queue.APP_CODE,
                            APPLY_AMT = queue.APPLY_AMT,
                            AREA_CODE = queue.AREA_CODE,
                            AREA_NAME = queue.AREA_NAME,
                            CHANGED_TIME = DateTime.Now,
                            ENTRY_NAME = queue.ENTRY_NAME,
                            CHANGED_USER = UserService.GetCurrentUser().Account,
                            COM_NAME = queue.COM_NAME,
                            CONTRACT_AMT = queue.CONTRACT_AMT,
                            CREATED_TIME = queue.CREATED_TIME,
                            CREATED_USER = queue.CREATED_USER,
                            CSAD_NAME = queue.CSAD_NAME,
                            CSAD_NO = queue.CSAD_NO,
                            DATE_APPLY = queue.DATE_APPLY,
                            DCOUT_VIP = queue.DCOUT_VIP,
                            DECISION_CODE = queue.DECISION_CODE,
                            DECISION_NAME = queue.DECISION_NAME,
                            DECISION_NO_FINAL = queue.DECISION_NO_FINAL,
                            DECISION_NO_FIRST = queue.DECISION_NO_FIRST,
                            ENTRY_NO = queue.ENTRY_NO,
                            ES_DECISION = queue.ES_DECISION,
                            FRAUD_NO = queue.FRAUD_NO,
                            GENDER = queue.GENDER,
                            ID_NO = queue.ID_NO,
                            ID_TYPE = queue.ID_TYPE,
                            IS_REPEAT = queue.IS_REPEAT,
                            LOAN_AMT = queue.LOAN_AMT,
                            LOGO = queue.LOGO,
                            MANUAL_DECISION = queue.MANUAL_DECISION,
                            QUEUE_ID = queue.ID,
                            NAME = queue.NAME,
                            PAY_TYPE = queue.PAY_TYPE,
                            PRODUCT_CODE = queue.PRODUCT_CODE,
                            PRODUCT_NAME = queue.PRODUCT_NAME,
                            QUEUE_ORDER = queue.QUEUE_ORDER,
                            SALES_NAME = queue.SALES_NAME,
                            SALES_NO = queue.SALES_NO,
                            STORE_CODE = queue.STORE_CODE,
                            STORE_NAME = queue.STORE_NAME,
                            TERMS = queue.TERMS,
                            APP_APPR1_SD_STATUS = queue.APP_APPR1_SD_STATUS,
                            APP_ID = queue.APP_ID,
                            APP_STATUS_DONE = queue.APP_STATUS_DONE,
                            APPLY_CITY_CODE = queue.APPLY_CITY_CODE,
                            APPROVE_NAME = queue.APPROVE_NAME,
                            CUSTOMERTYPE = queue.CUSTOMERTYPE,
                            DCOUT_IS_AUTO_FINAL = queue.DCOUT_IS_AUTO_FINAL,
                            DECISION_NO_HIST = queue.DECISION_NO_HIST,
                            DES_RUNNING_TAG = queue.DES_RUNNING_TAG,
                            ENTRY_ORG_CODE = queue.ENTRY_ORG_CODE,
                            ENTRYORGNAME = queue.ENTRYORGNAME,
                            FRAUD_NAME = queue.FRAUD_NAME,
                            IS_AUDIT_APPROVE_FLAG = queue.IS_AUDIT_APPROVE_FLAG,
                            IS_AUDIT_FLAG = queue.IS_AUDIT_FLAG,
                            IS_AUDO_FINAL = queue.IS_AUDO_FINAL,
                            IS_FINAL = queue.IS_FINAL
                        };
                        APPQueueLog appQueueLog2 = new APPQueueLog(true);
                        long id2 = appQueueLog2.ID;
                        appQueueLog2 = appQueueLog.Clone() as APPQueueLog;
                        appQueueLog2.ID = id2;
                        appQueueLog2.CHANGED_TIME = DateTime.Now.AddSeconds(1);

                        /********/

                        queue.CHANGED_USER = UserService.GetCurrentUser().Account;
                        queue.CHANGED_TIME = DateTime.Now;
                        switch ((EnterStatusType)Enum.Parse(typeof(EnterStatusType), queue.APP_STATUS))
                        {
                            //目前我们待补件和补件中一起处理的
                            case EnterStatusType.SDENTRYWT:
                            case EnterStatusType.SDENTRYING:
                                {
                                    appQueueLog.APP_STATUS = EnterStatusType.SDENTRYING.ToString();
                                    appQueueLog.DO_ACTION = GlobalSetting.SD_DO_ACTION[EnterStatusType.SDENTRYWT.ToString()];
                                    appQueueLog.APP_NR_SD_STATUS = EnterStatusType.SDENTRYING.ToString();
                                    appQueueLog.STATUS_PHASE = ApproveFlagType.PHASE_ENTRY_SD.ToString();

                                    appQueueLog2.APP_STATUS = EnterStatusType.SDENTRYOK.ToString();
                                    appQueueLog2.DO_ACTION = GlobalSetting.SD_DO_ACTION[EnterStatusType.SDENTRYING.ToString()];
                                    appQueueLog2.APP_NR_SD_STATUS = EnterStatusType.SDENTRYOK.ToString();
                                    appQueueLog2.STATUS_PHASE = ApproveFlagType.PHASE_ENTRY_SD_OK.ToString();

                                    queue.DO_ACTION = GlobalSetting.SD_DO_ACTION[EnterStatusType.SDENTRYING.ToString()];
                                    queue.APP_NR_SD_STATUS = EnterStatusType.SDENTRYOK.ToString();
                                    queue.APP_STATUS = EnterStatusType.SDENTRYOK.ToString();
                                    queue.STATUS_PHASE = ApproveFlagType.PHASE_ENTRY_SD_OK.ToString();
                                }
                                break;
                            case EnterStatusType.SDAPPRWT:
                            case EnterStatusType.SDAPPRING:
                                {
                                    appQueueLog.APP_STATUS = EnterStatusType.SDAPPRING.ToString();
                                    appQueueLog.DO_ACTION = GlobalSetting.SD_DO_ACTION[EnterStatusType.SDAPPRWT.ToString()];
                                    appQueueLog.APP_NR_SD_STATUS = EnterStatusType.SDAPPRING.ToString();
                                    appQueueLog.STATUS_PHASE = ApproveFlagType.PHASE_ENTRY_APPR.ToString();

                                    appQueueLog2.APP_STATUS = EnterStatusType.SDAPPROK.ToString();
                                    appQueueLog2.DO_ACTION = GlobalSetting.SD_DO_ACTION[EnterStatusType.SDAPPRING.ToString()];
                                    appQueueLog2.APP_NR_SD_STATUS = EnterStatusType.SDAPPROK.ToString();
                                    appQueueLog2.STATUS_PHASE = ApproveFlagType.PHASE_ENTRY_APPR_OK.ToString();

                                    queue.DO_ACTION = GlobalSetting.SD_DO_ACTION[EnterStatusType.SDAPPRING.ToString()];
                                    queue.APP_APPR1_SD_STATUS = EnterStatusType.SDAPPROK.ToString();
                                    queue.APP_STATUS = EnterStatusType.SDAPPROK.ToString();
                                    queue.STATUS_PHASE = ApproveFlagType.PHASE_ENTRY_APPR_OK.ToString();
                                }
                                break;
                            default: break;
                        }
                        AppqueueService.Update(queue);
                        AppqueueService.UnitOfWork.SaveChanges();

                        //写APP_QUEUE_LOG
                        QueueService.AddAppQueueLog(appQueueLog);
                        QueueService.AddAppQueueLog(appQueueLog2);

                        Infrastructure.Log4Net.LogWriter.Biz(string.Format("此[ID]的进件补件完毕[{0}]，写入补件历史。", strStatus), appId.ToString());
                    }
                }
            }
            catch
            {
                return err;
            }
            return string.Empty;
        }

        /// <summary>
        /// 车贷补件完成，各表状态更新
        /// </summary>
        /// <param name="appId"></param>
        /// <returns></returns>
        public string UpdateSDStatusOKCar(long appId)
        {
            string err = string.Empty;
            try
            {
                err = "更新进件状态时出错！补件提交失败！";
                string strStatus = string.Empty;
                APP_MAIN main = AppmainService.First(a => a.ID == appId);
                if (main != null)
                {
                    main.CHANGED_USER = UserService.GetCurrentUser().Account;
                    main.CHANGED_TIME = DateTime.Now;

                    //update 20160518 车贷补件现分为四种情况，分别是 录入中，初审待补件，零售信贷专员待补，件贷后管理专员待补件，对应的后续状态也为四种
                    switch ((EnterStatusType)Enum.Parse(typeof(EnterStatusType), main.APP_STATUS))
                    {
                        //录入待补件
                        case EnterStatusType.CARENTRYPATCH: 
                            strStatus =
                                main.APP_APPR1_SD_STATUS = main.APP_NR_SD_STATUS = main.APP_STATUS = EnterStatusType.CARENTRYING.ToString();
                            break;
                        //初审待补件
                        case EnterStatusType.CARAPPRWT:
                            strStatus = main.APP_APPR1_SD_STATUS = main.APP_NR_SD_STATUS = main.APP_STATUS = EnterStatusType.CARAPPROK.ToString();
                            break;
                        //零售信贷员待补件
                        case EnterStatusType.CARCAPPRWT:
                            strStatus = main.APP_APPR1_SD_STATUS = main.APP_NR_SD_STATUS = main.APP_STATUS = EnterStatusType.CARCAPPROK.ToString();
                            break;
                        //贷后管理专员待补件
                        case EnterStatusType.CARPLMSAPPRWT:
                            strStatus = main.APP_APPR1_SD_STATUS = main.APP_NR_SD_STATUS = main.APP_STATUS = EnterStatusType.CARPLMSAPPROK.ToString();
                            break;
                        default: break;
                    }

                    AppmainService.Update(main);
                    Infrastructure.Log4Net.LogWriter.Biz(string.Format("此[ID]的进件补件（车贷）完毕[{0}]，更新主表状态。", strStatus), appId.ToString());

                    err = "更新补件序列状态时出错！补件提交失败！";
                    //只更新还在补件期限内，并且没有补过的件
                    //增加查询条件 nr_seq
                    var query =
                        from n in NrListService.Find(n => n.APP_ID == appId
                            && n.NR_DATE_SUBMIT_FIRST == null)
                        join a in AppmainService.Find(a => a.ID == appId) on n.APP_ID equals a.ID
                        where n.NR_SEQ == a.NR_SEQ
                        select n;
                    //List<APP_NR_LIST> queryNr = NrListService.Find(n => n.APP_ID == appId
                    //    && n.NR_DATE_SUBMIT_FIRST == null).ToList();
                    List<APP_NR_LIST> queryNr = query.ToList();
                    if (queryNr.Count() > 0)
                    {
                        foreach (APP_NR_LIST anl in queryNr)
                        {
                            anl.NR_DATE_SUBMIT_FIRST = DateTime.Now;
                        }
                        NrListService.UpdateMultiple(queryNr);
                    }
                    //提交事务
                    AppmainService.UnitOfWork.SaveChanges();
                    Infrastructure.Log4Net.LogWriter.Biz(string.Format("此[ID]的进件补件（车贷）完毕[{0}]，更新补件队列。", strStatus), appId.ToString());
                }

                //补件历史状态不加入事务
                err = "更新补件历史状态时出错！但补件提交成功！";
                var queueQuery = AppqueueService.Find(q => q.APP_ID == appId);
                if (queueQuery != null && queueQuery.Count() > 0)
                {
                    APP_QUEUE queue = queueQuery.First();
                    if (queue != null)
                    {
                        //补件完成，将QUEUE_ORDER设置为1。Modified by Liuys
                        queue.QUEUE_ORDER = 1;

                        /***准备写APP_QUEUE_LOG****/
                        APPQueueLog appQueueLog = new APPQueueLog(true)
                        {
                            AGE = queue.AGE,
                            APP_CODE = queue.APP_CODE,
                            APPLY_AMT = queue.APPLY_AMT,
                            AREA_CODE = queue.AREA_CODE,
                            AREA_NAME = queue.AREA_NAME,
                            CHANGED_TIME = DateTime.Now,
                            ENTRY_NAME = queue.ENTRY_NAME,
                            CHANGED_USER = UserService.GetCurrentUser().Account,
                            COM_NAME = queue.COM_NAME,
                            CONTRACT_AMT = queue.CONTRACT_AMT,
                            CREATED_TIME = queue.CREATED_TIME,
                            CREATED_USER = queue.CREATED_USER,
                            CSAD_NAME = queue.CSAD_NAME,
                            CSAD_NO = queue.CSAD_NO,
                            DATE_APPLY = queue.DATE_APPLY,
                            DCOUT_VIP = queue.DCOUT_VIP,
                            DECISION_CODE = queue.DECISION_CODE,
                            DECISION_NAME = queue.DECISION_NAME,
                            DECISION_NO_FINAL = queue.DECISION_NO_FINAL,
                            DECISION_NO_FIRST = queue.DECISION_NO_FIRST,
                            ENTRY_NO = queue.ENTRY_NO,
                            ES_DECISION = queue.ES_DECISION,
                            FRAUD_NO = queue.FRAUD_NO,
                            GENDER = queue.GENDER,
                            ID_NO = queue.ID_NO,
                            ID_TYPE = queue.ID_TYPE,
                            IS_REPEAT = queue.IS_REPEAT,
                            LOAN_AMT = queue.LOAN_AMT,
                            LOGO = queue.LOGO,
                            MANUAL_DECISION = queue.MANUAL_DECISION,
                            QUEUE_ID = queue.ID,
                            NAME = queue.NAME,
                            PAY_TYPE = queue.PAY_TYPE,
                            PRODUCT_CODE = queue.PRODUCT_CODE,
                            PRODUCT_NAME = queue.PRODUCT_NAME,
                            QUEUE_ORDER = queue.QUEUE_ORDER,
                            SALES_NAME = queue.SALES_NAME,
                            SALES_NO = queue.SALES_NO,
                            STORE_CODE = queue.STORE_CODE,
                            STORE_NAME = queue.STORE_NAME,
                            TERMS = queue.TERMS,
                            APP_APPR1_SD_STATUS = queue.APP_APPR1_SD_STATUS,
                            APP_ID = queue.APP_ID,
                            APP_STATUS_DONE = queue.APP_STATUS_DONE,
                            APPLY_CITY_CODE = queue.APPLY_CITY_CODE,
                            APPROVE_NAME = queue.APPROVE_NAME,
                            CUSTOMERTYPE = queue.CUSTOMERTYPE,
                            DCOUT_IS_AUTO_FINAL = queue.DCOUT_IS_AUTO_FINAL,
                            DECISION_NO_HIST = queue.DECISION_NO_HIST,
                            DES_RUNNING_TAG = queue.DES_RUNNING_TAG,
                            ENTRY_ORG_CODE = queue.ENTRY_ORG_CODE,
                            ENTRYORGNAME = queue.ENTRYORGNAME,
                            FRAUD_NAME = queue.FRAUD_NAME,
                            IS_AUDIT_APPROVE_FLAG = queue.IS_AUDIT_APPROVE_FLAG,
                            IS_AUDIT_FLAG = queue.IS_AUDIT_FLAG,
                            IS_AUDO_FINAL = queue.IS_AUDO_FINAL,
                            IS_FINAL = queue.IS_FINAL
                        };
                        /********/

                        queue.CHANGED_USER = UserService.GetCurrentUser().Account;
                        queue.CHANGED_TIME = DateTime.Now;
                        if (GlobalSetting.Order_SD_Status_Need_Car.Keys.Contains(queue.APP_STATUS))
                        {
                            queue.APP_STATUS = strStatus;
                            appQueueLog.APP_STATUS = strStatus;
                        }

                        AppqueueService.Update(queue);
                        AppqueueService.UnitOfWork.SaveChanges();

                        //写APP_QUEUE_LOG
                        QueueService.AddAppQueueLog(appQueueLog);

                        Infrastructure.Log4Net.LogWriter.Biz(string.Format("此[ID]的进件补件（车贷）完毕[{0}]，写入补件历史。", strStatus), appId.ToString());
                    }
                }
            }
            catch
            {
                return err;
            }
            return string.Empty;
        }

        /// <summary>
        /// 房贷补件完成，对各表状态进行更新
        /// </summary>
        /// <param name="appId"></param>
        /// <returns></returns>
        public string UpdateSDStatusOKHouse(long appId)
        {
            string err = string.Empty;
            try
            {
                err = "更新进件状态时出错！补件提交失败！";
                string strStatus = string.Empty;
                APP_MAIN main = AppmainService.First(a => a.ID == appId);
                if (main != null)
                {
                    main.CHANGED_USER = UserService.GetCurrentUser().Account;
                    main.CHANGED_TIME = DateTime.Now;
                    switch ((EnterStatusType)Enum.Parse(typeof(EnterStatusType), main.APP_STATUS))
                    {
                        case EnterStatusType.HOUSESDAPPRWT:
                            {
                                strStatus = main.APP_NR_SD_STATUS = main.APP_STATUS = EnterStatusType.HOUSESDAPPROK.ToString();
                            }
                            break;
                        case EnterStatusType.HOUSESDFAPPRWT:
                            {
                                strStatus = main.APP_APPR1_SD_STATUS = main.APP_STATUS = EnterStatusType.HOUSESDFAPPROK.ToString();
                            }
                            break;
                        default: break;
                    }
                    AppmainService.Update(main);
                    Infrastructure.Log4Net.LogWriter.Biz(string.Format("此[ID]的进件补件（房贷）完毕[{0}]，更新主表状态。", strStatus), appId.ToString());

                    err = "更新补件序列状态时出错！补件提交失败！";
                    //只更新还在补件期限内，并且没有补过的件
                    //增加查询条件 nr_seq
                    var query =
                        from n in NrListService.Find(n => n.APP_ID == appId
                            && n.NR_DATE_SUBMIT_FIRST == null)
                        join a in AppmainService.Find(a => a.ID == appId) on n.APP_ID equals a.ID
                        where n.NR_SEQ == a.NR_SEQ
                        select n;
                    //List<APP_NR_LIST> queryNr = NrListService.Find(n => n.APP_ID == appId
                    //    && n.NR_DATE_SUBMIT_FIRST == null).ToList();
                    List<APP_NR_LIST> queryNr = query.ToList();

                    if (queryNr.Count() > 0)
                    {
                        foreach (APP_NR_LIST anl in queryNr)
                        {
                            anl.NR_DATE_SUBMIT_FIRST = DateTime.Now;
                        }
                        NrListService.UpdateMultiple(queryNr);
                    }
                    //提交事务
                    AppmainService.UnitOfWork.SaveChanges();
                    Infrastructure.Log4Net.LogWriter.Biz(string.Format("此[ID]的进件补件（房贷）完毕[{0}]，更新补件队列。", strStatus), appId.ToString());
                }

                //补件历史状态不加入事务
                err = "更新APP_QUEUE状态时出错！但补件提交成功！";
                var queueQuery = AppqueueService.Find(q => q.APP_ID == appId);
                if (queueQuery != null && queueQuery.Count() > 0)
                {
                    APP_QUEUE queue = queueQuery.First();
                    if (queue != null)
                    {
                        //补件完成，将QUEUE_ORDER设置为1。Modified by Liuys
                        queue.QUEUE_ORDER = 1;

                        /***准备写APP_QUEUE_LOG****/
                        //经协商，房贷这里不写APP_QUEUE_LOG了，由审核系统写
                        /********/

                        queue.CHANGED_USER = UserService.GetCurrentUser().Account;
                        queue.CHANGED_TIME = DateTime.Now;
                        switch ((EnterStatusType)Enum.Parse(typeof(EnterStatusType), queue.APP_STATUS))
                        {
                            //目前我们待补件和补件中一起处理的
                            case EnterStatusType.HOUSESDAPPRWT:
                                {
                                    queue.APP_STATUS = EnterStatusType.HOUSESDAPPROK.ToString();
                                }
                                break;
                            case EnterStatusType.HOUSESDFAPPRWT:
                                {
                                    queue.APP_STATUS = EnterStatusType.HOUSESDFAPPROK.ToString();
                                }
                                break;
                            default: break;
                        }
                        AppqueueService.Update(queue);
                        AppqueueService.UnitOfWork.SaveChanges();

                        Infrastructure.Log4Net.LogWriter.Biz(string.Format("此[ID]的进件补件（房贷）完毕[{0}]，更改APP_QUEUE状态。", strStatus), appId.ToString());
                    }
                }
            }
            catch
            {
                return err;
            }
            return string.Empty;
        }

        /// <summary>
        /// 根据进件状态查询申请单数目
        /// </summary>
        /// <param name="lstEnterStatus"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public int GetCountEnterByStatus(List<EnterStatusType> lstEnterStatus, string user)
        {
            if (lstEnterStatus == null || lstEnterStatus.Count == 0)
            {
                return VappmainService.Count(a => a.CSADNO == user);
            }
            else
            {
                List<string> lstStatus = new List<string>();
                foreach (EnterStatusType est in lstEnterStatus)
                {
                    lstStatus.Add(est.ToString());
                }
                return VappmainService.Count(a => a.CSADNO == user && lstStatus.Contains(a.APPSTATUS));
            }
        }

        /// <summary>
        /// 取得补件申请时间
        /// </summary>
        /// <param name="appId"></param>
        /// <param name="fileType"></param>
        /// <returns></returns>
        public DateTime? GetNrDateApply(long appId, string fileType)
        {
            try
            {
                //var query = NrListService.Find(n => n.NR_CODE == fileType && n.APP_ID == appId && n.NR_DATE_SUBMIT_FIRST == null);
                //增加查询条件 nr_seq
                var query =
                    from n in NrListService.Find(n => n.NR_CODE == fileType && n.APP_ID == appId && n.NR_DATE_SUBMIT_FIRST == null)
                    join a in AppmainService.Find(c => c.ID == appId) on n.APP_ID equals a.ID
                    where a.NR_SEQ == n.NR_SEQ
                    select n;

                if (query != null && query.Any())
                {
                    APP_NR_LIST anl = query.First();
                    return anl.NR_DATE_APPLY;
                }
                else
                {
                    return null;
                }
            }
            catch
            {
                return null;
            }
        }
    }
}
