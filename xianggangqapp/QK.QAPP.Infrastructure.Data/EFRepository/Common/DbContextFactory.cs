using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QK.QAPP.Infrastructure.Data.EFRepository.Repository
{
    public class DbContextFactory : IDbContextFactory
    {
        private string _ConnectionString = string.Empty;

        public DbContextFactory(string connectionString)
        {
            this._ConnectionString = connectionString;
        }

        private DbContext _dbContext;
        private DbContext dbContext
        {
            get
            {
                if (this._dbContext == null)
                {
                    Type t = typeof(DbContext);
                    this._dbContext = (DbContext)Activator.CreateInstance(t, this._ConnectionString);
                }
                return _dbContext;
            }
        }

        public DbContext GetDbContext()
        {
            return this.dbContext;
        }

    }
}
