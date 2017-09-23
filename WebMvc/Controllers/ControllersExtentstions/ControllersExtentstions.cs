using RoadFlow.Data.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace WebMvc.Controllers
{
    public static class ControllersExtentstions
    {
        #region 视图
        public static MvcHtmlString CheckBoxList(this HtmlHelper htmlHelper, string name, string codeCategory, RepeatDirection repeatDirection = RepeatDirection.Horizontal)
        {
            RoadFlow.Platform.DictionaryBLL bll = new RoadFlow.Platform.DictionaryBLL();
            var codes = bll.GetListByCode(codeCategory);
            return GenerateHtml(name, codes, repeatDirection, "checkbox", null);
        }
        public static MvcHtmlString CheckBoxListFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, string codeCategory, RepeatDirection repeatDirection = RepeatDirection.Horizontal)
        {
            RoadFlow.Platform.DictionaryBLL bll = new RoadFlow.Platform.DictionaryBLL();
            var codes = bll.GetListByCode(codeCategory);
            ModelMetadata metadata = ModelMetadata.FromLambdaExpression<TModel, TProperty>(expression, htmlHelper.ViewData);
            string name = ExpressionHelper.GetExpressionText(expression);
            string fullHtmlFieldName = htmlHelper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(name);
            return GenerateHtml(fullHtmlFieldName, codes, repeatDirection, "checkbox", metadata.Model);
        }

        public static MvcHtmlString GenerateHtml(string name, DataTable codes, RepeatDirection repeatDirection, string type, object stateValue)
        {
            TagBuilder table = new TagBuilder("table");
            int i = 0;
            bool isCheckBox = type == "checkbox";
            if (repeatDirection == RepeatDirection.Horizontal)
            {
                TagBuilder tr = new TagBuilder("tr");
                foreach (DataRow dr in codes.Rows)
                {
                    i++;
                    string id = string.Format("{0}_{1}", name, i);
                    TagBuilder td = new TagBuilder("td");

                    bool isChecked = false;
                    Guid code = (Guid)dr["ID"];
                    string description = (string)dr["Title"];
                    if (isCheckBox)
                    {
                        IEnumerable<Guid> currentValues = stateValue as IEnumerable<Guid>;
                        isChecked = (null != currentValues && currentValues.Contains(code));
                    }
                    else
                    {
                        Guid currentValue = (Guid)stateValue;
                        isChecked = (null != currentValue && code == currentValue);
                    }

                    td.InnerHtml = GenerateRadioHtml(name, id, description, code, isChecked, type);
                    tr.InnerHtml += td.ToString();
                }
                table.InnerHtml = tr.ToString();
            }
            else
            {
                foreach (DataRow dr in codes.Rows)
                {
                    TagBuilder tr = new TagBuilder("tr");
                    i++;
                    string id = string.Format("{0}_{1}", name, i);
                    TagBuilder td = new TagBuilder("td");

                    bool isChecked = false;
                    Guid code = (Guid)dr["ID"];
                    string description = (string)dr["Title"];
                    if (isCheckBox)
                    {
                        IEnumerable<Guid> currentValues = stateValue as IEnumerable<Guid>;
                        isChecked = (null != currentValues && currentValues.Contains(code));
                    }
                    else
                    {
                        Guid currentValue = (Guid)stateValue;
                        isChecked = (null != currentValue && code == currentValue);
                    }

                    td.InnerHtml = GenerateRadioHtml(name, id, description, code, isChecked, type);
                    tr.InnerHtml = td.ToString();
                    table.InnerHtml += tr.ToString();
                }
            }
            return new MvcHtmlString(table.ToString());
        }

        private static string GenerateRadioHtml(string name, string id, string labelText, Guid value, bool isChecked, string type)
        {
            StringBuilder sb = new StringBuilder();

            TagBuilder label = new TagBuilder("label");
            label.MergeAttribute("for", id);
            label.SetInnerText(labelText);

            TagBuilder input = new TagBuilder("input");
            input.GenerateId(id);
            input.MergeAttribute("name", name);
            input.MergeAttribute("type", type);
            input.MergeAttribute("value", value.ToString());
            if (isChecked)
            {
                input.MergeAttribute("checked", "checked");
            }
            sb.AppendLine(input.ToString());
            sb.AppendLine(label.ToString());
            return sb.ToString();
        }

        #endregion

        #region controller
        /// <summary>
        /// 导出前的预处理
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="fields">字段列表</param>
        /// <param name="includeOrExclude">字段列表是包含还是排除列表,true是包含</param>
        /// <returns></returns>
        public static DataTable ExportExcelPre(this DataTable dt, Dictionary<string, string> fields, bool includeOrExclude)
        {
            return dt.ToNewDataTable(fields, includeOrExclude);
        }

        /// <summary>
        /// 导出前的预处理
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="name">查询配置列表显示名称</param>
        /// <returns></returns>
        public static DataTable ExportExcelPre(this DataTable dt, string name)
        {
            #region 导出列
            Dictionary<string, string> fields = new Dictionary<string, string>();
            List<ColItem> colItemList = new RoadFlow.Platform.QueryDesign().GetColItemList(name);
            foreach (var item in colItemList)
            {
                if (item.chk == true)
                {
                    fields.Add(item.id, item.value);
                }
            }
            #endregion

            return dt.ToNewDataTable(fields);
        }

        /// <summary>
        /// 将数据处理充填到新的dataTable中。
        /// </summary>
        /// <returns></returns>
        private static DataTable ToNewDataTable(this DataTable dt, Dictionary<string, string> fields, bool includeOrExclude = true)
        {
            DataTable table = new DataTable("export");
            DataColumn column;
            DataRow row;
            #region 处理导出列
            if (includeOrExclude)
            {//包含
                foreach (var item in fields)
                {
                    column = new DataColumn();
                    column.DataType = dt.Columns[item.Key].DataType.ToType();
                    column.ColumnName = item.Key;
                    column.Caption = item.Value;
                    table.Columns.Add(column);
                }
            }
            else
            { //排除
                foreach (DataColumn item in dt.Columns)
                {
                    if (!fields.Keys.Contains(item.ColumnName))
                    {
                        column = new DataColumn();
                        column.DataType = item.DataType.ToType();
                        column.ColumnName = item.ColumnName;
                        column.Caption = fields[item.ColumnName];
                        table.Columns.Add(column);
                    }
                }
            }
            #endregion
            #region 处理导出行
            foreach (DataRow dr in dt.Rows)
            {
                row = table.NewRow();
                foreach (DataColumn item in table.Columns)
                {
                    row[item.ColumnName] = dr[item.ColumnName].IntOrDecimalGetVal(item.DataType);
                }
                table.Rows.Add(row);
            }

            #endregion

            return table.FindDictionaryTitle();
        }

        /// <summary>
        /// 编码转换，将Guid转成对应数据字典的名称
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        private static DataTable FindDictionaryTitle(this DataTable table)
        {
            #region 编码转换，将Guid转成对应数据字典的名称
            RoadFlow.Platform.DictionaryBLL dictionaryBLL = new RoadFlow.Platform.DictionaryBLL();
            var dictionarys = dictionaryBLL.GetListAll();
            //编码转换
            for (int i = 0; i < table.Rows.Count; i++)
            {
                for (int j = 0; j < table.Columns.Count; j++)
                {
                    if (dictionarys.FindAll(x => x.Code == table.Columns[j].ColumnName).Count > 0)
                    {
                        string temp = (string)table.Rows[i][j];
                        table.Rows[i][j] = dictionarys.Find(x => x.ID.ToString() == temp).Title;
                    }
                }
            }
            #endregion
            return table;
        }

        /// <summary>
        /// int,decimal特殊处理
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        private static Type ToType(this Type t)
        {
            if (t == typeof(decimal) || t == typeof(int))
            {
                return t;
            }
            return System.Type.GetType("System.String");
        }

        /// <summary>
        /// int,decimal取值特殊处理
        /// </summary>
        /// <returns></returns>
        private static object IntOrDecimalGetVal(this object v,Type columnType)
        {
            if (columnType == typeof(decimal) || columnType == typeof(int))
            {
                if (v is DBNull)
                {
                    return 0;
                }
                if (columnType == typeof(decimal))
                {
                    return (decimal)v;
                }
                return (int)v;
            }
            else
            {
                return v.ToString();
            }
        }
        #endregion

    }
}