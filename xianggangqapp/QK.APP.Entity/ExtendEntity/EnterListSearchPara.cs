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
    public class EnterListSearchPara
    {
        List<EnterStatusType> listEnterStatus = new List<EnterStatusType>();
        /// <summary>
        /// 进件查询时用于筛选的进件状态
        /// <para>默认new...</para>
        /// </summary>
        public List<EnterStatusType> ListEnterStatus
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

        /// <summary>
        /// 按照修改时间进行查询
        /// </summary>
        public bool NeedTag { get; set; }

        /// <summary>
        /// 申请单号
        /// </summary>
        public string AppCode { get; set; }

        /// <summary>
        /// 客户姓名
        /// </summary>
        public string CustomerName { get; set; }

        /// <summary>
        /// 客户身份证号
        /// </summary>
        public string CustomerIDCard { get; set; }

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
        /// 申请时间起
        /// </summary>
        public DateTime? ApplyStart { get; set; }

        /// <summary>
        /// 申请时间止
        /// </summary>
        public DateTime? ApplyEnd { get; set; }

        /// <summary>
        /// 客户经理代码
        /// </summary>
        public string SaleCode { get; set; }

        /// <summary>
        /// 客户经理姓名
        /// </summary>
        public string SaleName { get; set; }

        /// <summary>
        /// 客服代码
        /// </summary>
        public string CsacCode { get; set; }

        /// <summary>
        /// 客服姓名
        /// </summary>
        public string CsacName { get; set; }

        List<string> accessableCsac = new List<string> ();
        /// <summary>
        /// 登录者可访问的数据所属人员
        /// </summary>
        public List<string> AccessableCsac
        {
            get { return accessableCsac; }
            set { accessableCsac = value; }
        }

        /// <summary>
        /// 查询APP_AUTH表的MENUCODE，在从不同菜单查询相同logo产品时使用
        /// </summary>
        public string InputMenuCode { get; set; }
    }
}
