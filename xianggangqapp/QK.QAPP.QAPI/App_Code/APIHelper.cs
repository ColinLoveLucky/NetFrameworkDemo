using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace QK.QAPP.QAPI.App_Code
{
    /// <summary>
    /// QAPI银行验证接口帮助类
    /// </summary>
    public class APIHelper
    {
        /// <summary>
        /// 银行编码转换(从我们系统中的编码转换为验证接口用的编码)
        /// 创建人:张浩
        /// 创建日期：2016-02-29
        /// </summary>
        /// <param name="syscode">系统银行编码</param>
        /// <returns></returns>
        public static string ConvertBankCode(string syscode)
        {
            var apicode = string.Empty;
            var xmlurl = HttpContext.Current.Server.MapPath("~/XmlConfig/BankCodeMap.xml"); //取银行编码对应关系
            XDocument xDocument = XDocument.Load(xmlurl);
            var listbank=xDocument.Descendants("bank").ToList<XElement>();  //读取银行列表
            if (listbank != null && listbank.Any())
            {
                var model = listbank.FirstOrDefault(a => a.Attribute("syscode").Value == syscode);  //查找要转换的银行编码信息
                if (model != null)
                {
                    apicode = model.Attribute("apicode").Value;  //返回接口中对应的银行编码
                }
            }
            return apicode;
        }
    }
}