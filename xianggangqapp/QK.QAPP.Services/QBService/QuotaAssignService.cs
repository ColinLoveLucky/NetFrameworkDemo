using QK.QAPP.Global;
using QK.QAPP.Infrastructure;
using QK.QAPP.IServices;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QK.QAPP.Entity;

namespace QK.QAPP.Services
{
    public class QuotaAssignService : IQuotaAssignService
    {
        public IQFUserService UserService { get; set; }
        public IAPP_CITYSERVICE OrgService { get; set; }

        /// <summary>
        /// 特别说明：【District】和AD中保持一致，根据District模糊检索出来区域信息
        /// 例如：1区：District1  2区：District2   3区：District3  ....
        /// 如果ad中规则修改，此处也需要跟着一起修改
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, string> GetDistrict()
        {
            var dic = OrgService.GetOrgRoleList().Where(p=>p.OBJECTVALUE.StartsWith("District",StringComparison.CurrentCultureIgnoreCase)).OrderBy(o=>o.OBJECTNAME).ToDictionary(k=>k.OBJECTID,v=>v.OBJECTNAME);
            return dic;
        }
        public Decimal GetGlobalAvailableAmt(string dateTime)
        {
            NameValueCollection collection = new NameValueCollection { { "dateTime", dateTime } };
            var para = SecuritySignHelper.GetSecurityCollectionWithSign(collection);
            var rest = new RestApiHelper(GlobalApi.GetGlobalAvailableAmt);
            var result = rest.Get<String>(string.Empty, para);
            if (result == null) { return 0; }
            return Convert.ToDecimal(result);
        }

        public PageData<QB_AMT_LIMIT_ASSIGN> GetQuotaAssignList(AmtAssignListPara assignPara)
        {
            var securityKey = SecuritySignHelper.PostSecurityCollectionWithSign(Serializer.ObjToNameValueCollection(assignPara));
            var rest = new RestApiHelper(GlobalApi.GetQuotaAssignList);
            var result = rest.Post<PageData<QB_AMT_LIMIT_ASSIGN>>(rest.GetUrlParam(securityKey), Serializer.ObjToDictionary(assignPara));
            if (result == null) { return new PageData<QB_AMT_LIMIT_ASSIGN>(); }
            return result;
        }

        public string AddQuotaAssign(QB_AMT_LIMIT_ASSIGN amtAssign)
        {
            amtAssign.UPDATE_USER_CODE = UserService.GetCurrentUser().Account;
            amtAssign.UPDATE_USER_NAME = UserService.GetCurrentUser().RealName;
            amtAssign.DEPT_CODE = UserService.GetCurrentUser().DepartmentId;
            amtAssign.DEPT_NAME = UserService.GetCurrentUser().DepartmentName;
            var securityKey = SecuritySignHelper.PostSecurityCollectionWithSign(Serializer.ObjToNameValueCollection(amtAssign));
            var rest = new RestApiHelper(GlobalApi.AddQuotaAssign);
            var result = rest.Post<string>(rest.GetUrlParam(securityKey), Serializer.ObjToDictionary(amtAssign));
            if (result == null) { return string.Empty; }
            return result;
        }

        public QB_AMT_LIMIT_ASSIGN GetAssignQuotaById(string id)
        {
            NameValueCollection collection = new NameValueCollection { { "id", id } };
            var para = SecuritySignHelper.GetSecurityCollectionWithSign(collection);
            var rest = new RestApiHelper(GlobalApi.GetQuotaAssignById);
            var result = rest.Get<QB_AMT_LIMIT_ASSIGN>(string.Empty, para);
            if (result == null) { new QB_AMT_LIMIT_ASSIGN(); }
            return result;
        }

        public string AdjustQuotaAssign(string id ,string adjustType,string assignAmt)
        {
            QB_AMT_LIMIT_ASSIGN amtAssign = new QB_AMT_LIMIT_ASSIGN();
            amtAssign.ID = string.IsNullOrEmpty(id) ? 0 : int.Parse(id);
            amtAssign.AMT_ASSIGN = string.IsNullOrEmpty(assignAmt) ? 0 : Convert.ToDecimal(assignAmt);
            amtAssign.UPDATE_USER_CODE = UserService.GetCurrentUser().Account;
            amtAssign.UPDATE_USER_NAME = UserService.GetCurrentUser().RealName;
            amtAssign.DEPT_CODE = UserService.GetCurrentUser().DepartmentId;
            amtAssign.DEPT_NAME = UserService.GetCurrentUser().DepartmentName;
            var urlParas = SecuritySignHelper.PostSecurityCollectionWithSign(Serializer.ObjToNameValueCollection(amtAssign), "&adjustType=" + adjustType);
            var rest = new RestApiHelper(GlobalApi.AdjustQuotaAssign);
            //string global = "?adjustType=" + adjustType;//将调整类型加到put URL上
            urlParas.Add("adjustType", adjustType);//将调整类型加到put URL参数上
            var result = rest.Put<String>(rest.GetUrlParam(urlParas), Serializer.ObjToDictionary(amtAssign));
            if (result == null) { return string.Empty; }
            return result;
        }
    }
}
