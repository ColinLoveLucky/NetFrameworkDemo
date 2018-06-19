using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QK.QAPP.Infrastructure
{
    public static class DateTimeExtensions
    {
        public static DateTime ToDate(this DateTime dateTime, int year, int month, int date)
        {
            return new DateTime(year, month, date, dateTime.Hour, dateTime.Minute, dateTime.Second);
        }

        public static bool TimeEquals(this DateTime time, DateTime toCompare)
        {
            return time.Hour == toCompare.Hour && time.Minute == toCompare.Minute;
        }

        public static string ToStr(this DateTime? dateTime)
        {
            if (dateTime == null)
            {
                return string.Empty;
            }
            return dateTime.Value.ToString();
        }

        public static string ToStr(this DateTime? dateTime, string format)
        {
            if (dateTime == null)
            {
                return string.Empty;
            }
            return dateTime.Value.ToString(format);
        }

        /// <summary>
        /// 添加人：leiz
        /// 添加日期：20160113
        /// 描述：将等效的字符串日期转换为yyyy/MM/dd格式
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static string ToDate(this string date)
        {
            if (string.IsNullOrEmpty(date))
            {
                return string.Empty;
            }
            return Convert.ToDateTime(date).ToString("yyyy/MM/dd");
        }
        /// <summary>
        /// 添加人：leiz
        /// 添加日期：20160113
        /// 描述：将等效的字符串日期转换为yyyy/MM/dd HH:mm:ss格式
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static string ToDTime(this string dateTime)
        {
            if (string.IsNullOrEmpty(dateTime))
            {
                return string.Empty;
            }
            return Convert.ToDateTime(dateTime).ToString("yyyy/MM/dd HH:mm:ss");
        }
    }
}
