/***************************************************************************
 * 描述：受托信息
 * 创建/修改时间：20160318
 * 创建/修改人：net team
 ***************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QK.QAPP.Entity.QBThirdEntity.ContractInfo
{
    public class TRUT_INFO
    {
        /// <summary>
        /// 受托人姓名
        /// </summary>
        public string USER_NAME { get; set; }
        /// <summary>
        /// 受托人身份证号
        /// </summary>
        public string CERT_NO { get; set; }

        /// <summary>
        /// 受托人住所
        /// </summary>
        public string USER_ADDR { get; set; }
    }
}
