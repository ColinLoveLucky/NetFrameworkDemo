using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QK.QAPP.Entity
{
   public class ContractCreateRequest
    {
       /// <summary>
        /// {"ACTION" : "CREATE"}，申请生成新合同
        /// 是否必填:Y
       /// </summary>
       public string ACTION { get; set; }
       /// <summary>
       /// {" IS_SYNC " : "Y"}，是否同步调用。
        ///如果同步调用，申请系统需要等待，直到合同管理平台给出响应，或超时；
        ///如果异步调用，请求将会放入队列。 合同管理平台会按照FIFO（First In First Out，先入先出）的原则，对所有异步调用的申请进行处理。处理结束后，调用请求参数CALLBACK_URL指定的回调接口通知调用系统。
        ///是否必填:Y
       /// </summary>
       public string IS_SYNC { get; set; }
       /// <summary>
       /// 异步请求处理结束后的回调接口IS_SYNC=Y时必填
       /// 是否必填:N
       /// </summary>
       public string CALLBACK_URL { get; set; }
       /// <summary>
       /// {" NEED_SIGN " : "Y"}，是否需要用户线上签章
       /// 是否必填:N
       /// </summary>
       public string NEED_SIGN { get; set; }
       /// <summary>
       /// {
        ///"BIZ_KEY" : " CONTRACT_CODE",
        ///"BIZ_VAL" : "201602170001"
        ///},{
        ///"BIZ_KEY" : " TRANSACTION_ID",
        ///"BIZ_VAL" : "2016021700010001"
        ///}
        ///重复组，用于唯一标识一笔生成记录，接入系统根据实际情况进行定义。作用域为当前请求阶段。
       ///是否必填:Y
       /// </summary>
       public BIZ_VARS BIZ_VARS { get; set; }
       /// <summary>
       /// 生成合同时需要的填入的值
       /// 是否必填:Y
       /// </summary>
       public string BIZ_BODY { get; set; }

       
    }
    public class BIZ_VARS
    {
        public string BIZ_KEY { get; set; }
        public string BIZ_VAL { get; set; }
    }
}
