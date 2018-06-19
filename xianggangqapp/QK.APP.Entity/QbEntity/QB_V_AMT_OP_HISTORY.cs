using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QK.QAPP.Entity
{
    public class QB_V_AMT_OP_HISTORY
    {
        /// <summary>
        /// ID
        /// </summary>		
        public long ID { get; set; }


        /// <summary>
        /// AMT_LIMIT_UUID
        /// </summary>		
        public string AMT_LIMIT_UUID { get; set; }


        /// <summary>
        /// BES_ID
        /// </summary>		
        public string BES_ID { get; set; }


        /// <summary>
        /// AMT_OCCUR
        /// </summary>		
        public Nullable<decimal> AMT_OCCUR { get; set; }


        /// <summary>
        /// AMT_REMAINING
        /// </summary>		
        public Nullable<decimal> AMT_REMAINING { get; set; }


        /// <summary>
        /// AMT_OCCUR_TYPE
        /// </summary>		
        public string AMT_OCCUR_TYPE { get; set; }


        /// <summary>
        /// AMT_OCCUR_NAME
        /// </summary>		
        public string AMT_OCCUR_NAME { get; set; }


        /// <summary>
        /// AMT_OPERATE_TYPE
        /// </summary>		
        public string AMT_OPERATE_TYPE { get; set; }


        /// <summary>
        /// AMT_OPERATE_NAME
        /// </summary>		
        public string AMT_OPERATE_NAME { get; set; }


        /// <summary>
        /// AMT_ADJUST_MODE
        /// </summary>		
        public string AMT_ADJUST_MODE { get; set; }


        /// <summary>
        /// AMT_ADJUST_NAME
        /// </summary>		
        public string AMT_ADJUST_NAME { get; set; }


        /// <summary>
        /// UPDATE_USER_CODE
        /// </summary>		
        public string UPDATE_USER_CODE { get; set; }


        /// <summary>
        /// UPDATE_USER_NAME
        /// </summary>		
        public string UPDATE_USER_NAME { get; set; }


        /// <summary>
        /// DEPT_CODE
        /// </summary>		
        public string DEPT_CODE { get; set; }


        /// <summary>
        /// DEPT_NAME
        /// </summary>		
        public string DEPT_NAME { get; set; }


        /// <summary>
        /// CREATE_DATE
        /// </summary>		
        public Nullable<System.DateTime> CREATE_DATE { get; set; }


        /// <summary>
        /// USE_START_DATE
        /// </summary>		
        public Nullable<System.DateTime> USE_START_DATE { get; set; }


        /// <summary>
        /// USE_END_DATE
        /// </summary>		
        public Nullable<System.DateTime> USE_END_DATE { get; set; }     
    }
}
