using QK.QAPP.Entity;
using QK.QAPP.Infrastructure.Data.EFRepository.Repository;
using QK.QAPP.IServices;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;

namespace QK.QAPP.Services
{
    public class StaffPickService : IStaffPickService
    {
        [Dependency]
        public IV_ORG_ROLE_USERService service { get; set; }

        public List<StaffUnit> GetUnit(string parent, string roleCode, string companyId)
        {
            string getUrl = "/User/GetStaffUnit?parent=";
            List<StaffUnit> unit = new List<StaffUnit>();
            List<V_ORG_ROLE_USER> list = new List<V_ORG_ROLE_USER>();
            var sql = "SELECT * FROM V_ORG_ROLE_USER WHERE ENABLED=1 AND PARENTID=:parentID";
            if (!string.IsNullOrEmpty(roleCode))
            {
                sql += " AND (OBJECTTYPE !='item' OR (OBJECTTYPE ='item' AND ROLECODE like '%" + roleCode + "%'))";
            }
            if (string.IsNullOrEmpty(parent))
            {
                parent = "0";

            }
            if (string.IsNullOrEmpty(companyId))
            {
                list = service.SqlQuery<V_ORG_ROLE_USER>(sql, parent).ToList();
            }
            else
            {
                sql = "SELECT * FROM V_ORG_ROLE_USER WHERE OBJECTID=:companyId";
                list = service.SqlQuery<V_ORG_ROLE_USER>(sql, companyId).ToList();
            }

            list.ForEach(c =>
            {
                var unitEntity = new StaffUnit();

                unitEntity.id = c.OBJECTID;
                unitEntity.name = (c.OBJECTTYPE == "item" ? GetUserDisplayName(c.OBJECTNAME, c.USERCODE) : c.OBJECTNAME);//c.OBJECTNAME + (c.OBJECTTYPE == "item" ? "(" + c.USERCODE + ")" : "");
                unitEntity.type = c.OBJECTTYPE;
                unitEntity.value = c.OBJECTVALUE;
                unitEntity.parentID = c.PARENTID;
                if (c.OBJECTTYPE != "item")
                {
                    unitEntity.additionalParameters = getUrl + c.OBJECTID + "&roleid=" + roleCode;
                }
                unit.Add(unitEntity);
            });
            return unit;
        }

        public List<V_ORG_ROLE_USER> GetStaffByKeyWord(string keyWord, string roleCode, string companyId)
        {
            var companyName = "";
            if (!string.IsNullOrEmpty(companyId))
            {
                string sqlCompany = "SELECT FULLNAME FROM RMS_ORGANIZATION WHERE ORGANIZATIONID = :companyId";
                companyName = service.SqlQuery<string>(sqlCompany, companyId).ToList().FirstOrDefault();
            }
            string sql = "SELECT * FROM V_ORG_ROLE_USER WHERE '1'='1' AND ENABLED=1";
            if (!string.IsNullOrEmpty(companyName))
            {
                sql += " AND COMPANYNAME = '" + companyName + "'";
            }
            if (!string.IsNullOrEmpty(roleCode))
            {
                sql += " AND ROLECODE LIKE '%" + roleCode + "%'";
            }

            sql += " AND (COMPANYNAME like :keyWord OR DEPARTNAME like :keyWord OR OBJECTNAME like :keyWord OR ROLENAME like :keyWord OR OBJECTVALUE like :keyWord)";
            sql += " AND OBJECTTYPE='item'";
            var list = service.SqlQuery<V_ORG_ROLE_USER>(sql, "%" + keyWord + "%").ToList();
            foreach (var item in list)
            {
                item.OBJECTNAME = GetUserDisplayName(item.OBJECTNAME, item.USERCODE);
            }
            return list;
        }

        /// <summary>
        /// 获取用户显示名字
        /// </summary>
        /// <returns></returns>
        public string GetUserDisplayName(string name, string usercode)
        {
            string ret = "";
            string account = "";
            const int suxbit = 5;
            if (!string.IsNullOrEmpty(usercode) && !string.IsNullOrEmpty(usercode) && usercode.Length > 5)
            {
                account = usercode.Substring(usercode.Length - suxbit, suxbit);
                ret = string.Format("{0}({1})", name, account);
            }
            else
            {
                ret = name;
            }

            return ret;
        }
    }
}
