using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QK.QAPP.Entity
{
    public class GenesisResult
    {
        /// <summary>
        /// 响应编码
        /// </summary>
        public int code { get; set; }
        /// <summary>
        /// 响应消息
        /// </summary>
        public string msg { get; set; }
        /// <summary>
        /// 结果数据
        /// </summary>
        public object data { get; set; }
    }
}
