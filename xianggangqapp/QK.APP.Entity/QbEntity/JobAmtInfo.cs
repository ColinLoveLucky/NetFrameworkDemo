using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QK.QAPP.Entity
{
    public class JobAmtInfo
    {
        public string AmtType { get; set; }

        public List<string> JobInfo { get; set; }

        public Nullable<int> AutoDelDay { get; set; }
        public Nullable<int> AutoCancelDay { get; set; }     

    }
}
