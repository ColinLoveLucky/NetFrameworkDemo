using QK.QAPP.Entity;
using QK.QAPP.Infrastructure.Data.EFRepository.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QK.QAPP.IServices
{
    public partial interface IAPP_CITYSERVICE
    {
        /// <summary>
        /// 获取组织架构中的类型为folder的信息
        /// </summary>
        /// <returns></returns>
        List<V_ORG_ROLE_USER> GetOrgRoleList();

        IQueryable<APP_CITY> FilterByPlatform(string platform);
         /// <summary>
        /// 标的获取城市列表，不包括融予100的城市
        /// </summary>
        /// <returns></returns>
        List<APP_CITY> GetBidCityList();

        /// <summary>
        /// 根据菜单获取城市
        /// </summary>
        /// <param name="menuGroup"></param>
        /// <returns></returns>
        List<APP_CITY> FilterByMenuGroup(string menuGroup);
    }
}
