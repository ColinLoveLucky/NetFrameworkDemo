/*********************
 * 作    者：刘成帅
 * 创建时间：2014/9/25
 * 功    能：主键字段，防止值丢失
**********************/
using Antlr3.ST;
using QK.QAPP.Global;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QK.QAPP.MvcScaffold.DForm
{
    public class PrimaryKey : AFieldType
    {

        public PrimaryKey(long id)
            : base(id)
        {

        }
        public override string StringTemplatePath
        {
            get { return GlobalSetting.DFormPath + @"\PrimaryKey.html"; }
        }

        public override string GetHTML(bool readOnly)
        {
            var template = FileReader.GetTmpString(this.StringTemplatePath);
            StringTemplate st = new StringTemplate(template);
            st.SetAttribute("FieldType", this);
            st.SetAttribute("ReadOnly", readOnly);
            return st.ToString();
        }

        public override bool Vaidate(string value, out string error)
        {
            error = "";
            return true;
        }
    }
}
