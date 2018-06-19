using System.Diagnostics;
using Microsoft.Practices.Unity;
using QK.QAPP.Entity;
using QK.QAPP.Global;
using QK.QAPP.Infrastructure.Log4Net;
using QK.QAPP.Infrastructure.MessageQueue;
using QK.QAPP.IServices;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QK.QAPP.Infrastructure;
using System.Web.Mvc;
using Newtonsoft.Json;

namespace QK.QAPP.Services
{
    public class ApplicationService : IApplicationService
    {

        #region 属性注入
        [Dependency]
        public IAPP_CONTACTSERVICE ContactService { get; set; }

        [Dependency]
        public IAPP_CUSTOMERSERVICE CustomerService { get; set; }

        [Dependency]
        public IAPP_JOBSERVICE JobService { get; set; }

        [Dependency]
        public IAPP_BANKCARDSERVICE BankCardService { get; set; }

        [Dependency]
        public IAPP_STAFF_ONLYSERVICE StaffonlyService { get; set; }

        [Dependency]
        public ICR_DATA_DICService DataDicService { get; set; }

        [Dependency]
        public IAPP_MAINSERVICE MainService { get; set; }
        [Dependency]
        public IApplyTableService ApplyTableService { get; set; }
        [Dependency]
        public IQFUserService UserService { get; set; }
        [Dependency]
        public ICR_DATA_DICService DataService { get; set; }
        [Dependency]
        public IAPP_CARINFOSERVICE CarInfoService { get; set; }
        [Dependency]
        public IAPP_MSGBOXSERVICE MsgBoxService { get; set; }
        [Dependency]
        public IAPP_FORBIDEN_AREASERVICE ForbidenService { get; set; }
        [Dependency]
        public IAPP_HOUSESERVICE HouseService { get; set; }
        #endregion

        #region 接口实现
        #region 读取数据
        public APP_MAIN GetAPPMain(long id)
        {
            return MainService.FirstOrDefault(c => c.ID == id);
        }
        public List<APP_CONTACT> GetContacts(long APP_MAIN_Id)
        {
            var retList = new List<APP_CONTACT>();
            var con_prop = DataService.GetDICByParentCode("CONTACT_PROPERTY");
            if (con_prop != null)
            {
                foreach (var item in con_prop)
                {
                    var obj = ContactService.FirstOrDefault(c => c.APP_ID == APP_MAIN_Id && c.CONTACT_PROPERTY == item.DATA_CODE);
                    if (obj == null)
                    {
                        obj = new APP_CONTACT { APP_ID = APP_MAIN_Id, CONTACT_PROPERTY = item.DATA_CODE };
                    }
                    retList.Add(obj);
                }
            }

            return retList;
        }

        public APP_CUSTOMER GetUserBasic(long appid)
        {
            var obj = CustomerService.FirstOrDefault(c => c.APP_ID == appid);
            if (obj == null)
            {
                obj = new APP_CUSTOMER { APP_ID = appid };
            }
            return obj;
        }

        public APP_JOB GetUserJob(long appid)
        {
            var obj = JobService.FirstOrDefault(c => c.APP_ID == appid);
            if (obj == null)
            {
                obj = new APP_JOB { APP_ID = appid };
            }
            return obj;
        }

        public APP_BANKCARD GetCustomerBankCard(long appid)
        {
            var obj = BankCardService.FirstOrDefault(c => c.APP_ID == appid);
            if (obj == null)
            {
                obj = new APP_BANKCARD { APP_ID = appid };
                //保存银行卡值字段
            }
            return obj;
        }

        public APP_STAFF_ONLY GetStaffOnly(long appid)
        {
            var obj = StaffonlyService.FirstOrDefault(c => c.APP_ID == appid);
            if (obj == null)
            {
                obj = new APP_STAFF_ONLY { APP_ID = appid };
            }
            return obj;
        }

        public APP_HOUSE GetHouse(long appid)
        {
            var obj = HouseService.FirstOrDefault(h => h.APP_ID == appid);
            if (obj == null)
            {
                obj = new APP_HOUSE { APP_ID = appid };
            }
            return obj;
        }

        public List<APP_CONTACT> GetObligees(long appid)
        {
            var retList = new List<APP_CONTACT>();
            var con_prop = DataService.GetDICByParentCode("OBLIGEE_PROPERTY");
            if (con_prop != null)
            {
                foreach (var item in con_prop)
                {
                    var obj = ContactService.FirstOrDefault(c => c.APP_ID == appid && c.CONTACT_PROPERTY == item.DATA_CODE);
                    if (obj == null)
                    {
                        obj = new APP_CONTACT { APP_ID = appid, CONTACT_PROPERTY = item.DATA_CODE };
                    }
                    retList.Add(obj);
                }
            }

            return retList;
        }
        #endregion
        #region 写入数据

        public void EditContacts(APP_CONTACT entity)
        {
            //如果电话号码中包含下划线，则替换为空
            entity.MOBILE = HandleMobile(entity.MOBILE);
            //先根据APP_ID取ID，保证和数据库中ID一致
            var thisEntityId =
                ContactService.Find(
                    c => c.APP_ID == entity.APP_ID && c.CONTACT_PROPERTY == entity.CONTACT_PROPERTY).Select(c => c.ID).FirstOrDefault();
            entity.CON_ID_TYPE = "Id1";
            if (thisEntityId != 0)
            {
                entity.ID = thisEntityId;
                ContactService.Update(entity);
                Infrastructure.Log4Net.LogWriter.Biz("更新CONTACTS", entity.APP_ID + "", entity);
            }
            else
            {
                ContactService.Add(entity);
                Infrastructure.Log4Net.LogWriter.Biz("新增CONTACTS", entity.APP_ID + "", entity);
            }
            ContactService.UnitOfWork.SaveChanges();
        }

