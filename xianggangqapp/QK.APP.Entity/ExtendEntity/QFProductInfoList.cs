using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QK.QAPP.Entity
{
    public class QFProductInfoList
    {
        public List<QFProductInfo> productInfoList { get; set; }
    }

    public class QFProductInfo
    {
        public QFPInterest pInterest { get; set; }
        public QFPProduct pProduct { get; set; }
        public QFPLogo pLogo { get; set; }
        public List<CRDataDic> crDataDic { get; set; }

    }

    public class QFPInterest
    {
        //服务费率
        public decimal serviceChargeRatio { get; set; }
      
        //罚息
        public decimal defaultInterestRatio { get; set; }
        //利率类型
        public string rateType { get; set; }
        //利率
        public decimal rate { get; set; }
        //滞纳金率
        public decimal lateFeeRatio { get; set; }
        //提前还款手续费比率
        public decimal prepaymentRatio { get; set; }
    }

    public class CRDataDic
    {
        //字段编码
        public string dataCode { get; set; }
        //字段名称
        public string dataName { get; set; }
        //数据类型
        public List<DataType> dataType { get; set; }
    }

    public class DataType
    {
        //字段编码
        public string dataCode { get; set; }
        //字段名称
        public string dataName { get; set; }
        //数据类型
    }

    public class QFPProduct
    {
        public int id { get; set; }
        public string productCode { get; set; }
        public string productName { get; set; }
        //咨询费率
        public decimal consultationChargeRatio { get; set; }
        public int termStart { get; set; }
        public int termEnd { get; set; }
        public int termInterval { get; set; }

        //客户类型
        public string fit4customerType { get; set; }
        //申请号中产品编码
        public string productRegularId { get; set; }
        //产品版本
        public string prodVersion { get; set; }
        //风险准备金率
        public decimal reservesRatio { get; set; }
    }

    public class QFPLogo
    {
        public int id { get; set; }
        public string logo { get; set; }
        public string logoName { get; set; }
        public string logoActive { get; set; }
    }
}
