using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QK.QAPP.Entity
{
    /// <summary>
    /// 删除合同请求参数
    /// </summary>
    public class ContractDeleteRequest
    {

        /// <summary>
        /// 接入系统ID
        /// </summary>
        public string APP_ID { get; set; }
        /// <summary>
        /// 值为CONT_DELETE
        /// </summary>
        public string ACTION { get; set; }
        /// <summary>
        /// 申请的业务基本信息
        /// </summary>
        public _BIZ_INFO BIZ_INFO { get; set; }
        public class _BIZ_INFO
        {
            public List<BIZ_KEY_VAL> BASE_INFO { get; set; }
            public List<Dictionary<string, _FILE>> FILES { get; set; }
        }
        public class _FILE
        {
            public string FILE_ID { get; set; }
            public string KIND { get; set; }
        }
    }
}