using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QK.QAPP.Entity
{
    public class PreEnterListViewFiledList
    {
        /// <summary>
        /// 进件列表
        /// </summary>
        public List<V_PRE_APPMAIN> ListEnter { get; set; }

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

        public void SetParameters(IQueryable<V_PRE_APPMAIN> query, PreEnterListSearchPara para)
        {
            this.TotalRecords = query.Count();
            this.Rows = para.PageSize;

            //分页
            this.ListEnter = query.Skip((para.PageIndex - 1) * para.PageSize).Take(para.PageSize).ToList();
            this.TotalPages = ((this.TotalRecords % this.Rows) == 0
                ? (this.TotalRecords / this.Rows)
                : (this.TotalRecords / this.Rows) + 1);
        }
    }
}
