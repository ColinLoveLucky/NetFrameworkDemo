using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace QK.QAPP.Entity
{
    public class ExcelImportPara
    {
        public string FileName { get; set; } 
        public Stream Stream { get; set; }
        public bool IgnoreFirstLine { get; set; }
        public PropertyInfo[] Propertys { get; set; }
    }
}
