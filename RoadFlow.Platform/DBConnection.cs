using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;
using RoadFlow.Data.Model;

namespace RoadFlow.Platform
{
    public class DBConnection
    {
        private RoadFlow.Data.Interface.IDBConnection dataDBConnection;
        public DBConnection()
        {
            this.dataDBConnection = Data.Factory.Factory.GetDBConnection();
        }

        /// <summary>
        /// 连接类型
        /// </summary>
        public enum Types
        { 
            SqlServer,
            Oracle,
            MySql
        }

        /// <summary>
        /// 新增
        /// </summary>
        public int Add(RoadFlow.Data.Model.DBConnection model)
        {
            int i = dataDBConnection.Add(model);
            ClearCache();
            return i;
        }
        /// <summary>
        /// 更新
        /// </summary>
        public int Update(RoadFlow.Data.Model.DBConnection model)
        {
            int i = dataDBConnection.Update(model);
            ClearCache();
            return i;
        }
        /// <summary>
        /// 查询所有记录
        /// </summary>
        public List<RoadFlow.Data.Model.DBConnection> GetAll(bool fromCache=false)
        {
            if (!fromCache)
            {
                return dataDBConnection.GetAll();
            }
            else
            {
                string key = RoadFlow.Utility.Keys.CacheKeys.DBConnnections.ToString();
                object obj = RoadFlow.Cache.IO.Opation.Get(key);
                if (obj != null && obj is List<RoadFlow.Data.Model.DBConnection>)
                {
                    return obj as List<RoadFlow.Data.Model.DBConnection>;
                }
                else
                {
                    var list = dataDBConnection.GetAll();
                    RoadFlow.Cache.IO.Opation.Set(key, list);
                    return list;
                }
            }
        }
        /// <summary>
        /// 查询单条记录
        /// </summary>
        public RoadFlow.Data.Model.DBConnection Get(Guid id)
        {
            return dataDBConnection.Get(id);
        }
        /// <summary>
        /// 删除
        /// </summary>
        public int Delete(Guid id)
        {
            int i = dataDBConnection.Delete(id);
            ClearCache();
            return i;
        }
        /// <summary>
        /// 查询记录条数
        /// </summary>
        public long GetCount()
        {
            return dataDBConnection.GetCount();
        }
        /// <summary>
        /// 连接类型
        /// </summary>
        public enum ConnTypes
        {
            SqlServer,
            Oracle,
            MySql
        }
        /// <summary>
        /// 得到所有数据连接类型的下拉选择
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public string GetAllTypeOptions(string value = "")
        {
            StringBuilder options = new StringBuilder();
            var array = Enum.GetValues(typeof(ConnTypes));
            foreach (var arr in array)
            {
                options.AppendFormat("<option value=\"{0}\" {1}>{0}</option>", arr, arr.ToString() == value ? "selected=\"selected\"" : "");
            }
            return options.ToString();
        }
        /// <summary>
        /// 得到所有数据连接的下拉选择
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public string GetAllOptions(string value = "")
        {
            var conns = GetAll(true);
            StringBuilder options = new StringBuilder();
            foreach (var conn in conns)
            {
                options.AppendFormat("<option value=\"{0}\" {1}>{2}</option>", conn.ID,
                    string.Compare(conn.ID.ToString(), value, true) == 0 ? "selected=\"selected\"" : "", conn.Name);
            }
            return options.ToString();
        }
        /// <summary>
        /// 清除缓存
        /// </summary>
        public void ClearCache()
        {
            string key = RoadFlow.Utility.Keys.CacheKeys.DBConnnections.ToString();
            RoadFlow.Cache.IO.Opation.Remove(key);
        }

        /// <summary>
        /// 根据连接ID得到所有表
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<string> GetTables(Guid id)
        {
            var allConns = GetAll(true);
            var conn = allConns.Find(p => p.ID == id);
            if (conn == null) return new List<string>();
            List<string> tables = new List<string>();
            switch (conn.Type)
            {
                case "SqlServer":
                    tables = getTables_SqlServer(conn);
                    break;
                case "MySql":
                    tables = getTables_MySql(conn);
                    break;
            }
            return tables;
        }