        public void UpdateOrAddCustomer(APP_CUSTOMER entity)    //废弃方法
        {
            //先根据APP_ID取ID，保证和数据库中ID一致
            var thisEntityId = CustomerService.Find(c => c.APP_ID == entity.APP_ID).Select(c => c.ID).FirstOrDefault();
            if (thisEntityId != 0)
            {
                entity.ID = thisEntityId;
                CustomerService.Update(entity);
                Infrastructure.Log4Net.LogWriter.Biz("更新CUSTOMER", entity.APP_ID + "", entity);
            }
            else
            {
                CustomerService.Add(entity);
                Infrastructure.Log4Net.LogWriter.Biz("新增CUSTOMER", entity.APP_ID + "", entity);
            }

            CustomerService.UnitOfWork.SaveChanges();
        }

        public string SaveCustomerBasic(APP_CUSTOMER customer, APP_JOB job)
        {
            customer.ID_TYPE = "Id1";
            if (customer.BRITHDAY.HasValue)
            {
                //计算周岁
                var now = DateTime.Now;
                var bir = customer.BRITHDAY.Value;
                var age = now.Year - bir.Year;
                if (now.Month < bir.Month || (now.Month == bir.Month && now.Day < bir.Day))
                    age--;
                customer.AGE = age.ToInt16();

            }
            //如果电话号码出现下滑线的情况，将其替换为空
            customer.MOBILE1 = HandleMobile(customer.MOBILE1);
            customer.MOBILE2 = HandleMobile(customer.MOBILE2);
            customer.RESIDENT_TEL_NO = HandleMobile(customer.RESIDENT_TEL_NO);
            job.COM_TEL_NO = HandleMobile(job.COM_TEL_NO);

            //先根据APP_ID取ID，保证和数据库中ID一致
            var thisCustomerId = CustomerService.Find(c => c.APP_ID == customer.APP_ID).Select(c => c.ID).FirstOrDefault();
            if (thisCustomerId != 0)
            {
                customer.ID = thisCustomerId;
                CustomerService.Update(customer);
                Infrastructure.Log4Net.LogWriter.Biz("更新CUSTOMER", customer.APP_ID + "", customer);
            }
            else
            {
                CustomerService.Add(customer);
                Infrastructure.Log4Net.LogWriter.Biz("新增CUSTOMER", customer.APP_ID + "", customer);
            }
            //先根据APP_ID取ID，保证和数据库中ID一致
            var thisJobId = JobService.Find(c => c.APP_ID == job.APP_ID).Select(c => c.ID).FirstOrDefault();
            if (thisJobId != 0)
            {
                job.ID = thisJobId;
                JobService.Update(job);
                Infrastructure.Log4Net.LogWriter.Biz("更新Job", job.APP_ID + "", job);

            }
            else
            {
                JobService.Add(job);
                Infrastructure.Log4Net.LogWriter.Biz("添加Job", job.APP_ID + "", job);
            }
            CustomerService.UnitOfWork.SaveChanges();

            return "";
        }

        public string SaveCustomerBankCard(APP_BANKCARD bankCard)
        {
            string result = string.Empty;
            //处理电话中可能存在的下划线
            bankCard.BANK_MOBILE = HandleMobile(bankCard.BANK_MOBILE);

            #region 调用接口验证银行卡号
            if (GlobalSetting.IsCFCA)
            {
                result = CheckBankcard(bankCard);
                //#if DEBUG
                //result = "";
                //#endif
            }
            #endregion
            //写入银行名字
            if (bankCard.BANK_CODE != null)
            {
                var bank =
               DataDicService.SqlQuery<CR_DATA_DIC>
               ("SELECT BANK_TYPE as DATA_CODE,BANK_NAME as DATA_NAME FROM APP_BANK WHERE BANK_TYPE=:banktype", bankCard.BANK_CODE).FirstOrDefault();

                if (bank != null)
                {
                    bankCard.BANK_NAME = bank.DATA_NAME;
                }
            }
            //先根据APP_ID取ID，保证和数据库中ID一致
            var thisBankCardId = BankCardService.Find(c => c.APP_ID == bankCard.APP_ID).Select(c => c.ID).FirstOrDefault();
            if (thisBankCardId != 0)
            {
                bankCard.ID = thisBankCardId;
                BankCardService.Update(bankCard);
                Infrastructure.Log4Net.LogWriter.Biz("更新银行信息", bankCard.APP_ID + "", bankCard);
            }
            else
            {
                BankCardService.Add(bankCard);
                Infrastructure.Log4Net.LogWriter.Biz("新增银行信息", bankCard.APP_ID + "", bankCard);
            }
            BankCardService.UnitOfWork.SaveChanges();
            return result;
        }

