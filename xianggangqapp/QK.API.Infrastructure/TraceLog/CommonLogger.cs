using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Hosting;
using System.Web;

namespace QK.API.Infrastructure.TraceLog
{
    public class CommonLogger : ILogger
    {
        private readonly string _rootFolder;

        public string RootFolder
        {
            get { return _rootFolder; }
        }

        public CommonLogger()
        {
            _rootFolder = HostingEnvironment.ApplicationPhysicalPath;
        }

        protected virtual string GetFolder()
        {
            var now = DateTime.Now;
            var path1 = string.Format("logs/{0}/{1}/{2}", now.Year, now.Month, now.Day);
            return Path.Combine(RootFolder, path1);
        }

        /// <summary>
        /// 获取文件路径
        /// </summary>
        /// <param name="fileName">fileName=申请单号+Request/Response =>格式：申请单号_Request</param>
        /// <returns></returns>
        protected virtual string GetFilePath(string fileName)
        {
            var now = DateTime.Now;
            var folder = GetFolder();
            int iSerial = 1000;//初始化序列号
            DirectoryInfo dirInfo = new DirectoryInfo(folder);
            foreach (FileInfo file in dirInfo.GetFiles())
            {
                if (file.Name.Contains(fileName))
                {
                    iSerial++;
                }
            }
            var path1 = string.Format(fileName + "{0}.log", now.ToString("yyyyMMddHHmmss") + iSerial);
            return Path.Combine(folder, path1);
        }

        protected virtual string GetLogContent(object message)
        {
            return message.ToString();
        }

        public void Log(object message, string type)
        {
            try
            {
                var folder = GetFolder();
                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }
                string[] str = message.ToString().Split('$');
                string[] ary = str[0].Split(':');
                string fileName = ary.Length > 1 ? ary[ary.Length - 1].Trim() : ary[0].Trim();
                string msgInfo = str[1];
                var file = GetFilePath(fileName);
                if (!File.Exists(file))
                {
                    File.Create(file).Close();
                }
                File.AppendAllText(file, GetLogContent(msgInfo));
            }
            catch (Exception)
            {

            }
        }

        public void LogError(object message)
        {
            this.Log(message, LogLevel.Error.ToString());
        }

        public void LogInfo(object message)
        {
            this.Log(message, LogLevel.Info.ToString());
        }
    }
}
