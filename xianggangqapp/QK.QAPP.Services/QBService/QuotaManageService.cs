using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QK.QAPP.Infrastructure;
using QK.QAPP.Global;
using QK.QAPP.Entity.QbEntity;
using System.Collections.Specialized;
using QK.QAPP.Entity;
using Microsoft.Practices.Unity;
using QK.QAPP.Infrastructure.Cache;
using QK.QAPP.IServices;
using QK.QAPP.Infrastructure.Data.EFRepository.Repository;

namespace QK.QAPP.Services.QBService
{
    public class QuotaManageService : RepositoryBaseSql,IQuotaManageService
    {
        public QuotaManageService(IUnitOfWork unitOfWork):base(unitOfWork){ }
        [Dependency]
        public ICacheProvider CacheService { get; set; }
        [Dependency]
        public IQFUserService UserService { get; set; }
        public List<QB_V_AMT_ATTRIBUTE> GetQuotaType()
        {
            return CacheService.GetFromCacheOrProxy<List<QB_V_AMT_ATTRIBUTE>>("QB_GetQuotaType", () =>
            {
                var paras = SecuritySignHelper.GetSecurityCollectionWithSign(null);
                var rest = new RestApiHelper(GlobalApi.QKAmtLimitAttr);
                var result = rest.Get<List<QB_V_AMT_ATTRIBUTE>>(string.Empty, paras);
                if (result != null)
                {
                    return result;
                }
                return new List<QB_V_AMT_ATTRIBUTE>();
            });

        }

        public List<QB_V_AMT_ATTRIBUTE> GetQuotaTypeByParentCode(string parentCode)
        {
            var result = CacheService.Get<List<QB_V_AMT_ATTRIBUTE>>("QB_GetQuotaType").Where(p => p.PARENT_CODE == parentCode);
            return result.ToList();
        }

        public PageData<QB_V_AMTLIMIT> GetQuotaManageList(AmtLimitListPara amtListPara)
        {
            var securityKey = SecuritySignHelper.PostSecurityCollectionWithSign(Serializer.ObjToNameValueCollection(amtListPara));
            var rest = new RestApiHelper(GlobalApi.GetAmtList);
            var result = rest.Post<PageData<QB_V_AMTLIMIT>>(rest.GetUrlParam(securityKey), Serializer.ObjToDictionary(amtListPara));
            if (result == null) { return new PageData<QB_V_AMTLIMIT>(); }
            return result;
        }

        public PageData<QB_V_AMT_OP_HISTORY> GetQuotaHistoryList(AmtLimitListPara amtListPara)
        {
            var securityKey = SecuritySignHelper.PostSecurityCollectionWithSign(Serializer.ObjToNameValueCollection(amtListPara));
            var rest = new RestApiHelper(GlobalApi.GetAmtOPHistory);
            var result = rest.Post<PageData<QB_V_AMT_OP_HISTORY>>(rest.GetUrlParam(securityKey), Serializer.ObjToDictionary(amtListPara));
            if (result == null) { return new PageData<QB_V_AMT_OP_HISTORY>(); }
            return result;
        }
        public String SaveQuota(QB_AMT_LIMIT amtLimit)
        {
            amtLimit.UPDATE_USER_CODE = UserService.GetCurrentUser().Account;
            amtLimit.UPDATE_USER_NAME = UserService.GetCurrentUser().RealName;
            amtLimit.DEPT_CODE = UserService.GetCurrentUser().DepartmentId;
            amtLimit.DEPT_NAME = UserService.GetCurrentUser().DepartmentName;
            var securityKey = SecuritySignHelper.PostSecurityCollectionWithSign(Serializer.ObjToNameValueCollection(amtLimit));
            var rest = new RestApiHelper(GlobalApi.QuotaAdd);
            var result = rest.Post<String>(rest.GetUrlParam(securityKey), Serializer.ObjToDictionary(amtLimit));
            return result;
        }

