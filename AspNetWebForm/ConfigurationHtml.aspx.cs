using AspNetWebForm.ConfigruationDemo;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

namespace AspNetWebForm
{

    public partial class ConfigurationHtml : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ///Configuration
            //CustomConfigurationThird config =
            //   CustomConfigurationThird.Setting;
            //var elemInfo = config.ElementInformation;
            //labConfig.Text = string.Format("Name:{0},url:{1}",
            //    config.UrlElements.Count, config.UrlElements.Count);

            //foreach (UrlConfigurationElement item in config.UrlElements)
            //{
            //    labConfig.Text += string.Format("Name:{0},url:{1}",
            //     item.Name, item.Url);
            //}

            //var con = ConfigurationManager.GetSection("system.web");


            var file = Server.MapPath("web.config");
            Configuration config =
                ConfigurationManager.OpenMappedMachineConfiguration(new ConfigurationFileMap(file));
            var group = config.GetSectionGroup("appGroupC") as ConfigurationSectionGroup;
            var customerConfig = group.Sections["firstCustomConfiguration"] as CustomerConfiguration;
            var name = customerConfig.Name;

            var app = ConfigurationManager.AppSettings["test"];
            var conn = ConfigurationManager.ConnectionStrings["value"];
            var web = config.GetSection("runtime") as DefaultSection;
        }

        public class ConfigSectionClass
        {
            //  public ConfigurationElement Element { get; set; }
        }
    }
}