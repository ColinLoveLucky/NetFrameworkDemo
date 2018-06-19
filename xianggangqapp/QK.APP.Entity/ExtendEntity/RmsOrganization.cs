using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QK.QAPP.Entity.ExtendEntity
{
    public partial class RmsOrganization
    {
        /// <summary>
        /// 主键
        /// </summary>
        public string ORGANIZATIONID { get; set; }

        /// <summary>
        /// 父级ID
        /// </summary>
        public string PARENTID { get; set; }

        /// <summary>
        /// 编码
        /// </summary>
        public string CODE { get; set; }

        /// <summary>
        /// 简称
        /// </summary>
        public string SHORTNAME { get; set; }

        /// <summary>
        /// 全称
        /// </summary>
        public string FULLNAME { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        public string CATEGORY { get; set; }
    }
}
