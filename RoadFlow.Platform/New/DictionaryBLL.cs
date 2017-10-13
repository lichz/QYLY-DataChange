using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RoadFlow.Data.Interface;
using RoadFlow.Platform;
using RoadFlow.Data.Model;

namespace RoadFlow.Platform
{
    /// <summary>
    /// 字典
    /// </summary>
    public class DictionaryBLL {
        private static string _tableName = "Dictionary";
        private static string _order = "[Sort]";
        IBase BaseDb = RoadFlow.Data.Factory.Factory.GetBase(_tableName, _order);

        #region get
        /// <summary>
        /// 获取当前用户对应组织名称，然后用组织名称查询字典中对应的ssjd的ID.（仅用于街道）
        /// </summary>
        /// <returns></returns>
        public Guid? GetCurrentSSJD() {
            var relation = new RoadFlow.Platform.OrganizeBLL().GetByID(RoadFlow.Platform.UsersBLL.CurrentFirstRelationID);
            var dictionary = new DictionaryBLL().GetByName(relation.Name.Replace("街道", ""));
            return dictionary == null ? Guid.NewGuid() : dictionary.ID;
        }

        public DictionaryModel GetByID(Guid id) {
            return BaseDb.Get<RoadFlow.Data.Model.DictionaryModel>(new KeyValuePair<string, object>("ID", id));
        }

        public DictionaryModel GetByName(string name) {
            return BaseDb.Get<RoadFlow.Data.Model.DictionaryModel>( new KeyValuePair<string, object>("Title", name));
        }

        /// <summary>
        /// 按父节点code获取子节点列表
        /// </summary>
        /// <param name="code">父节点code</param>
        /// <returns></returns>
        public DataTable GetListByCode(string code) {
            DataTable parent = BaseDb.GetAllByPara(0, new KeyValuePair<string, object>("Code", code));
            if(parent.Rows.Count>0){
                return BaseDb.GetAllByPara(0,new KeyValuePair<string,object>("ParentID",parent.Rows[0]["ID"]));
            } else {
                return new DataTable();
            }
        }

        public List<RoadFlow.Data.Model.DictionaryModel> GetListAll() {
            return BaseDb.GetAll(0, new Dictionary<KeyValuePair<string, RoadFlow.Data.Model.SQLFilterType>, object>()).ToList<RoadFlow.Data.Model.DictionaryModel>();
        }
        #endregion


