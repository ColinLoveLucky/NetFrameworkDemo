using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using Autofac;
using Microsoft.AspNet.SignalR;
using Autofac.Integration.SignalR;
using QK.QAPP.Global;
using QK.QAPP.Infrastructure.Cache;
using QK.QAPP.Infrastructure.Data.EFRepository.Repository;
using QK.QAPP.SalesCenter.Hubs;

namespace QK.QAPP.SalesCenter.App_Start
{
    public class AutofacSignalRConfig
    {
        public static void Bootstrapper()
        {
            ContainerBuilder builder = new ContainerBuilder();

            //IUintOfWork
            builder.RegisterType<UnitOfWork>()
                .As<IUnitOfWork>()
                .InstancePerLifetimeScope();
        
            //注入数据库连接
            builder.RegisterType<DbContextFactory>()
                .WithParameter("connectionString", GlobalSetting.MainDataBasenameOrConnectionString)
                .As<IDbContextFactory>()
                .InstancePerLifetimeScope();

            //注入数据库事务 
            //builder.RegisterType<EFRepositoryTransaction>()
            //    .As<IRepositoryTransaction>()
            //    .InstancePerRequest();
            //注入数据库查询语句执行类
            builder.RegisterType<RepositoryBaseSql>()
                .As<IRepositoryBaseSql>()
                .InstancePerLifetimeScope();


                builder.RegisterType(Type.GetType("QK.QAPP.Services.APP_MSGBOX_USERSERVICE,QK.QAPP.Services"))
                    .As<QK.QAPP.IServices.IAPP_MSGBOX_USERSERVICE>().InstancePerLifetimeScope()
                    .PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies);


            ////注入服务层
            //Assembly[] AsmSvr = new Assembly[2];
            //AsmSvr[0] = Assembly.Load("QK.QAPP.Services");
            //AsmSvr[1] = Assembly.Load("QK.QAPP.IServices");
            //builder.RegisterAssemblyTypes(AsmSvr)
            //    .Where(t => t.Name.ToUpper().EndsWith("APP_MSGBOX_USERSERVICE"))
            //    .AsImplementedInterfaces()
            //    .InstancePerLifetimeScope()
            //    .PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies);


            //将builder注入hup的Assembly中
            builder.RegisterHubs(typeof(SupplementHub).Assembly);

            //builder.RegisterHubs(AsmSvr);
            IContainer container = builder.Build();

            //依赖注入SignalR的Host
            GlobalHost.DependencyResolver = new Autofac.Integration.SignalR.AutofacDependencyResolver(container);
        }

    }
}