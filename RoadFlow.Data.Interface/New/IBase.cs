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

        int DeleteByPara(dynamic para);
    }
}
