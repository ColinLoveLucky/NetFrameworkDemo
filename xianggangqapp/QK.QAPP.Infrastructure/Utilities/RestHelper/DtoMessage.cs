using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QK.QAPP.Infrastructure
{
    public class DtoMessage<T>
    {
        /// <summary>
        /// 返回对象
        /// </summary>
        public T ReturnObj { get; set; }
        /// <summary>
        /// 返回状态
        /// </summary>
        public DtoMessageStatus Status { get; set; }
        /// <summary>
        /// 错误信息
        /// </summary>
        public string Error { get; set; }
        public int CurPage { get; set; }
        public int PageSize { get; set; }
        public int TotalNum { get; set; }
    }
    public enum DtoMessageStatus
    {
        Fail,
        Success
    }
}
