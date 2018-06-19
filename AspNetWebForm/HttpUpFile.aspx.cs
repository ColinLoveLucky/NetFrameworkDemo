using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AspNetWebForm
{
    public partial class HttpUpFile : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            //要保存的位置
            string strDesPath = "D:\\360";
            string strFileName = this.firstFile.PostedFile.FileName;
            strFileName = strDesPath + strFileName;
            //
            this.firstFile.PostedFile.SaveAs(strFileName);
            this.Label1.Text = "文件保存到了：" + strFileName;
        }
    }
}