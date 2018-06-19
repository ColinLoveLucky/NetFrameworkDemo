using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QK.QAPP.Entity
{
    /// <summary>
    /// 返回参数
    /// </summary>
    public class ContractResultMsg
    {

        /// <summary>
        /// SUCCESS / ERROR
        /// </summary>
        public string RESULT { get; set; }
        /// <summary>
        /// 10001 – 参数不正确
        /// </summary>
        public string CODE { get; set; }
        /// <summary>
        /// 手动签章新URL，返回SUCCESS时有
        /// </summary>
        public string SIGN_URL { get; set; }
        /// <summary>
        /// 文件列表
        /// </summary>
        public FILE_LIST FILE_LIST { get; set; }
    }
}
