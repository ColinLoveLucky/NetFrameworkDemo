﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QK.QAPP.Infrastructure.Captcha
{
    public class Config
    {
        public static string SessionKey
        {
            get { return "Captcha"; }
        }

        public static string InputName
        {
            get { return "Captcha"; }
        }
    }
}
