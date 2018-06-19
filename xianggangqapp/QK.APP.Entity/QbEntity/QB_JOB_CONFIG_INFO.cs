using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QK.QAPP.Entity
{
    [Serializable]
    public class QB_JOB_CONFIG_INFO
    {
        /// <summary>
        /// ID
        /// </summary>		
        public long ID { get; set; }


        /// <summary>
        /// AMT_TYPE
        /// </summary>		
        public string AMT_TYPE { get; set; }


        /// <summary>
        /// JOB_TYPE
        /// </summary>		
        public string JOB_TYPE { get; set; }


        /// <summary>
        /// JOB_NAME
        /// </summary>		
        public string JOB_NAME { get; set; }


        /// <summary>
        /// EXCUTE_HOUR
        /// </summary>		
        public string EXCUTE_HOUR { get; set; }


        /// <summary>
        /// EXCUTE_MINUTE
        /// </summary>		
        public string EXCUTE_MINUTE { get; set; }


        /// <summary>
        /// JOB_START_HOUR
        /// </summary>		
        public string JOB_START_HOUR { get; set; }


        /// <summary>
        /// JOB_START_MINUTE
        /// </summary>		
        public string JOB_START_MINUTE { get; set; }


        /// <summary>
        /// JOB_END_HOUR
        /// </summary>		
        public string JOB_END_HOUR { get; set; }


        /// <summary>
        /// JOB_END_MINUTE
        /// </summary>		
        public string JOB_END_MINUTE { get; set; }


        /// <summary>
        /// JOB_INTERAL
        /// </summary>		
        public string JOB_INTERAL { get; set; }


        /// <summary>
        /// JOB_COUNT
        /// </summary>		
        public string JOB_COUNT { get; set; }


        /// <summary>
        /// JOB_EXECUTE_COUNT
        /// </summary>		
        public string JOB_EXECUTE_COUNT { get; set; }


        /// <summary>
        /// JOB_EXECUTE_MSG
        /// </summary>		
        public string JOB_EXECUTE_MSG { get; set; }


        /// <summary>
        /// JOB_STATE
        /// </summary>		
        public string JOB_STATE { get; set; }


        /// <summary>
        /// JOB_IS_ACTIVE
        /// </summary>		
        public string JOB_IS_ACTIVE { get; set; }


        /// <summary>
        /// JOB_LAST_EXEC_TIME
        /// </summary>		
        public Nullable<System.DateTime> JOB_LAST_EXEC_TIME { get; set; }


        /// <summary>
        /// CREATE_USER
        /// </summary>		
        public string CREATE_USER { get; set; }


        /// <summary>
        /// CREATE_TIME
        /// </summary>		
        public Nullable<System.DateTime> CREATE_TIME { get; set; }


        /// <summary>
        /// UPDATE_USER
        /// </summary>		
        public string UPDATE_USER { get; set; }


        /// <summary>
        /// UPDATE_TIME
        /// </summary>		
        public Nullable<System.DateTime> UPDATE_TIME { get; set; }

        /// <summary>
        /// JOB_PLAN
        /// </summary>		
        public string JOB_PLAN { get; set; }     

        /// <summary>
        /// IS_RUN
        /// </summary>		
        public string IS_RUN { get; set; }  

    }
}
