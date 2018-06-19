using System;
using System.Drawing;
using System.Text.RegularExpressions;

namespace QK.QAPP.Infrastructure
{
    public static class StringExtensions
    {
        public static bool Eq(this string input, string toCompare, StringComparison comparison = StringComparison.OrdinalIgnoreCase)
        {
            return input.Equals(toCompare, comparison);
        }

        public static string ToWords(this string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return input;
            }
            var chars = input.ToCharArray();
            var result = string.Empty;
            foreach (var c in chars)
            {
                var str = c.ToString();
                if (str == str.ToUpper())
                {
                    result += " ";
                }
                result += str;
            }
            return result;
        }

        /// <summary>
        /// 将字符串转换为Int
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static int ToInt(this string input)
        {
            int ret = 0;
            try
            {
                int.TryParse(input, out ret);
            }
            catch (Exception ex)
            {
                throw new ArgumentNullException(ex.Message);
            }
            return ret;
        }

        /// <summary>
        /// 将字符串转换为Int
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static long ToLong(this string input)
        {
            long ret = 0;
            try
            {
                long.TryParse(input, out ret);
            }
            catch (Exception ex)
            {
                throw new ArgumentNullException(ex.Message);
            }
            return ret;
        }
        /// <summary>
        /// 将字符串转化为Bool
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool ToBoolean(this string input)
        {
            bool ret = false;
            try
            {
                if (input == "1") return true;
                else if (input == "0") return false;
                else
                {
                    Boolean.TryParse(input, out ret);
                }

            }
            catch (Exception ex)
            {
                throw new ArgumentNullException(ex.Message);
            }
            return ret;
        }

        /// <summary>
        /// 是否为null
        /// </summary>
        /// <param name="value">值</param>
        /// <returns>是返回true,否则返回false</returns>
        public static bool IsNull(this Object value)
        {
            return value == null;
        }

        /// <summary>
        /// 是否为DBNull.Value
        /// </summary>
        /// <param name="value">值</param>
        /// <returns>是返回true,否则返回false</returns>
        public static bool IsDBNull(this Object value)
        {
            return value == DBNull.Value;
        }

        /// <summary>
        /// 是否为null或String.Empty
        /// </summary>
        /// <param name="value">值</param>
        /// <returns>是返回true,否返回false</returns>
        public static bool IsNullOrEmpty(this Object value)
        {
            if (value != null)
                return value.ToString().Length == 0;
            return true;
        }

        /// <summary>
        /// 是否为null或DBNull.Value
        /// </summary>
        /// <param name="value">值</param>
        /// <returns>是返回true,否则返回false</returns>
        public static bool IsNullOrDBNull(this Object value)
        {
            return value == null || value == DBNull.Value;
        }

        /// <summary>
        /// 是否为默认值.(null或DBNull.Value为任意类型默认值,String类型时null或DBNull.Value或String.Empty均返回true,引用类型默认值为null)
        /// </summary>
        /// <param name="value">值</param>
        /// <returns>是返回true,否则返回false</returns>
        public static bool IsDefaultValue(this Object value)
        {
            if (value.IsNullOrDBNull())
                return true;

            Type type = value.GetType();
            if (type.IsValueType)
            {
                return value == Activator.CreateInstance(type);
            }
            else
            {
                if (type == typeof(string) || type.IsSubclassOf(typeof(string)))
                    return string.IsNullOrEmpty(Convert.ToString(value));
                else
                    return false;
            }
        }

        public static DateTime DefaultDbDateTime = new DateTime(1753, 1, 1);

        /// <summary>
        /// 获取指定类型的默认值.引用类型(包含String)的默认值为null
        /// </summary>
        /// <returns>默认值</returns>
        public static T DefaultValue<T>()
        {
            return default(T);
        }

        /// <summary>
        /// 获取指定类型的默认值.引用类型(包含String)的默认值为null
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns>默认值</returns>
        public static Object DefaultValue(this Type type)
        {
            return type.IsValueType ? Activator.CreateInstance(type) : null;
        }


        /// <summary>
        /// 转换到指定类型,强转换.(null,DbNull.Value时返回默认值,转换失败抛出异常)
        /// </summary>
        /// <param name="value">值</param>
        /// <returns>目标类型的值</returns>
        public static T ChangeType<T>(this Object value)
        {
            return value.IsNullOrDBNull() ? default(T) : (T)Convert.ChangeType(value, typeof(T));
        }

