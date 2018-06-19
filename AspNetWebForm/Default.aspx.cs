using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security;
using System.Security.Permissions;
using System.Security.Principal;
using System.Threading;
using System.Web;
using System.Web.Hosting;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AspNetWebForm
{
    ///了解模板页的加载机制
    ///书写服务器端控件

    /// <summary>
    /// 模板引擎
    /// 大文件传输
    /// 断点续传
    /// 安全
    /// 加密
    /// rest 服务
    /// DDD 服务
    /// 设计模式
    /// web Form管道流 
    /// web api 
    /// web mvc
    /// rabbitMQ/active MQ
    /// IOC 框架
    /// </summary>
    public partial class _Default : Page
    {
        /// <summary>
        /// globalCulutre
        ///服务器IIS
        ///稳定性
        ///序列化工具pubffer
        /// 跑服务的工具
        /// AOP 框架
        /// IOC unity.spring.net
        /// rabbitMQ/active MQ
        /// 并发量
        /// 相应速度
        /// PV/UV
        /// elastic Search/Solor
        /// Http/Https协议
        /// TCP/UDP 协议
        /// FTP协议
        /// SMTP协议
        /// 日志监控kaflka
        /// cache/CDN/vanish
        /// 负载均衡ngix
        /// singelar
        /// 通信层面 WebSocket /Socket编程
        /// 服务器操作系统检测
        /// 自动化发布
        /// 代码管理svn/git
        /// http://www.cnblogs.com/Leo_wl/p/5049722.html 完成这个的一套流程
        /// DDD /设计模式 思考运用
        /// web开发的开源软件 从前段/服务注册/发现/日志监控/服务/缓存
        /// dubbo/SOA/SCA/微服务
        /// window Azure/Hadoop/Hive
        /// .net Core
        ///  linux
        /// sqlserver/oracle/mysql/mongdb/其它的
        /// C++/C/Java/javascript/jquery/angularjs/bootstrap/安卓 其它流行前段框架
        /// 区块链技术
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Page.User.Identity.IsAuthenticated)
            {
                labName.Text = Page.User.Identity.Name;
                HttpCookie cookie = Request.Cookies[FormsAuthentication.FormsCookieName];
                string encryptedTicket = cookie.Value;
                FormsAuthenticationTicket tickets = FormsAuthentication.Decrypt(encryptedTicket);
                labRoles.Text = tickets.UserData;
                FormsIdentity identity = new FormsIdentity(tickets);
                GenericPrincipal user = new GenericPrincipal(identity, tickets.UserData.Split(','));
                HttpContext.Current.User = user;
                if (HttpContext.Current.User.IsInRole("Admin"))
                {
                    labIsTrue.Text = HttpContext.Current.User.IsInRole("Admin").ToString();
                }
                var s = User.Identity.IsAuthenticated;
                ISAPIRuntime a = new ISAPIRuntime();



                // IHttpHandlerFactory
                //Page Control
                // ITypeDescriptorContext 
                // ExpandableObjectConverter
                //StateBag 
                // IStateManager
                //Style 
                //apllciationDomain
                //ITemplate 
                // DataBoundControl

                //inetinfo.exe
                //aspnet_isapi.dll, aspnet_filter.dll
                //w3wp.exe
                //ISAPIRuntim, HttpRuntime, HttpApplicationFactory, HttpApplication, HttpModule, HttpHandlerFactory, HttpHandler

                //ISAPIRuntime
                //ISAPIRuntime:主要作用是调用一些非托管代码生成HttpWorkerRequest对象，HttpWorkerRequest对象包含当前请求的所有信息，然后传递给HttpRuntime
                //HttpRuntime:根据HttpWorkerRequest对象生成HttpContext，HttpContext包含request、response等属性, 
                //再调用HttpApplicationFactory来生成IHttpHandler, 调用HttpApplication对象执行请求
                //HttpApplicationFactory: 生成一个HttpApplication对象
                //HttpApplication:进行HttpModule的初始化,HttpApplication创建针对此Http请求的 HttpContext对象
                //HttpModule: 当一个HTTP请求到达HttpModule时，整个ASP.NET Framework系统还并没有对这个HTTP请求做任何处理，
                //也就是说此时对于HTTP请求来讲，HttpModule是一个HTTP请求的“必经之路”，
                //所以可以在这个HTTP请求传递到真正的请求处理中心（HttpHandler）之前附加一些需要的信息在这个HTTP请求信息之上，
                //或者针对截获的这个HTTP请求信息作一些额外的工作，或者在某些情况下干脆终止满足一些条件的HTTP请求，
                //从而可以起到一个Filter过滤器的作用。
                //HttpHandlerFactory:把用户request 转发到HttpHandlerFactory,再由HttpHandlerFactory实例化HttpHandler对象来相应request
                //HttpHandle:Http处理程序，处理页面请求
                //在经典模式下，IIS会用ISAPI(aspnet_isapi.dll)扩展和ISAPI过滤器(aspnet_filter.dll)来调用ASP.NET运行库处理请求，
                //这是会用两个管道来处理请求，一个负责源代码，里一个负责托管代码。
                //在集成模式下，会有一个统一的请求处理管道，这时会发现，不管是托管代码还是本机代码，
                //都会在身份验证和执行处理被内核代码的托管代码拦截，它将ASP.NET请求管道与IIS核心管道组合在一起，
                //ASP.NET从IIS插件的角色进入IIS核心检测每个请求和操作。
                //FormsAuthenticationModule
                // Roles
                //RoleGroup
                //RoleManagerModule
                // RoleGroupCollection
                // RolePrincipal
                // RoleProvider
                // RoleProviderCollection

                //在w3wp.exe的内部，ASP.NET 是以 IIS ISAPI extension 的方式外加到
                //ASP_ISAPI.dll
                //IIS(其实包括 ASP 以及 PHP，也都以相同的方式配置),ASP.NET ISAPI进而加载CLR。
                //从而为ASP.NET Application创建一个托管的运行环境，
                //在CLR初始化的使用会加载两个重要的dll：AppManagerAppDomainFactory和ISAPIRuntime。通过AppManagerAppDomainFactory的Create方法
                //为Application创建一个Application Domain；通过ISAPIRuntime的ProcessRequest处理Request，
                //进而将流程拖入到ASP.NET Http Runtime Pipeline的范畴
            }
        }

        protected override void OnPreRenderComplete(EventArgs e)
        {
           var txtWriter= this.Response.Output;
            base.OnPreRenderComplete(e);
        }
    }
}