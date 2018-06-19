/***********************
 * 作    者：刘云松
 * 创建时间：‎‎2015-3-12 15:41:22
 * 作    用：提供展期、循环贷等查询的结果
*****************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QK.QAPP.Entity
{
    /// <summary>
    /// 提供展期、循环贷等查询的结果
    /// </summary>
    public class V_APPMAIN_EXTEND
    {
        public int ID { get; set; }


        public long? AppId { get; set; }


        public string AppCode { get; set; }


        public string Logo { get; set; }


        public string ProductCode { get; set; }


        public string CustomerName { get; set; }


        public string CustomerIDCard { get; set; }  


        public Nullable<decimal> ApplyAmt { get; set; }


        public Nullable<decimal> LoanAmtOfContract { get; set; }


        public string SalesName { get; set; }


        public string SalesNo { get; set; }


        public string CsadName { get; set; }


        public string CsadNo { get; set; }


        public string AppStatus { get; set; }


        public Nullable<decimal> Sorting { get; set; }


        public string AppStatusName { get; set; }


        public string ExtendAction { get; set; }


        public string CreatedUser { get; set; }


        public Nullable<System.DateTime> CreatedTime { get; set; }


        public string UpdateUser { get; set; }


        public Nullable<System.DateTime> UpdateTime { get; set; }

        public Nullable<System.DateTime> Back_loan_time { get; set; }
    }
}
