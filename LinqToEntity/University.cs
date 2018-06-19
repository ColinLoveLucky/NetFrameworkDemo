using System;
using System.Collections.Generic;
using System.Data.Entity.Spatial;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinqToEntity
{
    public class University
    {
        public int UniversityID { get; set; }
        public string Name { get; set; }
        public DbGeography Location { get; set; }
    }
}
