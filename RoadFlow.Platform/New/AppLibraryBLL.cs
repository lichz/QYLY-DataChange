using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using RoadFlow.Data.Interface;
using RoadFlow.Data.Factory;

namespace RoadFlow.Platform
{
    public class AppLibraryBLL
    {
        //private string cacheKey = RoadFlow.Utility.Keys.CacheKeys.AppLibrary.ToString();
        //private RoadFlow.Data.Interface.IAppLibrary dataAppLibrary;

        private static string _tableName = "AppLibrary";
        private static string _order = "[ID]";

        private IBase BaseDb = Factory.GetBase(_tableName,_order);

        //public AppLibrary()
        //{
        //    this.dataAppLibrary = Data.Factory.Factory.GetAppLibrary();
        //}
        /// <summary>
        /// 新增
        /// </summary>
        public int Add(RoadFlow.Data.Model.AppLibraryModel model)
        {
            //return dataAppLibrary.Add(model);
            return BaseDb.Add(model);
        }
        /// <summary>
        /// 更新
        /// </summary>
        public int Update(RoadFlow.Data.Model.AppLibraryModel model)
        {
            var id = model.ID.Value;
            model.ID = null;//model中ID带有值，会尝试将ID更新。所以这里赋值为空
            var result = BaseDb.UpdateByPara(model, new { id });
            if (result.Success)
            {
                return result.Data;
            }

            return 0;
        }

        /// <summary>
        /// 查询单条记录
        /// </summary>
        public RoadFlow.Data.Model.AppLibraryModel Get(Guid id, bool fromCache=false)
        {
            return BaseDb.Get<RoadFlow.Data.Model.AppLibraryModel>(new KeyValuePair<string, object>("ID", id));
        }
        /// <summary>
        /// 清除缓存
        /// </summary>
        //public void ClearCache()
        //{
        //    RoadFlow.Cache.IO.Opation.Remove(cacheKey);
        //}

        /// <summary>
        /// 删除
        /// </summary>
        public int Delete(Guid id)
        {
            //return dataAppLibrary.Delete(id);
            return BaseDb.DeleteByPara(new { id });
        }
        

        /// <summary>
        /// 得到一页数据
        /// </summary>
        /// <param name="pager"></param>
        /// <param name="query"></param>
        /// <param name="order"></param>
        /// <param name="size"></param>
        /// <param name="numbe"></param>
        /// <param name="title"></param>
        /// <param name="type"></param>
        /// <param name="address"></param>
        /// <returns></returns>
        public List<RoadFlow.Data.Model.AppLibraryModel> GetPagerData(out string pager, string query = "", string title = "", string type = "", string address = "")
        {
            //return dataAppLibrary.GetPagerData(out pager, query, "Type,Title", RoadFlow.Utility.Tools.GetPageSize(),
            //    RoadFlow.Utility.Tools.GetPageNumber(), title, type, address);

            List<string> typeList = type.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).ToList();

            var where = new List<Data.Model.Predicates>();

            #region 筛选条件
            if (!title.IsNullOrEmpty())
            {
                where.Add(new Data.Model.Predicates()
                {
                    FieldName = "Title",
                    Operator = Data.Model.SQLFilterType.CHARINDEX,
                    Value = title
                });
            }

            if (!address.IsNullOrEmpty())
            {
                where.Add(new Data.Model.Predicates()
                {
                    FieldName = "Address",
                    Operator = Data.Model.SQLFilterType.CHARINDEX,
                    Value = address
                });
            }

            if (typeList.Count > 0)
            {
                where.Add(new Data.Model.Predicates()
                {
                    FieldName = "Type",
                    Operator = Data.Model.SQLFilterType.IN,
                    Value = typeList
                });
            }

            #endregion

            var result = BaseDb.QueryListPaging<RoadFlow.Data.Model.AppLibraryModel>(out pager, RoadFlow.Utility.Tools.GetPageSize(), RoadFlow.Utility.Tools.GetPageNumber(), where);
            if (result.Success)
            {
                return result.Data;
            }
            return new List<Data.Model.AppLibraryModel>();
        }
        /// <summary>
        /// 查询一个类别下所有记录
        /// </summary>
        public List<RoadFlow.Data.Model.AppLibraryModel> GetAllByType(Guid type)
        {
            if (type.IsEmptyGuid())
            {
                return new List<RoadFlow.Data.Model.AppLibraryModel>();
            }
            //return dataAppLibrary.GetAllByType(GetAllChildsIDString(type)).OrderBy(p=>p.Title).ToList();
            var result = BaseDb.QueryByPara(0, new { type });
            if (result.Success)
            {
                return result.Data.ToList<RoadFlow.Data.Model.AppLibraryModel>();
            }
            return new List<Data.Model.AppLibraryModel>();
        }

        /// <summary>
        /// 删除记录
        /// </summary>
        public int Delete(string[] idArray)
        {
            //return dataAppLibrary.Delete(idArray);
            int count = 0;
            foreach (var item in idArray)
            {
                count += BaseDb.DeleteByPara(new { id=item });
            }
            return count;
        }
        /// <summary>
        /// 删除记录
        /// </summary>
        public int Delete(string idstring)
        {
            return idstring.IsNullOrEmpty() ? 0 : BaseDb.DeleteByPara(new { id = idstring });
        }
        /// <summary>
        /// 得到类型选择项
        /// </summary>
        /// <returns></returns>
        public string GetTypeOptions(string value="")
        {
            return new Dictionary().GetOptionsByCode("AppLibraryTypes", Dictionary.OptionValueField.ID, value);
        }

