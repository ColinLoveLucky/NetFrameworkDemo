/***********************
 * 作    者：刘云松
 * 创建时间：‎2014‎-0‎9-‎12‎ ‏‎15:08:50
 * 作    用：对接Java影像接口实现文件的上传、下载、读取、删除、排序等功能
*****************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QK.QAPP.IServices;
using QK.QAPP.Infrastructure;
using QK.QAPP.Entity;
using QK.QAPP.Global;
using System.IO;
using Microsoft.Practices.Unity;

namespace QK.QAPP.Services
{
    public class QFUploadService : IQFUploadService
    {
        #region 实现接口内成员

        #region 属性配置

        /// <summary>
        /// 提供APP_CUSTOMERSERVICE对象
        /// </summary>
        [Dependency]
        public IAPP_CUSTOMERSERVICE CustomerService
        {
            get;
            set;
        }

        /// <summary>
        /// 提供APP_MAINSERVICE对象
        /// </summary>
        [Dependency]
        public IAPP_MAINSERVICE MainService
        {
            get;
            set;
        }

        /// <summary>
        /// 提供QFUserService对象
        /// </summary>
        [Dependency]
        public IQFUserService UserService
        {
            get;
            set;
        }

        /// <summary>
        /// 提供FileCheck对象
        /// </summary>
        [Dependency]
        public IAPP_FILE_CHECKSERVICE FileCheckService
        {
            get;
            set;
        }

        /// <summary>
        /// 提供ApplyTableService对象
        /// </summary>
        [Dependency]
        public IApplyTableService ApplyHelper { get; set; }


        /// <summary>
        /// 提供FL_LISTSERVICE对象
        /// </summary>
        [Dependency]
        public IFL_LISTSERVICE FileService { get; set; }

        /// <summary>
        /// 提供FL_BIZSERVICE对象
        /// </summary>
        [Dependency]
        public IFL_BIZSERVICE FileBizService { get; set; }

        [Dependency]
        public IApplicationService applicationService { get; set; }

        /// <summary>
        /// 提供Rest服务，使用前先赋值new RestHelper(url)
        /// </summary>
        RestHelper restHelper = null;

        /// <summary>
        /// 文件上传格式
        /// </summary>
        /// <returns></returns>
        public string UploadFileFormat
        {
            get
            {
                return GlobalSetting.UploadFileFormat;
            }
        }

        /// <summary>
        /// 允许上传的最大文件（单位：KB）
        /// </summary>
        /// <returns></returns>
        public int UploadMaxSize
        {
            get
            {
                return GlobalSetting.UploadMaxSize;
            }
        }

        /// <summary>
        /// 分块上传时每块大小（单位：KB）
        /// </summary>
        /// <returns></returns>
        public int UploadChunkSize
        {
            get
            {
                return GlobalSetting.UploadChunkSize;
            }
        }

        /// <summary>
        /// 允许下载的最大文件（单位：KB）
        /// </summary>
        /// <returns></returns>
        public int DownloadMaxSize
        {
            get
            {
                return GlobalSetting.DownloadMaxSize;
            }
        }

        /// <summary>
        /// 分块下载时每块大小（单位：KB）
        /// </summary>
        /// <returns></returns>
        public int DownloadChunkSize
        {
            get
            {
                return GlobalSetting.DownloadChunkSize;
            }
        }

        /// <summary>
        /// 显示大图的URL
        /// </summary>
        public string BigPicUrl
        {
            get
            {
                return GlobalSetting.FileReadUrl;
            }
        }

        /// <summary>
        /// 待补件状态
        /// </summary>
        public Dictionary<string, string> Order_SD_Status_Need
        {
            get { return GlobalSetting.Order_SD_Status_Need; }
        }

        #endregion

        /// <summary>
        /// 保存文件
        /// </summary>
        /// <param name="request">发送请求的HttpRequestBase</param>
        /// <param name="reqFileName">保存文件名的Request参数名</param>
        /// <param name="reqAppId">保存申请单流水号的Request（GET/POST）参数名</param>
        /// <param name="reqFileType">保存证件类型的Request（GET/POST）参数名</param>
        /// <param name="dicLogEntity"></param>
        /// <returns></returns>
        public DtoMessage<QFFile> SaveFile(System.Web.HttpRequestBase request, string reqFileName, string reqAppId, string reqFileType, out QFFileLogTemp fileLog)
        {
            fileLog = new QFFileLogTemp();

            //当前用户对象
            QFUser currentUser = UserService.GetCurrentUser();

            fileLog.City = currentUser.City.CITY_NAME;

            DtoMessage<QFFile> dtoMsg = new DtoMessage<QFFile>();
            if (request.Files.Count == 0)
            {
                dtoMsg.ReturnObj = null;
                dtoMsg.Status = DtoMessageStatus.Fail;
                dtoMsg.Error = "There is no file to be uploaded!";
            }
            else
            {
                QFFile returnFile = new QFFile();
                string fileName = string.Empty;
                if (request[reqFileName] != null)
                {
                    fileName = request[reqFileName].Trim();
                }
                try
                {
                    System.Web.HttpPostedFileBase postedFile = request.Files[0];

                    fileName = string.IsNullOrEmpty(fileName) ? postedFile.FileName : fileName;
                    //IE10浏览器文件名包含全路径
                    if (fileName.LastIndexOf('\\') > -1)
                    {
                        returnFile.OldFileName = fileName.Substring(fileName.LastIndexOf('\\') + 1);
                    }
                    else
                    {
                        returnFile.OldFileName = fileName;
                    }
                    //上传后的新文件名
                    returnFile.NewFileName = returnFile.OldFileName;

                    if (postedFile.ContentLength > 0)
                    {
                        int fileLength = (int)postedFile.InputStream.Length;
                        byte[] buffer = new byte[fileLength];
                        postedFile.InputStream.Read(buffer, 0, fileLength);

                        long appId = long.Parse(request[reqAppId].ToString());

                        //从APP_CUSTOMER取得客户信息
                        APP_CUSTOMER appCustomer = CustomerService.Find(c => c.APP_ID == appId).First();

                        //从APP_MAIN取得订单信息
                        APP_MAIN appMain = MainService.Find(m => m.ID == appId).First();

                        //构建请求接口的post对象
                        QFFileUpload upEntity = new QFFileUpload();
                        upEntity.bizCode = appMain.APP_CODE;
                        upEntity.idNo = appCustomer.ID_NO;
                        upEntity.idType = appCustomer.ID_TYPE;
                        upEntity.userName = appCustomer.NAME;
                        upEntity.loginUser = currentUser.Code;
                        upEntity.fileName = fileName;
                        upEntity.fileType = request[reqFileType].ToString();
                        upEntity.fileContent = buffer;
                        //计时器
                        System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
                        sw.Start();

                        restHelper = new RestHelper(GlobalSetting.FileUploadUrl);
                        //请求接口上传文件，如果成功DtoMessage的ReturnObj 形如{"okTagMsg:":"445"}，其中445为DB里的ID
                        DtoMessage<QFFileUploadResult> communicateResult = restHelper.Post<QFFileUploadResult>(string.Empty, upEntity);
                        sw.Stop();

                        fileLog.AppId = appId;
                        fileLog.FileName = fileName;
                        fileLog.FileSize = (fileLength / 1024).ToString() + "KB";
                        fileLog.FileType = upEntity.fileType;
                        fileLog.JavaApiTime = sw.ElapsedMilliseconds;
                        if (communicateResult.Status == DtoMessageStatus.Success)
                        {
                            int fid = 0;
                            int.TryParse(communicateResult.ReturnObj.okTagMsg, out fid);
                            if (fid > 0)
                            {
                                returnFile.FileId = communicateResult.ReturnObj.okTagMsg;
                                dtoMsg.Status = DtoMessageStatus.Success;
                                fileLog.FileId = fid;
                                fileLog.Info = "上传成功！";
                                fileLog.Result = "OK";
                            }
                            else
                            {
                                dtoMsg.Status = DtoMessageStatus.Fail;
                                fileLog.Info = "上传接口出错，上传失败！";
                                fileLog.Result = "KO";
                            }
                        }
                        else
                        {
                            dtoMsg.Status = DtoMessageStatus.Fail;
                            dtoMsg.Error = communicateResult.Error;
                            fileLog.Info = communicateResult.Error;
                            fileLog.Result = "KO";
                        }
                        upEntity = null;
                    }
                }
                catch (Exception exIn)
                {
                    dtoMsg.Status = DtoMessageStatus.Fail;
                    dtoMsg.Error = exIn.Message;
                    fileLog.Info = exIn.Message;
                    fileLog.Result = "KO";
                }
                dtoMsg.ReturnObj = returnFile;
            }
            return dtoMsg;
        }

        /// <summary>
        /// 排序
        /// </summary>
        /// <param name="sortedIds">新顺序</param>
        /// <param name="fileLog"></param>
        /// <returns></returns>
        public DtoMessage<string> SaveNewSort(string sortedIds, out QFFileLogTemp fileLog)
        {
            fileLog = new QFFileLogTemp();

            //当前用户对象
            QFUser currentUser = UserService.GetCurrentUser();
            fileLog.City = currentUser.City.CITY_NAME;

            DtoMessage<string> dtoMsg = new DtoMessage<string>();
            dtoMsg.ReturnObj = sortedIds;
            try
            {
                Dictionary<string, string> dicParameter = new Dictionary<string, string>();
                dicParameter.Add("fileId", sortedIds);
                //计时器
                System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
                sw.Start();
                restHelper = new RestHelper(GlobalSetting.FileSortUrl);
                DtoMessage<object> communicateResult = restHelper.Post<object>(string.Empty, dicParameter);
                sw.Stop();
                fileLog.JavaApiTime = sw.ElapsedMilliseconds;
                if (communicateResult.Status == DtoMessageStatus.Success)
                {
                    if (communicateResult.ReturnObj.ToString().Contains("okTagMsg"))
                    {
                        dtoMsg.Status = DtoMessageStatus.Success;
                        fileLog.Result = "OK";
                        fileLog.Info = "变更排序成功！";
                    }
                    else
                    {
                        dtoMsg.Status = DtoMessageStatus.Fail;
                        dtoMsg.Error = "排序接口出错，排序失败！";
                        fileLog.Result = "KO";
                        fileLog.Info = "排序接口出错，排序失败！";
                    }
                }
                else
                {
                    dtoMsg.Status = DtoMessageStatus.Fail;
                    dtoMsg.Error = communicateResult.Error;
                    fileLog.Result = "KO";
                    fileLog.Info = dtoMsg.Error;
                }
            }
            catch (Exception ex)
            {
                dtoMsg.Status = DtoMessageStatus.Fail;
                dtoMsg.Error = ex.Message;
                fileLog.Result = "KO";
                fileLog.Info = dtoMsg.Error;
            }
            dtoMsg.Status = DtoMessageStatus.Success;
            return dtoMsg;
        }

        /// <summary>
        /// 读取文件列表
        /// </summary>
        /// <param name="appId">申请单流水号</param>
        /// <param name="fileType">文件类别</param>
        /// <param name="fileLog"></param>
        /// <returns></returns>
        public DtoMessage<List<QFFileReadListResult>> GetFileList(long appId, string fileType, out QFFileLogTemp fileLog)
        {
            fileLog = new QFFileLogTemp();

            //当前用户对象
            QFUser currentUser = UserService.GetCurrentUser();
            fileLog.City = currentUser.City.CITY_NAME;

            DtoMessage<List<QFFileReadListResult>> retMsg = new DtoMessage<List<QFFileReadListResult>>();
            try
            {
                //从APP_CUSTOMER取得客户信息
                //APP_CUSTOMER appCustomer = CustomerService.Find(c => c.APP_ID == appId).First();

                //从APP_MAIN取得订单信息
                APP_MAIN appMain = MainService.Find(m => m.ID == appId).First();

                //创建请求对象
                QFFileReadList frlEntity = new QFFileReadList()
                {
                    bizCode = appMain.APP_CODE,
                    //不传客户信息也能掉，降低数据库访问次数
                    //idNo = appCustomer.ID_NO,
                    //idType = appCustomer.ID_TYPE,
                    //userName = appCustomer.NAME,
                    idNo = "",
                    idType = "",
                    userName = "",
                    fileType = fileType
                };
                //计时器
                System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
                sw.Start();
                //20150209 刘成帅调整 可以配置是从数据库读取文件列表还是从接口读取 
                DtoMessage<QFFileReadListResultList> dtoMsg = new DtoMessage<QFFileReadListResultList>();
                if (GlobalSetting.FileListFromAPI)
                {
                    #region 用接口访问

                    restHelper = new RestHelper(GlobalSetting.FileListReadUrl);
                    //请求获取接口
                    dtoMsg = restHelper.Post<QFFileReadListResultList>(string.Empty, frlEntity);
                    fileLog.Info = "数据来源：JavaAPI接口。";
                    #endregion
                }
                else
                {
                    fileLog.Info = "数据来源：数据库读取。";

                    #region 从数据库直接访问

                    string error = "";
                    //查找ＡＰＰＣＯＤＥ
                    var appcodeEntity = MainService.FirstOrDefault(c => c.ID == appId);
                    if (appcodeEntity == null)
                    {
                        //TODO 记日志 返回错误
                        error = "进件单号未找到" + appId;
                    }
                    else
                    {
                        var listResultList = new QFFileReadListResultList { flListList = new List<QFFileReadListResult>() };
                        //查找FL_BIZ数据
                        var flBizEntity = FileBizService.FirstOrDefault(c => c.BIZ_CODE == appcodeEntity.APP_CODE);
                        if (flBizEntity == null)
                        {
                            dtoMsg = new DtoMessage<QFFileReadListResultList>()
                            {
                                ReturnObj = new QFFileReadListResultList(),
                                CurPage = 0,
                                Error = "",
                                PageSize = 0,
                                Status = DtoMessageStatus.Success,
                                TotalNum = 0
                            };
                        }
                        else
                        {
                            var fileTypeList = (fileType + "").Split(',');
                            var fileList = FileService.Find(c => c.FL_ID == flBizEntity.ID && c.STATUS == "Y").ToList().Where(c => fileTypeList.Contains(c.FL_TYPE)).ToList();
                            fileList.ForEach(fileInfo => listResultList.flListList.Add(new QFFileReadListResult()
                            {
                                changedTime = fileInfo.CHANGED_TIME.HasValue ? (fileInfo.CHANGED_TIME.Value.Subtract(new DateTime(1970, 1, 1, 8, 0, 0)).Ticks / 10000).ToString() : "1",
                                changedUser = fileInfo.CHANGED_USER,
                                createdTime = fileInfo.CREATED_TIME.HasValue ? (fileInfo.CREATED_TIME.Value.Subtract(new DateTime(1970, 1, 1, 8, 0, 0)).Ticks / 10000).ToString() : "1",
                                createdUser = fileInfo.CREATED_USER,
                                fileContent = null,
                                flBiz = fileInfo.FL_ID + "",
                                flHeighth = fileInfo.FL_HEIGHTH.HasValue ? fileInfo.FL_HEIGHTH.Value.ToInt32() : 0,
                                flMemo = fileInfo.FL_MEMO,
                                flName = fileInfo.FL_NAME,
                                flPath = GlobalSetting.FileReadUrl + "?id=" + fileInfo.ID,//fileInfo.FL_PATH,
                                flType = fileInfo.FL_TYPE,
                                flSeq = fileInfo.FL_SEQ.HasValue ? fileInfo.FL_SEQ.Value.ToInt32() : 0,
                                flSize = fileInfo.FL_SIZE.HasValue ? fileInfo.FL_SIZE.Value.ToInt32() : 0,
                                flWidth = fileInfo.FL_WIDTH.HasValue ? fileInfo.FL_WIDTH.Value.ToInt32() : 0,
                                id = fileInfo.ID
                            }));
                            dtoMsg = new DtoMessage<QFFileReadListResultList>()
                            {
                                ReturnObj = listResultList,
                                CurPage = 0,
                                Error = "",
                                PageSize = 0,
                                Status = DtoMessageStatus.Success,
                                TotalNum = 0

                            };
                        }

                    }

                    if (!string.IsNullOrEmpty(error))
                    {
                        dtoMsg.Error = error;
                        dtoMsg.Status = DtoMessageStatus.Fail;
                    }

                    #endregion

                }
                sw.Stop();

                fileLog.AppId = appId;
                fileLog.FileType = fileType;
                fileLog.JavaApiTime = sw.ElapsedMilliseconds;

                retMsg.Status = dtoMsg.Status;
                //请求成功
                if (dtoMsg.Status == DtoMessageStatus.Success)
                {
                    if (dtoMsg.ReturnObj == null || dtoMsg.ReturnObj.flListList == null)
                    {
                        retMsg.ReturnObj = new List<QFFileReadListResult>();
                    }
                    else
                    {
                        //只返回状态为未删除的文件，按顺序升序
                        var returnObj = (from f in dtoMsg.ReturnObj.flListList
                                         where f.status != "N"
                                         orderby f.flSeq ascending
                                         select f).ToList();
                        retMsg.ReturnObj = returnObj;
                    }
                    fileLog.Result = "OK";
                    fileLog.Info += "文件列表获取成功！";
                }
                else
                {
                    retMsg.Error = dtoMsg.Error;
                    retMsg.ReturnObj = new List<QFFileReadListResult>();
                    fileLog.Result = "KO";
                    fileLog.Info += dtoMsg.Error;
                }
            }
            catch (Exception ex)
            {
                retMsg.Status = DtoMessageStatus.Fail;
                retMsg.Error = "参数错误。";
                retMsg.ReturnObj = new List<QFFileReadListResult>();
                fileLog.Result = "KO";
                fileLog.Info += ex.Message;
            }
            return retMsg;
        }

        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="fileId">文件ID</param>
        /// <returns></returns>
        public DtoMessage<String> DeleteFile(string fileId, out QFFileLogTemp fileLog)
        {
            fileLog = new QFFileLogTemp();

            //当前用户对象
            QFUser currentUser = UserService.GetCurrentUser();
            fileLog.City = currentUser.City.CITY_NAME;

            DtoMessage<String> dtoMsg = new DtoMessage<string>();
            dtoMsg.ReturnObj = fileId;
            try
            {
                Dictionary<string, string> dicParameter = new Dictionary<string, string>();
                dicParameter.Add("fileId", fileId);
                dicParameter.Add("loginUser", UserService.GetCurrentUser().Code);
                //计时器
                System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
                sw.Start();
                restHelper = new RestHelper(GlobalSetting.FileRemoveUrl);
                DtoMessage<string> communicateResult = restHelper.Get<string>(string.Empty, dicParameter);
                sw.Stop();
                fileLog.JavaApiTime = sw.ElapsedMilliseconds;
                fileLog.FileId = int.Parse(fileId);

                if (communicateResult.Status == DtoMessageStatus.Success)
                {
                    if (communicateResult.ReturnObj.StartsWith("okTagMsg", StringComparison.InvariantCultureIgnoreCase))
                    {
                        dtoMsg.Status = DtoMessageStatus.Success;
                        fileLog.Result = "OK";
                        fileLog.Info = "删除成功！";
                    }
                    else
                    {
                        dtoMsg.Status = DtoMessageStatus.Fail;
                        dtoMsg.Error = "接口出错，文件删除失败！";
                        fileLog.Result = "KO";
                        fileLog.Info = dtoMsg.Error;
                    }
                }
                else
                {
                    dtoMsg.Status = DtoMessageStatus.Fail;
                    dtoMsg.Error = communicateResult.Error;
                    fileLog.Result = "KO";
                    fileLog.Info = communicateResult.Error;
                }
            }
            catch (Exception ex)
            {
                dtoMsg.Status = DtoMessageStatus.Fail;
                dtoMsg.Error = ex.Message;
                fileLog.Result = "KO";
                fileLog.Info = ex.Message;
            }
            return dtoMsg;
        }

        /// <summary>
        /// URL请求下载文件
        /// </summary>
        /// <param name="context">当前请求</param>
        /// <param name="reqFileName">存储文件名的Request参数（GET/POST）名</param>
        /// <param name="reqFileId">存储文件ID的Request参数（GET/POST）名</param>
        /// <param name="fileLog"></param>
        /// <returns></returns>
        public string DownloadFileByUrl(System.Web.HttpContextBase context, string reqFileName, string reqFileId, out QFFileLogTemp fileLog)
        {
            fileLog = new QFFileLogTemp();

            //当前用户对象
            QFUser currentUser = UserService.GetCurrentUser();
            fileLog.City = currentUser.City.CITY_NAME;

            if (context.Request[reqFileName] == null || context.Request[reqFileId] == null)
            {
                fileLog.Result = "KO";
                fileLog.Info = "直接访问该页导致参数错误！";
                return "错误：参数错误！请勿直接访问该页。";
            }
            string fileName = System.Web.HttpUtility.UrlDecode(context.Request[reqFileName]);
            string fileId = context.Request[reqFileId];

            byte[] buffer;

            //记录请求文件的耗时
            System.Diagnostics.Stopwatch swJava = new System.Diagnostics.Stopwatch();
            swJava.Start();
            try
            {
                using (System.Net.WebClient wc = new System.Net.WebClient())
                {
                    buffer = wc.DownloadData(string.Format(GlobalSetting.FileReadUrl + "?id={0}", fileId));
                }
            }
            catch (Exception ex)
            {
                swJava.Stop();
                Infrastructure.Log4Net.LogWriter.Error(string.Format("进件（appId:未记录）文件类型（fileType:未记录）下载文件（fileId:{0}）失败。", fileId), ex);
                return "错误：下载失败，请稍后重试。如果频繁出错，请与IT部门联系！";
            }
            swJava.Stop();

            //记录Log
            fileLog.FileId = int.Parse(fileId);
            fileLog.FileName = fileName;
            fileLog.FileSize = (buffer.Length / 1024) + "KB";
            fileLog.JavaApiTime = swJava.ElapsedMilliseconds;

            long fileLength = buffer.Length;
            if (fileLength == 0)
            {
                fileLog.Result = "KO";
                fileLog.Info = "下载失败，未找到请求的文件！";
                return "错误：下载失败，未找到您请求的文件！";
            }
            else if (fileLength > (DownloadMaxSize * 1024))
            {
                fileLog.Result = "KO";
                fileLog.Info = string.Format("请求对象大小超过了系统限制（{0}KB），系统拒绝下载！", DownloadMaxSize);
                return "错误：请求对象大小超过了系统限制，如仍要下载此文件，请与IT部门联系！";
            }
            else if (fileLength < 200)
            {
                fileName = "文件过小，请确认是否是您需要的文件，如不能预览请修改为txt查看内容" + fileName.Substring(fileName.LastIndexOf('.'));
            }


            fileLog.Result = "OK";
            fileLog.Info = "成功下载！";

            context.Response.Clear();
            context.Response.ContentType = "application/octet-stream;charset=utf-8";
            context.Response.ContentEncoding = System.Text.Encoding.UTF8;
            if (!context.Request.Browser.Browser.ToLower().Contains("firefox"))
            {
                fileName = System.Web.HttpUtility.UrlEncode(fileName, Encoding.UTF8);
            }
            context.Response.AddHeader("Content-Disposition", "attachment; filename=" + fileName);
            context.Response.AddHeader("Content-Length", fileLength.ToString());
            context.Response.BinaryWrite(buffer);
            context.Response.Flush();
            context.Response.End();

            return string.Empty;
        }


        /// <summary>
        /// 更新FileCheck状态
        /// </summary>
        /// <param name="appId">申请单流水号</param>
        /// <param name="fileType">文件类别</param>
        /// <param name="isUpload">是上传动作触发？</param>
        /// <param name="isSdNrList">是否是补件？</param>
        /// <returns></returns>
        public bool UpdateFileCheck(long appId, string fileType, bool isUpload, bool isSdNrList)
        {
            try
            {
                var fileFind = FileCheckService.Find(f => f.APP_ID == appId && f.FILE_TYPE == fileType);
                if (isUpload)
                {
                    if (!fileFind.Any())
                    {
                        FileCheckService.Add(
                            new APP_FILE_CHECK()
                            {
                                APP_ID = appId,
                                FILE_TYPE = fileType,
                                FILE_IS_UPLOADED = "Y"
                            }
                            );
                        FileCheckService.UnitOfWork.SaveChanges();
                    }
                }
                else
                {
                    QFFileLogTemp fileLog = new QFFileLogTemp();
                    //请求接口查看本进件+文件类别还剩多少文件
                    DtoMessage<List<QFFileReadListResult>> crntFiles = GetFileList(appId, fileType, out fileLog);
                    if (crntFiles.Status == DtoMessageStatus.Success)
                    {
                        if (crntFiles.ReturnObj.Count == 0)
                        {
                            //如果还有记录信息，删除
                            if (fileFind.Any())
                            {
                                FileCheckService.DeleteMultiple(fileFind.ToList());
                                FileCheckService.UnitOfWork.SaveChanges();
                            }
                            //待补件的某类型下如果没有了文件，则将补件队列中的上传时间去掉
                            if (isSdNrList)
                            {
                                ApplyHelper.UpdateNrListUpdateTime(appId, fileType, true);
                            }
                        }
                        else
                        {
                            DateTime? dtmNr = ApplyHelper.GetNrDateApply(appId, fileType);
                            if (isSdNrList && dtmNr.HasValue)
                            {
                                long javaTime = (dtmNr.Value.Ticks - new DateTime(1970, 1, 1, 8, 0, 0).Ticks) / 10000L;
                                //javaTime < createdtime 是补件文件
                                //查找是否还有补件文件
                                QFFileReadListResult nrFile = crntFiles.ReturnObj.Find(file => file.createdTime.ToLong() > javaTime);
                                //待补件的某类型下如果没有了补件文件，则将补件队列中的上传时间去掉
                                if (nrFile == null)
                                {
                                    ApplyHelper.UpdateNrListUpdateTime(appId, fileType, true);
                                }
                            }
                        }
                    }
                    //获取文件数目失败
                    else
                    {
                        return false;
                    }
                }
            }
            catch
            {
                return false;
            }
            return true;
        }


        //返回委托书上传限制
        public Dictionary<string, List<string>> CreditAccount(long appid)
        {
            APP_MAIN main = applicationService.GetAPPMain(appid);
            string creditCode = "";
            Dictionary<string, List<string>> result = new Dictionary<string, List<string>>();
            if (main != null)
            {
                creditCode = main.CREDIT_CHANNEL_CODE;
            }
            if (!string.IsNullOrEmpty(creditCode))
            {
                if (creditCode == CreditChannel.CREDIT_CHANNEL_HAIR.ToString())
                {
                    result.Add("Pboc2", new List<string>() { CreditChannel.CREDIT_CHANNEL_HAIR.ToString(), "4" });
                }
                else if (creditCode == CreditChannel.CREDIT_CHANNEL_HQ.ToString())
                {
                    result.Add("Pboc2", new List<string>() { CreditChannel.CREDIT_CHANNEL_HQ.ToString(), "4" });
                }
            }
            if (result.Count == 0)
            {
                result.Add("Pboc2", new List<string>() { CreditChannel.CREDIT_CHANNEL_OTHER.ToString(), "2" });
            }
            return result;
        }

        #endregion
    }
}