        public QB_V_AMTLIMIT GetQuotaInfoById(string id)
        {
            NameValueCollection collection = new NameValueCollection { { "id", id } };
            var paras = SecuritySignHelper.GetSecurityCollectionWithSign(collection);
            var rest = new RestApiHelper(GlobalApi.GetAmtListById);
            var result = rest.Get<QB_V_AMTLIMIT>(string.Empty, paras);
            if (result == null) { return new QB_V_AMTLIMIT(); }
            result.USE_START_DATE = result.USE_START_DATE.ToDate();
            result.USE_END_DATE = result.USE_END_DATE.ToDate();
            result.UPDATE_DATE = result.UPDATE_DATE.ToDTime();
            return result;
        }

        public string ModifyQuota(QB_AMT_LIMIT amtLimit)
        {
            amtLimit.UPDATE_USER_CODE = UserService.GetCurrentUser().Account;
            amtLimit.UPDATE_USER_NAME = UserService.GetCurrentUser().RealName;
            amtLimit.DEPT_CODE = UserService.GetCurrentUser().DepartmentId;
            amtLimit.DEPT_NAME = UserService.GetCurrentUser().DepartmentName;
            var securityKey = SecuritySignHelper.PostSecurityCollectionWithSign(Serializer.ObjToNameValueCollection(amtLimit));
            var rest = new RestApiHelper(GlobalApi.QuotaModify);
            var result = rest.Put<String>(rest.GetUrlParam(securityKey), Serializer.ObjToDictionary(amtLimit));
            return result;
        }

        public string AdjustQuota(string id, string adjustType, string quotaAmt)
        {
            QB_AMT_LIMIT amtLimit = new QB_AMT_LIMIT();
            amtLimit.ID = string.IsNullOrEmpty(id) ? 0 : int.Parse(id);
            amtLimit.AMT = string.IsNullOrEmpty(quotaAmt) ? 0 : decimal.Parse(quotaAmt);
            amtLimit.UPDATE_USER_CODE = UserService.GetCurrentUser().Account;
            amtLimit.UPDATE_USER_NAME = UserService.GetCurrentUser().RealName;
            amtLimit.DEPT_CODE = UserService.GetCurrentUser().DepartmentId;
            amtLimit.DEPT_NAME = UserService.GetCurrentUser().DepartmentName;
            var urlParas = SecuritySignHelper.PostSecurityCollectionWithSign(Serializer.ObjToNameValueCollection(amtLimit), "&adjustType=" + adjustType);
            var rest = new RestApiHelper(GlobalApi.QuotaAdjust);
            urlParas.Add("adjustType", adjustType);//将调整类型加到put URL参数上
            var result = rest.Put<String>(rest.GetUrlParam(urlParas), Serializer.ObjToDictionary(amtLimit));
            if (result == null) { return string.Empty; }
            return result;
        }

        public string DeleteQuota(string id)
        {
            //public IHttpActionResult Delete(long id, string userCode, string userName, string dept, string deptName) 
            NameValueCollection collection = new NameValueCollection 
            { 
              { "id", id },
              { "userCode",UserService.GetCurrentUser().Account},
              { "userName",UserService.GetCurrentUser().RealName},
              { "dept",UserService.GetCurrentUser().DepartmentId},
              { "deptName",UserService.GetCurrentUser().DepartmentName}
            };
            var paras = SecuritySignHelper.GetSecurityCollectionWithSign(collection);
            var rest = new RestApiHelper(GlobalApi.QuotaDelete);
            var result = rest.Delete<String>(string.Empty, paras);
            if (result == null) { return string.Empty; }
            return result;
        }

