using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using QK.QAPP.Infrastructure.Log4Net;

namespace QK.QAPP.Infrastructure
{
    public class Ioc
    {
        public static T GetService<T>()
        {
            T ret = default(T);
            try
            {
                ret = DependencyResolver.Current.GetService<T>();
            }
            catch (Exception ex)
            {
                //System.Diagnostics.Trace.WriteLine(ex,"error");
                LogWriter.Error("Ioc注入出错<" + typeof(T).Name+">", ex);
            }
            return ret;
        }
    }
}