        /// <summary>
        /// 转换到指定类型,强转换.(null,DbNull.Value时返回默认值,转换失败抛出异常)
        /// </summary>
        /// <param name="value">值</param>
        /// <param name="type">目标类型</param>
        /// <returns>目标类型的值</returns>
        public static Object ChangeType(this Object value, Type type)
        {
            return value.IsNullOrDBNull() ? type.DefaultValue() : Convert.ChangeType(value, type);
        }


        /// <summary>
        /// 如果value为null或DBNull.Value则返回默认值,否则返回原值.默认值:Object.DefaultValue()
        /// </summary>
        /// <param name="value">Object对象</param>
        /// <returns>处理后值</returns>
        public static T ToDefaultableValue<T>(this Object value)
        {
            return value.IsNullOrDBNull() ? default(T) : (T)value;
        }

        /// <summary>
        /// 如果value为null或DBNull.Value则返回默认值,否则返回原值.默认值:Object.DefaultValue()
        /// </summary>
        /// <param name="value">Object对象</param>
        /// <param name="type">Object类型</param>
        /// <returns>处理后值</returns>
        public static Object ToDefaultableValue(this Object value, Type type)
        {
            return value.IsNullOrDBNull() ? type.DefaultValue() : value;
        }


        /// <summary>
        /// 转换为枚举类型,强转换.(null,DbNull.Value时返回默认值,转换失败抛出异常)
        /// </summary>
        /// <param name="value">值</param>
        /// <returns>枚举值</returns>
        public static T ToEnum<T>(this Object value)
        {
            return value.IsNullOrDBNull() ? default(T) : (T)Enum.Parse(typeof(T), Convert.ToString(value));
        }

        /// <summary>
        /// 转换为枚举类型,强转换.(null,DbNull.Value时返回默认值,转换失败抛出异常)
        /// </summary>
        /// <param name="value">值</param>
        /// <param name="type">具体枚举类型</param>
        /// <returns>枚举值</returns>
        public static Object ToEnum(this Object value, Type type)
        {
            return value.IsNullOrDBNull() ? type.DefaultValue() : Enum.Parse(type, Convert.ToString(value));
        }


        /// <summary>
        /// 如果value为DBNull.Value则返回null.否则返回原值
        /// </summary>
        /// <param name="value">值</param>
        /// <returns>处理后值</returns>
        public static Object ToNullableValue(this Object value)
        {
            return value.IsDBNull() ? null : value;
        }

        /// <summary>
        /// 如果value为null,则返回DBNull.Value.否则返回原值
        /// </summary>
        /// <param name="value">值</param>
        /// <returns>处理后值</returns>
        public static Object ToDBNullableValue(this Object value)
        {
            return value.IsNull() ? DBNull.Value : value;
        }


        /// <summary>
        /// 转换为可逆转换为颜色的字符串.逆转换请使用Object.ToColor()方法.(null,DbNull.Value时返回null,转换失败抛出异常)
        /// </summary>
        /// <param name="color">颜色</param>
        /// <returns>可逆转换为颜色的字符串</returns>
        public static String ToStringEx(this Color color)
        {
            return color.IsNullOrDBNull() ? null : ColorTranslator.ToHtml(color);
        }

        /// <summary>
        /// 转换为颜色,颜色转字符串请使用Color.ToStringEx()方法.(null,DbNull.Value时返回默认值,转换失败抛出异常)
        /// </summary>
        /// <param name="value">可逆转换为颜色的字符串</param>
        /// <returns>颜色</returns>
        public static Color ToColor(this Object value)
        {
            return value.IsNullOrDBNull() ? default(Color) : ColorTranslator.FromHtml(Convert.ToString(value));
        }


        /// <summary>
        /// 转换为可逆转换位字体的字符串,逆转换请使用Object.ToFont()方法.(null,DbNull.Value时返回null,转换失败抛出异常)
        /// </summary>
        /// <param name="font">字体</param>
        /// <returns>可逆转换为字体的字符串</returns>
        public static String ToStringEx(this Font font)
        {
            return font.IsNullOrDBNull() ? null : new FontConverter().ConvertToInvariantString(font);
        }

