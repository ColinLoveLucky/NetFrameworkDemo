using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QK.QAPP.Entity
{
    public class PageData<T>
    {
        public List<T> DataList { get; set; }
        /// <summary>
        /// 总页数
        /// </summary>
        public int TotalCount { get; set; }
        /// <summary>
        /// 总条数
        /// </summary>
        public int TotalRecords { get; set; }
        /// <summary>
        /// 当前页数
        /// </summary>
        public int CurrentPage { get; set; }
    }
}
