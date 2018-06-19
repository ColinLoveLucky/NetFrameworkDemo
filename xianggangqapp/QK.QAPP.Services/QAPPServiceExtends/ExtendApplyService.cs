/***********************
 * 作    者：刘云松
 * 创建时间：‎‎2015-3-9 15:41:22
 * 作    用：提供展期、循环贷等扩展申请单服务
*****************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QK.QAPP.Entity;
using QK.QAPP.Global;
using QK.QAPP.Infrastructure;
using QK.QAPP.Infrastructure.Cache;
using QK.QAPP.Infrastructure.Data.EFRepository.Repository;
using QK.QAPP.IServices;
using Microsoft.Practices.Unity;
using QK.QAPP.Infrastructure.Sql;

namespace QK.QAPP.Services
{
    public class ExtendApplyService : RepositoryBaseSql, IExtendApplyService
    {
        #region 属性

        /// <summary>
        /// 可展期的进件状态
        /// </summary>
        public Dictionary<string, string> NeedExtendStatus_Extend
        {
            get { return GlobalSetting.NeedExtendStatus_Extend; }
        }

        /// <summary>
        /// 除了补件之外的所有进件状态
        /// </summary>
        public Dictionary<string, string> Order_ExceptSD_Status_Car
        {
            get { return GlobalSetting.Order_ExceptSD_Status_Car; }
        }

        /// <summary>
        /// 扩展申请单的方式：展期？循环贷？
        /// <para>取GlobalSetting.APPExtendConfig_</para>
        /// </summary>
        public List<string> ActionGroup_Extend
        {
            get
            {
                return GlobalSetting.APPExtendConfig_Extend;
            }
        }

        [Dependency]
        public IQFUserService UserService { get; set; }

        [Dependency]
        public IAPP_EXTEND_RELATIONSERVICE ExtendRelationService { get; set; }

        [Dependency]
        public IV_APPMAINSERVICE VappmainService { get; set; }

        #endregion

        public ExtendApplyService(IUnitOfWork unitOfWork) : base(unitOfWork) { }

        /// <summary>
        /// 待扩展的申请单列表
        /// </summary>
        /// <param name="searchCondition"></param>
        /// <returns></returns>
        public ExtendApplyViewFieldList ExtendListToBe(ExtendApplySearchPara searchCondition)
        {
            //返回值
            ExtendApplyViewFieldList enterList = new ExtendApplyViewFieldList();
            //未授权访问任何资料
            if (searchCondition.AccessableCsac.Count == 0)
            {
                return enterList;
            }

            //准备用于检查权限的变量
            QFUserAuth currentAuth = UserService.GetUserAuth();
            List<object> lstSqlPara = new List<object>();

            string strSort = string.Empty;
            if (searchCondition.Sort != null && searchCondition.Sort.Count > 0)
            {
                foreach (KeyValuePair<string, string> kv in searchCondition.Sort)
                {
                    strSort += string.Format(" AE.{0} {1}", kv.Key, kv.Value);
                }
            }

            StringBuilder strSQL = new StringBuilder();
            strSQL.Append("SELECT * ");
            strSQL.Append("FROM (SELECT ROWNUM ID, TEMP.* ");
            strSQL.Append("FROM (SELECT AE.* ");
            strSQL.Append("FROM V_APPMAIN_EXTEND AE ");
            //最大权限则不需要检查权限
            if (!currentAuth.IsSelectAll)
            {
                strSQL.Append(" LEFT JOIN APP_AUTH AA ");
                strSQL.Append(" ON AE.AppId = AA.APP_ID ");
                strSQL.Append(" WHERE 1=1 ");
                // 查询APP_AUTH表的MENUCODE，在从不同菜单查询相同logo产品时使用
                // lys 2016-3-30
                if (!string.IsNullOrEmpty(searchCondition.InputMenuCode))
                {
                    strSQL.Append(" AND (AA.MENUCODE = :MENUCODE OR AA.MENUCODE IS NULL) ");
                    lstSqlPara.Add(searchCondition.InputMenuCode);
                }
            }
            else
            {
                // 查询APP_AUTH表的MENUCODE，在从不同菜单查询相同logo产品时使用
                // lys 2016-3-30
                if (!string.IsNullOrEmpty(searchCondition.InputMenuCode))
                {
                    strSQL.Append(" LEFT JOIN APP_AUTH AA ");
                    strSQL.Append(" ON AE.AppId = AA.APP_ID ");
                    strSQL.Append(" WHERE 1=1 ");
                    strSQL.Append(" AND (AA.MENUCODE = :MENUCODE OR AA.MENUCODE IS NULL) ");
                    lstSqlPara.Add(searchCondition.InputMenuCode);
                }
                else
                {
                    strSQL.Append(" WHERE 1=1 ");
                }
            }

            if (searchCondition.ExtendCondition != null)
                searchCondition.ExtendCondition(strSQL);

            //在系统时间不超过还款日前5个工作日的条件，只比较年月日部分
            //strSQL.Append("AND to_date(to_char(sysdate,'YYYYMMDD'),'yyyy/mm/dd')+(select count(0) from app.app_main_sysdisused_weekend amsw where amsw.weekend_date between sysdate and sysdate+" + GlobalSetting.BackLoanDay + ")+" + GlobalSetting.BackLoanDay + " < to_date(to_char(AE.Back_loan_time,'YYYYMMDD'),'yyyy/mm/dd') ");
            //strSQL.Append(" AND ceil(to_date(to_char(AE.Back_loan_time,'YYYYMMDD'),'yyyy/mm/dd')-sysdate)-(select count(0)-1 from app.app_main_sysdisused_weekend amsw where amsw.weekend_date between sysdate and to_date(to_char(AE.Back_loan_time,'YYYYMMDD'),'yyyy/mm/dd')) >= " + GlobalSetting.BackLoanDay);
            //展期状态为Y标识已经做过展期，不能再申请了
            //strSQL.Append(" AND AE.has_extend is null ");

            #region 界面查询条件

            if (searchCondition.FuzzySearch)
            {
                if (!string.IsNullOrWhiteSpace(searchCondition.AppCode) && !string.IsNullOrWhiteSpace(searchCondition.CustomerName))
                {
                    strSQL.Append(" AND (AE.APPCODE LIKE :APPCODE  OR AE.CUSTOMERNAME LIKE :CUSTOMERNAME )");
                    lstSqlPara.Add("%" + searchCondition.AppCode + "%");
                    lstSqlPara.Add("%" + searchCondition.CustomerName + "%");
                }
                else
                {
                    if (!string.IsNullOrWhiteSpace(searchCondition.AppCode))
                    {
                        strSQL.Append(" AND AE.APPCODE LIKE :APPCODE ");
                        lstSqlPara.Add("%" + searchCondition.AppCode + "%");
                    }
                    if (!string.IsNullOrWhiteSpace(searchCondition.CustomerName))
                    {
                        strSQL.Append(" AND AE.CUSTOMERNAME LIKE :CUSTOMERNAME ");
                        lstSqlPara.Add("%" + searchCondition.CustomerName + "%");
                    }
                }
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(searchCondition.AppCode))
                {
                    strSQL.Append(" AND AE.APPCODE LIKE :APPCODE ");
                    lstSqlPara.Add("%" + searchCondition.AppCode + "%");
                }
                if (!string.IsNullOrWhiteSpace(searchCondition.CustomerName))
                {
                    strSQL.Append(" AND AE.CUSTOMERNAME LIKE :CUSTOMERNAME ");
                    lstSqlPara.Add("%" + searchCondition.CustomerName + "%");
                }
                if (!string.IsNullOrWhiteSpace(searchCondition.CustomerIDCard))
                {
                    strSQL.Append(" AND AE.CUSTOMERIDCARD LIKE :CUSTOMERIDCARD ");
                    lstSqlPara.Add("%" + searchCondition.CustomerIDCard + "%");
                }
                if (searchCondition.ApplyStart != null)
                {
                    strSQL.Append(" AND AE.CREATEDTIME >= :APPLYSTART ");
                    lstSqlPara.Add(searchCondition.ApplyStart.Value);
                }
                if (searchCondition.ApplyEnd != null)
                {
                    strSQL.Append(" AND AE.CREATEDTIME <= :APPLYEND ");
                    lstSqlPara.Add(searchCondition.ApplyEnd.Value);
                }
                if (!string.IsNullOrWhiteSpace(searchCondition.SaleCode) && !string.IsNullOrWhiteSpace(searchCondition.SaleName))
                {
                    strSQL.Append(" AND (AE.SALESNO LIKE :SALESNO  OR AE.SALESNAME LIKE :SALESNAME )");
                    lstSqlPara.Add("%" + searchCondition.SaleCode + "%");
                    lstSqlPara.Add("%" + searchCondition.SaleName + "%");
                }
                else
                {
                    if (!string.IsNullOrWhiteSpace(searchCondition.SaleCode))
                    {
                        strSQL.Append(" AND AE.SALESNO LIKE :SALESNO ");
                        lstSqlPara.Add("%" + searchCondition.SaleCode + "%");
                    }
                    if (!string.IsNullOrWhiteSpace(searchCondition.SaleName))
                    {
                        strSQL.Append(" AND AE.SALESNAME LIKE :SALESNAME ");
                        lstSqlPara.Add("%" + searchCondition.SaleName + "%");
                    }
                }
                if (!string.IsNullOrWhiteSpace(searchCondition.CsacCode) && !string.IsNullOrWhiteSpace(searchCondition.CsacName))
                {
                    strSQL.Append(" AND (AE.CSADNO LIKE :CSADNO  OR AE.CSADNAME LIKE :CSADNAME )");
                    lstSqlPara.Add("%" + searchCondition.CsacCode + "%");
                    lstSqlPara.Add("%" + searchCondition.CsacName + "%");
                }
                else
                {
                    if (!string.IsNullOrWhiteSpace(searchCondition.CsacCode))
                    {
                        strSQL.Append(" AND AE.CSADNO LIKE :CSADNO ");
                        lstSqlPara.Add("%" + searchCondition.CsacCode + "%");
                    }
                    if (!string.IsNullOrWhiteSpace(searchCondition.CsacName))
                    {
                        strSQL.Append(" AND AE.CSADNAME LIKE :CSADNAME ");
                        lstSqlPara.Add("%" + searchCondition.CsacName + "%");
                    }
                }
            }
            #endregion

            string tempSql = string.Empty;
            //最大权限则不需要检查权限
            if (!currentAuth.IsSelectAll)
            {
                if (searchCondition.DataPermissionExtend != null)
                    searchCondition.DataPermissionExtend(currentAuth, strSQL, lstSqlPara);
            }

            //准备状态参数
            strSQL.Append(" AND AE.AppStatus IN (");
            tempSql = string.Empty;
            for (int j = 0; j < searchCondition.ListEnterStatus.Count; j++)
            {
                {
                    tempSql += string.Format(":AppStatus_{0},", j);
                    lstSqlPara.Add(searchCondition.ListEnterStatus[j].ToString());
                }
            }
            strSQL.Append(string.IsNullOrEmpty(tempSql) ? "''" : tempSql.TrimEnd(',')).Append(") ");

            //筛选logo
            strSQL.Append(" AND AE.LOGO IN (");
            tempSql = string.Empty;
            for (int l = 0; l < searchCondition.ListLogo.Count; l++)
            {
                if (!string.IsNullOrEmpty(searchCondition.ListLogo[l]))
                {
                    tempSql += string.Format(":Logo_{0},", l);
                    lstSqlPara.Add(searchCondition.ListLogo[l]);
                }
            }
            strSQL.Append(string.IsNullOrEmpty(tempSql) ? "''" : tempSql.TrimEnd(',')).Append(") ");

            //准备ExtendAction参数
            strSQL.Append(" AND AE.ExtendAction IN ( ");
            tempSql = string.Empty;
            for (int i = 0; i < searchCondition.ExtendActionGroup.Count; i++)
            {
                if (!string.IsNullOrEmpty(searchCondition.ExtendActionGroup[i]))
                {
                    tempSql += string.Format(":ExtendAction_{0},", i);
                    lstSqlPara.Add(searchCondition.ExtendActionGroup[i]);
                }
            }
            strSQL.Append(string.IsNullOrEmpty(tempSql) ? "''" : tempSql.TrimEnd(','));

            strSQL.AppendFormat(") ORDER BY {0}) TEMP) T ", string.IsNullOrEmpty(strSort) ? "AE.Sorting, AE.AppId DESC" : strSort);
            strSQL.Append(" WHERE T.ID BETWEEN :STARTID AND :ENDID ");
            lstSqlPara.Add((searchCondition.PageIndex - 1) * searchCondition.PageSize + 1);
            lstSqlPara.Add(searchCondition.PageIndex * searchCondition.PageSize);

            //UNION查询,用于计算符合条件的总数目,目的是减少查询次数
            strSQL.Append("UNION ");
            strSQL.Append("SELECT COUNT(0),null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null");
            strSQL.Append("  FROM V_APPMAIN_EXTEND AE ");
            //最大权限则不需要检查权限
            if (!currentAuth.IsSelectAll)
            {
                strSQL.Append(" LEFT JOIN APP_AUTH AA ");
                strSQL.Append(" ON AE.AppId = AA.APP_ID ");
                strSQL.Append(" WHERE 1=1 ");
                // 查询APP_AUTH表的MENUCODE，在从不同菜单查询相同logo产品时使用
                // lys 2016-3-30
                if (!string.IsNullOrEmpty(searchCondition.InputMenuCode))
                {
                    strSQL.Append(" AND (AA.MENUCODE = :MENUCODE OR AA.MENUCODE IS NULL) ");
                    lstSqlPara.Add(searchCondition.InputMenuCode);
                }
            }
            else
            {
                // 查询APP_AUTH表的MENUCODE，在从不同菜单查询相同logo产品时使用
                // lys 2016-3-30
                if (!string.IsNullOrEmpty(searchCondition.InputMenuCode))
                {
                    strSQL.Append(" LEFT JOIN APP_AUTH AA ");
                    strSQL.Append(" ON AE.AppId = AA.APP_ID ");
                    strSQL.Append(" WHERE 1=1 ");
                    strSQL.Append(" AND (AA.MENUCODE = :MENUCODE OR AA.MENUCODE IS NULL) ");
                    lstSqlPara.Add(searchCondition.InputMenuCode);
                }
                else
                {
                    strSQL.Append(" WHERE 1=1 ");
                }
            }

            if (searchCondition.ExtendCondition != null)
                searchCondition.ExtendCondition(strSQL);

            //在系统时间不超过还款日前5个工作日的条件，只比较年月日部分
            //strSQL.Append("AND to_date(to_char(sysdate,'YYYYMMDD'),'yyyy/mm/dd')+(select count(0) from app.app_main_sysdisused_weekend amsw where amsw.weekend_date between sysdate and sysdate+" + GlobalSetting.BackLoanDay + ")+" + GlobalSetting.BackLoanDay + " < to_date(to_char(AE.Back_loan_time,'YYYYMMDD'),'yyyy/mm/dd') ");
            //strSQL.Append(" AND ceil(to_date(to_char(AE.Back_loan_time,'YYYYMMDD'),'yyyy/mm/dd')-sysdate)-(select count(0)-1 from app.app_main_sysdisused_weekend amsw where amsw.weekend_date between sysdate and to_date(to_char(AE.Back_loan_time,'YYYYMMDD'),'yyyy/mm/dd')) >= " + GlobalSetting.BackLoanDay);
            //展期状态为Y标识已经做过展期，不能再申请了
            //strSQL.Append(" AND AE.has_extend is null ");

            #region 界面查询条件

            if (searchCondition.FuzzySearch)
            {
                if (!string.IsNullOrWhiteSpace(searchCondition.AppCode) && !string.IsNullOrWhiteSpace(searchCondition.CustomerName))
                {
                    strSQL.Append(" AND (AE.APPCODE LIKE :APPCODE  OR AE.CUSTOMERNAME LIKE :CUSTOMERNAME )");
                    lstSqlPara.Add("%" + searchCondition.AppCode + "%");
                    lstSqlPara.Add("%" + searchCondition.CustomerName + "%");
                }
                else
                {
                    if (!string.IsNullOrWhiteSpace(searchCondition.AppCode))
                    {
                        strSQL.Append(" AND AE.APPCODE LIKE :APPCODE ");
                        lstSqlPara.Add("%" + searchCondition.AppCode + "%");
                    }
                    if (!string.IsNullOrWhiteSpace(searchCondition.CustomerName))
                    {
                        strSQL.Append(" AND AE.CUSTOMERNAME LIKE :CUSTOMERNAME ");
                        lstSqlPara.Add("%" + searchCondition.CustomerName + "%");
                    }
                }
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(searchCondition.AppCode))
                {
                    strSQL.Append(" AND AE.APPCODE LIKE :APPCODE ");
                    lstSqlPara.Add("%" + searchCondition.AppCode + "%");
                }
                if (!string.IsNullOrWhiteSpace(searchCondition.CustomerName))
                {
                    strSQL.Append(" AND AE.CUSTOMERNAME LIKE :CUSTOMERNAME ");
                    lstSqlPara.Add("%" + searchCondition.CustomerName + "%");
                }
                if (!string.IsNullOrWhiteSpace(searchCondition.CustomerIDCard))
                {
                    strSQL.Append(" AND AE.CUSTOMERIDCARD LIKE :CUSTOMERIDCARD ");
                    lstSqlPara.Add("%" + searchCondition.CustomerIDCard + "%");
                }
                if (searchCondition.ApplyStart != null)
                {
                    strSQL.Append(" AND AE.CREATEDTIME >= :APPLYSTART ");
                    lstSqlPara.Add(searchCondition.ApplyStart.Value);
                }
                if (searchCondition.ApplyEnd != null)
                {
                    strSQL.Append(" AND AE.CREATEDTIME <= :APPLYEND ");
                    lstSqlPara.Add(searchCondition.ApplyEnd.Value);
                }
                if (!string.IsNullOrWhiteSpace(searchCondition.SaleCode) && !string.IsNullOrWhiteSpace(searchCondition.SaleName))
                {
                    strSQL.Append(" AND (AE.SALESNO LIKE :SALESNO  OR AE.SALESNAME LIKE :SALESNAME )");
                    lstSqlPara.Add("%" + searchCondition.SaleCode + "%");
                    lstSqlPara.Add("%" + searchCondition.SaleName + "%");
                }
                else
                {
                    if (!string.IsNullOrWhiteSpace(searchCondition.SaleCode))
                    {
                        strSQL.Append(" AND AE.SALESNO LIKE :SALESNO ");
                        lstSqlPara.Add("%" + searchCondition.SaleCode + "%");
                    }
                    if (!string.IsNullOrWhiteSpace(searchCondition.SaleName))
                    {
                        strSQL.Append(" AND AE.SALESNAME LIKE :SALESNAME ");
                        lstSqlPara.Add("%" + searchCondition.SaleName + "%");
                    }
                }
                if (!string.IsNullOrWhiteSpace(searchCondition.CsacCode) && !string.IsNullOrWhiteSpace(searchCondition.CsacName))
                {
                    strSQL.Append(" AND (AE.CSADNO LIKE :CSADNO OR AE.CSADNAME LIKE :CSADNAME )");
                    lstSqlPara.Add("%" + searchCondition.CsacCode + "%");
                    lstSqlPara.Add("%" + searchCondition.CsacName + "%");
                }
                else
                {
                    if (!string.IsNullOrWhiteSpace(searchCondition.CsacCode))
                    {
                        strSQL.Append(" AND AE.CSADNO LIKE :CSADNO ");
                        lstSqlPara.Add("%" + searchCondition.CsacCode + "%");
                    }
                    if (!string.IsNullOrWhiteSpace(searchCondition.CsacName))
                    {
                        strSQL.Append(" AND AE.CSADNAME LIKE :CSADNAME ");
                        lstSqlPara.Add("%" + searchCondition.CsacName + "%");
                    }
                }
            }
            #endregion

            tempSql = string.Empty;
            //最大权限则不需要检查权限
            if (!currentAuth.IsSelectAll)
            {
                if (searchCondition.DataPermissionExtend != null)
                    searchCondition.DataPermissionExtend(currentAuth, strSQL, lstSqlPara);
            }

            //准备状态参数
            strSQL.Append(" AND AE.AppStatus IN (");
            tempSql = string.Empty;
            for (int j = 0; j < searchCondition.ListEnterStatus.Count; j++)
            {
                {
                    tempSql += string.Format(":AppStatus_{0},", j);
                    lstSqlPara.Add(searchCondition.ListEnterStatus[j].ToString());
                }
            }
            strSQL.Append(string.IsNullOrEmpty(tempSql) ? "''" : tempSql.TrimEnd(',')).Append(") ");

            //筛选logo
            strSQL.Append(" AND AE.LOGO IN (");
            tempSql = string.Empty;
            for (int l = 0; l < searchCondition.ListLogo.Count; l++)
            {
                if (!string.IsNullOrEmpty(searchCondition.ListLogo[l]))
                {
                    tempSql += string.Format(":Logo_{0},", l);
                    lstSqlPara.Add(searchCondition.ListLogo[l]);
                }
            }
            strSQL.Append(string.IsNullOrEmpty(tempSql) ? "''" : tempSql.TrimEnd(',')).Append(") ");

            //准备ExtendAction参数
            strSQL.Append(" AND AE.ExtendAction IN ( ");
            tempSql = string.Empty;
            for (int i = 0; i < searchCondition.ExtendActionGroup.Count; i++)
            {
                if (!string.IsNullOrEmpty(searchCondition.ExtendActionGroup[i]))
                {
                    tempSql += string.Format(":ExtendAction_{0},", i);
                    lstSqlPara.Add(searchCondition.ExtendActionGroup[i]);
                }
            }
            strSQL.Append(string.IsNullOrEmpty(tempSql) ? "''" : tempSql.TrimEnd(','));
            strSQL.Append(") ");
            IEnumerable<V_APPMAIN_EXTEND> lstAppMainExtend = this.SqlQuery<V_APPMAIN_EXTEND>(strSQL.ToString(), lstSqlPara.ToArray());
            enterList.SetParameters(lstAppMainExtend, searchCondition);
            return enterList;
        }

        /// <summary>
        /// 车贷可展条件SQL（其中 V_APPMAIN_EXTEND 为 AE）
        /// </summary>
        /// <param name="strSQL"></param>
        public void AddExtendConditionCar(StringBuilder strSQL)
        {
            if (strSQL == null)
                return;
            //在系统时间不超过还款日前5个工作日的条件，只比较年月日部分
            strSQL.Append(" AND ceil(to_date(to_char(AE.Back_loan_time,'YYYYMMDD'),'yyyy/mm/dd')-sysdate)-(select count(0)-1 from app.app_main_sysdisused_weekend amsw where amsw.weekend_date between sysdate and to_date(to_char(AE.Back_loan_time,'YYYYMMDD'),'yyyy/mm/dd')) >= " + GlobalSetting.BackLoanDay);
            //展期状态为Y标识已经做过展期，不能再申请了
            strSQL.Append(" AND AE.has_extend is null ");
        }

        /// <summary>
        /// 房贷可展条件SQL（其中 V_APPMAIN_EXTEND 为 AE）
        /// </summary>
        /// <param name="strSQL"></param>
        public void AddExtendConditionHouse(StringBuilder strSQL)
        {
            if (strSQL == null)
                return;
            //在系统时间位于第2个还款日之后的第一个工作日至到期日前一天之间，
            strSQL.Append(" AND floor(sysdate-add_months(to_date(to_char(AE.Back_loan_time,'YYYYMMDD'),'yyyy/mm/dd'), -1))-(select case when (count(0)-1<0) then 0 else count(0)-1 end from app.app_main_sysdisused_weekend amsw where amsw.weekend_date between add_months(to_date(to_char(AE.Back_loan_time,'YYYYMMDD'),'yyyy/mm/dd'),-1) and sysdate)>0 ");
            strSQL.Append(" AND sysdate + ").Append(GlobalSetting.BackLoanDayHouse).Append(" <to_date(to_char(AE.Back_loan_time,'YYYYMMDD'),'yyyy/mm/dd') ");

            //当前合同状态为正常
            strSQL.Append(" AND AE.overdue_status = '").Append(OverdueStatus.NotOverdue).Append("' ");

            //展期状态为Y标识已经做过展期，不能再申请了
            strSQL.Append(" AND AE.has_extend is null ");
        }

        /// <summary>
        /// 可展列表数据权限（用户权限）
        /// </summary>
        /// <param name="currentAuth"></param>
        /// <param name="strSQL"></param>
        /// <param name="lstSqlPara"></param>
        public void AddExtendPermission(QFUserAuth currentAuth, StringBuilder strSQL, List<object> lstSqlPara)
        {
            //最大权限则不需要检查权限
            if (!currentAuth.IsSelectAll)
            {
                //准备权限COMPANY参数
                strSQL.Append(" AND (AA.COMPANY IN (");
                string tempSql = string.Empty;
                for (int c = 0; c < currentAuth.CompanyList.Count; c++)
                {
                    if (!string.IsNullOrEmpty(currentAuth.CompanyList[c]))
                    {
                        tempSql += string.Format(":COMPANY_{0},", c);
                        lstSqlPara.Add(currentAuth.CompanyList[c]);
                    }
                }
                strSQL.Append(string.IsNullOrEmpty(tempSql) ? "''" : tempSql.TrimEnd(','));

                //准备权限ACCOUNT参数
                strSQL.Append(") OR AA.ACCOUNT IN (");
                tempSql = string.Empty;
                for (int c = 0; c < currentAuth.AccountList.Count; c++)
                {
                    if (!string.IsNullOrEmpty(currentAuth.AccountList[c]))
                    {
                        tempSql += string.Format(":ACCOUNT_{0},", c);
                        lstSqlPara.Add(currentAuth.AccountList[c]);
                    }
                }
                strSQL.Append(string.IsNullOrEmpty(tempSql) ? "''" : tempSql.TrimEnd(','));

                //准备权限PARENT_ORGANIZATION参数
                strSQL.Append(") OR AA.PARENT_ORGANIZATION IN ( ");
                tempSql = string.Empty;
                for (int c = 0; c < currentAuth.ParentIdList.Count; c++)
                {
                    if (!string.IsNullOrEmpty(currentAuth.ParentIdList[c]))
                    {
                        tempSql += string.Format(":ORGANIZATION_{0},", c);
                        lstSqlPara.Add(currentAuth.ParentIdList[c]);
                    }
                }
                strSQL.Append(string.IsNullOrEmpty(tempSql) ? "''" : tempSql.TrimEnd(','));
                strSQL.Append("))");
            }
        }

        /// <summary>
        /// 可展列表数据权限（房贷，在用户当前权限基础上添加查看所在城市的权限）
        /// </summary>
        /// <param name="currentAuth"></param>
        /// <param name="strSQL"></param>
        /// <param name="lstSqlPara"></param>
        public void AddExtendPermissionHouse(QFUserAuth currentAuth, StringBuilder strSQL, List<object> lstSqlPara)
        {
            //最大权限则不需要检查权限
            if (!currentAuth.IsSelectAll)
            {
                var currentUser = UserService.GetCurrentUser();
                //准备权限COMPANY参数
                strSQL.Append(" AND (AA.COMPANY IN (");
                string tempSql = string.Empty;
                for (int c = 0; c < currentAuth.CompanyList.Count; c++)
                {
                    if (!string.IsNullOrEmpty(currentAuth.CompanyList[c]))
                    {
                        tempSql += string.Format(":COMPANY_{0},", c);
                        lstSqlPara.Add(currentAuth.CompanyList[c]);
                    }
                }
                //对于房贷，加入自己所在的城市
                tempSql += ":COMPANY_SELF,";
                lstSqlPara.Add(currentUser.CompanyId);

                strSQL.Append(string.IsNullOrEmpty(tempSql) ? "''" : tempSql.TrimEnd(','));

                //准备权限ACCOUNT参数
                strSQL.Append(") OR AA.ACCOUNT IN (");
                tempSql = string.Empty;
                for (int c = 0; c < currentAuth.AccountList.Count; c++)
                {
                    if (!string.IsNullOrEmpty(currentAuth.AccountList[c]))
                    {
                        tempSql += string.Format(":ACCOUNT_{0},", c);
                        lstSqlPara.Add(currentAuth.AccountList[c]);
                    }
                }
                strSQL.Append(string.IsNullOrEmpty(tempSql) ? "''" : tempSql.TrimEnd(','));

                //准备权限PARENT_ORGANIZATION参数
                strSQL.Append(") OR AA.PARENT_ORGANIZATION IN ( ");
                tempSql = string.Empty;
                for (int c = 0; c < currentAuth.ParentIdList.Count; c++)
                {
                    if (!string.IsNullOrEmpty(currentAuth.ParentIdList[c]))
                    {
                        tempSql += string.Format(":ORGANIZATION_{0},", c);
                        lstSqlPara.Add(currentAuth.ParentIdList[c]);
                    }
                }
                strSQL.Append(string.IsNullOrEmpty(tempSql) ? "''" : tempSql.TrimEnd(','));
                strSQL.Append("))");
            }
        }

        /// <summary>
        /// 扩展历史
        /// </summary>
        /// <param name="appCode"></param>
        /// <param name="actionGroup"></param>
        /// <returns></returns>
        public List<APP_EXTEND_RELATION> ExtendHistory(string appCode, string actionGroup)
        {
            /*注意：这里查询数据时 APP_EXTEND_RELATION 中
             * 字段 CONTRACT_ID 意义为 订单状态 APPSTATUS
             * 字段 INIT_CONTRACTID 意义为 订单状态名 APPSTATUSNAME
            */
            StringBuilder sbSQL = new StringBuilder();
            sbSQL.Append("SELECT AER.ID,");
            sbSQL.Append("AER.APP_CODE,");
            sbSQL.Append("AER.Init_App_Code,");
            sbSQL.Append("AER.PARENT_APP_CODE,");
            sbSQL.Append("AER.PARENT_CONTRACT_ID,");
            sbSQL.Append("AER.ACTION_GROUP,");
            sbSQL.Append("VA.LOANAMTOFCONTRACT SETTLEMENT_AMOUNT,");
            sbSQL.Append("AER.PERIOD_AMOUNT,");
            sbSQL.Append("AER.PERIOD_AMOUNT_USED,");
            sbSQL.Append("C.DATA_NAME PERIOD_STATUS,");
            sbSQL.Append("AER.CHANGED_TIME,");
            sbSQL.Append("AER.CHANGED_USER,");
            sbSQL.Append("AER.CREATED_TIME,");
            sbSQL.Append("AER.CREATED_USER,");
            sbSQL.Append("VA.APPSTATUS CONTRACT_ID,");
            sbSQL.Append("VA.APPSTATUSNAME INIT_CONTRACTID,");
            sbSQL.Append("AER.EXP_RATE,");
            sbSQL.Append("AER.EXP_FLOAT_RATE,");
            sbSQL.Append("AER.TIMES_NUMBER_USED");
            sbSQL.Append("  FROM app_extend_relation AER ");
            sbSQL.Append("    LEFT JOIN V_APPMAIN VA ON VA.APPCODE = AER.APP_CODE ");
            sbSQL.Append("    LEFT JOIN cr_data_dic C ON AER.PERIOD_STATUS = C.DATA_CODE ");
            sbSQL.Append(" AND AER.ACTION_GROUP = :ACTIONGROUP");
            sbSQL.Append(" where AER.Init_App_Code in (select A.init_app_code from app_extend_relation A where A.app_code= :APPCODE)");
            sbSQL.Append("ORDER BY ID");

            List<APP_EXTEND_RELATION> lstHistory = ExtendRelationService.SqlQuery(sbSQL.ToString(), new string[] { actionGroup, appCode }).ToList();
            string aCode = appCode;
            if (lstHistory.Count > 0)
            {
                aCode = lstHistory.First().INIT_APP_CODE;
            }
            sbSQL = new StringBuilder();
            sbSQL.Append("SELECT 0 ID, ");
            sbSQL.Append("       A.APPCODE APP_CODE,");
            sbSQL.Append("       NULL PARENT_APP_CODE,");
            sbSQL.Append("       NULL Init_App_Code,");
            sbSQL.Append("       NULL PARENT_CONTRACT_ID,");
            sbSQL.Append("       NULL ACTION_GROUP,");
            sbSQL.Append("       A.LOANAMTOFCONTRACT SETTLEMENT_AMOUNT,");
            sbSQL.Append("       NULL PERIOD_AMOUNT,");
            sbSQL.Append("       NULL PERIOD_AMOUNT_USED,");
            sbSQL.Append("       A.APPSTATUSNAME PERIOD_STATUS,");
            sbSQL.Append("       A.UPDATETIME CHANGED_TIME,");
            sbSQL.Append("       NULL CHANGED_USER,");
            sbSQL.Append("       A.CREATEDTIME CREATED_TIME,");
            sbSQL.Append("       A.CREATEDUSER CREATED_USER,");
            sbSQL.Append("       A.APPSTATUS CONTRACT_ID,");
            sbSQL.Append("       A.APPSTATUSNAME INIT_CONTRACTID,");
            sbSQL.Append("       NULL EXP_RATE,");
            sbSQL.Append("       NULL EXP_FLOAT_RATE,");
            sbSQL.Append("       NULL TIMES_NUMBER_USED");
            sbSQL.Append("  FROM V_APPMAIN A");
            sbSQL.Append(" WHERE A.AppCode = :APPCODE");
            APP_EXTEND_RELATION newAppExt = ExtendRelationService.SqlQuery(sbSQL.ToString(), aCode).FirstOrDefault();
            lstHistory.Add(newAppExt);
            lstHistory = lstHistory.OrderBy(h => h.CREATED_TIME).ToList();
            return lstHistory;
        }

        /// <summary>
        /// 已扩展过的申请单
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        public EnterListViewFiledList ExtendedList(ExtendApplySearchPara para)
        {
            //返回值
            EnterListViewFiledList enterList = new EnterListViewFiledList();
            //未授权访问任何资料
            if (para.AccessableCsac.Count == 0)
            {
                return enterList;
            }
            var execsqlCount = string.Empty;
            var execsqlData = string.Empty;

            var lstStatus = para.ListEnterStatus.Select(a => a.ToString()).ToList();

            string selectStr = "a.APPID,a.APPCODE,a.LOGO,a.PRODUCTCODE,a.PRODUCTNAME,a.CUSTOMERNAME,a.CUSTOMERIDCARD,a.APPLYAMT,a.LOANAMTOFCONTRACT,a.EXTENDAMTOFCONTRACT,a.SALESNAME,a.SALESNO,a.CSADNAME,a.CSADNO,a.BUSINESSDEPARTMENT,a.APPSTATUS,a.SORTING,a.APPNRSDSTATUS,a.APPAPPR1SDSTATUS,a.APPSTATUSNAME,a.CREATEDUSER,a.CREATEDTIME,a.UPDATETIME";    //要查询的字段
            string resultStr = selectStr.Replace("a.", ""); //最终显示的字段
            string fromStr = "app.V_APPMAIN a inner join app.APP_AUTH b on a.APPID=b.APP_ID"; //数据源
            string sortStr = "a.SORTING asc,a.CREATEDTIME desc"; //初始排序字段
            string whereStr = string.Empty;
            StringBuilder whereBuilder = new StringBuilder();

            //只查询扩展申请
            if (para.FuzzySearch)
            {
                if (!string.IsNullOrEmpty(para.AppCode) && !string.IsNullOrEmpty(para.CustomerName))
                {
                    whereBuilder.Append("and (instr(a.APPCODE,'" + para.AppCode + "')>0 ");
                    whereBuilder.Append("or instr(a.CUSTOMERNAME,'" + para.CustomerName + "')>0) ");
                }
            }
            else
            {
                #region 高级查询条件
                if (!string.IsNullOrEmpty(para.AppCode))
                {
                    whereBuilder.Append("and instr(a.APPCODE,'" + para.AppCode + "')>0 ");
                }
                if (!string.IsNullOrEmpty(para.CustomerName))
                {
                    whereBuilder.Append("and instr(a.CUSTOMERNAME,'" + para.CustomerName + "')>0 ");
                }
                if (!string.IsNullOrEmpty(para.CustomerIDCard))
                {
                    whereBuilder.Append("and instr(a.CUSTOMERIDCARD,'" + para.CustomerIDCard + "')>0 ");
                }
                if (para.ApplyStart != null)
                {
                    whereBuilder.Append("and a.CREATEDTIME>=to_date('" + para.ApplyStart.Value + "','YYYY-MM-DD HH24:MI:SS') ");
                }
                if (para.ApplyEnd != null)
                {
                    whereBuilder.Append("and a.CREATEDTIME<=to_date('" + para.ApplyEnd.Value + "','YYYY-MM-DD HH24:MI:SS') ");
                }
                if (!string.IsNullOrEmpty(para.SaleCode) && !string.IsNullOrEmpty(para.SaleName))
                {
                    whereBuilder.Append("and (instr(a.SALESNO,'" + para.SaleCode + "')>0 ");
                    whereBuilder.Append("or instr(a.SALESNAME,'" + para.SaleName + "')>0) ");
                }
                if (!string.IsNullOrEmpty(para.CsacCode) && !string.IsNullOrEmpty(para.CsacName))
                {
                    whereBuilder.Append("and (instr(a.CSADNO,'" + para.CsacCode + "')>0 ");
                    whereBuilder.Append("or instr(a.CSADNAME,'" + para.CsacName + "')>0) ");
                }
                #endregion
            }

            #region 通用查询条件
            if (lstStatus.Any())    //筛选进件状态或补件状态
            {
                whereBuilder.Append("and (");
                string statusStr = string.Empty;
                lstStatus.ForEach(a => statusStr += "a.APPSTATUS='" + a + "' or ");
                whereBuilder.Append(statusStr.Substring(0, statusStr.Length - 4) + ") ");
            }
            if (para.ListLogo.Any())    //筛选产品Logo    
            {
                whereBuilder.Append("and (");
                string logoStr = string.Empty;
                para.ListLogo.ForEach(a => logoStr += "a.LOGO='" + a + "' or ");
                whereBuilder.Append(logoStr.Substring(0, logoStr.Length - 4) + ") ");
            }
            //展期数据筛选
            whereBuilder.Append("and exists(select 1 from app.APP_EXTEND_RELATION where APP_CODE=a.APPCODE ");
            if (para.ExtendActionGroup.Any())  //筛选展期方式
            {
                whereBuilder.Append("and (");
                string actiongroupStr = string.Empty;
                para.ExtendActionGroup.ForEach(a => actiongroupStr += "ACTION_GROUP='" + a + "' or ");
                whereBuilder.Append(actiongroupStr.Substring(0, actiongroupStr.Length - 4) + ") ");
            }
            whereBuilder.Append(") ");
            string userAuthWhereStr = UserService.GetUserAuthWhereStr("b", para.InputMenuCode);
            if (!string.IsNullOrEmpty(userAuthWhereStr))
            {
                whereBuilder.Append("and (" + userAuthWhereStr + ") "); //筛选用户权限
            }
            #endregion
            whereStr = whereBuilder.ToString(); //去除前面的"and "
            #region 自定义排序条件
            if (para.Sort.Count > 0 && para.Sort.Any(a => a.Key != "" && a.Value != ""))  //点击表格标题排序
            {
                sortStr = string.Empty;
                foreach (KeyValuePair<string, string> kv in para.Sort)
                {
                    sortStr += kv.Key + " " + kv.Value + ",";
                }
                sortStr = sortStr.Substring(0, sortStr.Length - 1);
            }
            #endregion
            execsqlCount = PagingHelper.GetPagingCountSql(fromStr, whereStr);   //取数据总数sql
            execsqlData = PagingHelper.GetPagingDataSql(selectStr, resultStr, fromStr, sortStr, whereStr, para.PageIndex, para.PageSize);  //取分页数据sql
            var dataCount = VappmainService.SqlQuery(typeof(int), execsqlCount).Cast<int>().FirstOrDefault();  //数据总数
            var dataList = VappmainService.SqlQuery(execsqlData).ToList();    //分页数据
            enterList.SetParameters(dataCount, dataList, para);
            return enterList;
        }
    }
}
