using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QK.QAPP.Entity
{
    public class QB_AUTO_JOB_LOG
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
        /// AMT_TYPE_NAME
        /// </summary>		
        public string AMT_TYPE_NAME { get; set; }

        /// <summary>
        /// JOB_TYPE
        /// </summary>		
        public string JOB_TYPE { get; set; }

        /// <summary>
        /// JOB_TYPE_NAME
        /// </summary>		
        public string JOB_TYPE_NAME { get; set; }

        /// <summary>
        /// CREATE_USER
        /// </summary>		
        public string CREATE_USER { get; set; }


        /// <summary>
        /// CREATE_TIME
        /// </summary>		
        public Nullable<System.DateTime> CREATE_TIME { get; set; }


        /// <summary>
        /// EXCUTE_MSG
        /// </summary>		
        public string EXCUTE_MSG { get; set; }


        /// <summary>
        /// LOG_TYPE
        /// </summary>		
        public string LOG_TYPE { get; set; }


        /// <summary>
        /// EXCUTE_COUNT
        /// </summary>		
        public Nullable<int> EXCUTE_COUNT { get; set; }   
    }
}
