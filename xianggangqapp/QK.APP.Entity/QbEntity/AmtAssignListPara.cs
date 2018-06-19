using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QK.QAPP.Entity
{
    /// <summary>
    /// 额度分配列表参数对象
    /// </summary>
    public class AmtAssignListPara : ListViewBase
    {
        /// <summary>
        /// 区域ID
        /// </summary>
        public string Area { get; set; }

        /// <summary>
        /// 额度使用日期
        /// </summary>
        public string UseDate { get; set; }
    }
}
