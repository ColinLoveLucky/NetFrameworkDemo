using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;
using QK.QAPP.Entity;
using QK.QAPP.Global;
using QK.QAPP.Infrastructure;
using QK.QAPP.Infrastructure.MessageQueue;
using QK.QAPP.IServices;

namespace QK.QAPP.Services
{
    public partial class APP_QUANTSERVICE
    {
        public IAPP_QUANT_TEMPSERVICE QuantTempService { get; set; }

        public IQFUserService UserService { get; set; }

        public IAPP_MAINSERVICE MainService { get; set; }
        public IAPP_CUSTOMERSERVICE CustomerService { get; set; }
        public IAPP_BANKCARDSERVICE BankcardService { get; set; }
        public IAPP_LOANSERVICE LoanService { get; set; }
        public IAPP_QUANTSERVICE QuantService { get; set; }
        public ICR_DATA_DICService CrDataDicService { get; set; }
        public IAPP_BANKSERVICE BankService { get; set; }
        public IQFProductInfoService ProductService { get; set; }
        public IAPP_STAFF_ONLYSERVICE StaffOnlyService { get; set; }

        private List<CR_DATA_DIC> _provinceList;

        public List<CR_DATA_DIC> ProvinceList
        {
            get
            {
                if (_provinceList == null)
                {
                    _provinceList = CrDataDicService.GetDICByParentCode("AREA_CODE");
                    foreach (var p in _provinceList)
                    {
                        p.DATA_NAME = p.DATA_NAME.Trim();
                    }
                }
                return _provinceList;
            }
        }

        private List<QFProductInfo> _productList;

        public List<QFProductInfo> ProductList
        {
            get
            {
                return _productList ??
                       (_productList =
                           ProductService.GetProductListByLogo(PInfoInterfaceURLAccount.logo.ToString(),
                               "productCodeQuantGroup"));
            }
        }

        public QuantImportResult ImportData(System.Web.HttpRequestBase request)
        {
            var resultObj = new QuantImportResult();

            if (request.Files.Count == 0)
            {
                resultObj.Message = "抱歉，没有找到可上传的文件！";
            }
            else
            {
                var file = request.Files[0];
                var type = typeof(APP_QUANT_TEMP);
                var para = new ExcelImportPara()
                {
                    FileName = file.FileName,
                    Stream = file.InputStream,
                    IgnoreFirstLine = true,
                    Propertys = new PropertyInfo[]
                    {
                        type.GetProperty("APPLY_NUMBER"),
                        type.GetProperty("CONTRACT_ID"),
                        type.GetProperty("NAME"),
                        type.GetProperty("ID_NO"),
                        type.GetProperty("MOBILE"),
                        type.GetProperty("ADDRESS"),
                        type.GetProperty("BANK"),
                        type.GetProperty("BANK_CARD"),
                        type.GetProperty("BANK_PROVINCE"),
                        type.GetProperty("BANK_CITY"),
                        type.GetProperty("APPLY_AMT"),
                        type.GetProperty("TERMS"),
                        type.GetProperty("CREDIT_SCORE"),
                        type.GetProperty("REPAYMENT_PLAN"),
                        type.GetProperty("SUBMIT_DATE"),
                        type.GetProperty("LENDING_DATE"),
                        type.GetProperty("HAS_LENDING"),
                        type.GetProperty("FAIL_REASON")
                    }
                };

                try
                {
                    string msg;
                    var list = ExcelImporter.ReadListAllowEmptyCell<APP_QUANT_TEMP>(para, out msg);
                    resultObj.Message += msg;
                    resultObj.TotleCount = list.Count;
                    var downloadeStatus = QuantStatus.Downloaded.ToString();
                    foreach (var item in list)
                    {
                        item.HANDLE_STATUS = QuantStatus.UnProcessed.ToString();

                        var item1 = item;
                        var exist = QuantTempService
                            .FirstOrDefault(q => q.APPLY_NUMBER == item1.APPLY_NUMBER
                                && q.HANDLE_STATUS != downloadeStatus);
                        if (exist == null)
                        {
                            if (!string.IsNullOrEmpty(item1.APPLY_NUMBER))
                            {
                                item.CREATED_TIME = DateTime.Now;
                                item.CHANGED_TIME = DateTime.Now;
                                //insert
                                QuantTempService.Add(item1);
                                resultObj.InsertCount++;
                            }
                        }
                        else if (exist.HANDLE_STATUS == QuantStatus.UnProcessed.ToString()
                            || exist.HANDLE_STATUS == QuantStatus.Fail.ToString())
                        {
                            //update
                            SetNewData(exist, item1);
                            exist.CHANGED_TIME = DateTime.Now;
                            exist.HANDLE_STATUS = QuantStatus.UnProcessed.ToString();
                            QuantTempService.Update(exist);
                            resultObj.UpdateCount++;
                        }
                        else if (exist.HANDLE_STATUS == QuantStatus.Success.ToString())
                        {
                            //warn
                            resultObj.ExsitsList.Add(exist.APPLY_NUMBER);
                        }
                    }

                    QuantTempService.UnitOfWork.SaveChanges();

                    Infrastructure.Log4Net.LogWriter.Biz(string.Format("成功导入量化派数据{0}笔，其中insert{1}条，update{2}条，已存在{3}条",
                        resultObj.TotleCount, resultObj.InsertCount, resultObj.UpdateCount, resultObj.ExsitsList.Count), "QUANT", resultObj);
                }
                catch (Exception ex)
                {
                    //日志
                    resultObj.Message += "量化派信息导入出错！";
                    Infrastructure.Log4Net.LogWriter.Error(resultObj.Message, ex);
                }
            }

            return resultObj;
        }

