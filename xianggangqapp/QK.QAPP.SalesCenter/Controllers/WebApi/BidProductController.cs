using QK.QAPP.Entity;
using QK.QAPP.Infrastructure;
using QK.QAPP.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QK.QAPP.SalesCenter.Controllers
{
    public class BidProductController : Controller
    {
        public IQFProductInfoService ProductInfoService { get; set; }

        public ISyncBidProductService SyncBidProductService { get; set; }


        #region 接收标的系统变更产品时推送的变更数据
        public JsonResult SyncBidProudctByProductCode(string appCode, string productCode, string contractNo, string version, string appkey, string timeStamp, string sign)
        {
            if (string.IsNullOrWhiteSpace(appCode))
                return new JsonResult() { Data = new SyncProductResponse() { BizErrorMsg = "  申请编号：appCode  不能为空 " } };

            if (string.IsNullOrWhiteSpace(productCode))
                return new JsonResult() { Data = new SyncProductResponse() { BizErrorMsg = " 产品代码：productCode 不能为空 " } };

            if (string.IsNullOrWhiteSpace(contractNo))
                return new JsonResult() { Data = new SyncProductResponse() { BizErrorMsg = " 合同号： productCode 不能为空 " } };

            var request = new SyncProductRequest
            {
                AppCode = appCode,
                ContractNo = contractNo,
                ProductCode = appCode,
                AppKey = appkey,
                Version = version,
                Sign = sign,
                TimeStamp = timeStamp,
            };

            // 验证api用户是否合法
            if (!AppAccountBiz.Instance.Validate(request.GetSign(), request.GetSysParamDic()))
                throw new Exception(" 身份验证失败,AppKey与AppSecret不匹配 ");

            return SyncBidProudctByProductCode(request);
        }

        /// <summary>
        /// 接收标的系统变更产品时推送的变更数据
        /// 2016年5月9日13:26:17  by shawn
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public JsonResult SyncBidProudctByProductCode(SyncProductRequest request)
        {
            #region 1.验证参数
            var product = ProductInfoService.GetProductListByProductCode(PInfoInterfaceURLAccount.productCode.ToString(), request.ProductCode);

            if (product == null)
                return new JsonResult() { Data = new SyncProductResponse() { BizErrorMsg = " 该产品不存在! " } };
            #endregion

            #region 2.更新业务信息
            var dic = new Dictionary<string, string>();

            dic.Add("APP_CODE", request.AppCode);
            dic.Add("ContractNo", request.ContractNo);

            if (product.pLogo != null)
                dic.Add("LOGO", product.pLogo.logo);

            if (product.pProduct != null)
            {
                dic.Add("PRODUCT_CODE", product.pProduct.productCode);
                dic.Add("PRODUCT_NAME", product.pProduct.productName);
                dic.Add("PROD_VERSION", product.pProduct.prodVersion);
            }

            string resultMsg = string.Empty;

            SyncBidProductService.UpdateMainByProductCode(dic, product, out resultMsg);

            #endregion

            if (!string.IsNullOrWhiteSpace(resultMsg))
                new JsonResult() { Data = new SyncProductResponse() { BizErrorMsg = resultMsg } };

            return new JsonResult() { Data = new SyncProductResponse() { BizErrorMsg = resultMsg } };
        }
        #endregion

        #region 请求相关类
        public class SyncProductRequest : BaseApiRequest
        {
            /// <summary>
            /// 申请编号
            /// </summary>
            public string AppCode { get; set; }

            /// <summary>
            /// 产品代码
            /// </summary>
            public string ProductCode { get; set; }

            /// <summary>
            ///  合同号
            /// </summary>
            public string ContractNo { get; set; }
        }


        public class SyncProductResponse : BaseApiResponse
        {
        }
        #endregion
    }
}