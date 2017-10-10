using RoadFlow.Data.Interface;
using RoadFlow.Data.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace RoadFlow.Data.MSSQL
{
    /// <summary>
    /// 基类
    /// </summary>
    public class Base:IBase {
        private DBHelper dbHelper = new DBHelper();
        private string _tableName;
        private string _modifyTableName;
        private string _order;
        public Base(string tableName,string order) {
            _tableName = tableName;
            _order = order;
        }

        public Base(string tableName,string modifyTableName,string order) {
            _tableName = tableName;
            _modifyTableName = modifyTableName;
            _order = order;
        }

        #region get
        /// <summary>
        /// 查询所有记录(带翻页)
        /// </summary>
        public virtual DataTable GetPagerData(out string pager, int size, int number, Dictionary<KeyValuePair<string, SQLFilterType>, object> where) {
            Dictionary<string, object> dictionary = dbHelper.KeysToWhereString(where);
            SqlParameter[] parameters = dictionary["para"] as SqlParameter[];
            long count = 0;
            string sql = dbHelper.GetPaerSql(_tableName, "*", dictionary["where"].ToString(), _order, size, number, out count, parameters);
            pager = RoadFlow.Utility.New.Tools.GetPagerHtml(count, size, number);  //生成HTML的分页
            return dbHelper.GetDataTable(sql, parameters);
        }


        /// <summary>
        /// 获取数据列表(带翻页)
        /// </summary>
        /// <returns></returns>
        public virtual List<T> GetListPagerData<T>(out string pager, int size, int number, Dictionary<KeyValuePair<string, SQLFilterType>, object> where) {
            Dictionary<string, object> dictionary = dbHelper.KeysToWhereString(where);
            SqlParameter[] parameters = dictionary["para"] as SqlParameter[];
            long count = 0;
            string sql = dbHelper.GetPaerSql(_tableName, "*", dictionary["where"].ToString(), _order, size, number, out count, parameters);
            pager = RoadFlow.Utility.New.Tools.GetPagerHtml(count, size, number);  //生成HTML的分页
            return dbHelper.GetList<T>(sql, parameters);
        }

        /// <summary>
        /// 查询所有记录(top重载，为0则取所有)
        /// </summary>
        public virtual DataTable GetAll(int top,Dictionary<KeyValuePair<string, SQLFilterType>, object> where) {
            Dictionary<string, object> dictionary = dbHelper.KeysToWhereString(where);
            SqlParameter[] para = dictionary["para"] as SqlParameter[];
            string sql = string.Empty;
            if(top!=0){
                sql = "SELECT top " + top + " * FROM " + _tableName + " where 1=1 " + dictionary["where"].ToString() + " order by " + _order;
            } else {
                sql = "SELECT * FROM " + _tableName + " where 1=1 " + dictionary["where"].ToString() + " order by " + _order;
            }
            return dbHelper.GetDataTable(sql, para);
        }

   
        /// <summary>
        /// 查询所有记录(和getall参数方式不同，建议基于“=”的用此方法。)
        /// </summary>
        /// <param name="top">前边几条</param>
        /// <param name="para">参数数组</param>
        /// <returns></returns>
        public virtual DataTable GetAllByPara(int top, params KeyValuePair<string, object>[] para) {
            //设置sql参数列表，拼接where语句。
            StringBuilder where = new StringBuilder();
            List<SqlParameter> parameter = new List<SqlParameter>();
            foreach (KeyValuePair<string, object> item in para) {
                where.Append(" And [").Append(item.Key).Append("]=@").Append(item.Key);
                parameter.Add(new SqlParameter("@" + item.Key, item.Value));
            }
            //拼接sql
            string sql = string.Empty;
            if (top != 0) {
                sql = "SELECT top "+top+" * FROM " + _tableName + " where 1=1 " + where.ToString() + " order by " + _order;
            } else {
                sql = "SELECT * FROM " + _tableName + " where 1=1 " + where.ToString() + " order by " + _order;
            }
            return dbHelper.GetDataTable(sql, parameter.ToArray());
        }

        /// <summary>
        /// 根据条件查询一条记录
        /// </summary>
        public virtual T Get<T>(params KeyValuePair<string, object>[] para) {
            return dbHelper.GetModel<T>(_tableName,_order,para);
        }
        #endregion

        /// <summary>
        /// 添加记录
        /// </summary>
        /// <returns>操作所影响的行数</returns>
        public virtual int Add<T>(T o) {
            return dbHelper.AddModel<T>(o, string.IsNullOrWhiteSpace(_modifyTableName)?_tableName:_modifyTableName);
        }

        /// <summary>
        /// 更新记录
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model"></param>
        /// <param name="para">筛选条件（一般是主键）</param>
        /// <returns></returns>
        public virtual int Update<T>(T model, params KeyValuePair<string, object>[] para) {
            Dictionary<string, SqlParameter> where = new Dictionary<string, SqlParameter>();
            StringBuilder sql = new StringBuilder();
            foreach (KeyValuePair<string, object> item in para) {
                sql.Clear();
                sql.Append(" And [").Append(item.Key).Append("]=@").Append(item.Key);
                where.Add(sql.ToString(), new SqlParameter("@"+item.Key, item.Value));
            }
            return dbHelper.UpdateModel<T>(model, where, string.IsNullOrWhiteSpace(_modifyTableName) ? _tableName : _modifyTableName);
        }

        public virtual int Delete(object id)
        {
            string table = string.IsNullOrEmpty(_modifyTableName) ? _tableName : _modifyTableName;
            string sql = "update [" + table + "] set [Status]=" + (int)Status.Deleted + " where [ID]=@DeleteID";
            return dbHelper.Execute(sql, new SqlParameter[] { new SqlParameter("@DeleteID", id) });
        }


        #region new 区别主要在参数传递上。将Dictionary<KeyValuePair<string, Data.Model.SQLFilterType>, object> 改为 List<Predicates>,将 params KeyValuePair<string, object>[]改为object或dynamic
        /// <summary>
        /// 查询所有记录(带翻页)
        /// </summary>
        public virtual Result<DataTable> QueryPaging(out string pager, int size, int number, List<Predicates> predicates, bool isAutoStatus = true)
        {
            Result<DataTable> result = new Result<DataTable>();
            long count = 0;
            pager = string.Empty;

            Dictionary<string, object> dictionary = dbHelper.PredicatesToWhereString(predicates, isAutoStatus);
            SqlParameter[] parameters = dictionary["para"] as SqlParameter[];
            try
            {
                string sql = dbHelper.GetPaerSql(_tableName, "*", dictionary["where"].ToString(), _order, size, number, out count, parameters);
                pager = RoadFlow.Utility.New.Tools.GetPagerHtml(count, size, number);  //生成HTML的分页
                result.Data = dbHelper.GetDataTable(sql, parameters);
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.ErrMSG = ex.Message;
                result.Success = false;
            }

            return result;
        }

        /// <summary>
        /// 查询列表(带翻页)
        /// </summary>
        public virtual Result<List<T>> QueryListPaging<T>(out string pager, int size, int number, List<Predicates> predicates, bool isAutoStatus = true)
        {
            Result<List<T>> result = new Result<List<T>>();
            long count = 0;
            pager = string.Empty;

            Dictionary<string, object> dictionary = dbHelper.PredicatesToWhereString(predicates, isAutoStatus);
            SqlParameter[] parameters = dictionary["para"] as SqlParameter[];
            try
            {
                string sql = dbHelper.GetPaerSql(_tableName, "*", dictionary["where"].ToString(), _order, size, number, out count, parameters);
                pager = RoadFlow.Utility.New.Tools.GetPagerHtml(count, size, number);  //生成HTML的分页
                result.Data = dbHelper.GetList<T>(sql, parameters);
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.ErrMSG = ex.Message;
                result.Success = false;
            }

            return result;
        }

        /// <summary>
        /// 查询所有记录(top重载，为0则取所有)
        /// </summary>
        /// <param name="top"></param>
        /// <param name="predicates"></param>
        /// <param name="isAutoStatus">是否自动增加状态筛选</param>
        /// <returns></returns>
        public virtual Result<DataTable> QueryAll(int top, List<Predicates> predicates, bool isAutoStatus = true)
        {
            Result<DataTable> result = new Result<DataTable>();

            Dictionary<string, object> dictionary = dbHelper.PredicatesToWhereString(predicates, isAutoStatus);
            SqlParameter[] parameters = dictionary["para"] as SqlParameter[];
            try
            {
                string sql = string.Empty;
                if (top != 0)
                {
                    sql = "SELECT top " + top + " * FROM " + _tableName + " where 1=1 " + dictionary["where"].ToString() + " order by " + _order;
                }
                else
                {
                    sql = "SELECT * FROM " + _tableName + " where 1=1 " + dictionary["where"].ToString() + " order by " + _order;
                }
                result.Data = dbHelper.GetDataTable(sql, parameters);
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.ErrMSG = ex.Message;
                result.Success = false;
            }

            return result;

        }

        /// <summary>
        /// 查询所有记录(和getall参数方式不同，建议基于“=”的用此方法。)
        /// </summary>
        /// <param name="top">前边几条</param>
        /// <param name="para">参数数组</param>
        /// <returns></returns>
        public virtual Result<DataTable> QueryByPara(int top, dynamic para, bool isAutoStatus = true)
        {
            Result<DataTable> result = new Result<DataTable>();

            //设置sql参数列表，拼接where语句。
            Dictionary<string, object> dictionary = dbHelper.DynamicToWhereString(para, isAutoStatus);
            SqlParameter[] parameters = dictionary["para"] as SqlParameter[];

            //拼接sql
            string sql = string.Empty;
            if (top != 0)
            {
                sql = "SELECT top " + top + " * FROM " + _tableName + " where 1=1 " + dictionary["where"].ToString() + " order by " + _order;
            }
            else
            {
                sql = "SELECT * FROM " + _tableName + " where 1=1 " + dictionary["where"].ToString() + " order by " + _order;
            }
            try
            {
                result.Data = dbHelper.GetDataTable(sql, parameters);
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.ErrMSG = ex.Message;
                result.Success = false;
            }

            return result;
        }

        /// <summary>
        /// 根据条件查询一条记录
        /// </summary>
        public virtual Result<T> Query<T>(dynamic para)
        {
            Result<T> result = new Result<T>();
            try
            {
                result.Data = dbHelper.GetByPara<T>(_tableName,_order, para);
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.ErrMSG = ex.Message;
                result.Success = false;
            }
            return result;
        }

        public virtual Result<int> UpdateByPara<T>(T model,dynamic para)
        {
            Result<int> result = new Result<int>();
       
            try
            {
                result.Data = dbHelper.UpdateByPara(model,_tableName, para);
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.ErrMSG = ex.Message;
                result.Success = false;
            }
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="para">筛选条件</param>
        /// <returns></returns>
        public virtual int DeleteByPara(dynamic para)
        {
            var where = dbHelper.DynamicToWhereString(para, false);
            string table = string.IsNullOrEmpty(_modifyTableName) ? _tableName : _modifyTableName;
            string sql = "update [" + table + "] set [Status]=" + (int)Status.Deleted + " where 1=1 " + where["where"];
            return dbHelper.Execute(sql, where["para"]);
        }
        #endregion

    }
}