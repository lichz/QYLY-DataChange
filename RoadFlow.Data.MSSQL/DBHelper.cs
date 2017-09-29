using System;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Reflection;
using RoadFlow.Utility;

namespace RoadFlow.Data.MSSQL
{
    /// <summary>
    /// SQLSERVER助手类
    /// </summary>
    public class DBHelper
    {
        private string connectionString;

        public DBHelper()
        {
            this.connectionString = RoadFlow.Utility.Config.PlatformConnectionStringMSSQL;
        }
        public DBHelper(string connString)
        {
            this.connectionString = connString;
        }

        /// <summary>
        /// 连接字符串
        /// </summary>
        public string ConnectionString
        {
            get { return this.connectionString; }
        }

        /// <summary>
        /// 释放连接
        /// </summary>
        public void Dispose()
        {

        }

        /// <summary>
        /// 得到一个SqlDataReader
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <returns></returns>
        public SqlDataReader GetDataReader(string sql)
        {
            SqlConnection conn = new SqlConnection(ConnectionString);
            conn.Open();
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Prepare();
                return cmd.ExecuteReader(CommandBehavior.CloseConnection);
            }
        }

        /// <summary>
        /// 得到一个SqlDataReader
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <returns></returns>
        public SqlDataReader GetDataReader(string sql, SqlParameter[] parameter)
        {
            try
            {
                SqlConnection conn = new SqlConnection(ConnectionString);
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    if (parameter != null && parameter.Length > 0)
                        cmd.Parameters.AddRange(parameter);
                    SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                    cmd.Parameters.Clear();
                    cmd.Prepare();
                    return dr;

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IDataAdapter GetDataAdapter(string sql, IDataParameter[] parameter) {
            IDbDataAdapter dataAdapter = null;
            SqlConnection conn = new SqlConnection(ConnectionString);
            conn.Open();
            using (SqlCommand cmd = new SqlCommand(sql, (SqlConnection)conn)) {
                if (parameter != null && parameter.Length > 0) {
                    cmd.Parameters.AddRange(parameter);
                }
                dataAdapter = new SqlDataAdapter(cmd);
            }
            return dataAdapter;
        }


        /// <summary>
        /// 得到一个DataTable 
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <returns></returns>
        public DataTable GetDataTable(string sql)
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    SqlDataReader dr = cmd.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Load(dr);
                    dr.Close();
                    dr.Dispose();
                    cmd.Prepare();
                    return dt;
                }
            }
        }


        /// <summary>
        /// 得到一个DataTable
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <returns></returns>
        public DataTable GetDataTable(string sql, IDataParameter[] parameter)
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    if (parameter != null && parameter.Length > 0)
                        cmd.Parameters.AddRange(parameter);
                    SqlDataReader dr = cmd.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Load(dr);
                    dr.Close();
                    dr.Dispose();
                    cmd.Parameters.Clear();
                    cmd.Prepare();
                    return dt;
                }
            }
        }

        /// <summary>
        /// 得到数据集
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public DataSet GetDataSet(string sql)
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                using (SqlDataAdapter dap = new SqlDataAdapter(sql, conn))
                {
                    DataSet ds = new DataSet();
                    dap.Fill(ds);
                    return ds;
                }
            }
        }

        /// <summary>
        /// 执行SQL
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public int Execute(string sql)
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Prepare();
                    return cmd.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// 执行SQL(事务)
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public int Execute(List<string> sqlList)
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    int i = 0;
                    cmd.Connection = conn;
                    foreach (string sql in sqlList)
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = sql;
                        cmd.Prepare();
                        i += cmd.ExecuteNonQuery();
                    }
                    return i;
                }
            }
        }

        /// <summary>
        /// 执行带参数的SQL
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public int Execute(string sql, IDataParameter[] parameter)
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    if (parameter != null && parameter.Length > 0)
                        cmd.Parameters.AddRange(parameter);
                    int i = cmd.ExecuteNonQuery();
                    cmd.Parameters.Clear();
                    cmd.Prepare();
                    return i;
                }
            }
        }

        /// <summary>
        /// 执行SQL(事务)
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public int Execute(List<string> sqlList, List<SqlParameter[]> parameterList)
        {
            if (sqlList.Count > parameterList.Count)
            {
                throw new Exception("参数错误");
            }
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    int i = 0;
                    cmd.Connection = conn;
                    for (int j = 0; j < sqlList.Count; j++)
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = sqlList[j];
                        if (parameterList[j] != null && parameterList[j].Length > 0)
                        {
                            cmd.Parameters.AddRange(parameterList[j]);
                        }
                        i += cmd.ExecuteNonQuery();
                        cmd.Parameters.Clear();
                        cmd.Prepare();
                    }
                    return i;
                }
            }
        }

        /// <summary>
        /// 得到一个字段的值
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public string ExecuteScalar(string sql)
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    object obj = cmd.ExecuteScalar();
                    cmd.Prepare();
                    return obj != null ? obj.ToString() : string.Empty;
                }
            }
        }

        /// <summary>
        /// 得到一个字段的值
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public string ExecuteScalar(string sql, SqlParameter[] parameter)
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    if (parameter != null && parameter.Length > 0)
                        cmd.Parameters.AddRange(parameter);
                    object obj = cmd.ExecuteScalar();
                    cmd.Parameters.Clear();
                    cmd.Prepare();
                    return obj != null ? obj.ToString() : string.Empty;
                }
            }
        }
        /// <summary>
        /// 得到一个字段的值
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public string GetFieldValue(string sql)
        {
            return ExecuteScalar(sql);
        }
        /// <summary>
        /// 得到一个字段的值
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public string GetFieldValue(string sql, SqlParameter[] parameter)
        {
            return ExecuteScalar(sql, parameter);
        }

        /// <summary>
        /// 获取一个sql的字段名称
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <returns></returns>
        public string GetFields(string sql, SqlParameter[] param)
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                System.Text.StringBuilder names = new System.Text.StringBuilder(500);
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    if (param != null && param.Length > 0)
                        cmd.Parameters.AddRange(param);
                    SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.SchemaOnly);
                    for (int i = 0; i < dr.FieldCount; i++)
                    {
                        names.Append("[" + dr.GetName(i) + "]" + (i < dr.FieldCount - 1 ? "," : string.Empty));
                    }
                    cmd.Parameters.Clear();
                    dr.Close();
                    dr.Dispose();
                    cmd.Prepare();
                    return names.ToString();
                }
            }
        }

        /// <summary>
        /// 获取一个sql的字段名称
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <param name="tableName">表名 </param>
        /// <returns></returns>
        public string GetFields(string sql, SqlParameter[] param, out string tableName)
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                System.Text.StringBuilder names = new System.Text.StringBuilder(500);
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    if (param != null && param.Length > 0)
                        cmd.Parameters.AddRange(param);
                    SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.SchemaOnly);
                    tableName = dr.GetSchemaTable().TableName;
                    for (int i = 0; i < dr.FieldCount; i++)
                    {
                        names.Append("[" + dr.GetName(i) + "]" + (i < dr.FieldCount - 1 ? "," : string.Empty));
                    }
                    cmd.Parameters.Clear();
                    dr.Close();
                    dr.Dispose();
                    cmd.Prepare();
                    return names.ToString();
                }
            }
        }

        /// <summary>
        /// 得到分页sql
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public string GetPaerSql(string sql, int size, int number, out long count, SqlParameter[] param = null)
        {
            string count1 = GetFieldValue(string.Format("select count(*) from ({0}) as PagerCountTemp", sql), param);
            long i;
            count = count1.IsLong(out i) ? i : 0;

            StringBuilder sql1 = new StringBuilder();
            sql1.Append("select * from (");
            sql1.Append(sql);
            sql1.AppendFormat(") as PagerTempTable");
            if (count > size)
            {
                sql1.AppendFormat(" where PagerAutoRowNumber between {0} and {1}", number * size - size + 1, number * size);
            }

            return sql1.ToString();
        }

        /// <summary>
        /// 得到分页sql
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public string GetPaerSql(string table, string fileds, string where, string order, int size, int number, out long count, SqlParameter[] param = null)
        {
            string where1 = string.Empty;
            if (where.IsNullOrEmpty())
            {
                where1 = "";
            }
            else
            {
                where1 = where.Trim();
                if (where1.StartsWith("and", StringComparison.CurrentCultureIgnoreCase))
                {
                    where1 = where1.Substring(3);
                }
            }
            string where2 = where1.IsNullOrEmpty() ? "" : "where " + where1;
            #region Modify L before
            string sql = string.Format("select {0},ROW_NUMBER() OVER(ORDER BY {1}) as PagerAutoRowNumber from {2} {3}", fileds, order, table, where2);
            string count1 = GetFieldValue(string.Format("select count(*) from {0} {1}", table, where2), param);
            #endregion

            //#region Modify L after
            //string sql = string.Empty;
            //string count1= string.Empty;
            //if (fileds.Contains("Distinct")) {
            //    sql = string.Format("select *,ROW_NUMBER() OVER(ORDER BY {1}) as PagerAutoRowNumber from (select {0} from {2} {3}) as TempTable", fileds, order, table, where2);
            //} else { 
            //    sql = string.Format("select {0},ROW_NUMBER() OVER(ORDER BY {1}) as PagerAutoRowNumber from {2} {3}", fileds, order, table, where2);
            //}
            //if (fileds.Contains("Distinct")) {
            //    string replace = string.Format("select *,ROW_NUMBER() OVER(ORDER BY {0}) as PagerAutoRowNumber from ", order);
            //    count1 = GetFieldValue(string.Format("select count(*) from {0} {1}", sql.Replace(replace, ""), where2), param);
            //} else {
            //    count1 = GetFieldValue(string.Format("select count(*) from {0} {1}", table, where2), param);
            //}
            //#endregion

            long i;
            count = count1.IsLong(out i) ? i : 0;

            StringBuilder sql1 = new StringBuilder();
            sql1.AppendFormat("select {0} from (", fileds.IsNullOrEmpty() ? "*" : fileds);
            sql1.Append(sql);
            sql1.AppendFormat(") as PagerTempTable");
            if (count > size)
            {
                sql1.AppendFormat(" where PagerAutoRowNumber between {0} and {1}", number * size - size + 1, number * size);
            }

            return sql1.ToString();
        }

        /// <summary>
        /// 得到分页sql,字段为表头说明
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public string GetPaerSql1(string table, string fileds, string namelist, string where, string order, int size, int number, out long count, SqlParameter[] param = null)
        {
            string where1 = string.Empty;
            if (where.IsNullOrEmpty())
            {
                where1 = "";
            }
            else
            {
                where1 = where.Trim();
                if (where1.StartsWith("and", StringComparison.CurrentCultureIgnoreCase))
                {
                    where1 = where1.Substring(3);
                }
            }
            string where2 = where1.IsNullOrEmpty() ? "" : "where " + where1;
            string sql = string.Format("select {0},ROW_NUMBER() OVER(ORDER BY {1}) as PagerAutoRowNumber from {2} {3}", fileds, order, table, where2);


            string count1 = GetFieldValue(string.Format("select count(*) from {0} {1}", table, where2), param);
            long i;
            count = count1.IsLong(out i) ? i : 0;

            StringBuilder sql1 = new StringBuilder();
            sql1.AppendFormat("select {0}  from (", namelist);
            sql1.Append(sql);
            sql1.AppendFormat(") as PagerTempTable");
            if (count > size)
            {
                sql1.AppendFormat(" where PagerAutoRowNumber between {0} and {1}", number * size - size + 1, number * size);
            }

            return sql1.ToString();
        }


        #region author L

        /// <summary>
        /// 执行存储过程
        /// </summary>
        public Dictionary<string, object> ExecuteProcedure(string procedureName, SqlParameter[] parameter, SqlParameter[] outParameter) {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            using (SqlConnection conn = new SqlConnection(ConnectionString)) {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(procedureName, conn)) {
                    cmd.CommandType = CommandType.StoredProcedure;
                    if (parameter != null && parameter.Length > 0)
                        cmd.Parameters.AddRange(parameter);
                    if (outParameter != null && outParameter.Length > 0) {
                        foreach (var item in outParameter) {
                            cmd.Parameters.Add(item);
                            cmd.Parameters[item.ParameterName].Direction = ParameterDirection.Output;
                        }
                    }
                    int i = cmd.ExecuteNonQuery();
                    if (outParameter != null && outParameter.Length > 0) {
                        foreach (var item in outParameter) {
                            dictionary.Add(item.ParameterName, cmd.Parameters[item.ParameterName].Value);
                        }
                    }
                    cmd.Parameters.Clear();
                    cmd.Prepare();
                    return dictionary;
                }
            }
        }

        /// <summary>
        /// 执行存储过程
        /// </summary>
        public int ExecuteProcedure(string procedureName, IDataParameter[] parameter) {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            using (SqlConnection conn = new SqlConnection(ConnectionString)) {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(procedureName, conn)) {
                    cmd.CommandType = CommandType.StoredProcedure;
                    if (parameter != null && parameter.Length > 0)
                        cmd.Parameters.AddRange(parameter);
                    int i = cmd.ExecuteNonQuery();
                    cmd.Parameters.Clear();
                    cmd.Prepare();
                    return i;
                }
            }
        }

        /// <summary>
        /// 存储参数的集合组装成where字符串
        /// </summary>
        /// <returns>Key为'where'保存的是where字符串，Key为para保存的是参数数组</returns>
        public Dictionary<string, object> KeysToWhereString(Dictionary<KeyValuePair<string, RoadFlow.Data.Model.SQLFilterType>, object> where) {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            Dictionary<string, string> inWhere = new Dictionary<string, string>();//in这种条件特殊使用

            List<SqlParameter> parameters = new List<SqlParameter>();
            foreach (var item in where) {
                if (item.Key.Value == Model.SQLFilterType.MIN) {
                    parameters.Add(new SqlParameter("@" + item.Key.Key + "Min", item.Value));
                } else if (item.Key.Value == Model.SQLFilterType.MINNotEqual) {
                    parameters.Add(new SqlParameter("@" + item.Key.Key + "MinNotEqual", item.Value));
                } else if (item.Key.Value == Model.SQLFilterType.IN) {
                    StringBuilder value = new StringBuilder();
                    foreach (string str in (List<string>)item.Value) {
                        if (value.ToString().IsNullOrEmpty()) {
                            value.Append("'").Append(str).Append("'");
                        } else {
                            value.Append(",").Append("'").Append(str).Append("'");
                        }
                    }
                    inWhere.Add(item.Key.Key, value.ToString());
                } else {
                    parameters.Add(new SqlParameter("@" + item.Key.Key, item.Value));
                }
            }
            dictionary.Add("para", parameters.ToArray());

            StringBuilder whereStr = new StringBuilder();
            foreach (var item in where.Keys) {
                if (item.Value == Model.SQLFilterType.CHARINDEX) {
                    whereStr.AppendFormat(" And CHARINDEX(@{0},[{0}])>0", item.Key);
                } else if (item.Value == Model.SQLFilterType.EQUAL) {
                    whereStr.AppendFormat(" And [{0}]=@{0}", item.Key);
                } else if (item.Value == Model.SQLFilterType.MIN) {//大于小于这里很可能两个过滤参数，但是关键字只有一个。因为都是关于一个字段的筛选。
                    whereStr.AppendFormat(" And [{0}]>=@{0}Min", item.Key);
                } else if (item.Value == Model.SQLFilterType.MAX) {
                    whereStr.AppendFormat(" And [{0}]<=@{0}", item.Key);
                } else if (item.Value == Model.SQLFilterType.MINNotEqual) {
                    whereStr.AppendFormat(" And [{0}]>@{0}MinNotEqual", item.Key);
                } else if (item.Value == Model.SQLFilterType.MAXNotEqual) {
                    whereStr.AppendFormat(" And [{0}]<@{0}", item.Key);
                } else if (item.Value == Model.SQLFilterType.IN) {
                    whereStr.AppendFormat(" And [{0}] in ({1})", item.Key, inWhere[item.Key]);
                }
            }
            dictionary.Add("where", whereStr.ToString());
            return dictionary;
        }

        /// <summary>
        /// 存储参数的集合组装成where字符串
        /// </summary>
        /// <returns>Key为'where'保存的是where字符串，Key为para保存的是参数数组</returns>
        public Dictionary<string, object> PredicatesToWhereString(List<RoadFlow.Data.Model.Predicates> predicates, bool isAutoStatus)
        {
            Dictionary<string, object> result = new Dictionary<string, object>();
            StringBuilder whereStr = new StringBuilder();
            List<SqlParameter> parameters = new List<SqlParameter>();

            foreach (var item in predicates)
            {
                //Model.SQLFilterType
                if (item.Operator == Model.SQLFilterType.MIN)
                {//大于小于这里很可能两个过滤参数，但是关键字只有一个。因为都是关于一个字段的筛选。
                    parameters.Add(new SqlParameter("@" + item.FieldName + item.Operator, item.Value));
                    whereStr.AppendFormat(" And [{0}]>=@{1}", item.FieldName, item.FieldName + item.Operator);
                }
                else if (item.Operator == Model.SQLFilterType.MINNotEqual)
                {
                    parameters.Add(new SqlParameter("@" + item.FieldName + "MinNotEqual", item.Value));
                    whereStr.AppendFormat(" And [{0}]>@{0}MinNotEqual", item.FieldName);
                }
                else if (item.Operator == Model.SQLFilterType.IN)
                {
                    StringBuilder value = new StringBuilder();
                    foreach (string str in (List<string>)item.Value)
                    {
                        if (value.ToString().IsNullOrEmpty())
                        {
                            value.Append("'").Append(str).Append("'");
                        }
                        else
                        {
                            value.Append(",").Append("'").Append(str).Append("'");
                        }
                    }
                    whereStr.AppendFormat(" And [{0}] in ({1})", item.FieldName, value.ToString());
                }
                else
                {
                    parameters.Add(new SqlParameter("@" + item.FieldName, item.Value));
                    if (item.Operator == Model.SQLFilterType.CHARINDEX)
                    {
                        whereStr.AppendFormat(" And CHARINDEX(@{0},[{0}])>0", item.FieldName);
                    }
                    else if (item.Operator == Model.SQLFilterType.EQUAL)
                    {
                        whereStr.AppendFormat(" And [{0}]=@{0}", item.FieldName);
                    }

                    else if (item.Operator == Model.SQLFilterType.MAX)
                    {
                        whereStr.AppendFormat(" And [{0}]<=@{0}", item.FieldName);
                    }

                    else if (item.Operator == Model.SQLFilterType.MAXNotEqual)
                    {
                        whereStr.AppendFormat(" And [{0}]<@{0}", item.FieldName);
                    }
                }
            }

            if (isAutoStatus)
            {
                whereStr.AppendFormat(" And [Status]={0}", (int)RoadFlow.Data.Model.Status.Normal);
            }

            result.Add("para", parameters.ToArray());
            result.Add("where", whereStr.ToString());

            return result;
        }


        /// <summary>
        /// 存储参数的集合组装成where字符串
        /// </summary>
        /// <param name="isAutoNormal">是否自动筛选状态正常的数据。</param>
        /// <returns>Key为'where'保存的是where字符串，Key为para保存的是参数数组</returns>
        public Dictionary<string, object> DynamicToWhereString(dynamic para, bool isAutoNormal)
        {
            Dictionary<string, object> result = new Dictionary<string, object>();
            StringBuilder whereStr = new StringBuilder();
            List<SqlParameter> parameters = new List<SqlParameter>();

            DynamicParaToSqlPraAndWhere(whereStr, parameters, para);

            if (isAutoNormal)
            {
                whereStr.AppendFormat(" And [Status]={0}", (int)RoadFlow.Data.Model.Status.Normal);
            }

            result.Add("para", parameters.ToArray());
            result.Add("where", whereStr.ToString());
            return result;
        }

        public T GetModel<T>(string tableName, string order, params KeyValuePair<string, object>[] para) {
            StringBuilder where = new StringBuilder("where 1=1 ");
            List<SqlParameter> parameter = new List<SqlParameter>();

            foreach (KeyValuePair<string, object> item in para)
            {
                where.Append(" And [").Append(item.Key).Append("]=@").Append(item.Key);
                parameter.Add(new SqlParameter("@" + item.Key, item.Value));
            }

            return GetModelByWhereAndSqlParameter<T>(tableName, where.ToString(), parameter.ToArray(), order);
        }

        public T GetByPara<T>(string tableName, string order, dynamic para)
        {
            StringBuilder where = new StringBuilder("where 1=1 ");
            List<SqlParameter> parameter = new List<SqlParameter>();

            #region 反射回去过滤参数并生成where语句和SqlParameter
            DynamicParaToSqlPraAndWhere(where, parameter, para);
            #endregion

            return GetModelByWhereAndSqlParameter<T>(tableName, where.ToString(), parameter.ToArray(), order);
        }

        public List<T> GetList<T>(string sql, SqlParameter[] para) {
            //return DataTableToList<T>(GetDataTable(sql, para));
            return GetDataTable(sql, para).ToList<T>();
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model"></param>
        /// <returns></returns>
        public int AddModel<T>(T model, string tableName = "") {
            Type t = typeof(T);
            StringBuilder valuesName = new StringBuilder();
            List<SqlParameter> parameters = new List<SqlParameter>();
            StringBuilder column = new StringBuilder();
            StringBuilder sql = new StringBuilder();

            RoadFlow.Utility.Tools.GetPropertiesValue(delegate (int col,object value,PropertyInfo propertyInfo)
            {
                if (value==null)
                {
                    return true;
                }

                column.Append(",[").Append(propertyInfo.Name).Append("]");
                parameters.Add(new SqlParameter("@" + propertyInfo.Name, value));
                valuesName.Append(",@").Append(propertyInfo.Name);
                return false;
            }, model);

            if (column.ToString().IsNullOrEmpty()) {//没有添加任何列
                return 0;
            } else {
                column.Remove(0, 1);//移除首个"，"
                valuesName.Remove(0, 1);//移除首个"，"
                if (tableName.IsNullOrEmpty()) {
                    sql.AppendFormat("insert into [{0}]({1}) values({2})", t.Name, column.ToString(), valuesName.ToString());
                } else {
                    sql.AppendFormat("insert into [{0}]({1}) values({2})", tableName, column.ToString(), valuesName.ToString());
                }
                return Execute(sql.ToString(), parameters.ToArray());
            }
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model"></param>
        /// <returns></returns>
        public int UpdateModel<T>(T model, string tableName, params KeyValuePair<string, object>[] where) {
            Type t = typeof(T);
            StringBuilder values = new StringBuilder();
            List<SqlParameter> parameters = new List<SqlParameter>();
            StringBuilder column = new StringBuilder();
            StringBuilder sql = new StringBuilder();

            UpdateModelOfGetPropertiesValue(values, parameters, model);

            if (values.ToString().IsNullOrEmpty()) {//没有添加任何列
                return 0;
            } else {
                values.Remove(0, 1);//移除首个"，"

                StringBuilder where2 = new StringBuilder(" where 1=1");
                foreach (var item in where) {
                    where2.Append(" And " + item.Key + "=@" + item.Key + "Where");
                    parameters.Add(new SqlParameter("@" + item.Key + "Where", item.Value));//加"where"是为了让筛选条件和更新参数不冲突
                }
                if (tableName.IsNullOrEmpty()) {
                    sql.AppendFormat("update [{0}] set {1} {2}", t.Name, values.ToString(), where2.ToString());
                } else {
                    sql.AppendFormat("update [{0}] set {1} {2}", tableName, values.ToString(), where2.ToString());
                }
                return Execute(sql.ToString(), parameters.ToArray());
            }
        }

        /// <summary>
        /// 编辑
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model"></param>
        /// <returns></returns>
        public int UpdateModel<T>(T model, Dictionary<string, SqlParameter> where, string tableName) {
            Type t = typeof(T);
            StringBuilder values = new StringBuilder();
            List<SqlParameter> parameters = new List<SqlParameter>();
            StringBuilder column = new StringBuilder();
            StringBuilder sql = new StringBuilder();

            UpdateModelOfGetPropertiesValue(values, parameters, model);

            if (string.IsNullOrWhiteSpace(values.ToString())) {//没有添加任何列
                return 0;
            } else {
                values.Remove(0, 1);//移除首个"，"

                StringBuilder where2 = new StringBuilder(" where 1=1");
                foreach (var item in where) {
                    where2.Append(item.Key);
                    parameters.Add(item.Value);
                }

                sql.AppendFormat("update [{0}] set {1} {2}", tableName, values.ToString(), where2.ToString());
                return Execute(sql.ToString(), parameters.ToArray());
            }
        }

        #region 公共方法
        private T GetModelByWhereAndSqlParameter<T>(string tableName, string where, SqlParameter[] parameter, string order)
        {
            T result = default(T);

            //生成sql
            string sql = "select * from [" + tableName + "] " + where + " order by " + order;

            //反射生成对象
            DataTable dt = GetDataTable(sql, parameter);
            if (dt.Rows.Count > 0)
            {
                result = RoadFlow.Utility.Tools.SetPropertiesValue<T>(delegate (PropertyInfo propertyInfo)
                {//获取设置的值
                    return GetValueByPropertyNameFromDataTable(dt, propertyInfo);
                });
            }
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private List<T> DataTableToList<T>(DataTable dt) {
            List<T> list = new List<T>();
            Type t = typeof(T);
            if (dt.Rows.Count > 0) {
                //循环充填模型对象。
                foreach (DataRow dr in dt.Rows) {
                    T instance = RoadFlow.Utility.Tools.SetPropertiesValue<T>(delegate (PropertyInfo propertyInfo)
                    {//获取设置的值
                        return GetValueByPropertyNameFromDataTable(dt,propertyInfo);
                    });
                    list.Add(instance);
                }
            }
            return list;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="propertyInfo"></param>
        /// <returns></returns>
        private object GetValueByPropertyNameFromDataTable(DataTable dt,PropertyInfo propertyInfo)
        {
            if (dt.Columns.Contains(propertyInfo.Name) && !(dt.Rows[0][propertyInfo.Name] is DBNull))
            {
                return dt.Rows[0][propertyInfo.Name];
            }
            return null;
        }

        /// <summary>
        /// UpdateModel的获取属性值并拼接参数
        /// </summary>
        private void UpdateModelOfGetPropertiesValue<T>(StringBuilder values,List<SqlParameter> parameters,T model)
        {
            RoadFlow.Utility.Tools.GetPropertiesValue(delegate (int col, object value, PropertyInfo propertyInfo)
            {
                if (value == null)
                {//continue
                    return true;
                }
                parameters.Add(new SqlParameter("@" + propertyInfo.Name, value));
                values.Append(",[").Append(propertyInfo.Name).Append("]=@").Append(propertyInfo.Name);
                return false;
            }, model);
        }

        private void DynamicParaToSqlPraAndWhere(StringBuilder where,List<SqlParameter> parameter,dynamic para)
        {
            RoadFlow.Utility.AfterGetPropertiesValueByDynamic action = delegate (object value, PropertyInfo propertyInfo)
            {
                if (value == null)
                {
                    return true;
                }
                parameter.Add(new SqlParameter("@" + propertyInfo.Name, value));
                where.AppendFormat(" And [{0}]=@{0}", propertyInfo.Name);
                return false;
            };
            RoadFlow.Utility.Tools.GetPropertiesValueByDynamic(action, para);
        }
        
        #endregion
        #endregion

    }
}
