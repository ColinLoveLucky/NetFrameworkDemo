using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QK.QAPP.Entity
{
    public class ContractOperateRequest
    {
        public string ContractIsCreated { get; set; }

        public DateTime ContractCreateTime { get; set; }

        public string BidId { get; set; }
    }
}
