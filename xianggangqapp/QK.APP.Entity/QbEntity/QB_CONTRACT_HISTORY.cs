using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QK.QAPP.Entity
{
   public class QB_CONTRACT_HISTORY
    {
        /// <summary>
        /// ID
        /// </summary>		
        public long ID { get; set; }


        /// <summary>
        /// UUID
        /// </summary>		
        public string UUID { get; set; }


        /// <summary>
        /// BID_CODE
        /// </summary>		
        public string BID_CODE { get; set; }


        /// <summary>
        /// BES_ID
        /// </summary>		
        public string BES_ID { get; set; }


        /// <summary>
        /// CONTRACT_OP_TYPE
        /// </summary>		
        public string CONTRACT_OP_TYPE { get; set; }


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
        /// USE_DATE
        /// </summary>		
        public string USE_DATE { get; set; }


        /// <summary>
        /// CREATE_DATE
        /// </summary>		
        public string CREATE_DATE { get; set; }     
    }
}
