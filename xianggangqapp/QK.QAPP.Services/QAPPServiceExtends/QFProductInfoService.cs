/*********************
 * 作者：吕冉
 * 时间：2014/9/22
 * 功能：获取产品信息
**********************/
using Microsoft.Practices.Unity;
using QK.QAPP.Entity;
using QK.QAPP.Global;
using QK.QAPP.Infrastructure;
using QK.QAPP.Infrastructure.Cache;
using QK.QAPP.IServices;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.ObjectBuilder2;


namespace QK.QAPP.Services
{
    public class QFProductInfoService : IQFProductInfoService
    {

        [Dependency]
        public ICacheProvider CacheService { get; set; }

        private List<QFPInterest> interestList;
        private List<QFPLogo> logoList;
        private List<QFPProduct> productList;
        private List<CRDataDic> crDataDicList;
        private List<QFProductInfo> productInfoList;
        private QFProductInfo product;

        private void GetQFProductInfoList(string urlAccount, string selectKey)
        {
            interestList = new List<QFPInterest>();
            logoList = new List<QFPLogo>();
            productList = new List<QFPProduct>();
            crDataDicList = new List<CRDataDic>();

            string ProductInfoInterfaceUrl = GlobalSetting.ProductionInfoInterfaceURL.Replace("{account}", urlAccount);
            RestHelper restClient = new RestHelper(ProductInfoInterfaceUrl);
            var QFProduct = CacheService.GetFromCacheOrProxy<QFProductInfoList>(
                "QAPP_SERVICE_GetQFProductInfoList_" + urlAccount + "_" + selectKey,
                () => restClient.Get<QFProductInfoList>(selectKey).ReturnObj);

            if (QFProduct != null && QFProduct.productInfoList != null)
            {
                //排序
                QFProduct.productInfoList = QFProduct.productInfoList
                    .OrderBy(p => p.pLogo.id)
                    .ThenBy(p => p.pProduct.termStart).ToList();
            }

            if (QFProduct == null || QFProduct.productInfoList == null)
            {
                CacheService.Remove("QAPP_SERVICE_GetQFProductInfoList_" + urlAccount + "_" + selectKey);
                QFProduct = CacheService.GetFromCacheOrProxy<QFProductInfoList>(
                    "QAPP_SERVICE_GetQFProductInfoList_" + urlAccount + "_" + selectKey,
                    () => restClient.Get<QFProductInfoList>(selectKey).ReturnObj);
            }
            else
            {
                foreach (var item in QFProduct.productInfoList)
                {
                    interestList.Add(item.pInterest);
                    logoList.Add(item.pLogo);
                    productList.Add(item.pProduct);
                    crDataDicList.AddRange(item.crDataDic);
                }
            }

        }
        private void GetQFProductInfoLists(string urlAccount, string selectKey)
        {
            interestList = new List<QFPInterest>();
            logoList = new List<QFPLogo>();
            productList = new List<QFPProduct>();
            crDataDicList = new List<CRDataDic>();
            productInfoList = new List<QFProductInfo>();
            string strFuHao = "-";
            if (selectKey == null || selectKey == "")
            {
                selectKey = "";
                strFuHao = "";
            }
            string ProductInfoInterfaceUrl = GlobalSetting.ProductionInfoInterfaceURL.Replace("{account}", urlAccount);
            RestHelper restClient = new RestHelper(ProductInfoInterfaceUrl);
            var QFProduct = CacheService.GetFromCacheOrProxy<QFProductInfoList>(
                "QAPP_SERVICE_GetQFProductInfoList_" + urlAccount + strFuHao + selectKey,
                () => restClient.Get<QFProductInfoList>(selectKey).ReturnObj);

            if (QFProduct != null && QFProduct.productInfoList != null)
            {
                //排序
                QFProduct.productInfoList = QFProduct.productInfoList
                    .OrderBy(p => p.pLogo.id)
                    .ThenBy(p => p.pProduct.productCode)
                    .ThenBy(p => p.pProduct.termStart).ToList();
            }
            productInfoList = QFProduct.productInfoList;
        }
        public List<QFPInterest> GetInterestList(string urlAccount, string selectKey)
        {
            GetQFProductInfoLists(urlAccount, selectKey);
            return productInfoList.Select(p => p.pInterest).ToList();
            //return interestList;
        }

        public List<QFPLogo> GetLogoList(string urlAccount, string selectKey)
        {
            GetQFProductInfoLists(urlAccount, selectKey);
            return productInfoList.Select(p => p.pLogo).ToList();
            //return logoList;
        }

