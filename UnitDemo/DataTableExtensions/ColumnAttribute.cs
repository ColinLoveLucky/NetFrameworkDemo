using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnitDemo.DataTableExtensions
{
    public class ColumnAttribute : Attribute
    {
        public ColumnAttribute(string name, Type type)
        {
            this.Name = name;

            this.Type = type;
        }

        public string Name
        {
            get;
            set;
        }

        public Type Type
        {
            get;
            set;
        }
    }
}
