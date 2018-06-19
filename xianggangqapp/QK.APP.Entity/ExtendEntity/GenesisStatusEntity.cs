using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QK.QAPP.Entity
{
    public class GenesisStatusEntity
    {
        /// <summary>
        /// Pboc认证状态 202：采集成功
        /// </summary>
        public string PbocStatus { get; set; }
        /// <summary>
        /// 公积金认证状态 202：采集成功
        /// </summary>
        public string FundStatus { get; set; }
        /// <summary>
        /// 网银认证状态 202：采集成功
        /// </summary>
        public string NetbankStatus { get; set; }
        /// <summary>
        /// 通话详单拉取状态 202：拉取成功
        /// </summary>
        public string MobileStatus { get; set; }
    }
}
