using System;
using System.Collections.Generic;
using QK.QAPP.Entity;
using QK.QAPP.Infrastructure.Data.EFRepository.Repository;
using QK.QAPP.IServices;
using System.Linq;
using QK.QAPP.Infrastructure.Cache;
using Microsoft.Practices.Unity;

namespace QK.QAPP.Services
{

    public partial class APP_BANKSERVICE : RepositoryBaseSql, IAPP_BANKSERVICE
    {
        [Dependency]
        public ICacheProvider CacheService { get; set; }

        public APP_BANKSERVICE(IUnitOfWork unitOfWork)
            :base(unitOfWork)
        {

        }

        public APP_BANK GetBankByID(long id)
        {
            return CacheService.GetFromCacheOrProxy<APP_BANK>("QAPP_GetBankByID_" + id, () =>
            {
                var sql = "SELECT * FROM APP_BANK WHERE ID=:id";
                var entity = this.SqlQuery<APP_BANK>(sql, id).FirstOrDefault();
                if (entity == null)
                {
                    entity = new APP_BANK();
                }
                return entity;
            });
        }

        public APP_BANK GetBankByType(string bankType)
        {
            return CacheService.GetFromCacheOrProxy<APP_BANK>("QAPP_GetBankByType_" + bankType, () =>
            {
                var sql = "SELECT * FROM APP_BANK WHERE BANK_TYPE=:bankType";
                var entity = this.SqlQuery<APP_BANK>(sql, bankType).FirstOrDefault();
                if (entity == null)
                {
                    entity = new APP_BANK();
                }
                return entity;
            });
        }

        public string GetBankNameByType(string bankType)
        {
            return CacheService.GetFromCacheOrProxy<string>("QAPP_GetBankNameByType_" + bankType, () =>
            {
                var sql = "SELECT BANK_NAME FROM APP_BANK WHERE BANK_TYPE=:bankType";
                var entity = this.SqlQuery<string>(sql, bankType).FirstOrDefault();
                if (entity == null)
                {
                    entity = string.Empty;
                }
                return entity;
            });
        }

        public List<APP_BANK> GetBankList()
        {
            return CacheService.GetFromCacheOrProxy<List<APP_BANK>>("QAPP_GetBankList", () =>
            {
                var sql = "SELECT * FROM APP_BANK";
                var list = this.SqlQuery<APP_BANK>(sql).ToList();
                return list;
            });
        }
    }
}
