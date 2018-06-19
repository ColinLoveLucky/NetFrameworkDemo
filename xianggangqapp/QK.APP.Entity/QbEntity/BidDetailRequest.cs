using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QK.QAPP.Entity
{
    public class BidDetailRequest
    {
        public string BidIdentifier { get; set; }
        /// <summary>
        /// 是否是返回所有数据
        /// </summary>
        public bool IsSelectAll { get; set; }
        /// <summary>
        /// 返回本人的数据
        /// </summary>
        public string AccountList { get; set; }
        /// <summary>
        /// 返回本部门/任意选择的部门数据
        /// </summary>
        public string ParentIdList { get; set; }
        /// <summary>
        /// 返回本公司的数据    
        /// </summary>
        public string CompanyList { get; set; }
    }
}
