using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QK.QAPP.Entity
{
    public class OnlineUserViewField
    {
        /// <summary>
        /// 在线用户列表
        /// </summary>
        public List<OnlineUserTable> OnlineUserList { get; set; }
        /// <summary>
        /// 每页记录数
        /// </summary>
        public int Rows { get; set; }

        /// <summary>
        /// 总页数
        /// </summary>
        public int TotalPages { get; set; }

        /// <summary>
        /// 总记录数
        /// </summary>
        public int TotalRecords { get; set; }

        /// <summary>
        /// 当前页
        /// </summary>
        public int CurrentPage { get; set; }

    }

    public class OnlineUserTable
    {
        public string DISPLAYNAME { get; set; }
        public string COMPANYNAME { get; set; }
        public string DEPARTNAME { get; set; }
        public string ROLENAME { get; set; }
        public string USERNAME { get; set; }
        public DateTime? CREATETIME { get; set; }
        public string USERIP { get; set; }
        public string USERBROWSER { get; set; }
        public string USERBROWSERVERSION { get; set; }

        public DateTime? LASTUPDATETIME { get; set; }
        public string MACHINENAME { get; set; }

        public string COUNT { get; set; }
    }

    public class OnlineUserTotal
    {
        public int TOTALCOUNT { get; set; }
    }
}