        #region old迁移
        //public Dictionary()
        //{
        //    this.dataDictionary = Data.Factory.Factory.GetDictionary();
        //}
        //public DataTable GetByID(Guid id)
        //{
        //    //return dataDictionary.GetByID(id);
        //    var result = BaseDb.QueryByPara(new { id });
        //    if (result.Success)
        //    {
        //        return result.Data;
        //    }
        //    return new DataTable();
        //}
        /// <summary>
        /// 获取所有文章类型
        /// </summary>
        /// <returns></returns>
        public List<RoadFlow.Data.Model.DictionaryModel> GetArticleType()
        {
            //return dataDictionary.GetArticleType();
            //var result = BaseDb.QueryAll(new List<Data.Model.Predicates>()
            //{
            //    new Data.Model.Predicates()
            //    {
            //        FieldName = "Code",
            //         Operator = Data.Model.SQLFilterType.IN,
            //          Value = new List<string>()
            //          {
            //              "BuildingInformation","QYFeatures"
            //          }
            //    }
            //});
            //if (result.Success)
            //{
            //    return result.Data;
            //}
            //return new DataTable();
            var articleRoot = BaseDb.Query<RoadFlow.Data.Model.DictionaryModel>(new { code= "ArticleType" });//文章类型根节点
            if (articleRoot.Success)
            {
                var typeList = GetAllChilds(articleRoot.Data.ID.Value);
                //移除两个类型分类
                typeList.RemoveAll(p => p.Code == "BuildingInformation" || p.Code == "QYFeatures");
                return typeList;
            }
            

            return new List<DictionaryModel>();
        }
        /// <summary>
        /// 新增
        /// </summary>
        public int Add(RoadFlow.Data.Model.DictionaryModel model)
        {
            //return dataDictionary.Add(model);
            return BaseDb.Add(model);
        }
        /// <summary>
        /// 更新
        /// </summary>
        public int Update(RoadFlow.Data.Model.DictionaryModel model)
        {
            //return dataDictionary.Update(model);
            var id = model.ID;
            model.ID = null;
            var result = BaseDb.UpdateByPara(model, new { id });
            if (result.Success)
            {
                return result.Data;
            }
            return 0;
        }
        /// <summary>
        /// 查询所有记录
        /// </summary>
        public List<RoadFlow.Data.Model.DictionaryModel> GetAll()
        {
            //if (!fromCache)
            //{
            //    return dataDictionary.GetAll();
            //}
            //else
            //{
            //    object obj = RoadFlow.Cache.IO.Opation.Get(cacheKey);
            //    if (obj != null && obj is List<RoadFlow.Data.Model.Dictionary>)
            //    {
            //        return obj as List<RoadFlow.Data.Model.Dictionary>;
            //    }
            //    else
            //    {
            //        var list = dataDictionary.GetAll();
            //        RoadFlow.Cache.IO.Opation.Set(cacheKey, list);
            //        return list;
            //    }
            //}
            var result = BaseDb.QueryAll(new List<Data.Model.Predicates>());
            if (result.Success)
            {
                return result.Data.ToList<RoadFlow.Data.Model.DictionaryModel>();
            }
            return new List<Data.Model.DictionaryModel>();
        }
        /// <summary>
        /// 查询单条记录
        /// </summary>
        public RoadFlow.Data.Model.DictionaryModel Get(Guid id)
        {
            //return fromCache ? GetAll(true).Find(p => p.ID == id) : dataDictionary.Get(id);
            var result = BaseDb.Query<RoadFlow.Data.Model.DictionaryModel>(new { id });
            if (result.Success)
            {
                return result.Data;
            }
            return null;
        }
        /// <summary>
        /// 删除
        /// </summary>
        public int Delete(Guid id)
        {
            //return dataDictionary.Delete(id);
            return BaseDb.DeleteByPara(new { id });
        }


        /// <summary>
        /// 查询根记录
        /// </summary>
        public RoadFlow.Data.Model.DictionaryModel GetRoot()
        {
            //return dataDictionary.GetRoot();
            var result = BaseDb.Query<RoadFlow.Data.Model.DictionaryModel>(new { ParentId = Guid.Empty });
            if (result.Success)
            {
                return result.Data;
            }
            return null;
        }

        /// <summary>
        /// 查询下级记录
        /// </summary>
        public List<RoadFlow.Data.Model.DictionaryModel> GetChilds(Guid parentID)
        {
            //return fromCache ? getChildsByIDFromCache(id) : dataDictionary.GetChilds(id);
            var result = BaseDb.QueryByPara(new { parentID });
            if (result.Success)
            {
                return result.Data.ToList<RoadFlow.Data.Model.DictionaryModel>();
            }
            return new List<Data.Model.DictionaryModel>();
        }

        /// <summary>
        /// 查询下级记录
        /// </summary>
        public List<RoadFlow.Data.Model.DictionaryModel> GetChilds(string code)
        {
            //return code.IsNullOrEmpty() ? new List<RoadFlow.Data.Model.Dictionary>() :
            //  fromCache ? getChildsByCodeFromCache(code) :
            //    dataDictionary.GetChilds(code.Trim());
            if (!code.IsNullOrEmpty())
            {
                var parent = BaseDb.Query<RoadFlow.Data.Model.DictionaryModel>(new { code = code.Trim() });
                if (parent.Success)
                {
                    return GetChilds(parent.Data.ID.Value);
                }
            }

            return new List<Data.Model.DictionaryModel>();

        }