        /// <summary>
        /// 转换为字体,字体转字符串请使用Font.ToStringEx()方法.(null,DbNull.Value时返回null,转换失败抛出异常)
        /// </summary>
        /// <param name="value">可逆转换为字体的字符串</param>
        /// <returns>字体</returns>
        public static Font ToFont(this Object value)
        {
            return value.IsNullOrDBNull() ? null : (Font)new FontConverter().ConvertFromInvariantString(Convert.ToString(value));
        }


        /// <summary>
        /// 是否为布尔类型,弱转换
        /// </summary>
        /// <param name="value">值</param>
        /// <returns>是返回true,否则返回false</returns>
        public static bool IsBoolean(this Object value)
        {
            try { Boolean.Parse(value.ToString()); return true; }
            catch { return false; }
        }

        /// <summary>
        /// 转换为布尔类型,强转换.(null,DbNull.Value时返回默认值,转换失败抛出异常)
        /// </summary>
        /// <param name="value">值</param>
        /// <returns>转换后的值</returns>
        public static Boolean ToBoolean(this Object value)
        {
            return value.IsNullOrDBNull() ? default(Boolean) : Convert.ToBoolean(value);
        }

        /// <summary>
        /// 转换为布尔类型,强转换.(null,DbNull.Value时返回默认值,转换失败返回默认值)
        /// </summary>
        /// <param name="value">值</param>
        /// <returns>转换后的值</returns>
        public static Boolean ToDefaultableBoolean(this Object value)
        {
            try { return value.ToBoolean(); }
            catch { return default(Boolean); }
        }

        /// <summary>
        /// 转换为可空布尔类型,弱转换.(null,DbNull.Value时返回null,转换失败返回null)
        /// </summary>
        /// <param name="value">值</param>
        /// <returns>转换后的值</returns>
        public static Boolean? ToNullableBoolean(this Object value)
        {
            if (value.IsNullOrDBNull())
                return null;
            else
                try { return Boolean.Parse(value.ToString()); }
                catch { return null; }
        }


        /// <summary>
        /// 是否为 8 位有符号整数,弱转换
        /// </summary>
        /// <param name="value">值</param>
        /// <returns>是返回true,否则返回false</returns>
        public static bool IsSByte(this Object value)
        {
            try { SByte.Parse(value.ToString()); return true; }
            catch { return false; }
        }

        /// <summary>
        /// 转换为 8 位有符号整数,强转换.(null,DbNull.Value时返回默认值,转换失败抛出异常)
        /// </summary>
        /// <param name="value">值</param>
        /// <returns>转换后的值</returns>
        public static SByte ToSByte(this Object value)
        {
            return value.IsNullOrDBNull() ? default(SByte) : Convert.ToSByte(value);
        }

        /// <summary>
        /// 转换为 8 位有符号整数,强转换.(null,DbNull.Value时返回默认值,转换失败返回默认值)
        /// </summary>
        /// <param name="value">值</param>
        /// <returns>转换后的值</returns>
        public static SByte ToDefaultableSByte(this Object value)
        {
            try { return value.ToSByte(); }
            catch { return default(SByte); }
        }

        /// <summary>
        /// 转换为可空 8 位有符号整数,弱转换.(null,DbNull.Value时返回null,转换失败返回null)
        /// </summary>
        /// <param name="value">值</param>
        /// <returns>转换后的值</returns>
        public static SByte? ToNullableSByte(this Object value)
        {
            if (value.IsNullOrDBNull())
                return null;
            else
                try { return SByte.Parse(value.ToString()); }
                catch { return null; }
        }


        /// <summary>
        /// 是否为 16 位有符号整数,弱转换
        /// </summary>
        /// <param name="value">值</param>
        /// <returns>是返回true,否则返回false</returns>
        public static bool IsInt16(this Object value)
        {
            try { Int16.Parse(value.ToString()); return true; }
            catch { return false; }
        }

        /// <summary>
        /// 转换为 16 位有符号整数,强转换.(null,DbNull.Value时返回默认值,转换失败抛出异常)
        /// </summary>
        /// <param name="value">值</param>
        /// <returns>转换后的值</returns>
        public static Int16 ToInt16(this Object value)
        {
            return value.IsNullOrDBNull() ? default(Int16) : Convert.ToInt16(value);
        }

