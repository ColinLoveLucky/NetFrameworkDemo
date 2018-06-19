using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QK.QAPP.Entity
{
    public class QFFileLogTemp
    {
        /// <summary>
        /// 进件城市
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// 进件ID
        /// </summary>
        public long AppId { get; set; }

        /// <summary>
        /// 文件ID
        /// </summary>
        public long FileId { get; set; }

        /// <summary>
        /// 文件名
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// 文件类别
        /// </summary>
        public string FileType { get; set; }

        /// <summary>
        /// 文件大小
        /// </summary>
        public string FileSize { get; set; }

        /// <summary>
        /// 请求耗时
        /// </summary>
        public long RequestTime { get; set; }

        /// <summary>
        /// java接口耗时
        /// </summary>
        public long JavaApiTime { get; set; }

        /// <summary>
        /// 结果：OK 或 KO
        /// </summary>
        public string Result { get; set; }

        /// <summary>
        /// 信息
        /// </summary>
        public string Info { get; set; }
    }
}
