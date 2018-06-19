using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Common;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Infrastructure.Interception;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace LinqToEntity.Migrations
{
    public class MyConfiguration : DbConfiguration
    {
        public MyConfiguration()
        {
            SetTransactionHandler(SqlProviderServices.ProviderInvariantName, () => new CommitFailureHandler());
            //SetExecutionStrategy(SqlProviderServices.ProviderInvariantName, () => new SqlAzureExecutionStrategy());

            //SetDatabaseLogFormatter(
            //   (context, action) => new SingleLineFormatter(context, action));
            //   DbInterception.Add(new NLogCommandInterceptor());

            //DbConfiguration.Loaded += (_, a) =>
            //{
            //    a.ReplaceService<DbProviderServices>((s, k) => new MyProviderServices(s));
            //    a.ReplaceService<IDbConnectionFactory>((s, k) => new MyConnectionFactory(s));
            //};

            //  System.Data.Entity.Infrastructure.SqlCeConnectionFactory

            //Database.SetInitializer(
            //         new CreateDatabaseIfNotExists<BreakAwayContext>());
            //DropCreateDatabaseAlways<t>
        }
    }
}
