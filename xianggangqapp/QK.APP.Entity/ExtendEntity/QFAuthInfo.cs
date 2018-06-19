using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QK.QAPP.Entity.ExtendEntity
{
    public class QFAuthInfo
    {
        /// <summary>
        /// 上级组织架构
        /// </summary>
        public string ParentOrganization { get; set; }

        /// <summary>
        /// 公司
        /// </summary>
        public string Company { get; set; }

        /// <summary>
        /// 区域
        /// </summary>
        public string Area { get; set; }

        /// <summary>
        /// 总公司
        /// </summary>
        public string Headquarters { get; set; }
    }
}
