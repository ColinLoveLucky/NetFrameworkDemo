using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinqToEntity
{
   public  class Instructor
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public virtual List<Course> Course { get; set; }
    }
}