        public QuantHandleResult HandleQuantData()
        {
            var resultObj = new QuantHandleResult();
            var unProcessStatus = QuantStatus.UnProcessed.ToString();
            var failStatus = QuantStatus.Fail.ToString();
            var unProcessList = new List<APP_QUANT_TEMP>();
            //获取所有未处理及失败的数据
            var query = QuantTempService.Find(q => q.HANDLE_STATUS == unProcessStatus || q.HANDLE_STATUS == failStatus);
            if (query.Any())
                unProcessList = query.ToList();
            resultObj.TotleCount = unProcessList.Count;
            //依次处理
            foreach (var item in unProcessList)
            {
                try
                {
                    //创建各业务实体
                    var msg = string.Empty;
                    var appMain = InitAppMain(item);
                    var appCustomer = InitAppCustomer(item, appMain, ref msg);
                    var appLoan = InitAppLoan(item, appMain, ref msg);
                    var appBankcard = InitAppBankcard(item, appMain, ref msg);
                    var appQuant = InitAppQuant(item, appMain, ref msg);
                    var appStaffOnly = InitStaffOnly(appMain, ref msg);

                    //修改处理状态
                    if (string.IsNullOrEmpty(msg))
                    {
                        item.HANDLE_STATUS = QuantStatus.Success.ToString();
                        MainService.Add(appMain);
                        CustomerService.Add(appCustomer);
                        LoanService.Add(appLoan);
                        BankcardService.Add(appBankcard);
                        QuantService.Add(appQuant);
                        StaffOnlyService.Add(appStaffOnly);
                        QuantTempService.Update(item);
                        //提交事务
                        MainService.UnitOfWork.SaveChanges();
                        //发送MQ
                        if (!PublishMQ(appMain))
                        {
                            msg = "【数据提交成功，但MQ消息发送失败】";
                            resultObj.ResultDic.Add(appMain.APP_CODE, msg);
                        }
                        resultObj.SuccessCount++;
                    }
                    else
                    {
                        item.HANDLE_STATUS = QuantStatus.Fail.ToString();
                        QuantTempService.Update(item);
                        QuantTempService.UnitOfWork.SaveChanges();
                        resultObj.ResultDic.Add(item.APPLY_NUMBER, msg);
                        resultObj.FailCount++;
                    }
                }
                catch (Exception ex)
                {
                    Infrastructure.Log4Net.LogWriter.Error("量化派处理数据时发生异常:ID=" + item.ID, ex);
                }
            }

            return resultObj;
        }

        public QuantHandleResult HandleInfo()
        {
            var resultObj = new QuantHandleResult();
            var unProcessStatus = QuantStatus.UnProcessed.ToString();
            var failStatus = QuantStatus.Fail.ToString();

            resultObj.UnProcess = QuantTempService.Count(q => q.HANDLE_STATUS == unProcessStatus);
            resultObj.FailCount = QuantTempService.Count(q => q.HANDLE_STATUS == failStatus);

            return resultObj;
        }

