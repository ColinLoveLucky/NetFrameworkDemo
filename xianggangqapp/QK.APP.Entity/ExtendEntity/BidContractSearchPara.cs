using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QK.QAPP.Entity.ExtendEntity
{
    public class BidContractSearchPara
    {
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public string BidIdentifier { get; set; }
        public string ArrangementId { get; set; }
    }
}
