using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QK.QAPP.Infrastructure
{
    public interface ILogger
    {
        void LogError(object message);
        void LogInfo(object message);        
    }
}
