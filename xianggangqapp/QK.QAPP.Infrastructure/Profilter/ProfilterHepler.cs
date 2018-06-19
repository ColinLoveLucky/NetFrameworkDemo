using StackExchange.Profiling;
using StackExchange.Profiling.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QK.QAPP.Infrastructure
{
    public class ProfilterHepler
    {
        public static void Init()
        {
            //MiniProfilter 性能监听
            MiniProfiler.Settings.SqlFormatter = new StackExchange.Profiling.SqlFormatters.SqlServerFormatter();
            MiniProfilerEF.InitializeEF42();
            //ORM Entity Framework Profilter 监测工具监听
            //HibernatingRhinos.Profiler.Appender.EntityFramework.EntityFrameworkProfiler.Initialize();           

            //MiniProfiler.Settings.Results_List_Authorize = IsUserAllowedToSeeMiniProfilerUI;
        }
    }
}
