/***********************
 * 作    者：刘云松
 * 创建时间：‎2014‎-10‎-19 ‏‎18:25:13
 * 作    用：记录业务方法到Log的特性
*****************************/
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using QK.QAPP.Infrastructure.Log4Net;

namespace QK.QAPP.Infrastructure
{
    /// <summary>
    /// 记录业务方法到Log的特性
    /// <para>说明：此特性用于记录何人于何时进行了何种操作，并写道Log中</para>
    /// </summary>
    public class LogicalActionFilterAttribute : System.Web.Mvc.ActionFilterAttribute
    {
        /// <summary>
        /// Action作用描述，将用于描述操作人的动作
        /// </summary>
        public string ActionSummary { get; set; }

        public override void OnResultExecuted(System.Web.Mvc.ResultExecutedContext filterContext)
        {
            base.OnResultExecuted(filterContext);
            if (filterContext == null)
                return;
            AuthorizationAttribute author = new AuthorizationAttribute();
            QK.QAPP.Entity.QFUser currentUser = author.GetAuthorization(new AuthorizationContext(filterContext));
            if (currentUser == null)
                return;

            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.AppendFormat("\r\n用户{0}执行了Controller（{1}）的Action（{2}）\r\n"
                , currentUser.Account
                , filterContext.RouteData.Values["controller"]
                , filterContext.RouteData.Values["action"]);
            sb.AppendFormat("Action说明：{0}\r\n", string.IsNullOrWhiteSpace(ActionSummary) ? "[未指定]" : ActionSummary);
            sb.Append("参数：");

            System.Web.HttpRequestBase request = filterContext.HttpContext.Request;
            System.Web.Script.Serialization.JavaScriptSerializer jsSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            jsSerializer.MaxJsonLength = QK.QAPP.Global.GlobalSetting.MaxJsonLength;
            System.Collections.Generic.Dictionary<string, string> dicPara = new System.Collections.Generic.Dictionary<string, string>();
            foreach (string key in request.Form.AllKeys)
            {
                if (!string.IsNullOrWhiteSpace(key) && !string.IsNullOrWhiteSpace(request.Form[key]) && !dicPara.ContainsKey(key))
                {
                    dicPara.Add(key, request.Form[key]);
                }
            }
            foreach (string key2 in request.QueryString.AllKeys)
            {
                if (!string.IsNullOrWhiteSpace(key2) && !string.IsNullOrWhiteSpace(request.QueryString[key2]) && !dicPara.ContainsKey(key2))
                {
                    dicPara.Add(key2, request.QueryString[key2]);
                }
            }
            string strContent = jsSerializer.Serialize(dicPara);
            strContent += "\r\n";
            sb.Append(strContent);

            //Trace.Write(sb.ToString());
            //暂时注释 减少日志记载大小
            //LogWriter.Info(sb.ToString());
            //QK.QAPP.Infrastructure.Logger.Info(sb.ToString());
        }
    }
}
