using System.Linq;
using System.Web;
using log4net;
using Newtonsoft.Json;
using QK.QAPP.Entity;
using System;

namespace QK.QAPP.Infrastructure.Log4Net
{
    /// <summary>
    /// 系统日志
    /// </summary>
    public static class LogWriter
    {
        private static readonly ILog SysLoger = LogManager.GetLogger("logsys");
        private static readonly ILog BizLoger = LogManager.GetLogger("logbiz");
        private static string SessionUser = Global.GlobalSetting.AuthSaveKey;// "SESSION_USER";



        #region 业务日志
        /// <summary>
        /// 业务日志写入
        /// </summary>
        /// <param name="description">业务日志描述</param>
        public static void Biz(string description)
        {
            Biz(description, "", new object());
        }

        /// <summary>
        /// 业务日志写入
        /// </summary>
        /// <param name="description">业务日志描述</param>
        /// <param name="key">业务日志主键（业务实体的主键或者重要对象主键方便查询和跟踪）</param>
        public static void Biz(string description, string key)
        {
            Biz(description, key, new object());
        }

        /// <summary>
        /// 业务日志写入
        /// </summary>
        /// <param name="description">业务日志描述</param>
        /// <param name="entity">业务日志实体</param>
        public static void Biz(string description, object entity)
        {
            Biz(description, "", entity);
        }
        /// <summary>
        /// 业务日志写入
        /// </summary>
        /// <param name="description">业务日志描述</param>
        /// <param name="key">业务日志主键（业务实体的主键或者重要对象主键方便查询和跟踪）</param>
        /// <param name="entity">业务日志实体</param>
        public static void Biz(string description, string key, object entity)
        {
            var rq = HttpContext.Current;
            string userName;    //用户姓名
            string userId;      //用户账户或机器名
            string clientIp;    //客户端IP

            GetUserPara(out userName, out  userId, out  clientIp);
            var log = new BizLog
            {
                Description = description,
                Entity = ConvertToJson(entity),
                Function = rq.Request.Url + "",
                Key = key,
                UserName = userName,
                UserId = userId,
                Machine = clientIp
            };
            BizLoger.Info(log);
        }
        #endregion

        #region 不同级别的系统日志
        public static void Warn(string description)
        {
            var sysEnity = InitSysLogEntity(description);
            SysLoger.Warn(sysEnity);
        }
        public static void Warn(string description, Exception exception)
        {
            var sysEnity = InitSysLogEntity(description);
            SysLoger.Warn(sysEnity, exception);
        }
        public static void Debug(string description)
        {
            var sysEnity = InitSysLogEntity(description);
            SysLoger.Debug(sysEnity);
        }
        public static void Debug(string description, Exception exception)
        {
            var sysEnity = InitSysLogEntity(description);
            SysLoger.Debug(sysEnity, exception);
        }
        public static void Error(string description)
        {
            var sysEnity = InitSysLogEntity(description);
            SysLoger.Error(sysEnity);
        }
        public static void Error(string description, Exception exception)
        {
            var sysEnity = InitSysLogEntity(description);
            SysLoger.Error(sysEnity, exception);
        }
        public static void Info(string description)
        {
            var sysEnity = InitSysLogEntity(description);
            SysLoger.Info(sysEnity);
        }
        public static void Info(string description, Exception exception)
        {
            var sysEnity = InitSysLogEntity(description);
            SysLoger.Info(sysEnity, exception);
        }
        public static void Fatal(string description)
        {
            var sysEnity = InitSysLogEntity(description);
            SysLoger.Fatal(sysEnity);
        }
        public static void Fatal(string description, Exception exception)
        {
            var sysEnity = InitSysLogEntity(description);
            SysLoger.Fatal(sysEnity, exception);
        }
        #endregion