        /// <summary>
        /// 得到下级ID字符串
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string GetAllChildsIDString(Guid id, bool isSelf = true)
        {
            return new Dictionary().GetAllChildsIDString(id, true);
        }

        /// <summary>
        /// 得到一个类型选择项
        /// </summary>
        /// <param name="type">程序类型</param>
        /// <param name="value"></param>
        /// <returns></returns>
        public string GetAppsOptions(Guid type, string value = "")
        {
            if (type.IsEmptyGuid()) return "";
            var apps = GetAllByType(type);
            StringBuilder options = new StringBuilder();
            foreach (var app in apps)
            {
                options.AppendFormat("<option value=\"{0}\" {1}>{2}</option>", app.ID, 
                    string.Compare(app.ID.ToString(), value, true) == 0 ? "selected=\"selected\"" : "",
                    app.Title
                    );
            }
            return options.ToString();
        }
        /// <summary>
        /// 根据ID得到类型
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string GetTypeByID(Guid id)
        {
            var app = Get(id);
            return app == null ? "" : app.Type.ToString();
        }

        /// <summary>
        /// 根据代码查询一条记录
        /// </summary>
        public RoadFlow.Data.Model.AppLibraryModel GetByCode(string code)
        {
            if (code.IsNullOrEmpty())
            {
                return null;
            }

            var result = BaseDb.Query<RoadFlow.Data.Model.AppLibraryModel>(new { code = code.Trim() });
            if (result.Success)
            {
                return result.Data;
            }

            return null;
        }

        /// <summary>
        /// 得到流程运行时地址
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public string GetFlowRunAddress(RoadFlow.Data.Model.AppLibraryModel app, string query="")
        {
            StringBuilder sb = new StringBuilder();
            if (app.Params.IsNullOrEmpty())
            {
                if (!app.Address.Contains("?"))
                {
                    sb.Append(app.Address);
                    sb.Append("?1=1");
                }
            }
            else
            {
            
                if (app.Address.Contains("?"))
                {
                    sb.Append(app.Address);
                    sb.Append("&");
                    sb.Append(app.Params.TrimStart('?').TrimStart('&'));
                }
                else
                {
                    sb.Append(app.Address);
                    sb.Append("?");
                    sb.Append(app.Params.TrimStart('?').TrimStart('&'));
                }
            }
            if (!query.IsNullOrEmpty())
            {
                sb.Append("&");
                sb.Append(query.TrimStart('?').TrimStart('&'));
            }

            return sb.ToString();
            
        }

        /// <summary>
        /// 更新应用程序库使用人员缓存
        /// </summary>
        /// <param name="appid"></param>
        /// <param name="userIdString"></param>
        public List<Guid> UpdateUseMemberCache(Guid appid)
        {
            string key = RoadFlow.Utility.Keys.CacheKeys.AppLibraryUseMember.ToString();
            //var obj = RoadFlow.Cache.IO.Opation.Get(key);
            Dictionary<Guid, List<Guid>> dict = new Dictionary<Guid, List<Guid>>();
            //if (obj != null && obj is Dictionary<Guid, List<Guid>>)
            //{
            //    dict = obj as Dictionary<Guid, List<Guid>>;
            //}
            //else
            //{
            //    dict = new Dictionary<Guid, List<Guid>>();
            //}
            var app = Get(appid);
            if (app == null)
            {
                return new List<Guid>();
            }
            if(!app.UseMember.IsNullOrEmpty())
            {
                var userIDs = new Organize().GetAllUsersIdList(app.UseMember);
                dict.Add(appid, userIDs);
                return userIDs;
            }
            //if (dict.ContainsKey(appid))
            //{
            //    if (app.UseMember.IsNullOrEmpty())
            //    {
            //        dict.Remove(appid);
            //        return new List<Guid>();
            //    }
            //    else
            //    {
            //        var userIDs = new Organize().GetAllUsersIdList(app.UseMember);
            //        dict[appid] = userIDs;
            //        return userIDs;
            //    }
            //}
            //else if(!app.UseMember.IsNullOrEmpty())
            //{
            //    var userIDs = new Organize().GetAllUsersIdList(app.UseMember);
            //    dict.Add(appid, userIDs);
            //    return userIDs;
            //}
            return new List<Guid>();
        }
        /// <summary>
        /// 得到一个应用程序库的使用人员
        /// </summary>
        /// <param name="appid"></param>
        /// <returns></returns>
        public List<Guid> GetUseMemberCache(Guid appid)
        {
            string key = RoadFlow.Utility.Keys.CacheKeys.AppLibraryUseMember.ToString();
            var obj = RoadFlow.Cache.IO.Opation.Get(key);
            if (obj != null && obj is Dictionary<Guid, List<Guid>>)
            {
                var dict = obj as Dictionary<Guid, List<Guid>>;
                if (dict.ContainsKey(appid))
                {
                    return dict[appid];
                }
            }
            var app = new AppLibraryBLL().Get(appid);
            if (app == null || app.UseMember.IsNullOrEmpty())
            {
                return new List<Guid>();
            }
            return UpdateUseMemberCache(appid);
        }
        /// <summary>
        /// 清除应用程序库的使用人员缓存
        /// </summary>
        public void ClearUseMemberCache()
        {
            string key = RoadFlow.Utility.Keys.CacheKeys.AppLibraryUseMember.ToString();
            Cache.IO.Opation.Remove(key);
        }
    }
}
