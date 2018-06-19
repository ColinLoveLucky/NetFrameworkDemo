using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QK.QAPP.Infrastructure
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ExcelAttribute : Attribute
    {
        public string ColumnName { get; set; }

        public int ColumnIndex { get; set; }

        public ExcelAttribute(string columnName)
        {
            this.ColumnName = columnName;
        }

        public ExcelAttribute(string columnName, int columnIndex)
        {
            this.ColumnName = columnName;
            this.ColumnIndex = columnIndex;
        }
    }
}
