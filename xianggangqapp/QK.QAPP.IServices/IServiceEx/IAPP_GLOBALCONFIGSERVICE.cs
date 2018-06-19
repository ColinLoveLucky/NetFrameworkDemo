using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QK.QAPP.IServices
{
    public partial interface IAPP_GLOBALCONFIGSERVICE
    {
        /// <summary>
        /// 通过键获取系统配置值
        /// </summary>
        /// <param name="key">键</param>
        /// <returns>值</returns>
        string GetValueByKey(string key);

        /// <summary>
        /// 清理缓存
        /// </summary>
        void ClearCacheByKey(string key);
    }


}
