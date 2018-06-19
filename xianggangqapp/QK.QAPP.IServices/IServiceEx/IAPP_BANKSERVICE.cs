using System;
using System.Collections.Generic;
using QK.QAPP.Entity;
using QK.QAPP.Infrastructure.Data.EFRepository.Repository;

namespace QK.QAPP.IServices
{
    public partial interface IAPP_BANKSERVICE : IRepositoryBaseSql
    {
        /// <summary>
        /// 通过ID获取银行实体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        APP_BANK GetBankByID(long id);

        /// <summary>
        /// 通过Type获取银行实体
        /// </summary>
        /// <param name="bankType"></param>
        /// <returns></returns>
        APP_BANK GetBankByType(string bankType);

        /// <summary>
        /// 通过Type获取银行名称
        /// </summary>
        /// <param name="bankType"></param>
        /// <returns></returns>
        string GetBankNameByType(string bankType);

        /// <summary>
        /// 获取银行列表
        /// </summary>
        /// <returns></returns>
        List<APP_BANK> GetBankList();

    }
}
