using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QK.QAPP.Entity;

namespace QK.QAPP.IServices
{
    public partial interface IAPP_CITY_PRODUCTSERVICE
    {
        /// <summary>
        /// 根据菜单取城市产品关系列表（有缓存）
        /// </summary>
        /// <param name="menuGroup">菜单（如：CREDIT）</param>
        /// <returns></returns>
        List<APP_CITY_PRODUCT> FilterByMenuGroup(string menuGroup);

        /// <summary>
        /// 根据菜单与城市编码取城市产品关系（有缓存）
        /// </summary>
        /// <param name="cityCode">城市编码</param>
        /// <param name="menuGroup">菜单</param>
        /// <returns></returns>
        APP_CITY_PRODUCT FindByCityCodeAndMenuGroup(string cityCode, string menuGroup);

        /// <summary>
        /// 根据城市编码删除配置
        /// </summary>
        /// <param name="cityCode"></param>
        /// <returns></returns>
        bool DeleteByCityCode(string cityCode);

        /// <summary>
        /// 更新CITY_CODE
        /// </summary>
        /// <param name="oldCityCode"></param>
        /// <param name="newCityCode"></param>
        /// <returns></returns>
        bool UpdateCityCodeByCityCode(string oldCityCode, string newCityCode);
    }
}
