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
    public class AutoJobLogListPara : ListViewBase
    {
        /// <summary>
        /// 日志类型
        /// </summary>
        public string LogType { get; set; }
        /// <summary>
        /// 任务类型
        /// </summary>
        public string JobType { get; set; }

        /// <summary>
        /// 额度类型
        /// </summary>
        public string AmtType { get; set; }

        /// <summary>
        /// 额度使用日期
        /// </summary>
        public string UseDate { get; set; }
    }
}
