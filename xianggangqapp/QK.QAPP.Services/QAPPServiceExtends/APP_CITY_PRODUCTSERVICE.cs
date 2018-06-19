using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;
using QK.QAPP.Entity;
using QK.QAPP.Infrastructure.Cache;

namespace QK.QAPP.Services
{
    public partial class APP_CITY_PRODUCTSERVICE
    {
        [Dependency]
        public ICacheProvider CacheService { get; set; }

        public List<APP_CITY_PRODUCT> FilterByMenuGroup(string menuGroup)
        {
            string cacheKey = string.Format("QAPP_CityProduct_MenuGroup_{0}", menuGroup);
            return CacheService.GetFromCacheOrProxy(cacheKey, () =>
            {
                return Find(c => menuGroup.Equals(c.MENU_GROUP)).ToList();
            });
        }

        public APP_CITY_PRODUCT FindByCityCodeAndMenuGroup(string cityCode, string menuGroup)
        {
            string cacheKey = string.Format("QAPP_CityProduct_MenuGroup_{0}_CityCode_{1}", menuGroup, cityCode);
            return CacheService.GetFromCacheOrProxy(cacheKey, () =>
            {
                return FirstOrDefault(c => menuGroup.Equals(c.MENU_GROUP) && cityCode.Equals(c.CITY_CODE));
            });
        }

        public bool DeleteByCityCode(string cityCode)
        {
            if (string.IsNullOrEmpty(cityCode))
                return false;

            var result = Find(c => c.CITY_CODE == cityCode).ToList();
            return DeleteMultiple(result);
        }

        public bool UpdateCityCodeByCityCode(string oldCityCode, string newCityCode)
        {
            if (string.IsNullOrEmpty(oldCityCode) || string.IsNullOrEmpty(newCityCode))
                return false;

            var list = this.Find(c => c.CITY_CODE == oldCityCode).ToList();
            foreach (var item in list)
            {
                item.CITY_CODE = newCityCode;
            }

            return this.UpdateMultiple(list);
        }
    }
}
