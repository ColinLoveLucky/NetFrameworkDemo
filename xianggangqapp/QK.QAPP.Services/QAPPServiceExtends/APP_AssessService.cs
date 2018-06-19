/***********************
 * 作    者：刘云松
 * 创建时间：‎‎2015-3-20 14:22:07
 * 作    用：提供（车贷）评估相关操作
*****************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QK.QAPP.Entity;
using QK.QAPP.IServices;
using Microsoft.Practices.Unity;
using QK.QAPP.Infrastructure;
using QK.QAPP.Global;
using QK.QAPP.Infrastructure.MessageQueue;

namespace QK.QAPP.Services
{
    public class APP_AssessService : IAPP_AssessService
    {
        #region 属性

        /// <summary>
        /// 待评估状态
        /// </summary>
        public Dictionary<string, string> AssessQueueStatus
        {
            get { return GlobalSetting.AssessQueueStatus; }
        }

        /// <summary>
        /// 需要评估的车贷logo
        /// </summary>
        public List<string> NeedAssessProductLogo
        {
            get
            {
                return GlobalSetting.NeedAssessProductLogo;
            }
        }


        [Dependency]
        public IAPP_QUEUE_ASSESSSERVICE QueueAssessService { get; set; }

        [Dependency]
        public IQFUserService UserService { get; set; }

        [Dependency]
        public IPAD_CARJUDGESERVICE carJudgeService { get; set; }

        [Dependency]
        public IApplyTableService applyTabelService { get; set; }

        [Dependency]
        public IAPP_QUEUESERVICE queueService { get; set; }

        [Dependency]
        public IAPP_MAINSERVICE mainService { get; set; }


        [Dependency]
        public IAPP_MSGBOXSERVICE MsgBoxService { get; set; }

        #endregion
        public AssessQueueViewFieldList GetAssessListByMainStatus(AssessListSearchPara para)
        {
            //返回值
            AssessQueueViewFieldList assessList = new AssessQueueViewFieldList();
            //未授权访问任何资料
            if (para.AccessableCsac.Count == 0)
            {
                return assessList;
            }

            //将查询条件的进件状态转换由枚举转换成字符串
            List<string> lstStatus = new List<string>();
            foreach (AssessStatusType s in para.ListAssessStatus)
            {
                lstStatus.Add(s.ToString().ToUpper());
            }
            IQueryable<APP_QUEUE_ASSESS> query = null;
            if (para.FuzzySearch)
            {
                query = QueueAssessService.Find(a =>
                    (
                        (a.APP_CODE.IndexOf(para.AppCode) > -1 || string.IsNullOrEmpty(para.AppCode))
                        ||
                        (a.CUSTOMER_NAME.IndexOf(para.CustomerName) > -1 || string.IsNullOrEmpty(para.CustomerName))
                        )
                    && (para.ListLogo.Contains(a.PRODUCT_LOGO) || para.ListLogo.Count == 0)
                    && lstStatus.Contains(a.ASSESS_STATUS.ToUpper()));

            }
            else
            {
                query = QueueAssessService.Find(a =>
                            (a.APP_CODE.IndexOf(para.AppCode) > -1 || string.IsNullOrEmpty(para.AppCode))
                                // &&(a.PRODUCT_LOGO == "GPS")
                            && (a.CUSTOMER_NAME.IndexOf(para.CustomerName) > -1 || string.IsNullOrEmpty(para.CustomerName))

                            && (
                                (a.SALES_CODE.IndexOf(para.SaleCode) > -1 || string.IsNullOrEmpty(para.SaleCode))
                                ||
                                (a.SALES_NAME.IndexOf(para.SaleName) > -1 || string.IsNullOrEmpty(para.SaleName))
                               )
                            && (para.ListLogo.Contains(a.PRODUCT_LOGO) || para.ListLogo.Count == 0)
                            && lstStatus.Contains(a.ASSESS_STATUS.ToUpper()));


            }

            //联接APP_AUTH表进行查询MenuCode
            if (query != null)
            {
                query = UserService.QueryJoinMenuAuthOnly(query, a => a.APP_ID, o => o.APP_ID, para.InputMenuCode);
            }

            //排序条件
            if (para.Sort.Count > 0)
            {
                foreach (KeyValuePair<string, string> kv in para.Sort)
                {
                    if (kv.Key != "" && kv.Value != "")
                    {
                        query = query.Sort(kv.Key, kv.Value);
                    }
                }
            }
            assessList.SetParameters(query, para);
            return assessList;
        }

        /// <summary>
        /// 修改评估师
        /// </summary>
        /// <param name="appCode">APP_CODE</param>
        /// <param name="appCode">评估师账号</param>
        /// <param name="valuatorName">评估师姓名</param>
        /// <returns>ret 是否成功</returns>
        public bool UpdateValuatorByAppCode(string appCode, string valuatorCode, string valuatorName)
        {
            bool ret = false;
            string strStatus = AssessStatusType.CarAssessToBeAssess.ToString();
            APP_QUEUE_ASSESS query = QueueAssessService.Find(a => a.APP_CODE == appCode && a.ASSESS_STATUS == strStatus).FirstOrDefault();
            if (query != null)
            {
                query.VALUATOR = valuatorCode;
                query.VALUATOR_NAME = valuatorName;
                query.CHANGED_TIME = System.DateTime.Now;
                query.CHANGED_USER = UserService.GetCurrentUser().Account;
                query.VERSION++;
            }

            ret = QueueAssessService.Update(query);
            QueueAssessService.UnitOfWork.SaveChanges();

            return ret;
        }

        /// <summary>
        /// 查询已批复队列
        /// </summary>
        /// <param name="appCode"></param>
        /// <param name="customerName"></param>
        /// <returns></returns>
        public AssessQueueViewFieldList GetApprovedList(AssessListSearchPara para)
        {
            AssessQueueViewFieldList assessList = new AssessQueueViewFieldList();
            //未授权访问任何资料
            if (para.AccessableCsac.Count == 0)
            {
                return assessList;
            }
            string strStatus = para.ListAssessStatus[0].ToString();

            IQueryable<APP_QUEUE_ASSESS> query = null;
            if (para.FuzzySearch)
            {
                query = QueueAssessService.Find(a =>
                (
                   (string.IsNullOrEmpty(para.AppCode) || a.APP_CODE.IndexOf(para.AppCode) > -1)
                    ||
                   (string.IsNullOrEmpty(para.CustomerName) || a.CUSTOMER_NAME.IndexOf(para.CustomerName) > -1)
                )
                && (para.NeedAssess == null || (
                    (para.NeedAssess.Value && NeedAssessProductLogo.Contains(a.PRODUCT_LOGO))
                    || (!para.NeedAssess.Value && !NeedAssessProductLogo.Contains(a.PRODUCT_LOGO))
                    ))
                && (para.ListLogo.Contains(a.PRODUCT_LOGO) || para.ListLogo.Count == 0)
                && (a.ASSESS_STATUS == strStatus));
            }
            else
            {
                query = QueueAssessService.Find(a =>
                   (string.IsNullOrEmpty(para.AppCode) || a.APP_CODE.IndexOf(para.AppCode) > -1)
                && (string.IsNullOrEmpty(para.CustomerName) || a.CUSTOMER_NAME.IndexOf(para.CustomerName) > -1)
                && (
                        (a.SALES_CODE.IndexOf(para.SaleCode) > -1 || string.IsNullOrEmpty(para.SaleCode))
                        ||
                        (a.SALES_NAME.IndexOf(para.SaleName) > -1 || string.IsNullOrEmpty(para.SaleName))
                    )
                && (para.NeedAssess == null || (
                    (para.NeedAssess.Value && NeedAssessProductLogo.Contains(a.PRODUCT_LOGO))
                    || (!para.NeedAssess.Value && !NeedAssessProductLogo.Contains(a.PRODUCT_LOGO))
                    ))
                && (para.ListLogo.Contains(a.PRODUCT_LOGO) || para.ListLogo.Count == 0)
                && (a.ASSESS_STATUS == strStatus));
            }

            //联接APP_AUTH表进行查询MenuCode
            if (query != null)
            {
                query = UserService.QueryJoinMenuAuthOnly(query, a => a.APP_ID, o => o.APP_ID, para.InputMenuCode);
            }

            //排序条件
            if (para.Sort.Count > 0)
            {
                foreach (KeyValuePair<string, string> kv in para.Sort)
                {
                    if (kv.Key != "" && kv.Value != "")
                    {
                        query = query.Sort(kv.Key, kv.Value);
                    }
                }
            }
            assessList.SetParameters(query, para);
            return assessList;
        }

        /// <summary>
        /// 查询批复队列数据
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="logo"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public APP_QUEUE_ASSESS GetAssessInfo(long ID, string logo, string status)
        {
            return QueueAssessService.FirstOrDefault(a => a.ID == ID && a.PRODUCT_LOGO == logo && (string.IsNullOrEmpty(status) || status == a.ASSESS_STATUS));
        }

        /// <summary>
        /// 查询车况评估详情
        /// </summary>
        /// <param name="appCode"></param>
        /// <returns></returns>
        public PAD_CARJUDGE GetCarJudgeInfo(string appCode)
        {
            return carJudgeService.FirstOrDefault(a => a.APP_CODE == appCode);
        }

        /// <summary>
        /// 更新队列中某条记录
        /// <para>这里直接使用了update，所以不接受自己创建的对象</para>
        /// </summary>
        /// <param name="appAssess"></param>
        public void UpdateAssessInfo(APP_QUEUE_ASSESS appAssess)
        {
            appAssess.VERSION++;
            QueueAssessService.Update(appAssess);
            QueueAssessService.UnitOfWork.SaveChanges();
        }

        /// <summary>
        /// 根据id变更APP_QUEUE_ASSESS的assess_status
        /// </summary>
        /// <param name="id">车贷申请的id</param>
        /// <param name="statusType">变更后的状态</param>
        /// <param name="beforeStatus">变更前的状态</param>
        public bool UpdateAssessStatus(long id, AssessStatusType statusType, string beforeStatus)
        {
            bool error = true;
            APP_QUEUE_ASSESS entity = QueueAssessService.First(a => a.ID == id && a.ASSESS_STATUS == beforeStatus);
            if (entity != null)
            {
                //准备MQ
                MQProducer CAR_Producer = null;
                EnterStatusType appmainAfterStatus = EnterStatusType.CARASSESSOK;
                string assessQueue = String.Empty;
                switch (statusType)
                {
                    case AssessStatusType.CarAssessCustomerReject:
                        {
                            assessQueue = GlobalSetting.CAR_Assess_Cancelled;
                            appmainAfterStatus = EnterStatusType.CARASSESSCANCEL;
                        }
                        break;
                    case AssessStatusType.CarAssessSubmitted:
                        {
                            assessQueue = GlobalSetting.CAR_Assess_Done;
                            appmainAfterStatus = EnterStatusType.CARASSESSOK;
                        }
                        break;
                    default: break;
                }

                //更新APP_QUEUE_ASSESS
                entity.ASSESS_STATUS = statusType.ToString();
                entity.CHANGED_TIME = DateTime.Now;
                entity.CHANGED_USER = UserService.GetCurrentUser().Account;
                entity.VERSION++;

                //现由审核系统更新app_main及app_queue的进件状态
                //更新APP_MAIN
                //string mainBeforeStatus = EnterStatusType.CARASSESSWT.ToString();
                //APP_MAIN mainEntity = mainService.FirstOrDefault(m => m.ID == entity.APP_ID && m.APP_STATUS == mainBeforeStatus);
                //if (mainEntity == null)
                //{
                //    Infrastructure.Log4Net.LogWriter.Biz("变更此[ID]的进件状态（主表）时未找到需更新的数据。", entity.APP_ID.ToString(), new { before = mainBeforeStatus, after = appmainAfterStatus.ToString() });
                //    return false;
                //}
                //mainEntity.APP_STATUS = appmainAfterStatus.ToString();
                //mainEntity.CHANGED_TIME = DateTime.Now;
                //mainEntity.CHANGED_USER = entity.CHANGED_USER;

                //更新APP_QUEUE
                //APP_QUEUE queueEntity = queueService.FirstOrDefault(q => q.APP_ID == entity.APP_ID);
                //if (queueEntity == null)
                //{
                //    Infrastructure.Log4Net.LogWriter.Biz("变更此[ID]的进件状态（QUEUE表）时未找到需更新的数据。", entity.APP_ID.ToString(), new { before = mainBeforeStatus, after = appmainAfterStatus.ToString() });
                //    return false;
                //}
                //queueEntity.APP_STATUS = appmainAfterStatus.ToString();
                //queueEntity.CHANGED_TIME = DateTime.Now;
                //queueEntity.CHANGED_USER = entity.CHANGED_USER;

                //更新MSG_BOX
                var msgs = MsgBoxService.Find(m => m.APPCODE == entity.APP_CODE).ToList();
                foreach (var item in msgs)
                {
                    item.STATUS = MessageStatus.Processed.ToString();
                }

                QueueAssessService.Update(entity);
                //mainService.Update(mainEntity);
                //queueService.Update(queueEntity);
                MsgBoxService.UpdateMultiple(msgs);
                MsgBoxService.UnitOfWork.SaveChanges();

                Infrastructure.Log4Net.LogWriter.Biz(string.Format("变更ID={0}的APP_QUEUE_ASSESS的补件状态", id), entity.APP_ID.ToString(), statusType.ToString());
                //Infrastructure.Log4Net.LogWriter.Biz("变更此[ID]的进件状态（MAIN表）", entity.APP_ID.ToString(), new { before = mainBeforeStatus, after = appmainAfterStatus.ToString() });
                //Infrastructure.Log4Net.LogWriter.Biz("变更此[ID]的进件状态（QUEUE表）", entity.APP_ID.ToString(), new { before = mainBeforeStatus, after = appmainAfterStatus.ToString() });
                Infrastructure.Log4Net.LogWriter.Biz("变更MESSAGEBOX的状态", entity.APP_CODE.ToString(), MessageStatus.Processed.ToString());

                //using (CAR_Producer = new MQProducer(GlobalSetting.MQMultipleServer,
                //                                     GlobalSetting.MQUserName,
                //                                     GlobalSetting.MQUserPassword,
                //                                     assessQueue))
                //{
                //    //发送MQ
                //    if (CAR_Producer != null && CAR_Producer.Publish(entity.APP_CODE))
                //    {
                //        Infrastructure.Log4Net.LogWriter.Biz("MQ用户提交数据成功", entity.APP_ID + "", entity);
                //    }
                //    else
                //    {
                //        Infrastructure.Log4Net.LogWriter.Error("评估完成MQ消息发送失败");
                //        error = false;
                //    }
                //}
                if (MQHelper.Publish(
                    GlobalSetting.MQMultipleServer,
                    GlobalSetting.MQUserName,
                    GlobalSetting.MQUserPassword,
                    assessQueue,
                    entity.APP_CODE))
                {
                    Infrastructure.Log4Net.LogWriter.Biz("MQ用户提交数据成功", entity.APP_ID + "", entity);
                }
                else
                {
                    Infrastructure.Log4Net.LogWriter.Error("评估完成MQ消息发送失败");
                    error = false;
                }

            }
            return error;
        }

        public bool CheckBookTime(APP_QUEUE_ASSESS queueAssess)
        {
            if (queueAssess != null && queueAssess.CUSTOMER_BOOK_TIME.HasValue)
            {
                var dateNow = queueAssess.CUSTOMER_BOOK_TIME.Value.Date;
                var dateTomo = dateNow.AddDays(1).Date;
                //当前评估师当天的预约列表，不包含当前项
                var bookList = QueueAssessService.Find(q =>
                    q.VALUATOR == queueAssess.VALUATOR &&
                    q.ID != queueAssess.ID &&
                    q.CUSTOMER_BOOK_TIME > dateNow &&
                    q.CUSTOMER_BOOK_TIME < dateTomo);

                if (bookList.Any())
                {
                    foreach (var item in bookList)
                    {
                        if (item.CUSTOMER_BOOK_TIME.HasValue)
                        {
                            if ((queueAssess.CUSTOMER_BOOK_TIME.Value - item.CUSTOMER_BOOK_TIME.Value).TotalMinutes < 20)
                                return false;
                        }
                    }
                    return true;
                }
            }
            return true;
        }
    }
}
