using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Caching;
using Microsoft.Practices.Unity;
using QK.QAPP.Entity;
using QK.QAPP.Global;
using QK.QAPP.Infrastructure;
using QK.QAPP.IServices;

namespace QK.QAPP.Services
{
    public class MobileHistoryService : IMobileHistoryService
    {
        [Dependency]
        public IAPP_MAINSERVICE AppMainService { get; set; }

        public string GetStatusFormApi(string preAppCode)
        {
            var obj = new GenesisResult();
            var restHelper = new RestApiHelper(GlobalSetting.MobileHistoryUrl);
            try
            {
                var sw = new Stopwatch();
                sw.Start();
                obj = restHelper.Get<GenesisResult>(string.Empty, new Dictionary<string, string>()
                {
                    {"preappcode", preAppCode}
                });
                sw.Stop();
                Infrastructure.Log4Net.LogWriter
                    .Biz(string.Format("通话详单接口调用成功！接口耗时{0}", sw.ElapsedMilliseconds), preAppCode, obj);
            }
            catch (Exception ex)
            {
                Infrastructure.Log4Net.LogWriter.Error("通话详单状态接口调用失败！", ex);
            }

            if (obj != null)
            {
                return obj.code.ToString();
            }
            else
            {
                return "500";
            }
        }

        public string GetStatus(long appId)
        {
            var appMain = AppMainService.FirstOrDefault(a => a.ID == appId);
            if (appMain != null)
            {
                return appMain.MOBILE_HISTORY_STATUS;
            }

            return String.Empty;
        }
    }
}
