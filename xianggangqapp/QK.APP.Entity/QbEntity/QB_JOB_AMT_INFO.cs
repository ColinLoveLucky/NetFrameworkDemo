using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QK.QAPP.Entity
{
    public class QB_JOB_AMT_INFO
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
        /// IS_ACTIVE
        /// </summary>		
        public string IS_ACTIVE { get; set; }


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
        /// AUTO_DEL_DAY
        /// </summary>		
        public Nullable<int> AUTO_DEL_DAY { get; set; }

        public Nullable<int> AUTO_CANCEL_DAY { get; set; }

    }
}
