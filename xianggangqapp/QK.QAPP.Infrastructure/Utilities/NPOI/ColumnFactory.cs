﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QK.QAPP.Infrastructure
{
    public class ColumnFactory<T>
    {
        public List<ColumnBuilder<T>> Columns = new List<ColumnBuilder<T>>();

        public int ColumnCount
        {
            get { return Columns.Count; }
        }

        public ColumnBuilder<T> Bound(Func<T, object> property)
        {
            var builder = new ColumnBuilder<T>(property);
            Columns.Add(builder);
            return builder;
        }
    }
}
