using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QK.QAPP.Global
{
    /// <summary>
    /// 全局字典配置
    /// </summary>
    public class GlobalDict
    {
        #region 申请表单字典
        /// <summary>
        /// 职位
        /// </summary>
        public static readonly string QAPP_POSITION = "POSITION";
        /// <summary>
        /// 员工关系
        /// </summary>
        public static readonly string QAPP_STAFF_RELATIONSHIP = "STAFF_RELATIONSHIP";
        /// <summary>
        /// 人员关系
        /// </summary>
        public static readonly string QAPP_COM_STAFF_RELATION = "COM_STAFF_RELATION";
        /// <summary>
        /// 企业类型
        /// </summary>
        public static readonly string QAPP_BUSINESS_TYPE = "BUSINESS_TYPE";
        /// <summary>
        /// 居住情况
        /// </summary>
        public static readonly string QAPP_LIVING_CONDITION = "LIVING_CONDITION";
        /// <summary>
        /// 借款用途
        /// </summary>
        public static readonly string QAPP_BORROW_USE = "BORROW_USE";
        /// <summary>
        /// 借款人身份
        /// </summary>
        public static readonly string QAPP_IDENTITY_OF_BORROWER = "IDENTITY_OF_BORROWER";
        /// <summary>
        /// 教育程度
        /// </summary>
        public static readonly string QAPP_EDUCATION_LEVEL = "EDUCATION_LEVEL";
        /// <summary>
        /// 婚姻状况
        /// </summary>
        public static readonly string QAPP_MARITAL_STATUS = "MARITAL_STATUS";
        /// <summary>
        /// 公司性质-授薪人群
        /// </summary>
        public static readonly string QAPP_COM_TYPE_SALARIED = "COM_TYPE_SALARIED";
        /// <summary>
        /// 告知书邮寄地址
        /// </summary>
        public static readonly string QAPP_EMAIL_ADDR_FOR_REPORT = "EMAIL_ADDR_FOR_REPORT";
        /// <summary>
        /// 产品类型
        /// </summary>
        public static readonly string QAPP_PRODUC_TTYPE = "PRODUC_TTYPE";
        /// <summary>
        /// 本市房产情况
        /// </summary>
        public static readonly string QAPP_LOCAL_ADDRHOUSE = "LOCAL_ADDRHOUSE";
        /// <summary>
        /// 本人名下房产
        /// </summary>
        public static readonly string QAPP_HOUSE_PROPERTY = "HOUSE_PROPERTY";

        
        #endregion

        #region 进件状态
        /// <summary>
        /// 进件状态
        /// </summary>
        public static readonly string QAPP_ENTER_STATUS = "ENTER_STATUS";
        #endregion

        #region 还款方式
        /// <summary>
        /// 还款方式
        /// </summary>
        public static readonly string QAPP_REPAY_TYPE = "REPAY_TYPE";
        #endregion

        #region 担保类型
        /// <summary>
        /// 担保类型
        /// </summary>
        public static readonly string QAPP_GUARANTEE_TYPE = "GUARANTEE_TYPE";
        #endregion

        #region 主担保方式
        /// <summary>
        /// 主担保方式
        /// </summary>
        public static readonly string QAPP_MAIN_GUARANTEE_WAY = "MAIN_GUARANTEE_WAY";
        #endregion

        #region 申请要件资料
        /// <summary>
        /// 申请要件资料
        /// </summary>
        public static readonly string QAPP_APPLY_INFO = "APPLY_INFO";
        #endregion

        #region QAPP全局缓存KEY值
        /// <summary>
        /// 缓存名称前缀
        /// </summary>
        public static readonly string CachePrefix = "QAPP_";

        #endregion
    }
}
