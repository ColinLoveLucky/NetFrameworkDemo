/***********************
 * 作    者：王瑞
 * 创建时间：‎‎2015-12-03
 * 作    用：逾期状态
*****************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QK.QAPP.Entity
{
    public enum OverdueStatus
    {
        /// <summary>
        /// 未逾期
        /// </summary>
        NotOverdue,

        /// <summary>
        /// 已逾期
        /// </summary>
        Overdue
    }
}