        /// <summary>
        /// 转换为 16 位有符号整数,强转换.(null,DbNull.Value时返回默认值,转换失败返回默认值)
        /// </summary>
        /// <param name="value">值</param>
        /// <returns>转换后的值</returns>
        public static Int16 ToDefaultableInt16(this Object value)
        {
            try { return value.ToInt16(); }
            catch { return default(Int16); }
        }

        /// <summary>
        /// 转换为可空 16 位有符号整数,弱转换.(null,DbNull.Value时返回null,转换失败返回null)
        /// </summary>
        /// <param name="value">值</param>
        /// <returns>转换后的值</returns>
        public static Int16? ToNullableInt16(this Object value)
        {
            if (value.IsNullOrDBNull())
                return null;
            else
                try { return Int16.Parse(value.ToString()); }
                catch { return null; }
        }


        /// <summary>
        /// 是否为 32 位有符号整数,弱转换
        /// </summary>
        /// <param name="value">值</param>
        /// <returns>是返回true,否则返回false</returns>
        public static bool IsInt32(this Object value)
        {
            try { Int32.Parse(value.ToString()); return true; }
            catch { return false; }
        }

        /// <summary>
        /// 转换为 32 位有符号整数,强转换.(null,DbNull.Value时返回默认值,转换失败抛出异常)
        /// </summary>
        /// <param name="value">值</param>
        /// <returns>转换后的值</returns>
        public static Int32 ToInt32(this Object value)
        {
            return value.IsNullOrDBNull() ? default(Int32) : Convert.ToInt32(value);
        }

        /// <summary>
        /// 转换为 32 位有符号整数,强转换.(null,DbNull.Value时返回默认值,转换失败返回默认值)
        /// </summary>
        /// <param name="value">值</param>
        /// <returns>转换后的值</returns>
        public static Int32 ToDefaultableInt32(this Object value)
        {
            try { return value.ToInt32(); }
            catch { return default(Int32); }
        }

        /// <summary>
        /// 转换为可空 32 位有符号整数,弱转换.(null,DbNull.Value时返回null,转换失败返回null)
        /// </summary>
        /// <param name="value">值</param>
        /// <returns>转换后的值</returns>
        public static Int32? ToNullableInt32(this Object value)
        {
            if (value.IsNullOrDBNull())
                return null;
            else
                try { return Int32.Parse(value.ToString()); }
                catch { return null; }
        }


        /// <summary>
        /// 是否为 64 位有符号整数,弱转换
        /// </summary>
        /// <param name="value">值</param>
        /// <returns>是返回true,否则返回false</returns>
        public static bool IsInt64(this Object value)
        {
            try { Int64.Parse(value.ToString()); return true; }
            catch { return false; }
        }

        /// <summary>
        /// 转换为 64 位有符号整数,强转换.(null,DbNull.Value时返回默认值,转换失败抛出异常)
        /// </summary>
        /// <param name="value">值</param>
        /// <returns>转换后的值</returns>
        public static Int64 ToInt64(this Object value)
        {
            return value.IsNullOrDBNull() ? default(Int64) : Convert.ToInt64(value);
        }

        /// <summary>
        /// 转换为 64 位有符号整数,强转换.(null,DbNull.Value时返回默认值,转换失败返回默认值)
        /// </summary>
        /// <param name="value">值</param>
        /// <returns>转换后的值</returns>
        public static Int64 ToDefaultableInt64(this Object value)
        {
            try { return value.ToInt64(); }
            catch { return default(Int64); }
        }

        /// <summary>
        /// 转换为可空 64 位有符号整数,弱转换.(null,DbNull.Value时返回null,转换失败返回null)
        /// </summary>
        /// <param name="value">值</param>
        /// <returns>转换后的值</returns>
        public static Int64? ToNullableInt64(this Object value)
        {
            if (value.IsNullOrDBNull())
                return null;
            else
                try { return Int64.Parse(value.ToString()); }
                catch { return null; }
        }


        /// <summary>
        /// 是否为 8 位无符号整数,弱转换
        /// </summary>
        /// <param name="value">值</param>
        /// <returns>是返回true,否则返回false</returns>
        public static bool IsByte(this Object value)
        {
            try { Byte.Parse(value.ToString()); return true; }
            catch { return false; }
        }

