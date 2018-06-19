using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QK.QAPP.Entity.ExtendEntity;
using QK.QAPP.Entity.QbEntity;

namespace QK.QAPP.IServices.IQBService
{
    public interface IBidSystemConfigService
    {
        List<BidSystemConfigInfo> GetBidSystemConfigList();

        BidSystemConfigInfo GetInfoById(string Id);

        void Update(BidSystemConfigInfo model);

        bool IsExistKey(string key);

        void Add(BidSystemConfigInfo model);

        bool Delete(string Id);
    }
}