        private string CheckBankcard(APP_BANKCARD bankCard)
        {
            string result = string.Empty;
            string accountName, identificationNumber, bankID, accountNumber, appCode, mobile, appId, cardType, verifyChannel;
            var appMainEntity = GetAPPMain(bankCard.APP_ID);
            var customerEntity = CustomerService.FirstOrDefault(c => c.APP_ID == bankCard.APP_ID);
            if (customerEntity != null)
            {
                accountName = customerEntity.NAME;
                identificationNumber = customerEntity.ID_NO;
                bankID = bankCard.BANK_CODE;
                accountNumber = bankCard.BANK_ACCOUNT;
                appCode = appMainEntity.APP_CODE;
                mobile = bankCard.BANK_MOBILE;
                appId = GlobalSetting.QappAppId;
                cardType = GlobalSetting.BankCardType;
                verifyChannel = GlobalSetting.VerifyChannel;
                if (!string.IsNullOrEmpty(accountName)
                    && !string.IsNullOrEmpty(identificationNumber)
                    && !string.IsNullOrEmpty(bankID)
                    && !string.IsNullOrEmpty(accountNumber)
                    && !string.IsNullOrEmpty(mobile))
                {
                    //TODO 验证 如果失败 直接RETURN ERR
                    var paras = new Dictionary<string, string>
                        {
                            {"AccountName", accountName},
                            {"BankID", bankID},
                            {"AccountNumber", accountNumber},
                            {"CardType", cardType},
                            {"AppId", appId},
                            {"IdentificationType", "ID_CARD"},
                            {"IdentificationNumber", identificationNumber},
                            {"Mobile", mobile},
                            {"AppCode", appCode},
                            {"VerifyChannel", verifyChannel}
                        };
                    Stopwatch sw = new Stopwatch();
                    sw.Start();
                    var restClient = new RestHelper(GlobalSetting.CFCAPaymentURL);
                    var returnResult = restClient.Get<string>(string.Empty, paras);
                    sw.Stop();

                    LogWriter.Biz("调用验证接口服务完成", bankCard.APP_ID + string.Empty, paras);
                    
                    if (returnResult != null && returnResult.Status == DtoMessageStatus.Success)
                    {
                        var retuanObj = JsonConvert.DeserializeObject<CFCAPaymentResult>(returnResult.ReturnObj);
                        LogWriter.Biz("验证接口调用成功",bankCard.APP_ID + string.Empty, retuanObj);
                        if (!string.IsNullOrEmpty(retuanObj.ResultMessage) && retuanObj.Status!=1)  //如果接口存在返回的错误信息，就直接返回接口的提示信息
                        {
                            result = retuanObj.ResultMessage;
                        }
                        else   //否则根据状态返回提示信息
                        {
                            switch (retuanObj.Status)
                            {
                                case 0:
                                    result ="银行卡验证参数错误，请联系管理员！";
                                    break;
                                case 1:
                                    result = string.Empty;
                                    break;
                                case 2:
                                    result = "您填写的姓名，身份证号，银行预留手机和银行账户不匹配！";
                                    break;
                                case 3:
                                    result = "信息正在验证中，请稍后再试！";
                                    break;
                                default:
                                    result = "银行卡验证接口返回未知结果，请联系管理员！";
                                    break;
                            }
                        }
                        //result = returnResult.ReturnObj != "1" ? "您填写的姓名，身份证号，银行预留手机和银行账户不匹配！" : "";
                    }
                    else
                    {
                        result = "银行验证接口服务出错，请联系管理员！";
                        LogWriter.Biz("银行验证接口服务出错", bankCard.APP_ID + String.Empty,
                            new Dictionary<string, object>
                                    {
                                        {"接口耗时", sw.ElapsedMilliseconds},
                                        {"接口返回数据", returnResult}
                                    });
                    }
                }
                else
                {
                    result = "您填写的姓名、身份证号、银行预留手机或者银行卡信息不完整!";
                    LogWriter.Biz("您填写的姓名、身份证号、银行预留手机或者银行卡信息不完整!", bankCard.APP_ID + String.Empty);
                }
            }
            else
            {
                result = "未找到对应客户的记录!";
                LogWriter.Biz("未调用银行验证接口，因没有找到客户信息", bankCard.APP_ID + String.Empty);
            }

            return result;
        }

        public string SaveBankCardNoCode(APP_BANKCARD bankCard)
        {
            string result = string.Empty;
            //先根据APP_ID取ID，保证和数据库中ID一致
            var thisBankCardId = BankCardService.Find(c => c.APP_ID == bankCard.APP_ID).Select(c => c.ID).FirstOrDefault();
            if (thisBankCardId != 0)
            {
                bankCard.ID = thisBankCardId;
                BankCardService.Update(bankCard);
                Infrastructure.Log4Net.LogWriter.Biz("更新银行信息", bankCard.APP_ID + "", bankCard);
            }
            else
            {
                BankCardService.Add(bankCard);
                Infrastructure.Log4Net.LogWriter.Biz("新增银行信息", bankCard.APP_ID + "", bankCard);
            }
            BankCardService.UnitOfWork.SaveChanges();
            return result;
        }

        public string SaveStaffOnly(APP_STAFF_ONLY so)
        {
            //添加合作渠道的name
            if (so != null && !string.IsNullOrEmpty(so.CHANNEL_CODE))
            {
                so.CHANNEL_NAME = DataDicService.GetDICNameByCode(so.CHANNEL_CODE);

            }
            //处理电话号码下划线问题
            so.RECOMMEND_PERSON_TEL = HandleMobile(so.RECOMMEND_PERSON_TEL);

            //先根据APP_ID取ID，保证和数据库中ID一致
            var thisSoId = StaffonlyService.Find(c => c.APP_ID == so.APP_ID).Select(c => c.ID).FirstOrDefault();
            if (thisSoId != 0)
            {
                so.ID = thisSoId;
                StaffonlyService.Update(so);
                Infrastructure.Log4Net.LogWriter.Biz("更新员工专用栏", so.APP_ID + "", so);

            }
            else
            {
                StaffonlyService.Add(so);
                Infrastructure.Log4Net.LogWriter.Biz("添加员工专用栏", so.APP_ID + "", so);
            }
            StaffonlyService.UnitOfWork.SaveChanges();
            return "";
        }

