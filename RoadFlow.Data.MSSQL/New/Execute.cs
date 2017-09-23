using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using RoadFlow.Data.Interface;

namespace RoadFlow.Data.MSSQL {
    /// <summary>
    /// 直接执行（如sql,存储过程等。）
    /// </summary>
    public class Execute : IExecute {
        private DBHelper dbHelper = new DBHelper();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="procedureName">存储过程名称</param>
        /// <param name="parameter">输入参数</param>
        /// <param name="outParameter">输出参数</param>
        /// <returns></returns>
        public virtual int ExecuteProcedure(string procedureName, IDataParameter[] parameter)
        {
            return dbHelper.ExecuteProcedure(procedureName, parameter);
        }

        /// <summary>
        /// 执行sql（用于操作，比如更新，删除等）
        /// </summary>
        /// <param name="sql">sql</param>
        /// <returns></returns>
        public virtual int ExecuteSQL(string sql) {
            return dbHelper.Execute(sql);
        }

        /// <summary>
        /// 执行sql（用于操作，比如更新，删除等）
        /// </summary>
        /// <param name="sql">sql</param>
        /// <param name="parameter">输入参数</param>
        /// <returns></returns>
        public virtual int ExecuteSQL(string sql, IDataParameter[] parameter) {
            return dbHelper.Execute(sql, parameter);
        }

        /// <summary>
        /// 执行sql,查询数据
        /// </summary>
        /// <param name="sql">sql</param>
        /// <returns>查询数据</returns>
        public virtual DataTable GetDataTable(string sql) {
            return dbHelper.GetDataTable(sql);
        }

        /// <summary>
        /// 执行sql,查询数据
        /// </summary>
        /// <param name="sql">sql</param>
        /// <param name="parameter">输入参数</param>
        /// <returns>查询数据</returns>
        public virtual DataTable GetDataTable(string sql, IDataParameter[] parameter) {
            return dbHelper.GetDataTable(sql, parameter);
        }

        public virtual IDataAdapter GetDataAdapter(string sql, IDataParameter[] parameter) {
            return dbHelper.GetDataAdapter(sql,parameter);
        }
    }
}

