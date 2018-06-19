/* ==============================================================================
 * 功能描述：拒贷信息APP_REFUSELOANSERVICE  
 * 创 建 者：leiz
 * 创建日期：2015/3/6 17:53:08
 * ==============================================================================*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;
using QK.QAPP.Entity;
using QK.QAPP.Global;
using QK.QAPP.Infrastructure;
using QK.QAPP.Infrastructure.Cache;
using QK.QAPP.Infrastructure.Data.EFRepository.Repository;
using QK.QAPP.IServices;

namespace QK.QAPP.Services
{
    public partial class APP_REFUSELOANSERVICE : RepositoryBaseSql, IAPP_REFUSELOANSERVICE
    {
        [Dependency]
        public ICR_DATA_DICService CrDataDicService { get; set; }
        /// <summary>
        /// 记录一级拒贷信息
        /// </summary>
        private string refuseLoanInfo = string.Empty;
        /// <summary>
        /// 人工或者系统拒贷中间变量
        /// </summary>
        private string tempRefuseLoanInfo = string.Empty;
        /// <summary>
        /// 黑名单拒贷信息中间变量
        /// </summary>
        //private string tempBlackInfo = string.Empty;

        public APP_REFUSELOANSERVICE(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {

        }

        /// <summary>
        /// 获取一级拒贷描述信息
        /// </summary>
        /// <param name="appid">申请ID</param>
        /// <returns>拒贷信息</returns>
        public string GetRefuseLoanInfo(string appid)
        {
            CR_DATA_DIC dataDic = new CR_DATA_DIC();
            APP_DCIN appDcinEntity = GetRefuseLoanEntity(appid);
            string dcinDecisionCode = appDcinEntity.DCIN_DECISION_CODE;
            string dcinRefuseCode = appDcinEntity.DCIN_REFUSE_CODE;
            try
            {
                //人工，考虑到会有多个情况，以逗号分隔，如：（D010302,D1109,D1109)
                if (!string.IsNullOrEmpty(dcinDecisionCode))
                {
                    string[] refuseCodes = dcinDecisionCode.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).ToArray();
                    foreach (var refuseCode in refuseCodes)
                    {
                        dataDic = CrDataDicService.GetDICByCode(refuseCode);
                        RefuseLoanCodeHandle(dataDic);
                        if (!refuseLoanInfo.Contains(tempRefuseLoanInfo))
                        {
                            /*自定义分隔符，多个拒贷类型以分号分割*/
                            string separator = string.IsNullOrEmpty(refuseLoanInfo) ? "" : ";";
                            refuseLoanInfo += separator + tempRefuseLoanInfo;
                        }
                    }
                }
                //RE拒贷 表app_dcin的dcin_Refuse_Code，可能有N个拒贷码，以逗号分隔  如：（D010302,D1109,D1109)
                if (!string.IsNullOrEmpty(dcinRefuseCode))
                {
                    string[] refuseCodes = dcinRefuseCode.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).ToArray();
                    foreach (var refuseCode in refuseCodes)
                    {
                        dataDic = CrDataDicService.GetDICByCode(refuseCode);
                        RefuseLoanCodeHandle(dataDic);
                        if (!refuseLoanInfo.Contains(tempRefuseLoanInfo))
                        {
                            /*自定义分隔符，多个拒贷类型以分号分割*/
                            string separator = string.IsNullOrEmpty(refuseLoanInfo) ? "" : ";";
                            refuseLoanInfo += separator + tempRefuseLoanInfo;
                        }
                    }
                }
                //反欺诈认定类别拒贷 包括黑名单和灰名单
                foreach (var appBlMainBlCodeEntity in GetBlackLoanEntity(appid))
                {
                    string blackCode = appBlMainBlCodeEntity.BL_CODE;
                    if (!string.IsNullOrEmpty(blackCode))
                    {
                        dataDic = CrDataDicService.GetDICByCode(blackCode);
                        RefuseLoanCodeHandle(dataDic);
                        if (!refuseLoanInfo.Contains(tempRefuseLoanInfo))
                        {
                            /*自定义分隔符，多个黑名单拒贷类型以分号分割*/
                            string separator = string.IsNullOrEmpty(refuseLoanInfo) ? "" : ";";
                            refuseLoanInfo += separator + tempRefuseLoanInfo;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Infrastructure.Log4Net.LogWriter.Biz("获取一级拒贷描述信息异常", appid, ex.Message.ToString());
            }
            
            Infrastructure.Log4Net.LogWriter.Biz("成功获取一级拒贷描述信息", appid, "拒贷信息："+refuseLoanInfo);
            return refuseLoanInfo;
        }

        /// <summary>
        /// 人工、系统拒贷、反欺诈认定类别拒贷逻辑处理
        /// </summary>
        /// <param name="dataDic"></param>
        public void RefuseLoanCodeHandle(CR_DATA_DIC dataDic)
        {
            long parentid = Convert.ToInt64(dataDic.PARENT_ID);
            CR_DATA_DIC dataDic2 = CrDataDicService.GetDICByID(parentid);
            //父id为空时取一级描述信息
            if (string.IsNullOrEmpty(dataDic2.PARENT_ID.ToString()))
            {
                tempRefuseLoanInfo = dataDic.DATA_NAME;
                return;
            }
            RefuseLoanCodeHandle(dataDic2);
        }

        /// <summary>
        /// 获取人工或者RE拒贷实体
        /// </summary>
        /// <param name="appid">appid</param>
        /// <returns></returns>
        public APP_DCIN GetRefuseLoanEntity(string appid)
        {
            string strSql = "select t.dcin_decision_code,t.dcin_Refuse_Code from app_dcin t where app_id=:appid";
            var entity = this.SqlQuery<APP_DCIN>(strSql, appid).FirstOrDefault();
            if (entity == null)
            {
                entity = new APP_DCIN();
            }
            Infrastructure.Log4Net.LogWriter.Biz("获取人工或者RE拒贷实体", appid, entity);
            return entity;
        }

        /// <summary>
        /// 获取黑名单/灰名单拒贷实体
        /// </summary>
        /// <param name="appid"></param>
        /// <returns></returns>
        public List<APP_BL_MAIN_BL_CODE> GetBlackLoanEntity(string appid)
        {
            string strSql = "select t1.*,t2.app_id from APP_BL_MAIN_BL_CODE t1 join APP_BL_MAIN t2 on t1.bl_main_id=t2.id where t2.app_id=:appid";
            var entity = this.SqlQuery<APP_BL_MAIN_BL_CODE>(strSql, appid).ToList();
            if (entity.Count == 0)
            {
                entity = new List<APP_BL_MAIN_BL_CODE>();
            }
            Infrastructure.Log4Net.LogWriter.Biz("获取黑名单/灰名单拒贷实体", appid, entity);
            return entity;
        }
    }
}
