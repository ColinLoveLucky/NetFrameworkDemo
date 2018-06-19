using QK.QAPP.Entity;
using QK.QAPP.Infrastructure;
using QK.QAPP.Infrastructure.Data.EFRepository.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QK.QAPP.Entity.ExtendEntity;

namespace QK.QAPP.IServices
{
    public interface IBID_ContractService
    {

        /// <summary>
        /// 生成合同
        /// </summary>
        /// <param name="conRequest"></param>
        /// <param name="bid"></param>
        /// <param name="contractGlobal"></param>
        /// <param name="user"></param>
        /// <param name="isCreate"></param>
        /// <returns></returns>
        Task<bool> ContractCreate(ContractCreateRequest conRequest, QB_V_BID_DETAIL bid, Global.ContractGlobalConfig contractGlobal, QFUser user, bool isCreate);
        /// <summary>
        /// 准备合同生成参数
        /// </summary>
        /// <param name="bid"></param>
        /// <param name="contractGlobal"></param>
        /// <param name="user"></param>
        /// <param name="conRequest"></param>
        /// <returns>返回是否重新生成合同</returns>
        bool CreateC(QB_V_BID_DETAIL bid, Global.ContractGlobalConfig contractGlobal, QFUser user, out ContractCreateRequest conRequest);
        /// <summary>
        /// 执行合同生成
        /// </summary>
        /// <param name="conRequest"></param>
        /// <param name="bid"></param>
        /// <param name="contractGlobal"></param>
        /// <param name="user"></param>
        /// <param name="isCreate"></param>
        /// <returns>返回合同是否生成成功</returns>
        bool RunCreateCon(ContractCreateRequest conRequest, QB_V_BID_DETAIL bid, Global.ContractGlobalConfig contractGlobal, QFUser user, bool isCreate);

        /// <summary>
        /// 获取创建失败合约列表
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        PageData<QB_V_BIDLIST> GetFailContractList(BidContractSearchPara para);

        /// <summary>
        /// 重新提交合约
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        string RePost(ArrangementActivityItfDTO para);
    }
}
