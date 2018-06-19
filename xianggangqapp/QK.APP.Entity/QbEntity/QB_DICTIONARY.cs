using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QK.QAPP.Entity
{
    public class QB_DICTIONARY
    {

        /// <summary>
        /// ID
        /// </summary>		
        public long ID { get; set; }


        /// <summary>
        /// DIC_CODE
        /// </summary>		
        public string DIC_CODE { get; set; }


        /// <summary>
        /// DIC_NAME
        /// </summary>		
        public string DIC_NAME { get; set; }


        /// <summary>
        /// DIC_TYPE
        /// </summary>		
        public string DIC_TYPE { get; set; }


        /// <summary>
        /// LEVEL_CODE
        /// </summary>		
        public string LEVEL_CODE { get; set; }


        /// <summary>
        /// PARENT_CODE
        /// </summary>		
        public string PARENT_CODE { get; set; }


        /// <summary>
        /// LEVEL_NO
        /// </summary>		
        public Nullable<decimal> LEVEL_NO { get; set; }


        /// <summary>
        /// DIC_REMARK
        /// </summary>		
        public string DIC_REMARK { get; set; }


        /// <summary>
        /// DIC_STATE
        /// </summary>		
        public string DIC_STATE { get; set; }     
    }
}
