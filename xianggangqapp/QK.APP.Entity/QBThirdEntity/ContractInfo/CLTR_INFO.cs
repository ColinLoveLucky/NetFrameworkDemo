/***************************************************************************
 * 描述：抵押物信息 
 * 创建/修改时间：20160310
 * 创建/修改人：net team
 ***************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QK.QAPP.Entity
{
    public class CLTR_INFO
    {
        /// <summary>
        /// 车牌号码
        /// </summary>
        public string CAR_NO { get; set; }
        /// <summary>
        /// 车辆品牌型号
        /// </summary>
        public string CAR_BRAD { get; set; }
        /// <summary>
        /// 车辆型号
        /// </summary>
        public string CAR_MODL_NO { get; set; }
        /// <summary>
        /// 车辆颜色
        /// </summary>
        public string CAR_COLR { get; set; }
        /// <summary>
        /// 发动机号
        /// </summary>
        public string CAR_ENGN_NO { get; set; }
        /// <summary>
        /// 车辆识别代号/车架号/vin码
        /// </summary>
        public string CAR_ID_NO { get; set; }

        /// <summary>
        /// 权属证书编号 无处可取，暂时留空
        /// </summary>
        public string CER_ID_NO { get; set; }
        /// <summary>
        /// 资产类型
        /// </summary>
       public string   ASST_TYPE {get;set;}
        /// <summary>
       /// 资产评估价值
        /// </summary>
       public string ASST_VAL { get; set; }
        /// <summary>
        /// 抵押率
        /// </summary>
       public string MOR_RATE { get; set; }
        /// <summary>
       /// FOTIC - 外贸信托展期协议-房屋抵押金额*1.2%
        /// </summary>
       public string HOUSE_MONEY { get; set; }

    }
}
