using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QK.QAPP.Entity
{
    public class BidListQueryPara
    {
       /// <summary>
       /// 协议状态
       /// </summary>
        public string BidAgreementState { get; set; }
        /// <summary>
        /// 进件编号
        /// </summary>
        public string BidAppCode { get; set; }

       /// <summary>
       /// 业务品种
       /// </summary>
        public string BidBusType { get; set; }
        /// <summary>
        /// 标的编号
        /// </summary>
        public string BidCode { get; set; }
        /// <summary>
        /// 发标客户经理
        /// </summary>
        public string BidCustomerManager { get; set; }
        /// <summary>
        /// 客户经理姓名
        /// </summary>
        public string BidCustomerManagerName { get; set; }
        /// <summary>
        /// 客户名称
        /// </summary>
        public string BidCustomerName { get; set; }
       /// <summary>
       /// 划标类型
       /// </summary>
        public string BidDivideType { get; set; }
        /// <summary>
        /// 挂标类型
        /// </summary>
        public string BidHangType { get; set; }
       /// <summary>
       /// 发生类型
       /// </summary>
        public string BidOccurType { get; set; }
        /// <summary>
        /// 合同号
        /// </summary>
        public string BidContractNo { get; set; }
     /// <summary>
     /// 录件客服姓名
     /// </summary>
        public string BidServiceName { get; set; }
     /// <summary>
     /// 标的阶段
     /// </summary>
        public string BidStep { get; set; }
        /// <summary>
        /// 标的状态
        /// </summary>
        public string BidState { get; set; }
        /// <summary>
        /// 第三方推送状态
        /// </summary>
        public string BidThirdState { get; set; }
        /// <summary>
        /// 全局模糊搜索1，完全匹配0
        /// </summary>
    
        public int GlobalSearch { get; set; }
   
        public string SearchMsg { get; set; }


    /// <summary>
    /// 当前页
    /// </summary>
        public int CurrentPage { get; set; }

   /// <summary>
   /// 每页行数
   /// </summary>
        public int PageSize { get; set; }

        public string UserCode { get; set; }
        public string SubUserCodeList { get; set; } 
        /// <summary>
        /// 进件城市编号
        /// </summary>
        public string BidCityCode { get; set; }
        /// <summary>
        /// 签约日期
        /// </summary>
        public string BidSignedTime { get; set; }

        #region 数据权限相关
        /// <summary>
        /// 是否是返回所有数据
        /// </summary>
        public bool IsSelectAll { get; set; }
        /// <summary>
        /// 返回本人的数据
        /// </summary>
        public string AccountList { get; set; }
        /// <summary>
        /// 返回本部门/任意选择的部门数据
        /// </summary>
        public string ParentIdList { get; set; }
        /// <summary>
        /// 返回本公司的数据
        /// </summary>
        public string CompanyList { get; set; }
        /// <summary>
        /// 渠道
        /// </summary>
        public string BidChannel { get; set; }
        #endregion 

        /// <summary>
        /// 进件是否已创建合约
        /// </summary>
        public string BidIsContractCreated { get; set; }
    }
}