        //private List<RoadFlow.Data.Model.Dictionary> getChildsByIDFromCache(Guid id)
        //{
        //    var list = GetAll(true);
        //    return list.FindAll(p => p.ParentID == id).OrderBy(p=>p.Sort).ToList();
        //}

        /// <summary>
        /// 得到所有下级
        /// </summary>
        /// <param name="code"></param>
        /// <param name="fromCache">是否使用缓存</param>
        /// <returns></returns>
        public List<RoadFlow.Data.Model.DictionaryModel> GetAllChilds(string code)
        {
            //if (code.IsNullOrEmpty()) return new List<RoadFlow.Data.Model.Dictionary>();
            //var dict = GetByCode(code, fromCache);
            //if (dict == null) return new List<RoadFlow.Data.Model.Dictionary>();
            //return GetAllChilds(dict.ID, fromCache);
            var parent = GetByCode(code);
            return GetAllChilds(parent.ID.Value);
        }

        /// <summary>
        /// 得到所有下级
        /// </summary>
        /// <param name="id"></param>
        /// <param name="fromCache">是否使用缓存</param>
        /// <returns></returns>
        public List<RoadFlow.Data.Model.DictionaryModel> GetAllChilds(Guid id)
        {
            List<RoadFlow.Data.Model.DictionaryModel> list = new List<RoadFlow.Data.Model.DictionaryModel>();
            addChilds(list, id);
            return list;
        }

        private void addChilds(List<RoadFlow.Data.Model.DictionaryModel> list, Guid id)
        {
            //var childs = fromCache ? getChildsByIDFromCache(id) : GetChilds(id);
            var childs = GetChilds(id);
            foreach (var child in childs)
            {
                list.Add(child);
                addChilds(list, child.ID.Value);
            }
        }

        /// <summary>
        /// 得到一个项的所有下级项ID字符串
        /// </summary>
        /// <param name="id"></param>
        /// <param name="isSelf">是否包含自己</param>
        /// <returns></returns>
        public string GetAllChildsIDString(Guid id, bool isSelf = true)
        {
            StringBuilder sb = new StringBuilder();
            if (isSelf)
            {
                sb.Append(id);
                sb.Append(",");
            }
            var childs = GetAllChilds(id);
            foreach (var child in childs)
            {
                sb.Append(child.ID);
                sb.Append(",");
            }
            return sb.ToString().TrimEnd(',');
        }

        /// <summary>
        /// 是否包含下级记录
        /// </summary>
        public bool HasChilds(Guid id)
        {
            //return dataDictionary.HasChilds(id);
            return GetAllChilds(id).Count > 0;
        }

        /// <summary>
        /// 得到最大排序
        /// </summary>
        public int GetMaxSort(Guid id)
        {
            //return dataDictionary.GetMaxSort(id);
            var childs = GetAllChilds(id);
            if (childs.Count > 0)
            {
                return childs.Last().Sort.Value + 1;
            }
            return 1;
        }

        /// <summary>
        /// 更新排序
        /// </summary>
        public int UpdateSort(Guid id, int sort)
        {
            //return dataDictionary.UpdateSort(id, sort);
            return Update(new Data.Model.DictionaryModel() { Sort = sort, ID = id });
        }

        /// <summary>
        /// 根据代码查询一条记录
        /// </summary>
        /// <param name="code"></param>
        /// <param name="fromCache">是否使用缓存</param>
        /// <returns></returns>
        public RoadFlow.Data.Model.DictionaryModel GetByCode(string code)
        {
            if (!code.IsNullOrEmpty())
            {
                var result = BaseDb.Query<RoadFlow.Data.Model.DictionaryModel>(new { code = code.Trim() });
                if (result.Success)
                {
                    return result.Data;
                }
            }
            return null;
            //return code.IsNullOrEmpty() ? null :
            //    fromCache ? GetAll(true).Find(p => string.Compare(p.Code, code, true) == 0) : dataDictionary.GetByCode(code.Trim());

        }

