using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QK.QAPP.Entity;

namespace QK.QAPP.Services
{
    public partial class APP_AEOSERVICE
    {
        public ViewListByPage<APP_AEO> GetCompanyList(string aeoType, string keyWord, int iPageIndex, int iPageSize)
        {
            ViewListByPage<APP_AEO> aeoList = new ViewListByPage<APP_AEO>();
            IQueryable<APP_AEO> query;
            if (string.IsNullOrEmpty(keyWord))
            {
                query = this.Find(p => p.AEO_TYPE == aeoType && p.ENABLE == "Y").OrderBy(o => o.AEO_NAME);
            }
            else
            {
                query =this.Find(p => p.AEO_TYPE == aeoType && p.ENABLE == "Y" && p.AEO_NAME.Contains(keyWord)).OrderBy(o => o.AEO_NAME);
            }
            aeoList.SetParameters(query, iPageIndex, iPageSize);
            return aeoList;
        }

        public ViewListByPage<APP_AEO> ListByCategory(string category, string keyWord, int iPageIndex, int iPageSize)
        {
            ViewListByPage<APP_AEO> aeoList = new ViewListByPage<APP_AEO>();
            IQueryable<APP_AEO> query;
            if (string.IsNullOrEmpty(keyWord))
            {
                query = this.Find(p => p.AEO_CATEGORY == category && p.ENABLE == "Y").OrderBy(o => o.AEO_NAME);
            }
            else
            {
                query = this.Find(p => p.AEO_CATEGORY == category && p.ENABLE == "Y" && p.AEO_NAME.Contains(keyWord)).OrderBy(o => o.AEO_NAME);
            }
            aeoList.SetParameters(query, iPageIndex, iPageSize);
            return aeoList;
        }
    }
}
