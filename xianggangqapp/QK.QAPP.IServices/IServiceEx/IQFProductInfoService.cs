using QK.QAPP.Entity;
using QK.QAPP.Infrastructure.Cache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace QK.QAPP.IServices
{
    /// <summary>
    /// 获取产品信息
    /// </summary>
    public interface IQFProductInfoService
    {
        /// <summary>
        /// 获取产品利率信息相关列表
        /// </summary>
        /// <param name="urlAccount">接口地址参数</param>
        /// <param name="selectKey">查询键</param>
        /// <returns></returns>
        List<QFPInterest> GetInterestList(string urlAccount, string selectKey);

        /// <summary>
        /// 获取产品品牌信息相关列表
        /// </summary>
        /// <param name="urlAccount">接口地址参数</param>
        /// <param name="selectKey">查询键</param>
        /// <returns></returns>
        List<QFPLogo> GetLogoList(string urlAccount, string selectKey);

        /// <summary>
        /// 获取产品信息相关列表
        /// </summary>
        /// <param name="urlAccount">接口地址参数</param>
        /// <param name="selectKey">查询键</param>
        /// <returns></returns>
        List<QFPProduct> GetProductList(string urlAccount, string selectKey);
        /// <summary>
        /// 获取所有产品信息相关列表
        /// </summary>
        /// <param name="urlAccount">接口地址参数</param>
        /// <returns></returns>
        List<QFProductInfo> GetProductAllList(string urlAccount);
        /// <summary>
        /// 根据产品productCode获取产品列表
        /// </summary>
        /// <param name="urlAccount">接口地址参数</param>
        /// <param name="selectKey">查询键</param>
        /// <returns></returns>
       QFProductInfo GetProductListByProductCode(string urlAccount, string selectKey);
         /// <summary>
         /// 根据logoCode获取logo列表
         /// </summary>
        /// <param name="urlAccount">接口地址参数</param>
        /// <param name="logoCode">查询键</param>
         /// <returns></returns>
         List<QFPLogo> GetLogoListByLogoCode(string urlAccount, string logoCode);
        /// <summary>
        /// 获取产品期数
        /// </summary>
        /// <param name="pCode">产品编码</param>
        /// <returns></returns>
        List<int> GetQFProductTerm(string pCode);

        /// <summary>
        /// 获取还款方式
        /// </summary>
        /// <param name="urlAccount">接口地址参数</param>
        /// <param name="selectKey"></param>
        /// <returns></returns>
        List<DataType> GetRePayTypeList(string orgId);

        /// <summary>
        /// 根据logo获取产品列表
        /// </summary>
        /// <param name="urlAccount">接口地址参数</param>
        /// <param name="logo">查询键</param>
        /// <returns></returns>
        List<QFProductInfo> GetProductListByLogo(string urlAccount, string logo);

        /// <summary>
        /// 按产品获取还款方式
        /// </summary>
        /// <param name="productCode"></param>
        /// <returns></returns>
        List<DataType> GetRepayTypeByProductCode(string productCode);

        /// <summary>
        /// 从已获取的产品数据中拿还款方式
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        List<DataType> GetRepayTypeFromProInfo(QFProductInfo product);
    }
}
