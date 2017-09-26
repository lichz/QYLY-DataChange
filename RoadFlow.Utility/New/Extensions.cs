//========================================
// Copyright © 2017
// 
// CLR版本 	: 4.0.30319.42000
// 计算机  	: USER-20170420WC
// 文件名  	: Extensions.cs
// 创建人  	: kaifa5
// 创建时间	: 2017/9/25 13:39:58
// 文件版本	: 1.0.0
// 文件描述	: 
//========================================

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace RoadFlow.Utility.New {
    /// <summary>
    /// 辅助拓展方法静态封装类。
    /// </summary>
    public static class Extensions {
        /// <summary>
        /// 将object对象转换为指定类型的对象。
        /// </summary>
        /// <typeparam name="T">转换的目标数据类型。</typeparam>
        /// <param name="object">需要转换的对象。</param>
        /// <returns>若转换成功则为转换后的对象，否则为default(T)。</returns>
        public static T Convert<T>(this object @object) {
            return @object.Convert<T>(default(T));
        }

        /// <summary>
        /// 将object对象转换为指定类型的对象。
        /// </summary>
        /// <param name="object">需要转换的对象。</param>
        /// <param name="type">指定转换的目标类型。</param>
        /// <returns>若转换成功为响应类型的对象，否则为null。</returns>
        /// <exception cref="ArgumentNullException">参数type为null时将抛出该异常。</exception>
        public static object Convert(this object @object, Type type) {
            if (type == null) {
                throw new ArgumentNullException("type");
            }
            if (@object == null) {
                return null;
            }
            if (@object is DBNull) {
                return null;
            }
            if (type.IsAssignableFrom(@object.GetType())) {
                return @object;
            }
            object result;
            try {
                if (type.IsGenericType && type.GetGenericTypeDefinition().Equals(typeof(Nullable<>))) {
                    result = System.Convert.ChangeType(@object, Nullable.GetUnderlyingType(type));
                } else if (type.Equals(typeof(Guid))) {
                    result = Guid.Parse(@object.ToString());
                } else {
                    result = System.Convert.ChangeType(@object, type);
                }
            } catch (Exception) {
                return null;
            }
            return result;
        }

        /// <summary>
        /// 将object对象转换为指定类型的对象，并指定转换失败时的默认值。
        /// </summary>
        /// <typeparam name="T">转换的目标数据类型。</typeparam>
        /// <param name="object">需要转换的对象。</param>
        /// <param name="default">转换失败时的默认值。</param>
        /// <returns>若转换成功则为转换后的对象，否则为预先提供的默认值。</returns>
        public static T Convert<T>(this object @object, T @default) {
            object result = @object.Convert(typeof(T));
            return result != null ? (T)result : @default;
        }

        /// <summary>
        /// 将中文字符串转换为每个汉字拼音的首字母形式。
        /// </summary>
        /// <param name="string">需要转换的中文字符串。</param>
        /// <returns>中文字符串对应的汉字拼音首字母。</returns>
        public static string ToChineseSpell(this string @string) {
            if (string.IsNullOrEmpty(@string)) {
                return string.Empty;
            }
            StringBuilder builder = new StringBuilder();
            int[] areacode = {
                    45217, 45253, 45761, 46318,46826, 47010, 47297, 47614,
                    48119, 48119, 49062, 49324,49896, 50371, 50614, 50622,
                    50906, 51387, 51446, 52218,52698, 52698, 52698, 52980,
                    53689, 54481
                };
            foreach (char @char in @string) {
                byte[] buffer = Encoding.Default.GetBytes(@char.ToString());
                if (buffer.Length > 1) {
                    int area = System.Convert.ToInt32(buffer[0]);
                    int pos = System.Convert.ToInt32(buffer[1]);
                    int code = (area << 8) + pos;
                    for (int i = 0; i < 26; i++) {
                        int max = 55290;
                        if (i != 25) {
                            max = areacode[i + 1];
                        }
                        if (areacode[i] <= code && code < max) {
                            return Encoding.Default.GetString(new byte[] { (byte)(65 + i) });
                        }
                    }
                    return "x";
                } else {
                    builder.Append(@char);
                }
            }
            return builder.ToString().ToLower();
        }

        #region ====与原代码保持一致性保留的代码====
        [Obsolete("新代码请不要使用该方法!")]
        public static bool Contains(this string @string, string subString, StringComparison comparison) {
            if (string.IsNullOrEmpty(@string)) {
                return false;
            }
            if (string.IsNullOrEmpty(subString)) {
                return true;
            }
            return @string.IndexOf(subString, comparison) >= 0;
        }

        [Obsolete("新代码请不要使用该方法!")]
        public static string HtmlEncode(this string @string) {
            if (string.IsNullOrEmpty(@string)) {
                return string.Empty;
            }
            return System.Web.HttpUtility.HtmlEncode(@string);
        }

        [Obsolete("新代码请不要使用该方法!")]
        public static bool In(this int @int, params int[] param) {
            for (int i = 0; i < param.Length; i++) {
                if (param[i].Equals(@int)) {
                    return true;
                }
            }
            return false;
        }

        [Obsolete("新代码请不要使用该方法!")]
        public static bool IsDateTime(this object @object) {
            if (@object == null) {
                return false;
            }
            if (typeof(DateTime).Equals(@object.GetType())) {
                return true;
            }
            string @string = @object as string;
            if (string.IsNullOrEmpty(@string)) {
                return false;
            }
            DateTime dateTime;
            return DateTime.TryParse(@string, out dateTime);
        }

        [Obsolete("新代码请不要使用该方法!")]
        public static bool IsDecimal(this object @object) {
            if (@object == null) {
                return false;
            }
            if (typeof(decimal).Equals(@object.GetType())) {
                return true;
            }
            string @string = @object as string;
            if (string.IsNullOrEmpty(@string)) {
                return false;
            }
            return Regex.IsMatch(@string, "^(?:0|\\-?(?:0\\.\\d*[1-9]|[1-9]\\d*(?:\\.\\d*[1-9])?))$");
        }

        [Obsolete("新代码请不要使用该方法!")]
        public static bool IsDouble(this object @object) {
            if (@object == null) {
                return false;
            }
            if (typeof(double).Equals(@object.GetType())) {
                return true;
            }
            string @string = @object as string;
            if (string.IsNullOrEmpty(@string)) {
                return false;
            }
            return Regex.IsMatch(@string, "^(?:0|\\-?(?:0\\.\\d*[1-9]|[1-9]\\d*(?:\\.\\d*[1-9])?))$");
        }

        [Obsolete("新代码请不要使用该方法!")]
        public static bool IsEmptyGuid(this Guid guid) {
            return Guid.Empty.Equals(guid);
        }

        [Obsolete("新代码请不要使用该方法!")]
        public static bool IsGuid(this object @object) {
            if (@object == null) {
                return false;
            }
            if (typeof(Guid).Equals(@object.GetType())) {
                return true;
            }
            string @string = @object as string;
            if (string.IsNullOrEmpty(@string)) {
                return false;
            }
            return Regex.IsMatch(@string, "^[\\(\\{]?[\\da-f]{8}\\-?(?:[\\da-f]{4}\\-?){3}[\\da-f]{12}[\\}\\)]?$", RegexOptions.IgnoreCase);
        }

        [Obsolete("新代码请不要使用该方法!")]
        public static bool IsGuid(this object @object, out Guid guid) {
            if (@object == null) {
                guid = Guid.Empty;
                return false;
            }
            if (typeof(Guid).Equals(@object.GetType())) {
                guid = (Guid)@object;
                return true;
            }
            string @string = @object as string;
            return Guid.TryParse(@string, out guid);
        }

        [Obsolete("新代码请不要使用该方法!")]
        public static bool IsInt(this object @object) {
            if (@object == null) {
                return false;
            }
            if (typeof(int).Equals(@object.GetType())) {
                return true;
            }
            string @string = @object as string;
            if (string.IsNullOrEmpty(@string)) {
                return false;
            }
            return Regex.IsMatch(@string, "^\\-?[1-9]\\d*$");
        }

        [Obsolete("新代码请不要使用该方法!")]
        public static bool IsInt(this object @object, out int @int) {
            if (@object == null) {
                @int = 0;
                return false;
            }
            if (typeof(int).Equals(@object.GetType())) {
                @int = (int)@object;
                return true;
            }
            string @string = @object as string;
            return int.TryParse(@string, out @int);
        }

        [Obsolete("新代码请不要使用该方法!")]
        public static bool IsLong(this object @object, out long @long) {
            if (@object == null) {
                @long = 0L;
                return false;
            }
            if (typeof(long).Equals(@object.GetType())) {
                @long = (long)@object;
                return true;
            }
            string @string = @object as string;
            return long.TryParse(@string, out @long);
        }

        [Obsolete("新代码请不要使用该方法!")]
        public static bool IsNullOrEmpty(this string @string) {
            return string.IsNullOrEmpty(@string);
        }

        [Obsolete("新代码请不要使用该方法!")]
        public static string ReplaceSelectSql(this string @string) {
            if (string.IsNullOrEmpty(@string)) {
                return string.Empty;
            }
            return @string.Replace("DELETE", "").Replace("INSERT", "").Replace("UPDATE", "");
        }

        [Obsolete("新代码请不要使用该方法!")]
        public static string ReplaceSql(this string @string) {
            if (string.IsNullOrEmpty(@string)) {
                return string.Empty;
            }
            return @string.Replace("'", "").Replace("--", " ").Replace(";", "");
        }

        [Obsolete("新代码请不要使用该方法!")]
        public static string Serialize(this object @object) {
            if (@object == null) {
                return string.Empty;
            }
            Type type = @object.GetType();
            if (type.IsSerializable) {
                try {
                    StringBuilder builder = new StringBuilder();
                    System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(type);
                    System.Xml.XmlWriterSettings settings = new System.Xml.XmlWriterSettings();
                    settings.CloseOutput = true;
                    settings.Encoding = Encoding.UTF8;
                    settings.Indent = true;
                    settings.CheckCharacters = false;
                    System.Xml.XmlWriter writer = System.Xml.XmlWriter.Create(builder, settings);
                    serializer.Serialize(writer, @object);
                    writer.Flush();
                    writer.Close();
                    return builder.ToString();
                } catch (Exception) {
                    return string.Empty;
                }
            }
            return string.Empty;
        }

        [Obsolete("新代码请不要使用该方法!")]
        public static decimal ToDecimal(this object @object) {
            if (@object.IsDecimal()) {
                return decimal.Parse(@object.ToString());
            }
            return 0M;
        }

        [Obsolete("新代码请不要使用该方法!")]
        public static DateTime ToDateTime(this object @object) {
            if (@object.IsDateTime()) {
                return DateTime.Parse(@object.ToString());
            }
            return default(DateTime);
        }

        [Obsolete("新代码请不要使用该方法!")]
        public static string ToDateWeekString(this DateTime dateTime) {
            string[] week = new string[] {
            "日","一","二","三","四","五","六"
        };
            int index = System.Convert.ToInt32(dateTime.DayOfWeek);
            return string.Format("{0:yyyy年MM月dd日} 星期{1}", dateTime, week[index]);
        }

        [Obsolete("新代码请不要使用该方法!")]
        public static double ToDouble(this object @object) {
            if (@object.IsDouble()) {
                return double.Parse(@object.ToString());
            }
            return 0D;
        }

        [Obsolete("新代码请不要使用该方法!")]
        public static Guid ToGuid(this object @object) {
            if (@object.IsGuid()) {
                return Guid.Parse(@object.ToString());
            }
            return Guid.Empty;
        }

        [Obsolete("新代码请不要使用该方法!")]
        public static int ToInt(this object @object) {
            if (@object.IsInt()) {
                return int.Parse(@object.ToString());
            }
            return 0;
        }

        [Obsolete("新代码请不要使用该方法!")]
        public static int ToInt(this object @object, int @default) {
            if (@object.IsInt()) {
                return int.Parse(@object.ToString());
            }
            return @default;
        }

        [Obsolete("新代码请不要使用该方法!")]
        public static Nullable<int> ToIntOrNull(this object @object) {
            int result;
            if (@object.IsInt(out result)) {
                return result;
            }
            return null;
        }

        [Obsolete("kaifa5--为了编译通过暂时将方法放这里!")]
        public static List<T> ToList<T>(this System.Data.DataTable dt) {
            List<T> list = new List<T>();
            Type t = typeof(T);
            if (dt.Rows.Count > 0) {
                //循环充填模型对象。
                foreach (System.Data.DataRow dr in dt.Rows) {
                    T instance = (T)Activator.CreateInstance(t);
                    foreach (System.Reflection.PropertyInfo pi in t.GetProperties()) {
                        if (dt.Columns.Contains(pi.Name) && pi.CanWrite) {
                            if (dr[pi.Name] is DBNull) {
                                pi.SetValue(instance, null, null);
                            } else {
                                if (pi.PropertyType.IsGenericType && pi.PropertyType.GetGenericTypeDefinition().Equals(typeof(Nullable<>)) && Nullable.GetUnderlyingType(pi.PropertyType).IsEnum) {//nullable枚举
                                    pi.SetValue(instance, Enum.Parse(Nullable.GetUnderlyingType(pi.PropertyType), dr[pi.Name].ToString()), null);
                                } else {
                                    pi.SetValue(instance, dr[pi.Name], null);
                                }
                            }
                        }
                    }
                    list.Add(instance);
                }
            }
            return list;
        }

        [Obsolete("新代码请不要使用该方法!")]
        public static string UrlEncode(this string @string) {
            if (string.IsNullOrEmpty(@string)) {
                return string.Empty;
            }
            return System.Web.HttpUtility.UrlEncode(@string);
        }
        #endregion
    }
}