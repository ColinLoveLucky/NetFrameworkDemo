/*************************************************************************************
 * CLR版本：4.0.30319.34209
 * 命名空间：QK.QAPP.Entity
 * 类 名 称：APP_MAIN_SYSDISUSED_WEEKEND
 * 文 件 名：APP_MAIN_SYSDISUSED_WEEKEND
 * 用户所在的域：QUARK
 * 机器名称：QF-LEIZ-01
 * 作    者：leiz
 * 创建时间：2016/4/8 21:03:12
 * 描    述：非工作日实体类
*************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QK.QAPP.Entity
{
    public class APP_MAIN_SYSDISUSED_WEEKEND
    {
        /// <summary>
        /// 非工作日期
        /// </summary>
        public DateTime WEEKEND_DATE { get; set; }
        /// <summary>
        /// 星期几
        /// </summary>
        public string WEEK_NAME { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string MEMO { get; set; }
    }
}