        public string SaveStaffOnly(APP_STAFF_ONLY so, APP_MAIN main)
        {
            //先根据ID取ID，保证和数据库中ID一致
            var thisMainId = MainService.Find(m => m.ID == main.ID).Select(c => c.ID).FirstOrDefault();
            if (thisMainId != 0)
            {
                main.ID = thisMainId;
                MainService.Update(main);
                Infrastructure.Log4Net.LogWriter.Biz("更新APP_MAIN(征信渠道)", so.APP_ID + "", so);
            }
            else
            {
                MainService.Add(main);
                Infrastructure.Log4Net.LogWriter.Biz("添加APP_MAIN（征信渠道）", so.APP_ID + "", so);
            }
            //添加合作渠道的name
            if (so != null && !string.IsNullOrEmpty(so.CHANNEL_CODE))
            {
                so.CHANNEL_NAME = DataDicService.GetDICNameByCode(so.CHANNEL_CODE);
            }

            //处理电话号码下划线问题
            so.RECOMMEND_PERSON_TEL = HandleMobile(so.RECOMMEND_PERSON_TEL);

            //先根据APP_ID取ID，保证和数据库中ID一致
            var thisSoId = StaffonlyService.Find(c => c.APP_ID == so.APP_ID).Select(c => c.ID).FirstOrDefault();
            if (thisSoId != 0)
            {
                so.ID = thisSoId;
                StaffonlyService.Update(so);
                Infrastructure.Log4Net.LogWriter.Biz("更新员工专用栏", so.APP_ID + "", so);

            }
            else
            {
                StaffonlyService.Add(so);
                Infrastructure.Log4Net.LogWriter.Biz("添加员工专用栏", so.APP_ID + "", so);
            }
            StaffonlyService.UnitOfWork.SaveChanges();
            return "";
        }

        public APP_CARINFO GetCarInfo(long appid)
        {
            var obj = CarInfoService.FirstOrDefault(c => c.APP_ID == appid);
            if (obj == null)
            {
                obj = new APP_CARINFO { APP_ID = appid };
            }
            return obj;
        }

        public string SaveCarInfo(APP_CARINFO carInfo)
        {
            //先根据APP_ID取对象，保证ID为数据库中ID
            var thisCarInfoId = CarInfoService.Find(c => c.APP_ID == carInfo.APP_ID).Select(c => c.ID).FirstOrDefault();
            //如果没有值则默认为从信息库中选择
            if (string.IsNullOrEmpty(carInfo.CARINFO_TYPE))
            {
                carInfo.CARINFO_TYPE = "CarInfoType_Library";
            }
            if (thisCarInfoId != 0)
            {
                carInfo.ID = thisCarInfoId;
                CarInfoService.Update(carInfo);
                Infrastructure.Log4Net.LogWriter.Biz("更新车辆信息", carInfo.APP_ID + "", carInfo);
            }
            else
            {
                CarInfoService.Add(carInfo);
                Infrastructure.Log4Net.LogWriter.Biz("添加车辆信息", carInfo.APP_ID + "", carInfo);
            }
            CarInfoService.UnitOfWork.SaveChanges();
            return "";
        }

        public string SaveHouse(APP_HOUSE appHouse)
        {
            var thisHouseId = HouseService.Find(h => h.APP_ID == appHouse.APP_ID).Select(h => h.ID).FirstOrDefault();
            if (thisHouseId != 0)
            {
                appHouse.ID = thisHouseId;
                HouseService.Update(appHouse);
                LogWriter.Biz("更新抵押房产资料", appHouse.APP_ID + String.Empty, appHouse);
            }
            else
            {
                HouseService.Add(appHouse);
                LogWriter.Biz("添加抵押房产资料", appHouse.APP_ID + String.Empty, appHouse);
            }
            HouseService.UnitOfWork.SaveChanges();
            return String.Empty;
        }

