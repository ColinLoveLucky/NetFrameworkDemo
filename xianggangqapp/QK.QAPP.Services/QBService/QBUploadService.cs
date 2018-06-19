using QK.QAPP.Entity;
using QK.QAPP.Global;
using QK.QAPP.Infrastructure;
using QK.QAPP.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace QK.QAPP.Services
{
    public class QBUploadService : IQBUploadService
    {
        #region 实现接口内成员

        #region 属性配置

        /// <summary>
        /// 提供APP_CUSTOMERSERVICE对象
        /// </summary>
        public IAPP_CUSTOMERSERVICE CustomerService
        {
            get;
            set;
        }

        /// <summary>
        /// 提供APP_MAINSERVICE对象
        /// </summary>
        public IAPP_MAINSERVICE MainService
        {
            get;
            set;
        }

        /// <summary>
        /// 提供QFUserService对象
        /// </summary>
        public IQFUserService UserService
        {
            get;
            set;
        }



        /// <summary>
        /// 提供FL_LISTSERVICE对象
        /// </summary>
        public IFL_LISTSERVICE FileService { get; set; }

        /// <summary>
        /// 提供FL_BIZSERVICE对象
        /// </summary>
        public IFL_BIZSERVICE FileBizService { get; set; }

        public IBID_LabelPactService LabelPactService { get; set; }

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
                return GlobalSetting.Con_UploadFileFormat;
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
                return GlobalSetting.Con_UploadMaxSize;
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
                return GlobalSetting.Con_UploadChunkSize;
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
                return GlobalSetting.Con_DownloadMaxSize;
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
                return GlobalSetting.Con_DownloadChunkSize;
            }
        }

        /// <summary>
        /// 显示大图的URL
        /// </summary>
        public string BigPicUrl
        {
            get
            {
                return GlobalSetting.Con_FileReadUrl;
            }
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
        public DtoMessage<ContractResultMsg> SaveFile(System.Web.HttpRequestBase request, string reqFileId, string reqFileName, string bidContractNo, string bidAppCode, string reqFileType, out QBFileLogTemp fileLog)
        {
            fileLog = new QBFileLogTemp();
            fileLog.BizNo = request[bidContractNo]; 
            DtoMessage<ContractResultMsg> dtoMsg = new DtoMessage<ContractResultMsg>();
            ContractResultMsg conResultMsg = new ContractResultMsg();

            if (request.Files.Count == 0)
            {
                dtoMsg.ReturnObj = conResultMsg;
                dtoMsg.Status = DtoMessageStatus.Fail;
                dtoMsg.Error = "There is no file to be uploaded!";
            }
            else
            {
                string fileId = string.Empty;
                string fileName = string.Empty;
                string bidContract_No = string.Empty;//合同编号
                string bidApp_Code = string.Empty;//进件编号
                string fileType = string.Empty;
                if (request[reqFileId] != null && request[bidContractNo] != null && request[bidAppCode] != null)
                {
                    fileType = request[reqFileType].Trim();
                    bidContract_No = request[bidContractNo];//合同编号
                    bidApp_Code = request[bidAppCode];//进件编号
                    /*由于‘刘云松对肖国栋说’加入上传其他选项，此处自定义文件id【otherFileId】，上传时，判断文件id如果是otherFileId，则新生成一个文件id，如果不是，则按照真实的文件id上传*/
                    fileId = request[reqFileId].Trim().Equals("otherFileId", StringComparison.CurrentCultureIgnoreCase) ? CreateFileid(bidContract_No) : request[reqFileId].Trim();

                    try
                    {
                        System.Web.HttpPostedFileBase postedFile = request.Files[0];

                        fileName = string.IsNullOrEmpty(fileName) ? postedFile.FileName : fileName;
                        //IE10浏览器文件名包含全路径
                        if (fileName.LastIndexOf('\\') > -1)
                        {
                            fileName = fileName.Substring(fileName.LastIndexOf('\\') + 1);
                        }
                        if (postedFile.ContentLength > 0)
                        {
                            string[] fileNamestr = fileName.Split('.');
                            if (fileNamestr.Length >= 2)
                            {
                                fileName = fileNamestr[0];
                                fileType = fileNamestr[1];
                            }
                            int fileLength = (int)postedFile.InputStream.Length;
                            byte[] buffer = new byte[fileLength];
                            postedFile.InputStream.Read(buffer, 0, fileLength);
                            //构建请求接口的post对象
                            ContractUploadRequest._BIZ_INFO bizinfo = new ContractUploadRequest._BIZ_INFO();
                            bizinfo.BASE_INFO = new List<BIZ_KEY_VAL> { 
                                new BIZ_KEY_VAL { BIZ_KEY = GlobalSetting.Contract_BIZ_ID, BIZ_VAL = bidApp_Code },
                                new BIZ_KEY_VAL { BIZ_KEY = GlobalSetting.Contract_ID, BIZ_VAL = bidContract_No } 
                            };
                            bizinfo.FILE = new _FILE { FILE_ID = fileId, FILE_TITLE = fileName, FILE_TYPE = fileType, FILE_CONTENT = buffer };
                            ContractUploadRequest conUpload = new ContractUploadRequest();
                            conUpload.APP_ID = GlobalSetting.Contract_APP_ID;
                            conUpload.ACTION = GlobalSetting.Contract_CONT_UPLOAD;
                            conUpload.BIZ_INFO = bizinfo;

                            //计时器
                            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
                            sw.Start();

                            var restHelper = new RestApiHelper(GlobalSetting.ContractUrl);
                            conResultMsg = restHelper.Post<ContractResultMsg>(string.Empty, conUpload);
                            sw.Stop();

                            fileLog.BizNo = bidContract_No;
                            fileLog.FileName = fileName;
                            fileLog.FileSize = (fileLength / 1024).ToString() + "KB";
                            fileLog.FileType = fileType;
                            fileLog.JavaApiTime = sw.ElapsedMilliseconds;
                            if (conResultMsg.RESULT == "SUCCESS")
                            {
                                dtoMsg.Status = DtoMessageStatus.Success;
                                fileLog.FileId = fileId.ToLong();
                                fileLog.Info = "上传成功！";
                                fileLog.Result = "OK";
                            }
                            else
                            {
                                dtoMsg.Status = DtoMessageStatus.Fail;
                                dtoMsg.Error = conResultMsg.CODE;
                                fileLog.Info = conResultMsg.CODE;
                                fileLog.Result = "KO";
                            }
                        }
                    }
                    catch (Exception exIn)
                    {
                        dtoMsg.Status = DtoMessageStatus.Fail;
                        dtoMsg.Error = exIn.Message;
                        fileLog.Info = exIn.Message;
                        fileLog.Result = "KO";
                    }
                }
                else
                {
                    dtoMsg.Status = DtoMessageStatus.Fail;
                    dtoMsg.Error = "文件id为空";
                    fileLog.Result = "KO";
                    fileLog.Info += "文件列表上传失败";
                }
            }
            return dtoMsg;
        }
        public DtoMessage<ContractResultMsg> SaveFile(System.Web.HttpRequestBase request, string reqFileName, string bidContractNo, string bidAppCode, string reqFileType, out QBFileLogTemp fileLog)
        {
            fileLog = new QBFileLogTemp();
            fileLog.BizNo = request[bidContractNo]; 
            DtoMessage<ContractResultMsg> dtoMsg = new DtoMessage<ContractResultMsg>();
            ContractResultMsg conResultMsg = new ContractResultMsg();

            if (request.Files.Count == 0)
            {
                dtoMsg.ReturnObj = conResultMsg;
                dtoMsg.Status = DtoMessageStatus.Fail;
                dtoMsg.Error = "There is no file to be uploaded!";
            }
            else
            {
                string fileId = string.Empty;
                string fileName = string.Empty;
                string bidContract_No = string.Empty;//合同编号
                string bidApp_Code = string.Empty;//进件编号
                string fileType = string.Empty;
                if (request[bidContractNo] != null && request[bidAppCode] != null)
                {
                    fileType = request[reqFileType].Trim();
                    bidContract_No = request[bidContractNo];//合同编号
                    bidApp_Code = request[bidAppCode];//进件编号
                    fileId = CreateFileid(bidContract_No);

                    try
                    {
                        System.Web.HttpPostedFileBase postedFile = request.Files[0];

                        fileName = string.IsNullOrEmpty(fileName) ? postedFile.FileName : fileName;
                        //IE10浏览器文件名包含全路径
                        if (fileName.LastIndexOf('\\') > -1)
                        {
                            fileName = fileName.Substring(fileName.LastIndexOf('\\') + 1);
                        }
                        if (postedFile.ContentLength > 0)
                        {
                            string[] fileNamestr = fileName.Split('.');
                            if (fileNamestr.Length >= 2)
                            {
                                fileName = fileNamestr[0];
                                fileType = fileNamestr[1];
                            }
                            int fileLength = (int)postedFile.InputStream.Length;
                            byte[] buffer = new byte[fileLength];
                            postedFile.InputStream.Read(buffer, 0, fileLength);
                            //构建请求接口的post对象
                            ContractUploadRequest._BIZ_INFO bizinfo = new ContractUploadRequest._BIZ_INFO();
                            bizinfo.BASE_INFO = new List<BIZ_KEY_VAL> { 
                                new BIZ_KEY_VAL { BIZ_KEY = GlobalSetting.Contract_BIZ_ID, BIZ_VAL = bidApp_Code },
                                new BIZ_KEY_VAL { BIZ_KEY = GlobalSetting.Contract_ID, BIZ_VAL = bidContract_No } 
                            };
                            bizinfo.FILE = new _FILE { FILE_ID = fileId, FILE_TITLE = fileName, FILE_TYPE = fileType, FILE_CONTENT = buffer };
                            ContractUploadRequest conUpload = new ContractUploadRequest();
                            conUpload.APP_ID = GlobalSetting.Contract_APP_ID;
                            conUpload.ACTION = GlobalSetting.Contract_CONT_UPLOAD;
                            conUpload.BIZ_INFO = bizinfo;

                            //计时器
                            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
                            sw.Start();

                            var restHelper = new RestApiHelper(GlobalSetting.ContractUrl);
                            conResultMsg = restHelper.Post<ContractResultMsg>(string.Empty, conUpload);
                            sw.Stop();

                            fileLog.BizNo = bidContract_No;
                            fileLog.FileName = fileName;
                            fileLog.FileSize = (fileLength / 1024).ToString() + "KB";
                            fileLog.FileType = fileType;
                            fileLog.JavaApiTime = sw.ElapsedMilliseconds;
                            if (conResultMsg.RESULT == "SUCCESS")
                            {
                                dtoMsg.Status = DtoMessageStatus.Success;
                                fileLog.FileId = fileId.ToLong();
                                fileLog.Info = "上传成功！";
                                fileLog.Result = "OK";
                            }
                            else
                            {
                                dtoMsg.Status = DtoMessageStatus.Fail;
                                dtoMsg.Error = conResultMsg.CODE;
                                fileLog.Info = conResultMsg.CODE;
                                fileLog.Result = "KO";
                            }
                        }
                    }
                    catch (Exception exIn)
                    {
                        dtoMsg.Status = DtoMessageStatus.Fail;
                        dtoMsg.Error = exIn.Message;
                        fileLog.Info = exIn.Message;
                        fileLog.Result = "KO";
                    }
                }
                else
                {
                    dtoMsg.Status = DtoMessageStatus.Fail;
                    dtoMsg.Error = "文件id为空";
                    fileLog.Result = "KO";
                    fileLog.Info += "文件列表上传失败";
                }
            }
            return dtoMsg;
        }

        /// <summary>
        /// 排序
        /// </summary>
        /// <param name="sortedIds">新顺序</param>
        /// <param name="fileLog"></param>
        /// <returns></returns>
        public DtoMessage<string> SaveNewSort(string sortedIds, out QBFileLogTemp fileLog)
        {
            fileLog = new QBFileLogTemp();

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
        /// <param name="bidContractNo">合同编号</param>
        /// <param name="fileType">文件类别</param>
        /// <param name="fileLog"></param>
        /// <returns></returns>
        public DtoMessage<ContractResultMsg> GetFileList(string bidContractNo, string bidAppCode, string fileType, out QBFileLogTemp fileLog)
        {
            fileLog = new QBFileLogTemp();
            fileLog.BizNo = bidContractNo;
            DtoMessage<ContractResultMsg> retMsg = new DtoMessage<ContractResultMsg>();
            ContractResultMsg conResultMsg = new ContractResultMsg();
            try
            {
                //创建请求对象
                ContractViewRequest._BIZ_INFO bizInfo = new ContractViewRequest._BIZ_INFO();
                bizInfo.BASE_INFO =
                    new List<BIZ_KEY_VAL>()
                    {
                        new BIZ_KEY_VAL(){
                        BIZ_KEY = GlobalSetting.Contract_BIZ_ID,
                        BIZ_VAL = bidAppCode
                        },
                            new BIZ_KEY_VAL(){
                        BIZ_KEY = GlobalSetting.Contract_ID,
                        BIZ_VAL = bidContractNo
                        }
                    
                };
                ContractViewRequest contractView = new ContractViewRequest
                {
                    APP_ID = GlobalSetting.Contract_APP_ID,
                    ACTION = GlobalSetting.Contract_CONT_VIEW,
                    BIZ_INFO = bizInfo
                };

                //计时器
                System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
                sw.Start();
                #region 用接口访问
                RestApiHelper restHelper = new RestApiHelper(GlobalSetting.ContractUrl);
                //请求获取接口
                conResultMsg = restHelper.Post<ContractResultMsg>(string.Empty, contractView);
                #endregion


                sw.Stop();

                //请求成功
                if (conResultMsg.RESULT == "SUCCESS")
                {
                    retMsg.ReturnObj = conResultMsg;
                    retMsg.Status = DtoMessageStatus.Success;
                    fileLog.Result = "OK";
                    fileLog.Info += "文件列表获取成功";
                }
                else
                {
                    retMsg.Status = DtoMessageStatus.Fail;
                    retMsg.Error = conResultMsg.CODE + "参数不正确";
                    retMsg.ReturnObj = conResultMsg;
                    fileLog.Result = "KO";
                    fileLog.Info += "文件列表获取失败";
                }
            }
            catch (Exception ex)
            {
                retMsg.Status = DtoMessageStatus.Fail;
                retMsg.Error = "参数错误。";
                retMsg.ReturnObj = conResultMsg;
                fileLog.Result = "KO";
                fileLog.Info += "文件列表获取失败";
            }
            return retMsg;
        }
        /// <summary>
        /// 文件下载列表
        /// </summary>
        /// <param name="bidContractNo"></param>
        /// <param name="bidAppCode"></param>
        /// <param name="fileLog"></param>
        /// <returns></returns>
        public DtoMessage<ContractResultMsg> GetDownloadFileUrl(string bidContractNo, string bidAppCode, out QBFileLogTemp fileLog)
        {
            fileLog = new QBFileLogTemp();
            fileLog.BizNo = bidContractNo;
            DtoMessage<ContractResultMsg> retMsg = new DtoMessage<ContractResultMsg>();
            ContractResultMsg conResultMsg = new ContractResultMsg();
            try
            {
                //创建请求对象
                ContractDownLoadRequest._BIZ_INFO bizInfo = new ContractDownLoadRequest._BIZ_INFO();
                bizInfo.BASE_INFO =
                    new List<BIZ_KEY_VAL>()
                    {
                        new BIZ_KEY_VAL(){
                        BIZ_KEY = GlobalSetting.Contract_BIZ_ID,
                        BIZ_VAL = bidAppCode
                        },
                            new BIZ_KEY_VAL(){
                        BIZ_KEY = GlobalSetting.Contract_ID,
                        BIZ_VAL = bidContractNo
                        }
                    
                };
                ContractDownLoadRequest contractDownLoad = new ContractDownLoadRequest
                {
                    APP_ID = GlobalSetting.Contract_APP_ID,
                    ACTION = GlobalSetting.Contract_CONT_DOWNLOAD,
                    BIZ_INFO = bizInfo
                };
                //计时器
                System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
                sw.Start();
                #region 用接口访问
                RestApiHelper restHelper = new RestApiHelper(GlobalSetting.ContractUrl);
                //请求获取接口
                conResultMsg = restHelper.Post<ContractResultMsg>(string.Empty, contractDownLoad);
                #endregion


                sw.Stop();

                //请求成功
                if (conResultMsg.RESULT == "SUCCESS")
                {
                    retMsg.ReturnObj = conResultMsg;
                    retMsg.Status = DtoMessageStatus.Success;
                    fileLog.Result = "OK";
                    fileLog.Info += "文件列表获取成功";
                }
                else
                {
                    retMsg.Status = DtoMessageStatus.Fail;
                    retMsg.Error = conResultMsg.CODE + "参数不正确";
                    retMsg.ReturnObj = conResultMsg;
                    fileLog.Result = "KO";
                    fileLog.Info += "文件列表获取失败";
                }
            }
            catch (Exception ex)
            {
                retMsg.Status = DtoMessageStatus.Fail;
                retMsg.Error = "参数错误。";
                retMsg.ReturnObj = conResultMsg;
                fileLog.Result = "KO";
                fileLog.Info += "文件列表获取失败";
            }
            return retMsg;
        }
        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="fileId">文件ID</param>
        /// <returns></returns>
        public DtoMessage<ContractResultMsg> DeleteFile(string bidContractNo, string bidAppCode, string fileId, string fileType, out QBFileLogTemp fileLog)
        {

            fileLog = new QBFileLogTemp();
            fileLog.BizNo = bidContractNo;
            DtoMessage<ContractResultMsg> retMsg = new DtoMessage<ContractResultMsg>();
            ContractResultMsg conResultMsg = new ContractResultMsg();
            try
            {

                //创建请求对象
                ContractDeleteRequest._FILE file = new ContractDeleteRequest._FILE() { FILE_ID = fileId, KIND = fileType };
                Dictionary<string, ContractDeleteRequest._FILE> dicfile = new Dictionary<string, ContractDeleteRequest._FILE>();
                dicfile.Add("FILE1", file);
                ContractDeleteRequest._BIZ_INFO bizInfo = new ContractDeleteRequest._BIZ_INFO();
                bizInfo.BASE_INFO =
                    new List<BIZ_KEY_VAL>()
                    {
                        new BIZ_KEY_VAL(){
                        BIZ_KEY = GlobalSetting.Contract_BIZ_ID,
                        BIZ_VAL = bidAppCode
                        },
                            new BIZ_KEY_VAL(){
                        BIZ_KEY = GlobalSetting.Contract_ID,
                        BIZ_VAL = bidContractNo
                        }
                    
                };
                bizInfo.FILES = new List<Dictionary<string, ContractDeleteRequest._FILE>> { dicfile };
                ContractDeleteRequest contractDelete = new ContractDeleteRequest
                {
                    APP_ID = GlobalSetting.Contract_APP_ID,
                    ACTION = GlobalSetting.Contract_CONT_DELETE,
                    BIZ_INFO = bizInfo
                };
                //计时器
                System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
                sw.Start();
                RestApiHelper restHelper = new RestApiHelper(GlobalSetting.ContractUrl);
                conResultMsg = restHelper.Post<ContractResultMsg>(string.Empty, contractDelete);
                sw.Stop();
                //请求成功
                if (conResultMsg.RESULT == "SUCCESS")
                {
                    retMsg.ReturnObj = conResultMsg;
                    retMsg.Status = DtoMessageStatus.Success;
                    fileLog.Result = "OK";
                    fileLog.Info = "删除成功！";
                }
                else
                {
                    retMsg.Status = DtoMessageStatus.Fail;
                    retMsg.Error = conResultMsg.CODE + "参数不正确";
                    retMsg.ReturnObj = conResultMsg;
                    fileLog.Result = "KO";
                    fileLog.Info = "接口出错，文件删除失败！";
                }

            }
            catch (Exception ex)
            {
                retMsg.Status = DtoMessageStatus.Fail;
                retMsg.Error = ex.Message;
                retMsg.ReturnObj = conResultMsg;
                fileLog.Result = "KO";
                fileLog.Info = ex.Message;
            }
            return retMsg;
        }
        /// <summary>
        /// 合同手动签章
        /// </summary>
        /// <param name="fileId">文件ID</param>
        /// <returns></returns>
        public DtoMessage<ContractResultMsg> GetContractManualSign(string bidContractNo, string bidAppCode, string fileId, out QBFileLogTemp fileLog)
        {

            fileLog = new QBFileLogTemp();
            fileLog.BizNo = bidContractNo;
            DtoMessage<ContractResultMsg> retMsg = new DtoMessage<ContractResultMsg>();
            ContractResultMsg conResultMsg = new ContractResultMsg();
            try
            {
                //创建请求对象
                ContractManualSign._FILESign file = new ContractManualSign._FILESign { FILE_ID = fileId };
                ContractManualSign._BIZ_INFO bizInfo = new ContractManualSign._BIZ_INFO();
                bizInfo.BASE_INFO =
                    new List<BIZ_KEY_VAL>()
                    {
                        new BIZ_KEY_VAL(){
                        BIZ_KEY = GlobalSetting.Contract_BIZ_ID,
                        BIZ_VAL = bidAppCode
                        },
                            new BIZ_KEY_VAL(){
                        BIZ_KEY = GlobalSetting.Contract_ID,
                        BIZ_VAL = bidContractNo
                        }
                    
                };
                bizInfo.FILE = file;
                ContractManualSign contractManualSign = new ContractManualSign
                {
                    APP_ID = GlobalSetting.Contract_APP_ID,
                    ACTION = GlobalSetting.Contract_MANUAL_SIGN,
                    RETURN_URL = "#",
                    BIZ_INFO = bizInfo
                };
                //计时器
                System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
                sw.Start();
                RestApiHelper restHelper = new RestApiHelper(GlobalSetting.ContractUrl);
                conResultMsg = restHelper.Post<ContractResultMsg>(string.Empty, contractManualSign);
                sw.Stop();
                //请求成功
                if (conResultMsg.RESULT == "SUCCESS")
                {
                    retMsg.ReturnObj = conResultMsg;
                    retMsg.Status = DtoMessageStatus.Success;
                    fileLog.Result = "OK";
                    fileLog.Info = "合同手动签章！";
                }
                else
                {
                    retMsg.Status = DtoMessageStatus.Fail;
                    retMsg.Error = conResultMsg.CODE + "参数不正确";
                    retMsg.ReturnObj = conResultMsg;
                    fileLog.Result = "KO";
                    fileLog.Info = "接口出错，合同手动签章！";
                }

            }
            catch (Exception ex)
            {
                retMsg.Status = DtoMessageStatus.Fail;
                retMsg.Error = ex.Message;
                retMsg.ReturnObj = conResultMsg;
                fileLog.Result = "KO";
                fileLog.Info = ex.Message;
            }
            return retMsg;
        }
        /// <summary>
        /// URL请求下载文件
        /// </summary>
        /// <param name="context">当前请求</param>
        /// <param name="reqFileName">存储文件名的Request参数（GET/POST）名</param>
        /// <param name="reqFileId">存储文件ID的Request参数（GET/POST）名</param>
        /// <param name="fileLog"></param>
        /// <returns></returns>
        public string DownloadFileByUrl(System.Web.HttpContextBase context, string reqFileName, string reqSrc, string reqFileId, out QBFileLogTemp fileLog)
        {
            fileLog = new QBFileLogTemp();

            if (context.Request[reqFileName] == null || context.Request[reqFileId] == null || context.Request[reqSrc] == null)
            {
                fileLog.Result = "KO";
                fileLog.Info = "直接访问该页导致参数错误！";
                return "错误：参数错误！请勿直接访问该页。";
            }
            string fileName = System.Web.HttpUtility.UrlDecode(context.Request[reqFileName]).Replace(" ", "");
            string fileId = context.Request[reqFileId];
            string downloadUrl = System.Web.HttpUtility.UrlDecode(context.Request[reqSrc]);//文件http下载地址

            byte[] buffer;

            //记录请求文件的耗时
            System.Diagnostics.Stopwatch swJava = new System.Diagnostics.Stopwatch();
            swJava.Start();
            try
            {
                using (System.Net.WebClient wc = new System.Net.WebClient())
                {
                    buffer = wc.DownloadData(downloadUrl);
                }
            }
            catch (Exception ex)
            {
                swJava.Stop();
                Infrastructure.Log4Net.LogWriter.Error(string.Format("合同号（bidContractNo:{0}）文件类型（fileType:未记录）下载文件（fileId:{1}）失败。", context.Request["bidCode"], fileId), ex);
                return "错误：下载失败，请稍后重试。如果频繁出错，请与IT部门联系！";
            }
            swJava.Stop();

            //记录Log
            //fileLog.FileId = int.Parse(fileId);
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
        /// 补件生成文件id
        /// </summary>
        /// <param name="bidContractNo">合同编号</param>
        /// <returns></returns>
        public string CreateFileid(string bidContractNo)
        {
            return GlobalSetting.Contract_APP_ID + "_" + bidContractNo + "_" + Guid.NewGuid();
        }
        /// <summary>
        /// 判断是否是补录上传的文件
        /// </summary>
        /// <param name="fileId">文件id</param>
        /// <param name="bidContractNo">合同号</param>
        /// <returns></returns>
        public bool IsAdditionalFile(string fileId, string bidContractNo)
        {
            bool isDel = false;
            if (!string.IsNullOrWhiteSpace(fileId))
            {
                string[] temp = fileId.Split(new char[] { '_' }, StringSplitOptions.RemoveEmptyEntries);
                if (temp.Length >= 3)
                {
                    if (GlobalSetting.Contract_APP_ID.Equals(temp[0]) && bidContractNo.Equals(temp[1]) && IsGuid(temp[2]))
                    {
                        isDel = true;
                    }
                }
                else
                    return isDel;
            }
            return isDel;
        }
        /// <summary>
        /// 验证guid
        /// </summary>
        /// <param name="strSrc"></param>
        /// <returns></returns>
        public bool IsGuid(string strSrc)
        {
            Guid g = Guid.Empty;
            return Guid.TryParse(strSrc, out g);

        }

        #endregion
        
        /// <summary>
        /// 获取合同手动签章更新URL
        /// </summary>
        /// <param name="bidContractNo">合同号</param>
        /// <param name="bidAppCode">申请号</param>
        /// <param name="fileId">文件ID</param>
        /// <param name="fileLog">fileLog</param>
        /// <returns></returns>
        public DtoMessage<ContractResultMsg> GetUpdateUrl(string bidContractNo, string bidAppCode, string fileId, ref QBFileLogTemp fileLog)
        {
            DtoMessage<ContractResultMsg> retMsg = new DtoMessage<ContractResultMsg>();
            ContractResultMsg conResultMsg = new ContractResultMsg();
            try
            {
                //创建请求对象
                ContractSignUrl._BIZ_INFO bizInfo = new ContractSignUrl._BIZ_INFO();
                bizInfo.BASE_INFO =
                    new List<BIZ_KEY_VAL>()
                    {
                        new BIZ_KEY_VAL(){
                        BIZ_KEY = GlobalSetting.Contract_BIZ_ID,
                        BIZ_VAL = bidAppCode
                        },
                            new BIZ_KEY_VAL(){
                        BIZ_KEY = GlobalSetting.Contract_ID,
                        BIZ_VAL = bidContractNo
                        }
                    
                };
                bizInfo.FILE = new ContractSignUrl._FILE() { FILE_ID = fileId };

                ContractSignUrl csu = new ContractSignUrl()
                {
                    APP_ID = GlobalSetting.Contract_APP_ID,
                    ACTION = GlobalSetting.Contract_UPDATE_MANUAL_SIGN_URL,
                    BIZ_INFO = bizInfo
                };
                var rest = new RestApiHelper(GlobalSetting.ContractUrl);
                conResultMsg = rest.Post<ContractResultMsg>(string.Empty, csu);
                //请求成功
                if (conResultMsg.RESULT == "SUCCESS")
                {
                    retMsg.Status = DtoMessageStatus.Success;
                    retMsg.ReturnObj = conResultMsg;
                    fileLog.Result = "OK";
                    fileLog.Info += "手动签章URL更新接口调用成功";
                }
                else
                {
                    retMsg.Status = DtoMessageStatus.Fail;
                    retMsg.Error = conResultMsg.CODE;
                    retMsg.ReturnObj = conResultMsg;
                    fileLog.Result = "KO";
                    fileLog.Info += "手动签章URL更新接口调用失败";
                }
            }
            catch (Exception ex)
            {
                retMsg.Status = DtoMessageStatus.Fail;
                retMsg.Error = ex.Message;
                retMsg.ReturnObj = conResultMsg;
                fileLog.Result = "KO";
                fileLog.Info = ex.Message;
            }
            return retMsg;
        }
    }
}
