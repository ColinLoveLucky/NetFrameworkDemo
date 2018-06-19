using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QK.QAPP.Entity
{
    public enum PInfoInterfaceURLAccount
    {
        /// <summary>
        /// 根据区域编号查询
        /// </summary>
        orgId=0,

        /// <summary>
        /// 根据品牌代码查询
        /// </summary>
        logo,

        /// <summary>
        /// 根据品牌主键(品牌编号)查询
        /// </summary>
        logoId,

        /// <summary>
        /// 根据品牌类型查询
        /// </summary>
        loanType,

        /// <summary>
        /// 根据产品代码查询
        /// </summary>
        productCode,

        /// <summary>
        /// 根据产品主键(产品编号)查询
        /// </summary>
        productId,
        /// <summary>
        /// 获取所有产品
        /// </summary>
        productList
    }
}
