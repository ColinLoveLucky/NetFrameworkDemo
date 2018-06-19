using QK.QAPP.Entity.QbEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QK.QAPP.Entity;
using QK.QAPP.Infrastructure.Data.EFRepository.Repository;

namespace QK.QAPP.IServices
{
    public partial interface IQuotaManageService : IRepositoryBaseSql
    {
        /// <summary>
        /// 获取额度类型集合
        /// </summary>
        /// <returns></returns>
        List<QB_V_AMT_ATTRIBUTE> GetQuotaType();
        /// <summary>
        /// 获取额度类型属性集合
        /// </summary>
        /// <returns></returns>
        List<QB_V_AMT_ATTR_DETAIL> GetQuotaAttributeList();
        /// <summary>
        ///根据id获取额度类型属性
        /// </summary>
        /// <returns></returns>
        QB_AMT_LIMIT_ATTRIBUTE GetQuotaAttributeById(string id);

        /// <summary>
        /// 根据parentCode获取额度类型
        /// </summary>
        /// <param name="dicCode"></param>
        /// <returns></returns>
        List<QB_V_AMT_ATTRIBUTE> GetQuotaTypeByParentCode(string parentCode);
        /// <summary>
        /// 获取额度列表
        /// </summary>
        /// <param name="amtListPara">额度参数实体</param>
        /// <returns></returns>
        PageData<QB_V_AMTLIMIT> GetQuotaManageList(AmtLimitListPara amtListPara);

        /// <summary>
        /// 获取额度历史列表
        /// </summary>
        /// <param name="amtListPara">额度参数实体</param>
        /// <returns></returns>
        PageData<QB_V_AMT_OP_HISTORY> GetQuotaHistoryList(AmtLimitListPara amtListPara);
        /// <summary>
        /// 新增额度
        /// </summary>
        /// <param name="amtLimit"></param>
        /// <returns></returns>
        String SaveQuota(QB_AMT_LIMIT amtLimit);

        /// <summary>
        /// 通过id查找额度信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        QB_V_AMTLIMIT GetQuotaInfoById(string id);

        /// <summary>
        /// 修改额度
        /// </summary>
        /// <param name="amtLimit"></param>
        /// <returns></returns>
        String ModifyQuota(QB_AMT_LIMIT amtLimit);

        /// <summary>
        /// 调整额度
        /// </summary>
        /// <param name="id">主键id</param>
        /// <param name="adjustType">调整类型</param>
        /// <param name="quotaAmt">调整金额</param>
        /// <returns></returns>
        String AdjustQuota(string id, string adjustType, string quotaAmt);

        /// <summary>
        /// 删除额度
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        String DeleteQuota(string id);
        /// <summary>
        /// 新增额度类型属性
        /// </summary>
        /// <param name="amtLimitAttribute"></param>
        /// <returns></returns>
        String SaveQuotaAttribute(QB_AMT_LIMIT_ATTRIBUTE amtLimitAttribute);
        /// <summary>
        /// 更新额度类型属性
        /// </summary>
        /// <param name="amtLimitAttribute"></param>
        /// <returns></returns>
        String UpdateQuotaAttribute(string id, QB_AMT_LIMIT_ATTRIBUTE amtLimitAttribute);
        /// <summary>
        /// 删除额度类型属性
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        String DeleteQuotaAttribute(string id);

        /// <summary>
        /// 额度复核
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        String QuotaReCheck(string id);

        /// <summary>
        /// 额度复核（批量）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        String QuotaReCheckBatch(string idList);

        /// <summary>
        /// 额度信息总览
        /// </summary>
        /// <param name="datetime">T日/T+1日</param>
        /// <returns></returns>
        List<QB_V_AMTLIMIT> GetQuotaInfo(string datetime);
        /// <summary>
        /// 获取第一个T+i(i≥0)工作日
        /// </summary>
        /// <param name="weekend">非工作日集合</param>
        /// <param name="i">T+i工作日</param>
        /// <param name="T1Date">返回T+i工作日</param>
        /// <returns></returns>
        String GetTiWorkDay(List<APP_MAIN_SYSDISUSED_WEEKEND> weekend, int i, out string T1Date);
        /// <summary>
        /// 描述：获取非工作日集合
        /// 添加时间：20160408
        /// 添加人：zhaolei
        /// </summary>
        /// <returns></returns>
        List<APP_MAIN_SYSDISUSED_WEEKEND> GetWeekend();
    }
}
