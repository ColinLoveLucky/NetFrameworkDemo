using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QK.QAPP.Entity;
using QK.QAPP.Global;
using QK.QAPP.Infrastructure;
using QK.QAPP.Infrastructure.Log4Net;
using QK.QAPP.IServices;

namespace QK.QAPP.Services
{
    public class GenesisService : IGenesisService
    {
        public IAPP_MAINSERVICE AppMainService { get; set; }
        public string GetPbocStatusFromApi(string preAppCode)
        {
            var obj = new GenesisResult();
            var restHelper = new RestApiHelper(GlobalSetting.PbocUrl);
            try
            {
                var sw = new Stopwatch();
                sw.Start();
                obj = restHelper.Get<GenesisResult>(string.Empty, new Dictionary<string, string>() 
                {
                    {"preappcode", preAppCode}
                });
                sw.Stop();
                LogWriter
                    .Biz(string.Format("Pboc认证接口调用成功！接口耗时{0}", sw.ElapsedMilliseconds), preAppCode, obj);
            }
            catch (Exception ex)
            {
                LogWriter.Error("Pboc认证接口调用失败！", ex);
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

        public string GetNetbankStatusFromApi(string preAppCode)
        {
            var obj = new GenesisResult();
            var restHelper = new RestApiHelper(GlobalSetting.NetbankUrl);
            try
            {
                var sw = new Stopwatch();
                sw.Start();
                obj = restHelper.Get<GenesisResult>(string.Empty, new Dictionary<string, string>()
                {
                    {"preappcode", preAppCode}
                });
                sw.Stop();
                LogWriter
                    .Biz(string.Format("网银认证接口调用成功！接口耗时{0}", sw.ElapsedMilliseconds), preAppCode, obj);
            }
            catch (Exception ex)
            {
                LogWriter.Error("网银认证接口调用失败！", ex);
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

        public string GetFundStatusFromApi(string preAppCode)
        {
            var obj = new GenesisResult();
            var restHelper = new RestApiHelper(GlobalSetting.FundUrl);
            try
            {
                var sw = new Stopwatch();
                sw.Start();
                obj = restHelper.Get<GenesisResult>(string.Empty, new Dictionary<string, string>()
                {
                    {"preappcode", preAppCode}
                });
                sw.Stop();
                LogWriter
                    .Biz(string.Format("公积金认证接口调用成功！接口耗时{0}", sw.ElapsedMilliseconds), preAppCode, obj);
            }
            catch (Exception ex)
            {
                LogWriter.Error("公积金认证接口调用失败！", ex);
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

        public GenesisStatusEntity GetGenesisStatus(long appId)
        {
            var result = new GenesisStatusEntity();
            var appMain = AppMainService.FirstOrDefault(a => a.ID == appId);
            if (appMain != null)
            {
                result.FundStatus = appMain.FUND_STATUS;
                result.PbocStatus = appMain.PBOC_STATUS;
                result.MobileStatus = appMain.MOBILE_HISTORY_STATUS;
                result.NetbankStatus = appMain.NETBANK_STATUS;
            }

            return result;
        }
    }
}
