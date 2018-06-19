using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QK.QAPP.Infrastructure.Data.EFRepository.Repository
{
    public interface IDbContextFactory
    {
        DbContext GetDbContext();
    }
}
