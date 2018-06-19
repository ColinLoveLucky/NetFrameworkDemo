/************************************************************************************
 *创建人：赵磊
 *创建时间：20150118
 *功能描述：封装额度查询、额度复核参数信息
 ************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QK.QAPP.Entity.QbEntity
{
    public class AmtLimitListPara : ListViewBase
    {
        /// <summary>
        /// 额度ID
        /// </summary>
        public string AMT_ID { get; set; }
        /// <summary>
        /// 额度类型
        /// </summary>
        public string AMT_TYPE { get; set; }

        /// <summary>
        /// 额度使用日期
        /// </summary>
        public string AMT_USE_DATE { get; set; }

        /// <summary>
        /// 额度部门
        /// </summary>
        public string AMT_DEPT { get; set; }

        /// <summary>
        /// 额度状态
        /// </summary>
        public string AMT_STATE { get; set; }

        /// <summary>
        /// 额度是否生效
        /// </summary>
        public string AMT_EFFECTIVE { get; set; }
        /// <summary>
        /// 描述：复核时：1:复核；0：非复核
        /// 额度操作历史时：1:财务部；0：非财务部
        /// </summary>
        public int IsAdjusted { get; set; }
    }
}