        /// <summary>
        /// 转换为 8 位无符号整数,强转换.(null,DbNull.Value时返回默认值,转换失败抛出异常)
        /// </summary>
        /// <param name="value">值</param>
        /// <returns>转换后的值</returns>
        public static Byte ToByte(this Object value)
        {
            return value.IsNullOrDBNull() ? default(Byte) : Convert.ToByte(value);
        }

        /// <summary>
        /// 转换为 8 位无符号整数,强转换.(null,DbNull.Value时返回默认值,转换失败返回默认值)
        /// </summary>
        /// <param name="value">值</param>
        /// <returns>转换后的值</returns>
        public static Byte ToDefaultableByte(this Object value)
        {
            try { return value.ToByte(); }
            catch { return default(Byte); }
        }

        /// <summary>
        /// 转换为可空 8 位无符号整数,弱转换.(null,DbNull.Value时返回null,转换失败返回null)
        /// </summary>
        /// <param name="value">值</param>
        /// <returns>转换后的值</returns>
        public static Byte? ToNullableByte(this Object value)
        {
            if (value.IsNullOrDBNull())
                return null;
            else
                try { return Byte.Parse(value.ToString()); }
                catch { return null; }
        }


        /// <summary>
        /// 是否为 16 位无符号整数,弱转换
        /// </summary>
        /// <param name="value">值</param>
        /// <returns>是返回true,否则返回false</returns>
        public static bool IsUInt16(this Object value)
        {
            try { UInt16.Parse(value.ToString()); return true; }
            catch { return false; }
        }

        /// <summary>
        /// 转换为 16 位无符号整数,强转换.(null,DbNull.Value时返回默认值,转换失败抛出异常)
        /// </summary>
        /// <param name="value">值</param>
        /// <returns>转换后的值</returns>
        public static UInt16 ToUInt16(this Object value)
        {
            return value.IsNullOrDBNull() ? default(UInt16) : Convert.ToUInt16(value);
        }

        /// <summary>
        /// 转换为 16 位无符号整数,强转换.(null,DbNull.Value时返回默认值,转换失败返回默认值)
        /// </summary>
        /// <param name="value">值</param>
        /// <returns>转换后的值</returns>
        public static UInt16 ToDefaultableUInt16(this Object value)
        {
            try { return value.ToUInt16(); }
            catch { return default(UInt16); }
        }

        /// <summary>
        /// 转换为可空 16 位无符号整数,弱转换.(null,DbNull.Value时返回null,转换失败返回null)
        /// </summary>
        /// <param name="value">值</param>
        /// <returns>转换后的值</returns>
        public static UInt16? ToNullableUInt16(this Object value)
        {
            if (value.IsNullOrDBNull())
                return null;
            else
                try { return UInt16.Parse(value.ToString()); }
                catch { return null; }
        }


        /// <summary>
        /// 是否为 32 位无符号整数,弱转换
        /// </summary>
        /// <param name="value">值</param>
        /// <returns>是返回true,否则返回false</returns>
        public static bool IsUInt32(this Object value)
        {
            try { UInt32.Parse(value.ToString()); return true; }
            catch { return false; }
        }

        /// <summary>
        /// 转换为 32 位无符号整数,强转换.(null,DbNull.Value时返回默认值,转换失败抛出异常)
        /// </summary>
        /// <param name="value">值</param>
        /// <returns>转换后的值</returns>
        public static UInt32 ToUInt32(this Object value)
        {
            return value.IsNullOrDBNull() ? default(UInt32) : Convert.ToUInt32(value);
        }

        /// <summary>
        /// 转换为 32 位无符号整数,强转换.(null,DbNull.Value时返回默认值,转换失败返回默认值)
        /// </summary>
        /// <param name="value">值</param>
        /// <returns>转换后的值</returns>
        public static UInt32 ToDefaultableUInt32(this Object value)
        {
            try { return value.ToUInt32(); }
            catch { return default(UInt32); }
        }

        /// <summary>
        /// 转换为可空 32 位无符号整数,弱转换.(null,DbNull.Value时返回null,转换失败返回null)
        /// </summary>
        /// <param name="value">值</param>
        /// <returns>转换后的值</returns>
        public static UInt32? ToNullableUInt32(this Object value)
        {
            if (value.IsNullOrDBNull())
                return null;
            else
                try { return UInt32.Parse(value.ToString()); }
                catch { return null; }
        }


