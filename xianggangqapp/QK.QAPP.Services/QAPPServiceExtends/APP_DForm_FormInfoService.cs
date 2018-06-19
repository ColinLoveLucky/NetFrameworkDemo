using QK.QAPP.Entity;
using QK.QAPP.Infrastructure.Data.EFRepository.Repository;
using QK.QAPP.IServices;

namespace QK.QAPP.Services.QAPPService
{
    public partial class APP_DFORM_FORMINFOService : RepositoryBase<APP_DFORM_FORMINFO>
    {
        public APP_DFORM_FORMINFOService()
            : base("name=APPEntities")
        {
           
        }
    }
}