        public string SaveObligees(APP_CONTACT entity)
        {
            //如果电话号码中包含下划线，则替换为空
            entity.MOBILE = HandleMobile(entity.MOBILE);
            //先根据APP_ID取ID，保证和数据库中ID一致
            var thisEntityId =
                ContactService.Find(
                    c => c.APP_ID == entity.APP_ID && c.CONTACT_PROPERTY == entity.CONTACT_PROPERTY).Select(c => c.ID).FirstOrDefault();
            //entity.CON_ID_TYPE = "Id1";
            if (thisEntityId != 0)
            {
                entity.ID = thisEntityId;
                ContactService.Update(entity);
                Infrastructure.Log4Net.LogWriter.Biz("更新CONTACTS-房屋权利人", entity.APP_ID + "", entity);
            }
            else
            {
                ContactService.Add(entity);
                Infrastructure.Log4Net.LogWriter.Biz("新增CONTACTS-房屋权利人", entity.APP_ID + "", entity);
            }
            ContactService.UnitOfWork.SaveChanges();
            return String.Empty;
        }
        #endregion
        #region 数据提交
        public string SubmitLoan(long appid)
        {
            string error = "";
            var customer = CustomerService.Find(p => p.APP_ID == appid).FirstOrDefault();
            var entity = MainService.FirstOrDefault(c => c.ID == appid);
            if (entity != null)
            {
                //提交
                if (entity.APP_STATUS == EnterStatusType.PENDING + "")
                {
                    bool isForbiden = IsForbiden(entity.APPLY_CITY_CODE, customer, entity.LOGO);
                    if (!isForbiden)
                    {
                        entity.APP_STATUS = EnterStatusType.SUBMIT + "";
                        entity.SUBMIT_TIME = DateTime.Now;
                        MainService.Update(entity);
                        MainService.UnitOfWork.SaveChanges();
                        //using (MQProducer NR_Producer = new MQProducer(GlobalSetting.MQMultipleServer,
                        //    GlobalSetting.MQUserName,
                        //    GlobalSetting.MQUserPassword,
                        //    GlobalSetting.MQApplication_PEN))
                        //{
                        //    if (NR_Producer.Publish(entity.APP_CODE))
                        //    {
                        //        Infrastructure.Log4Net.LogWriter.Biz("MQ用户提交数据成功", appid + "", entity);
                        //    }
                        //    else
                        //    {
                        //        error += "表单提交出错，消息未成功发送，请联系管理员！";
                        //        Infrastructure.Log4Net.LogWriter.Error("MQ消息发送失败");
                        //    }
                        //}

                        if (MQHelper.Publish(
                            GlobalSetting.MQMultipleServer,
                            GlobalSetting.MQUserName,
                            GlobalSetting.MQUserPassword,
                            GlobalSetting.MQApplication_PEN, 
                            entity.APP_CODE))
                        {
                            Infrastructure.Log4Net.LogWriter.Biz("MQ用户提交数据成功", appid + "", entity);
                        }
                        else
                        {
                            error += "表单提交出错，消息未成功发送，请联系管理员！";
                            Infrastructure.Log4Net.LogWriter.Error("MQ消息发送失败");
                        }
                    }
                    else
                    {
                        entity.APP_STATUS = EnterStatusType.DISUSED + "";
                        MainService.Update(entity);
                        MainService.UnitOfWork.SaveChanges();
                        error += "对不起，该客户的身份证号段或居住地或户籍地受限，不予申请。";
                        Infrastructure.Log4Net.LogWriter.Biz("该客户的身份证号段或居住地或户籍地受限，不予申请。进件自动废弃。", appid + "", entity);
                    }
                }
                //车贷补件
                else if (GlobalSetting.CheDaiLogos.Contains(entity.LOGO)
                    && GlobalSetting.Order_SD_Status_Need_Car.Keys.Contains(entity.APP_STATUS))
                {
                    string status = entity.APP_STATUS;
                    //检查是否已经补全
                    error += ApplyTableService.CheckSDUploaded(appid);
                    if (!string.IsNullOrEmpty(error))
                    {
                        return error;
                    }
                    else
                    {
                        error += ApplyTableService.UpdateSDStatusOKCar(appid);

                        //MQProducer NR_Producer = new MQProducer(GlobalSetting.MQMultipleServer,
                        //                            GlobalSetting.MQUserName,
                        //                            GlobalSetting.MQUserPassword,
                        //                                GlobalSetting.MQ_Car_Application_NR_Done);

                        //更改APP_MSGBOX中状态
                        var msgs = MsgBoxService.Find(m => m.APPCODE == entity.APP_CODE).ToList();
                        foreach (var item in msgs)
                        {
                            item.STATUS = MessageStatus.Processed.ToString();
                        }
                        MsgBoxService.UpdateMultiple(msgs);
                        MsgBoxService.UnitOfWork.SaveChanges();

                        //向车贷补件队列发送MQ
                        //注意：车贷补件完成队列 （录入补件） 与 （初审补件，零售信贷员补件，贷后管理专员补件）队列不同
                        //update by ruiwang 20160518
                        string nrDoneQueue =
                            status == EnterStatusType.CARENTRYPATCH.ToString()
                            ? GlobalSetting.MQ_Car_Entry_Application_NR_Done
                            : GlobalSetting.MQ_Car_Application_NR_Done;

                        if (!MQHelper.Publish(
                            GlobalSetting.MQMultipleServer,
                            GlobalSetting.MQUserName,
                            GlobalSetting.MQUserPassword,
                            nrDoneQueue,
                            entity.APP_CODE))
                        {
                            error += "表单提交出错，车贷补件消息未成功发送，请联系管理员";
                        }

                        Infrastructure.Log4Net.LogWriter.Biz("车贷补件消息发送成功", appid + "", entity.APP_CODE);
                    }
                }
                //房贷补件
                else if (GlobalSetting.LogoGroupForMenu["HOUSE"].Contains(entity.LOGO)
                    && GlobalSetting.Order_SD_Status_Need_House.Keys.Contains(entity.APP_STATUS))
                {
                    //检查是否已经补全
                    error += ApplyTableService.CheckSDUploaded(appid);
                    if (!string.IsNullOrEmpty(error))
                    {
                        return error;
                    }
                    else
                    {
                        error += ApplyTableService.UpdateSDStatusOKHouse(appid);

                        //MQProducer NR_Producer = new MQProducer(GlobalSetting.MQMultipleServer,
                        //                            GlobalSetting.MQUserName,
                        //                            GlobalSetting.MQUserPassword,
                        //                                GlobalSetting.MQ_House_Application_NR_Done);

                        //更改APP_MSGBOX中状态
                        var msgs = MsgBoxService.Find(m => m.APPCODE == entity.APP_CODE).ToList();
                        foreach (var item in msgs)
                        {
                            item.STATUS = MessageStatus.Processed.ToString();
                        }
                        MsgBoxService.UpdateMultiple(msgs);
                        MsgBoxService.UnitOfWork.SaveChanges();

                        //向房贷补件队列发送MQ
                        //if (!NR_Producer.Publish(entity.APP_CODE))
                        //{
                        //    error += "表单提交出错，房贷补件消息未成功发送，请联系管理员";
                        //}
                        if (!MQHelper.Publish(
                            GlobalSetting.MQMultipleServer,
                            GlobalSetting.MQUserName,
                            GlobalSetting.MQUserPassword,
                            GlobalSetting.MQ_House_Application_NR_Done,
                            entity.APP_CODE))
                        {
                            error += "表单提交出错，房贷补件消息未成功发送，请联系管理员";
                        }
                        Infrastructure.Log4Net.LogWriter.Biz("房贷补件消息发送成功", appid + "", entity.APP_CODE);
                    }
                }
                //信贷补件
                else if (Global.GlobalSetting.Order_SD_Status_Need.Keys.Contains(entity.APP_STATUS))
                {
                    error += ApplyTableService.CheckSDUploaded(appid);
                    if (!string.IsNullOrEmpty(error))
                    {
                        return error;
                    }
                    else
                    {
                        error += ApplyTableService.UpdateSDStatusOK(appid);

                        //更改APP_MSGBOX中状态
                        var msgs = MsgBoxService.Find(m => m.APPCODE == entity.APP_CODE).ToList();
                        foreach (var item in msgs)
                        {
                            item.STATUS = MessageStatus.Processed.ToString();
                        }
                        MsgBoxService.UpdateMultiple(msgs);
                        MsgBoxService.UnitOfWork.SaveChanges();

                        //MQProducer NR_Producer = new MQProducer(GlobalSetting.MQMultipleServer,
                        //                            GlobalSetting.MQUserName,
                        //                            GlobalSetting.MQUserPassword,
                        //                           GlobalSetting.MQApplication_NR_Done);

                        //向信贷补件完成队列发送MQ
                        //if (!NR_Producer.Publish(entity.APP_CODE))
                        //{
                        //    error += "表单提交出错，消息未成功发送，请联系管理员";
                        //}
                        if (!MQHelper.Publish(
                            GlobalSetting.MQMultipleServer,
                            GlobalSetting.MQUserName,
                            GlobalSetting.MQUserPassword,
                            GlobalSetting.MQApplication_NR_Done,
                            entity.APP_CODE))
                        {
                            error += "表单提交出错，消息未成功发送，请联系管理员";
                        }
                        Infrastructure.Log4Net.LogWriter.Biz("信贷补件消息发送成功", appid + "", entity.APP_CODE);
                    }
                }
                //极客贷补件
                else if (GlobalSetting.LogoGroupForMenu["GEEK"].Contains(entity.LOGO) && GlobalSetting.Order_SD_Status_Need_Geek.Keys.Contains(entity.APP_STATUS))
                {
                    error += ApplyTableService.CheckSDUploaded(appid);
                    if (!string.IsNullOrEmpty(error))
                    {
                        return error;
                    }
                    else
                    {
                        error += ApplyTableService.UpdateSDStatusOK(appid);

                        //更改APP_MSGBOX中状态
                        var msgs = MsgBoxService.Find(m => m.APPCODE == entity.APP_CODE).ToList();
                        foreach (var item in msgs)
                        {
                            item.STATUS = MessageStatus.Processed.ToString();
                        }
                        MsgBoxService.UpdateMultiple(msgs);
                        MsgBoxService.UnitOfWork.SaveChanges();

                        //MQProducer NR_Producer = new MQProducer(GlobalSetting.MQMultipleServer,
                        //                            GlobalSetting.MQUserName,
                        //                            GlobalSetting.MQUserPassword,
                        //                           GlobalSetting.MQApplication_NR_Done);

                        //向极客贷补件完成队列发送MQ
                        //if (!NR_Producer.Publish(entity.APP_CODE))
                        //{
                        //    error += "表单提交出错，消息未成功发送，请联系管理员";
                        //}
                        if (!MQHelper.Publish(
                            GlobalSetting.MQMultipleServer,
                            GlobalSetting.MQUserName,
                            GlobalSetting.MQUserPassword,
                            GlobalSetting.MQApplication_NR_Done,
                            entity.APP_CODE))
                        {
                            error += "表单提交出错，消息未成功发送，请联系管理员";
                        }
                        Infrastructure.Log4Net.LogWriter.Biz("极客贷补件消息发送成功", appid + "", entity.APP_CODE);
                    }
                }
                else
                {
                    Infrastructure.Log4Net.LogWriter.Biz("提交失败，进件状态不正确", entity.APP_STATUS);
                    return "提交失败请检查你的进件状态！";
                }
            }
            else
            {
                Infrastructure.Log4Net.LogWriter.Biz("提交失败，申请未找到", appid + "");
                error += "申请未找到";
            }
            return error;
        }

