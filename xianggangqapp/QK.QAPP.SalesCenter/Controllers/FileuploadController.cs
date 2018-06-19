/***********************
 * 作    者：刘云松
 * 创建时间：‎2014‎-0‎9-‎12‎ ‏‎15:08:50
 * 作    用：提供文件的上传、下载、读取、删除、排序等功能
*****************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QK.QAPP.Entity;
using QK.QAPP.IServices;
using QK.QAPP.Infrastructure;
using Microsoft.Practices.Unity;
using System.Diagnostics;

namespace QK.QAPP.SalesCenter.Controllers
{
    public class FileuploadController : Controller
    {
        #region Service对象、属性

        /// <summary>
        /// 提供QFUploadService对象
        /// </summary>
        [Dependency]
        public IQFUploadService QFUploadHelper { get; set; }

        /// <summary>
        /// 提供ApplyTableService对象
        /// </summary>
        [Dependency]
        public IApplyTableService ApplyHelper { get; set; }

        [Dependency]
        public IV_APPMAINSERVICE VAppmainHelper { get; set; }

        [Dependency]
        public IQFUserService UserHelper { get; set; }

        [Dependency]
        public IMobileHistoryService MobileHistoryService { get; set; }

        public IGenesisService GenesisService { get; set; }

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
            //appId
            long appId = 0L;
            if (Request.QueryString["appId"] == null)
            {
                sw.Stop();
                Infrastructure.Log4Net.LogWriter.Biz("非法访问，缺少参数appId。");
                return new RedirectResult(NoAuthorization);
            }
            else
            {
                if (!long.TryParse(Request.QueryString["appId"].ToString(), out appId))
                {
                    sw.Stop();
                    Infrastructure.Log4Net.LogWriter.Biz(
                        string.Format("非法访问，参数appId类型错误（appId:{0}）。", Request.QueryString["appId"].ToString())
                    );
                    return new RedirectResult(NoAuthorization);
                }
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
            string isSdStatus = Request["IsSDStatud"] ?? string.Empty;
            string strNrListId = Request["NrListId"] ?? string.Empty;
            #endregion

            #region 权限验证

            //验证是否有数据权限
            V_APPMAIN vappMainEntity = VAppmainHelper.FirstOrDefault(a => a.APPID == appId);
            //不存在进件，无权访问
            if (vappMainEntity == null)
            {
                sw.Stop();
                Infrastructure.Log4Net.LogWriter.Biz(string.Format("非法访问，进件（appId:{0}）不存在。", appId), appId.ToString());
                return new RedirectResult(NoAuthorization);
            }
            //进件对应客服人员为空白，无权访问
            if (string.IsNullOrWhiteSpace(vappMainEntity.CSADNO))
            {
                sw.Stop();
                Infrastructure.Log4Net.LogWriter.Biz(string.Format("非法访问，进件（appId:{0}）没有客服专员。", appId), appId.ToString());
                return new RedirectResult(NoAuthorization);
            }
            //没有数据权限，无权访问
            //if (!UserHelper.GetCurrentUser().DataPermission.Contains(vappMainEntity.CSADNO, StringComparer.OrdinalIgnoreCase))
            if (!UserHelper.CheckDataPermission(appId))
            {
                sw.Stop();
                Infrastructure.Log4Net.LogWriter.Biz(
                    string.Format("非法访问，操作人数据权限与进件（appId:{0}）客服专员【{1}（{2}）】不匹配。",
                        appId,
                        vappMainEntity.CSADNO,
                        vappMainEntity.CSADNAME
                    ), appId.ToString()
                );
                return new RedirectResult(NoAuthorization);
            }

            //View中使用的数据
            if (Request["readonly"] != null)
            {
                ViewData["readonly"] = true;
            }
            else
            {
                //非只读模式，需验证进件状态和权限
                if (vappMainEntity.APPSTATUS != EnterStatusType.PENDING.ToString())
                {
                    //补件队列的ID（app_nr_List）是否存在
                    long nrListId = 0L;
                    if (isSdStatus.ToUpper() != "TRUE" || !long.TryParse(strNrListId, out nrListId))
                    {
                        sw.Stop();
                        Infrastructure.Log4Net.LogWriter.Biz(
                            string.Format("非法访问，进件（appId:{0}）不是PENDING状态，但文件列表欲以非只读／非补件（isSdStatus:{1}，nrListId:{2}）方式打开。",
                                appId,
                                isSdStatus,
                                nrListId
                            ), appId.ToString()
                        );
                        return new RedirectResult(NoAuthorization);
                    }

                    if (QFUploadHelper.Order_SD_Status_Need.Keys.ToArray().Contains(vappMainEntity.APPSTATUS))
                    {
                        //检查该类型是否需要补件
                        if (!ApplyHelper.CheckNrNeedSD(appId, nrListId, fileType))
                        {
                            sw.Stop();
                            Infrastructure.Log4Net.LogWriter.Biz(
                                string.Format("非法访问，进件（appId:{0}）的文件类型（fileType:{1}，nrListId:{2}）不需要补件。",
                                    appId,
                                    fileType,
                                    nrListId
                                ), appId.ToString()
                            );
                            return new RedirectResult(NoAuthorization);
                        }

                        ViewData["NrDateApply"] = ApplyHelper.GetNrDateApply(appId, fileType) ?? DateTime.MinValue;
                    }
                    else
                    {
                        sw.Stop();
                        Infrastructure.Log4Net.LogWriter.Biz(
                            string.Format("非法访问，进件（appId:{0}）当前状态为{1}，不需要补件。", appId, vappMainEntity.APPSTATUS), appId.ToString()
                        );
                        return new RedirectResult(NoAuthorization);
                    }
                }
            }
            #endregion

            ViewData["BigPicUrl"] = QFUploadHelper.BigPicUrl;
            ViewData["UploadFileFormat"] = QFUploadHelper.UploadFileFormat;
            ViewData["UploadMaxSize"] = QFUploadHelper.UploadMaxSize;
            ViewData["UploadChunkSize"] = QFUploadHelper.UploadChunkSize;
            ViewData["appId"] = appId;
            ViewData["fileType"] = fileType;
            ViewData["IsSDStatud"] = isSdStatus;
            ViewData["NrListId"] = strNrListId;
            ViewData["NrDateApply"] = ViewData["NrDateApply"] ?? DateTime.MinValue;

            //记录文件接口访问log
            QFFileLogTemp fileLog = new QFFileLogTemp();

            //请求获取当前进件+文件类别下的文件列表
            DtoMessage<List<QFFileReadListResult>> dtoMsg = QFUploadHelper.GetFileList(appId, fileType, out fileLog);
            if (dtoMsg.Status == DtoMessageStatus.Fail)
            {
                Infrastructure.Log4Net.LogWriter.Biz(
                    string.Format("在上传/编辑页未成功获取到进件（appId:{0}）文件类型（fileType:{1}）的文件列表，为保证文件一致，暂不提供任何操作。",
                        appId,
                        fileType
                    ), appId.ToString()
                );
                return new RedirectResult(NoAuthorization);
            }
            sw.Stop();
            fileLog.RequestTime = sw.ElapsedMilliseconds;
            Infrastructure.Log4Net.LogWriter.Biz("文件上传/编辑页打开", appId + "", fileLog);

            //返回数据呈现
            return View(dtoMsg.ReturnObj);
        }

        [HttpPost]
        public JsonResult FileList()
        {
            //记录页面打开时间
            Stopwatch sw = new Stopwatch();
            sw.Start();

            DtoMessage<List<QFFileReadListResult>> dtoMsg = new DtoMessage<List<QFFileReadListResult>>();
            #region 参数验证
            //appId
            long appId = 0L;
            if (Request["appId"] == null)
            {
                sw.Stop();
                dtoMsg.Status = DtoMessageStatus.Fail;
                return new JsonResult()
                {
                    Data = dtoMsg
                };
            }
            else
            {
                if (!long.TryParse(Request["appId"].ToString(), out appId))
                {
                    sw.Stop();
                    dtoMsg.Status = DtoMessageStatus.Fail;
                    return new JsonResult()
                    {
                        Data = dtoMsg
                    };
                }
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
            QFFileLogTemp fileLog = new QFFileLogTemp();

            //请求获取当前进件+文件类别下的文件列表
            dtoMsg = QFUploadHelper.GetFileList(appId, fileType, out fileLog);
            sw.Stop();
            fileLog.RequestTime = sw.ElapsedMilliseconds;
            Infrastructure.Log4Net.LogWriter.Biz("获取文件列表", appId + "", fileLog);
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

            QFFileLogTemp fileLog = new QFFileLogTemp();
            //调取上传方法
            DtoMessage<QFFile> dtoMsg = QFUploadHelper.SaveFile(Request, "file__name", "appId", "fileType", out fileLog);
            if (dtoMsg.Status == DtoMessageStatus.Success)
            {
                //上传成功后需要更新file_check的记录
                long appId = long.Parse(Request["appId"].ToString());
                QFUploadHelper.UpdateFileCheck(appId, Request["fileType"].ToString(), true, false);

                //如果是补件，需要更新补件队列的上传时间
                string isSdStatus = Request["IsSDStatud"] ?? string.Empty;
                string strNrListId = Request["NrListId"] ?? string.Empty;
                long nrListId = 0L;
                if (isSdStatus.ToUpper() == "TRUE" && long.TryParse(strNrListId, out nrListId))
                {
                    ApplyHelper.UpdateNrListUpdateTime(appId, nrListId, false);
                }
            }
            sw.Stop();
            fileLog.RequestTime = sw.ElapsedMilliseconds;
            Infrastructure.Log4Net.LogWriter.Biz("文件上传", fileLog.AppId + "", fileLog);

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

            QFFileLogTemp fileLog = new QFFileLogTemp();
            //调用排序接口
            dtoMsg = QFUploadHelper.SaveNewSort(idList, out fileLog);

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

            DtoMessage<string> dtoMsg = new DtoMessage<string>();

            #region 参数验证
            //appId
            long appId = 0L;
            if (Request["appId"] == null)
            {
                sw.Stop();
                dtoMsg.Status = DtoMessageStatus.Fail;
                return new JsonResult()
                {
                    Data = dtoMsg
                };
            }
            else
            {
                if (!long.TryParse(Request["appId"].ToString(), out appId))
                {
                    sw.Stop();
                    dtoMsg.Status = DtoMessageStatus.Fail;
                    return new JsonResult()
                    {
                        Data = dtoMsg
                    };
                }
            }
            //fileId
            string fileId = string.Empty;
            if (Request["fileId"] != null && !string.IsNullOrWhiteSpace(Request["fileId"].ToString()))
            {
                fileId = Request["fileId"];
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

            #region 权限验证

            //验证是否有数据权限
            V_APPMAIN vappMainEntity = VAppmainHelper.FirstOrDefault(a => a.APPID == appId);
            //不存在进件，或者进件对应客服人员为空白，无权访问
            if (vappMainEntity == null || string.IsNullOrWhiteSpace(vappMainEntity.CSADNO))
            {
                sw.Stop();
                dtoMsg.Status = DtoMessageStatus.Fail;
                return new JsonResult()
                {
                    Data = dtoMsg
                };
            }
            //没有数据权限，无权访问
            //if (!UserHelper.GetCurrentUser().DataPermission.Contains(vappMainEntity.CSADNO, StringComparer.OrdinalIgnoreCase))
            if (!UserHelper.CheckDataPermission(appId))
            {
                sw.Stop();
                dtoMsg.Status = DtoMessageStatus.Fail;
                return new JsonResult()
                {
                    Data = dtoMsg
                };
            }
            //验证状态，只有pending或者补件的状态才能调用删除
            if (vappMainEntity.APPSTATUS != EnterStatusType.PENDING.ToString())
            {
                //不在补件队列中，不能删除
                if (QFUploadHelper.Order_SD_Status_Need.Keys.ToArray().Contains(vappMainEntity.APPSTATUS))
                {
                    //检查该类型是否需要补件
                    if (!ApplyHelper.CheckNrNeedSD(appId, fileType))
                    {
                        sw.Stop();
                        dtoMsg.Status = DtoMessageStatus.Fail;
                        return new JsonResult()
                        {
                            Data = dtoMsg
                        };
                    }
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
            }
            #endregion

            QFFileLogTemp fileLog = new QFFileLogTemp();
            //验证通过：调取删除接口//
            dtoMsg = QFUploadHelper.DeleteFile(fileId, out fileLog);
            if (dtoMsg.Status == DtoMessageStatus.Success)
            {
                //删除成功后更新file_check状态

                if (QFUploadHelper.Order_SD_Status_Need.Keys.ToArray().Contains(vappMainEntity.APPSTATUS))
                {
                    QFUploadHelper.UpdateFileCheck(appId, Request["fileType"].ToString(), false, true);
                }
                else
                {
                    QFUploadHelper.UpdateFileCheck(appId, Request["fileType"].ToString(), false, false);
                }
            }
            sw.Stop();
            fileLog.RequestTime = sw.ElapsedMilliseconds;
            Infrastructure.Log4Net.LogWriter.Biz("文件删除", fileLog);

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
            QFFileLogTemp fileLog = new QFFileLogTemp();
            string returnValue = QFUploadHelper.DownloadFileByUrl(this.HttpContext, "theName", "theId", out fileLog);
            sw.Stop();
            fileLog.RequestTime = sw.ElapsedMilliseconds;
            Infrastructure.Log4Net.LogWriter.Biz("文件下载", fileLog);

            return returnValue;
        }

        [HttpPost]
        public JsonResult CheckFileType()
        {
            long appId = 0L;
            if (Request["appId"] != null)
            {
                long.TryParse(Request["appId"].ToString(), out appId);
            }
            string fileType = string.Empty;
            if (Request["FileType"] != null && !string.IsNullOrWhiteSpace(Request["FileType"].ToString()))
            {
                fileType = Request["FileType"];
            }
            string[] fileTypeAry = fileType.Split(',');

            Dictionary<string, long> dic = new Dictionary<string, long>();
            foreach (string s in fileTypeAry)
            {
                if (string.IsNullOrWhiteSpace(s))
                    continue;
                dic.Add(s, -1L);
            }
            if (appId > 0L)
            {
                List<APP_NR_LIST> lst = ApplyHelper.GetNRList(appId);
                if (lst != null && lst.Count > 0)
                {
                    foreach (APP_NR_LIST anl in lst)
                    {
                        if (dic.Keys.ToList().Contains(anl.NR_CODE, StringComparer.OrdinalIgnoreCase))
                            dic[anl.NR_CODE] = anl.ID;
                    }
                }
            }
            return new JsonResult() { Data = dic };
        }

        [HttpPost]
        public JsonResult GetSpecialFileAmount()
        {
            long appId = 0L;
            if (Request["appId"] != null)
            {
                long.TryParse(Request["appId"].ToString(), out appId);
            }
            Dictionary<string, List<string>> dic = new Dictionary<string, List<string>>();
            dic = QFUploadHelper.CreditAccount(appId);
            //dic.Add("Pboc2", 3);
            return new JsonResult() { Data = dic };
        }

        [HttpGet]
        public bool HasMobileHistory(long appId)
        {
            var status = MobileHistoryService.GetStatus(appId);
            if (status == MobileHistoryStatus.Finish.ToString())
            {
                return true;
            }
            return false;
        }

        [HttpGet]
        public JsonResult GenesisStatus(long appId)
        {
            var resultObj = new Dictionary<string, bool>();
            var status = GenesisService.GetGenesisStatus(appId);
            resultObj.Add("MobileStatus", status.MobileStatus == "202");
            resultObj.Add("PbocStatus", status.PbocStatus == "202");
            resultObj.Add("NetbankStatus", status.NetbankStatus == "202");
            resultObj.Add("FundStatus", status.FundStatus == "202");
            return Json(resultObj, JsonRequestBehavior.AllowGet);
        }
    }
}