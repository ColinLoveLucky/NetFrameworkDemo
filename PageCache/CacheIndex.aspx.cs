using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PageCache
{
	public partial class CacheIndex : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			//Response.AddHeader("Cache-Control", "public");
			//Response.AddHeader("Pragma", "Pragma");
			//Response.Expires = 20;

			Response.Write("Hello World"+DateTime.Now);

		}
	}
}