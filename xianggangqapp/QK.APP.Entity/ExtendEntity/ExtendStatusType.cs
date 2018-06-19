/***********************
 * 作    者：刘云松
 * 创建时间：‎‎2015-3-20 14:22:07
 * 作    用：提供（车贷）展期状态
*****************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QK.QAPP.Entity
{
    public enum ExtendStatusType
    {
        /// <summary>
        /// 展期已结清
        /// </summary>
        CarExtendClear,

        /// <summary>
        /// 展期未结清
        /// </summary>
        CarExtendNotClear
    }
}
