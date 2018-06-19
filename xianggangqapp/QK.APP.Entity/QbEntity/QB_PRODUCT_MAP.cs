using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QK.QAPP.Entity
{
   public class QB_PRODUCT_MAP
    {
        /// <summary>
        /// ID
        /// </summary>		
        public int ID { get; set; }


        /// <summary>
        /// QB_BID_PRD_CODE
        /// </summary>		
        public string QB_BID_PRD_CODE { get; set; }


        /// <summary>
        /// QB_T24_PRD_ODE
        /// </summary>		
        public string QB_T24_PRD_ODE { get; set; }


        /// <summary>
        /// QB_PRD_NAME
        /// </summary>		
        public string QB_PRD_NAME { get; set; }


        /// <summary>
        /// QB_TYPE
        /// </summary>		
        public Nullable<int> QB_TYPE { get; set; }


        /// <summary>
        /// QB_BASIC_RATE
        /// </summary>		
        public string QB_BASIC_RATE { get; set; }


        /// <summary>
        /// QB_BASIC_RATE_TYPE
        /// </summary>		
        public string QB_BASIC_RATE_TYPE { get; set; }


        /// <summary>
        /// QB_FINE_RATE
        /// </summary>		
        public string QB_FINE_RATE { get; set; }


        /// <summary>
        /// QB_SERVICE_RATE
        /// </summary>		
        public string QB_SERVICE_RATE { get; set; }


        /// <summary>
        /// QB_LATE_RATE
        /// </summary>		
        public string QB_LATE_RATE { get; set; }


        /// <summary>
        /// QB_PREPAYMENT_RATE
        /// </summary>		
        public string QB_PREPAYMENT_RATE { get; set; }


        /// <summary>
        /// QB_T24_BRANCH
        /// </summary>		
        public string QB_T24_BRANCH { get; set; }     
    }
}