        /// <summary>
        /// 车贷黑名单的过滤
        /// </summary>
        /// <param name="appCity">进件城市</param>
        /// <param name="customer">客户信息</param>
        /// <param name="logo">产品Logo</param>
        /// <returns></returns>
        private bool IsForbiden(string appCity, APP_CUSTOMER customer, string logo)
        {
            //返回值，true表示废弃
            bool result = false;
            //规则标志，分别表示规则中 省，市，详细 是否为空； （0表示空）
            char[] flags = new[] { '0', '0', '0' };
            int flag;

            string idCard = customer.ID_NO.Trim().Substring(0, 6);
            var rules = ForbidenService.Find(p => p.PRODUCT_LOGO == logo);
            if (rules != null)
            {
                foreach (var rule in rules)
                {
                    if (rule.FORBIDEN_ID_START == idCard)
                    {
                        result = true;
                    }
                    else
                    {
                        flags[0] = string.IsNullOrEmpty(rule.PROVINCE_CODE) ? '0' : '1';
                        flags[1] = string.IsNullOrEmpty(rule.PRPVINCE_CITY) ? '0' : '1';
                        flags[2] = string.IsNullOrEmpty(rule.FORBIDEN_AREA) ? '0' : '1';
                        flag = Convert.ToInt32(string.Join("", flags), 2);
                        //只校验 省,市,详细地址 中非空的情况
                        switch (flag)
                        {
                            case 0:
                                break;
                            case 1:
                                result = CheckAddress(rule, customer);
                                break;
                            case 2:
                                result = CheckCity(rule, customer);
                                break;
                            case 3:
                                result = CheckCity(rule, customer) && CheckAddress(rule, customer);
                                break;
                            case 4:
                                result = CheckProvince(rule, customer);
                                break;
                            case 5:
                                result = CheckProvince(rule, customer) && CheckAddress(rule, customer);
                                break;
                            case 6:
                                result = CheckProvince(rule, customer) && CheckCity(rule, customer);
                                break;
                            case 7:
                                result = CheckProvince(rule, customer) && CheckCity(rule, customer) &&
                                         CheckAddress(rule, customer);
                                break;
                        }
                    }

                    //如果匹配到规则
                    if (result)
                    {
                        //若仅在设置的城市有效，则不废弃，继续匹配
                        //1：仅在设置的城市有效
                        //0：全国有效
                        if (rule.CITY_CODE != appCity && rule.JUST_FOR_CITY == "1")
                        {
                            result = false;
                            continue;
                        }
                        //跳出
                        break;
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// 车贷黑名单，判断省份是否匹配
        /// </summary>
        /// <param name="rule"></param>
        /// <param name="customer"></param>
        /// <returns></returns>
        private bool CheckProvince(APP_FORBIDEN_AREA rule, APP_CUSTOMER customer)
        {
            bool r = rule.PROVINCE_CODE == customer.REGISTER_PROVINCE || rule.PROVINCE_CODE == customer.RESIDENT_PROVINCE;
            return r;
        }

        /// <summary>
        ///  车贷黑名单，判断市区是否匹配
        /// </summary>
        /// <param name="rule"></param>
        /// <param name="customer"></param>
        /// <returns></returns>
        private bool CheckCity(APP_FORBIDEN_AREA rule, APP_CUSTOMER customer)
        {
            bool r = rule.PRPVINCE_CITY == customer.REGISTER_CITY || rule.PRPVINCE_CITY == customer.RESIDENT_CITY;
            return r;
        }

        /// <summary>
        ///  车贷黑名单，判断详细地址是否匹配
        /// </summary>
        /// <param name="rule"></param>
        /// <param name="customer"></param>
        /// <returns></returns>
        private bool CheckAddress(APP_FORBIDEN_AREA rule, APP_CUSTOMER customer)
        {
            bool a = false, b = false;
            if (!string.IsNullOrEmpty(customer.REGISTER_ADDRESS))
            {
                a = customer.REGISTER_ADDRESS.Contains(rule.FORBIDEN_AREA);
            }
            if (!string.IsNullOrEmpty(customer.RESIDENT_ADDRESS))
            {
                b = customer.RESIDENT_ADDRESS.Contains(rule.FORBIDEN_AREA);
            }

            return a || b;
        }

        /// <summary>
        /// 车贷黑名单的过滤
        /// </summary>
        /// <param name="appcity">开办业务城市</param>
        /// <param name="customer"></param>
        /// <returns></returns>
        //public bool IsForbiden(string appcity, APP_CUSTOMER customer, string logo)
        //{
        //    bool app = true;
        //    string IDcard = customer.ID_NO.Trim().Substring(0, 6);
        //    var resultlogo = ForbidenService.Find(p => p.PRODUCT_LOGO == logo).ToList();
        //    if (resultlogo != null)
        //    {
        //        foreach (var item in resultlogo)
        //        {
        //            if (item.FORBIDEN_ID_START == IDcard)
        //            {
        //                //是否能匹配到对应的开办业务城市
        //                if (item.CITY_CODE != appcity)
        //                {
        //                    //是否仅在设置的城市有效
        //                    //1：仅在设置的城市有效
        //                    //0：全国有效
        //                    if (item.JUST_FOR_CITY == "0")
        //                    {
        //                        app = false;
        //                        break;
        //                    }
        //                }
        //                else
        //                {
        //                    app = false;
        //                    break;
        //                }
        //            }
        //            else
        //            {
        //                //先查省份CODE在表里的数据
        //                if (item.PROVINCE_CODE == customer.REGISTER_PROVINCE || item.PROVINCE_CODE == customer.RESIDENT_PROVINCE)
        //                {
        //                    app = Is(appcity, customer, app, item);
        //                    if (!app)
        //                    {
        //                        break;
        //                    }
        //                }
        //            }
        //        }
        //    }

        //    return app;
        //}

        //private static bool Is(string appcity, APP_CUSTOMER customer, bool app, APP_FORBIDEN_AREA item)
        //{
        //    if (string.IsNullOrEmpty(item.PRPVINCE_CITY))
        //    {
        //        //是否能匹配到对应的开办业务城市
        //        if (item.CITY_CODE != appcity)
        //        {
        //            //是否仅在此开办业务城市有效
        //            if (item.JUST_FOR_CITY == "0")
        //            {
        //                app = false;
        //            }
        //        }
        //        else
        //        {
        //            app = false;
        //        }
        //    }
        //    if (item.PRPVINCE_CITY == customer.REGISTER_CITY || item.PRPVINCE_CITY == customer.RESIDENT_CITY)
        //    {
        //        if (string.IsNullOrEmpty(item.FORBIDEN_AREA))
        //        {
        //            //是否能匹配到对应的开办业务城市
        //            if (item.CITY_CODE != appcity)
        //            {
        //                //是否仅在此开办业务城市有效
        //                if (item.JUST_FOR_CITY == "0")
        //                {
        //                    app = false;
        //                }
        //            }
        //            else
        //            {
        //                app = false;
        //            }
        //        }
        //        if (!string.IsNullOrEmpty(item.FORBIDEN_AREA) &&
        //            (customer.REGISTER_ADDRESS.Contains(item.FORBIDEN_AREA) || customer.RESIDENT_ADDRESS.Contains(item.FORBIDEN_AREA)))
        //        {
        //            //是否能匹配到对应的开办业务城市
        //            if (item.CITY_CODE != appcity)
        //            {
        //                //是否仅在此开办业务城市有效
        //                if (item.JUST_FOR_CITY == "0")
        //                {
        //                    app = false;
        //                }
        //            }
        //            else
        //            {
        //                app = false;
        //            }
        //        }

        //    }
        //    return app;
        //}
        #endregion
        #endregion
        public string CheckDataPermission(long appid, ENUM_FormOperation operation)
        {
            //var staffOnly = GetStaffOnly(appid);
            var appMain = MainService.FirstOrDefault(c => c.ID == appid);
            if (appMain != null)
            {
                //检查状态和现在操作符合性
                switch (operation)
                {
                    case ENUM_FormOperation.ADD:
                        if (appMain.APP_STATUS != EnterStatusType.PENDING + "")
                        {
                            Infrastructure.Log4Net.LogWriter.Biz("越权请求编辑申请", appid + "", appMain);
                            return "该申请现在不能编辑！";
                        }
                        break;
                    case ENUM_FormOperation.EDIT:
                        if (!GlobalSetting.Order_SD_Status_Need.ContainsKey(appMain.APP_STATUS))
                        {
                            Infrastructure.Log4Net.LogWriter.Biz("请求非补件状态的申请补件", appid + "", appMain);
                            return "该申请现在不能补件！";
                        }
                        break;
                    case ENUM_FormOperation.READONLY:
                        break;
                    case ENUM_FormOperation.REISSUE:
                        if (!GlobalSetting.Order_SD_Status_Need_Car.ContainsKey(appMain.APP_STATUS)
                            && !GlobalSetting.Order_SD_Status_Need_House.ContainsKey(appMain.APP_STATUS))
                        {
                            Infrastructure.Log4Net.LogWriter.Biz("请求非补件状态的申请补件", appid + "", appMain);
                            return "该申请现在不能补件！";
                        }
                        if (!GlobalSetting.CheDaiLogos.Contains(appMain.LOGO)
                            && !GlobalSetting.LogoGroupForMenu["HOUSE"].Contains(appMain.LOGO))
                        {
                            Infrastructure.Log4Net.LogWriter.Biz("请求信贷的申请补件", appid + "", appMain);
                            return "所请求的补件类型不正确，不能补件！";
                        }
                        break;
                }

                //通过AppAuth验证单条数据权限
                if (UserService.CheckDataPermission(appMain.ID))
                {
                    return String.Empty;
                }
                else
                {
                    Infrastructure.Log4Net.LogWriter.Biz(string.Format("越权请求，当前用户无进件（appId:{0}）的数据权限", appid), appid + "", appMain);
                    return "非法访问，您无法访问该申请！";
                }

                //if (appMain.APP_STATUS == EnterStatusType.PENDING + "")
                //{
                //    if (UserService.CheckDataPermission(staffOnly.CSAD_CODE))
                //    {
                //        return "";
                //    }
                //    else
                //    {
                //        Infrastructure.Log4Net.LogWriter.Biz("越权请求，当前用户无法操作申请单", appid + "", appMain);
                //        return "非法访问，您无法访问该申请！";
                //    }
                //}
                return string.Empty;
            }
            return "数据不存在!";
        }

        /// <summary>
        /// 申请表单保存时验证表单是否可编辑
        /// author:张浩
        /// date:2016-03-30
        /// </summary>
        /// <param name="status">申请状态</param>
        /// <returns>true 可编辑，false 不可编辑</returns>
        public bool CheckIsAllowEdit(string status)
        {
            bool result = GlobalSetting.AllowEditOrderStatus.Contains(status);
            return result;
        }

        /// <summary>
        /// 申请表单保存时验证表单是否可编辑
        /// author:张浩
        /// date:2016-03-30
        /// </summary>
        /// <param name="appid">申请ID</param>
        /// <returns>true 可编辑，false 不可编辑</returns>
        public bool CheckIsAllowEdit(long appid)
        {
            var main = MainService.FirstOrDefault(a => a.ID == appid, false);
            if (main != null)
            {
                bool result = GlobalSetting.AllowEditOrderStatus.Contains(main.APP_STATUS);
                if (!result)
                {
                    LogWriter.Biz("进件ID" + appid + "状态已变更，不能重复保存或提交");
                }
                return result;
            }
            //如果APP_MAIN表查不到记录，记录日志
            LogWriter.Error("appid" + appid + "没有对应的APP_MAIN记录");
            return false;
        }

        /// <summary>
        /// 处理电话号码，如果有下划线，则替换为空
        /// </summary>
        /// <param name="mobile"></param>
        /// <returns></returns>
        private string HandleMobile(string mobile)
        {
            if (string.IsNullOrEmpty(mobile))
            {
                return mobile;
            }
            mobile = mobile.Replace("_", "");
            return string.IsNullOrEmpty(mobile) ? null : mobile;

        }
    }
}
