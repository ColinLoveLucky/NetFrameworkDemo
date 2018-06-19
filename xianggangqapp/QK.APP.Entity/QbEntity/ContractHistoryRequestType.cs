using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QK.QAPP.Entity
{
    public class ContractHistoryRequest
    {
        public List<ContractHistoryRequestType> ContractHistory { get; set; }
    }
    public class ContractHistoryRequestType
    {
        public string BidCode { get; set; }

        public string ContractCode { get; set; }

        public string UserCode { get; set; }

        public string DeptName { get; set; }
        public string DeptCode { get; set; }
        public string UserName { get; set; }

        public ContractOPTYPE ContractOptype { get; set; }
    }
}
