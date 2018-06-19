using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QK.QAPP.Entity
{
    public class CR_DATA_DIC
    {
        /// <summary>
        /// 字典主键
        /// </summary>
        public long ID { get; set; }
        /// <summary>
        /// 父键
        /// </summary>
        public Nullable<long> PARENT_ID { get; set; }
        /// <summary>
        /// 数据代码
        /// </summary>
        public string DATA_CODE { get; set; }
        /// <summary>
        /// 数据类型
        /// </summary>
        public string DATA_TYPE { get; set; }
        /// <summary>
        /// 数据值
        /// </summary>
        public string DATA_NAME { get; set; }
        /// <summary>
        ///  排序类型
        /// </summary>
        public string DATA_DESC { get; set; }
        /// <summary>
        /// 排序值
        /// </summary>
        public Nullable<int> ORDER_ID { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public string STATUS { get; set; }
        /// <summary>
        /// 创建者
        /// </summary>
        public string CREATED_USER { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public Nullable<DateTime> CREATED_TIME { get; set; }
        /// <summary>
        /// 修改者
        /// </summary>
        public string CHANGED_USER { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public Nullable<DateTime> CHANGED_TIME { get; set; }
    }
}