        public string QuotaReCheck(string id)
        {
            QB_AMT_LIMIT amtLimit = new QB_AMT_LIMIT();
            amtLimit.ID = Convert.ToInt64(id);
            amtLimit.UPDATE_USER_CODE = UserService.GetCurrentUser().Account;
            amtLimit.UPDATE_USER_NAME = UserService.GetCurrentUser().RealName;
            amtLimit.DEPT_CODE = UserService.GetCurrentUser().DepartmentId;
            amtLimit.DEPT_NAME = UserService.GetCurrentUser().DepartmentName;
            var securityKey = SecuritySignHelper.PostSecurityCollectionWithSign(Serializer.ObjToNameValueCollection(amtLimit));
            var rest = new RestApiHelper(GlobalApi.QuotaCheck);
            var result = rest.Put<String>(rest.GetUrlParam(securityKey), Serializer.ObjToDictionary(amtLimit));
            if (result == null) { return string.Empty; }
            return result;
        }

        public string QuotaReCheckBatch(string idList)
        {
            var result = string.Empty;
            List<QB_AMT_LIMIT> listQB_AMT_LIMIT = new List<QB_AMT_LIMIT>();
            QB_AMT_LIMIT amtLimit = null;
            if (!String.IsNullOrEmpty(idList) && idList.Split(',').Length > 0)
            {
                foreach (var id in idList.Split(','))
                {
                    amtLimit = new QB_AMT_LIMIT();
                    amtLimit.ID = Convert.ToInt64(id);
                    amtLimit.UPDATE_USER_CODE = UserService.GetCurrentUser().Account;
                    amtLimit.UPDATE_USER_NAME = UserService.GetCurrentUser().RealName;
                    amtLimit.DEPT_CODE = UserService.GetCurrentUser().DepartmentId;
                    amtLimit.DEPT_NAME = UserService.GetCurrentUser().DepartmentName;
                    listQB_AMT_LIMIT.Add(amtLimit);
                }
                var rest = new RestApiHelper(GlobalApi.MultiReCheck);
                result = rest.Post<String>(string.Empty, listQB_AMT_LIMIT);
                if (result == null) { result = string.Empty; }
            }
            return result;
        }

        /// <summary>
        /// 获取额度类型属性集合
        /// </summary>
        /// <returns></returns>
        public List<QB_V_AMT_ATTR_DETAIL> GetQuotaAttributeList()
        {
            var paras = SecuritySignHelper.GetSecurityCollectionWithSign(null);
            var rest = new RestApiHelper(GlobalApi.QKAmtLimitAttrConfig);
            var result = rest.Get<List<QB_V_AMT_ATTR_DETAIL>>(string.Empty, paras);
            if (result != null)
            {
                return result.OrderByDescending(t => t.ID).ToList();
            }
            return new List<QB_V_AMT_ATTR_DETAIL>();
        }
        public QB_AMT_LIMIT_ATTRIBUTE GetQuotaAttributeById(string id)
        {
            NameValueCollection collection = new NameValueCollection();
            collection.Add("id", id);
            var paras = SecuritySignHelper.GetSecurityCollectionWithSign(collection);
            var rest = new RestApiHelper(GlobalApi.QKAmtLimitAttrConfig);
            var result = rest.Get<QB_AMT_LIMIT_ATTRIBUTE>(string.Empty, paras);
            if (result != null)
            {
                return result;
            }
            return new QB_AMT_LIMIT_ATTRIBUTE();
        }
        public String SaveQuotaAttribute(QB_AMT_LIMIT_ATTRIBUTE amtLimitAttribute)
        {
            var securityKey = SecuritySignHelper.PostSecurityCollectionWithSign(Serializer.ObjToNameValueCollection(amtLimitAttribute));
            var rest = new RestApiHelper(GlobalApi.QKAmtLimitAttrAdd);
            var result = rest.Post<String>(rest.GetUrlParam(securityKey), Serializer.ObjToDictionary(amtLimitAttribute));
            return result;
        }
        public String UpdateQuotaAttribute(string id, QB_AMT_LIMIT_ATTRIBUTE amtLimitAttribute)
        {
            var urlParas = SecuritySignHelper.PostSecurityCollectionWithSign(Serializer.ObjToNameValueCollection(amtLimitAttribute), "&id=" + id);
            var rest = new RestApiHelper(GlobalApi.QKAmtLimitAttrUpdate);
            //string global = "?id=" + id;//将调整类型加到put URL上
            urlParas.Add("id", id);//将调整类型加到put URL参数上
            var result = rest.Put<String>(rest.GetUrlParam(urlParas), Serializer.ObjToDictionary(amtLimitAttribute));
            return result;
        }
        public String DeleteQuotaAttribute(string id)
        {
            NameValueCollection collection = new NameValueCollection { { "id", id } };
            var paras = SecuritySignHelper.GetSecurityCollectionWithSign(collection);
            var rest = new RestApiHelper(GlobalApi.QKAmtLimitAttrDelete);
            var result = rest.Delete<String>(string.Empty, paras);
            return result;
        }