        /// <summary>
        /// 得到所有字段
        /// </summary>
        /// <param name="id">连接ID</param>
        /// <param name="table">表名</param>
        /// <returns></returns>
        public Dictionary<string, string> GetFields(Guid id, string table)
        {
            if (table.IsNullOrEmpty()) return new Dictionary<string, string>();
            var allConns = GetAll(true);
            var conn = allConns.Find(p => p.ID == id);
            if (conn == null) return new Dictionary<string, string>();
            Dictionary<string, string> fields = new Dictionary<string, string>();
            switch (conn.Type)
            {
                case "SqlServer":
                    fields = getFields_SqlServer(conn, table);
                    break;
                case "MySql":
                    fields = getFields_MySql(conn, table);
                    break;
            }
            return fields;
        }


        public Dictionary<string, string> GetOptionsFields(Guid id, string sql)
        {
            Dictionary<string, string> list = new Dictionary<string, string>();
            RoadFlow.Platform.DBConnection bdbconn = new RoadFlow.Platform.DBConnection();
            RoadFlow.Data.Model.DBConnection dbconn = bdbconn.Get(id);
            using (System.Data.IDbConnection conn = bdbconn.GetConnection(dbconn))
            {
                if (conn == null)
                {
                    return list;
                }
                try
                {
                    conn.Open();
                }
                catch (Exception ex)
                {
                    System.Web.HttpContext.Current.Response.Write("连接数据库出错：" + ex.Message);
                    RoadFlow.Platform.Log.Add(ex);
                }
                List<System.Data.IDataParameter> parList = new List<System.Data.IDataParameter>();
                System.Data.IDbDataAdapter dataAdapter = bdbconn.GetDataAdapter(conn, dbconn.Type, sql, parList.ToArray());
                System.Data.DataSet ds = new System.Data.DataSet();
                dataAdapter.Fill(ds);
                foreach (System.Data.DataRow row in ds.Tables[0].Rows)
                {
                    list.Add(row[0].ToString(),row[1].ToString());
                }
                return list;
            }
        }

        /// <summary>
        /// 得到一个连接一个表一个字段的值
        /// </summary>
        /// <param name="link_table_field"></param>
        /// <returns></returns>
        public string GetFieldValue(string link_table_field, Dictionary<string,string> pkFieldValue)
        {
            if (link_table_field.IsNullOrEmpty()) return "";
            string[] array = link_table_field.Split('.');
            if (array.Length != 3) return "";
            string link = array[0];
            string table = array[1];
            string field = array[2];
            var allConns = GetAll(true);
            Guid linkid;
            if (!link.IsGuid(out linkid)) return "";
            var conn = allConns.Find(p => p.ID == linkid);
            if (conn == null) return "";
            List<string> fields = new List<string>();
            string value = string.Empty;
            switch (conn.Type)
            {
                case "SqlServer":
                    value = getFieldValue_SqlServer(conn, table, field, pkFieldValue);
                    break;
                case "Oracle":
                    value = getFieldValue_Oracle(conn, table, field, pkFieldValue);
                    break;
                case "MySql":
                    value = getFieldValue_MySql(conn, table, field, pkFieldValue);
                    break;
            }
            return value;
        }

        /// <summary>
        /// 得到一个连接一个表一个字段的值
        /// </summary>
        /// <param name="link_table_field"></param>
        /// <returns></returns>
        public string GetFieldValue(string link_table_field, string pkField, string pkFieldValue)
        {
            if (link_table_field.IsNullOrEmpty())
            {
                return "";
            }
            string[] array = link_table_field.Split('.');
            if (array.Length != 3)
            {
                return "";
            }
            string link = array[0];
            string table = array[1];
            string field = array[2];
            var allConns = GetAll(true);
            Guid linkid;
            if (!link.IsGuid(out linkid))
            {
                return "";
            }
            var conn = allConns.Find(p => p.ID == linkid);
            if (conn == null)
            {
                return "";
            }
            string value = string.Empty;
            switch (conn.Type)
            {
                case "SqlServer":
                    value = getFieldValue_SqlServer(conn, table, field, pkField, pkFieldValue);
                    break;
                case "Oracle":
                    value = getFieldValue_Oracle(conn, table, field, pkField, pkFieldValue);
                    break;
                case "MySql":
                    value = getFieldValue_MySql(conn, table, field, pkField, pkFieldValue);
                    break;
            }
            return value;
        }

