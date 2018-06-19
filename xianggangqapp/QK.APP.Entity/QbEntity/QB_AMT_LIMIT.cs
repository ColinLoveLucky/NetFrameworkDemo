using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QK.QAPP.Entity.QbEntity
{
    public class QB_AMT_LIMIT
    {
        [Sequence("SEQ_QB_AMT_LIMIT")]

        /// <summary>
        /// ID
        /// </summary>		
        public long ID { get; set; }


        /// <summary>
        /// UUID
        /// </summary>		
        public string UUID { get; set; }


        /// <summary>
        /// AMT
        /// </summary>		
        public Nullable<decimal> AMT { get; set; }


        /// <summary>
        /// AMT_USABLE
        /// </summary>		
        public Nullable<decimal> AMT_USABLE { get; set; }


        /// <summary>
        /// AMT_TYPE
        /// </summary>		
        public string AMT_TYPE { get; set; }


        /// <summary>
        /// AMT_SECOND_TYPE
        /// </summary>		
        public string AMT_SECOND_TYPE { get; set; }


        /// <summary>
        /// AMT_THIRD_TYPE
        /// </summary>		
        public string AMT_THIRD_TYPE { get; set; }


        /// <summary>
        /// CONFIRM_FLAG
        /// </summary>		
        public string CONFIRM_FLAG { get; set; }


        /// <summary>
        /// RAISE_PLAN_NO
        /// </summary>		
        public string RAISE_PLAN_NO { get; set; }


        /// <summary>
        /// BOUNCE_RATE
        /// </summary>		
        public Nullable<decimal> BOUNCE_RATE { get; set; }


        /// <summary>
        /// USE_START_DATE
        /// </summary>		
        public string USE_START_DATE { get; set; }


        /// <summary>
        /// USE_END_DATE
        /// </summary>		
        public string USE_END_DATE { get; set; }


        /// <summary>
        /// CREATE_DATE
        /// </summary>		
        public string CREATE_DATE { get; set; }


        /// <summary>
        /// UPDATE_DATE
        /// </summary>		
        public string UPDATE_DATE { get; set; }


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
    }
}
