using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace QK.QAPP.Infrastructure
{
    public abstract class QKWebViewPage<T> : WebViewPage<T>
    {
        public override void Write(object value)
        {
            if (value is string)
            {
                WriteLiteral(value);
            }
            else
            {
                base.Write(value);
            }
        }
    }

    public abstract class QKWebViewPage : WebViewPage
    {
        public override void Write(object value)
        {
            if (value is string)
            {
                WriteLiteral(value);
            }
            else
            {
                base.Write(value);
            }
        }
    }
}
