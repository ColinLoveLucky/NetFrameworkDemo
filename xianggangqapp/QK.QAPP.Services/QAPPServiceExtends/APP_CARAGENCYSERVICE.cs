using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QK.QAPP.Entity;
using QK.QAPP.Global;

namespace QK.QAPP.Services
{
    public partial class APP_CARAGENCYSERVICE
    {
        /// <summary>
        /// 获取车易贷经销商
        /// 修改人：张浩
        /// 修改日期：2016-04-26
        /// 修改原因：增加根据产品进行过滤
        /// </summary>
        /// <param name="product">产品logo</param>
        /// <param name="keyWord">查询关键字</param>
        /// <param name="iPageIndex">页码</param>
        /// <param name="iPageSize">每页行数</param>
        /// <returns></returns>
        public ViewListByPage<APP_CARAGENCY> GetCarAgencyList(string product, string keyWord, int iPageIndex, int iPageSize)
        {
            ViewListByPage<APP_CARAGENCY> carAgencyList = new ViewListByPage<APP_CARAGENCY>();
            IQueryable<APP_CARAGENCY> query;
            product=product.ToLower();  //产品logo转为小写
            if (GlobalSetting.ShowAllAgencyOfCar)
            {
                query = string.IsNullOrEmpty(keyWord) ?
                    this.Find(p => p.MOTO_PRODUCT == product).OrderBy(o => o.MOTO_NAME) :
                    this.Find(p => p.MOTO_PRODUCT == product && p.MOTO_NAME.Contains(keyWord)).OrderBy(o => o.MOTO_NAME);
            }
            else
            {
                query = string.IsNullOrEmpty(keyWord) ?
                    this.Find(p => p.MOTO_PRODUCT == product && p.IS_USE == "1").OrderBy(o => o.MOTO_NAME) :
                    this.Find(p => p.MOTO_PRODUCT == product && p.IS_USE == "1" && p.MOTO_NAME.Contains(keyWord)).OrderBy(o => o.MOTO_NAME);
            }
            carAgencyList.SetParameters(query, iPageIndex, iPageSize);
            return carAgencyList;
        }
    }
}