        public List<QFPProduct> GetProductList(string urlAccount, string selectKey)
        {
            GetQFProductInfoLists(urlAccount, selectKey);
            return productInfoList.Select(p => p.pProduct).ToList();
            //return productList;
        }
        /// <summary>
        /// 获取所有产品列表
        /// </summary>
        /// <param name="urlAccount">接口地址参数</param>
        /// <returns></returns>
        public List<QFProductInfo> GetProductAllList(string urlAccount)
        {
            GetQFProductInfoLists(urlAccount, String.Empty);
            return productInfoList;
        }
        /// <summary>
        /// 根据产品productCode获取产品列表
        /// </summary>
        /// <param name="urlAccount">接口地址参数</param>
        /// <param name="selectKey">查询键</param>
        /// <returns></returns>
        public QFProductInfo GetProductListByProductCode(string urlAccount, string selectKey)
        {
            GetQFProductInfoLists(urlAccount, selectKey);
            if (productInfoList != null)
            {
                product = productInfoList.FirstOrDefault(o => o.pProduct.productCode == selectKey);
            }
            return product;
        }
        /// <summary>
        /// 根据logoCode获取logo列表
        /// </summary>
        /// <param name="urlAccount">接口地址参数</param>
        /// <param name="logoCode">查询键</param>
        /// <returns></returns>
        public List<QFPLogo> GetLogoListByLogoCode(string urlAccount, string logoCode)
        {
            GetQFProductInfoLists(urlAccount, "");
            if (productInfoList != null && logoCode != "" && logoCode != null)
            {
                logoList = productInfoList.Where(a => a.pLogo.logo == logoCode).Select(b => b.pLogo).ToList();
            }
            else if (productInfoList != null)
            {
                logoList = productInfoList.Select(b => b.pLogo).ToList();
            }
            return logoList;
        }

        public List<QFProductInfo> GetProductListByLogo(string urlAccount, string selectKey)
        {
            GetQFProductInfoLists(urlAccount, selectKey);
            return productInfoList;
        }

        private List<CRDataDic> GetCRDataDicList(string urlAccount, string selectKey)
        {
            GetQFProductInfoLists(urlAccount, selectKey);

            productInfoList.ForEach(p => crDataDicList.AddRange(p.crDataDic));

            return crDataDicList;
        }

        //获取产品期数
        public List<int> GetQFProductTerm(string pCode)
        {
            List<int> qfProductTerm = new List<int>();
            var product = GetProductListByProductCode(PInfoInterfaceURLAccount.productCode.ToString(), pCode);
            if (product == null)
                return qfProductTerm;
            QFPProduct p = product.pProduct;
            if (p != null)
            {
                if (p.termEnd <= p.termStart)
                {
                    qfProductTerm.Add(p.termStart);
                }
                else
                {
                    int terms = (p.termEnd - p.termStart + p.termInterval) / p.termInterval;
                    for (int i = 0; i < terms; i++)
                    {
                        if (i == 0)
                        {
                            qfProductTerm.Add(p.termStart);
                        }
                        else
                        {
                            qfProductTerm.Add(p.termStart + p.termInterval);
                        }
                    }
                }
            }
            return qfProductTerm;
        }

        //获取还款方式
        public List<DataType> GetRePayTypeList(string orgId)
        {
            List<DataType> dataTypeList = new List<DataType>();
            var dataDicList = GetCRDataDicList(PInfoInterfaceURLAccount.orgId.ToString(), orgId);
            foreach (var item in dataDicList)
            {
                if (item.dataCode == GlobalDict.QAPP_REPAY_TYPE)
                {
                    dataTypeList.AddRange(item.dataType);
                }
            }
            //根据dataCode去重
            return dataTypeList.Distinct(c => c.dataCode);
        }

        /// <summary>
        /// 按产品获取还款方式
        /// </summary>
        /// <param name="productCode"></param>
        /// <returns></returns>
        public List<DataType> GetRepayTypeByProductCode(string productCode)
        {
            List<DataType> dataTypeList = new List<DataType>();
            var dataDicList = GetCRDataDicList(PInfoInterfaceURLAccount.productCode.ToString(), productCode);
            foreach (var item in dataDicList)
            {
                if (item.dataCode == GlobalDict.QAPP_REPAY_TYPE)
                {
                    dataTypeList.AddRange(item.dataType);
                }
            }
            //根据dataCode去重
            return dataTypeList.Distinct(c => c.dataCode);
        }

        /// <summary>
        /// 从已获取的产品数据中拿还款方式
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        public List<DataType> GetRepayTypeFromProInfo(QFProductInfo product)
        {
            List<DataType> dataTypeList = new List<DataType>();
            if (product != null)
            {
                var dataDicList = product.crDataDic;
                foreach (var item in dataDicList)
                {
                    if (item.dataCode == GlobalDict.QAPP_REPAY_TYPE)
                    {
                        dataTypeList.AddRange(item.dataType);
                    }
                }
            }

            return dataTypeList.Distinct(c => c.dataCode);
        }

        //public class QFPLogoComparer : IEqualityComparer<QFPLogo>
        //{

        //    public bool Equals(QFPLogo x, QFPLogo y)
        //    {
        //        if (x.logo == y.logo)
        //        {
        //            return true;
        //        }
        //        else
        //        {
        //            return false;
        //        }
        //    }

        //    public int GetHashCode(QFPLogo obj)
        //    {
        //        //Check whether the object is null
        //        if (Object.ReferenceEquals(obj, null)) return 0;

        //        //Get hash code for the Name field if it is not null.
        //        int hashLogoCode = obj.logo == null ? 0 : obj.logo.GetHashCode();

        //        //Calculate the hash code for the product.
        //        return hashLogoCode;

        //    }
    }
}
