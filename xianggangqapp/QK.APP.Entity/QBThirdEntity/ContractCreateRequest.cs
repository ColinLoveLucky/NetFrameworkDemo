using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QK.QAPP.Entity
{
    /// <summary>
    /// 合同生成申请参数类
    /// </summary>
    public class ContractCreateRequest
    {
        /// <summary>
        /// Y	{"APP_ID":""}，接入系统ID
        /// </summary>
        public string APP_ID { get; set; }
        /// <summary>
        /// Y	{"ACTION":""}，值为“CONT_CREATE”
        /// </summary>
        public string ACTION { get; set; }
        /// <summary>
        /// Y	{" IS_SYNC" : ""}，是否同步调用，值为Y或N。
        /// 如果同步调用，申请系统需要等待，直到合同管理平台给出响应，或超时；
        ///如果异步调用，请求将会放入队列，按照FIFO（First In First Out，先入先出）的原则进行处理。
        ///处理结束后，调用请求参数NOTIFY_URL指定的回调接口通知调用系统。
        ///默认同步调用
        /// </summary>
        public string IS_SYNC { get; set; }
        /// <summary>
        /// N	异步请求处理结束后的回调接口。IS_SYNC=Y时必填
        /// </summary>
        public string NOTIFY_URL { get; set; }

        /// <summary>
        /// Y	{"SIGN_TYPE":""}，用户电子签章为手动或自动，值为AUTO（自动）或MANUAL（手动）。
        /// 手动签章，会在公司电子签章后，返回三方提供的页面签章URL，供接入系统展示给用户进行手动签章。
        /// 自动签章，会通过合同管理平台存储的用户在RA机构的信息，调用自动电子签章。
        /// </summary>
        public string SIGN_TYPE { get; set; }
        public CA_INFO CA_INFO { get; set; }

        /// <summary>
        /// 业务和合同模板的关联关系。
        /// 接入系统需要提前和合同管理平台约定“业务和合同模板的关联关系”。
        /// </summary>
        public List<BIZ_KEY_VAL> BIZ_TEMP_MAP { get; set; }
        public BIZ_INFO BIZ_INFO { get; set; }
    }
   
}
