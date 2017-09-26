// // 
// // ==============================================================================
// // 
// // Version: 1.0
// // Compiler: Visual Studio 2013
// // Created: 2015-10-15 10:15
// // Updated: 2015-10-15 10:15
// //  
// // Author: work
// // Company: World
// // 
// // Project: Sevn.Tools
// // Filename: DataSetExpand.cs
// // Description: 
// // 
// // ==============================================================================

using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace RoadFlow.Utility
{
    public static class DataSetExpand
    {

        #region wubo.tools

        public static T ReaderToModel<T>(this IDataReader dr)
        {
            try
            {
                using (dr)
                {
                    if (dr.Read())
                    {
                        List<string> list = new List<string>(dr.FieldCount);
                        for (int i = 0; i < dr.FieldCount; i++)
                        {
                            list.Add(dr.GetName(i).ToLower());
                        }
                        T model = Activator.CreateInstance<T>();
                        foreach (PropertyInfo pi in model.GetType().GetProperties(BindingFlags.GetProperty | BindingFlags.Public | BindingFlags.Instance))
                        {
                            if (list.Contains(pi.Name.ToLower()))
                            {
                                if (!IsNullOrDbNull(dr[pi.Name]))
                                {
                                    pi.SetValue(model, HackType(dr[pi.Name], pi.PropertyType), null);
                                }
                            }
                        }
                        return model;
                    }
                }
                return default(T);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<T> ReaderToList<T>(this IDataReader dr) where T : new()
        {
            using (dr)
            {
                List<string> field = new List<string>(dr.FieldCount);
                for (int i = 0; i < dr.FieldCount; i++)
                {
                    field.Add(dr.GetName(i).ToLower());
                }
                List<T> list = new List<T>();
                while (dr.Read())
                {
                    T model = Activator.CreateInstance<T>();
                    foreach (PropertyInfo property in model.GetType().GetProperties(BindingFlags.GetProperty | BindingFlags.Public | BindingFlags.Instance))
                    {
                        if (field.Contains(property.Name.ToLower()))
                        {
                            if (!IsNullOrDbNull(dr[property.Name]))
                            {
                                property.SetValue(model, HackType(dr[property.Name], property.PropertyType), null);
                            }
                        }
                    }
                    list.Add(model);
                }
                return list;
            }
        }
        //这个类对可空类型进行判断转换，要不然会报错
        private static object HackType(object value, Type conversionType)
        {
            if (conversionType.IsGenericType && conversionType.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                if (value == null)
                    return null;

                System.ComponentModel.NullableConverter nullableConverter = new System.ComponentModel.NullableConverter(conversionType);
                conversionType = nullableConverter.UnderlyingType;
            }
            return Convert.ChangeType(value, conversionType);
        }

        private static bool IsNullOrDbNull(object obj)
        {
            return ((obj is DBNull) || string.IsNullOrEmpty(obj.ToString())) ? true : false;
        }

        public static T GetAttribute<T>(this MemberInfo member, bool isRequired)
        where T : Attribute
        {
            var attribute = member.GetCustomAttributes(typeof(T), false).SingleOrDefault();

            if (attribute == null && isRequired)
            {
                throw new ArgumentException(
                    string.Format(
                        CultureInfo.InvariantCulture,
                        "The {0} attribute must be defined on member {1}",
                        typeof(T).Name,
                        member.Name));
            }

            return (T)attribute;
        }

        ///// <summary>
        ///// IDataReader转List
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <param name="rdr"></param>
        ///// <returns></returns>
        //public static List<T> ReaderToList<T>(this IDataReader rdr)
        //{
        //    List<T> list = new List<T>();

        //    while (rdr.Read())
        //    {
        //        T t = System.Activator.CreateInstance<T>();
        //        Type obj = t.GetType();
        //        // 循环字段  
        //        for (int i = 0; i < rdr.FieldCount; i++)
        //        {
        //            object tempValue = null;

        //            if (rdr.IsDBNull(i))
        //            {

        //                string typeFullName = obj.GetProperty(rdr.GetName(i)).PropertyType.FullName;
        //                //tempValue = GetDBNullValue(typeFullName);

        //            }
        //            else
        //            {
        //                tempValue = rdr.GetValue(i);

        //            }

        //            obj.GetProperty(rdr.GetName(i)).SetValue(t, tempValue, null);

        //        }

        //        list.Add(t);

        //    }
        //    return list;
        //}

        ///// <summary>  
        ///// 返回值为DBnull的默认值  
        ///// </summary>  
        ///// <param name="typeFullName">数据类型的全称，类如：system.int32</param>  
        ///// <returns>返回的默认值</returns>  
        //private static object GetDBNullValue(string typeFullName)
        //{

        //    typeFullName = typeFullName.ToLower();

        //    if (typeFullName == DataType.String)
        //    {
        //        return String.Empty;
        //    }
        //    if (typeFullName == DataType.Int32)
        //    {
        //        return 0;
        //    }
        //    if (typeFullName == DataType.DateTime)
        //    {
        //        return Convert.Convert<DateTime>(BaseSet.DateTimeLongNull);
        //    }
        //    if (typeFullName == DataType.Boolean)
        //    {
        //        return false;
        //    }
        //    if (typeFullName == DataType.Int)
        //    {
        //        return 0;
        //    }

        //    return null;
        //} 

        /// <summary>
        /// DataTable转List
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static List<T> DataTableToList<T>(this DataTable dt) where T : new()
        {

            List<T> ts = new List<T>();// 定义集合
            Type type = typeof(T); // 获得此模型的类型
            string tempName = "";
            foreach (DataRow dr in dt.Rows)
            {
                T t = new T();
                PropertyInfo[] propertys = t.GetType().GetProperties();// 获得此模型的公共属性
                foreach (PropertyInfo pi in propertys)
                {
                    tempName = pi.Name;

                    if (dt.Columns.Contains(pi.Name) && pi.CanWrite) {
                        if (dr[pi.Name] is DBNull) {
                            pi.SetValue(t, null, null);
                        } else {
                            if (pi.PropertyType.IsGenericType && pi.PropertyType.GetGenericTypeDefinition().Equals(typeof(Nullable<>)) && Nullable.GetUnderlyingType(pi.PropertyType).IsEnum) {//nullable枚举
                                pi.SetValue(t, Enum.Parse(Nullable.GetUnderlyingType(pi.PropertyType), dr[pi.Name].ToString()), null);
                            } else {
                                pi.SetValue(t, dr[pi.Name], null);
                            }
                        }
                    }
                }
                ts.Add(t);
            }
            return ts;
        }

        #endregion
    }
}