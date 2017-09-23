using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace RoadFlow.Data.Interface {
    /// <summary>
    /// 存储过程
    /// </summary>
    public interface IExecute {
        int ExecuteProcedure(string procedureName, IDataParameter[] parameter);

        int ExecuteSQL(string sql);

        int ExecuteSQL(string sql, IDataParameter[] parameter);
        
        DataTable GetDataTable(string sql);

        DataTable GetDataTable(string sql, IDataParameter[] parameter);

        IDataAdapter GetDataAdapter(string sql, IDataParameter[] parameter);
    }
}
