using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QK.QAPP.Entity
{
    public class QB_RAISEPLAN
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
        /// THIRD_PART_ID
        /// </summary>		
        public string THIRD_PART_ID { get; set; }


        /// <summary>
        /// THIRD_PART_NAME
        /// </summary>		
        public string THIRD_PART_NAME { get; set; }


        /// <summary>
        /// RAISE_FUND_NO
        /// </summary>		
        public string RAISE_FUND_NO { get; set; }


        /// <summary>
        /// RAISE_FUND_NAME
        /// </summary>		
        public string RAISE_FUND_NAME { get; set; }


        /// <summary>
        /// RAISE_FUND_AMT
        /// </summary>		
        public decimal RAISE_FUND_AMT { get; set; }


        /// <summary>
        /// RAISE_FUND_BEG_DATE
        /// </summary>		
        public string RAISE_FUND_BEG_DATE { get; set; }


        /// <summary>
        /// RAISE_FUND_TERM_DAY
        /// </summary>		
        public string RAISE_FUND_TERM_DAY { get; set; }


        /// <summary>
        /// RAISE_FUND_REJECT_PER
        /// </summary>		
        public decimal RAISE_FUND_REJECT_PER { get; set; }


        /// <summary>
        /// ACCOUNT_NUMBER
        /// </summary>		
        public string ACCOUNT_NUMBER { get; set; }


        /// <summary>
        /// ACCOUNT_NAME
        /// </summary>		
        public string ACCOUNT_NAME { get; set; }


        /// <summary>
        /// ACCOUNT_BANK_NAME
        /// </summary>		
        public string ACCOUNT_BANK_NAME { get; set; }


        /// <summary>
        /// ACCOUNT_BRANCH_NAME
        /// </summary>		
        public string ACCOUNT_BRANCH_NAME { get; set; }


        /// <summary>
        /// ACCOUNT_PROVINCE
        /// </summary>		
        public string ACCOUNT_PROVINCE { get; set; }


        /// <summary>
        /// ACCOUNT_CITY
        /// </summary>		
        public string ACCOUNT_CITY { get; set; }


        /// <summary>
        /// ACCOUNT_ZONE
        /// </summary>		
        public string ACCOUNT_ZONE { get; set; }


        /// <summary>
        /// ACCOUNT_BRANCH_ADDR
        /// </summary>		
        public string ACCOUNT_BRANCH_ADDR { get; set; }


        /// <summary>
        /// QX_DATE
        /// </summary>		
        public string QX_DATE { get; set; }


        /// <summary>
        /// MTR_DATE
        /// </summary>		
        public string MTR_DATE { get; set; }


        /// <summary>
        /// CREATE_DATE
        /// </summary>		
        public string CREATE_DATE { get; set; }


        /// <summary>
        /// AMT_TYPE
        /// </summary>		
        public string AMT_TYPE { get; set; }


        /// <summary>
        /// RAISE_PRO_NO
        /// </summary>		
        public string RAISE_PRO_NO { get; set; }


        /// <summary>
        /// RAISE_SOURCE
        /// </summary>		
        public Nullable<int> RAISE_SOURCE { get; set; }

        /// <summary>
        /// RAISE_SOURCE_NAME
        /// </summary>		
        public string RAISE_SOURCE_NAME { get; set; }
    }
}
