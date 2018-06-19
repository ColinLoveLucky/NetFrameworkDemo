using QK.QAPP.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QK.QAPP.IServices
{
    public interface IQuotaBidDicConfigService
    {
        /// <summary>
        /// 根据字典类型获取字典
        /// </summary>
        /// <param name="dicType"></param>
        /// <returns></returns>
        Dictionary<string, string> GetQbDicByType(string dicType);
        /// <summary>
        /// 根据字典类型获取字典
        /// </summary>
        /// <param name="dicType"></param>
        /// <returns></returns>
        List<QB_DICTIONARY> GetQbDicType(string dicType);
        /// <summary>
        /// 获取公司部门编号名称
        /// </summary>
        /// <returns></returns>
        Dictionary<string, string> GetOrgList();
        /// <summary>
        /// 获取协议驳回原因
        /// </summary>
        /// <returns></returns>
        Dictionary<string, string> GetRejectReason();
        /// <summary>
        /// 根据字典类型获取字典
        /// </summary>
        /// <param name="type">字典类型</param>
        /// <returns></returns>
        Dictionary<string, string> GetDicByType(string type);
    }
}
