/***********************
 * 作    者：刘云松
 * 创建时间：‎2014‎-0‎9-‎12‎ ‏‎15:08:50
 * 作    用：对接Java影像接口实现文件的上传、下载、读取、删除、排序等功能
*****************************/
using QK.QAPP.Entity;
using QK.QAPP.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QK.QAPP.IServices
{
    public interface IQFUploadService
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

        /// <summary>
        /// 待补件状态
        /// </summary>
        Dictionary<string, string> Order_SD_Status_Need { get; }

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
        DtoMessage<QFFile> SaveFile(System.Web.HttpRequestBase request, string reqFileName, string reqAppId, string reqFileType, out QFFileLogTemp fileLog);

        /// <summary>
        /// 保存新排序
        /// </summary>
        /// <param name="sortedIds">新顺序</param>
        /// <param name="appId">申请单流水号</param>
        /// <param name="fileType">证件类别</param>
        /// <param name="fileLog"></param>
        /// <returns></returns>
        DtoMessage<String> SaveNewSort(string sortedIds, out QFFileLogTemp fileLog);

        /// <summary>
        /// 读取文件列表
        /// </summary>
        /// <param name="appId">申请单流水号</param>
        /// <param name="fileType">文件类别</param>
        /// <param name="fileLog"></param>
        /// <returns></returns>
        DtoMessage<List<QFFileReadListResult>> GetFileList(long appId, string fileType, out QFFileLogTemp fileLog);

        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="fileId">文件ID</param>
        /// <param name="fileLog"></param>
        /// <returns></returns>
        DtoMessage<String> DeleteFile(string fileId, out QFFileLogTemp fileLog);

        /// <summary>
        /// URL请求下载文件。如成功下载，返回string.empty；否则返回错误。
        /// </summary>
        /// <param name="context">当前请求</param>
        /// <param name="reqFileName">存储文件名的Request参数（GET/POST）名</param>
        /// <param name="reqFileId">存储文件ID的Request参数（GET/POST）名</param>
        /// <param name="fileLog"></param>
        /// <returns></returns>
        string DownloadFileByUrl(System.Web.HttpContextBase context, string reqFileName, string reqFileId, out QFFileLogTemp fileLog);

        /// <summary>
        /// 更新FileCheck状态
        /// </summary>
        /// <param name="appId">申请单流水号</param>
        /// <param name="fileType">文件类别</param>
        /// <param name="isUpload">是上传动作触发？</param>
        /// <param name="isSdNrList">是否是补件？</param>
        /// <returns></returns>
        bool UpdateFileCheck(long appId, string fileType, bool isUpload, bool isSdNrList);

        /// <summary>
        /// 返回委托书上传限制
        /// </summary>
        /// <param name="appid"></param>
        /// <returns></returns>
        Dictionary<string, List<string>> CreditAccount(long appid);
    }
}
