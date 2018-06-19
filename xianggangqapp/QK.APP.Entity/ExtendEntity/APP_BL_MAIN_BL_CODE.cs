/* ==============================================================================
 * 功能描述：APP_BL_MAIN_BL_CODE  
 * 创 建 者：leiz
 * 创建日期：2015/3/6 17:43:22
 * ==============================================================================*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QK.QAPP.Entity
{
    public partial class APP_BL_MAIN_BL_CODE
    {
        /// <summary>
        /// 主键
        /// </summary>		
        public decimal ID { get; set; }
        /// <summary>
        /// APP_BL_MAIN(ID)外键
        /// </summary>		
        public decimal BL_MAIN_ID { get; set; }
        /// <summary>
        /// 黑名单类别
        /// </summary>		
        public string BL_CODE { get; set; }
        /// <summary>
        /// 字段备注
        /// </summary>		
        public string BL_CODE_OTHER_MEMO { get; set; }

        /*以下APP_BL_MAIN*/
		/// <summary>
		/// 申请信息外键
        /// </summary>		
        public decimal APP_ID{get;set;}        
		/// <summary>
		/// 加黑类型:黑、灰
        /// </summary>		
        public string BL_ADD_TYPE{get;set;}        
		/// <summary>
		/// 原因说明
        /// </summary>		
        public string BL_REASON{get;set;}        
		/// <summary>
		/// 回退说明
        /// </summary>		
        public string BL_RETURN{get;set;}        
		/// <summary>
		/// 黑名单处理状态  Y确认(黑名单确认中的最终状态)/N回退/T提交(初终审中的提交)/P未处理(初终审中的暂存)/S(ave)暂存(黑名单确认中的暂存)
        /// </summary>		
        public string BL_STATUS{get;set;}        
		/// <summary>
		/// 修改时间
        /// </summary>		
        public DateTime CHANGED_TIME{get;set;}        
		/// <summary>
		/// 修改者
        /// </summary>		
        public string CHANGED_USER{get;set;}        
		/// <summary>
		/// 创建者
        /// </summary>		
        public string CREATED_USER{get;set;}        
		/// <summary>
		/// 创建时间
        /// </summary>		
        public DateTime CREATED_TIME { get; set; }        
		/// <summary>
		/// 公司名称
        /// </summary>		
        public string COMPANY_NAME{get;set;}        
		/// <summary>
		/// 补充说明
        /// </summary>		
        public string MEMO_CONFIRM{get;set;}        
    }
}
