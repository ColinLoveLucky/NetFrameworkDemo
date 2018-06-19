/***********************
 * 作    者：刘云松
 * 创建时间：‎2014‎-10‎-‎29 ‏‎18:25:13
 * 作    用：提供补件记录相关的功能
*****************************/
using QK.QAPP.Entity;
using QK.QAPP.IServices;
using QK.QAPP.Infrastructure.Data.EFRepository.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QK.QAPP.Services
{
    public class APPQueueLogService : RepositoryBaseSql, IAPPQueueLogService
    {
        public APPQueueLogService(IUnitOfWork unitOfWork)
            :base(unitOfWork)
        {

        }

        /// <summary>
        /// 添加AppQueueLog
        /// </summary>
        /// <param name="appQueueLog"></param>
        /// <returns></returns>
        public void AddAppQueueLog(APPQueueLog appQueueLog)
        {
            if (appQueueLog == null)
                return;

            StringBuilder sql = new StringBuilder();
            sql.Append(" INSERT INTO APP_QUEUE_LOG");
            sql.Append(" ( ID, QUEUE_ID, APP_CODE, NAME, GENDER, AGE, ID_NO, ID_TYPE, LOGO, PRODUCT_CODE, PRODUCT_NAME, COM_NAME, DCOUT_VIP, IS_REPEAT, DO_ACTION, APPLY_AMT");
            sql.Append(" , CONTRACT_AMT, PAY_TYPE, DATE_APPLY, SALES_NO, SALES_NAME, AREA_CODE, AREA_NAME, STORE_CODE, STORE_NAME, CSAD_NO, CSAD_NAME, ENTRY_NO, ENTRY_NAME");
            sql.Append(" , DECISION_NO_FIRST, DECISION_NAME, MANUAL_DECISION, ES_DECISION, APP_STATUS, CREATED_USER, CREATED_TIME, CHANGED_USER, CHANGED_TIME, DECISION_NO_FINAL");
            sql.Append(" , FRAUD_NO, QUEUE_ORDER, LOAN_AMT, TERMS, DECISION_CODE, LATEST_APP_STATUS, LATEST_ACTION, CUSTOMERTYPE, DECISION_NO_HIST, APPROVE_NAME, FRAUD_NAME");
            sql.Append(" , DCOUT_IS_AUTO_FINAL, APP_ID, APP_STATUS_DONE, APP_NR_SD_STATUS, APP_APPR1_SD_STATUS, DES_RUNNING_TAG, STATUS_ORDER, IS_FINAL, IS_AUDIT_FLAG, IS_AUDIT_APPROVE_FLAG");
             sql.Append(" , IS_AUDO_FINAL, STATUS_PHASE, APPLY_CITY_CODE, ENTRY_ORG_CODE, ENTRYORGNAME)");
            sql.Append(" VALUES");
            sql.Append(" (:ID,:QUEUE_ID,:APP_CODE,:NAME,:GENDER,:AGE,:ID_NO,:ID_TYPE,:LOGO,:PRODUCT_CODE,:PRODUCT_NAME,:COM_NAME,:DCOUT_VIP,:IS_REPEAT,:DO_ACTION,:APPLY_AMT");
            sql.Append(" ,:CONTRACT_AMT,:PAY_TYPE,:DATE_APPLY,:SALES_NO,:SALES_NAME,:AREA_CODE,:AREA_NAME,:STORE_CODE,:STORE_NAME,:CSAD_NO,:CSAD_NAME,:ENTRY_NO,:ENTRY_NAME");
            sql.Append(" ,:DECISION_NO_FIRST,:DECISION_NAME,:MANUAL_DECISION,:ES_DECISION,:APP_STATUS,:CREATED_USER,:CREATED_TIME,:CHANGED_USER,:CHANGED_TIME,:DECISION_NO_FINAL");
            sql.Append(" ,:FRAUD_NO,:QUEUE_ORDER,:LOAN_AMT,:TERMS,:DECISION_CODE,:LATEST_APP_STATUS,:LATEST_ACTION,:CUSTOMERTYPE,:DECISION_NO_HIST,:APPROVE_NAME,:FRAUD_NAME");
            sql.Append(" ,:DCOUT_IS_AUTO_FINAL,:APP_ID,:APP_STATUS_DONE,:APP_NR_SD_STATUS,:APP_APPR1_SD_STATUS,:DES_RUNNING_TAG,:STATUS_ORDER,:IS_FINAL,:IS_AUDIT_FLAG,:IS_AUDIT_APPROVE_FLAG");
            sql.Append(" ,:IS_AUDO_FINAL,:STATUS_PHASE,:APPLY_CITY_CODE,:ENTRY_ORG_CODE,:ENTRYORGNAME)");
            List<object> paras = new List<object>();
            paras.Add(appQueueLog.ID);
            paras.Add(appQueueLog.QUEUE_ID);
            paras.Add(appQueueLog.APP_CODE);
            paras.Add(appQueueLog.NAME);
            paras.Add(appQueueLog.GENDER);
            paras.Add(appQueueLog.AGE);
            paras.Add(appQueueLog.ID_NO);
            paras.Add(appQueueLog.ID_TYPE);
            paras.Add(appQueueLog.LOGO);
            paras.Add(appQueueLog.PRODUCT_CODE);
            paras.Add(appQueueLog.PRODUCT_NAME);
            paras.Add(appQueueLog.COM_NAME);
            paras.Add(appQueueLog.DCOUT_VIP);
            paras.Add(appQueueLog.IS_REPEAT);
            paras.Add(appQueueLog.DO_ACTION);
            paras.Add(appQueueLog.APPLY_AMT);
            paras.Add(appQueueLog.CONTRACT_AMT);
            paras.Add(appQueueLog.PAY_TYPE);
            paras.Add(appQueueLog.DATE_APPLY);
            paras.Add(appQueueLog.SALES_NO);
            paras.Add(appQueueLog.SALES_NAME);
            paras.Add(appQueueLog.AREA_CODE);
            paras.Add(appQueueLog.AREA_NAME);
            paras.Add(appQueueLog.STORE_CODE);
            paras.Add(appQueueLog.STORE_NAME);
            paras.Add(appQueueLog.CSAD_NO);
            paras.Add(appQueueLog.CSAD_NAME);
            paras.Add(appQueueLog.ENTRY_NO);
            paras.Add(appQueueLog.ENTRY_NAME);
            paras.Add(appQueueLog.DECISION_NO_FIRST);
            paras.Add(appQueueLog.DECISION_NAME);
            paras.Add(appQueueLog.MANUAL_DECISION);
            paras.Add(appQueueLog.ES_DECISION);
            paras.Add(appQueueLog.APP_STATUS);
            paras.Add(appQueueLog.CREATED_USER);
            paras.Add(appQueueLog.CREATED_TIME);
            paras.Add(appQueueLog.CHANGED_USER);
            paras.Add(appQueueLog.CHANGED_TIME);
            paras.Add(appQueueLog.DECISION_NO_FINAL);
            paras.Add(appQueueLog.FRAUD_NO);
            paras.Add(appQueueLog.QUEUE_ORDER);
            paras.Add(appQueueLog.LOAN_AMT);
            paras.Add(appQueueLog.TERMS);
            paras.Add(appQueueLog.DECISION_CODE);
            paras.Add(appQueueLog.LATEST_APP_STATUS);
            paras.Add(appQueueLog.LATEST_ACTION);
            paras.Add(appQueueLog.CUSTOMERTYPE);
            paras.Add(appQueueLog.DECISION_NO_HIST);
            paras.Add(appQueueLog.APPROVE_NAME);
            paras.Add(appQueueLog.FRAUD_NAME);
            paras.Add(appQueueLog.DCOUT_IS_AUTO_FINAL);
            paras.Add(appQueueLog.APP_ID);
            paras.Add(appQueueLog.APP_STATUS_DONE);
            paras.Add(appQueueLog.APP_NR_SD_STATUS);
            paras.Add(appQueueLog.APP_APPR1_SD_STATUS);
            paras.Add(appQueueLog.DES_RUNNING_TAG);
            paras.Add(appQueueLog.STATUS_ORDER);
            paras.Add(appQueueLog.IS_FINAL);
            paras.Add(appQueueLog.IS_AUDIT_FLAG);
            paras.Add(appQueueLog.IS_AUDIT_APPROVE_FLAG);
            paras.Add(appQueueLog.IS_AUDO_FINAL);
            paras.Add(appQueueLog.STATUS_PHASE);
            paras.Add(appQueueLog.APPLY_CITY_CODE);
            paras.Add(appQueueLog.ENTRY_ORG_CODE);
            paras.Add(appQueueLog.ENTRYORGNAME);
            this.ExecuteSqlCommand(sql.ToString(), paras.ToArray());
        }
    }
}
