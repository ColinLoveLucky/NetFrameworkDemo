using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QK.QAPP.Entity
{
    public class BIZ_INFO
    {
        /// <summary>
        /// BASE_INFO：该笔申请的业务基本信息，唯一标识一笔合同生成申请，比如进件ID等。
        /// </summary>
        public List<BIZ_KEY_VAL> BASE_INFO { get; set; }
        /// <summary>
        ///  BIZ_ENTS：一笔合同有若干个业务实体组成，比如交易信息、借款人信息、抵押物信息、担保信息等，每种业务实体为一个BIZ_ENT。
        /// </summary>
        public Object BIZ_ENTS { get; set; }
        
    }

    /// <summary>
    /// BIZ_ENTS：一笔合同有若干个业务实体组成，比如交易信息、借款人信息、抵押物信息、担保信息等，每种业务实体为一个BIZ_ENT。
    ///业务实体BIZ_ENTS内的蓝色字体部分：BIZ_KEY为业务实体的名称，BIZ_ENT为该业务实体所包含的数据实体字段。
    ///具体的字段定义，详情请参考“合同管理平台_业务字段字典”
    /// </summary>
    public class BIZ_ENTS<T>
    {
        /// <summary>
        /// BIZ_KEY为业务实体的名称
        /// </summary>
        public string BIZ_KEY
        {
            get {
                if (typeof(T).IsGenericType)
                {
                    return typeof(T).GenericTypeArguments.First().Name;
                }
                return typeof(T).Name; }
        }
        /// <summary>
        /// BIZ_ENT为该业务实体所包含的数据实体字段
        /// </summary>
        public T BIZ_ENT { get; set; }

    }
}
