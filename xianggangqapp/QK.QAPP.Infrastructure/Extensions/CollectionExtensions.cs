﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QK.QAPP.Infrastructure
{
    public static class CollectionExtensions
    {
        public static void AddRange<T>(this ICollection<T> source, IEnumerable<T> items)
        {
            if (items == null)
            {
                return;
            }
            foreach (var item in items)
            {
                source.Add(item);
            }
        }
    }
}