        #region 私有函数
        /// <summary>
        /// 获取用户参数
        /// </summary>
        /// <param name="userName">用户姓名</param>
        /// <param name="userId">用户ID（如果取不到用户ID，则为客户端机器名）</param>
        /// <param name="clientMachineIp">客户端IP</param>
        private static void GetUserPara(out string userName, out string userId, out string clientMachineName)
        {
            try
            {
                var rq = HttpContext.Current;
                //计算客户端IP
                clientMachineName = "未知服务器名";
                if (rq.Request.UserHostAddress != null)
                {
                    clientMachineName = rq.Request.UserHostAddress;
                }

                

                //计算用户姓名和用户ID
                userName = "unknown user";
                userId = clientMachineName;
                try
                {
                    var user = rq.Session[SessionUser] as QFUser;
                    if (user != null)
                    {
                        userName = user.RealName;
                        userId = user.Account;
                    }
                }
                catch { }
            }
            catch
            {
                userName = "未知用户姓名";
                userId = "未知用户账户";
                clientMachineName = "未知服务器名";
            }
        }

        /// <summary>
        /// 将实体幻化为JSON
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        private static string ConvertToJson(object entity)
        {
            var jsonStr = JsonConvert.SerializeObject(entity, new JsonSerializerSettings
            {
                //这句是解决问题的关键,也就是json.net官方给出的解决配置选项.                 
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
            return jsonStr;
        }

        /// <summary>
        /// 获取系统日志实体
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        private static SysLog InitSysLogEntity(string description)
        {
            string userName;    //用户姓名
            string userId;      //用户账户或机器名
            string clientIp;    //客户端IP

            GetUserPara(out userName, out  userId, out  clientIp);
            
            return new SysLog
            {
                Description = description,
                UserId = userId,
                UserName = userName,
                Host = clientIp
            };
        }

        #endregion
        #region  
        /// <summary>
        /// 业务日志写入
        /// </summary>
        /// <param name="description">业务日志描述</param>
        /// <param name="key">业务日志主键（业务实体的主键或者重要对象主键方便查询和跟踪）</param>
        /// <param name="user">用户信息</param>
        public static void Biz(string description, string key, QFUser user)
        {
            Biz(description, key, new object(), user);
        }
        /// <summary>
        /// 业务日志写入
        /// </summary>
        /// <param name="description">业务日志描述</param>
        /// <param name="key">业务日志主键（业务实体的主键或者重要对象主键方便查询和跟踪）</param>
        /// <param name="entity">业务日志实体</param>
        public static void Biz(string description, string key, object entity, QFUser user)
        {
            string userName = user.RealName == null ? "未知用户姓名" : user.RealName;    //用户姓名
            string userId = user.Account == null ? "未知用户账户" : user.Account;      //用户账户或机器名
            string clientIp = "未知服务器名";    //客户端IP
            var log = new BizLog
            {
                Description = description,
                Entity = ConvertToJson(entity),
                Function = "",
                Key = key,
                UserName = userName,
                UserId = userId,
                Machine = clientIp
            };
            BizLoger.Info(log);
        }
        /// <summary>
        /// 异常日志写入
        /// </summary>
        /// <param name="description">描述信息</param>
        /// <param name="exception">异常信息</param>
        /// <param name="user">用户信息</param>
        public static void Error(string description, Exception exception, QFUser user)
        {
            var sysEnity = InitSysLogEntity(description, user);
            SysLoger.Error(sysEnity, exception);
        }
        /// <summary>
        /// 获取系统日志实体
        /// </summary>
        /// <param name="description"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        private static SysLog InitSysLogEntity(string description, QFUser user)
        {
            string userName = user.RealName == null ? "未知用户姓名" : user.RealName;    //用户姓名
            string userId = user.Account == null ? "未知用户账户" : user.Account;      //用户账户或机器名
            string clientIp = "未知服务器名";    //客户端IP
            return new SysLog
            {
                Description = description,
                UserId = userId,
                UserName = userName,
                Host = clientIp
            };
        }
        #endregion
    }
}
