using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QK.QAPP.Entity
{
    public class QB_RAISEPLAN_HISTORY
    {
        /// <summary>
        /// ID
        /// </summary>		
        public long ID { get; set; }


        /// <summary>
        /// APPNO
        /// </summary>		
        public string APPNO { get; set; }


        /// <summary>
        /// RAISE_FUND_NO
        /// </summary>		
        public string RAISE_FUND_NO { get; set; }


        /// <summary>
        /// CONTRACT_AMT
        /// </summary>		
        public Nullable<decimal> CONTRACT_AMT { get; set; }


        /// <summary>
        /// PUTOUT_AMT
        /// </summary>		
        public Nullable<decimal> PUTOUT_AMT { get; set; }


        /// <summary>
        /// RETURN_CONTRACT_AMT
        /// </summary>		
        public Nullable<decimal> RETURN_CONTRACT_AMT { get; set; }


        /// <summary>
        /// RETURN_PUTOUT_AMT
        /// </summary>		
        public Nullable<decimal> RETURN_PUTOUT_AMT { get; set; }


        /// <summary>
        /// AUDIT_STATUS
        /// </summary>		
        public string AUDIT_STATUS { get; set; }


        /// <summary>
        /// AUDIT_DATE
        /// </summary>		
        public string AUDIT_DATE { get; set; }


        /// <summary>
        /// CREATE_DATE
        /// </summary>		
        public string CREATE_DATE { get; set; }


        /// <summary>
        /// UPDATE_DATE
        /// </summary>		
        public string UPDATE_DATE { get; set; }


        /// <summary>
        /// RAISE_FUND_AMT
        /// </summary>		
        public Nullable<decimal> RAISE_FUND_AMT { get; set; }


        /// <summary>
        /// VALIDATE_RESULT
        /// </summary>		
        public string VALIDATE_RESULT { get; set; }


        /// <summary>
        /// DISPLAY_FLAG
        /// </summary>		
        public string DISPLAY_FLAG { get; set; }     
    }
}