        private void SetNewData(APP_QUANT_TEMP oldEntity, APP_QUANT_TEMP newEntity)
        {
            oldEntity.CONTRACT_ID = newEntity.CONTRACT_ID;
            oldEntity.NAME = newEntity.NAME;
            oldEntity.ID_NO = newEntity.ID_NO;
            oldEntity.MOBILE = newEntity.MOBILE;
            oldEntity.ADDRESS = newEntity.ADDRESS;
            oldEntity.BANK = newEntity.BANK;
            oldEntity.BANK_CARD = newEntity.BANK_CARD;
            oldEntity.BANK_PROVINCE = newEntity.BANK_PROVINCE;
            oldEntity.BANK_CITY = newEntity.BANK_CITY;
            oldEntity.APPLY_AMT = newEntity.APPLY_AMT;
            oldEntity.TERMS = newEntity.TERMS;
            oldEntity.CREDIT_SCORE = newEntity.CREDIT_SCORE;
            oldEntity.REPAYMENT_PLAN = newEntity.REPAYMENT_PLAN;
            oldEntity.SUBMIT_DATE = newEntity.SUBMIT_DATE;
            oldEntity.LENDING_DATE = newEntity.LENDING_DATE;
            oldEntity.HAS_LENDING = newEntity.HAS_LENDING;
            oldEntity.FAIL_REASON = newEntity.FAIL_REASON;
        }

        private APP_MAIN InitAppMain(APP_QUANT_TEMP quantTemp)
        {
            var currentUser = UserService.GetCurrentUser().Account;
            var appMain = new APP_MAIN(true);
            appMain.APP_CODE = quantTemp.APPLY_NUMBER;
            appMain.APP_STATUS = EnterStatusType.SUBMIT.ToString();
            appMain.CREATED_TIME = DateTime.Now;
            appMain.CREATED_USER = currentUser;
            appMain.CHANGED_TIME = DateTime.Now;
            appMain.CHANGED_USER = currentUser;

            return appMain;
        }

        private APP_CUSTOMER InitAppCustomer(APP_QUANT_TEMP quantTemp, APP_MAIN appMain, ref string resultMsg)
        {
            var appCustomer = new APP_CUSTOMER();
            appCustomer.APP_ID = appMain.ID;
            appCustomer.NAME = quantTemp.NAME;
            appCustomer.ID_NO = quantTemp.ID_NO;
            appCustomer.ID_TYPE = "Id1";
            appCustomer.MOBILE1 = quantTemp.MOBILE;
            if (!MatchAddress(appCustomer, quantTemp.ADDRESS))
                resultMsg += "【居住地址未匹配成功】";
            return appCustomer;
        }

        private APP_LOAN InitAppLoan(APP_QUANT_TEMP quantTemp, APP_MAIN appMain, ref string resultMsg)
        {
            resultMsg += String.Empty;
            var appLoan = new APP_LOAN();
            appLoan.APP_ID = appMain.ID;
            appLoan.LOAN_AMT_OF_CONTRACT = ConvertToDecimal(quantTemp.APPLY_AMT);
            if (appLoan.LOAN_AMT_OF_CONTRACT == null)
                resultMsg += "【转换贷款金额时发生异常】";
            appLoan.TERMS = ConvertToShort(quantTemp.TERMS);
            if (appLoan.TERMS == null)
                resultMsg += "【转换贷款期数时发生异常】";
            foreach (var p in ProductList)
            {
                if (p.pProduct.termStart == appLoan.TERMS)
                {
                    //产品信息
                    appMain.LOGO = p.pLogo.logo;
                    appMain.PRODUCT_CODE = p.pProduct.productCode;
                    appMain.PRODUCT_NAME = p.pProduct.productName;
                    appMain.PROD_VERSION = p.pProduct.prodVersion;

                    //相关费率
                    //咨询费
                    appLoan.CONSULTATION_CHARGE_RATIO = p.pProduct.consultationChargeRatio;
                    appLoan.CONSULTATION_CHARGE_AMT = appLoan.LOAN_AMT_OF_CONTRACT * appLoan.CONSULTATION_CHARGE_RATIO;
                    //罚息
                    appLoan.DEFAULT_INTEREST_RATIO = p.pInterest.defaultInterestRatio;
                    //服务费
                    appLoan.SERVICE_CHARGE_RATIO = p.pInterest.serviceChargeRatio;
                    appLoan.SERVICE_CHARGE_AMT = appLoan.LOAN_AMT_OF_CONTRACT * appLoan.SERVICE_CHARGE_RATIO;
                    //利率
                    appLoan.RATE = p.pInterest.rate;
                    //利率类型
                    appLoan.RATE_TYPE = p.pInterest.rateType;
                    //风险准备金
                    appLoan.RESERVES_RATIO = p.pProduct.reservesRatio;
                    appLoan.RESERVES_AMT = appLoan.LOAN_AMT_OF_CONTRACT * appLoan.RESERVES_RATIO;
                    //到手金额 = 合同金额 - 咨询费
                    appLoan.LOAN_AMT = appLoan.LOAN_AMT_OF_CONTRACT - appLoan.CONSULTATION_CHARGE_AMT;
                    appLoan.APPLY_AMT = appLoan.LOAN_AMT;

                    break;
                }
            }
            if (string.IsNullOrEmpty(appMain.LOGO)
                || string.IsNullOrEmpty(appMain.PRODUCT_CODE)
                || string.IsNullOrEmpty(appMain.PRODUCT_NAME))
                resultMsg += "【量化派产品和期数未匹配成功】";

            return appLoan;
        }

