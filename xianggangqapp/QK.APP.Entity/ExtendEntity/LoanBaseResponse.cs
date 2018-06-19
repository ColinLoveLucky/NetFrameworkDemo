using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QK.QAPP.Entity.ExtendEntity
{
    public class LoanBaseResponse<T>
    {
        public string transactionId { get; set; }
        public string messageId { get; set; }
        public string successIndicator { get; set; }
        public string application { get; set; }
        public T messages { get; set; }

    }
}
