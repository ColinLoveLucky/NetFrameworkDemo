using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QK.QAPP.Entity
{
    public class ViewListByPage<T>
    {
        /// <summary>
        /// 结果列表
        /// </summary>
        public List<T> ViewList { get; set; }
        /// <summary>
        /// 每页记录数
        /// </summary>
        public int Rows { get; set; }

        /// <summary>
        /// 总页数
        /// </summary>
        public int TotalPages { get; set; }

        /// <summary>
        /// 总记录数
        /// </summary>
        public int TotalRecords { get; set; }
        /// <summary>
        /// 当前页
        /// </summary>
        public int CurrentPage { get; set; }

        public void SetParameters(IQueryable<T> query, int iPageIndex, int iPageSize)
        {
            this.TotalRecords = query.Count();
            this.Rows = iPageSize;

            //分页
            this.ViewList = query.Skip((iPageIndex - 1) * iPageSize).Take(iPageSize).ToList();
            this.TotalPages = ((this.TotalRecords % this.Rows) == 0
                ? (this.TotalRecords / this.Rows)
                : (this.TotalRecords / this.Rows) + 1);
        }  
    }
}
