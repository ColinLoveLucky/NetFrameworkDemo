using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QK.QAPP.Entity
{
    /// <summary>
    /// 请求上传接口的返回值
    /// </summary>
    public class QFFileUploadResult
    {
        /// <summary>
        /// 请求OK后返回的文件ID
        /// </summary>
        public string okTagMsg { get; set; }
    }
}