        private APP_BANKCARD InitAppBankcard(APP_QUANT_TEMP quantTemp, APP_MAIN appMain, ref string resultMsg)
        {
            var appBankcard = new APP_BANKCARD();
            appBankcard.APP_ID = appMain.ID;
            appBankcard.BANK_ACCOUNT = quantTemp.BANK_CARD;
            if (!MatchBank(appBankcard, quantTemp.BANK))
                resultMsg += "【发卡行未匹配成功】";
            if (!MatchBankAddress(appBankcard, quantTemp))
                resultMsg += "【发卡省或发卡市未匹配成功】";
            return appBankcard;
        }

        private APP_QUANT InitAppQuant(APP_QUANT_TEMP quantTemp, APP_MAIN appMain, ref string resultMsg)
        {
            resultMsg += String.Empty;
            var appQuant = new APP_QUANT();
            appQuant.APP_ID = appMain.ID;
            appQuant.CONTRACT_ID = quantTemp.CONTRACT_ID;
            appQuant.CREDIT_SCORE = ConvertToDecimal(quantTemp.CREDIT_SCORE);
            if (appQuant.CREDIT_SCORE == null)
                resultMsg += "【转换信用分时发生异常】";
            appQuant.REPAYMENT_PLAN = quantTemp.REPAYMENT_PLAN;
            appQuant.SUBMIT_DATE = ConvertToDateTime(quantTemp.SUBMIT_DATE);
            if (appQuant.SUBMIT_DATE == null)
                resultMsg += "【转换提交日期时发生异常】";
            appQuant.LENDING_DATE = ConvertToDateTime(quantTemp.LENDING_DATE);
            appQuant.HAS_LENDING = quantTemp.HAS_LENDING;
            appQuant.FAIL_REASON = quantTemp.FAIL_REASON;
            appQuant.CREATED_TIME = DateTime.Now;
            appQuant.CHANGED_TIME = DateTime.Now;

            return appQuant;
        }

        private APP_STAFF_ONLY InitStaffOnly(APP_MAIN appMain, ref string resultMsg)
        {
            resultMsg += String.Empty;
            var staffOnly = new APP_STAFF_ONLY();
            staffOnly.APP_ID = appMain.ID;
            staffOnly.CHANNEL_CODE = "000000000";
            staffOnly.CHANNEL_NAME = "直销进件";
            return staffOnly;
        }

        private bool PublishMQ(APP_MAIN appMain)
        {
            //using (MQProducer NR_Producer = new MQProducer(GlobalSetting.MQMultipleServer,
            //    GlobalSetting.MQUserName,
            //    GlobalSetting.MQUserPassword,
            //    GlobalSetting.MQApplication_PEN))
            //{
            //    if (NR_Producer.Publish(appMain.APP_CODE))
            //    {
            //        Infrastructure.Log4Net.LogWriter.Biz("量化派-MQ用户提交数据成功", appMain.ID + "", appMain);
            //        return true;
            //    }
            //}
            if (MQHelper.Publish(
                GlobalSetting.MQMultipleServer,
                GlobalSetting.MQUserName,
                GlobalSetting.MQUserPassword,
                GlobalSetting.MQApplication_PEN,
                appMain.APP_CODE))
            {
                Infrastructure.Log4Net.LogWriter.Biz("量化派-MQ用户提交数据成功", appMain.ID + "", appMain);
                return true;
            }

            Infrastructure.Log4Net.LogWriter.Error("量化派-MQ消息发送失败");
            return false;
        }

