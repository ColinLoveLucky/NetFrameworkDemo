/***********************
 * 作    者：刘云松
 * 创建时间：‎‎2015-3-10 15:41:22
 * 作    用：提供展期、循环贷等查询的条件
*****************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QK.QAPP.Entity
{
    public class ExtendApplySearchPara : EnterListSearchPara
    {
        List<string> extendActionGroup = new List<string>();
        /// <summary>
        /// 扩展申请单的方式：展期？循环贷？
        /// <para>取GlobalSetting.APPExtendConfig_</para>
        /// </summary>
        public List<string> ExtendActionGroup
        {
            get { return extendActionGroup; }
            set { extendActionGroup = value; }
        }

        Dictionary<string,string>  orderExceptSDStatus = new Dictionary<string, string>();
        public Dictionary<string, string> OrderExceptSDStatus
        {
            get { return orderExceptSDStatus; }
            set { orderExceptSDStatus = value; }
        }

        /// <summary>
        /// 据代码显示判断
        /// </summary>
        public Func<string, bool> IsDisplayRefuseFunc { get; set; }

        /// <summary>
        /// 展期条件SQL
        /// </summary>
        public Action<StringBuilder> ExtendCondition { get; set; }

        /// <summary>
        /// 可展期列表数据权限
        /// </summary>
        public Action<QFUserAuth, StringBuilder, List<object>> DataPermissionExtend { get; set; }
    }
}
