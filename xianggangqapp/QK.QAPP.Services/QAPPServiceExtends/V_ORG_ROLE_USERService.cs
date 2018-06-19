using QK.QAPP.Entity;
using QK.QAPP.Infrastructure.Data.EFRepository.Repository;
using QK.QAPP.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QK.QAPP.Services
{
    public class V_ORG_ROLE_USERService :RepositoryBaseSql, IV_ORG_ROLE_USERService
    {
        public V_ORG_ROLE_USERService(IUnitOfWork unitOfWork)
            :base(unitOfWork)
        {

        }
    }
}
