/***********************
 * 作    者：刘云松
 * 创建时间：‎‎2015-3-12 15:41:22
 * 作    用：批复队列/评估队列查询列表
*****************************/
using QK.QAPP.Entity.ExtendEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QK.QAPP.Entity
{
    public class AssessQueueViewFieldList
    {
        /// <summary>
        /// 进件列表
        /// </summary>
        public List<APP_QUEUE_ASSESS> ListEnter { get; set; }

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

        public void SetParameters(IEnumerable<APP_QUEUE_ASSESS> query, AssessListSearchPara para)
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
