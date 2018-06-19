/* ==============================================================================
 * 功能描述：拒贷信息IAPP_REFUSELOANSERVICE  
 * 创 建 者：leiz
 * 创建日期：2015/3/6 17:50:37
 * ==============================================================================*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QK.QAPP.Entity;
using QK.QAPP.Infrastructure.Data.EFRepository.Repository;

namespace QK.QAPP.IServices
{
    public partial interface IAPP_REFUSELOANSERVICE : IRepositoryBaseSql
    {
        /// <summary>
        /// 获取1级人拒贷信息
        /// </summary>
        /// <param name="appid">appid</param>
        /// <returns></returns>
        String GetRefuseLoanInfo(string appid);
    }
}
