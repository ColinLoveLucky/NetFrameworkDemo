/***********************
 * 作    者：余方华
 * 创建时间：‎‎2015-3-24 13:56:31
 * 作    用：批复队列/评估队列查询条件
*****************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QK.QAPP.Entity
{
    public class AssessListSearchPara
    {
        List<AssessStatusType> listAssessStatus = new List<AssessStatusType>();

        /// <summary>
        /// 评估队列查询时需要的评估状态
        /// <para>默认new...</para>
        /// </summary>
        public List<AssessStatusType> ListAssessStatus
        {
            get { return listAssessStatus; }
            set { listAssessStatus = value; }
        }


        bool fuzzySearch = false;
        /// <summary>
        /// true:简单查询；false:高级查询
        /// </summary>
        public bool FuzzySearch
        {
            get { return fuzzySearch; }
            set { fuzzySearch = value; }
        }

        public bool? NeedAssess
        {
            get;
            set;
        }

        /// <summary>
        /// 申请单号
        /// </summary>
        public string AppCode { get; set; }

        /// <summary>
        /// 客户姓名
        /// </summary>
        public string CustomerName { get; set; }

        /// <summary>
        /// 当前页
        /// </summary>
        public int PageIndex { get; set; }

        /// <summary>
        /// 每页记录数
        /// </summary>
        public int PageSize { get; set; }

        Dictionary<string, string> sort = new Dictionary<string, string>();
        /// <summary>
        /// 排序，如ID:DESC
        /// </summary>
        public Dictionary<string, string> Sort
        {
            get { return sort; }
            set { sort = value; }
        }

        /// <summary>
        /// 客户经理代码
        /// </summary>
        public string SaleCode { get; set; }

        /// <summary>
        /// 客户经理姓名
        /// </summary>
        public string SaleName { get; set; }

        List<string> accessableCsac = new List<string>();
        /// <summary>
        /// 登录者可访问的数据所属人员
        /// </summary>
        public List<string> AccessableCsac
        {
            get { return accessableCsac; }
            set { accessableCsac = value; }
        }

        List<string> listLogo = new List<string>();
        /// <summary>
        /// 查询时用于筛选的产品logo
        /// </summary>
        public List<string> ListLogo
        {
            get { return listLogo; }
            set { listLogo = value; }
        }

        /// <summary>
        /// 查询APP_AUTH表的MENUCODE，在从不同菜单查询相同logo产品时使用
        /// </summary>
        public string InputMenuCode { get; set; }
    }
}
