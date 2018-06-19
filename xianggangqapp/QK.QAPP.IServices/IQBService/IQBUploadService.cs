using QK.QAPP.Entity;
using QK.QAPP.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QK.QAPP.IServices
{
    public interface IQBUploadService
    {
        #region 属性配置

        /// <summary>
        /// 文件上传格式
        /// </summary>
        /// <returns></returns>
        string UploadFileFormat { get; }

        /// <summary>
        /// 允许上传的最大文件（单位：KB）
        /// </summary>
        /// <returns></returns>
        int UploadMaxSize { get; }

        /// <summary>
        /// 分块上传时每块大小（单位：KB）
        /// </summary>
        /// <returns></returns>
        int UploadChunkSize { get; }

        /// <summary>
        /// 允许下载的最大文件（单位：KB）
        /// </summary>
        /// <returns></returns>
        int DownloadMaxSize { get; }

        /// <summary>
        /// 分块下载时每块大小（单位：KB）
        /// </summary>
        /// <returns></returns>
        int DownloadChunkSize { get; }

        /// <summary>
        /// 显示大图的URL
        /// </summary>
        string BigPicUrl { get; }


        #endregion

        /// <summary>
        /// 保存文件
        /// </summary>
        /// <param name="request">发送请求的HttpRequestBase</param>
        /// <param name="reqFileName">保存文件名的Request（GET/POST）参数名</param>
        /// <param name="reqAppId">保存申请单流水号的Request（GET/POST）参数名</param>
        /// <param name="reqFileType">保存证件类型的Request（GET/POST）参数名</param>
        /// <param name="fileLog"></param>
        /// <returns></returns>
        DtoMessage<ContractResultMsg> SaveFile(System.Web.HttpRequestBase request, string reqFileId,string reqFileName, string bidContractNo, string bidAppCode,string reqFileType, out QBFileLogTemp fileLog);
        DtoMessage<ContractResultMsg> SaveFile(System.Web.HttpRequestBase request, string reqFileName, string bidContractNo, string bidAppCode, string reqFileType, out QBFileLogTemp fileLog);
        /// <summary>
        /// 保存新排序
        /// </summary>
        /// <param name="sortedIds">新顺序</param>
        /// <param name="appId">申请单流水号</param>
        /// <param name="fileType">证件类别</param>
        /// <param name="fileLog"></param>
        /// <returns></returns>
        DtoMessage<String> SaveNewSort(string sortedIds, out QBFileLogTemp fileLog);

        /// <summary>
        /// 读取文件列表
        /// </summary>
        /// <param name="bidContractNo">合同编号</param>
        /// <param name="fileType">文件类别</param>
        /// <param name="fileLog"></param>
        /// <returns></returns>
        DtoMessage<ContractResultMsg> GetFileList(string bidContractNo, string bidAppCode, string fileType, out QBFileLogTemp fileLog);
        /// <summary>
        /// 文件下载列表
        /// </summary>
        /// <param name="bidContractNo"></param>
        /// <param name="bidAppCode"></param>
        /// <param name="fileLog"></param>
        /// <returns></returns>
        DtoMessage<ContractResultMsg> GetDownloadFileUrl(string bidContractNo, string bidAppCode, out QBFileLogTemp fileLog);

        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="fileId">文件ID</param>
        /// <param name="fileLog"></param>
        /// <returns></returns>
        DtoMessage<ContractResultMsg> DeleteFile(string bidContractNo, string bidAppCode, string fileId,string fileType, out QBFileLogTemp fileLog);
        /// <summary>
        /// 合同手动签章
        /// </summary>
        /// <param name="fileId">文件ID</param>
        /// <returns></returns>
        DtoMessage<ContractResultMsg> GetContractManualSign(string bidContractNo, string bidAppCode, string fileId, out QBFileLogTemp fileLog);

        /// <summary>
        /// URL请求下载文件。如成功下载，返回string.empty；否则返回错误。
        /// </summary>
        /// <param name="context">当前请求</param>
        /// <param name="reqFileName">存储文件名的Request参数（GET/POST）名</param>
        /// <param name="reqFileId">存储文件ID的Request参数（GET/POST）名</param>
        /// <param name="fileLog"></param>
        /// <returns></returns>
        string DownloadFileByUrl(System.Web.HttpContextBase context, string reqFileName, string reqSrc, string reqFileId, out QBFileLogTemp fileLog);
        /// <summary>
        /// 判断是否是补录文件  true:补录文件
        /// </summary>
        /// <param name="fileId">文件id</param>
        /// <param name="bidContractNo">合同号</param>
        /// <returns></returns>
        bool IsAdditionalFile(string fileId, string bidContractNo);

        /// <summary>
        /// 获取合同手动签章更新URL
        /// </summary>
        /// <param name="bidContractNo">合同号</param>
        /// <param name="bidAppCode">申请号</param>
        /// <param name="fileId">文件ID</param>
        /// <param name="fileLog">fileLog</param>
        /// <returns></returns>
        DtoMessage<ContractResultMsg> GetUpdateUrl(string bidContractNo, string bidAppCode, string fileId, ref QBFileLogTemp fileLog);
    }
}