        public List<QB_V_AMTLIMIT> GetQuotaInfo(string datetime)
        {
            NameValueCollection collection = new NameValueCollection { { "useDate", datetime } };
            var paras = SecuritySignHelper.GetSecurityCollectionWithSign(collection);
            var rest = new RestApiHelper(GlobalApi.GetQuotaInfoView);
            var result = rest.Get<List<QB_V_AMTLIMIT>>(string.Empty, paras);
            if (result == null) { return new List<QB_V_AMTLIMIT>(); }

            #region 已注释--此段逻辑放在前端实现，方便维护调整
            /*QB_AMT_INFO qInfo = new QB_AMT_INFO();
            foreach (var itemList in result.ToLookup(p => p.AMT_DEPT_NAME))
            {
                if (itemList.Key.Trim().Equals("个人金融部"))
                {
                    #region 01 P2P额度总览
                    qInfo.GJ_ZT_TOTAL_AMT = itemList.Where(p => p.AMT_TYPE.Equals("P2P_ZT")).Sum(s => s.AMT);
                    qInfo.GJ_ZT_TOTAL_AMT_USABLE = itemList.Where(p => p.AMT_TYPE.Equals("P2P_ZT")).Sum(s => s.AMT_USABLE);
                    qInfo.GJ_LC_TOTAL_AMT = itemList.Where(p => p.AMT_TYPE.Equals("P2P_LC")).Sum(s => s.AMT);
                    qInfo.GJ_LC_TOTAL_AMT_USABLE = itemList.Where(p => p.AMT_TYPE.Equals("P2P_LC")).Sum(s => s.AMT_USABLE);
                    #endregion

                    #region 02 T2P额度总览
                    qInfo.GJ_T2P_WM_DYCD_AMT = itemList.First(p => p.AMT_THIRD_TYPE.Equals("T2P_WM_DYCD")).AMT;
                    qInfo.GJ_T2P_WM_DYCD_AMT_USABLE = itemList.First(p => p.AMT_THIRD_TYPE.Equals("T2P_WM_DYCD")).AMT_USABLE;
                    qInfo.GJ_T2P_ZH_DYCD_AMT = itemList.First(p => p.AMT_THIRD_TYPE.Equals("T2P_ZH_DYCD")).AMT;
                    qInfo.GJ_T2P_ZH_DYCD_AMT_USABLE = itemList.First(p => p.AMT_THIRD_TYPE.Equals("T2P_ZH_DYCD")).AMT_USABLE;
                    qInfo.GJ_T2P_WM_OTHER_AMT = itemList.First(p => p.AMT_THIRD_TYPE.Equals("T2P_WM_OTHER")).AMT;
                    qInfo.GJ_T2P_WM_OTHER_AMT_USABLE = itemList.First(p => p.AMT_THIRD_TYPE.Equals("T2P_WM_OTHER")).AMT_USABLE;
                    qInfo.GJ_T2P_ZH_OTHER_AMT = itemList.First(p => p.AMT_THIRD_TYPE.Equals("T2P_ZH_OTHER")).AMT;
                    qInfo.GJ_T2P_ZH_OTHER_AMT_USABLE = itemList.First(p => p.AMT_THIRD_TYPE.Equals("T2P_ZH_OTHER")).AMT_USABLE;
                    #endregion
                }
                if (itemList.Key.Trim().Equals("房贷管理部"))
                {
                    #region 01 P2P额度总览
                    qInfo.FD_LC_TOTAL_AMT = itemList.Where(p => p.AMT_TYPE.Equals("P2P_LC")).Sum(s => s.AMT);
                    qInfo.FD_LC_TOTAL_AMT_USABLE = itemList.Where(p => p.AMT_TYPE.Equals("P2P_LC")).Sum(s => s.AMT_USABLE);
                    #endregion

                    #region 02 T2P额度总览
                    qInfo.FD_T2P_WM_FD_AMT = itemList.First(p => p.AMT_THIRD_TYPE.Equals("T2P_WM_FD")).AMT;
                    qInfo.FD_T2P_WM_FD_AMT_USABLE = itemList.First(p => p.AMT_THIRD_TYPE.Equals("T2P_WM_FD")).AMT_USABLE;
                    qInfo.FD_T2P_ZH_FD_AMT = itemList.First(p => p.AMT_THIRD_TYPE.Equals("T2P_ZH_FD")).AMT;
                    qInfo.FD_T2P_ZH_FD_AMT_USABLE = itemList.First(p => p.AMT_THIRD_TYPE.Equals("T2P_ZH_FD")).AMT_USABLE;
                    #endregion
                }
                if (itemList.Key.Trim().Equals("创新业务部"))
                {
                    #region 01 P2P额度总览
                    qInfo.CX_LC_TOTAL_AMT = itemList.Where(p => p.AMT_TYPE.Equals("P2P_LC")).Sum(s => s.AMT);
                    qInfo.CX_LC_TOTAL_AMT_USABLE = itemList.Where(p => p.AMT_TYPE.Equals("P2P_LC")).Sum(s => s.AMT_USABLE);
                    #endregion

                    #region 02 消费信贷额度总览
                    qInfo.CX_P2P_XFXD_XFXDFK_AMT = itemList.First(p => p.AMT_THIRD_TYPE.Equals("P2P_XFXD_XFXDFK")).AMT;
                    qInfo.CX_P2P_XFXD_XFXDFK_AMT_USABLE = itemList.First(p => p.AMT_THIRD_TYPE.Equals("P2P_XFXD_XFXDFK")).AMT_USABLE;
                    #endregion
                }
            }*/
            #endregion

            return result;
        }

        /// <summary>
        /// 获取第一个T+i(i≥0)工作日
        /// </summary>
        /// <param name="weekend">非工作日集合</param>
        /// <param name="i">T+i工作日</param>
        /// <param name="T1Date">返回T+i工作日</param>
        /// <returns></returns>
        public string GetTiWorkDay(List<APP_MAIN_SYSDISUSED_WEEKEND> weekend, int i, out string T1Date)
        {

            if (weekend.Any(a => a.WEEKEND_DATE == DateTime.Now.AddDays(i).Date))
            {
                GetTiWorkDay(weekend, i + 1, out T1Date);
            }
            else
            {
                T1Date = DateTime.Now.AddDays(i).ToShortDateString();
            }
            return T1Date;
        }

        /// <summary>
        /// 描述：获取非工作日集合
        /// 添加时间：20160408
        /// 添加人：zhaolei
        /// </summary>
        /// <returns></returns>
        public List<APP_MAIN_SYSDISUSED_WEEKEND> GetWeekend()
        {
            var sql = "SELECT * FROM  APP_MAIN_SYSDISUSED_WEEKEND";
            var weekendList = this.SqlQuery<APP_MAIN_SYSDISUSED_WEEKEND>(sql).ToList();
            return weekendList;
        }
    }
}
