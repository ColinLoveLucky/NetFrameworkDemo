using QK.QAPP.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QK.QAPP.IServices
{
    public interface IStaffPickService
    {
        List<StaffUnit> GetUnit(string parent, string roleCode, string companyId);

        List<V_ORG_ROLE_USER> GetStaffByKeyWord(string keyWord, string roleCode, string companyId);

        string GetUserDisplayName(string name, string usercode);
    }
}
