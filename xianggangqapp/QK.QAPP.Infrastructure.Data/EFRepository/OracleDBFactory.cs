using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QK.QAPP.Infrastructure.Data.EFRepository.Repository
{
    public class OracleDBFactory : Disposable, IDBFactory
    {
        private DbContext context;

        public DbContext GetDbContent(bool LazyLoadingEnabled = true)
        {
            var config = System.Configuration.ConfigurationManager.AppSettings["MainDataBasenameOrConnectionString"].ToString();
            string nameOrConnectionString = config;
            if (!string.IsNullOrWhiteSpace(nameOrConnectionString))
            {
                context = new DbContext(nameOrConnectionString);
                context.Configuration.LazyLoadingEnabled = LazyLoadingEnabled;
            }
            return context;
        }

        protected override void DisposeCore()
        {
            if (context != null)
                context.Dispose();
        }
    }
}