        /// <summary>
        /// 测试一个连接
        /// </summary>
        /// <param name="connID"></param>
        /// <returns></returns>
        public string Test(Guid connID)
        {
            var link = Get(connID);
            if (link == null) return "未找到连接!";
            switch (link.Type)
            { 
                case "SqlServer":
                    return test_SqlServer(link);
                case "Oracle":
                    return test_Oracle(link);
                case "MySql":
                    return test_MySql(link);

            }

            return "";
        }
        #region 测试连接
        /// <summary>
        /// 测试一个连接
        /// </summary>
        /// <param name="conn"></param>
        /// <returns></returns>
        private string test_SqlServer(RoadFlow.Data.Model.DBConnection conn)
        {
            using (SqlConnection sqlConn = new SqlConnection(conn.ConnectionString))
            {
                try
                {
                    sqlConn.Open();
                    return "连接成功!";
                }
                catch (SqlException err)
                {
                    return err.Message;
                }
            }
        }
        private string test_MySql(RoadFlow.Data.Model.DBConnection conn)
        {
            using (MySqlConnection sqlConn = new MySqlConnection(conn.ConnectionString))
            {
                try
                {
                    sqlConn.Open();
                    return "连接成功!";
                }
                catch (MySqlException err)
                {
                    return err.Message;
                }
            }
        }
        
        /// <summary>
        /// 测试一个连接(oracle)
        /// </summary>
        /// <param name="conn"></param>
        /// <returns></returns>
        private string test_Oracle(RoadFlow.Data.Model.DBConnection conn)
        {
            return "";
        }
        #endregion

        #region 测试一个sql条件合法性
        /// <summary>
        /// 测试一个sql条件合法性
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        private string testSql_SqlServer(RoadFlow.Data.Model.DBConnection conn, string sql)
        {
            using (SqlConnection sqlConn = new SqlConnection(conn.ConnectionString))
            {
                try
                {
                    sqlConn.Open();
                }
                catch (SqlException err)
                {
                    return err.Message;
                }
                using (SqlCommand cmd = new SqlCommand(sql, sqlConn))
                {
                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch (SqlException err)
                    {
                        return err.Message;
                    }
                }
                return "";
            }
        }
        private string testSql_MySql(RoadFlow.Data.Model.DBConnection conn, string sql)
        {
            using (MySqlConnection sqlConn = new MySqlConnection(conn.ConnectionString))
            {
                try
                {
                    sqlConn.Open();
                }
                catch (MySqlException err)
                {
                    return err.Message;
                }
                using (MySqlCommand cmd = new MySqlCommand(sql, sqlConn))
                {
                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch (MySqlException err)
                    {
                        return err.Message;
                    }
                }
                return "";
            }
        }

        /// <summary>
        /// 测试一个sql条件合法性(oracle)
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        private string testSql_Oracle(RoadFlow.Data.Model.DBConnection conn, string sql)
        {
            return "";
        }

        #endregion


        #region 得到一个连接所有表
        /// <summary>
        /// 得到一个连接所有表
        /// </summary>
        /// <param name="conn"></param>
        /// <returns></returns>
        private List<string> getTables_SqlServer(RoadFlow.Data.Model.DBConnection conn)
        {
            using (SqlConnection sqlConn = new SqlConnection(conn.ConnectionString))
            {
                try
                {
                    sqlConn.Open();
                }
                catch (SqlException err)
                {
                    Log.Add(err);
                    return new List<string>();
                }
                List<string> tables = new List<string>();
                string sql = "SELECT name FROM sysobjects WHERE xtype='U' OR xtype='V' ORDER BY name";
                using (SqlCommand sqlCmd = new SqlCommand(sql, sqlConn))
                {
                    SqlDataReader dr = sqlCmd.ExecuteReader();
                    while (dr.Read())
                    {
                        tables.Add(dr.GetString(0));
                    }
                    dr.Close();
                    return tables;
                }
            }
        }

