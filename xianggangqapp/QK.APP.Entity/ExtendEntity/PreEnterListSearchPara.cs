using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QK.QAPP.Entity
{
    /// <summary>
    /// 进件查询搜索条件
    /// </summary>
    public class PreEnterListSearchPara
    {

        /// <summary>
        /// 申请单号
        /// </summary>
        public string PreAppCode { get; set; }

        /// <summary>
        /// 客户姓名
        /// </summary>
        public string CustomerName { get; set; }

        /// <summary>
        /// 客户手机号码
        /// </summary>
        public string CustomerMobile { get;set;}

        /// <summary>
        /// 当前页
        /// </summary>
        public int PageIndex { get; set; }

        /// <summary>
        /// 每页记录数
        /// </summary>
        public int PageSize { get; set; }

        
        /// <summary>
        /// 客户经理代码
        /// </summary>
        public string SaleCode { get; set; }

        /// <summary>
        /// 客户经理姓名
        /// </summary>
        public string SaleName { get; set; }

        Dictionary<string, string> sort = new Dictionary<string, string>();
        /// <summary>
        /// 排序，如ID:DESC
        /// </summary>
        public Dictionary<string, string> Sort
        {
            get { return sort; }
            set { sort = value; }
        }

        List<PreEnterStatusType> listEnterStatus = new List<PreEnterStatusType>();
        /// <summary>
        /// 预申请查询时需要的进件状态
        /// <para>默认new...</para>
        /// </summary>
        public List<PreEnterStatusType> ListEnterStatus
        {
            get { return listEnterStatus; }
            set { listEnterStatus = value; }
        }
        List<string> listLogo = new List<string>();
        /// <summary>
        /// 进件查询时用于筛选的Logo产品
        /// <para>默认new...</para>
        /// </summary>
        public List<string> ListLogo
        {
            get { return listLogo; }
            set { listLogo = value; }
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
    }
}
