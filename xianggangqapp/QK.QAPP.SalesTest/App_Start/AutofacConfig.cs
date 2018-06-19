using Autofac;
using Autofac.Integration.Mvc;
using QK.QAPP.Global;
using QK.QAPP.Infrastructure;
using QK.QAPP.Infrastructure.Cache;
using QK.QAPP.Infrastructure.Data.EFRepository;
using QK.QAPP.Infrastructure.Data.EFRepository.Repository;
using QK.QAPP.Infrastructure.Log4Net;
using QK.QAPP.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace QK.QAPP.SalesTest.App_Start
{
    public class AutofacConfig
    {
        public static void Bootstrapper()
        {
            ContainerBuilder builder = new ContainerBuilder();

            //注入Controller
            builder.RegisterControllers(typeof(MvcApplication).Assembly).PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies);
            //IUintOfWork
            builder.RegisterType<UnitOfWork>()
                .As<IUnitOfWork>()
                .InstancePerRequest();
            //注入Redis缓存
            builder.RegisterType<RedisCacheProvider>()
                .WithParameter("Configuration", GlobalSetting.CacheSeverConfiguration)
                .As<ICacheProvider>()
                .SingleInstance();
            //注入数据库连接
            builder.RegisterType<DbContextFactory>()
                .WithParameter("connectionString", GlobalSetting.MainDataBasenameOrConnectionString)
                .As<IDbContextFactory>()
                .InstancePerRequest();
            //注入数据库事务 
            builder.RegisterType<EFRepositoryTransaction>()
                .As<IRepositoryTransaction>()
                .InstancePerRequest();

            ////注入数据库查询语句执行类
            //builder.RegisterType<IRepositoryBaseSql>()
            //    .As<RepositoryBaseSql>()
            //    .InstancePerRequest();

            //注入服务层
            Assembly[] AsmSvr = new Assembly[2];
            AsmSvr[0] = Assembly.Load("QK.QAPP.Services");
            AsmSvr[1] = Assembly.Load("QK.QAPP.IServices");
            builder.RegisterAssemblyTypes(AsmSvr)
                .Where(t => t.Name.ToLower().EndsWith("service"))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope()
                .PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies);

            //表单注入
            Assembly AsmDForm = Assembly.Load("QK.QAPP.MvcScaffold");
            builder.RegisterAssemblyTypes(AsmDForm)
                   .InstancePerLifetimeScope()
                   .PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies);

            builder.RegisterFilterProvider();
            IContainer container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

            //using (var scope = container.BeginLifetimeScope())
            //{
            //    var service = QK.QAPP.Infrastructure.Ioc.GetService<IRepositoryTransaction>();
            //}
        }

        /// <summary>
        /// 初始化系统配置 必须要在依赖注入以后
        /// </summary>
        public static void InitGlobalConfig()
        {
            var globalConfigService = Ioc.GetService<IAPP_GLOBALCONFIGSERVICE>();
            Global.GlobalSetting.GetConfigValueDelegate = GetValueByKey;
        }


        static string GetValueByKey(string key)
        {
            var caheService = Ioc.GetService<ICacheProvider>();
            var configValue = caheService.GetFromCacheOrProxy("QAPP_GLOBALCONFIG_" + key, () =>
            {
                IAPP_GLOBALCONFIGSERVICE globalconfigservice = Ioc.GetService<IAPP_GLOBALCONFIGSERVICE>();
                var obj = globalconfigservice.FirstOrDefault(c => c.KEY == key);
                if (obj != null)
                {
                    return obj.VALUE;
                }
                //System.Diagnostics.Trace.Write("未取到系统配置[" + key + "]的值！", "error");
                LogWriter.Warn("未取到系统配置[" + key + "]的值！");
                return "";
            });
            if (string.IsNullOrEmpty(configValue))
            {
                //System.Diagnostics.Trace.TraceWarning("取到系统配置[" + key + "]的值为空！");
                LogWriter.Warn("未取到系统配置[" + key + "]的值！");
            }
            return configValue;
        }

    }
}