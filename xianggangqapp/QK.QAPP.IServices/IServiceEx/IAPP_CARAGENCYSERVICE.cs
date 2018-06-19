using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QK.QAPP.Entity;

namespace QK.QAPP.IServices
{
   public partial interface IAPP_CARAGENCYSERVICE
    {
       /// <summary>
       /// 获取经销商列表
       /// </summary>
        /// <param name="product">产品logo</param>
       /// <param name="keyWord">搜索关键字</param>
       /// <param name="iPageIndex">页码</param>
       /// <param name="iPageSize">每页显示条数</param>
       /// <returns></returns>
       ViewListByPage<APP_CARAGENCY> GetCarAgencyList(string product, string keyWord, int iPageIndex, int iPageSize);
    }
}
