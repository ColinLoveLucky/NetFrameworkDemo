using QK.QAPP.Entity;
using QK.QAPP.Global;
using QK.QAPP.Infrastructure;
using QK.QAPP.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QK.QAPP.Services
{
    public class QuotaUsageService : IQuotaUsageService
    {
        public IAPP_CITYSERVICE OrgService { get; set; }

        public string GetAmtUseSummary(AmtAssignListPara para)
        {
            RestApiHelper rest = new RestApiHelper(GlobalApi.GetAmtUseSummary);
            var securityKey = SecuritySignHelper.PostSecurityCollectionWithSign(Serializer.ObjToNameValueCollection(para));
            var result = rest.Post<List<QB_V_AMT_USE_SUMMARY>>(rest.GetUrlParam(securityKey), Serializer.ObjToDictionary(para));
            if (result == null) { return string.Empty; }

            return GetTreeTable(result);
        }

        #region 额度使用情况拼接table
        /// <summary>
        /// 组织架构表
        /// </summary>
        public string GetTreeTable(List<QB_V_AMT_USE_SUMMARY> list)
        {
            StringBuilder TableTreeList = new StringBuilder();
            int eRowIndex = 0;
            foreach (QB_V_AMT_USE_SUMMARY entity in list)
            {
                /*说明：salesshanghai(个人金融部) 和 District(区)  皆为ad里定义的编码，如果ad中有更改，此处也需要修改*/
                if (entity.CODE.Trim().ToLower().Equals("salesshanghai"))
                {
                    var district = list.Where(p => p.CODE.StartsWith("District", StringComparison.CurrentCultureIgnoreCase));//过滤出所有区的额度信息
                    string trID = "node-" + eRowIndex.ToString();
                    TableTreeList.Append("<tr id='" + trID + "'>");
                    TableTreeList.Append("<td style='padding-left:20px;'><span>总计</span></td>");
                    TableTreeList.Append("<td>" + district.Sum(s => s.AMT_ASSIGN) + "</td>");
                    TableTreeList.Append("<td>" + district.Sum(s => s.USEABLE_AMT) + "</td>");

                    #region P2P理财
                    TableTreeList.Append("<td>" + district.Sum(s => s.P2P_LC_TBJ) + "</td>");
                    TableTreeList.Append("<td>" + district.Sum(s => s.P2P_LC_TBJ_CONFIRM) + "</td>");
                    TableTreeList.Append("<td>" + district.Sum(s => s.P2P_LC_RRJC) + "</td>");
                    TableTreeList.Append("<td>" + district.Sum(s => s.P2P_LC_RRJC_CONFIRM) + "</td>");
                    //TableTreeList.Append("<td>" + district.Sum(s => s.P2P_LC_DYFD) + "</td>");
                    //TableTreeList.Append("<td>" + district.Sum(s => s.P2P_LC_DYFD_CONFIRM) + "</td>");
                    //TableTreeList.Append("<td>" + district.Sum(s => s.P2P_LC_JKD) + "</td>");
                    //TableTreeList.Append("<td>" + district.Sum(s => s.P2P_LC_JKD_CONFIRM) + "</td>");
                    TableTreeList.Append("<td>" + district.Sum(s => s.P2P_LC_OTHER) + "</td>");
                    TableTreeList.Append("<td>" + district.Sum(s => s.P2P_LC_OTHER_CONFIRM) + "</td>");
                    
                    #endregion

                    #region P2P直投
                    TableTreeList.Append("<td>" + district.Sum(s => s.P2P_ZT_TBJ) + "</td>");
                    TableTreeList.Append("<td>" + district.Sum(s => s.P2P_ZT_TBJ_CONFIRM) + "</td>");
                    TableTreeList.Append("<td>" + district.Sum(s => s.P2P_ZT_RRJC) + "</td>");
                    TableTreeList.Append("<td>" + district.Sum(s => s.P2P_ZT_RRJC_CONFIRM) + "</td>");
                    TableTreeList.Append("<td>" + district.Sum(s => s.P2P_ZT_58QG) + "</td>");
                    TableTreeList.Append("<td>" + district.Sum(s => s.P2P_ZT_58QG_CONFIRM) + "</td>");
                    TableTreeList.Append("<td>" + district.Sum(s => s.P2P_ZT_BYXF) + "</td>");
                    TableTreeList.Append("<td>" + district.Sum(s => s.P2P_ZT_BYXF_CONFIRM) + "</td>");
                    TableTreeList.Append("<td>" + district.Sum(s => s.P2P_DR_OTHER) + "</td>");
                    TableTreeList.Append("<td>" + district.Sum(s => s.P2P_DR_OTHER_CONFIRM) + "</td>");
                    #endregion

                    //#region P2P消费信贷
                    //TableTreeList.Append("<td>" + district.Sum(s => s.P2P_XFXD_XFXDFK) + "</td>");
                    //TableTreeList.Append("<td>" + district.Sum(s => s.P2P_XFXD_XFXDFK_CONFIRM) + "</td>");
                    //#endregion

                    #region T2P
                    TableTreeList.Append("<td>" + district.Sum(s => s.T2P_WM_DYCD) + "</td>");
                    TableTreeList.Append("<td>" + district.Sum(s => s.T2P_WM_DYCD_CONFIRM) + "</td>");
                    //TableTreeList.Append("<td>" + district.Sum(s => s.T2P_WM_FD) + "</td>");
                    //TableTreeList.Append("<td>" + district.Sum(s => s.T2P_WM_FD_CONFIRM) + "</td>");
                    TableTreeList.Append("<td>" + district.Sum(s => s.T2P_WM_OTHER) + "</td>");
                    TableTreeList.Append("<td>" + district.Sum(s => s.T2P_WM_OTHER_CONFIRM) + "</td>");
                    TableTreeList.Append("<td>" + district.Sum(s => s.T2P_ZH_DYCD) + "</td>");
                    TableTreeList.Append("<td>" + district.Sum(s => s.T2P_ZH_DYCD_CONFIRM) + "</td>");
                    //TableTreeList.Append("<td>" + district.Sum(s => s.T2P_ZH_FD) + "</td>");
                    //TableTreeList.Append("<td>" + district.Sum(s => s.T2P_ZH_FD_CONFIRM) + "</td>");
                    TableTreeList.Append("<td>" + district.Sum(s => s.T2P_ZH_OTHER) + "</td>");
                    TableTreeList.Append("<td>" + district.Sum(s => s.T2P_ZH_OTHER_CONFIRM) + "</td>");
                    #endregion

                    TableTreeList.Append("</tr>");
                    //创建子节点
                    TableTreeList.Append(GetTableTreeNode(entity.ORGANIZATIONID, list, trID));
                    eRowIndex++;
                }
            }
            return TableTreeList.ToString();
        }
        /// <summary>
        /// 创建子节点
        /// </summary>
        /// <param name="parentID">父节点主键</param>
        /// <param name="list">菜单集合</param>
        /// <returns></returns>
        public string GetTableTreeNode(string parentID, List<QB_V_AMT_USE_SUMMARY> list, string parentTRID)
        {
            StringBuilder sb_TreeNode = new StringBuilder();
            int i = 1;
            foreach (QB_V_AMT_USE_SUMMARY entity in list.OrderBy(o => o.FULLNAME))
            {
                if (entity.PARENTID == parentID)
                {
                    string trID = parentTRID + "-" + i.ToString();
                    sb_TreeNode.Append("<tr id='" + trID + "' class='child-of-" + parentTRID + "'>");
                    sb_TreeNode.Append("<td style='padding-left:20px;'><span>" + entity.FULLNAME + "</span></td>");
                    sb_TreeNode.Append("<td>" + entity.AMT_ASSIGN + "</td>");//分配金额
                    sb_TreeNode.Append("<td>" + entity.USEABLE_AMT + "</td>");//已挂标额度

                    #region P2P理财
                    sb_TreeNode.Append("<td>" + entity.P2P_LC_TBJ + "</td>");
                    sb_TreeNode.Append("<td>" + entity.P2P_LC_TBJ_CONFIRM + "</td>");
                    sb_TreeNode.Append("<td>" + entity.P2P_LC_RRJC + "</td>");
                    sb_TreeNode.Append("<td>" + entity.P2P_LC_RRJC_CONFIRM + "</td>");
                    //sb_TreeNode.Append("<td>" + entity.P2P_LC_DYFD + "</td>");
                    //sb_TreeNode.Append("<td>" + entity.P2P_LC_DYFD_CONFIRM + "</td>");
                    //sb_TreeNode.Append("<td>" + entity.P2P_LC_JKD + "</td>");
                    //sb_TreeNode.Append("<td>" + entity.P2P_LC_JKD_CONFIRM + "</td>");
                    sb_TreeNode.Append("<td>" + entity.P2P_LC_OTHER + "</td>");
                    sb_TreeNode.Append("<td>" + entity.P2P_LC_OTHER_CONFIRM + "</td>");
                    #endregion

                    #region P2P直投
                    sb_TreeNode.Append("<td>" + entity.P2P_ZT_TBJ + "</td>");
                    sb_TreeNode.Append("<td>" + entity.P2P_ZT_TBJ_CONFIRM + "</td>");
                    sb_TreeNode.Append("<td>" + entity.P2P_ZT_RRJC + "</td>");
                    sb_TreeNode.Append("<td>" + entity.P2P_ZT_RRJC_CONFIRM + "</td>");
                    sb_TreeNode.Append("<td>" + entity.P2P_ZT_58QG + "</td>");
                    sb_TreeNode.Append("<td>" + entity.P2P_ZT_58QG_CONFIRM + "</td>");
                    sb_TreeNode.Append("<td>" + entity.P2P_ZT_BYXF + "</td>");
                    sb_TreeNode.Append("<td>" + entity.P2P_ZT_BYXF_CONFIRM + "</td>");
                    sb_TreeNode.Append("<td>" + entity.P2P_DR_OTHER + "</td>");
                    sb_TreeNode.Append("<td>" + entity.P2P_DR_OTHER_CONFIRM + "</td>");
                    #endregion

                    //#region P2P消费信贷
                    //sb_TreeNode.Append("<td>" + entity.P2P_XFXD_XFXDFK + "</td>");
                    //sb_TreeNode.Append("<td>" + entity.P2P_XFXD_XFXDFK_CONFIRM + "</td>");
                    //#endregion

                    #region T2P
                    sb_TreeNode.Append("<td>" + entity.T2P_WM_DYCD + "</td>");
                    sb_TreeNode.Append("<td>" + entity.T2P_WM_DYCD_CONFIRM + "</td>");
                    //sb_TreeNode.Append("<td>" + entity.T2P_WM_FD + "</td>");
                    //sb_TreeNode.Append("<td>" + entity.T2P_WM_FD_CONFIRM + "</td>");
                    sb_TreeNode.Append("<td>" + entity.T2P_WM_OTHER + "</td>");
                    sb_TreeNode.Append("<td>" + entity.T2P_WM_OTHER_CONFIRM + "</td>");
                    sb_TreeNode.Append("<td>" + entity.T2P_ZH_DYCD + "</td>");
                    sb_TreeNode.Append("<td>" + entity.T2P_ZH_DYCD_CONFIRM + "</td>");
                    //sb_TreeNode.Append("<td>" + entity.T2P_ZH_FD + "</td>");
                    //sb_TreeNode.Append("<td>" + entity.T2P_ZH_FD_CONFIRM + "</td>");
                    sb_TreeNode.Append("<td>" + entity.T2P_ZH_OTHER + "</td>");
                    sb_TreeNode.Append("<td>" + entity.T2P_ZH_OTHER_CONFIRM + "</td>");
                    #endregion

                    sb_TreeNode.Append("</tr>");
                    //创建子节点
                    sb_TreeNode.Append(GetTableTreeNode(entity.ORGANIZATIONID, list, trID));
                    i++;
                }
            }
            return sb_TreeNode.ToString();
        }
        #endregion
    }
}
