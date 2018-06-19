/*********************
 * 作者：刘成帅
 * 时间：2014/10/16
 * 功能：字典表读取
**********************/

using QK.QAPP.Entity;
using QK.QAPP.Infrastructure.Data.EFRepository.Repository;
using System.Collections.Generic;

namespace QK.QAPP.IServices
{
    public interface ICR_DATA_DICService : IRepositoryBaseSql
    {
        /// <summary>
        /// 通过ID获取字典对象
        /// </summary>
        /// <param name="id">字典主键</param>
        /// <returns>字典对象</returns>
        CR_DATA_DIC GetDICByID(long id);

        /// <summary>
        /// 获取字典表子对象
        /// </summary>
        /// <param name="parentID">字典父键</param>
        /// <returns>字典对象集合</returns>
        List<CR_DATA_DIC> GeDICListByParentID(long parentID);

        /// <summary>
        /// 通过CODE获取实体
        /// </summary>
        /// <param name="code">字典代码</param>
        /// <returns>对象实体</returns>
        CR_DATA_DIC GetDICByCode(string code);

        /// <summary>
        /// 通过父类获取子类集合
        /// </summary>
        /// <param name="parentCode">父CODE</param>
        /// <returns>对象集合</returns>
        List<CR_DATA_DIC> GetDICByParentCode(string parentCode);

        /// <summary>
        /// 通过CODE获取名字
        /// </summary>
        /// <param name="code">字典代码</param>
        /// <returns>类型</returns>
        string GetDICNameByCode(string code);
    }
}
