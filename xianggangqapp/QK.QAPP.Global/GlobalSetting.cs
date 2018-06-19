using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace QK.QAPP.Global
{


    /// <summary>
    /// 全局配置字符串
    /// </summary>
    public class GlobalSetting
    {

        public static Dictionary<string, string> GlobalSettingDic = new Dictionary<string, string>();



        #region 只读字段
        /// <summary>
        /// Signalr in redis key
        /// </summary>
        public static readonly string SignalREventKey = "QAPPSignalr";
        #endregion


        /// <summary>
        /// 委托  获取值的方式
        /// </summary>
        public static Func<string, string> GetConfigValueDelegate
        {
            private get;
            set;
        }
        /// <summary>
        /// 读取数据
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private static string GetValue(string key)
        {
            if (GetConfigValueDelegate != null)
            {
                var value = GetConfigValueDelegate(key);
                if (!string.IsNullOrEmpty(value))
                {
                    return value;

                }

            }
            return (ConfigurationManager.AppSettings[key] + "").Trim(); ;

        }
        private static string GetValueForWebConfig(string key)
        {
            return (ConfigurationManager.AppSettings[key] + "").Trim(); ;
        }

        private static Dictionary<string, string> SplitStandardFormatConfig(string key)
        {
            Dictionary<string, string> dicStatus = new Dictionary<string, string>();
            string strStatus = GetValue(key);
            string[] temp = strStatus.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string t in temp)
            {
                string[] ary = t.Split(':');
                if (ary.Length > 1)
                {
                    dicStatus.Add(ary[0], ary[1]);
                }
            }
            return dicStatus;
        }
        private static Dictionary<string, string> SplitStandardFormatConfigDic(string key,char sp)
        {
            Dictionary<string, string> dicStatus = new Dictionary<string, string>();
            string strStatus = GetValue(key);
            string[] temp = strStatus.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string t in temp)
            {
                string[] ary = t.Split(sp);
                if (ary.Length > 1)
                {
                    dicStatus.Add(ary[0], ary[1]);
                }
            }
            return dicStatus;
        }
        private static Dictionary<string,List<string>> SplitStandardFormatConfigList(string key)
        {
            Dictionary<string, List<string>> dicStatus = new Dictionary<string, List<string>>();
            string strStatus = GetValue(key);
            string[] temp = strStatus.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string t in temp)
            {
                string[] ary = t.Split(':');
                if (ary.Length > 1)
                {
                    dicStatus.Add(ary[0], new List<string>(ary[1].Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries)));
                    
                }else
                {
                    dicStatus.Add(ary[0], new List<string>());
                }
            }
            return dicStatus;
        }

        #region 0、数据库相关
        /// <summary>
        /// 权限系统菜单获取地址
        /// </summary>
        public static string MainDataBasenameOrConnectionString
        {
            get
            {
                return GetValueForWebConfig("MainDataBasenameOrConnectionString");
            }
        }
        #endregion

        #region 1、外部数据相关
        /// <summary>
        /// 权限系统菜单获取地址
        /// </summary>
        public static string AuthInterfaceURL
        {
            get
            {
                return GetValue("AuthInterfaceURL");
            }
        }
        /// <summary>
        /// 当前是否允许登录 OPEN,CLOSE
        /// </summary>
        public static string AllowLogin
        {
            get
            {
                return GetValue("AllowLogin");
            }
        }
        /// <summary>
        /// 禁用登录系统时允许登陆的账户，多个账户中间用“;”分割
        /// </summary>
        public static string AllowAcount
        {
            get
            {
                return GetValue("AllowAcount");
            }
        }
        /// <summary>
        /// 是否允许用户不同客户端重复登录（是-OPEN，否-CLOSE）
        /// </summary>
        public static string MultipleLogin
        {
            get
            {
                return GetValue("MultipleLogin");
            }
        }
        /// <summary>
        /// 禁用登录系统时提示信息
        /// </summary>
        public static string TipInfo
        {
            get
            {
                return GetValue("TipInfo");
            }
        }
        /// <summary>
        /// 用户相片
        /// </summary>
        public static string UserImageUrl
        {
            get
            {
                return GetValue("UserImageUrl");
            }
        }
        /// <summary>
        /// 跟菜单编码
        /// </summary>
        public static string RootMenuCode
        {
            get
            {
                return GetValue("RootMenuCode");
            }
        }

        /// <summary>
        /// 文件上传接口地址
        /// </summary>
        public static string FileUploadUrl
        {
            get
            {
                return GetValue("FileUploadUrl");
            }
        }

        /// <summary>
        /// 文件列表读取接口地址
        /// </summary>
        public static string FileListReadUrl
        {
            get
            {
                return GetValue("FileListReadUrl");
            }
        }

        /// <summary>
        /// 文件读取接口地址
        /// </summary>
        public static string FileReadUrl
        {
            get
            {
                return GetValue("FileReadUrl");
            }
        }

        /// <summary>
        /// 文件排序接口地址
        /// </summary>
        public static string FileSortUrl
        {
            get
            {
                return GetValue("FileSortUrl");
            }
        }

        /// <summary>
        /// 文件删除接口地址
        /// </summary>
        public static string FileRemoveUrl
        {
            get
            {
                return GetValue("FileRemoveUrl");
            }
        }
        /// <summary>
        /// 是否开启中金验证
        /// </summary>
        public static bool IsCFCA
        {
            get { return GetValue("IsCFCA") == "1" ? true : false; }
        }
        /// <summary>
        /// 中金认证API接口地址
        /// </summary>
        public static string CFCAPaymentURL
        {
            get { return GetValue("CFCAPaymentURL"); }
        }

        /// <summary>
        /// 银行卡验证应用ID
        /// </summary>
        public static string BankCardVerifyAppID
        {
            get { return GetValue("BankCardVerifyAppID"); }
        }

        /// <summary>
        /// 通话详单状态接口地址
        /// </summary>
        public static string MobileHistoryUrl
        {
            get { return GetValue("MobileHistoryUrl"); }
        }

        /// <summary>
        /// 网银认证状态接口
        /// </summary>
        public static string NetbankUrl
        {
            get { return GetValue("NetbankUrl"); }
        }

        /// <summary>
        /// Pboc认证接口
        /// </summary>
        public static string PbocUrl
        {
            get { return GetValue("PbocUrl"); }
        }

        /// <summary>
        /// 公积金认证接口
        /// </summary>
        public static string FundUrl
        {
            get { return GetValue("FundUrl"); }
        }
        #endregion

        #region 2、菜单权限相关
        /// <summary>
        /// 用户存储的Session
        /// </summary>
        public static string AuthSaveKey
        {
            get
            {
                return GetValueForWebConfig("AuthSaveKey");
            }
        }
        /// <summary>
        /// 菜单保存的Session
        /// </summary>
        public static string MenuSaveKey
        {
            get
            {
                return GetValueForWebConfig("MenuSaveKey");
            }
        }
        /// <summary>
        /// 登录页面的URL
        /// </summary>
        public static string AuthUrl
        {
            get
            {
                return GetValueForWebConfig("AuthUrl");
            }
        }
        /// <summary>
        /// 用户保存的位置
        /// </summary>
        public static string AuthSaveType
        {
            get
            {
                return GetValueForWebConfig("AuthSaveType");
            }
        }
        public static string AdminCode
        {
            get
            {
                return GetValue("AdminCode");
            }
        }
        #endregion

        #region 3、日志监控相关
        /// <summary>
        /// 是否开启底层数据库语句 选项值:OPEN,CLOSE
        /// </summary>
        public static string SqlTraceing
        {
            get
            {
                return GetValue("SqlTraceing");
            }
        }
        /// <summary>
        /// 页面性能监测  选项值：OPEN,CLOSE
        /// </summary>
        public static string PageTraceing
        {
            get
            {
                return GetValue("PageTraceing");
            }
        }
        #endregion

        #region 4、缓存相关配置

        /// <summary>
        /// 缓存服务器分布式配置
        /// </summary>
        public static string CacheSeverConfiguration
        {
            get
            {
                return GetValueForWebConfig("CacheSeverConfiguration");
            }
        }

        /// <summary>
        /// 缓存服务器地址
        /// </summary>
        public static string CahcheServer
        {
            get
            {
                return CacheSeverConfiguration.Split(',')[0].Split(':')[0];
            }
        }
        /// <summary>
        /// 缓存服务器端口
        /// </summary>
        public static int CahcheServerPort
        {
            get
            {
                int OutInt = 6397;
                string[] temp = CacheSeverConfiguration.Split(',')[0].Split(':');
                if (temp.Length > 1)
                    int.TryParse(temp[1], out OutInt);
                return OutInt;
            }
        }
        #endregion

        #region 5、业务相关配置
        /// <summary>
        /// 动态表单当前使用版本
        /// </summary>
        public static Dictionary<string, string> DFormVersions
        {
            get
            {
                return SplitStandardFormatConfig("DFormVersion");
            }
        }

        public static Dictionary<string, string> RyDFormVersions
        {
            get
            {
                return SplitStandardFormatConfig("RyDFormVersion");
            }
        }
        /// <summary>
        /// 动态表单路径
        /// </summary>
        public static string DFormPath
        {
            get
            {
                return System.AppDomain.CurrentDomain.BaseDirectory + GetValue("DFormPath");
            }
        }

        /// <summary>
        /// 动态表单后端错误提示模板
        /// </summary>
        public static string DFormErrorTemp
        {
            get { return GetValue("DFormErrorTemp"); }
        }

        /// <summary>
        /// 车贷Logo（车贷部）
        /// </summary>
        public static string[] CheDaiLogos
        {
            get
            {
                return LogoGroupForMenu["VEHICLE"].ToArray();
            }
        }

        /// <summary>
        /// 车贷Logo（个金）
        /// </summary>
        public static string[] GJCheDaiLogos
        {
            get
            {
                return LogoGroupForMenu["GJVEHICLE"].ToArray();
            }
        }

        /// <summary>
        /// 客户类型-经营类
        /// </summary>
        public static string CustomerJYCategory
        {
            get { return GetValue("CustomerJYCategory"); }
        }
        /// <summary>
        /// 客户类型-授薪类
        /// </summary>
        public static string CustomerSXCategory
        {
            get { return GetValue("CustomerSXCategory"); }
        }
        /// <summary>
        /// 微车贷所属父表单名字
        /// </summary>
        public static string CheDaiCustomerCategoryParent
        {
            get { return GetValue("CheDaiCustomerCategoryParent"); }
        }
        /// <summary>
        /// 客户类型经营人群所属组
        /// </summary>
        public static string CustomerJYGroup
        {
            get { return GetValue("CustomerJYGroup"); }
        }
        /// <summary>
        /// 客户类型授薪人群所属组
        /// </summary>
        public static string CustomerSXGroup
        {
            get { return GetValue("CustomerSXGroup"); }
        }

        /// <summary>
        /// 系统版本
        /// </summary>
        public static string SystemVersion
        {
            get { return GetValueForWebConfig("SystemVersion"); }
        }

        /// <summary>
        /// 车贷申请页面申请金额字段显示的标题名
        /// </summary>
        public static Dictionary<string, string> CarLoan_LoanAmountTitle
        {
            get
            {
                Dictionary<string, string> dict = new Dictionary<string, string>();
                string strVers = GetValue("CarLoan_LoanAmountTitle");
                string[] temp = strVers.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var item in temp)
                {
                    string[] ary = item.Split(':');
                    if (ary.Length > 1)
                    {
                        dict.Add(ary[0], ary[1]);
                    }
                }
                return dict;
            }
        }

        /// <summary>
        /// 申请时不需要填借款用途的产品
        /// </summary>
        public static string NoNeed_LoanPurpose_Product
        {
            get { return GetValue("NoNeed_LoanPurpose_Product"); }
        }
        #endregion

        #region 6、文件上传相关配置

        /// <summary>
        /// 文件上传格式
        /// </summary>
        public static string UploadFileFormat
        {
            get
            {
                return GetValue("UploadFileFormat");
            }
        }

        /// <summary>
        /// 允许上传的最大文件（单位：KB）
        /// </summary>
        public static int UploadMaxSize
        {
            get
            {
                string strSize = GetValue("UploadMaxSize");
                int intSize = 10240;
                int.TryParse(strSize, out intSize);
                return intSize;
            }
        }

        /// <summary>
        /// 分块上传时每块大小（单位：KB）
        /// </summary>
        public static int UploadChunkSize
        {
            get
            {
                string strSize = GetValue("UploadChunkSize");
                int intSize = 5120;
                int.TryParse(strSize, out intSize);
                return intSize;
            }
        }

        /// <summary>
        /// 允许下载的最大文件（单位：KB）
        /// </summary>
        public static int DownloadMaxSize
        {
            get
            {
                string strSize = GetValue("DownloadMaxSize");
                int intSize = 10240;
                int.TryParse(strSize, out intSize);
                return intSize;
            }
        }

        /// <summary>
        /// 分块下载时每块大小（单位：KB）
        /// </summary>
        public static int DownloadChunkSize
        {
            get
            {
                string strSize = GetValue("DownloadChunkSize");
                int intSize = 5120;
                int.TryParse(strSize, out intSize);
                return intSize;
            }
        }

        /// <summary>
        /// JavaScriptSerializer的最大MaxJsonLength
        /// </summary>
        public static int MaxJsonLength
        {
            get
            {
                string strSize = GetValue("MaxJsonLength");
                int intSize = 5120;
                int.TryParse(strSize, out intSize);
                return intSize;
            }
        }

        /// <summary>
        /// 文件列表是否从接口获取
        /// </summary>
        public static bool FileListFromAPI
        {
            get
            {
                string isFrom = GetValue("FileListFromAPI");
                bool boolIsFrom = false;
                bool.TryParse(isFrom, out boolIsFrom);
                return boolIsFrom;
            }
        }

        #endregion

        #region 7、产品数据相关
        /// <summary>
        /// 获取产品服务接口地址
        /// </summary>
        public static string ProductionInfoInterfaceURL
        {
            get
            {
                return GetValue("ProductInfoInterfaceURL");
            }
        }
        /// <summary>
        /// 城市产品配置中的目标使用平台
        /// </summary>
        public static Dictionary<string, string> UsingPlatformForCityProduct
        {
            get
            {
                return SplitStandardFormatConfig("UsingPlatformForCityProduct");
            }
        }

        /// <summary>
        /// 申请系统在城市产品配置中的使用平台代码
        /// </summary>
        public static string UsingPlatformForCityProduct_QAPP
        {
            get
            {
                return GetValue("UsingPlatformForCityProduct_QAPP");
            }
        }
        #endregion

        #region 8.ApacheMQ消息列表配置
        public static string MQMultipleServer
        {
            get
            {
                return GetValue("MQMultipleServer");
            }
        }

        /// <summary>
        /// MQ用户名
        /// </summary>
        public static string MQUserName
        {
            get
            {
                return GetValue("MQUserName");
            }
        }
        /// <summary>
        /// MQ密码
        /// </summary>
        public static string MQUserPassword
        {
            get
            {
                return GetValue("MQUserPassword");
            }
        }

        /// <summary>
        /// 信贷补件队列
        /// </summary>
        public static string MQApplication_NR
        {
            get
            {
                return GetValue("APPLICATION_NR");
            }
        }

        /// <summary>
        /// 车贷补件队列
        /// （车贷补件和资料修改是一起的）
        /// </summary>
        public static string MQ_Car_Application_NR
        {
            get
            {
                return GetValue("CAR_APPLICATION_NR");
            }
        }

        /// <summary>
        /// 车贷补件完成队列
        /// </summary>
        public static string MQ_Car_Application_NR_Done
        {
            get
            {
                return GetValue("CAR_APPLICATION_NR_DONE");
            }
        }

        /// <summary>
        /// 车贷待评估队列
        /// </summary>
        public static string MQ_CAR_TO_BE_ASSESSED
        {
            get
            {
                return GetValue("MQ_CAR_TO_BE_ASSESSED");
            }
        }

        /// <summary>
        /// 信贷补件队列消息通知模板
        /// </summary>
        public static string APPLICATION_NR_CONTENT
        {
            get
            {
                return GetValue("APPLICATION_NR_CONTENT");
            }
        }

        /// <summary>
        /// 车贷补件队列消息通知模板
        /// </summary>
        public static string CAR_TO_BE_ASSESSED_CONTENT
        {
            get
            {
                return GetValue("CAR_TO_BE_ASSESSED_CONTENT");
            }
        }

        /// <summary>
        /// 车贷待评估队列消息通知模板
        /// </summary>
        public static string CAR_APPLICATION_NR_CONTENT
        {
            get
            {
                return GetValue("CAR_APPLICATION_NR_CONTENT");
            }
        }

        /// <summary>
        /// 信贷补件队列监听
        /// </summary>
        public static string NR_Listener_Enable
        {
            get
            {
                return GetValue("NR_Listener_Enable");
            }
        }

        /// <summary>
        /// 车贷补件队列监听开启
        /// </summary>
        public static string Car_NR_Listener_Enable
        {
            get
            {
                return GetValue("CAR_NR_Listener_Enable");
            }
        }


        /// <summary>
        /// 申请列表
        /// </summary>
        public static string MQApplication_PEN
        {
            get
            {
                return GetValue("APPLICATION_PEN");
            }
        }

        public static string MQApplication_NR_Done
        {
            get
            {
                return GetValue("APPLICATION_NR_DONE");
            }
        }

        public static Dictionary<string, string> APP_MSGBOXCATEGORY
        {
            get
            {
                return SplitStandardFormatConfig("APP_MSGBOXCATEGORY");
            }
        }
        #endregion

        #region 9.进件逻辑处理相关配置

        /// <summary>
        /// 信贷补件申请的有效天数
        /// </summary>
        public static int Order_SD_AbidanceDay
        {
            get
            {
                string strDays = GetValue("Order_SD_AbidanceDay");
                int intDays = 7;
                int.TryParse(strDays, out intDays);
                return intDays;
            }
        }

        /// <summary>
        /// 车贷补件申请的有效天数
        /// </summary>
        public static int Order_SD_AbidanceDay_Car
        {
            get
            {
                string strDays = GetValue("Order_SD_AbidanceDay_Car");
                int intDays = 10;
                int.TryParse(strDays, out intDays);
                return intDays;
            }
        }

        /// <summary>
        /// 需要补件的状态
        /// </summary>
        public static Dictionary<string, string> Order_SD_Status_Need
        {
            get
            {
                return SplitStandardFormatConfig("Order_SD_Status_Need");
            }
        }
        /// <summary>
        /// 极客贷需要补件的状态
        /// </summary>
        public static Dictionary<string, string> Order_SD_Status_Need_Geek
        {
            get
            {
                return SplitStandardFormatConfig("Order_SD_Status_Need_Geek");
            }
        }
        /// <summary>
        /// 已经补件的状态
        /// </summary>
        public static Dictionary<string, string> Order_SD_Status_Had
        {
            get
            {
                return SplitStandardFormatConfig("Order_SD_Status_Had");
            }
        }
        /// <summary>
        /// 极客贷已经补件的状态
        /// </summary>
        public static Dictionary<string, string> Order_SD_Status_Had_Geek
        {
            get
            {
                return SplitStandardFormatConfig("Order_SD_Status_Had_Geek");
            }
        }

        /// <summary>
        /// 补件失效的状态
        /// </summary>
        public static Dictionary<string, string> Order_SD_Status_Cancel
        {
            get
            {
                return SplitStandardFormatConfig("Order_SD_Status_Cancel");
            }
        }
        /// <summary>
        /// 极客贷补件失效的状态
        /// </summary>
        public static Dictionary<string, string> Order_SD_Status_Cancel_Geek
        {
            get
            {
                return SplitStandardFormatConfig("Order_SD_Status_Cancel_Geek");
            }
        }

        /// <summary>
        /// 房贷补件失效的状态
        /// </summary>
        public static Dictionary<string, string> Order_SD_Status_Cancel_House
        {
            get
            {
                return SplitStandardFormatConfig("Order_SD_Status_Cancel_House");
            }
        }

        /// <summary>
        /// 所有非补件状态
        /// </summary>
        public static Dictionary<string, string> Order_ExceptSD_Status
        {
            get
            {
                return SplitStandardFormatConfig("Order_ExcSD_Status");
            }
        }
        /// <summary>
        /// 极客贷所有非补件状态
        /// </summary>
        public static Dictionary<string, string> Order_ExceptSD_Status_Geek
        {
            get
            {
                return SplitStandardFormatConfig("Order_ExcSD_Status_Geek");
            }
        }

        public static Dictionary<string, string> Order_ExceptSD_Status_Ry100
        {
            get { return SplitStandardFormatConfig("Order_ExceptSD_Status_Ry100"); }
        }

        /// <summary>
        /// 补件状态对应APP_QUEUE的DO_ACTION动作
        /// </summary>
        public static Dictionary<string, string> SD_DO_ACTION
        {
            get
            {
                return SplitStandardFormatConfig("SD_DO_ACTION");
            }
        }

        /// <summary>
        /// 允许修改表单的状态
        /// </summary>
        public static string[] AllowEditOrderStatus
        {
            get
            {
                string strStatus = GetValue("AllowEditOrderStatus");
                return strStatus.Split(',');
            }
        }

        #endregion

        #region QAPP_V2 Global Config

        /// <summary>
        /// 车贷录入待补件队列
        /// </summary>
        public static string MQ_Car_Entry_Application_NR
        {
            get { return GetValue("MQ_Car_Entry_Application_NR"); }
        }

        /// <summary>
        /// 车贷录入补件完成队列
        /// </summary>
        public static string MQ_Car_Entry_Application_NR_Done
        {
            get { return GetValue("MQ_Car_Entry_Application_NR_Done"); }
        }

        /// <summary>
        /// 需要补件的状态
        /// </summary>
        public static Dictionary<string, string> Order_SD_Status_Need_Car
        {
            get
            {
                return SplitStandardFormatConfig("Order_SD_Status_Need_Car");
            }
        }

        /// <summary>
        /// 所有非补件状态
        /// </summary>
        public static Dictionary<string, string> Order_ExceptSD_Status_Car
        {
            get
            {
                return SplitStandardFormatConfig("Order_ExceptSD_Status_Car");
            }
        }

        /// <summary>
        /// 已经补件的状态
        /// </summary>
        public static Dictionary<string, string> Order_SD_Status_Had_Car
        {
            get
            {
                return SplitStandardFormatConfig("Order_SD_Status_Had_Car");
            }
        }

        /// <summary>
        /// APPExtendConfig表中“展期”的actionGroup
        /// </summary>
        public static List<string> APPExtendConfig_Extend
        {
            get
            {
                return GetValue("APPExtendConfig_Extend").Split(',').ToList();
            }
        }

        /// <summary>
        /// APPExtendConfig表中“循环贷”的actionGroup
        /// </summary>
        public static List<string> APPExtendConfig_Circle
        {
            get
            {
                return GetValue("APPExtendConfig_Circle").Split(',').ToList();
            }
        }

        /// <summary>
        /// 需要扩展的订单状态（展期）
        /// </summary>
        public static Dictionary<string, string> NeedExtendStatus_Extend
        {
            get
            {
                return SplitStandardFormatConfig("NeedExtendStatus_Extend");
            }
        }

        /// <summary>
        /// 需要扩展的订单状态（循环贷）
        /// </summary>
        public static Dictionary<string, string> NeedExtendStatus_Circle
        {
            get
            {
                return SplitStandardFormatConfig("NeedExtendStatus_Circle");
            }
        }
        /// <summary>
        /// GPS购车评估状态
        /// </summary>
        public static Dictionary<string, string> AssessQueueStatus
        {
            get
            {
                return SplitStandardFormatConfig("AssessQueueStatus");
            }
        }
        
        /// <summary>
        /// 菜单中URL使用key,用于区分信用贷和车贷等
        /// </summary>
        public static Dictionary<string, List<string>> LogoGroupForMenu
        {
            get
            {
                Dictionary<string, List<string>> dic = new Dictionary<string, List<string>>();
                string strStatus = GetValue("LogoGroupForMenu");
                string[] temp = strStatus.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var item in temp)
                {
                    string[] ary = item.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
                    if (ary.Length > 1)
                    {
                        string[] ary2 = ary[1].Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                        dic.Add(ary[0], ary2.ToList());
                    }
                }
                return dic;
            }
        }

        public static string[] GetLogo(string key)
        {
            List<string> listDic = new List<string>();
            listDic = LogoGroupForMenu[key];
            return listDic.ToArray();
        }
        /// <summary>
        /// 产品和可选购车种类的对应关系
        /// </summary>
        public static Dictionary<string, string> LogoAndCarKindRelation
        {
            get
            {
                return SplitStandardFormatConfig("LogoAndCarKindRelation");
            }
        }

        /// <summary>
        /// 需要评估的车贷logo
        /// </summary>
        public static List<string> NeedAssessProductLogo
        {
            get
            {
                return GetValue("NeedAssessProductLogo").Split(',').ToList();
            }
        }

        /// <summary>
        /// 评估取消队列
        /// </summary>
        public static string CAR_Assess_Cancelled
        {
            get
            {
                return GetValue("CAR_Assess_Cancelled");
            }
        }

        /// <summary>
        /// 评估完成队列
        /// </summary>
        public static string CAR_Assess_Done
        {
            get
            {
                return GetValue("CAR_Assess_Done");
            }
        }

        /// <summary>
        /// 车贷补件表单版本后缀
        /// </summary>
        public static string DFormVersion_Suffix_NR
        {
            get
            {
                return GetValue("DFormVersion_Suffix_NR");
            }
        }

        /// <summary>
        /// 展期最后期限距还款日天数（工作日）
        /// </summary>
        public static string BackLoanDay
        {
            get
            {
                return GetValue("BackLoanDay");
            }
        }

        /// <summary>
        /// 车辆贷款比例（单位：%）
        /// </summary>
        public static string CarLoanRatio
        {
            get
            {
                return GetValue("CarLoanRatio");
            }
        }
        #endregion

        #region 10.从预申请信息进件时字段间映射配置

        /// <summary>
        /// 申请系统从预申请单子进件时Customer对象与PreCustomer对象间字段的映射关系
        /// </summary>
        public static Dictionary<string, string> CustomerToPreCustomerMapping
        {
            get
            {
                return SplitStandardFormatConfig("CustomerToPreCustomerMapping");
            }
        }

        /// <summary>
        /// 申请系统从预申请单子进件时Job对象与PreJob对象间字段的映射关系
        /// </summary>
        public static Dictionary<string, string> JobToPreJobMapping
        {
            get
            {
                return SplitStandardFormatConfig("JobToPreJobMapping");
            }
        }

        /// <summary>
        /// 申请系统从预申请单子进件时Loan对象与PreLoan对象间字段的映射关系
        /// </summary>
        public static Dictionary<string, string> LoanToPreLoanMapping
        {
            get
            {
                return SplitStandardFormatConfig("LoanToPreLoanMapping");
            }
        }

        /// <summary>
        /// 申请系统从预申请单子进件时Bankcard对象与PreBankcard对象间字段的映射关系
        /// </summary>
        public static Dictionary<string, string> BankcardToPreBankcardMapping
        {
            get
            {
                return SplitStandardFormatConfig("BankcardToPreBankcardMapping");
            }
        }

        /// <summary>
        /// 申请系统从预申请单子进件时StaffOnly对象与PreStaffOnly对象间字段的映射关系
        /// </summary>
        public static Dictionary<string, string> StaffOnlyToPreStaffOnlyMapping
        {
            get
            {
                return SplitStandardFormatConfig("StaffOnlyToPreStaffOnlyMapping");
            }
        }

        /// <summary>
        /// 预申请管理中多少天内的预申请能进件
        /// </summary>
        public static int PreQappForApplyDays
        {
            get
            {
                int OutInt;
                var StrValue = GetValue("PreQappForApplyDays");
                if (!int.TryParse(StrValue, out OutInt))
                    OutInt = 30;
                return OutInt;
            }
        }

        /// <summary>
        /// 从预申请进件时的请求处理时限（单位：秒）默认为600
        /// </summary>
        public static int PreProcessingTime
        {
            get
            {
                int intResult;
                var strValue = GetValue("PreProcessingTime");
                if (!int.TryParse(strValue, out intResult))
                    intResult = 600;
                return intResult;
            }
        }

        /// <summary>
        /// 预申请入口进件对应的申请表单版本后缀
        /// </summary>
        public static string DFormVersion_Suffix_Pre
        {
            get
            {
                var preStr = GetValue("DFormVersion_Suffix_Pre");
                if (string.IsNullOrEmpty(preStr))
                    preStr = "_PRE";
                return preStr;
            }
        }
        #endregion

        #region 11.房贷配置

        /// <summary>
        /// 房贷申请抵押率最大值（%）
        /// </summary>
        public static string HouseLoanRatio
        {
            get
            {
                return GetValue("HouseLoanRatio");
            }
        }

        /// <summary>
        /// 房贷待补件列表
        /// </summary>
        public static Dictionary<string, string> Order_SD_Status_Need_House
        {
            get
            {
                return SplitStandardFormatConfig("Order_SD_Status_Need_House");
            }
        }

        /// <summary>
        /// 房贷已补件列表
        /// </summary>
        public static Dictionary<string, string> Order_SD_Status_Had_House
        {
            get
            {
                return SplitStandardFormatConfig("Order_SD_Status_Had_House");
            }
        }

        /// <summary>
        /// 房贷补件完成队列
        /// </summary>
        public static string MQ_House_Application_NR_Done
        {
            get
            {
                return GetValue("HOUSE_APPLICATION_NR_DONE");
            }
        }

        /// <summary>
        /// 房贷待补件队列
        /// </summary>
        public static string MQ_House_Application_NR
        {
            get
            {
                return GetValue("HOUSE_APPLICATION_NR");
            }
        }

        public static Dictionary<string, string> Order_ExceptSD_Status_House
        {
            get
            {
                return SplitStandardFormatConfig("Order_ExceptSD_Status_House");
            }
        }

        /// <summary>
        /// 房贷申请logos
        /// </summary>
        public static string[] HouseLogos
        {
            get
            {
                return LogoGroupForMenu["HOUSE"].ToArray();
            }
        }

        /// <summary>
        /// 房贷补件队列侦听控制
        /// </summary>
        public static string House_NR_Listener_Enable
        {
            get
            {
                return GetValue("House_NR_Listener_Enable");
            }
        }

        /// <summary>
        /// 房贷展期可以在到期日前多少个自然日提出申请
        /// </summary>
        public static string BackLoanDayHouse
        {
            get { return GetValue("BackLoanDayHouse"); }
        }

        /// <summary>
        /// 房贷补件有效天数
        /// </summary>
        public static int Order_SD_AbidanceDay_House
        {
            get
            {
                string strDays = GetValue("Order_SD_AbidanceDay_House");
                int intDays = 20;
                int.TryParse(strDays, out intDays);
                return intDays;
            }
        }

        #endregion

        #region 12.系统配置相关

        public static string Platform
        {
            get
            {
                return GetValueForWebConfig("Platform");
            }
        }

        public static string ServerName
        {
            get
            {
                return GetValueForWebConfig("ServerName");
            }
        }

        /// <summary>
        /// 标的自动任务任务类型 
        /// </summary>
        public static Dictionary<string, string> Bid_AuotJob_Type
        {
            get
            {
                return SplitStandardFormatConfig("Bid_AuotJob_Type");
            }
        }
        #endregion

        #region 13.标的额度相关配置
        /// <summary>
        /// 标的额度接口服务地址
        /// </summary>
        public static string QBApiHost
        {
            get
            {
                return GetValue("QBApiHost");
            }
        }
        /// <summary>
        /// 签名私钥
        /// </summary>
        public static string QbPartner
        {
            get
            {
                return GetValue("QbPartner");
            }
        }
        /// <summary>
        /// 签名公钥
        /// </summary>
        public static string QbPartnerKey
        {
            get
            {
                return GetValue("QbPartnerKey");
            }
        }
        /// <summary>
        /// 标的未发标、未挂标状态
        /// </summary>
        public static Dictionary<string, string> Bid_Step_NotSendBid
        {
            get
            {
                return SplitStandardFormatConfig("Bid_Step_NotSendBid");
            }
        }
        /// <summary>
        /// 标的在凑标、已挂标状态
        /// </summary>
        public static Dictionary<string, string> Bid_Step_CollectBid
        {
            get
            {
                return SplitStandardFormatConfig("Bid_Step_CollectBid");
            }
        }
        /// <summary>
        /// 标的已满标状态
        /// </summary>
        public static Dictionary<string, string> Bid_Step_FullBid
        {
            get
            {
                return SplitStandardFormatConfig("Bid_Step_FullBid");
            }
        }
        /// <summary>
        /// 标的已流标状态
        /// </summary>
        public static Dictionary<string, string> Bid_Step_FailBid
        {
            get
            {
                return SplitStandardFormatConfig("Bid_Step_FailBid");
            }
        }

        /// <summary>
        /// 是否显示风险补偿金
        /// </summary>
        public static string IsShowLoanLossProvision
        {
            get {
                return GetValue("IsShowLoanLossProvision");
            }
        }
        #endregion

        #region 14.合同相关配置
        /// <summary>
        /// 标的调用合同系统的接入id
        /// </summary>
        public static string Contract_APP_ID
        {
            get
            {
                return GetValue("Contract_APP_ID");
            }
        }
        /// <summary>
        /// 调用合同系统请求参数BIZ_KEY的固定值
        /// </summary>
        public static string Contract_BIZ_ID
        {
            get
            {
                return GetValue("Contract_BIZ_ID");
            }
        }
        /// <summary>
        /// 调用合同系统请求参数BIZ_KEY的固定值
        /// </summary>
        public static string Contract_ID
        {
            get
            {
                return GetValue("Contract_ID");
            }
        }
        /// <summary>
        /// RA_CODE：RA（Registration Authority， 数字证书注册机构）的代码，默认值FDD
        /// </summary>
        public static string RA_CODE
        {
            get
            {
                return GetValue("RA_CODE");
            }
        }
        /// <summary>
        /// 试算接口地址
        /// </summary>
        public static string CalcURL
        {
            get
            {
                return GetValue("CalcURL");
            }
        }
        /// <summary>
        /// 试算版本
        /// </summary>
        public static string CalcVersion
        {
            get
            {
                return GetValue("CalcVersion");
            }
        }
        /// <summary>
        /// 合同接口url地址
        /// </summary>
        public static string ContractUrl
        {
            get
            {
                return GetValue("ContractUrl");
            }
        }
        /// <summary>
        /// 合同查看接口ACTION参数
        /// </summary>
        public static string Contract_CONT_VIEW
        {
            get
            {
                return GetValue("Contract_CONT_VIEW");
            }
        }
        /// <summary>
        /// 合同上传接口ACTION参数
        /// </summary>
        public static string Contract_CONT_UPLOAD
        {
            get
            {
                return GetValue("Contract_CONT_UPLOAD");
            }
        }
        /// <summary>
        /// 合同下载接口ACTION参数
        /// </summary>
        public static string Contract_CONT_DOWNLOAD
        {
            get
            {
                return GetValue("Contract_CONT_DOWNLOAD");
            }
        }
        /// <summary>
        /// 合同删除接口ACTION参数
        /// </summary>
        public static string Contract_CONT_DELETE
        {
            get
            {
                return GetValue("Contract_CONT_DELETE");
            }
        }
        /// <summary>
        /// 合同生成接口ACTION参数
        /// </summary>
        public static string Contract_CONT_CREATE
        {
            get
            {
                return GetValue("Contract_CONT_CREATE");
            }
        }
        
         /// <summary>
        /// 合同手动签章接口ACTION参数
        /// </summary>
        public static string Contract_MANUAL_SIGN
        {
            get
            {
                return GetValue("Contract_MANUAL_SIGN");
            }
        }

        /// <summary>
        /// 合同手动签章URL更新接口 ACTION参数
        /// </summary>
        public static string Contract_UPDATE_MANUAL_SIGN_URL
        {
            get {
                return GetValue("Contract_UPDATE_MANUAL_SIGN_URL");
            }
        }

        /// <summary>
        ///P2P FOTIC - 外贸信托 AVICTC - 中航信托 资金渠道和合同模板的对应关系
        /// </summary>
        public static Dictionary<string,List<string>>  Contract_FundChannel
        {
            get
            {
                return SplitStandardFormatConfigList("Contract_FundChannel");
            }
        }
        /// <summary>
        ///渠道和信托机构代码对应关系
        /// </summary>
        public static Dictionary<string, List<string>> Contract_Channel_Trust
        {
            get
            {
                return SplitStandardFormatConfigList("Contract_Channel_Trust");
            }
        }
        /// <summary>
        /// 渠道合同模板品牌的对应关系KEY值
        /// </summary>
        public static string Contract_BRAND_CODE
        {
            get
            {
                return GetValue("Contract_BRAND_CODE");
            }
        }
        /// <summary>
        /// 渠道合同模板资金渠道的对应关系品牌KEY值
        /// </summary>
        public static string Contract_CHANNEL_CODE
        {
            get
            {
                return GetValue("Contract_CHANNEL_CODE");
            }
        }
        /// <summary>
        /// 合同打印显示的类型 E_MANUAL-手动电子签章,P_MANUAL-手动签字,P_NON_SEAL-空的签章
        /// </summary>
        public static string Contract_PRINT_TYPE
        {
            get
            {
                return GetValue("Contract_PRINT_TYPE");
            }
        }
        /// <summary>
        /// 是否每次重新生成合同：FALSE,TRUE
        /// </summary>
        public static string CONTRACT_IS_CREATE
        {
            get
            {
                return GetValue("CONTRACT_IS_CREATE");
            }
        }
        /// <summary>
        /// GPS安装费
        /// </summary>
        public static string INST_FEE_GPS
        {
            get
            {
                return GetValue("INST_FEE_GPS");
            }
        }
        /// <summary>
        /// GPS服务费
        /// </summary>
        public static string SERV_FEE_GPS
        {
            get
            {
                return GetValue("SERV_FEE_GPS");
            }
        }
        /// <summary>
        /// 中航房贷14：00之前起息日T、放款日T--14：00之后起息日T+1、放款日T+1
        /// </summary>
        public static Dictionary<string,string> Contract_T2P_ZH_FD
        {
            get
            {
                return SplitStandardFormatConfigDic("Contract_T2P_ZH_FD",'=');
            }
        }
        /// <summary>
        /// 起息日T+1、放款日T+1额度类型配置
        /// </summary>
        public static List<string> Contract_T1_QB_AMT_CODE
        {
            get
            {
                string QB_AMT_CODE = GetValue("Contract_T1_QB_AMT_CODE");
                return QB_AMT_CODE.Split(',').ToList();
            }
        }
        /// <summary>
        ///控制协议详情页面（PACTDETAIL）、协议上传页面（PACTUPLOAD）、协议确认页面（PACTCONFIRM）能够操作的协议状态。
        /// </summary>
        public static Dictionary<string, List<string>> PACTDETAIL_ORDER_STATUS
        {
            get
            {
                return SplitStandardFormatConfigList("PACTDETAIL_ORDER_STATUS");
            }
        }
        /// <summary>
        ///控制协议编辑状态 1：上传协议 2：编辑协议 3：查看协议
        /// </summary>
        public static Dictionary<string, List<string>> PACTDETAIL_OPERATION_STATUS
        {
            get
            {
                return SplitStandardFormatConfigList("PACTDETAIL_OPERATION_STATUS");
            }
        }
        #endregion

        #region  15.合同文件上传、下载相关配置
        /// <summary>
        /// 文件读取接口地址
        /// </summary>
        public static string Con_FileReadUrl
        {
            get
            {
                return GetValue("Con_FileReadUrl");
            }
        }
        /// <summary>
        /// 文件上传格式
        /// </summary>
        public static string Con_UploadFileFormat
        {
            get
            {
                return GetValue("Con_UploadFileFormat");
            }
        }

        /// <summary>
        /// 允许上传的最大文件（单位：KB）
        /// </summary>
        public static int Con_UploadMaxSize
        {
            get
            {
                string strSize = GetValue("Con_UploadMaxSize");
                int intSize = 10240;
                int.TryParse(strSize, out intSize);
                return intSize;
            }
        }

        /// <summary>
        /// 分块上传时每块大小（单位：KB）
        /// </summary>
        public static int Con_UploadChunkSize
        {
            get
            {
                string strSize = GetValue("Con_UploadChunkSize");
                int intSize = 5120;
                int.TryParse(strSize, out intSize);
                return intSize;
            }
        }

        /// <summary>
        /// 允许下载的最大文件（单位：KB）
        /// </summary>
        public static int Con_DownloadMaxSize
        {
            get
            {
                string strSize = GetValue("Con_DownloadMaxSize");
                int intSize = 10240;
                int.TryParse(strSize, out intSize);
                return intSize;
            }
        }

        /// <summary>
        /// 分块下载时每块大小（单位：KB）
        /// </summary>
        public static int Con_DownloadChunkSize
        {
            get
            {
                string strSize = GetValue("Con_DownloadChunkSize");
                int intSize = 5120;
                int.TryParse(strSize, out intSize);
                return intSize;
            }
        }

        /// <summary>
        /// JavaScriptSerializer的最大MaxJsonLength
        /// </summary>
        public static int Con_MaxJsonLength
        {
            get
            {
                string strSize = GetValue("Con_MaxJsonLength");
                int intSize = 5120;
                int.TryParse(strSize, out intSize);
                return intSize;
            }
        }

        public static string[] GlimpseAccounts
        {
            get
            {
                var accountStr = GetValue("GlimpseAccounts");
                if (string.IsNullOrEmpty(accountStr))
                    return new string[] { };
                return accountStr
                    .Split(new string[] { ";" }, System.StringSplitOptions.RemoveEmptyEntries)
                    .ToArray();
            }
        }

        /// <summary>
        /// 应用程序ID（默认：QUARK_QAPP）
        /// </summary>
        public static string QappAppId
        {
            get
            {
                string str = GetValue("QappAppId");
                if (string.IsNullOrEmpty(str))
                    str = "QUARK_QAPP";
                return str;
            }
        }

        /// <summary>
        /// 银行卡类型（默认：DR）
        /// </summary>
        public static string BankCardType
        {
            get
            {
                string str = GetValue("BankCardType");
                if (string.IsNullOrEmpty(str))
                    str = "DR";
                return str;
            }
        }

        /// <summary>
        /// 验证渠道（默认：CMBC）
        /// </summary>
        public static string VerifyChannel
        {
            get
            {
                string str = GetValue("VerifyChannel");
                if (string.IsNullOrEmpty(str))
                    str = "CMBC";
                return str;
            }
        }

        /// <summary>
        /// 车辆经销商是否全部显示控制（默认：true）
        /// </summary>
        public static bool ShowAllAgencyOfCar
        {
            get
            {
                bool f = true;
                string str = GetValue("ShowAllAgencyOfCar");
                if (!string.IsNullOrEmpty(str) && str.ToLower() == "false")
                    f = false;
                return f;
            }
        }
        #endregion

        #region 13.融誉100

        /// <summary>
        /// 融誉100待补件
        /// </summary>
        public static Dictionary<string, string> Order_SD_Status_Need_Ry100
        {
            get { return SplitStandardFormatConfig("Order_SD_Status_Need_Ry100"); }
        }

        /// <summary>
        /// 融誉100已补件
        /// </summary>
        public static Dictionary<string, string> Order_SD_Status_Had_Ry100
        {
            get { return SplitStandardFormatConfig("Order_SD_Status_Had_Ry100"); }
        }

        /// <summary>
        /// 融誉100补件失效
        /// </summary>
        public static Dictionary<string, string> Order_SD_Status_Cancel_Ry100
        {
            get { return SplitStandardFormatConfig("Order_SD_Status_Cancel_Ry100"); }
        }

        #endregion

        #region 16.试算接口配置相关
        public static string ContarctApiUrl
        {
            get
            {
                return GetValue("ContarctApiUrl");
            }
        }
        #endregion

        #region 17.提示消息相关配置
        public static string AuditMessageContent
        {
            get
            {
                return GetValue("AuditMessageContent");
            }
        }
        #endregion
    }
}