        /// <summary>
        /// 是否为 64 位无符号整数,弱转换
        /// </summary>
        /// <param name="value">值</param>
        /// <returns>是返回true,否则返回false</returns>
        public static bool IsUInt64(this Object value)
        {
            try { UInt64.Parse(value.ToString()); return true; }
            catch { return false; }
        }

        /// <summary>
        /// 转换为 64 位无符号整数,强转换.(null,DbNull.Value时返回默认值,转换失败抛出异常)
        /// </summary>
        /// <param name="value">值</param>
        /// <returns>转换后的值</returns>
        public static UInt64 ToUInt64(this Object value)
        {
            return value.IsNullOrDBNull() ? default(UInt64) : Convert.ToUInt64(value);
        }

        /// <summary>
        /// 转换为 64 位无符号整数,强转换.(null,DbNull.Value时返回默认值,转换失败返回默认值)
        /// </summary>
        /// <param name="value">值</param>
        /// <returns>转换后的值</returns>
        public static UInt64 ToDefaultableUInt64(this Object value)
        {
            try { return value.ToUInt64(); }
            catch { return default(UInt64); }
        }

        /// <summary>
        /// 转换为可空 64 位无符号整数,弱转换.(null,DbNull.Value时返回null,转换失败返回null)
        /// </summary>
        /// <param name="value">值</param>
        /// <returns>转换后的值</returns>
        public static UInt64? ToNullableUInt64(this Object value)
        {
            if (value.IsNullOrDBNull())
                return null;
            else
                try { return UInt64.Parse(value.ToString()); }
                catch { return null; }
        }


        /// <summary>
        /// 是否为十进制数,弱转换
        /// </summary>
        /// <param name="value">值</param>
        /// <returns>是返回true,否则返回false</returns>
        public static bool IsDecimal(this Object value)
        {
            try { Decimal.Parse(value.ToString()); return true; }
            catch { return false; }
        }

        /// <summary>
        /// 转换为十进制数,强转换.(null,DbNull.Value时返回默认值,转换失败抛出异常)
        /// </summary>
        /// <param name="value">值</param>
        /// <returns>转换后的值</returns>
        public static Decimal ToDecimal(this Object value)
        {
            return value.IsNullOrDBNull() ? default(Decimal) : Convert.ToDecimal(value);
        }

        /// <summary>
        /// 转换为十进制数,强转换.(null,DbNull.Value时返回默认值,转换失败返回默认值)
        /// </summary>
        /// <param name="value">值</param>
        /// <returns>转换后的值</returns>
        public static Decimal ToDefaultableDecimal(this Object value)
        {
            try { return value.ToDecimal(); }
            catch { return default(Decimal); }
        }

        /// <summary>
        /// 转换为可空十进制数,弱转换.(null,DbNull.Value时返回null,转换失败返回null)
        /// </summary>
        /// <param name="value">值</param>
        /// <returns>转换后的值</returns>
        public static Decimal? ToNullableDecimal(this Object value)
        {
            if (value.IsNullOrDBNull())
                return null;
            else
                try { return Decimal.Parse(value.ToString()); }
                catch { return null; }
        }


        /// <summary>
        /// 是否为单精度浮点数,弱转换
        /// </summary>
        /// <param name="value">值</param>
        /// <returns>是返回true,否则返回false</returns>
        public static bool IsSingle(this Object value)
        {
            try { Single.Parse(value.ToString()); return true; }
            catch { return false; }
        }

        /// <summary>
        /// 转换为单精度浮点数,强转换.(null,DbNull.Value时返回默认值,转换失败抛出异常)
        /// </summary>
        /// <param name="value">值</param>
        /// <returns>转换后的值</returns>
        public static Single ToSingle(this Object value)
        {
            return value.IsNullOrDBNull() ? default(Single) : Convert.ToSingle(value);
        }

        /// <summary>
        /// 转换为单精度浮点数,强转换.(null,DbNull.Value时返回默认值,转换失败返回默认值)
        /// </summary>
        /// <param name="value">值</param>
        /// <returns>转换后的值</returns>
        public static Single ToDefaultableSingle(this Object value)
        {
            try { return value.ToSingle(); }
            catch { return default(Single); }
        }

