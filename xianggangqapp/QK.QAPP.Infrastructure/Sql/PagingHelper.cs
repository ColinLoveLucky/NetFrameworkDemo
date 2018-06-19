using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QK.QAPP.Infrastructure.Sql
{
    public class PagingHelper
    {
        /// <summary>
        /// 获取分页数据Sql
        /// </summary>
        /// <param name="selectField">查询字段</param>
        /// <param name="resultField">最终显示字段</param>
        /// <param name="fromStr">数据表sql</param>
        /// <param name="sortStr">排序字段</param>
        /// <param name="whereStr">条件sql</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">每页数</param>
        /// <returns>sql查询语句</returns>
        public static string GetPagingDataSql(string selectField, string resultField, string fromStr, string sortStr, string whereStr, int pageIndex, int pageSize)
        {
            int beginNum = (pageIndex - 1) * pageSize;  //开始行数
            int endNum = pageSize * pageIndex;  //结束行数
            var sb = new StringBuilder();
            sb.Append("select ");
            sb.Append(resultField);
            sb.Append(" from (");

            sb.Append("select ");
            sb.Append(selectField);
            sb.Append(",row_number() over(order by ");  //roe_number()排序
            sb.Append(sortStr);
            sb.Append(") as row_number from ");
            sb.Append(fromStr);
            if (!string.IsNullOrEmpty(whereStr))  //加where条件
            {
                sb.Append(" where 1=1 ");
                sb.Append(whereStr);
            }
            sb.Append(") where row_number>" + beginNum + " and row_number<="+endNum);
            return sb.ToString();
        }

        /// <summary>
        /// 获取分页总数Sql
        /// </summary>
        /// <param name="fromStr">数据表sql</param>
        /// <param name="whereStr">条件sql</param>
        /// <returns>>sql查询语句</returns>
        public static string GetPagingCountSql(string fromStr, string whereStr)
        {
            var sb = new StringBuilder();
            sb.Append("select count (1) from ");
            sb.Append(fromStr);
            if (!string.IsNullOrEmpty(whereStr))  //加where条件
            {
                sb.Append(" where 1=1 ");
                sb.Append(whereStr);
            }
            return sb.ToString();
        }
    }
}
