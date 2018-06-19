using QK.QAPP.Entity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QK.QAPP.IServices
{
    public interface IApplicationService
    {

        APP_MAIN GetAPPMain(long id);
        /// <summary>
        /// 获取联系人列表
        /// </summary>
        List<APP_CONTACT> GetContacts(long APP_MAIN_Id);

        /// <summary>
        /// 编辑联系人 删除 或者修改 或者添加
        /// </summary>
        void EditContacts(APP_CONTACT entity);

        /// <summary>
        /// 更新或者添加联系人
        /// </summary>
        void UpdateOrAddCustomer(APP_CUSTOMER entity);

        /// <summary>
        /// 保存用户基本信息
        /// </summary>
        string SaveCustomerBasic(APP_CUSTOMER customer, APP_JOB job);

        /// <summary>
        /// 获取用户信息
        /// </summary>
        APP_CUSTOMER GetUserBasic(long appid);

        /// <summary>
        /// 获取用户工作信息
        /// </summary>
        /// <param name="appid"></param>
        /// <returns></returns>
        APP_JOB GetUserJob(long appid);

        /// <summary>
        /// 获取购车信息
        /// </summary>
        /// <param name="appid"></param>
        /// <returns></returns>
        APP_CARINFO GetCarInfo(long appid);

        /// <summary>
        /// 保存用户银行信息
        /// </summary>s
        string SaveCustomerBankCard(APP_BANKCARD bankCard);

        /// <summary>
        /// 保存银行信息（不进行中金验证，只存储银行名称，不存编码）
        /// </summary>
        /// <param name="bankCard"></param>
        /// <returns></returns>
        string SaveBankCardNoCode(APP_BANKCARD bankCard);

        /// <summary>
        /// 保存用户银行信息
        /// </summary>
        APP_BANKCARD GetCustomerBankCard(long appid);

        /// <summary>
        /// 保存员工专用栏
        /// </summary>
        string SaveStaffOnly(APP_STAFF_ONLY so);

        /// <summary>
        /// 保存员工专用栏
        /// </summary>
        string SaveStaffOnly(APP_STAFF_ONLY so, APP_MAIN main);

        /// <summary>
        /// 保存购车信息
        /// </summary>
        /// <param name="carInfo"></param>
        /// <returns></returns>
        string SaveCarInfo(APP_CARINFO carInfo);
        /// <summary>
        /// 获取员工专用栏
        /// </summary>
        APP_STAFF_ONLY GetStaffOnly(long appid);

        /// <summary>
        /// 提交表单
        /// </summary>
        /// <param name="appid"></param>
        /// <returns></returns>
        string SubmitLoan(long appid);

        /// <summary>
        /// 检查权限
        /// </summary>
        /// <param name="appid"></param>
        /// <returns></returns>
        string CheckDataPermission(long appid,ENUM_FormOperation operation);

        /// <summary>
        /// 申请表单保存时验证表单是否可编辑
        /// author:张浩
        /// date:2016-03-30
        /// </summary>
        /// <param name="status">申请状态</param>
        /// <returns>true 可编辑，false 不可编辑</returns>
        bool CheckIsAllowEdit(string status);

        /// <summary>
        /// 申请表单保存时验证表单是否可编辑
        /// author:张浩
        /// date:2016-03-30
        /// </summary>
        /// <param name="appid">申请ID</param>
        /// <returns>true 可编辑，false 不可编辑</returns>
        bool CheckIsAllowEdit(long appid);

        /// <summary>
        /// 获取抵押房产资料
        /// </summary>
        /// <param name="appid"></param>
        /// <returns></returns>
        APP_HOUSE GetHouse(long appid);

        /// <summary>
        /// 保存抵押房产资料
        /// </summary>
        /// <param name="appHouse"></param>
        /// <returns></returns>
        string SaveHouse(APP_HOUSE appHouse);

        /// <summary>
        /// 获取房屋权利人资料
        /// </summary>
        /// <param name="appid"></param>
        /// <returns></returns>
        List<APP_CONTACT> GetObligees(long appid);

        /// <summary>
        /// 保存房屋权利人资料
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        string SaveObligees(APP_CONTACT entity);
        
    }
}
