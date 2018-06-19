using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace QK.QAPP.Entity
{
    public class QFFile
    {
        /// <summary>
        /// 文件ID
        /// </summary>
        public string FileId { get; set; }

        /// <summary>
        /// 文件原名
        /// </summary>
        public string OldFileName { get; set; }

        /// <summary>
        /// 文件现名
        /// </summary>
        public string NewFileName { get; set; }

        /// <summary>
        /// 文件地址
        /// </summary>
        public string ReturnUrl { get; set; }

    }
}
