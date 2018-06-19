using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QK.QAPP.Entity
{
    public class APPQueueLog : BasicEntity, ICloneable
    {
        public APPQueueLog() : base(false) { }
        public APPQueueLog(bool isGenerated = false) : base(isGenerated) { }

        [Sequence("SEQ_APP_QUEUE_LOG")]
        public long ID { get; set; }
        public long QUEUE_ID { get; set; }
        public string APP_CODE { get; set; }
        public string NAME { get; set; }
        public string GENDER { get; set; }
        public short? AGE { get; set; }
        public string ID_NO { get; set; }
        public string ID_TYPE { get; set; }
        public string LOGO { get; set; }
        public string PRODUCT_CODE { get; set; }
        public string PRODUCT_NAME { get; set; }
        public string COM_NAME { get; set; }
        public string DCOUT_VIP { get; set; }
        public string IS_REPEAT { get; set; }
        public string DO_ACTION { get; set; }
        public decimal? APPLY_AMT { get; set; }
        public decimal? CONTRACT_AMT { get; set; }
        public string PAY_TYPE { get; set; }
        public DateTime? DATE_APPLY { get; set; }
        public string SALES_NO { get; set; }
        public string SALES_NAME { get; set; }
        public string AREA_CODE { get; set; }
        public string AREA_NAME { get; set; }
        public string STORE_CODE { get; set; }
        public string STORE_NAME { get; set; }
        public string CSAD_NO { get; set; }
        public string CSAD_NAME { get; set; }
        public string ENTRY_NO { get; set; }
        public string ENTRY_NAME { get; set; }
        public string DECISION_NO_FIRST { get; set; }
        public string DECISION_NAME { get; set; }
        public string MANUAL_DECISION { get; set; }
        public string ES_DECISION { get; set; }
        public string APP_STATUS { get; set; }
        public string CREATED_USER { get; set; }
        public DateTime? CREATED_TIME { get; set; }
        public string CHANGED_USER { get; set; }
        public DateTime? CHANGED_TIME { get; set; }
        public string DECISION_NO_FINAL { get; set; }
        public string FRAUD_NO { get; set; }
        public short? QUEUE_ORDER { get; set; }
        public decimal? LOAN_AMT { get; set; }
        public short? TERMS { get; set; }
        public string DECISION_CODE { get; set; }
        public string LATEST_APP_STATUS { get; set; }
        public string LATEST_ACTION { get; set; }
        public string CUSTOMERTYPE { get; set; }
        public string DECISION_NO_HIST { get; set; }
        public string APPROVE_NAME { get; set; }
        public string FRAUD_NAME { get; set; }
        public string DCOUT_IS_AUTO_FINAL { get; set; }
        public long APP_ID { get; set; }
        public string APP_STATUS_DONE { get; set; }
        public string APP_NR_SD_STATUS { get; set; }
        public string APP_APPR1_SD_STATUS { get; set; }
        public string DES_RUNNING_TAG { get; set; }
        public string STATUS_ORDER { get; set; }
        public string IS_FINAL { get; set; }
        public string IS_AUDIT_FLAG { get; set; }
        public string IS_AUDIT_APPROVE_FLAG { get; set; }
        public string IS_AUDO_FINAL { get; set; }
        public string STATUS_PHASE { get; set; }
        public string APPLY_CITY_CODE { get; set; }
        public string ENTRY_ORG_CODE { get; set; }
        public string ENTRYORGNAME { get; set; }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
