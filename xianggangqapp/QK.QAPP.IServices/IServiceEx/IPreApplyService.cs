using QK.QAPP.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QK.QAPP.IServices
{
    public interface IPreApplyService
    {
        /// <summary>
        /// 查询预申请列表
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        PreEnterListViewFiledList GetPreApplyList(PreEnterListSearchPara para);
        /// <summary>
        /// 获取预申请列表
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        PreEnterListViewFiledList GetPreApplyByList(PreEnterListSearchPara para);

        /// <summary>
        /// 从预申请信息中进件
        /// </summary>
        /// <param name="preAppId">预申请ID</param>
        /// <param name="formDic">表单信息</param>
        /// <param name="resultMsg">提示信息</param>
        /// <returns></returns>
        APP_MAIN ApplyLoan(long preAppId, Dictionary<string, string> formDic, out string resultMsg);
        /// <summary>
        /// 从车贷预申请进件
        /// </summary>
        /// <param name="preAppId">预申请id</param>
        /// <param name="formDic"></param>
        /// <param name="resultMsg"></param>
        /// <returns></returns>
        APP_MAIN ApplyLoanCar(long preAppId, Dictionary<string, string> formDic, out string resultMsg);

        string CheckPermission(PRE_APP_MAIN preAppMain);
    }
}
