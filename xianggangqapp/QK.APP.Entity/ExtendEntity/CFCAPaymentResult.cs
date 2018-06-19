using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QK.QAPP.Entity
{
    public class CFCAPaymentResult
    {
        /// <summary>
        /// 0.参数错误 1.成功 2.失败 3.处理中
        /// </summary>
        public int Status { get; set; }

        public string VerityToken { get; set; }

        /// <summary>
        /// 错误代码
        /// </summary>
        public string ErrorCode { get; set; }

        /// <summary>
        /// 返回消息
        /// </summary>
        public string ResultMessage { get; set; }
    }
}
