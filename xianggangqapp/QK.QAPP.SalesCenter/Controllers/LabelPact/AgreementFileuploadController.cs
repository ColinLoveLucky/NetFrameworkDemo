using QK.QAPP.Entity;
using QK.QAPP.Infrastructure;
using QK.QAPP.IServices;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;


namespace QK.QAPP.SalesCenter.Controllers.LabelPact
{
    public class AgreementFileuploadController : Controller
    {
        #region Service对象、属性

        /// <summary>
        /// 提供QBUploadService对象
        /// </summary>
        public IQBUploadService QBUploadHelper { get; set; }

        /// <summary>
        /// 提供ApplyTableService对象
        /// </summary>
        public IApplyTableService ApplyHelper { get; set; }

        public IBID_LabelPactService LabelPactService { get; set; }

        public IQFUserService UserHelper { get; set; }
        public IQFUserService QFUserHelper { get; set; }
        public IBID_PactHistoryService pactHistoryService { get; set; }

        /// <summary>
        /// 非法访问跳转地址
        /// </summary>
        const string NoAuthorization = "/Home/NoAuthorization";

        #endregion

        public ActionResult Index()
        {
            //记录页面打开时间
            Stopwatch sw = new Stopwatch();
            sw.Start();

            #region 参数验证
            if (string.IsNullOrWhiteSpace(Request.QueryString["bidContractNo"]))
            {
                sw.Stop();
                Infrastructure.Log4Net.LogWriter.Biz("非法访问，缺少参数bidContractNo。");
                return new RedirectResult(NoAuthorization);
            }
            //fileType
            string fileType = string.Empty;
            if (Request.QueryString["FileType"] != null && !string.IsNullOrWhiteSpace(Request.QueryString["FileType"].ToString()))
            {
                fileType = Request.QueryString["FileType"];
            }
            else
            {
                sw.Stop();
                Infrastructure.Log4Net.LogWriter.Biz("非法访问，缺少参数fileType。");
                return new RedirectResult(NoAuthorization);
            }
            string isAdditional = Request["IsAdditional"] ?? string.Empty;
            string bidContractNo = Request.QueryString["bidContractNo"];
            string bidAppCode = Request.QueryString["bidAppCode"];
            bool isReadonly=Request["readonly"]==null?false:true;
            #endregion

            #region 权限验证

            //验证是否有数据权限
            QB_V_BID_DETAIL vBidList = LabelPactService.GetBidDetail(bidContractNo);
            //不存在进件，无权访问
            if (vBidList.BID_CONTRACT_NO == null)
            {
                sw.Stop();
                Infrastructure.Log4Net.LogWriter.Biz(string.Format("非法访问，协议（bidContractNo:{0}）不存在。", bidContractNo), bidContractNo.ToString());
                return new RedirectResult(NoAuthorization);
            }
            //View中使用的数据
            if (isReadonly)
            {
                ViewData["readonly"] = true;
            }
            
            #endregion
           

            //记录文件接口访问log
            QBFileLogTemp fileLog = new QBFileLogTemp();

            //请求获取当前进件+文件类别下的文件列表
            var dtoMsg = QBUploadHelper.GetFileList(bidContractNo, bidAppCode, fileType, out fileLog);
            if (dtoMsg.Status == DtoMessageStatus.Fail)
            {
                Infrastructure.Log4Net.LogWriter.Biz(
                    string.Format("在上传/编辑页未成功获取到协议（bidContractNo:{0}）文件类型（fileType:{1}）的文件列表，为保证文件一致，暂不提供任何操作。",
                        bidContractNo,
                        fileType
                    ), bidContractNo.ToString()
                );
                return new RedirectResult(NoAuthorization);
            }
            sw.Stop();
            fileLog.RequestTime = sw.ElapsedMilliseconds;
            Infrastructure.Log4Net.LogWriter.Biz("合同文件上传/编辑页打开", bidContractNo + "", fileLog);
            StringBuilder htmlSb = new StringBuilder();
            if (isReadonly)
            {
                htmlSb.AppendLine("<div id='container'>&nbsp;</div>");

            }
            else if (isAdditional.ToUpper() == "TRUE")
            {
                htmlSb.AppendLine(@"<div id='container'>
                <button id='pickfiles' title='上传'>
                    <i class='icon-upload-alt'> 上传</i>
                </button>
                <div class='cnfg'>
                    <i id='SortImg' class='icon-edit' title='编辑(自动保存变更)'></i>
                    <i id='CloseSortImg' class='icon-check' title='关闭编辑'></i>
                </div>
            </div>");
            }
            else
            {

                StringBuilder fileSb = new StringBuilder();
                fileSb.AppendLine(@"<div class='panel-group' id='accordion'>
                                            <div class='panel panel-default'>
                                                <div class='panel-heading'>
                                                    <h4 class='panel-title'>
                                                        <a data-toggle='collapse' data-parent='#accordion'
                                                           href='#collapseOne'>
                                                            选择上传
                                                        </a>
                                                    </h4>
                                                </div>
                                                <div id='collapseOne' class='panel-collapse collapse in'>
                                <div class='panel-body' style='cursor: pointer; padding:15px 5px'>");
                foreach (var item in dtoMsg.ReturnObj.FILE_LIST.FILES)
                {
                    foreach (var file in item.Values)
                    {
                        if (file.P_MANUAL != null)
                        {
                            /*{0}&upload解释：由于页面加载时，下拉选项中的文件id和展示的文件id相同，会导致上传有问题，此处加入后缀【&upload】以示区分*/
                            fileSb.AppendLine(string.Format("<a  id='{0}&upload' name='{1}' >{2}</a><hr style='margin:5px 0'>", file.P_MANUAL.FILE_ID, file.P_MANUAL.FILE_ID, file.P_MANUAL.FILE_TITLE));
                        }
                    }

                }
                /*由于‘刘云松对肖国栋说’，加入上传其他选项，此处自定义文件id【otherFileId】，上传时，判断文件id如果是otherFileId，则新生成一个文件id，如果不是，则按照真实的文件id上传*/
                fileSb.AppendFormat("<a id='otherFileId&upload'>其他</a><hr style='margin:5px 0'>");
               
                fileSb.AppendLine(@"</div>
                            </div>
                        </div>
                    </div>");
                htmlSb.AppendLine(@"<div id='container' style='height: auto; margin: 0 0;'>");
                htmlSb.AppendLine(fileSb.ToString());
                htmlSb.AppendLine(@"<div class='cnfg'>
                    <i id='SortImg' class='icon-edit' title='编辑(自动保存变更)'></i>
                    <i id='CloseSortImg' class='icon-check' title='关闭编辑'></i>
                </div>
            </div>");
            }
            ViewBag.htmlString = htmlSb.ToString();
            ViewData["BigPicUrl"] = QBUploadHelper.BigPicUrl;
            ViewData["UploadFileFormat"] = QBUploadHelper.UploadFileFormat;
            ViewData["UploadMaxSize"] = QBUploadHelper.UploadMaxSize;
            ViewData["UploadChunkSize"] = QBUploadHelper.UploadChunkSize;
            ViewData["bidContractNo"] = bidContractNo;
            ViewData["bidCode"] = vBidList.BID_CODE;
            ViewData["bidAppCode"] = bidAppCode;
            ViewData["fileType"] = fileType;
            ViewData["IsAdditional"] = isAdditional;
            ViewData["FILES"] = dtoMsg.ReturnObj.FILE_LIST.FILES;
            //返回数据呈现
            return View();
        }

        [HttpPost]
        public JsonResult FileList()
        {
            //记录页面打开时间
            Stopwatch sw = new Stopwatch();
            sw.Start();

            var dtoMsg = new DtoMessage<ContractResultMsg>();
            #region 参数验证
            if (string.IsNullOrEmpty(Request["bidContractNo"])||string.IsNullOrEmpty(Request["bidAppCode"]))
            {
                sw.Stop();
                dtoMsg.Status = DtoMessageStatus.Fail;
                return new JsonResult()
                {
                    Data = dtoMsg
                };
            }
            //fileType
            string fileType = string.Empty;
            if (Request["FileType"] != null && !string.IsNullOrWhiteSpace(Request["FileType"].ToString()))
            {
                fileType = Request["FileType"];
            }
            else
            {
                sw.Stop();
                dtoMsg.Status = DtoMessageStatus.Fail;
                return new JsonResult()
                {
                    Data = dtoMsg
                };
            }
            #endregion

            //记录文件接口访问log
            QBFileLogTemp fileLog = new QBFileLogTemp();
            string bidContractNo = Request["bidContractNo"];
            string bidAppCode = Request["bidAppCode"];
            //请求获取当前进件+文件类别下的文件列表
            dtoMsg = QBUploadHelper.GetFileList(bidContractNo,bidAppCode, fileType, out fileLog);
            sw.Stop();
            fileLog.RequestTime = sw.ElapsedMilliseconds;
            Infrastructure.Log4Net.LogWriter.Biz("获取合同文件列表", bidContractNo + "", fileLog);
            return new JsonResult()
            {
                Data = dtoMsg
            };
        }

        [HttpPost]
        [LogicalActionFilter(ActionSummary = "调用接口上传文件")]
        public string UploadFile()
        {
            //计时器，记录请求时间
            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            sw.Start();

            QBFileLogTemp fileLog = new QBFileLogTemp();
            DtoMessage<ContractResultMsg> dtoMsg = new DtoMessage<ContractResultMsg>();
            //如果是补件上传走文件id本地生成的流程，上传操作走合同生成的文件id
            if (string.IsNullOrWhiteSpace(Request["isAdditional"]))
            {
                //调取上传方法
                 dtoMsg = QBUploadHelper.SaveFile(Request, "fileId", "fileName", "bidContractNo", "bidAppCode", "fileType", out fileLog);
            }
            else
            {
                //调取文件id自动生成的上传方法
                dtoMsg = QBUploadHelper.SaveFile(Request, "fileName", "bidContractNo", "bidAppCode", "fileType", out fileLog);
                if (dtoMsg.Status == DtoMessageStatus.Success)
                {
                    #region  调用接口判断是否需要推送ams系统
                   var amsReslut= LabelPactService.AgreementAdditionalUpload(Request["bidContractNo"]);
                   Infrastructure.Log4Net.LogWriter.Biz("文件补录上传推送ams系统", fileLog.BizNo + "", amsReslut);
                    #endregion
                }

            }
            sw.Stop();
            fileLog.RequestTime = sw.ElapsedMilliseconds;
            Infrastructure.Log4Net.LogWriter.Biz("合同文件上传", fileLog.BizNo + "", fileLog);

            //返回上传结果
            return Newtonsoft.Json.JsonConvert.SerializeObject(dtoMsg);
        }

        [HttpPost]
        public JsonResult SaveNewSort()
        {
            //计时器，记录请求时间
            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            sw.Start();

            DtoMessage<string> dtoMsg = new DtoMessage<string>();
            string idList = Request.Form["newOrder"];
            if (string.IsNullOrWhiteSpace(idList))
            {
                dtoMsg.Status = DtoMessageStatus.Fail;
                return new JsonResult()
                {
                    Data = dtoMsg
                };
            }

            QBFileLogTemp fileLog = new QBFileLogTemp();
            //调用排序接口
            dtoMsg = QBUploadHelper.SaveNewSort(idList, out fileLog);

            sw.Stop();
            fileLog.RequestTime = sw.ElapsedMilliseconds;
            Infrastructure.Log4Net.LogWriter.Biz("文件排序", fileLog);

            return new JsonResult()
            {
                Data = dtoMsg
            };
        }

        [HttpPost]
        [LogicalActionFilter(ActionSummary = "调用接口删除文件")]
        public JsonResult DeleteFile()
        {
            //计时器，记录请求时间
            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            sw.Start();

            DtoMessage<ContractResultMsg> dtoMsg = new DtoMessage<ContractResultMsg>();

            #region 参数验证
            string bidContractNo = string.Empty;
            string bidAppCode = string.Empty;
            if (Request["bidContractNo"] == null||Request["bidAppCode"]==null)
            {
                sw.Stop();
                dtoMsg.Status = DtoMessageStatus.Fail;
                Infrastructure.Log4Net.LogWriter.Biz("文件删除参数验证失败：bidContractNo值为null或bidAppCode值为null");
                return new JsonResult()
                {
                    Data = dtoMsg
                };
            }
            bidContractNo = Request["bidContractNo"];
            bidAppCode = Request["bidAppCode"];
            string fileId = string.Empty;
            if (Request["fileId"] != null)
            {
                fileId = Request["fileId"];
            }

            string fileType = string.Empty;
            if (Request["FileType"] != null )
            {
                fileType = Request["FileType"];
            }
            #endregion

            #region 权限验证

            //验证是否有数据权限
            QB_V_BID_DETAIL bidInfo = LabelPactService.GetBidDetail(bidContractNo);
            //不存在进件，或者进件对应客服人员为空白，无权访问
            if (bidInfo == null )
            {
                sw.Stop();
                dtoMsg.Status = DtoMessageStatus.Fail;
                dtoMsg.Error = "不存在进件，无权访问!";
                Infrastructure.Log4Net.LogWriter.Biz("合同文件删除：不存在进件，无权访问!");
                return new JsonResult()
                {
                    Data = dtoMsg
                };
            }
            //验证状态，只有上传或者补录的状态才能调用删除
            if (bidInfo.BID_STATE != BidStatusQueryType.BS_XY_WSC&&bidInfo.BID_STATE!=BidStatusQueryType.BS_XY_YSC&&bidInfo.BID_STATE!=BidStatusQueryType.BS_XY_YQR)
            {
                    sw.Stop();
                    dtoMsg.Status = DtoMessageStatus.Fail;
                    dtoMsg.Error = "只有上传或者补录的状态才能调用删除";
                    Infrastructure.Log4Net.LogWriter.Biz("合同文件删除：只有上传或者补录的状态才能调用删除");
                    return new JsonResult()
                    {
                        Data = dtoMsg
                    };
            }
            if (!QBUploadHelper.IsAdditionalFile(fileId, bidContractNo) && bidInfo.BID_STATE==BidStatusQueryType.BS_XY_YQR)
            {
                dtoMsg.Status = DtoMessageStatus.Fail;
                dtoMsg.Error = "文件不符合删除条件";
                Infrastructure.Log4Net.LogWriter.Biz("合同文件删除：文件不符合删除条件，不能在补录状态下删除已确认的文件！");
                return new JsonResult()
                {
                    Data = dtoMsg
                };
            }
            #endregion
            //如果是协议已经确认即协议补录操作删除的文件type传空
            if(bidInfo.BID_STATE==BidStatusQueryType.BS_XY_YQR)
            {
                fileType = "";
            }

            QBFileLogTemp fileLog = new QBFileLogTemp();
            //验证通过：调取删除接口//
            dtoMsg = QBUploadHelper.DeleteFile(bidContractNo,bidAppCode,fileId,fileType, out fileLog);
            sw.Stop();
            fileLog.RequestTime = sw.ElapsedMilliseconds;
            Infrastructure.Log4Net.LogWriter.Biz("合同文件删除", fileLog.BizNo + "", fileLog);

            return new JsonResult()
            {
                Data = dtoMsg
            };
        }

        public string Download()
        {
            //计时器，记录请求时间
            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            sw.Start();
            #region  参数权限验证
            string bidCode = string.Empty;
            if (Request["bidCode"] == null)
            {
                sw.Stop();
                Infrastructure.Log4Net.LogWriter.Biz("合同文件下载参数验证失败：bidCode值为null");
                return "合同文件下载参数验证失败：bidCode值为null";
                
            }
            bidCode = Request["bidCode"];
            //验证是否有数据权限
            QB_V_BID_DETAIL bidInfo = LabelPactService.GetBidDetail(bidCode);
            //不存在进件，或者进件对应客服人员为空白，无权访问
            if (bidInfo == null )
            {
                sw.Stop();
                Infrastructure.Log4Net.LogWriter.Biz("合同文件下载：不存在进件，无权访问!");
                return "合同文件下载：不存在进件，无权访问!";
            }
            #endregion
            var user = QFUserHelper.GetCurrentUser();
            QBFileLogTemp fileLog = new QBFileLogTemp();
            string returnValue = QBUploadHelper.DownloadFileByUrl(this.HttpContext, "theName","theSrc", "theId", out fileLog);
            if (string.IsNullOrEmpty(returnValue))
            {
                //下载历史记录
                ContractHistoryRequest historyList = new ContractHistoryRequest();
                List<ContractHistoryRequestType> history = new List<ContractHistoryRequestType>();
                history.Add(new ContractHistoryRequestType() { BidCode = this.HttpContext.Request["bidCode"], ContractCode = bidInfo.BID_CONTRACT_NO, UserCode = user.Account, UserName = user.RealName, DeptCode = user.DepartmentId, DeptName = user.DepartmentName, ContractOptype = ContractOPTYPE.Contract_OP_DOWNLOAD });
                historyList.ContractHistory = history;
                pactHistoryService.AgreementAddHistory(historyList);
            }
            sw.Stop();
            fileLog.RequestTime = sw.ElapsedMilliseconds;
            Infrastructure.Log4Net.LogWriter.Biz("合同文件下载", bidInfo.BID_CONTRACT_NO + "", fileLog);

            return returnValue;
        }
	}
}