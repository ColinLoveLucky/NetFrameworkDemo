/*********************
 * 作者：刘成帅
 * 时间：2014/9/10
 * 功能：读取文件的静态方法
**********************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace QK.QAPP.MvcScaffold.DForm
{
    /// <summary>
    /// 读取文件的静态方法
    /// </summary>
    public class FileReader
    {
        /// <summary>
        /// 从文件获取模板
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string GetTmpString(string path)
        {
            try
            {
                using (StreamReader reader = new StreamReader(path))
                {
                    return reader.ReadToEnd();
                }
            }
            catch (Exception)
            {
                return "";
            }

        }
    }
}
