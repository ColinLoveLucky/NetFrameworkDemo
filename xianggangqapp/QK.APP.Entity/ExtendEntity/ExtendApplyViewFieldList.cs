/***********************
 * 作    者：刘云松
 * 创建时间：‎‎2015-3-12 15:41:22
 * 作    用：提供展期、循环贷等查询的结果
*****************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QK.QAPP.Entity
{
    public class ExtendApplyViewFieldList
    {
        /// <summary>
        /// 进件列表
        /// </summary>
        public List<V_APPMAIN_EXTEND> ListEnter { get; set; }

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

        public void SetParameters(IEnumerable<V_APPMAIN_EXTEND> query, ExtendApplySearchPara para)
        {
            //分页
            this.ListEnter = query.ToList();
            V_APPMAIN_EXTEND va = ListEnter.Find(v => v.AppId == null);
            if (va != null)
            {
                this.TotalRecords = va.ID;
                this.Rows = para.PageSize;
                this.TotalPages = ((this.TotalRecords % this.Rows) == 0
                    ? (this.TotalRecords / this.Rows)
                    : (this.TotalRecords / this.Rows) + 1);

                this.ListEnter.Remove(va);
            }
        }
    }
}
