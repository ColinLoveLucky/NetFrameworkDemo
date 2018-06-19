using Microsoft.Practices.Unity;
using QK.QAPP.Entity;
using QK.QAPP.Infrastructure.Cache;
using QK.QAPP.Infrastructure.Data.EFRepository.Repository;
using QK.QAPP.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QK.QAPP.Services
{
    public partial class APP_CITYSERVICE
    {
        [Dependency]
        public IV_ORG_ROLE_USERService OrgRoleService { get; set; }

        [Dependency]
        public ICacheProvider CacheService { get; set; }

        [Dependency]
        public IAPP_CITY_PRODUCTSERVICE CityProductService { get; set; }
        public List<V_ORG_ROLE_USER> GetOrgRoleList()
        {
            return CacheService.GetFromCacheOrProxy<List<V_ORG_ROLE_USER>>("QAPP_GetOrgRoleList_folder", () =>
            {
                StringBuilder sql = new StringBuilder();
                sql.Append(" SELECT * FROM V_ORG_ROLE_USER ");
                sql.Append(" WHERE OBJECTTYPE='folder' ");
                sql.Append(" ORDER BY COMPANYNAME ");

                var list = OrgRoleService.SqlQuery<V_ORG_ROLE_USER>(sql.ToString()).ToList();

                return list;
            });
        }

        public IQueryable<APP_CITY> FilterByPlatform(string platform)
        {
            return this.GetQuery(c => platform.Equals(c.PLATFORM));
        }
        /// <summary>
        /// 标的获取城市列表，不包括融予100的城市
        /// </summary>
        /// <returns></returns>
        public List<APP_CITY> GetBidCityList()
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("SELECT * FROM APP_CITY C ");
            sql.Append(" WHERE C.CITY_CODE IN ( SELECT  CP.CITY_CODE FROM APP_CITY_PRODUCT CP WHERE CP.MENU_GROUP <> 'RY100')  ");
            var list =SqlQuery(sql.ToString());
            return list.ToList();
        }

        public List<APP_CITY> FilterByMenuGroup(string menuGroup)
        {
            string cacheKey = string.Format("QAPP_AppCity_MenuGroup_{0}", menuGroup);
            var cityCodeList = CityProductService
                .FilterByMenuGroup(menuGroup)
                .Where(c=>c.ENABLE == 1)
                .Select(c=>c.CITY_CODE);

            return CacheService.GetFromCacheOrProxy<List<APP_CITY>>(cacheKey, () =>
            {
                return GetQuery(c => cityCodeList.Contains(c.CITY_CODE)).ToList();
            });
        }

    }
}
