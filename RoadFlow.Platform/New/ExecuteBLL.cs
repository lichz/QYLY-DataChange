using RoadFlow.Data.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using RoadFlow.Platform;
using System.Data.SqlClient;

namespace RoadFlow.Platform
{
    /// <summary>
    /// 执行sql,存储过程等。
    /// </summary>
    public class ExecuteBLL
    {
        private IExecute executeDb = RoadFlow.Data.Factory.Factory.GetExecute();
        #region get
        /// <summary>
        /// 执行sql,查询数据
        /// </summary>
        /// <param name="sql">sql</param>
        /// <returns>查询数据</returns>
        public DataTable GetDataTable(string sql) {
            return executeDb.GetDataTable(sql);
        }

        /// <summary>
        /// 执行sql,查询数据
        /// </summary>
        /// <param name="sql">sql</param>
        /// <param name="para">参数</param>
        /// <returns>查询数据</returns>
        public DataTable GetDataTable(string sql, IDataParameter[] para) {
            return executeDb.GetDataTable(sql, para);
        }

        /// <summary>
        /// 获取表结构
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public DataTable GetTableSchema(string tableName) {
            string sql = string.Format(@"select a.name as f_name,b.name as t_name,[length],a.isnullable as is_null, a.cdefault as cdefault,COLUMNPROPERTY( OBJECT_ID('{0}'),a.name,'IsIdentity') as isidentity from 
                    sys.syscolumns a inner join sys.types b on b.user_type_id=a.xtype 
                    where object_id('{0}')=id order by a.colid", tableName);
            return GetDataTable(sql);
        }

        public IDataAdapter GetDataAdapter(string sql, IDataParameter[] parameter) {
            return executeDb.GetDataAdapter(sql, parameter);
        }

        #endregion

        #region Modify
        public int ExecuteProcedure(string procedure, IDataParameter[] para) {
            return executeDb.ExecuteProcedure(procedure, para);
        }
        public int ExecuteSQL(string sql) {
            return executeDb.ExecuteSQL(sql);
        }

        public int ExecuteSQL(string sql, IDataParameter[] para) {
            return executeDb.ExecuteSQL(sql, para);
        }
        #endregion


    }
}
