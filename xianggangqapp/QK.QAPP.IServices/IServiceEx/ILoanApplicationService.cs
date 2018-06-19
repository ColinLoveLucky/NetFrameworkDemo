using QK.QAPP.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QK.QAPP.IServices
{
    public interface ILoanApplicationService
    {
        /// <summary>
        /// 融誉100贷款申请
        /// </summary>
        /// <param name="formDic"></param>
        /// <param name="resultMsg"></param>
        /// <returns></returns>
        APP_MAIN RyLoanApplication(Dictionary<string, string> formDic, out string resultMsg);
        /// <summary>
        /// 贷款信息编辑（极客贷）
        /// </summary>
        /// <param name="formDic"></param>
        /// <param name="resultMsg"></param>
        /// <returns></returns>
        APP_LOAN CreditAppLoanEdit(Dictionary<string, string> formDic, out string resultMsg);

        /// <summary>
        /// 极客贷申请
        /// </summary>
        /// <param name="formDic"></param>
        /// <param name="resultMsg"></param>
        APP_MAIN GeekLoanApplication(Dictionary<string, string> formDic, out string resultMsg);

        /// <summary>
        /// 房贷申请
        /// </summary>
        /// <param name="formDic">房贷申请表单</param>
        /// <param name="resultMsg">错误信息</param>
        /// <returns></returns>
        APP_MAIN HouseLoanApplication(Dictionary<string, string> formDic, out string resultMsg);

        /// <summary>
        /// 房贷申请编辑保存
        /// </summary>
        /// <param name="appIdOld">原申请单ID</param>
        /// <param name="formDic">表单信息</param>
        /// <param name="resultMsg">提示信息</param>
        /// <returns>新APP_LOAN对象</returns>
        APP_LOAN HouseAppLoanEdit(Dictionary<string, string> formDic, out string resultMsg);

        /// <summary>
        /// 描述：添加拒贷描述信息，此处根据权限和拒贷申请来判断是否显示拒贷信息，true(是)，false(否)
        /// 时间：2015-03-10
        /// 添加人：leiz
        /// </summary>
        /// <param name="status">申请状态</param>
        /// <returns>true/false</returns>
        bool IsDisplayRefuseLoan(string status);

        /// <summary>
        /// 车贷拒贷
        /// </summary>
        /// <param name="status">申请状态</param>
        /// <returns></returns>
        bool IsDisplayRefuseLoanCar(string status);

        /// <summary>
        /// 房贷拒贷
        /// </summary>
        /// <param name="status">申请状态</param>
        /// <returns></returns>
        bool IsDisplayRefuseLoanHouse(string status);

        /// <summary>
        /// 极客贷拒贷
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        bool IsDisplayRefuseLoanGeek(string status);

        /// <summary>
        /// 房贷计算申请金额，前期咨询费，借款服务费
        /// </summary>
        /// <param name="entity">APP_LOAN实体</param>
        /// <returns>申请金额</returns>
        void CalculateContractHouse(APP_LOAN entity);

        /// <summary>
        /// 计算合同金额，前期咨询费，借款服务费
        /// </summary>
        /// <param name="entity">APP_LOAN实体</param>
        /// <returns>合同金额</returns>
        void CalculateContractAmt(APP_LOAN entity);

        /// <summary>
        /// 融誉100拒贷信息
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        bool IsDisplayRefuseLoanRy(string status);
    }
}
