using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityFrameWorkInfrans
{
    public class BaseModel
    {
        public int Id { get; set; }

        [DefaultValue(1)]
        public int DeleteFlag { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }
        
        public DateTime? CreateTime { get; set; }

        public DateTime? UpdateTime { get; set; }

        public int SortId { get; set; }

    }
}
