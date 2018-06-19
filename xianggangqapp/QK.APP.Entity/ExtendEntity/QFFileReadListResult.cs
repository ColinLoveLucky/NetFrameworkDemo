using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QK.QAPP.Entity
{
    public class QFFileReadListResult
    {
        public long id { get; set; }

        public string status { get; set; }

        public string flMemo { get; set; }

        public string flType { get; set; }

        public int flSize { get; set; }

        public int flWidth { get; set; }

        public int flHeighth { get; set; }

        public string flBiz { get; set; }

        public string flName { get; set; }

        public string flPath { get; set; }

        public int flSeq { get; set; }

        public byte[] fileContent { get; set; }

        public string createdUser { get; set; }

        public string createdTime { get; set; }

        public string changedUser { get; set; }

        public string changedTime { get; set; }
    }
}
