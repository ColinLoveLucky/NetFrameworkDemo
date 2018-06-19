using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QK.QAPP.Entity
{
    public class QuantHandleResult
    {
        public QuantHandleResult()
        {
            TotleCount = 0;
            SuccessCount = 0;
            FailCount = 0;
            UnProcess = 0;
            ResultDic = new Dictionary<string, string>();
        }

        public int TotleCount { get; set; }
        public int SuccessCount { get; set; }
        public int FailCount { get; set; }
        public int UnProcess { get; set; }
        public Dictionary<string, string> ResultDic { get; set; } 
    }
}
