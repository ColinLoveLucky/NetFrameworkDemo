using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QK.QAPP.Entity.QbEntity
{
    [Serializable]
    public class BidSystemConfigInfo
    {
        public long ID { get; set; }

        public string SYS_KEY { get; set; }

        public string SYS_VALUE { get; set; }

        public string SYS_DEC { get; set; }

        public string SYS_ORDER { get; set; }

        public string SYS_TYPE { get; set; }
    }
}
