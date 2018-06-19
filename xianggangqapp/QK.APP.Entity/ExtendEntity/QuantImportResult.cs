using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QK.QAPP.Entity
{
    public class QuantImportResult
    {
        public QuantImportResult()
        {
            ExsitsList = new List<string>();
            TotleCount = 0;
            InsertCount = 0;
            UpdateCount = 0;
        }

        public int TotleCount { get; set; }
        public int InsertCount { get; set; }
        public int UpdateCount { get; set; }
        public List<string> ExsitsList { get; set; }
        public string Message { get; set; }
    }
}
