using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinqToEntity
{
    public class Person
    {
        public int Id { get; set; }
        public string Name { get; set; }

        [InverseProperty("CreatedBy")]
        public virtual List<Posts> PostsWritten { get; set; }

        [InverseProperty("UpdatedBy")]
        public virtual List<Posts> PostsUpdated { get; set; }
    }

}
