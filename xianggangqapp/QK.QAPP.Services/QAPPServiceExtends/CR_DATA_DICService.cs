using Microsoft.Practices.Unity;
using QK.QAPP.Entity;
using QK.QAPP.Infrastructure.Cache;
using QK.QAPP.Infrastructure.Data.EFRepository.Repository;
using QK.QAPP.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QK.QAPP.Services
{
    public class CR_DATA_DICService : RepositoryBaseSql, ICR_DATA_DICService
    {
        [Dependency]
        public ICacheProvider CacheService { get; set; }

        public CR_DATA_DICService(IUnitOfWork unitOfWork)
            :base(unitOfWork)
        {

        }

        public CR_DATA_DIC GetDICByID(long id)
        {

            return CacheService.GetFromCacheOrProxy<CR_DATA_DIC>("QAPP_GetDICByID_" + id, () =>
            {
                var sql = "SELECT * FROM CR_DATA_DIC WHERE ID=:id AND STATUS = 'OK'";
                var entity = this.SqlQuery<CR_DATA_DIC>(sql, id).FirstOrDefault();
                if (entity == null)
                {
                    entity = new CR_DATA_DIC();
                }
                return entity;
            });

        }

        public List<CR_DATA_DIC> GeDICListByParentID(long parentID)
        {
            return CacheService.GetFromCacheOrProxy<List<CR_DATA_DIC>>("QAPP_GeDICListByParentID_" + parentID, () =>
            {
                var sql = "SELECT * FROM CR_DATA_DIC WHERE PARENT_ID=:parentID AND STATUS = 'OK' ORDER BY ORDER_ID";
                var list = this.SqlQuery<CR_DATA_DIC>(sql, parentID).ToList();
                return list;
            });

        }

        public CR_DATA_DIC GetDICByCode(string code)
        {
            return CacheService.GetFromCacheOrProxy<CR_DATA_DIC>("QAPP_GetDICByCode_" + code, () =>
            {
                var sql = "SELECT * FROM CR_DATA_DIC WHERE DATA_CODE =:code AND STATUS = 'OK' ORDER BY ORDER_ID";
                var entity = this.SqlQuery<CR_DATA_DIC>(sql, code).FirstOrDefault();
                if (entity == null)
                {
                    entity = new CR_DATA_DIC();
                }
                return entity;
            });


        }

        public List<CR_DATA_DIC> GetDICByParentCode(string parentCode)
        {
            return CacheService.GetFromCacheOrProxy<List<CR_DATA_DIC>>("QAPP_GetDICByParentCode_" + parentCode, () =>
            {
                var sql = "SELECT * FROM CR_DATA_DIC WHERE PARENT_ID in (SELECT ID  FROM　CR_DATA_DIC WHERE DATA_CODE =:parentCode AND STATUS = 'OK') AND STATUS = 'OK' ORDER BY ORDER_ID";
                var list = this.SqlQuery<CR_DATA_DIC>(sql, parentCode).ToList();
                return list;
            });

        }

        public string GetDICNameByCode(string code)
        {
            return CacheService.GetFromCacheOrProxy<string>("QAPP_GetDICNameByCode_" + code, () =>
            {
                var sql = "SELECT DATA_NAME FROM CR_DATA_DIC WHERE DATA_CODE =:code AND STATUS = 'OK' ORDER BY ORDER_ID";
                var entity = this.SqlQuery<string>(sql, code).FirstOrDefault();
                if (entity == null)
                {
                    entity = "";
                }
                return entity;
            });
        }
    }
}
