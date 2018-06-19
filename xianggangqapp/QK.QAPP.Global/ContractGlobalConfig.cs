using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QK.QAPP.Global
{
    /// <summary>
    /// 通过多线程访问合同配置
    /// </summary>
    public class ContractGlobalConfig
    {

        /// <summary>
        /// 合同接口地址
        /// </summary>
        public  string ContractUrl = GlobalSetting.ContractUrl;
        /// <summary>
        /// 标的调用合同系统的接入id
        /// </summary>
        public  string Contract_APP_ID = GlobalSetting.Contract_APP_ID;
        /// <summary>
        /// 调用合同系统请求参数BIZ_KEY的固定值
        /// </summary>
        public  string Contract_BIZ_ID = GlobalSetting.Contract_BIZ_ID;
        /// <summary>
        /// 调用合同系统请求参数BIZ_KEY的固定值
        /// </summary>
        public  string Contract_ID = GlobalSetting.Contract_ID;
        /// <summary>
        /// RA_CODE：RA（Registration Authority， 数字证书注册机构）的代码，默认值FDD
        /// </summary>
        public  string RA_CODE = GlobalSetting.RA_CODE;
        /// <summary>
        /// 试算接口地址
        /// </summary>
        public  string CalcURL = GlobalSetting.CalcURL;
        /// <summary>
        /// 合同生成接口ACTION参数
        /// </summary>
        public  string Contract_CONT_CREATE = GlobalSetting.Contract_CONT_CREATE;
        /// <summary>
        ///P2P FOTIC - 外贸信托 AVICTC - 中航信托 资金渠道和合同模板的对应关系
        /// </summary>
        public  Dictionary<string, List<string>> Contract_FundChannel = GlobalSetting.Contract_FundChannel;
        /// <summary>
        ///渠道和信托机构代码对应关系
        /// </summary>
        public Dictionary<string, List<string>> Contract_Channel_Trust = GlobalSetting.Contract_Channel_Trust;
        /// <summary>
        /// 渠道合同模板品牌的对应关系KEY值
        /// </summary>
        public  string Contract_BRAND_CODE = GlobalSetting.Contract_BRAND_CODE;
        /// <summary>
        /// 渠道合同模板资金渠道的对应关系品牌KEY值
        /// </summary>
        public  string Contract_CHANNEL_CODE = GlobalSetting.Contract_CHANNEL_CODE;
        /// <summary>
        /// 是否每次重新生成合同：FALSE,TRUE
        /// </summary>
        public string CONTRACT_IS_CREATE = GlobalSetting.CONTRACT_IS_CREATE;
        /// <summary>
        /// 回写固定还款日到标的系统接口地址
        /// </summary>
        public string QKSetRepayMentDay = GlobalApi.QKSetRepayMentDay;
        /// <summary>
        /// 回写签约时间
        /// </summary>
        public string QKSetBidSignedTime = GlobalApi.QKSetBidSignedTime;
        /// <summary>
        /// 回写合同生成时间和标示
        /// </summary>
        public string QKSetContractState = GlobalApi.QKSetContractState;
        /// <summary>
        /// GPS安装费
        /// </summary>
        public  string INST_FEE_GPS=GlobalSetting.INST_FEE_GPS;
        /// <summary>
        /// GPS服务费
        /// </summary>
        public  string SERV_FEE_GPS = GlobalSetting.SERV_FEE_GPS;
        /// <summary>
        /// 签名私钥
        /// </summary>
        public  string QbPartner=GlobalSetting.QbPartner;
        /// <summary>
        /// 签名公钥
        /// </summary>
        public  string QbPartnerKey=GlobalSetting.QbPartnerKey;
        
    }
}
