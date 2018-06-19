using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QK.QAPP.Entity
{
    /// <summary>
    /// 消息推送类型
    /// </summary>
    public enum PushMsgType
    {
        /// <summary>
        /// 申请完成
        /// </summary>
        ApplyFinished = 1001,
        /// <summary>
        /// 信贷补件开始
        /// </summary>
        SupplementStart = 1002,
        /// <summary>
        /// 信贷补件完成
        /// </summary>
        SupplementFinished = 1003,
        /// <summary>
        /// 系统重启
        /// </summary>
        SystemReStart = 1004,
        /// <summary>
        /// 车贷补件开始
        /// </summary>
        CarSupplementStart = 1005,
        /// <summary>
        /// 车贷补件完成
        /// </summary>
        CarSupplementFinished = 1006,
        /// <summary>
        /// 车贷待评估
        /// </summary>
        CarToBeAssessed = 1007,

        /// <summary>
        /// 房贷补件开始
        /// </summary>
        HouseSupplementStart = 1008
    }

}
