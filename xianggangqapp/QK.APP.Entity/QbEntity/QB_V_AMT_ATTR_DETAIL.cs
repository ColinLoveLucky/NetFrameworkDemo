using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QK.QAPP.Entity
{
    public class QB_V_AMT_ATTR_DETAIL
    {
        /// <summary>
        /// ID
        /// </summary>		
        public long ID { get; set; }


        /// <summary>
        /// AMT_HAS_BOUNCE_RATE
        /// </summary>		
        public string AMT_HAS_BOUNCE_RATE { get; set; }


        /// <summary>
        /// AMT_CONFIRM_MODE
        /// </summary>		
        public string AMT_CONFIRM_MODE { get; set; }


        /// <summary>
        /// AMT_CONFIRM_MODE_NAME
        /// </summary>		
        public string AMT_CONFIRM_MODE_NAME { get; set; }


        /// <summary>
        /// AMT_MODIFIED_MODE
        /// </summary>		
        public string AMT_MODIFIED_MODE { get; set; }


        /// <summary>
        /// AMT_MODIFIED_MODE_NAME
        /// </summary>		
        public string AMT_MODIFIED_MODE_NAME { get; set; }


        /// <summary>
        /// AMT_DELETE_MODE
        /// </summary>		
        public string AMT_DELETE_MODE { get; set; }


        /// <summary>
        /// AMT_DELETE_MODE_NAME
        /// </summary>		
        public string AMT_DELETE_MODE_NAME { get; set; }


        /// <summary>
        /// AMT_ADJUST_MODE
        /// </summary>		
        public string AMT_ADJUST_MODE { get; set; }


        /// <summary>
        /// AMT_ADJUST_MODE_NAME
        /// </summary>		
        public string AMT_ADJUST_MODE_NAME { get; set; }


        /// <summary>
        /// AMT_RAISE_PLAN_MODE
        /// </summary>		
        public string AMT_RAISE_PLAN_MODE { get; set; }


        /// <summary>
        /// AMT_DEPT_CODE
        /// </summary>		
        public string AMT_DEPT_CODE { get; set; }


        /// <summary>
        /// AMT_DEPT_NAME
        /// </summary>		
        public string AMT_DEPT_NAME { get; set; }


        /// <summary>
        /// AMT_AMS_RAISE_INTERFACE
        /// </summary>		
        public string AMT_AMS_RAISE_INTERFACE { get; set; }


        /// <summary>
        /// AMT_T_VALUE_DATE
        /// </summary>		
        public string AMT_T_VALUE_DATE { get; set; }


        /// <summary>
        /// AMT_T_AGREE_VALUE_DATE
        /// </summary>		
        public string AMT_T_AGREE_VALUE_DATE { get; set; }


        /// <summary>
        /// AMT_LE_ZERO
        /// </summary>		
        public string AMT_LE_ZERO { get; set; }


        /// <summary>
        /// AMT_FINISH_DIV_BID
        /// </summary>		
        public string AMT_FINISH_DIV_BID { get; set; }
        /// <summary>
        /// AMT_CODE
        /// </summary>		
        public string AMT_CODE { get; set; }


        /// <summary>
        /// AMT_NAME
        /// </summary>		
        public string AMT_NAME { get; set; }

        /// <summary>
        /// AMT_PARENT_NAME
        /// </summary>		
        public string AMT_PARENT_NAME { get; set; }

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
