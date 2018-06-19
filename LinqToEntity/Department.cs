using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinqToEntity
{
    public class Department
    {
     
        public int DepartmentId { get; set; }

        public decimal ? Budget { get; set; }

        public DepartmentNames Name { get; set; }
    }

    public enum DepartmentNames
    {
        English,
        Math,
        Economics
    }
}
