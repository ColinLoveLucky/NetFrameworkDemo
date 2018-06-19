﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Hosting;

namespace QK.QAPP.Infrastructure
{
    public class Logger
    {
        private static readonly ILogger _errorLogger;

        static Logger()
        {
            _errorLogger = new CommonLogger();
        }

        public static void Error(object message)
        {
            _errorLogger.LogError(message);
        }

        public static void Info(object message)
        {
            _errorLogger.LogInfo(message);
        }

    }
}
