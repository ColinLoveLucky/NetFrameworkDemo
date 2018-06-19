using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QK.QAPP.Entity
{
    public class EnterListViewFiledList
    {
        /// <summary>
        /// 进件列表
        /// </summary>
        public List<V_APPMAIN> ListEnter { get; set; }

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

        public void SetParameters(IQueryable<V_APPMAIN> query, EnterListSearchPara para)
        {
            this.TotalRecords = query.Count();
            this.Rows = para.PageSize;
            
            //分页
            this.ListEnter = query.Skip((para.PageIndex - 1) * para.PageSize).Take(para.PageSize).ToList();
            this.TotalPages = ((this.TotalRecords % this.Rows) == 0
                ? (this.TotalRecords / this.Rows)
                : (this.TotalRecords / this.Rows) + 1);
        }

        public void SetParameters(int dataCount,List<V_APPMAIN> dataList, EnterListSearchPara para)
        {
            this.TotalRecords = dataCount;
            this.Rows = para.PageSize;

            //分页
            this.ListEnter = dataList; 
            this.TotalPages = ((this.TotalRecords % this.Rows) == 0
                ? (this.TotalRecords / this.Rows)
                : (this.TotalRecords / this.Rows) + 1);
        }
    }
}
