using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QK.QAPP.Entity
{
    public class QB_AMT_LIMIT_ATTRIBUTE
    {
        /// <summary>
        /// ID
        /// </summary>		
        public long ID { get; set; }


        /// <summary>
        /// 额度字典编码
        /// </summary>		
        public string AMT_CODE { get; set; }


        /// <summary>
        /// 拒件比例
        /// </summary>	
        public string AMT_HAS_BOUNCE_RATE { get; set; }


        /// <summary>
        /// 确认方式
        /// </summary>	
        public string AMT_CONFIRM_MODE { get; set; }


        /// <summary>
        /// 可否修改
        /// </summary>	
        public string AMT_MODIFIED_MODE { get; set; }


        /// <summary>
        /// 可否删除
        /// </summary>	
        public string AMT_DELETE_MODE { get; set; }


        /// <summary>
        /// 调整条件
        /// </summary>	
        public string AMT_ADJUST_MODE { get; set; }


        /// <summary>
        /// 生成募集计划方式
        /// </summary>	
        public string AMT_RAISE_PLAN_MODE { get; set; }


        /// <summary>
        /// 部门编号
        /// </summary>	
        public string AMT_DEPT_CODE { get; set; }


        /// <summary>
        /// AMS生产募集计划的接口
        /// </summary>	
        public string AMT_AMS_RAISE_INTERFACE { get; set; }


        /// <summary>
        /// T起息日控制时间点
        /// </summary>	
        public string AMT_T_VALUE_DATE { get; set; }


        /// <summary>
        /// T起息日协议确认控制时间点
        /// </summary>	
        public string AMT_T_AGREE_VALUE_DATE { get; set; }


        /// <summary>
        /// 挂标是否可以小于0
        /// </summary>		
        public string AMT_LE_ZERO { get; set; }


        /// <summary>
        /// 挂标结束时间
        /// </summary>	
        public string AMT_FINISH_DIV_BID { get; set; }


        /// <summary>
        /// 部门名称
        /// </summary>		
        public string AMT_DEPT_NAME { get; set; }

        /// <summary>
        /// 协议上传截止时间
        /// </summary>		
        public string AMT_AGREEMENT_UP_STOPTIME { get; set; }

        /// <summary>
        /// 额度划分类型:3-理财/融资匹配-手动,4-理财/融资匹配-自动,5-信托匹配-手动,6-信托匹配-自动
        /// </summary>		
        public string QB_LOAN_TYPE { get; set; }

        /// <summary>
        /// 额度放款类型:local("1","本地放款"),outerAndLocalVirtual("2","第三方放款（本地虚拟放款）"),LocalVirtual("3","纯本地虚拟放款")
        /// </summary>		
        public string QB_LOAN_CHANNEL { get; set; }   

    }
}
