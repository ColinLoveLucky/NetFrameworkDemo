using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QK.QAPP.Entity;

namespace QK.QAPP.IServices
{
    public partial interface IAPP_AEOSERVICE
    {
        /// <summary>
        /// 获取优质企业列表
        /// </summary>
        /// <param name="aeoType">企业类型（AEO_TYPE）</param>
        /// <param name="keyWord">搜索关键字</param>
        /// <param name="iPageIndex">页码</param>
        /// <param name="iPageSize">每页显示条数</param>
        /// <returns></returns>
        ViewListByPage<APP_AEO> GetCompanyList(string aeoType,string keyWord,int iPageIndex,int iPageSize);

        /// <summary>
        /// 获取优质企业列表
        /// </summary>
        /// <param name="category">企业类型（AEO_CATEGORY）</param>
        /// <param name="keyWord">搜索关键字</param>
        /// <param name="iPageIndex">页码</param>
        /// <param name="iPageSize">每页显示条数</param>
        ViewListByPage<APP_AEO> ListByCategory(string category, string keyWord, int iPageIndex, int iPageSize);
    }
}
