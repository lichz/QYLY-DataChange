using RoadFlow.Data.Model;
using System;
using System.Collections.Generic;
using System.Data;

namespace RoadFlow.Data.Interface {
    /// <summary>
    /// 基础接口
    /// </summary>
    public interface IBase {
        #region get
        DataTable GetPagerData(out string pager, int size, int number, Dictionary<KeyValuePair<string, RoadFlow.Data.Model.SQLFilterType>, object> where);
        List<T> GetListPagerData<T>(out string pager, int size, int number, Dictionary<KeyValuePair<string, RoadFlow.Data.Model.SQLFilterType>, object> where);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="top">取前几条，为0则取所有</param>
        /// <param name="where">筛选条件</param>
        /// <returns></returns>
        DataTable GetAll(int top, Dictionary<KeyValuePair<string, RoadFlow.Data.Model.SQLFilterType>, object> where);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="top">取前几条，为0则取所有</param>
        /// <param name="para"></param>
        /// <returns></returns>
        DataTable GetAllByPara(int top,params KeyValuePair<string, object>[] para);
        T Get<T>(params KeyValuePair<string, object>[] para);
        #endregion

        int Add<T>(T o);
        int Update<T>(T model, params KeyValuePair<string, object>[] para);

        int Delete(object id);

        #region new 区别主要在参数传递上。将Dictionary<KeyValuePair<string, Data.Model.SQLFilterType>, object> 改为 List<Predicates>,将 params KeyValuePair<string, object>[]改为object或dynamic
        /// <summary>
        /// 查询所有记录(带翻页)
        /// </summary>
        Result<DataTable> QueryPaging(out string pager, int size, int number, List<Predicates> predicates, bool isAutoStatus = true);
        /// <summary>
        /// 查询列表(带翻页)
        /// </summary>
        Result<List<T>> QueryListPaging<T>(out string pager, int size, int number, List<Predicates> predicates, bool isAutoStatus = true);
        /// <summary>
        /// 查询所有记录(top重载，为0则取所有)
        /// </summary>
        /// <param name="top"></param>
        /// <param name="predicates"></param>
        /// <param name="isAutoStatus">是否自动增加状态筛选</param>
        /// <returns></returns>
        Result<DataTable> QueryAll(int top, List<Predicates> predicates, bool isAutoStatus = true);
        /// <summary>
        /// 查询所有记录(和getall参数方式不同，建议基于“=”的用此方法。)
        /// </summary>
        /// <param name="top">前边几条,0表示所有</param>
        /// <param name="para">参数数组</param>
        /// <returns></returns>
        Result<DataTable> QueryByPara(int top, dynamic para, bool isAutoStatus = true);

        /// <summary>
        /// 根据条件查询一条记录
        /// </summary>
        Result<T> Query<T>(dynamic para);

        /// <summary>
        /// 更新 【注意：model中ID带有值，会尝试将ID更新。所以传入model前注意赋值为空】
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model"></param>
        /// <param name="para">筛选条件</param>
        /// <returns></returns>
        Result<int> UpdateByPara<T>(T model, dynamic para);


        int DeleteByPara(dynamic para);
        #endregion

    }
}
