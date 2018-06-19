using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QK.QAPP.Entity
{
    public class QFFileReadList
    {
        /// <summary>
        /// 申请编号
        /// </summary>
        public string bizCode
        {
            get;
            set;
        }

        /// <summary>
        /// 证件号码
        /// </summary>
        public string idNo
        {
            get;
            set;
        }

        /// <summary>
        /// 证件类型
        /// </summary>
        public string idType

        {
            get;
            set;
        }

        /// <summary>
        /// 客户名
        /// </summary>
        public string userName
        {
            get;
            set;
        }

        /// <summary>
        /// 文件类型
        /// </summary>
        public string fileType
        {
            get;
            set;
        }
    }
}
