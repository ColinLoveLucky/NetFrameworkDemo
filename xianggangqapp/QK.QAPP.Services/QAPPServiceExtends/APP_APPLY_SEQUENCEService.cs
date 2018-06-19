using Microsoft.Practices.Unity;
using QK.QAPP.Entity;
using QK.QAPP.Infrastructure;
using QK.QAPP.Infrastructure.Cache;
using QK.QAPP.Infrastructure.Data.EFRepository.Repository;
using QK.QAPP.Infrastructure.Log4Net;
using QK.QAPP.IServices;
using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace QK.QAPP.Services
{
    public partial class APP_APPLY_SEQUENCESERVICE
    {
        public string GetApplyNumber(string seqCode, string proNum, string cityCode)
        {

            if (seqCode == null || cityCode == null || proNum == null)
            {
                throw new Exception("方法GetApplyNumber的参数（seqCode,proNum,cityCode）不能为NULL！");
            }

            string returnNum = string.Empty;    //生成的申请号码
            string errorMsg = string.Empty;     //错误信息

            ObjectParameter[] parameters = new ObjectParameter[]
            {
                new ObjectParameter("i_vc_seqCode", seqCode),
                new ObjectParameter("i_vc_cityCode",cityCode),
                new ObjectParameter("i_vc_proNum",proNum), 
                new ObjectParameter("o_vc_returnNum",typeof(string)),
                new ObjectParameter("o_vc_errorMsg",typeof(string))
            };

            Ioc.GetService<IRepositoryBaseSql>().ExecuteFunction("P_APP_APPLYNUMBER", parameters);

            //获取输出参数
            returnNum = parameters[3].Value.ToString();
            errorMsg = parameters[4].Value.ToString();



            if (!string.IsNullOrEmpty(errorMsg))
            {
                //System.Diagnostics.Trace.WriteLine("生成流水号出现异常!", "申请流水号");
                LogWriter.Error("生成申请号出现异常!当前申请号为：" + returnNum);
            }

            return returnNum;
        }

        public string GetApplyNumberExtend(string seqCode, string proNum, string cityCode, string opeType)
        {
            if (String.IsNullOrEmpty(seqCode) || String.IsNullOrEmpty(proNum) || String.IsNullOrEmpty(cityCode)
                || String.IsNullOrEmpty(opeType))
            {
                throw new Exception("方法GetApplyNumber的参数（seqCode,proNum,cityCode,opeType）不能为空！");
            }

            string returnNum = string.Empty;    //生成的申请号码
            string errorMsg = string.Empty;     //错误信息

            ObjectParameter[] parameters = new ObjectParameter[]
            {
                new ObjectParameter("i_vc_seqCode", seqCode),
                new ObjectParameter("i_vc_cityCode",cityCode),
                new ObjectParameter("i_vc_proNum",proNum),
                new ObjectParameter("i_vc_opeType",opeType), 
                new ObjectParameter("o_vc_returnNum",typeof(string)),
                new ObjectParameter("o_vc_errorMsg",typeof(string))
            };

            Ioc.GetService<IRepositoryBaseSql>().ExecuteFunction("P_APP_APPLYNUMBER_EXTEND", parameters);

            //获取输出参数
            returnNum = parameters[4].Value.ToString();
            errorMsg = parameters[5].Value.ToString();

            if (!String.IsNullOrEmpty(errorMsg))
            {
                LogWriter.Error("生成申请号出现异常!当前申请号为：" + returnNum);
            }

            return returnNum;
        }

        public List<APP_APPLY_SEQUENCE> GetApplySeqByCityCode(string cityCode)
        {

            //var sql = "SELECT * FROM APP_APPLY_SEQUENCE WHERE CITY_CODE=:cityCode";
            //var list = this.SqlQuery<APP_APPLY_SEQUENCE>(sql, cityCode).ToList();
            //return list;

            return this.Find(c => c.CITY_CODE == cityCode).ToList();
        }

        public void UpdateSeq(APP_APPLY_SEQUENCE entity)
        {
            if (entity == null)
                return;
            entity.EDIT_TIME = DateTime.Now;

            this.Update(entity);


            //if (entity == null)
            //    return;
            //StringBuilder sql = new StringBuilder();
            //sql.Append(" UPDATE APP_APPLY_SEQUENCE SET ");
            //sql.Append(" SEQ_CODE = :SEQ_CODE, DESCR = :DESCR, CURRENT_SEQVALUE = :CURRENT_SEQVALUE, EDIT_TIME = :EDIT_TIME, DATA_FORMAT = :DATA_FORMAT, LENGTH = :LENGTH, INIT_VALUE = :INIT_VALUE, RESET_TYPE = :RESET_TYPE, LAST_DATE = :LAST_DATE, PREFIX = :PREFIX, OPE_TYPE = :OPE_TYPE, CITY_CODE = :CITY_CODE ");
            //sql.Append(" WHERE ID = :ID ");
            //List<object> paras = new List<object>();
            //paras.Add(entity.ID);
            //paras.Add(entity.SEQ_CODE);
            //paras.Add(entity.DESCR);
            //paras.Add(entity.CURRENT_SEQVALUE);
            //paras.Add(entity.EDIT_TIME);
            //paras.Add(entity.DATA_FORMAT);
            //paras.Add(entity.LENGTH);
            //paras.Add(entity.INIT_VALUE);
            //paras.Add(entity.RESET_TYPE);
            //paras.Add(entity.LAST_DATE);
            //paras.Add(entity.PREFIX);
            //paras.Add(entity.OPE_TYPE);
            //paras.Add(entity.CITY_CODE);
            //this.ExecuteSqlCommand(sql.ToString(), paras.ToArray());
        }

        /// <summary>
        /// 描述：更新城市编码
        /// 修改人：leiz
        /// 修改时间：20150403
        /// </summary>
        /// <param name="oldCityCode"></param>
        /// <param name="newCityCode"></param>
        /// <returns></returns>
        public bool UpdateCityCodeByCityCode(string oldCityCode, string newCityCode)
        {
            if (string.IsNullOrEmpty(oldCityCode) || string.IsNullOrEmpty(newCityCode))
                return false;

            var sequences = this.Find(c => c.CITY_CODE == oldCityCode).ToList();
            foreach (var item in sequences)
            {
                item.CITY_CODE = newCityCode;
            }

            return this.UpdateMultiple(sequences);

            //if (string.IsNullOrEmpty(oldCityCode) || string.IsNullOrEmpty(newCityCode))
            //    return;
            //string sql = " UPDATE APP_APPLY_SEQUENCE SET CITY_CODE=:newCityCode, EDIT_TIME=sysdate WHERE CITY_CODE=:oldCityCode ";
            //this.ExecuteSqlCommand(sql, newCityCode, oldCityCode);
        }




        public bool AddSeq(APP_APPLY_SEQUENCE entity)
        {
            if (entity == null)
                return false;

            entity.IS_RUNNING = "1";

            return this.Add(entity);

            //if (entity == null)
            //    return;
            //StringBuilder sql = new StringBuilder();
            //sql.Append(" INSERT INTO APP_APPLY_SEQUENCE ");
            //sql.Append(" (ID, SEQ_CODE, DESCR, CURRENT_SEQVALUE, CREATE_TIME, EDIT_TIME, DATA_FORMAT, LENGTH, INIT_VALUE, RESET_TYPE, LAST_DATE, IS_RUNNING, PREFIX, OPE_TYPE, CITY_CODE) ");
            //sql.Append(" VALUES ");
            //sql.Append(" (:ID, :SEQ_CODE, :DESCR, :CURRENT_SEQVALUE, :CREATE_TIME, :EDIT_TIME, :DATA_FORMAT, :LENGTH, :INIT_VALUE, :RESET_TYPE, :LAST_DATE, '1', :PREFIX, :OPE_TYPE, :CITY_CODE) ");
            //List<object> paras = new List<object>();
            //paras.Add(entity.ID);
            //paras.Add(entity.SEQ_CODE);
            //paras.Add(entity.DESCR);
            //paras.Add(entity.CURRENT_SEQVALUE);
            //paras.Add(entity.CREATE_TIME);
            //paras.Add(entity.EDIT_TIME);
            //paras.Add(entity.DATA_FORMAT);
            //paras.Add(entity.LENGTH);
            //paras.Add(entity.INIT_VALUE);
            //paras.Add(entity.RESET_TYPE);
            //paras.Add(entity.LAST_DATE);
            //paras.Add(entity.PREFIX);
            //paras.Add(entity.OPE_TYPE);
            //paras.Add(entity.CITY_CODE);
            //this.ExecuteSqlCommand(sql.ToString(), paras.ToArray());
        }

        /// <summary>
        /// 描述：删除城市编码
        /// 修改人：leiz
        /// 修改时间：20150403
        /// </summary>
        /// <param name="cityCode">城市编码</param>
        /// <returns></returns>
        public bool DeleteByCityCode(string cityCode)
        {
            if (string.IsNullOrEmpty(cityCode))
                return false;

            var sequences = this.Find(c => c.CITY_CODE == cityCode).ToList();

            return this.DeleteMultiple(sequences);

            //if (string.IsNullOrEmpty(cityCode))
            //    return;

            //string sql = "DELETE APP_APPLY_SEQUENCE WHERE CITY_CODE=:cityCode";

            //this.ExecuteSqlCommand(sql, cityCode);
        }
    }
}
