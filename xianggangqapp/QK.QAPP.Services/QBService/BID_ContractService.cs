using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QK.QAPP.IServices;
using QK.QAPP.Entity;
using QK.QAPP.Infrastructure;
using System.Net.Http;
using System.Net;
using QK.QAPP.Global;
using QK.QAPP.Entity.QBThirdEntity.ContractInfo;
using Newtonsoft.Json;
using QK.QAPP.Entity.ExtendEntity;
using QK.QAPP.Infrastructure.Data.EFRepository.Repository;
namespace QK.QAPP.Services
{
    public class BID_ContractService : IBID_ContractService
    {
        public ICR_DATA_DICService crDataDicService { get; set; }
        public ICalcService calcService { get; set; }
        public IV_APP_CONTRACTSERVICE appContractService { get; set; }
        public IAPP_MAINSERVICE appMainService { get; set; }
        public IAPP_EXTEND_RELATIONSERVICE appExtendService { get; set; }
        public IBID_LabelPactService labelPactService { get; set; }
        public IQuotaManageService quotaManageService { get; set; }


        /// <summary>
        /// 生成合同
        /// </summary>
        /// <returns></returns>
        public Task<bool> ContractCreate(ContractCreateRequest conRequest, QB_V_BID_DETAIL bid, Global.ContractGlobalConfig contractGlobal, QFUser user, bool isCreate)
        {
            return Task.Run(() =>
            {
                return RunCreateCon(conRequest, bid, contractGlobal, user, isCreate);
            });

        }
        /// <summary>
        /// 初始化赋值借款交易信息
        /// </summary>
        /// <param name="bowr">借款交易实体</param>
        /// <param name="calcContent">还款计划实体</param>
        /// <param name="appContract">当前进件实体</param>
        /// <returns></returns>
        private BOWR_DETAIL InitBowrDetail(BOWR_DETAIL bowr, List<CalcResponse.Content> calcContent, V_APP_CONTRACT appContract, QB_V_BID_DETAIL bid)
        {
            if (calcContent != null && calcContent.Count > 0)
            {
                bowr.BEGN_INTR_DATE = calcContent.First().opndate == null || calcContent.First().opndate.Length != 8 ? "" : calcContent.First().opndate.Insert(6, "-").Insert(4, "-");//起息日=签约日期
            }
            bowr.RISK_COMP = bid.BID_LOAN_LOSS_PROVISION.HasValue ? string.Format("{0:N}", bid.BID_LOAN_LOSS_PROVISION) : "0";//逾期风险补偿金
            bowr.PRIC_AMT = appContract.LOAN_AMT_OF_CONTRACT.HasValue ? string.Format("{0:N}", appContract.LOAN_AMT_OF_CONTRACT) : "0";//借款本金
            bowr.CNST_FEE = bid.BID_BORROW_FEE.HasValue ? string.Format("{0:N}", bid.BID_BORROW_FEE) : "0";//借款咨询费
            if (bid.BID_LOAN_LOSS_PROVISION.HasValue && bid.BID_BORROW_FEE.HasValue)
            {
                bowr.CNST_NRS_FEE = string.Format("{0:N}", bid.BID_BORROW_FEE - bid.BID_LOAN_LOSS_PROVISION);//借款咨询费（不包含风险补偿金）
            }
            bowr.REM_AMT = string.Format("{0:N}", appContract.LOAN_AMT_OF_CONTRACT - bid.BID_BORROW_FEE);//到手金额=合同金额-借款咨询费
            bowr.PMT_TERM = appContract.TERMS.HasValue ? appContract.TERMS.ToString() : "0";//还款分期期数
            bowr.TERM_MAX = "";//借款服务费分期
            if (calcContent != null && calcContent.Count > 0)
            {
                decimal pmtAmtMoth = calcContent.Where(w => w.termnum != 1).Sum(s => s.returnsum);//剩余月数偿还本息数额
                bowr.PMT_DATE = calcContent.First().enddate == null ? "" : Convert.ToInt32(calcContent.First().enddate.Substring(6, 2)).ToString();//还款日
                bowr.STAT_DATE = calcContent.First().opndate == null || calcContent.First().opndate.Length != 8 ? "" : calcContent.First().opndate.Insert(6, "-").Insert(4, "-");//还款开始日期
                bowr.END_DATE = calcContent.Last().enddate == null || calcContent.Last().enddate.Length != 8 ? "" : calcContent.Last().enddate.Insert(6, "-").Insert(4, "-");//还款结束日期
                bowr.FIRT_PMT_AMT = calcContent.First().returnsum.ToString("N");//第一期应还本息数额
                bowr.FIRT_SF_PMT_AMT = (calcContent.First().bromanfee + calcContent.First().returnsum).ToString("N");//第一期应还本息数额(含借款服务费)

                if (calcContent.Where(w => w.termnum != 1).Count() > 0)
                {
                    bowr.PMT_AMT_MOTH = calcContent.Where(w => w.termnum != 1).First().returnsum.ToString("N");//剩余月偿还本息数额（每月）
                    bowr.PMT_SF_AMT_MOTH = (calcContent.Where(w => w.termnum != 1).First().returnsum + calcContent.Where(w => w.termnum != 1).First().bromanfee).ToString("N");//剩余期数月偿还本息数额(含借款服务费)是指的每月
                }
                else
                {
                    bowr.PMT_SF_AMT_MOTH = (0).ToString("N");
                }
                bowr.RAPD_AMT = "0";//和顺利确认 都是0
                bowr.RESI_PRIC = "0";//和顺利确认 都是0
                bowr.TOTL_SERV_FEE = calcContent.Sum(s => s.bromanfee).ToString("N");//借款服务费总额
                bowr.SERV_FEE = calcContent.First().bromanfee.ToString("N");//每期借款服务费
                bowr.PMT_TERM = calcContent.Count().ToString();
                bowr.RATE_AMT = calcContent.Sum(s => s.interest).ToString("N"); //借款利息
            }
            return bowr;
        }
        /// <summary>
        /// 初始化展期交易信息
        /// </summary>
        /// <param name="rlov">展期实体</param>
        /// <param name="calcContent">还款计划</param>
        /// <param name="appContract">当前进件信息</param>
        /// <param name="parentAppInfo">上期进件信息</param>
        /// <param name="bid">标的信息</param>
        /// <returns></returns>
        private RLOV_INFO InitRlovInfo(RLOV_INFO rlov, List<CalcResponse.Content> calcContent, V_APP_CONTRACT appContract, V_APP_CONTRACT parentAppInfo, QB_V_BID_DETAIL bid)
        {
            rlov.CONT_CODE = bid.BID_CONTRACT_NO == null ? "" : bid.BID_CONTRACT_NO;
            rlov.CONT_SIGN_PLCE = "";//签署地点
            rlov.FUND_CHAN = bid.BID_CHANNELNAME == null ? "" : bid.BID_CHANNELNAME;//资金渠道
            rlov.NEED_AMT = "";//借款人需要偿还的金额

            rlov.MONTH_RATE = appContract.RATE.HasValue ? Convert.ToDouble(appContract.RATE * 100 / 12).ToString("0.00") : "0.00";//月利率

            rlov.RISK_COMP = bid.BID_LOAN_LOSS_PROVISION.HasValue ? string.Format("{0:N}", bid.BID_LOAN_LOSS_PROVISION) : "0";//风险补偿金
            rlov.RLOV_CNST_FEE = bid.BID_BORROW_FEE.HasValue ? string.Format("{0:N}", bid.BID_BORROW_FEE) : "0";//展期咨询费
            rlov.RLOV_PRIC_AMT = appContract.LOAN_AMT_OF_CONTRACT.HasValue ? string.Format("{0:N}", appContract.LOAN_AMT_OF_CONTRACT) : "0";//展期贷款本金数额
            rlov.RLOV_RATE = appContract.RATE.HasValue ? Convert.ToDouble(appContract.RATE * 100).ToString("0.00") : "";//展期利率
            rlov.RLOV_TERM = appContract.TERMS.HasValue ? appContract.TERMS.ToString() : "";//展期期限
            if (calcContent != null && calcContent.Count > 0)
            {
                rlov.BEGN_INTR_DATE = calcContent.First().opndate == null || calcContent.First().opndate.Length != 8 ? "" : calcContent.First().opndate.Insert(6, "-").Insert(4, "-");//起息日=签约日期
                rlov.PMT_STAT_DATE = calcContent.First().opndate == null || calcContent.First().opndate.Length != 8 ? "" : calcContent.First().opndate.Insert(6, "-").Insert(4, "-");//展期还款开始日期
                rlov.PMT_END_DATE = calcContent.Last().enddate == null || calcContent.Last().enddate.Length != 8 ? "" : calcContent.Last().enddate.Insert(6, "-").Insert(4, "-");//展期还款结束日期
                rlov.SERV_FEE = calcContent.First().bromanfee.ToString("N");//展期借款月服务费
                rlov.TOTL_SERV_FEE = calcContent.Sum(s => s.bromanfee).ToString("N");//展期借款服务费总额
                rlov.RLOV_PMT_DUE_DATE = calcContent.First().enddate == null ? "" : Convert.ToInt32(calcContent.First().enddate.Substring(6, 2)).ToString();//展期还款日
                rlov.RATE_AMT = calcContent.Sum(s => s.interest).ToString("N");//展期利息
                rlov.RLOV_RATE_AMT = calcContent.Sum(s => s.interest).ToString("N");//展期贷款利息
                rlov.PRIC_RATE = (calcContent.Sum(s => s.principle) + calcContent.Sum(s => s.interest)).ToString("N");//展期贷款本息
            }
            rlov.PREV_RATE_AMT = string.Format("{0:N}", parentAppInfo.RATE * parentAppInfo.LOAN_AMT_OF_CONTRACT / 12);//上期贷款利息 上期利息 = 上期借款本金*月利率
            if (parentAppInfo != null)
            {
                decimal amtp = parentAppInfo.LOAN_AMT_OF_CONTRACT == null ? 0 : parentAppInfo.LOAN_AMT_OF_CONTRACT.ToDecimal();
                decimal amta = appContract.LOAN_AMT_OF_CONTRACT == null ? 0 : appContract.LOAN_AMT_OF_CONTRACT.ToDecimal();
                rlov.PRIC_DIFF_AMT = (amtp - amta).ToString("N");//上期与本期贷款的本金差额

            }
            return rlov;
        }
        /// <summary>
        /// 根据字典CODE获取名称
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public string GetCrDataDicByCode(string code)
        {
            string value = "";
            if (!string.IsNullOrWhiteSpace(code))
            {
                var dicName = crDataDicService.GetDICByCode(code);
                if (dicName != null)
                {
                    return dicName.DATA_NAME;
                }
            }
            return value;
        }
        /// <summary>
        /// 获取合同模板及信托对应关系value
        /// </summary>
        /// <param name="dic"></param>
        /// <param name="channel"></param>
        /// <returns></returns>
        public string GetChannelValue(Dictionary<string, List<string>> dic, string channel)
        {
            string value = "";
            if (dic != null)
            {
                foreach (var item in dic)
                {
                    if (item.Value.Contains(channel))
                    {
                        return item.Key;
                    }
                }
            }
            return value;
        }
        public bool CreateC(QB_V_BID_DETAIL bid, Global.ContractGlobalConfig contractGlobal, QFUser user, out ContractCreateRequest conRequest)
        {
            DtoMessage<ContractCreateResponse> conReslut = new DtoMessage<ContractCreateResponse>();
            conRequest = new ContractCreateRequest();

            try
            {


                V_APP_CONTRACT parentAppInfo = null;//上期进件信息
                QB_V_BID_DETAIL parentBidInfo = null;//原合同信息
                // 01 获取申请端合同信息
                var appContract = appContractService.FirstOrDefault(f => f.APP_CODE == bid.BID_APP_CODE);
                //获取展期表中数据判断是否是展过期的
                var appExtend = appExtendService.FirstOrDefault(f => f.APP_CODE == bid.BID_APP_CODE);

                //获取原展期申请信息
                if (appExtend != null)
                {
                    parentAppInfo = appContractService.FirstOrDefault(f => f.APP_CODE == appExtend.PARENT_APP_CODE);
                    if (parentAppInfo != null)
                        parentBidInfo = labelPactService.GetBidDetail(parentAppInfo.APP_CODE, contractGlobal.QbPartner, contractGlobal.QbPartnerKey);
                }
                DateTime zhTime = DateTime.Now;//默认值
                // 02 请求试算接口，获取试算结果  试算结果的第一期开始日期是签约日期，结束日期取后两位是还款日
                CalcRequest calcRequest = new CalcRequest();
                calcRequest.version =  GlobalSetting.CalcVersion==null?"2.0":GlobalSetting.CalcVersion;//试算版本1.0-小贷通  2.0-T24
                calcRequest.sysid = "Qapp";
                calcRequest.idnum = bid.BID_CUSTOMER_ID_NO;
                calcRequest.pactno = bid.BID_CONTRACT_NO;
                if (appExtend != null)//如果是展期合同要取上一笔合同的结束日期作为试算接口的签约日期
                {

                    if (parentBidInfo != null)
                    {
                        calcRequest.signdate = parentBidInfo.BID_CONTRACT_END_DATE.HasValue ? Convert.ToDateTime(parentBidInfo.BID_CONTRACT_END_DATE).ToString("yyyyMMdd") : "";
                        calcRequest.enddate = Convert.ToDateTime(parentBidInfo.BID_CONTRACT_END_DATE).AddMonths(Int32.Parse(appContract.TERMS.HasValue ? appContract.TERMS.ToString() : "0")).ToString("yyyyMMdd");
                    }
                }
                else
                {
                    DateTime nowTime = DateTime.Now;
                    DateTime endTime = DateTime.Now;
                    //获取非工作日日期列表
                    var weekend = quotaManageService.GetWeekend();
                    //默认T+1排除非工作日
                    string T1Date = DateTime.Now.AddDays(1).ToShortDateString();
                    quotaManageService.GetTiWorkDay(weekend, 1, out T1Date);
                    if (GlobalSetting.Contract_T2P_ZH_FD.Keys.Contains(bid.BID_HANG_TYPE))//中航房贷14：00之前起息日T、放款日T--14：00之后起息日T+1、放款日T+1
                    {
                        zhTime = Convert.ToDateTime(GlobalSetting.Contract_T2P_ZH_FD[bid.BID_HANG_TYPE]);//配置的时间节点
                        calcRequest.signdate = nowTime.Date.ToString("yyyyMMdd");
                        if (DateTime.Now >= zhTime)
                        {
                            calcRequest.signdate = Convert.ToDateTime(T1Date).ToString("yyyyMMdd");
                            endTime = Convert.ToDateTime(T1Date);
                        }

                    }
                    else if (GlobalSetting.Contract_T1_QB_AMT_CODE.Contains(bid.BID_HANG_TYPE))//中航额度当日完成T+1
                    {
                        calcRequest.signdate = Convert.ToDateTime(T1Date).ToString("yyyyMMdd");
                        endTime = Convert.ToDateTime(T1Date);
                    }
                    else
                    {
                        calcRequest.signdate = nowTime.Date.ToString("yyyyMMdd");
                    }
                    calcRequest.enddate = endTime.AddMonths(Int32.Parse(appContract.TERMS.HasValue ? appContract.TERMS.ToString() : "0")).ToString("yyyyMMdd");
                }

                calcRequest.month = appContract.TERMS.HasValue ? appContract.TERMS.ToString() : "0"; //对应的期限
                calcRequest.pactamt = appContract.LOAN_AMT_OF_CONTRACT.HasValue ? appContract.LOAN_AMT_OF_CONTRACT.ToString() : "0.00";
                calcRequest.rate = appContract.RATE.HasValue ? (appContract.RATE * 100).ToString() : "0.00";
                calcRequest.srate = appContract.SERVICE_CHARGE_RATIO.HasValue ? (appContract.SERVICE_CHARGE_RATIO * 100).ToString() : "0.00";
                calcRequest.isfixdate = bid.BID_IS_FIX_REPAY; // 是否固定还款日，0-否（日对日），1-是(固定还款日)
                calcRequest.occtype = bid.BID_OCCUR_TYPE;//借款类型，1-普通2-循环贷3-借新还旧4-非继承还款日

                if (bid.BID_OCCUR_TYPE.Equals("2") || bid.BID_OCCUR_TYPE.Equals("3"))//当借款类型为 2、3时必为原合同号，当借款类型为1时传固定值”newPactno”
                {
                    if (parentBidInfo != null)
                    {
                        calcRequest.oldPactno = parentBidInfo.BID_CONTRACT_NO;
                    }
                    else
                        calcRequest.oldPactno = bid.BID_RO_CONTRACT_NO;//原合同号
                }
                else
                {
                    calcRequest.oldPactno = "newPactno";
                }
                calcRequest.returntype = bid.BID_REPAY_TYPE; //还款方式1-等额本息3-利随本清 4-按月结息
                var productMap= labelPactService.GetProductMap(bid.BID_BUS_TYPE);
                calcRequest.kindno = productMap.QB_T24_PRD_ODE == null ? bid.BID_BUS_TYPE : productMap.QB_T24_PRD_ODE;   //借款产品编号，由数据库统一同步到各系统或做转换
                CalcResponse calcResponse = new CalcResponse();
                var calcContent = new List<CalcResponse.Content>();
                calcResponse = calcService.GetCalcResult(calcRequest, contractGlobal.CalcURL);//试算接口请求返回
                if (calcResponse != null && calcResponse.code == "0000")
                {
                    calcContent = calcResponse.content.OrderBy(o => o.termnum).ToList();
                    PreCalcRequest pcr = new PreCalcRequest();
                    pcr.BidCode = bid.BID_CODE;
                    pcr.StarTime = Convert.ToDateTime(calcContent.First().opndate.Insert(6, "-").Insert(4, "-"));
                    pcr.EndTime = Convert.ToDateTime(calcContent.Last().enddate.Insert(6, "-").Insert(4, "-"));
                    pcr.Day = Convert.ToInt32(calcContent.First().enddate.Substring(6, 2)).ToString();
                    calcService.SetRepayMentDay(pcr, contractGlobal.QKSetRepayMentDay);
                }
                //获取app_main表数据
                APP_MAIN appMain = appMainService.FirstOrDefault(f => f.APP_CODE == bid.BID_APP_CODE);
                if (contractGlobal.CONTRACT_IS_CREATE.ToUpper() != "TRUE")
                {
                    var IsCreate = false;

                    if (appMain.CONTRACT_IS_CREATE == "Y")
                    {

                        if (bid.BID_SIGNED_TIME == null)//如果当前时间不是当天重新生成合同
                        {
                            IsCreate = true;
                        }
                        else
                        {
                            //如果签约时间<当前时间0点之前并且当前时间>=0点 需要重新生成合同
                            if (bid.BID_SIGNED_TIME < DateTime.Now.Date && DateTime.Now >= DateTime.Now.Date)
                            {
                                IsCreate = true;
                            }
                            //如果签约时间<配置时间并且当前时间>配置时间  需要重新生成合同
                            if (GlobalSetting.Contract_T2P_ZH_FD.Keys.Contains(bid.BID_HANG_TYPE) && bid.BID_SIGNED_TIME < zhTime && DateTime.Now >= zhTime)
                            {

                                IsCreate = true;
                            }
                        }
                        if (!IsCreate)
                        {
                            return IsCreate;
                        }
                    }
                }

                //还款计划
                var payment = new BIZ_ENTS<List<REPAYMENT>>();
                var paymentList = new List<REPAYMENT>();
                foreach (var item in calcContent)
                {
                    REPAYMENT pay = new REPAYMENT();
                    pay.QS = item.termnum.ToString();//第几期
                    pay.PMT_DATE = item.enddate;//还款日期
                    pay.PMT_AMT_MOTH = (item.principle + item.interest + item.bromanfee).ToString("N");//月还款额
                    pay.RESI_PRIC = (item.pactamt - calcContent.Where(w => w.termnum <= item.termnum).Sum(s => s.principle)).ToString("N");//剩余本金
                    pay.T_PRIC_AMT = item.principle.ToString("N");//偿还本金
                    pay.T_PMT_AMT = (item.interest + item.principle).ToString("N");//偿还本息
                    pay.T_RATE_AMT = item.interest.ToString("N");//偿还利息
                    pay.T_SERV_FEE = item.bromanfee.ToString("N");//借款服务费
                    pay.T_LS_REPAY_AMO = "";//一次性还款金额
                    paymentList.Add(pay);
                }
                payment.BIZ_ENT = paymentList;
                //产品信息
                var prodInfo = new BIZ_ENTS<PROD_INFO>();
                var prodinfo = new PROD_INFO() { LOAN_PURPOSE = "", RATE = "", RPMT_TYPE = "", TERM_MAX = "", TERM_MIN = "", MONTH_RATE = "", YEAR_RATE = "", RATE_TYPE = "", INST_FEE_GPS = "", SERV_FEE_GPS = "", PU_RATE = "" };
                prodinfo.RATE_TYPE = GetCrDataDicByCode(appContract.RATE_TYPE);//利率类型
                prodinfo.RATE = appContract.RATE.HasValue ? Convert.ToDouble(appContract.RATE * 100).ToString("0.00") : "0.00";//年利率
                prodinfo.YEAR_RATE = appContract.RATE.HasValue ? Convert.ToDouble(appContract.RATE * 100).ToString("0.00") : "0.00";//年利率
                prodinfo.MONTH_RATE = appContract.RATE.HasValue ? Convert.ToDouble(appContract.RATE * 100 / 12).ToString("0.00") : "0.00";//月利率
                prodinfo.PU_RATE = appContract.DEFAULT_INTEREST_RATIO.HasValue ? Convert.ToDouble(appContract.DEFAULT_INTEREST_RATIO * 100).ToString("0.00") : "0.00";//罚息利率
                prodinfo.RPMT_TYPE = GetCrDataDicByCode(appContract.PAYTYPE);//还款方式
                prodinfo.TERM_MAX = appContract.TERMS.HasValue ? appContract.TERMS.ToString() : "0";//最大期数
                prodinfo.LOAN_PURPOSE = GetCrDataDicByCode(appContract.LOAN_PURPOSE);//贷款用途
                prodinfo.INST_FEE_GPS = contractGlobal.INST_FEE_GPS;//常量500
                prodinfo.SERV_FEE_GPS = contractGlobal.SERV_FEE_GPS;//常量150

                prodInfo.BIZ_ENT = prodinfo;

                //基本信息
                var baseInfo = new BIZ_ENTS<BASE_INFO>();
                var baseinfo = new BASE_INFO() { CONT_CODE = "", CONT_SIGN_PLCE = "", FUND_CHAN = "", ADDR = "", CITY = "", PROV = "", REGN = "", ONE = "", THREE = "", TWO = "", HMT_END_DATE = "", HMT_STAT_DATE = "", CUS_SER_TEL = "" };
                if (parentBidInfo != null)//如果是展期合同信息
                {
                    baseinfo.CONT_CODE = parentBidInfo.BID_CONTRACT_NO == null ? "" : parentBidInfo.BID_CONTRACT_NO;
                    baseinfo.CONT_SIGN_DATE = parentBidInfo.BID_SIGNED_TIME == null ? "" : Convert.ToDateTime(parentBidInfo.BID_SIGNED_TIME).ToString("yyyy-MM-dd");
                }
                else
                {
                    baseinfo.CONT_CODE = bid.BID_CONTRACT_NO == null ? "" : bid.BID_CONTRACT_NO;
                }
                baseinfo.CONT_SIGN_PLCE = "";//签署地点
                baseinfo.FUND_CHAN = bid.BID_CHANNELNAME == null ? "" : bid.BID_CHANNELNAME;//资金渠道
                baseinfo.HMT_STAT_DATE = "";//合同开始日期
                baseinfo.HMT_END_DATE = "";//合同结束日期
                baseInfo.BIZ_ENT = baseinfo;

                //借款人信息
                var bowrInfo = new BIZ_ENTS<BOWR_INFO>();
                var bowrinfo = new BOWR_INFO() { BAND_DEPT = "", BAND_DEPT_ACNT = "", BAND_DEPT_NAME = "", CERT_NO = "", CERT_TYPE = "", RESV_MOBL = "", USER_ADDR = "", USER_HOU_ADDR = "", USER_MAIL = "", USER_NAME = "", QF_USERID = "", FAX = "", ZIDCODE = "", FADDR = "", FPR = "", FAREA = "", FIX_MOBL = "" };
                bowrinfo.BAND_DEPT = appContract.BANK_NAME == null ? "" : appContract.BANK_NAME;
                bowrinfo.BAND_DEPT_SUB = appContract.BANK_SUB == null ? bowrinfo.BAND_DEPT : bowrinfo.BAND_DEPT + appContract.BANK_SUB;
                bowrinfo.BAND_DEPT_ACNT = appContract.BANK_ACCOUNT == null ? "" : appContract.BANK_ACCOUNT;
                bowrinfo.CERT_TYPE = GetCrDataDicByCode(appContract.ID_TYPE);
                bowrinfo.RESV_MOBL = appContract.MOBILE1 == null ? "" : appContract.MOBILE1;
                bowrinfo.FIX_MOBL = appContract.RESIDENT_TEL_NO == null ? "" : appContract.RESIDENT_TEL_NO;
                bowrinfo.USER_ADDR = GetCrDataDicByCode(appContract.RESIDENT_PROVINCE) + GetCrDataDicByCode(appContract.RESIDENT_CITY) + appContract.RESIDENT_ADDRESS;
                bowrinfo.USER_HOU_ADDR = GetCrDataDicByCode(appContract.REGISTER_PROVINCE) + GetCrDataDicByCode(appContract.REGISTER_CITY) + appContract.REGISTER_ADDRESS;
                bowrinfo.USER_MAIL = appContract.EMAIL == null ? "" : appContract.EMAIL;
                bowrinfo.USER_NAME = appContract.NAME == null ? "" : appContract.NAME;
                bowrinfo.CERT_NO = appContract.ID_NO == null ? "" : appContract.ID_NO;
                bowrinfo.BAND_DEPT_NAME = appContract.NAME == null ? "" : appContract.NAME;
                bowrinfo.ZIDCODE = appContract.POSTCODE == null ? "" : appContract.POSTCODE;
                bowrinfo.FADDR = GetCrDataDicByCode(appContract.HOUSE_PROVINCE) + GetCrDataDicByCode(appContract.HOUSE_CITY) + appContract.HOUSE_ADDRESS;//借款人房产地址
                bowrinfo.FAREA = appContract.COVERED_AREA.HasValue ? appContract.COVERED_AREA.ToString() : "";//借款人房屋面积
                bowrInfo.BIZ_ENT = bowrinfo;
                //借款交易信息
                var bowrDetail = new BIZ_ENTS<BOWR_DETAIL>();
                var bowr = new BOWR_DETAIL() { BEGN_INTR_DATE = "", CNST_FEE = "", END_DATE = "", FIRT_PMT_AMT = "", PMT_AMT_MOTH = "", PMT_SF_AMT_MOTH = "", PMT_DATE = "", PMT_TERM = "", PRIC_AMT = "", RAPD_AMT = "", RATE_AMT = "", RESI_PRIC = "", RISK_COMP = "", SERV_FEE = "", STAT_DATE = "", TOTL_SERV_FEE = "", TERM_MAX = "", REM_AMT = "", CNST_NRS_FEE = "", FIRT_SF_PMT_AMT = "" };
                if (appExtend != null)//判断是否是展期
                {
                    if (parentAppInfo != null)
                    {
                        bowr.PRIC_AMT = parentAppInfo.LOAN_AMT_OF_CONTRACT.HasValue ? string.Format("{0:N}", parentAppInfo.LOAN_AMT_OF_CONTRACT) : "0";//原合同借款本金
                        if (parentBidInfo != null)
                        {
                            //int terms = parentAppInfo.TERMS == null ? 0 :Convert.ToInt32(parentAppInfo.TERMS);
                            bowr.PMT_DATE = parentBidInfo.BID_FIX_REPAY == null ? "" : Convert.ToInt32(parentBidInfo.BID_FIX_REPAY).ToString();//固定还款日
                            //bowr.STAT_DATE = parentBidInfo.BID_SIGNED_TIME == null ? "" : Convert.ToDateTime(parentBidInfo.BID_SIGNED_TIME).ToString("yyyy-MM-dd");
                            //bowr.END_DATE = parentBidInfo.BID_SIGNED_TIME == null ? "" : Convert.ToDateTime(parentBidInfo.BID_SIGNED_TIME).AddMonths(terms).ToString("yyyy-MM-dd");
                        }
                    }
                }
                else
                {
                    bowr = InitBowrDetail(bowr, calcContent, appContract, bid);
                }
                bowrDetail.BIZ_ENT = bowr;
                //出借人信息
                var lderInfo = new BIZ_ENTS<LDER_INFO>();
                var lderinfo = new LDER_INFO() { BAND_DEPT = "", BAND_DEPT_ACNT = "", BAND_DEPT_NAME = "", CERT_NO = "", CERT_TYPE = "", RESV_MOBL = "", USER_ADDR = "", USER_MAIL = "", USER_NAME = "", QF_USERID = "" };
                lderInfo.BIZ_ENT = lderinfo;
                //抵押物信息
                var cltrInfo = new BIZ_ENTS<CLTR_INFO>();
                var cltrinfo = new CLTR_INFO() { CAR_BRAD = "", CAR_COLR = "", CAR_ENGN_NO = "", CAR_ID_NO = "", CAR_MODL_NO = "", CAR_NO = "", CER_ID_NO = "", ASST_TYPE = "", ASST_VAL = "", MOR_RATE = "", HOUSE_MONEY = "" };
                decimal applyAmt = 0;//申请金额/到手金额
                if (appContract.LOAN_AMT.HasValue)
                {
                    applyAmt = Convert.ToDecimal(appContract.LOAN_AMT);
                }
                else
                {
                    applyAmt = appContract.APPLY_AMT.HasValue ? Convert.ToDecimal(appContract.APPLY_AMT) : 0;
                }
                cltrinfo.CAR_BRAD = appContract.CAR_BRAND == null ? "" : appContract.CAR_BRAND;
                cltrinfo.CAR_COLR = appContract.CAR_COLOR == null ? "" : appContract.CAR_COLOR;
                cltrinfo.CAR_ENGN_NO = appContract.ENGINE_NO == null ? "" : appContract.ENGINE_NO;
                cltrinfo.CAR_ID_NO = appContract.VIN_NO == null ? "" : appContract.VIN_NO;
                cltrinfo.CAR_MODL_NO = appContract.CAR_FACTORY == null ? "" : appContract.CAR_FACTORY;
                cltrinfo.CAR_NO = appContract.VEHICLE_NO == null ? "" : appContract.VEHICLE_NO;
                if (appContract.CAR_SELLINGPRICE.HasValue)
                {
                    cltrinfo.ASST_VAL = appContract.CAR_SELLINGPRICE.HasValue ? appContract.CAR_SELLINGPRICE.ToString() : "0.00";

                    if (appContract.CAR_SELLINGPRICE.ToInt32() != 0)
                    {
                        cltrinfo.MOR_RATE = string.Format("{0:N2}", applyAmt / appContract.CAR_SELLINGPRICE * 100);//车辆抵押率=到手金额/车辆开票价格
                    }
                }
                else if (appContract.ASSESSMENT_VALUE.HasValue)
                {
                    cltrinfo.ASST_VAL = appContract.ASSESSMENT_VALUE.HasValue ? appContract.ASSESSMENT_VALUE.ToString() : "0.00";
                    if (appContract.ASSESSMENT_VALUE.ToInt32() != 0)
                    {
                        cltrinfo.MOR_RATE = string.Format("{0:N2}", appContract.LOAN_AMT_OF_CONTRACT / appContract.ASSESSMENT_VALUE * 100);//房屋抵押率=合同金额/房屋价值
                    }
                }
                cltrinfo.HOUSE_MONEY = appContract.ASSESSMENT_VALUE.HasValue ? appContract.ASSESSMENT_VALUE.ToString() : "0.00";
                cltrInfo.BIZ_ENT = cltrinfo;
                //房产信息
                var tcltrHoseInfo = new BIZ_ENTS<List<TCLTRINFO>>();
                var tcltrhose = new List<TCLTRINFO>() { 
                 new TCLTRINFO(){
                     NUMBER="1", 
                     ADDR=appContract.HOUSE_ADDRESS == null ? "" :appContract.HOUSE_ADDRESS ,
                     MONEY=appContract.ASSESSMENT_VALUE == null ? "" :string.Format("{0:N}",appContract.ASSESSMENT_VALUE) , 
                     FAREA=appContract.COVERED_AREA == null ? "" : appContract.COVERED_AREA.ToString(), 
                     FPR="", 
                     OWNERSHIP=appContract.NAME == null ? "" :appContract.NAME , 
                     DES=appContract.HOUSE_TYPE_OTHER == null ? "" :appContract.HOUSE_TYPE_OTHER } 
                };
                tcltrHoseInfo.BIZ_ENT = tcltrhose;
                //担保信息
                var gartInfo = new BIZ_ENTS<GART_INFO>();
                var gartinfo = new GART_INFO() { ORG_CONT = "", FAX = "", REPR_NAME = "", RESV_MOBL = "", USER_ADDR = "", ZIDCODE = "", CERT_NO = "", CERT_TYPE = "", USER_MAIL = "", USER_NAME = "" };
                gartInfo.BIZ_ENT = gartinfo;
                //展期信息 
                var rlovInfo = new BIZ_ENTS<RLOV_INFO>();
                var rlov = new RLOV_INFO() { PREV_RATE_AMT = "", PRIC_DIFF_AMT = "", RLOV_CNST_FEE = "", RLOV_PMT_DUE_DATE = "", RLOV_PRIC_AMT = "", RLOV_RATE = "", RLOV_RATE_AMT = "", RLOV_TERM = "", ADDR = "", CITY = "", CONT_CODE = "", CONT_SIGN_PLCE = "", FUND_CHAN = "", PROV = "", REGN = "", RISK_COMP = "", SERV_FEE = "", TOTL_SERV_FEE = "", MONTH_RATE = "", RATE_AMT = "", NEED_AMT = "", PRIC_RATE = "", PMT_END_DATE = "", PMT_STAT_DATE = "", BEGN_INTR_DATE = "" };
                if (appExtend != null)//判断是否是展期
                {
                    rlov = InitRlovInfo(rlov, calcContent, appContract, parentAppInfo, bid);
                }
                rlovInfo.BIZ_ENT = rlov;
                //受托人信息
                var trusInfo = new BIZ_ENTS<TRUT_INFO>();
                var trusinfo = new TRUT_INFO() { CERT_NO = "", USER_ADDR = "", USER_NAME = "" };
                trusInfo.BIZ_ENT = trusinfo;
                //债权人信息
                var debtInfo = new BIZ_ENTS<DEBT_INFO>();
                var debtinfo = new DEBT_INFO() { CERT_NO = "", CERT_TYPE = "", FAX = "", RESV_MOBL = "", USER_ADDR = "", USER_MAIL = "", USER_NAME = "" };
                debtInfo.BIZ_ENT = debtinfo;
                //机构信息
                var tclInfo = new BIZ_ENTS<TC_INFO>();
                var tclinfo = new TC_INFO() { ORG_TYPE = "", ABBR_EN = "", ABBR_ZH = "", ACTV_DATE = "", BAND_DEPT = "", BAND_DEPT_ACNT = "", BAND_DEPT_NAME = "", CONT_NO = "", IS_ACTV = "", NAME_EN = "", NAME_ZH = "", ORG_ADDR = "", ORG_CODE = "", ORG_CONT = "", REGT_PLAC = "", REPR_NAME = "", RESV_MOBL = "" };
                var trustNo = GetChannelValue(contractGlobal.Contract_Channel_Trust, bid.BID_CHANNEL);
                if (trustNo != "")
                {
                    var OrgtrustInfo = labelPactService.GetP2Ptrust(trustNo);
                    if (OrgtrustInfo != null)
                    {
                        if (OrgtrustInfo.Count > 0)
                        {
                            string ACCOUNT_BRANCH_NAME = OrgtrustInfo[0].ACCOUNT_BRANCH_NAME == null ? "" : OrgtrustInfo[0].ACCOUNT_BRANCH_NAME;
                            tclinfo.BAND_DEPT = OrgtrustInfo[0].BANK_NAME == null ? "" + ACCOUNT_BRANCH_NAME : OrgtrustInfo[0].BANK_NAME + ACCOUNT_BRANCH_NAME;//信托开户行
                            tclinfo.BAND_DEPT_ACNT = OrgtrustInfo[0].ACCOUNT_NUMBER == null ? "" : OrgtrustInfo[0].ACCOUNT_NUMBER;//信托开户账号
                        }
                    }
                }
                tclInfo.BIZ_ENT = tclinfo;
                //保证人
                var indicator = new BIZ_ENTS<List<INDICATOR>>();
                var indicatorList = new List<INDICATOR>() { new INDICATOR() { CERT_NO = "", DATA = "", QIANMING = "", USER_NAME = "" } };
                indicator.BIZ_ENT = indicatorList;
                //共同借款人
                var g_bowr_info = new BIZ_ENTS<G_BOWR_INFO>();
                var g_bowr_infoList = new G_BOWR_INFO() { CERT_NO = "", DATA = "", QIANMING = "", USER_NAME = "" };
                g_bowr_info.BIZ_ENT = g_bowr_infoList;

                //附件出借人及还款信息
                var ptopmicar = new BIZ_ENTS<List<PTOPMICAR>>();
                var ptopmicarList = new List<PTOPMICAR>();
                PTOPMICAR ptopmi = new PTOPMICAR() { BAND_DEPT_ACNT = "", DEBT_NO = "", FIRT_PMT_AMT = "", MATC_AMT = "", MATC_PERC = "", PMT_STAT_DATE = "", PMT_TERM = "", RESI_PRIC = "", USER_NAME = "" };
                ptopmicarList.Add(ptopmi);
                ptopmicar.BIZ_ENT = ptopmicarList;
                //商户信息
                var storeinfo = new BIZ_ENTS<STORE_INFO>();
                var store = new STORE_INFO() { BAND_DEPT_NAME = "", BAND_DEPT = "", BAND_DEPT_ACNT = "", BAND_DEPT_SUB = "" };
                store.BAND_DEPT_NAME=bid.BID_MER_ACC_NAME==null?"":bid.BID_MER_ACC_NAME;
                store.BAND_DEPT_ACNT = bid.BID_MER_ACC_NO == null ? "" : bid.BID_MER_ACC_NO;
                store.BAND_DEPT = bid.BID_MER_ACC_BANK == null ? "" : bid.BID_MER_ACC_BANK;
                store.BAND_DEPT_SUB = bid.BID_MER_ACC_BANK_BRANCH == null ? "" + store.BAND_DEPT : bid.BID_MER_ACC_BANK_BRANCH;
                storeinfo.BIZ_ENT = store;
                // 03 请求合同生成接口    /*准备合同参数*/
                CA_INFO._CA_USER caUser = new CA_INFO._CA_USER();
                caUser.USER_NAME = bid.BID_CUSTOMER_NAME;
                caUser.NATIONAL_ID = bid.BID_CUSTOMER_ID_NO;
                caUser.MOBILE_NO = bid.BID_MOBILE_NO;
                caUser.EMAIL_ID = bid.BID_EMAIL_ID;

                CA_INFO caInfo = new CA_INFO()
                {
                    RA_CODE = contractGlobal.RA_CODE, /*RA_CODE：RA（Registration Authority， 数字证书注册机构）的代码，默认值FDD*/
                    CA_USER = caUser
                };

                BIZ_INFO bizInfo = new BIZ_INFO();


                bizInfo.BASE_INFO = new List<BIZ_KEY_VAL>() 
                { 
                    { new BIZ_KEY_VAL(){ BIZ_KEY=contractGlobal.Contract_BIZ_ID, BIZ_VAL=bid.BID_APP_CODE}}
                    ,
                    { new BIZ_KEY_VAL(){ BIZ_KEY=contractGlobal.Contract_ID, BIZ_VAL= bid.BID_CONTRACT_NO}}
                };
                //将合同相关实体对象添加到集合
                bizInfo.BIZ_ENTS = new List<Object>() { baseInfo, bowrInfo, g_bowr_info, indicator, lderInfo, debtInfo, cltrInfo, prodInfo, bowrDetail, trusInfo, tclInfo, tcltrHoseInfo, gartInfo, payment, ptopmicar, rlovInfo, storeinfo };
                var fundChannel = GetChannelValue(contractGlobal.Contract_FundChannel, bid.BID_CHANNEL);
                var productBrand = bid.BID_BUS_TYPE;
                //序列化合同生成实体
                conRequest.APP_ID = contractGlobal.Contract_APP_ID;
                conRequest.ACTION = contractGlobal.Contract_CONT_CREATE;
                conRequest.IS_SYNC = "Y";//  Y:同步、N是异步
                conRequest.NOTIFY_URL = "";
                conRequest.SIGN_TYPE = "";  //自动电子签章（E_AUTO）；手动电子签章（E_MANUAL）；手动签字并上传（P_MANUAL），生成合同信息时默认为空！
                conRequest.CA_INFO = caInfo;
                conRequest.BIZ_TEMP_MAP = new List<BIZ_KEY_VAL>() { 
                new BIZ_KEY_VAL() { BIZ_KEY = contractGlobal.Contract_BRAND_CODE, BIZ_VAL = productBrand }, //品牌合同模板的对应关系
                new BIZ_KEY_VAL() { BIZ_KEY = contractGlobal.Contract_CHANNEL_CODE, BIZ_VAL = fundChannel }//渠道合同模板的对应关系
            };
                conRequest.BIZ_INFO = bizInfo;
                return true;
            }
            catch (Exception ex)
            {
                Infrastructure.Log4Net.LogWriter.Error("合同生成准备参数异常" + bid.BID_CONTRACT_NO, ex, user);
                return false;
            }
        }
        public bool RunCreateCon(ContractCreateRequest conRequest, QB_V_BID_DETAIL bid, Global.ContractGlobalConfig contractGlobal, QFUser user, bool isCreate)
        {
            if (isCreate)
            {
                bool isconSucess = false;
                IRepositoryTransaction efrep = new EFRepositoryTransaction(GlobalSetting.MainDataBasenameOrConnectionString);
                var mainsvc = efrep.GetRepository<APP_MAIN>();
                try
                {
                    APP_MAIN appMain = mainsvc.FirstOrDefault(f => f.APP_CODE == bid.BID_APP_CODE);
                    string json = Serializer.ToJson(conRequest);
                    //请求合同生成接口
                    var bytes = System.Text.Encoding.UTF8.GetBytes(json);
                    ByteArrayContent content = new ByteArrayContent(bytes);
                    content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                    var handler = new HttpClientHandler() { AutomaticDecompression = DecompressionMethods.GZip };
                    HttpClient client = new HttpClient(handler);
                    Task<HttpResponseMessage> response = client.PostAsync(contractGlobal.ContractUrl, content);
                    var result = response.Result.Content.ReadAsStringAsync();
                    var contractMsg = JsonConvert.DeserializeObject<ContractCreateResponse>(result.Result);
                    Infrastructure.Log4Net.LogWriter.Biz("合同正在生成", bid.BID_CONTRACT_NO, user);
                    if (contractMsg.RESULT.ToUpper() == "SUCCESS" && contractMsg.CODE != "10007")//10007——合同文件未全部生成
                    {
                        appMain.CONTRACT_IS_CREATE = "Y";//更新app_main表合同是否全部生成
                        Infrastructure.Log4Net.LogWriter.Biz("合同生成成功", bid.BID_CONTRACT_NO, user);
                        isconSucess = true;
                    }
                    else
                    {
                        appMain.CONTRACT_IS_CREATE = null;//更新app_main表合同是否全部生成
                        Infrastructure.Log4Net.LogWriter.Biz("合同生成失败" + "CODE:" + contractMsg.CODE, bid.BID_CONTRACT_NO, user);
                        isconSucess = false;
                    }
                    mainsvc.Update(appMain);
                    efrep.Commit();
                }
                catch (Exception ex)
                {
                    Infrastructure.Log4Net.LogWriter.Error("合同生成异常" + bid.BID_CONTRACT_NO, ex, user);
                    isconSucess = false;
                }
                finally
                {
                    if (efrep != null)
                        efrep.Dispose();
                }
                return isconSucess;
            }
            else
                return false;
        }
        
        /// <summary>
        /// 获取创建失败合约列表
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        public PageData<QB_V_BIDLIST> GetFailContractList(Entity.ExtendEntity.BidContractSearchPara para)
        {
            RestApiHelper rest = new RestApiHelper(GlobalApi.QKGetFailContractList);
            return rest.Post<PageData<QB_V_BIDLIST>>(string.Empty, para);
        }

        /// <summary>
        /// 重新提交合约
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        public string RePost(Entity.ExtendEntity.ArrangementActivityItfDTO para)
        {
            RestApiHelper rest = new RestApiHelper(GlobalApi.QKDepositsArrangementUpdate);
            var result = rest.Post<LoanBaseResponse<List<string>>>(string.Empty, para);

            //TODO
            return "";
        }
    }
}

