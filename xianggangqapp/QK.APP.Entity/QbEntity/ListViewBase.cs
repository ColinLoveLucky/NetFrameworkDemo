/************************************************************************************
 *创建人：赵磊
 *创建时间：20160201
 *功能描述：定义查询变量，适用于所有接口查询
 ************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QK.QAPP.Entity
{
    public class ListViewBase
    {
        /// <summary>
        /// 每页记录数
        /// </summary>
        public int Rows { get; set; }

        /// <summary>
        /// 当前页
        /// </summary>
        public int CurrentPage { get; set; }

        /// <summary>
        /// 模糊查询关键字
        /// </summary>
        public string KEY_VALUE { get; set; }
    }
}
