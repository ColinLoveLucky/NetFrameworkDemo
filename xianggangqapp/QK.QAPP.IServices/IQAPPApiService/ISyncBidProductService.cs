using QK.QAPP.Entity;
using System.Collections.Generic;

namespace QK.QAPP.IServices
{
    public interface ISyncBidProductService
    {
        /// <summary>
        /// 根据产品代码 更新App_Main表 品牌，产品代码，产品名称，产品版本,更新 app_queue表,更新app_loan表。 
        /// 
        /// 2016年5月9日15:57:19 by shawn
        /// </summary>
        /// <param name="formDic"></param>
        /// <param name="resultMsg"></param>
        /// <returns></returns>
        APP_MAIN UpdateMainByProductCode(Dictionary<string, string> formDic, QFProductInfo product, out string resultMsg);
    }
}
