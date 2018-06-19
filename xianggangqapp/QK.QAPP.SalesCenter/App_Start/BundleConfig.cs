using System.Collections.Generic;
using System.Web;
using System.Web.Optimization;

namespace QK.QAPP.SalesCenter
{
    public class AsIsBundleOrderer : IBundleOrderer
    {
        public virtual IEnumerable<BundleFile> OrderFiles(BundleContext context, IEnumerable<BundleFile> files)
        {
            return files;
        }
    }
    public class BundleConfig
    {

        public static void RegisterBundles(BundleCollection bundles)
        {
            //异步JS
            bundles.Add(new ScriptBundle("~/bundles/rest").Include(
                        "~/Content/assets/js/library/jquery.rest.js"
                        ));
            //全局帮助方法
            bundles.Add(new ScriptBundle("~/bundles/Utilities").Include(
                        "~/Content/assets/js/library/golbal.js"
                        ));
            //对话框弹出
            bundles.Add(new ScriptBundle("~/bundles/bootbox").Include(
                         "~/Content/assets/js/bootbox.min.js"
                        ));


            #region 母版页注册
            //基础CSS
            bundles.Add(new StyleBundle("~/Content/assets/css/basic").Include(
                      "~/Content/assets/css/bootstrap.min.css",
                      "~/Content/assets/css/font-awesome.min.css",
                      "~/Content/assets/css/Customer.css"));
            //ace CSS
            bundles.Add(new StyleBundle("~/Content/assets/css/ace").Include(
                     "~/Content/assets/css/jquery.gritter.css",
                     "~/Content/assets/css/ace.min.css",
                     "~/Content/assets/css/ace-rtl.min.css",
                     "~/Content/assets/css/ace-skins.min.css"
                     ));
            //基本的JS
            bundles.Add(new ScriptBundle("~/bundles/basic").Include(
                        "~/Content/assets/js/bootstrap.min.js",
                        "~/Content/assets/js/typeahead-bs2.min.js",
                        "~/Content/assets/js/ace-elements.min.js",
                        "~/Content/assets/js/jquery.gritter.min.js",
                        "~/Content/assets/js/ace.min.js"
                        ));
            #endregion


            #region 用户登录注册
            //登录CSS
            bundles.Add(new StyleBundle("~/Content/assets/css/login").Include(
                     "~/Content/login/css/main.css"));
            #endregion

            #region 用户表单以及验证
            bundles.Add(new ScriptBundle("~/bundles/form").Include(
                       "~/Content/assets/js/fuelux/fuelux.wizard.min.js",//表单分组
                //"~/Content/assets/js/fuelux/fuelux.spinner.min.js",//数值控件
                       "~/Content/assets/js/jquery.validate.min.js",//表单验证
                       "~/Content/assets/js/additional-methods.min.js",
                       //"~/Content/assets/js/jquery.maskedinput.min.js",//表单模板插件
                       //"~/Content/assets/js/inputmask.min.js",
                       "~/Content/assets/js/jquery.inputmask.js",
                       "~/Content/assets/js/date-time/bootstrap-datepicker.min.js",//时间选择器
                       "~/Content/assets/js/select2.min.js",//select美化
                       "~/Content/assets/js/jquery.validate.unobtrusive.min.js",//验证 属性中验证
                       "~/Content/assets/js/library/dfrom.action.js"//表单行为控制
                       ));
            bundles.Add(new StyleBundle("~/Content/assets/css/form").Include(
                    "~/Content/assets/css/bootstrap-timepicker.css",
                    "~/Content/assets/css/datepicker.css"
               ));
            #endregion

            #region 文件上传注册
            //文件上传CSS
            bundles.Add(new StyleBundle("~/Content/assets/css/fileupload").Include(
                     "~/Content/assets/css/font-awesome.min.css",
                     "~/Content/plupload/js/jquery.plupload.queue/css/jquery.plupload.queue.css")
                     );

            var bundle = new Bundle("~/bundles/fileupload");
            bundle.Orderer = new AsIsBundleOrderer();
            bundle.Include(
                       "~/Scripts/jquery-1.10.2.min.js",
                       "~/Content/plupload/js/jquery-ui.min.js",
                       "~/Content/plupload/js/browserplus-min.js",
                       "~/Content/plupload/js/plupload.full.min.js",
                       "~/Content/plupload/js/jquery.ui.plupload/jquery.ui.plupload.js",
                       "~/Content/plupload/js/i18n/zh_CN.js",
                       "~/Content/assets/js/jquery-ui-1.10.3.custom.min.js",
                       "~/Content/assets/js/jquery.imageView.js"
                       );
            bundles.Add(bundle);

            #endregion

            #region 消息推送
            bundles.Add(new StyleBundle("~/Content/assets/css/pushmessage").Include(
                    "~/Content/assets/css/jquery.gritter.css"
                ));
            bundles.Add(new ScriptBundle("~/bundles/pushmessage").Include(
                    "~/Scripts/jquery.signalR-2.1.2.min.js",
                    "~/Content/assets/js/jquery.gritter.min.js",
                    "~/Content/biz/PushMessage.js"
                ));
            #endregion

            BundleTable.EnableOptimizations = false;
            //bundles.UseCdn = true;            
        }
    }
}
