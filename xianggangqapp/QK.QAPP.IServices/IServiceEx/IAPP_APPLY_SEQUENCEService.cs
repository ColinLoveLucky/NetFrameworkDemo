using QK.QAPP.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QK.QAPP.IServices
{
    public partial interface IAPP_APPLY_SEQUENCESERVICE
    {
        /// <summary>
        /// 生成申请单号
        /// </summary>
        /// <param name="seqCode">申请单类型代码(产品Logo)</param>
        /// <param name="proNum">申请单产品编号</param>
        /// <param name="cityCode">城市编号</param>
        /// <returns>申请单号</returns>
        string GetApplyNumber(string seqCode, string proNum, string cityCode);

        /// <summary>
        /// 根据cityCode查询申请号配置信息
        /// </summary>
        /// <param name="cityCode">城市区号</param>
        /// <returns></returns>
        List<APP_APPLY_SEQUENCE> GetApplySeqByCityCode(string cityCode);

        /// <summary>
        /// 只更新cityCode
        /// </summary>
        /// <param name="oldCityCode"></param>
        /// <param name="newCityCode"></param>
        bool UpdateCityCodeByCityCode(string oldCityCode, string newCityCode);

        /// <summary>
        /// 根据cityCode删除
        /// </summary>
        /// <param name="cityCode"></param>
        bool DeleteByCityCode(string cityCode);

        /// <summary>
        /// 添加实体
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        bool AddSeq(APP_APPLY_SEQUENCE entity);

        /// <summary>
        /// 生成申请号（展期）
        /// </summary>
        /// <param name="seqCode">申请单类型代码(产品Logo)</param>
        /// <param name="proNum">申请单产品编号</param>
        /// <param name="cityCode">城市编号</param>
        /// <param name="opeType">展期期数</param>
        /// <returns>申请单号</returns>
        string GetApplyNumberExtend(string seqCode, string proNum, string cityCode, string opeType);
    }
}