        /// <summary>
        /// 转换为可空单精度浮点数,弱转换.(null,DbNull.Value时返回null,转换失败返回null)
        /// </summary>
        /// <param name="value">值</param>
        /// <returns>转换后的值</returns>
        public static Single? ToNullableSingle(this Object value)
        {
            if (value.IsNullOrDBNull())
                return null;
            else
                try { return Single.Parse(value.ToString()); }
                catch { return null; }
        }


        /// <summary>
        /// 是否为双精度浮点数,弱转换
        /// </summary>
        /// <param name="value">值</param>
        /// <returns>是返回true,否则返回false</returns>
        public static bool IsDouble(this Object value)
        {
            try { Double.Parse(value.ToString()); return true; }
            catch { return false; }
        }

        /// <summary>
        /// 转换为双精度浮点数,强转换.(null,DbNull.Value时返回默认值,转换失败抛出异常)
        /// </summary>
        /// <param name="value">值</param>
        /// <returns>转换后的值</returns>
        public static Double ToDouble(this Object value)
        {
            Double val = 0;
            try
            {
                val = value.IsNullOrDBNull() ? default(Double) : Convert.ToDouble(value);
            }
            catch (Exception){ }
            return val;
        }

        /// <summary>
        /// 转换为双精度浮点数,强转换.(null,DbNull.Value时返回默认值,转换失败返回默认值)
        /// </summary>
        /// <param name="value">值</param>
        /// <returns>转换后的值</returns>
        public static Double ToDefaultableDouble(this Object value)
        {
            try { return value.ToDouble(); }
            catch { return default(Double); }
        }

        /// <summary>
        /// 转换为可空双精度浮点数,弱转换.(null,DbNull.Value时返回null,转换失败返回null)
        /// </summary>
        /// <param name="value">值</param>
        /// <returns>转换后的值</returns>
        public static Double? ToNullableDouble(this Object value)
        {
            if (value.IsNullOrDBNull())
                return null;
            else
                try { return Double.Parse(value.ToString()); }
                catch { return null; }
        }


        /// <summary>
        /// 是否为日期时间类型,弱转换
        /// </summary>
        /// <param name="value">值</param>
        /// <returns>是返回true,否则返回false</returns>
        public static bool IsDateTime(this Object value)
        {
            try { DateTime.Parse(value.ToString()); return true; }
            catch { return false; }
        }

        /// <summary>
        /// 转换为日期时间类型,强转换.(null,DbNull.Value时返回默认值,转换失败抛出异常)
        /// </summary>
        /// <param name="value">值</param>
        /// <returns>转换后的值</returns>
        public static DateTime ToDateTime(this Object value)
        {
            return value.IsNullOrDBNull() ? default(DateTime) : Convert.ToDateTime(value);
        }

        /// <summary>
        /// 转换为日期时间类型,强转换.(null,DbNull.Value时返回默认值,转换失败返回默认值)
        /// </summary>
        /// <param name="value">值</param>
        /// <returns>转换后的值</returns>
        public static DateTime ToDefaultableDateTime(this Object value)
        {
            try { return value.ToDateTime(); }
            catch { return default(DateTime); }
        }

        /// <summary>
        /// 转换为可空日期时间类型,弱转换.(null,DbNull.Value时返回null,转换失败返回null)
        /// </summary>
        /// <param name="value">值</param>
        /// <returns>转换后的值</returns>
        public static DateTime? ToNullableDateTime(this Object value)
        {
            if (value.IsNullOrDBNull())
                return null;
            else
                try { return DateTime.Parse(value.ToString()); }
                catch { return null; }
        }


        /// <summary>
        /// 是否为数据库日期时间类型,弱转换
        /// </summary>
        /// <param name="value">值</param>
        /// <returns>是返回true,否则返回false</returns>
        public static bool IsDbDateTime(this Object value)
        {
            try { DateTime dateTime = DateTime.Parse(value.ToString()); return dateTime >= DefaultDbDateTime; }
            catch { return false; }
        }

        /// <summary>
        /// 转换为数据库日期时间类型,强转换.(null,DbNull.Value时返回默认值,转换失败抛出异常)
        /// </summary>
        /// <param name="value">值</param>
        /// <returns>转换后的值</returns>
        public static DateTime ToDbDateTime(this Object value)
        {
            if (value.IsNullOrDBNull())
                return DefaultDbDateTime;
            else
            {
                DateTime dateTime = Convert.ToDateTime(value);
                return dateTime >= DefaultDbDateTime ? dateTime : DefaultDbDateTime;
            }
        }

