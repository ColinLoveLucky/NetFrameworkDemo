using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QK.QAPP.Entity
{
    public class V_ORG_ROLE_USER
    {
        /// <summary>
        /// 对象名
        /// </summary>
        public string OBJECTID { get; set; }
        /// <summary>
        /// 夫级名
        /// </summary>
        public string PARENTID { get; set; }
        /// <summary>
        /// 公司名
        /// </summary>
        public string COMPANYNAME { get; set; }
        /// <summary>
        /// 部门名
        /// </summary>
        public string DEPARTNAME { get; set; }
        /// <summary>
        /// 对象名
        /// </summary>
        public string OBJECTNAME { get; set; }
        /// <summary>
        /// 对象类型
        /// </summary>
        public string OBJECTTYPE { get; set; }
        /// <summary>
        /// 对象值
        /// </summary>
        public string OBJECTVALUE { get; set; }
        /// <summary>
        /// 角色CODE
        /// </summary>
        public string ROLECODE { get; set; }
        /// <summary>
        /// 角色名
        /// </summary>
        public string ROLENAME { get; set; }
        /// <summary>
        /// 用户编号
        /// </summary>
        public string USERCODE { get; set; }

    }
}