        /// <summary>
        /// 下拉选项时以哪个字段作为值字段
        /// </summary>
        public enum OptionValueField
        {
            ID,
            Title,
            Code,
            Value,
            Other,
            Note
        }

        /// <summary>
        /// 根据ID得到选项
        /// </summary>
        /// <param name="id"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public string GetOptionsByID(Guid id, OptionValueField valueField = OptionValueField.Value, string value = "")
        {
            var childs = GetAllChilds(id);
            StringBuilder options = new StringBuilder(childs.Count * 100);
            StringBuilder space = new StringBuilder();
            foreach (var child in childs)
            {
                space.Clear();
                int parentCount = getParentCount(childs, child);

                for (int i = 0; i < parentCount - 1; i++)
                {
                    space.Append("&nbsp;&nbsp;");
                }

                if (parentCount > 0)
                {
                    space.Append("┝");
                }
                string value1 = getOptionsValue(valueField, child);
                options.AppendFormat("<option value=\"{0}\"{1}>{2}{3}</option>" + parentCount, value1, value1 == value ? " selected=\"selected\"" : "", space.ToString(), child.Title);
            }
            return options.ToString();
        }



        /// <summary>
        /// 得到一个字典项的上级节点数
        /// </summary>
        /// <param name="dictList"></param>
        /// <param name="dict"></param>
        /// <returns></returns>
        private int getParentCount(List<RoadFlow.Data.Model.DictionaryModel> dictList, RoadFlow.Data.Model.DictionaryModel dict)
        {
            int parent = 0;
            RoadFlow.Data.Model.DictionaryModel parentDict = dictList.Find(p => p.ID == dict.ParentID);
            while (parentDict != null)
            {
                parentDict = dictList.Find(p => p.ID == parentDict.ParentID);
                parent++;
            }
            return parent;
        }

        /// <summary>
        /// 根据代码得到选项
        /// </summary>
        /// <param name="id"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public string GetOptionsByCode(string code, OptionValueField valueField = OptionValueField.Value, string value = "")
        {
            return GetOptionsByID(GetIDByCode(code), valueField, value);
        }


        //private string getRadios(List<RoadFlow.Data.Model.Dictionary> childs, string name, OptionValueField valueField = OptionValueField.Value, string value = "", string attr = "")
        //{
        //    StringBuilder options = new StringBuilder(childs.Count * 100);
        //    foreach (var child in childs)
        //    {
        //        string value1 = getOptionsValue(valueField, child);
        //        options.Append("<input type=\"radio\" style=\"vertical-align:middle;\" ");
        //        options.AppendFormat("id=\"{0}_{1}\" ", name, child.ID.ToString("N"));
        //        options.AppendFormat("name=\"{0}\" ", name);
        //        options.AppendFormat("value=\"{0}\" ", value1);
        //        options.Append(string.Compare(value, value1, true) == 0 ? "checked=\"checked\" " : "");
        //        options.Append(attr);
        //        options.Append("/>");
        //        options.AppendFormat("<label style=\"vertical-align:middle;margin-right:3px;\" for=\"{0}_{1}\">{2}</label>", name, child.ID.ToString("N"), child.Title);
        //    }
        //    return options.ToString();
        //}

        /// <summary>
        /// 根据ID得到多选项
        /// </summary>
        /// <param name="code"></param>
        /// <param name="name">名称</param>
        /// <param name="valueField"></param>
        /// <param name="value"></param>
        /// <param name="attr">其它属性</param>
        /// <returns></returns>
        //public string GetCheckboxsByID(Guid id, string name, OptionValueField valueField = OptionValueField.Value, string value = "", string attr = "")
        //{
        //    var childs = GetChilds(id, true);
        //    return getCheckboxs(childs, name, valueField, value, attr);
        //}

