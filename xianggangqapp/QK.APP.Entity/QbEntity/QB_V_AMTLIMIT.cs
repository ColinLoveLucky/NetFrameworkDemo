using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QK.QAPP.Entity
{
    public class QB_V_AMTLIMIT
    {
        [Sequence("SEQ_QB_V_AMTLIMIT")]

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
        public decimal AMT { get; set; }


        /// <summary>
        /// AMT_USABLE
        /// </summary>		
        public decimal  AMT_USABLE { get; set; }


        /// <summary>
        /// AMT_USED
        /// </summary>		
        public decimal AMT_USED { get; set; }


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
        /// AMT_TYPE_NAME
        /// </summary>		
        public string AMT_TYPE_NAME { get; set; }


        /// <summary>
        /// CONFIRM_FLAG
        /// </summary>		
        public string CONFIRM_FLAG { get; set; }


        /// <summary>
        /// AMT_STATE
        /// </summary>		
        public string AMT_STATE { get; set; }


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


        /// <summary>
        /// AMT_HAS_BOUNCE_RATE
        /// </summary>		
        public string AMT_HAS_BOUNCE_RATE { get; set; }


        /// <summary>
        /// AMT_CONFIRM_MODE
        /// </summary>		
        public string AMT_CONFIRM_MODE { get; set; }


        /// <summary>
        /// AMT_CONFIRM_MODE_NAME
        /// </summary>		
        public string AMT_CONFIRM_MODE_NAME { get; set; }


        /// <summary>
        /// AMT_MODIFIED_MODE
        /// </summary>		
        public string AMT_MODIFIED_MODE { get; set; }


        /// <summary>
        /// AMT_MODIFIED_MODE_NAME
        /// </summary>		
        public string AMT_MODIFIED_MODE_NAME { get; set; }


        /// <summary>
        /// AMT_DELETE_MODE
        /// </summary>		
        public string AMT_DELETE_MODE { get; set; }


        /// <summary>
        /// AMT_DELETE_MODE_NAME
        /// </summary>		
        public string AMT_DELETE_MODE_NAME { get; set; }


        /// <summary>
        /// AMT_ADJUST_MODE
        /// </summary>		
        public string AMT_ADJUST_MODE { get; set; }


        /// <summary>
        /// AMT_ADJUST_MODE_NAME
        /// </summary>		
        public string AMT_ADJUST_MODE_NAME { get; set; }


        /// <summary>
        /// AMT_RAISE_PLAN_MODE
        /// </summary>		
        public string AMT_RAISE_PLAN_MODE { get; set; }


        /// <summary>
        /// AMT_DEPT_CODE
        /// </summary>		
        public string AMT_DEPT_CODE { get; set; }


        /// <summary>
        /// AMT_DEPT_NAME
        /// </summary>		
        public string AMT_DEPT_NAME { get; set; }


        /// <summary>
        /// AMT_AMS_RAISE_INTERFACE
        /// </summary>		
        public string AMT_AMS_RAISE_INTERFACE { get; set; }


        /// <summary>
        /// AMT_T_VALUE_DATE
        /// </summary>		
        public string AMT_T_VALUE_DATE { get; set; }


        /// <summary>
        /// AMT_T_AGREE_VALUE_DATE
        /// </summary>		
        public string AMT_T_AGREE_VALUE_DATE { get; set; }


        /// <summary>
        /// AMT_LE_ZERO
        /// </summary>		
        public string AMT_LE_ZERO { get; set; }


        /// <summary>
        /// AMT_FINISH_DIV_BID
        /// </summary>		
        public string AMT_FINISH_DIV_BID { get; set; }

        /// <summary>
        /// AMT_EFFECTIVE
        /// </summary>		
        public Nullable<short> AMT_EFFECTIVE { get; set; }

        /// <summary>
        /// 协议上传截止时间
        /// </summary>		
        public string AMT_AGREEMENT_UP_STOPTIME { get; set; }

        /// <summary>
        /// 额度划分类型:3-理财/融资匹配-手动,4-理财/融资匹配-自动,5-信托匹配-手动,6-信托匹配-自动
        /// </summary>		
        public string QB_LOAN_TYPE { get; set; }   
    }
}
