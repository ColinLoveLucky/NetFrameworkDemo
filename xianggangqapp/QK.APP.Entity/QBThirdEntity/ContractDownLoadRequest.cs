﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QK.QAPP.Entity
{
    /// <summary>
    /// 文件下载请求参数
    /// </summary>
    public class ContractDownLoadRequest
    {
        /// <summary>
        /// 接入系统ID
        /// </summary>
        public string APP_ID { get; set; }
        /// <summary>
        /// 值为CONT_DOWNLOAD
        /// </summary>
        public string ACTION { get; set; }
        public _BIZ_INFO BIZ_INFO { get; set; }
        public class _BIZ_INFO
        {
            /// <summary>
            /// 申请的业务基本信息
            /// </summary>
            public List<BIZ_KEY_VAL> BASE_INFO { get; set; }
        }

    }
}