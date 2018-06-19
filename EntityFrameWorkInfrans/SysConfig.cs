using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityFrameWorkInfrans
{
    [Table("App_Dictionary")]
    public class Dictionary:BaseModel
    {

        [StringLength(20)]
        public string Code { get; set; }

        [StringLength(50)]
        public string Name { get; set; }

        public virtual List<DictionaryChild> Childs { get; set; }
    }

    [Table("App_Dictionary_Child")]
    public class DictionaryChild:BaseModel
    {

        [StringLength(100)]
        public string Field { get; set; }

        [StringLength(100)]
        public string Value { get; set; }

    }
}