        /// <summary>
        /// 根据代码得到多选项
        /// </summary>
        /// <param name="code"></param>
        /// <param name="name">名称</param>
        /// <param name="valueField"></param>
        /// <param name="value"></param>
        /// <param name="attr">其它属性</param>
        /// <returns></returns>
        //public string GetCheckboxsByCode(string code, string name, OptionValueField valueField = OptionValueField.Value, string value = "", string attr = "")
        //{
        //    if (code.IsNullOrEmpty()) return "";
        //    var childs = GetChilds(code.Trim(), true);
        //    return getCheckboxs(childs, name, valueField, value, attr);
        //}

        //private string getCheckboxs(List<RoadFlow.Data.Model.Dictionary> childs, string name, OptionValueField valueField = OptionValueField.Value, string value = "", string attr = "")
        //{
        //    StringBuilder options = new StringBuilder(childs.Count * 100);
        //    foreach (var child in childs)
        //    {
        //        string value1 = getOptionsValue(valueField, child);
        //        options.Append("<input type=\"checkbox\" style=\"vertical-align:middle;\" ");
        //        options.AppendFormat("id=\"{0}_{1}\" ", name, child.ID.ToString("N"));
        //        options.AppendFormat("name=\"{0}\" ", name);
        //        options.AppendFormat("value=\"{0}\" ", value1);
        //        options.Append(value.Contains(value1) ? "checked=\"checked\"" : "");
        //        options.Append(attr);
        //        options.Append("/>");
        //        options.AppendFormat("<label style=\"vertical-align:middle;margin-right:3px;\" for=\"{0}_{1}\">{2}</label>", name, child.ID.ToString("N"), child.Title);
        //    }
        //    return options.ToString();
        //}

        private string getOptionsValue(OptionValueField valueField, RoadFlow.Data.Model.DictionaryModel dict)
        {
            string value = string.Empty;
            switch (valueField)
            {
                case OptionValueField.Code:
                    value = dict.Code;
                    break;
                case OptionValueField.ID:
                    value = dict.ID.ToString();
                    break;
                case OptionValueField.Note:
                    value = dict.Note;
                    break;
                case OptionValueField.Other:
                    value = dict.Other;
                    break;
                case OptionValueField.Title:
                    value = dict.Title;
                    break;
                case OptionValueField.Value:
                    value = dict.Value;
                    break;
            }
            return value;
        }

        /// <summary>
        /// 刷新字典缓存
        /// </summary>
        //public void RefreshCache()
        //{
        //    RoadFlow.Cache.IO.Opation.Set(cacheKey, GetAll());
        //}

        /// <summary>
        /// 检查代码是否存在
        /// </summary>
        /// <param name="code"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool HasCode(string code, string id = "")
        {
            if (code.IsNullOrEmpty())
            {
                return false;
            }
            var dict = GetByCode(code.Trim());
            Guid gid;
            if (dict == null)
            {
                return false;
            }
            else
            {
                if (id.IsGuid(out gid) && dict.ID == gid)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }

        /// <summary>
        /// 删除一个字典及其所有下级
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int DeleteAndAllChilds(Guid id)
        {
            int i = 0;
            var childs = GetAllChilds(id);
            foreach (var child in childs)
            {
                Delete(child.ID.Value);
                i++;
            }
            Delete(id);
            i++;
            return i;
        }

        /// <summary>
        /// 得到标题
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string GetTitle(Guid id)
        {
            var dict = Get(id);
            return dict == null ? "" : dict.Title;
        }

        /// <summary>
        /// 根据代码得到ID
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public Guid GetIDByCode(string code)
        {
            var dict = GetByCode(code);
            return dict == null ? Guid.Empty : dict.ID.Value;
        }
        #endregion

    }
}