        private DateTime? ConvertToDateTime(string dateStr)
        {
            DateTime date;
            if (DateTime.TryParse(dateStr, out date))
                return date;
            return null;
        }

        private decimal? ConvertToDecimal(string decimalStr)
        {
            decimal dec;
            if (decimal.TryParse(decimalStr, out dec))
                return dec;
            return null;
        }

        private short? ConvertToShort(string shortStr)
        {
            short s;
            if (short.TryParse(shortStr, out s))
                return s;
            return null;
        }

        private bool MatchAddress(APP_CUSTOMER appCustomer, string addressStr)
        {
            appCustomer.RESIDENT_PROVINCE = String.Empty;
            appCustomer.RESIDENT_CITY = String.Empty;
            appCustomer.RESIDENT_ADDRESS = String.Empty;
            //匹配省份
            foreach (var p in ProvinceList)
            {
                if (addressStr.Contains(p.DATA_NAME))
                {
                    appCustomer.RESIDENT_PROVINCE = p.DATA_CODE;
                    //匹配市区
                    var cityList = CrDataDicService.GetDICByParentCode(appCustomer.RESIDENT_PROVINCE);
                    foreach (var c in cityList)
                    {
                        c.DATA_NAME = c.DATA_NAME.Trim();
                        if (addressStr.Contains(c.DATA_NAME))
                        {
                            appCustomer.RESIDENT_CITY = c.DATA_CODE;
                            //详细地址
                            appCustomer.RESIDENT_ADDRESS = addressStr.Replace(p.DATA_NAME, string.Empty).Replace(c.DATA_NAME, string.Empty);
                            break;
                        }
                    }
                    break;
                }
            }
            if (string.IsNullOrEmpty(appCustomer.RESIDENT_PROVINCE) || string.IsNullOrEmpty(appCustomer.RESIDENT_CITY))
                return false;
            return true;
        }

        private bool MatchBank(APP_BANKCARD appBankcard, string bankStr)
        {
            if (string.IsNullOrEmpty(bankStr))
                return false;
            appBankcard.BANK_CODE = String.Empty;
            appBankcard.BANK_NAME = String.Empty;
            var bankList = BankService.GetBankList();
            foreach (var b in bankList)
            {
                if (b.BANK_REMARK.Contains(bankStr) || b.BANK_NAME.Contains(bankStr) ||
                    bankStr.Contains(b.BANK_NAME) || bankStr.Contains(b.BANK_REMARK))
                {
                    appBankcard.BANK_CODE = b.BANK_TYPE;
                    appBankcard.BANK_NAME = b.BANK_NAME;
                }
            }

            if (string.IsNullOrEmpty(appBankcard.BANK_CODE) || string.IsNullOrEmpty(appBankcard.BANK_NAME))
                return false;
            return true;
        }

        private bool MatchBankAddress(APP_BANKCARD appBankcard, APP_QUANT_TEMP quantTemp)
        {
            if (string.IsNullOrEmpty(quantTemp.BANK_PROVINCE) || string.IsNullOrEmpty(quantTemp.BANK_CITY))
                return false;
            appBankcard.BANK_PROVINCE = String.Empty;
            appBankcard.BANK_CITY = String.Empty;
            //发卡省
            foreach (var p in ProvinceList)
            {
                if (p.DATA_NAME.Contains(quantTemp.BANK_PROVINCE) || quantTemp.BANK_PROVINCE.Contains(p.DATA_NAME))
                {
                    appBankcard.BANK_PROVINCE = p.DATA_CODE;
                    //发卡市
                    var cityList = CrDataDicService.GetDICByParentCode(appBankcard.BANK_PROVINCE);
                    foreach (var c in cityList)
                    {
                        c.DATA_NAME = c.DATA_NAME.Trim();
                        if (c.DATA_NAME.Contains(quantTemp.BANK_CITY) || quantTemp.BANK_CITY.Contains(c.DATA_NAME))
                        {
                            appBankcard.BANK_CITY = c.DATA_CODE;
                            break;
                        }
                    }
                    break;
                }
            }

            if (string.IsNullOrEmpty(appBankcard.BANK_PROVINCE) || string.IsNullOrEmpty(appBankcard.BANK_CITY))
                return false;
            return true;
        }
    }
}