        private List<string> getTables_MySql(RoadFlow.Data.Model.DBConnection conn)
        {
            using (MySqlConnection sqlConn = new MySqlConnection(conn.ConnectionString))
            {
                try
                {
                    sqlConn.Open();
                }
                catch (MySqlException err)
                {
                    Log.Add(err);
                    return new List<string>();
                }
                List<string> tables = new List<string>();
                string sql =string.Format( "select table_name from information_schema.tables where table_schema='{0}'",sqlConn.Database);
                using (MySqlCommand sqlCmd = new MySqlCommand(sql, sqlConn))
                {
                    MySqlDataReader dr = sqlCmd.ExecuteReader();
                    while (dr.Read())
                    {
                        tables.Add(dr.GetString(0));
                    }
                    dr.Close();
                    return tables;
                }
            }
        }

        #endregion

        #region 得到一个连接一个表所有字段
        /// <summary>
        /// 得到一个连接一个表或试图的所有字段及说明
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="table"></param>
        /// <returns></returns>
        private Dictionary<string, string> getFields_SqlServer(RoadFlow.Data.Model.DBConnection conn, string table)
        {
            using (SqlConnection sqlConn = new SqlConnection(conn.ConnectionString))
            {
                try
                {
                    sqlConn.Open();
                }
                catch (SqlException err)
                {
                    Log.Add(err);
                    return new Dictionary<string, string>();
                }
                Dictionary<string, string> fields = new Dictionary<string, string>();
                string sql = string.Format(@"SELECT a.name as f_name, b.value from 
                sys.syscolumns a LEFT JOIN sys.extended_properties b on a.id=b.major_id 
                AND a.colid=b.minor_id AND b.name='MS_Description' 
                WHERE object_id('{0}')=a.id ORDER BY a.colid", table);
                using (SqlCommand sqlCmd = new SqlCommand(sql, sqlConn))
                {
                    SqlDataReader dr = sqlCmd.ExecuteReader();
                    while (dr.Read())
                    {
                        fields.Add(dr.GetString(0), dr.IsDBNull(1) ? "" : dr.GetString(1));
                    }
                    dr.Close();
                }
                return fields;
            }
        }

        private Dictionary<string, string> getFields_MySql(RoadFlow.Data.Model.DBConnection conn, string table)
        {
            using (MySqlConnection MysqlConn = new MySqlConnection(conn.ConnectionString))
            {
                try
                {
                    MysqlConn.Open();
                }
                catch (MySqlException err)
                {
                    Log.Add(err);
                    return new Dictionary<string, string>();
                }
                Dictionary<string, string> fields = new Dictionary<string, string>();
                string sql = string.Format(@"select column_name as f_name,column_comment as value from information_schema.columns where  table_name='{0}'and table_schema ='{1}' ", table, MysqlConn.Database);
                using (MySqlCommand mysqlCmd = new MySqlCommand(sql, MysqlConn))
                {
                    MySqlDataReader dr = mysqlCmd.ExecuteReader();
                    while (dr.Read())
                    {
                        fields.Add(dr.GetString(0), dr.IsDBNull(1) ? "" : dr.GetString(1));
                    }
                    dr.Close();
                    return fields;
                }
            }
        }

        /// <summary>
        /// 得到一个连接一个表所有字段(Oracle)
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="table"></param>
        /// <returns></returns>
        private Dictionary<string, string> getFields_Oracle(RoadFlow.Data.Model.DBConnection conn, string table)
        {
            return new Dictionary<string, string>();
        }

        #endregion




        #region 得到一个连接一个表一个字段的值


        /// <summary>
        /// 得到一个连接一个表一个字段的值
        /// </summary>
        /// <param name="conn">连接ID</param>
        /// <param name="table">表名</param>
        /// <param name="field">字段名</param>
        /// <param name="pkFieldValue">主键和值字典</param>
        /// <returns></returns>
        private string getFieldValue_SqlServer(RoadFlow.Data.Model.DBConnection conn, string table, string field, Dictionary<string, string> pkFieldValue)
        {
            using (SqlConnection sqlConn = new SqlConnection(conn.ConnectionString))
            {
                try
                {
                    sqlConn.Open();
                }
                catch (SqlException err)
                {
                    Log.Add(err);
                    return "";
                }
                List<string> fields = new List<string>();
                StringBuilder sql = new StringBuilder();
                sql.AppendFormat("select {0} from {1} where 1=1", field, table);
                foreach (var pk in pkFieldValue)
                {
                    sql.AppendFormat(" and {0}='{1}'", pk.Key, pk.Value);
                }
               
                using (SqlCommand sqlCmd = new SqlCommand(sql.ToString(), sqlConn))
                {
                    SqlDataReader dr = sqlCmd.ExecuteReader();
                    string value = string.Empty;
                    if (dr.HasRows)
                    {
                        dr.Read();
                        value = dr.GetString(0);
                    }
                    dr.Close();
                    return value;
                }
            }
        }

        private string getFieldValue_MySql(RoadFlow.Data.Model.DBConnection conn, string table, string field, Dictionary<string, string> pkFieldValue)
        {
            using (MySqlConnection sqlConn = new MySqlConnection(conn.ConnectionString))
            {
                try
                {
                    sqlConn.Open();
                }
                catch (MySqlException err)
                {
                    Log.Add(err);
                    return "";
                }
                List<string> fields = new List<string>();
                StringBuilder sql = new StringBuilder();
                sql.AppendFormat("select {0} from {1} where 1=1", field, table);
                foreach (var pk in pkFieldValue)
                {
                    sql.AppendFormat(" and {0}='{1}'", pk.Key, pk.Value);
                }

                using (MySqlCommand sqlCmd = new MySqlCommand(sql.ToString(), sqlConn))
                {
                    MySqlDataReader dr = sqlCmd.ExecuteReader();
                    string value = string.Empty;
                    if (dr.HasRows)
                    {
                        dr.Read();
                        value = dr.GetString(0);
                    }
                    dr.Close();
                    return value;
                }
            }
        }

        /// <summary>
        /// 得到一个连接一个表一个字段的值(Oracle)
        /// </summary>
        /// <param name="conn">连接ID</param>
        /// <param name="table">表名</param>
        /// <param name="field">字段名</param>
        /// <param name="pkFieldValue">主键和值字典</param>
        /// <returns></returns>
        private string getFieldValue_Oracle(RoadFlow.Data.Model.DBConnection conn, string table, string field, Dictionary<string, string> pkFieldValue)
        {
            return "";
        }

        #endregion

        #region 得到一个连接一个表一个字段的值
        /// <summary>
        /// 得到一个连接一个表一个字段的值
        /// </summary>
        /// <param name="linkID">连接ID</param>
        /// <param name="table">表</param>
        /// <param name="field">字段</param>
        /// <param name="pkField">主键字段</param>
        /// <param name="pkFieldValue">主键值</param>
        /// <returns></returns>
        private string getFieldValue_SqlServer(RoadFlow.Data.Model.DBConnection conn, string table, string field, string pkField, string pkFieldValue)
        {
            string v = "";
            using (SqlConnection sqlConn = new SqlConnection(conn.ConnectionString))
            {
                try
                {
                    sqlConn.Open();
                }
                catch (SqlException err)
                {
                    Log.Add(err);
                    return "";
                }
                string sql = string.Format("SELECT {0} FROM {1} WHERE {2} = '{3}'", field, table, pkField, pkFieldValue);
                using (SqlDataAdapter dap = new SqlDataAdapter(sql, sqlConn))
                {
                    try
                    {
                        DataTable dt = new DataTable();
                        dap.Fill(dt);
                        if (dt.Rows.Count > 0)
                        {
                            v = dt.Rows[0][0].ToString();
                        }
                    }
                    catch (SqlException err)
                    {
                        Log.Add(err);
                    }
                    return v;
                }
            }
        }

        private string getFieldValue_MySql(RoadFlow.Data.Model.DBConnection conn, string table, string field, string pkField, string pkFieldValue)
        {
            string v = "";
            using (MySqlConnection sqlConn = new MySqlConnection(conn.ConnectionString))
            {
                try
                {
                    sqlConn.Open();
                }
                catch (MySqlException err)
                {
                    Log.Add(err);
                    return "";
                }
                string sql = string.Format("SELECT {0} FROM {1} WHERE {2} = '{3}'", field, table, pkField, pkFieldValue);
                using (MySqlDataAdapter dap = new MySqlDataAdapter(sql, sqlConn))
                {
                    try
                    {
                        DataTable dt = new DataTable();
                        dap.Fill(dt);
                        if (dt.Rows.Count > 0)
                        {
                            v = dt.Rows[0][0].ToString();
                        }
                    }
                    catch (MySqlException err)
                    {
                        Log.Add(err);
                    }
                    return v;
                }
            }
        }

        /// <summary>
        /// 得到一个连接一个表一个字段的值(Oracle)
        /// </summary>
        /// <param name="linkID">连接ID</param>
        /// <param name="table">表</param>
        /// <param name="field">字段</param>
        /// <param name="pkField">主键字段</param>
        /// <param name="pkFieldValue">主键值</param>
        /// <returns></returns>
        private string getFieldValue_Oracle(RoadFlow.Data.Model.DBConnection conn, string table, string field, string pkField, string pkFieldValue)
        {
            return "";
        }
        #endregion
        /// <summary>
        /// 根据连接实体得到连接
        /// </summary>
        /// <param name="linkID"></param>
        /// <returns></returns>
        public System.Data.IDbConnection GetConnection(RoadFlow.Data.Model.DBConnection dbconn)
        {
            if (dbconn == null || dbconn.Type.IsNullOrEmpty() || dbconn.ConnectionString.IsNullOrEmpty())
            {
                return null;
            }
            IDbConnection conn = null ;
            switch (dbconn.Type)
            { 
                case "SqlServer":
                    conn = new SqlConnection(dbconn.ConnectionString);
                    break;
                case "MySql":
                    conn = new MySqlConnection(dbconn.ConnectionString);
                    break;
                
            }

            return conn;

        }

        /// <summary>
        /// 根据连接实体得到数据适配器
        /// </summary>
        /// <param name="linkID"></param>
        /// <returns></returns>
        public System.Data.IDbDataAdapter GetDataAdapter(IDbConnection conn, string connType, string cmdText, IDataParameter[] parArray)
        {
            IDbDataAdapter dataAdapter = null;
            switch (connType)
            {
                case "SqlServer":
                    using (SqlCommand cmd = new SqlCommand(cmdText, (SqlConnection)conn))
                    {
                        if (parArray != null && parArray.Length > 0)
                        {
                            cmd.Parameters.AddRange(parArray);
                        }
                        dataAdapter = new SqlDataAdapter(cmd);
                    }
                    break;
                case "MySql":
                    using (MySqlCommand cmd = new MySqlCommand(cmdText, (MySqlConnection)conn))
                    {
                        if (parArray != null && parArray.Length > 0)
                        {
                            cmd.Parameters.AddRange(parArray);
                        }
                        dataAdapter = new MySqlDataAdapter(cmd);
                    }
                    break;
                
            }
            return dataAdapter;
        }

        /// <summary>
        /// 测试一个sql是否合法
        /// </summary>
        /// <param name="dbconn"></param>
        /// <param name="sql"></param>
        /// <returns></returns>
        public bool TestSql(RoadFlow.Data.Model.DBConnection dbconn, string sql)
        {
            if (dbconn == null)
            {
                return false;
            }
            switch (dbconn.Type)
            {
                case "SqlServer":
                    using (SqlConnection conn = new SqlConnection(dbconn.ConnectionString))
                    {
                        try
                        {
                            conn.Open();
                        }
                        catch
                        {
                            return false;
                        }
                        using (SqlCommand cmd = new SqlCommand(sql.ReplaceSelectSql(), conn))
                        {
                            try
                            {
                                cmd.ExecuteNonQuery();
                                return true;
                            }
                            catch
                            {
                                return false;
                            }
                        }
                    }
                    break;
                case "MySql":
                    using (MySqlConnection conn = new MySqlConnection(dbconn.ConnectionString))
                    {
                        try
                        {
                            conn.Open();
                        }
                        catch
                        {
                            return false;
                        }
                        using (MySqlCommand cmd = new MySqlCommand(sql.ReplaceSelectSql(), conn))
                        {
                            try
                            {
                                cmd.ExecuteNonQuery();
                                return true;
                            }
                            catch
                            {
                                return false;
                            }
                        }
                    }

                    break;
            }
            return false;
        }

        /// <summary>
        /// 根据连接实体得到数据表
        /// </summary>
        /// <param name="linkID"></param>
        /// <returns></returns>
        public System.Data.DataTable GetDataTable(RoadFlow.Data.Model.DBConnection dbconn, string sql)
        {
            if (dbconn == null || dbconn.Type.IsNullOrEmpty() || dbconn.ConnectionString.IsNullOrEmpty())
            {
                return null;
            }
            DataTable dt = new DataTable();
            switch (dbconn.Type)
            {

                case "SqlServer":
                    using (SqlConnection conn = new SqlConnection(dbconn.ConnectionString))
                    {
                        try
                        {
                            conn.Open();
                            using (SqlDataAdapter dap = new SqlDataAdapter(sql, conn))
                            {
                                dap.Fill(dt);
                            }
                        }
                        catch (SqlException ex)
                        {
                            Platform.Log.Add(ex);
                        }
                    }
                    break;
                case "MySql":
                    using (MySqlConnection conn = new MySqlConnection(dbconn.ConnectionString))
                    {
                        try
                        {
                            conn.Open();
                            using (MySqlDataAdapter dap = new MySqlDataAdapter(sql, conn))
                            {
                                dap.Fill(dt);
                            }
                        }
                        catch (MySqlException ex)
                        {
                            Platform.Log.Add(ex);
                        }
                    }
                    break;


            }

            return dt;
        }

        /// <summary>
        /// 得到一个表的结构
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="tableName"></param>
        /// <param name="dbType"></param>
        /// <returns></returns>
        public System.Data.DataTable GetTableSchema(System.Data.IDbConnection conn, string tableName, string dbType)
        {
            DataTable dt = new DataTable();
            try
            {
                switch (dbType)
                {
                    case "SqlServer":
                        string sql = string.Format(@"select a.name as f_name,b.name as t_name,[length],a.isnullable as is_null, a.cdefault as cdefault,COLUMNPROPERTY( OBJECT_ID('{0}'),a.name,'IsIdentity') as isidentity from 
                    sys.syscolumns a inner join sys.types b on b.user_type_id=a.xtype 
                    where object_id('{0}')=id order by a.colid", tableName);
                        SqlDataAdapter dap = new SqlDataAdapter(sql, (SqlConnection)conn);
                        dap.Fill(dt);
                        break;
                    case "MySql":
                        string mysql = string.Format("select COLUMN_NAME AS f_name,DaTA_TYPE as t_name,IS_NULLABLE as is_null,COLUMN_DEFAULT as cdefault,COLUMN_KEY AS isidentity from information_schema.columns where table_name='{0}' and TABLE_SCHEMA='{1}'", tableName,conn.Database);
                        MySqlDataAdapter mydap = new MySqlDataAdapter(mysql, (MySqlConnection)conn);
                        mydap.Fill(dt);
                        break;

                }
            }
            catch (Exception ee) { throw ee; }
            return dt;
        }

        /// <summary>
        /// 更新一个连接一个表一个字段的值
        /// </summary>
        /// <param name="connID"></param>
        /// <param name="table"></param>
        /// <param name="field"></param>
        /// <param name="value"></param>
        public void UpdateFieldValue(Guid connID, string table, string field, string value, string where)
        {

            string sql = string.Format("UPDATE {0} SET {1}=@value WHERE {2}", table, field, where);
            SqlParameter[] para = new SqlParameter[]{
                new SqlParameter("@value", value)
            };
            RoadFlow.Platform.ExecuteBLL executeBLL = new ExecuteBLL();
            executeBLL.ExecuteSQL(sql,para);

            //var conn = Get(connID);
            //if (conn == null)
            //{
            //    return;
            //}
            //switch (conn.Type)
            //{
      
            //    case "SqlServer":
            //        using (var dbconn = GetConnection(conn))
            //        {
            //            try
            //            {
            //                dbconn.Open();
            //            }
            //            catch(SqlException ex) 
            //            {
            //                Platform.Log.Add(ex);
            //            }
            //            string sql = string.Format("UPDATE {0} SET {1}=@value WHERE {2}", table, field, where);
            //            SqlParameter par = new SqlParameter("@value", value);
            //            using (SqlCommand cmd = new SqlCommand(sql, (SqlConnection)dbconn))
            //            {
            //                cmd.Parameters.Add(par);
            //                try
            //                {
            //                    cmd.ExecuteNonQuery();
            //                }
            //                catch (SqlException ex)
            //                {
            //                    Platform.Log.Add(ex);
            //                }
            //            }
            //        }
            //        break;
            //    case "MySql":
            //        using (var dbconn = GetConnection(conn))
            //        {
  
            //            try
            //            {
            //                dbconn.Open();
            //            }
            //            catch (Exception ex)
            //            {
            //                Platform.Log.Add(ex);
            //            }
            //            string sql = string.Format("UPDATE {0} SET {1}=@value WHERE {2}", table, field, where);
            //            MySqlParameter par = new MySqlParameter("@value", value);
            //            using (MySqlCommand cmd = new MySqlCommand(sql, (MySqlConnection)dbconn))
            //            {
            //                cmd.Parameters.Add(par);
            //                try
            //                {
            //                    cmd.ExecuteNonQuery();
            //                }
            //                catch (MySqlException ex)
            //                {
            //                    Platform.Log.Add(ex);
            //                }
            //            }
            //        }
            //        break;


            //}
        }

        /// <summary>
        /// 删除一个连接表的数据
        /// </summary>
        /// <param name="connID"></param>
        /// <param name="table"></param>
        /// <param name="pkFiled"></param>
        /// <param name="pkValue"></param>
        public void DeleteData(Guid connID, string table, string pkFiled, string pkValue)
        {
            var conn = Get(connID);
            if (conn == null)
            {
                return;
            }
            switch (conn.Type)
            {
          
                case "SqlServer":
                    using (var dbconn = GetConnection(conn))
                    {
                        try
                        {
                            dbconn.Open();
                        }
                        catch (SqlException ex)
                        {
                            Platform.Log.Add(ex);
                        }
                        string sql = string.Format("DELETE FROM {0} WHERE {1}=@{1}", table, pkFiled);
                        SqlParameter par = new SqlParameter("@" + pkFiled, pkValue);
                        using (SqlCommand cmd = new SqlCommand(sql, (SqlConnection)dbconn))
                        {
                            cmd.Parameters.Add(par);
                            try
                            {
                                cmd.ExecuteNonQuery();
                            }
                            catch (SqlException ex)
                            {
                                Platform.Log.Add(ex);
                            }
                        }
                    }
                    break;
                case "MySql":
                    using (var dbconn = GetConnection(conn))
                    {
                        try
                        {
                            dbconn.Open();
                        }
                        catch (MySqlException ex)
                        {
                            Platform.Log.Add(ex);
                        }
                        string sql = string.Format("DELETE FROM {0} WHERE {1}=@{1}", table, pkFiled);
                        MySqlParameter par = new MySqlParameter("@" + pkFiled, pkValue);
                        using (MySqlCommand cmd = new MySqlCommand(sql, (MySqlConnection)dbconn))
                        {
                            cmd.Parameters.Add(par);
                            try
                            {
                                cmd.ExecuteNonQuery();
                            }
                            catch (MySqlException ex)
                            {
                                Platform.Log.Add(ex);
                            }
                        }
                    }
                    break;
           
              
            }
        }



        ///// <summary>
        ///// 获取字段及其说明和字段类型属性  by hds  2016-3-3 11:23:40 SqlServer
        ///// </summary>
        ///// <param name="table"></param>
        ///// <returns></returns>
        //public List<Column> GetTableSchema(string tableName)
        //{
        //    string sql = string.Format(@"SELECT a.name as Value, b.value Name,d.name colType  from sys.syscolumns a LEFT JOIN sys.extended_properties b on a.id=b.major_id AND a.colid=b.minor_id AND b.name='MS_Description' join sysobjects c  on c.id=a.id and c.xtype='U' join systypes d on a.xtype=d.xusertype  WHERE object_id('{0}')=a.id ORDER BY a.colid", tableName);
        //    SqlDataReader dataReader = dbHelper.GetDataReader(sql);
        //    List<Column> List = new List<Column>();
        //    while (dataReader.Read())
        //    {
        //        List.Add(new Column()
        //        {
        //            Name = dataReader.GetValue(dataReader.GetOrdinal("Name")).ToString(),
        //            Value = dataReader.GetValue(dataReader.GetOrdinal("Value")).ToString(),
        //            ColType = dataReader.GetValue(dataReader.GetOrdinal("ColType")).ToString()
        //        });
        //    }
        //    return List;
        //}
    }
}
