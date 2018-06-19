using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QK.QAPP.Entity
{
  public  class MsgInfo<T>
    {
        /// <summary>
        /// 返回对象
        /// </summary>
        public T ReturnObj { get; set; }
        /// <summary>
        /// 返回状态
        /// </summary>
        public CMessageStatus Status { get; set; }
        /// <summary>
        /// 错误信息
        /// </summary>
        public string Error { get; set; }

    }
  public enum CMessageStatus
  {
      Fail,
      Success
  }
}
