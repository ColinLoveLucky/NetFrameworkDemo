using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebFormDeep
{
    public partial class Home : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        public override void ProcessRequest(HttpContext context)
        {
            
        }
        protected override void OnPreRenderComplete(EventArgs e)
        {
            //this.Response.Output = new StreamWriter("D:\\FileStream.txt");
            //this.Response.Output.Flush();
            base.OnPreRenderComplete(e);
        }
    }
}