        /// <summary>
        /// 转换为数据库日期时间类型,强转换.(null,DbNull.Value时返回默认值,转换失败返回默认值)
        /// </summary>
        /// <param name="value">值</param>
        /// <returns>转换后的值</returns>
        public static DateTime ToDefaultableDbDateTime(this Object value)
        {
            try { return value.ToDbDateTime(); }
            catch { return DefaultDbDateTime; }
        }

        /// <summary>
        /// 转换为可空数据库日期时间类型,弱转换.(null,DbNull.Value时返回null,转换失败返回null)
        /// </summary>
        /// <param name="value">值</param>
        /// <returns>转换后的值</returns>
        public static DateTime? ToNullableDbDateTime(this Object value)
        {
            if (value.IsNullOrDBNull())
                return null;
            else
                try { DateTime dateTime = DateTime.Parse(value.ToString()); return dateTime >= DefaultDbDateTime ? dateTime : DefaultDbDateTime; }
                catch { return null; }
        }


        /// <summary>
        /// 是否为一个 Unicode 字符,弱转换
        /// </summary>
        /// <param name="value">值</param>
        /// <returns>是返回true,否则返回false</returns>
        public static bool IsChar(this Object value)
        {
            try { Char.Parse(value.ToString()); return true; }
            catch { return false; }
        }

        /// <summary>
        /// 转换为一个 Unicode 字符,强转换.(null,DbNull.Value时返回默认值,转换失败抛出异常)
        /// </summary>
        /// <param name="value">值</param>
        /// <returns>转换后的值</returns>
        public static Char ToChar(this Object value)
        {
            return value.IsNullOrDBNull() ? default(Char) : Convert.ToChar(value);
        }

        /// <summary>
        /// 转换为一个 Unicode 字符,强转换.(null,DbNull.Value时返回默认值,转换失败返回默认值)
        /// </summary>
        /// <param name="value">值</param>
        /// <returns>转换后的值</returns>
        public static Char ToDefaultableChar(this Object value)
        {
            try { return value.ToChar(); }
            catch { return default(Char); }
        }

        /// <summary>
        /// 转换为可空一个 Unicode 字符,弱转换.(null,DbNull.Value时返回null,转换失败返回null)
        /// </summary>
        /// <param name="value">值</param>
        /// <returns>转换后的值</returns>
        public static Char? ToNullableChar(this Object value)
        {
            if (value.IsNullOrDBNull())
                return null;
            else
                try { return Char.Parse(value.ToString()); }
                catch { return null; }
        }


        /// <summary>
        /// 转换为一系列 Unicode 字符,强转换.(null,DbNull.Value时返回String.Empty,转换失败抛出异常)
        /// </summary>
        /// <param name="value">值</param>
        /// <returns>转换后的值</returns>
        public static String ToStringEx(this Object value)
        {
            return value.IsNullOrDBNull() ? String.Empty : Convert.ToString(value);
        }

        /// <summary>
        /// 转换为一系列 Unicode 字符,强转换.(null,DbNull.Value时返回String.Empty,转换失败抛出异常)
        /// </summary>
        /// <param name="value">值</param>
        /// <param name="format">格式</param>
        /// <returns>转换后的值</returns>
        public static String ToStringEx(this DateTime? value, string format)
        {
            return value.IsNullOrDBNull() ? String.Empty : value.Value.ToString(format);
        }

        /// <summary>
        /// 转换为一个 Unicode 字符,强转换.(null,DbNull.Value时返回String.Empty,转换失败返回String.Empty)
        /// </summary>
        /// <param name="value">值</param>
        /// <returns>转换后的值</returns>
        public static String ToDefaultableString(this Object value)
        {
            try { return value.ToStringEx(); }
            catch { return String.Empty; }
        }

        /// <summary>
        /// 转换为一系列 Unicode 字符,弱转换.(null,DbNull.Value时返回null,转换失败返回null)
        /// </summary>
        /// <param name="value">值</param>
        /// <returns>转换后的值</returns>
        public static String ToNullableString(this Object value)
        {
            if (value.IsNullOrDBNull())
                return null;
            else
                try { return value.ToString(); }
                catch { return null; }
        }

    }
}
