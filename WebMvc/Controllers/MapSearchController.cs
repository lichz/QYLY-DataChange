using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Data;
using System.Text;


namespace WebMvc.Controllers {
    public class MapSearchController : MyController {
        public ActionResult Index() {
            ViewBag.TypeFilter = TypeFilter();
            return View();
        }

        /// <summary>
        /// 获取点
        /// </summary>
        /// <param name="query">参数</param>
        /// <param name="perPage">每页显示条数</param>
        /// <param name="pageNumber">当前页</param>
        /// <returns></returns>
        public string GetPoint(string query, int perPage = 6, int pageNumber = 1) {
            int allPages = 0;
            //int count = 0;
            //DataTable dt = new RoadFlow.Platform.Enterprise().GetPagerData(out allPages, out count, query, pageNumber, perPage);
            DataTable dt = new RoadFlow.Platform.BuildingsAndBuildingMonthInfoBLL().OldGetAll(query);
            //dt = ConvertType(dt);
            if (dt == null || dt.Rows.Count < 1) {
                return RoadFlow.Utility.ObjectExpand.ToJson("");
            } else {
                return RoadFlow.Utility.ObjectExpand.ToJson(new { Dt = dt, AllPages = allPages });
            }
        }
        /// <summary>
        /// 获取所有过滤信息
        /// </summary>
        /// <returns></returns>
        public StringBuilder TypeFilter() {
            int beginNum = 11;
            StringBuilder content = new StringBuilder();
            //楼宇级别
            content.Append(TypeFilterRefining("LYJB", beginNum++, 3));
            //街道信息
            content.Append(TypeFilterRefining("SSJD", beginNum++));
            //建设阶段
            content.Append(TypeFilterRefining("JSJD", beginNum++));
            //楼宇类型
            content.Append(TypeFilterRefining("LYLX", beginNum++));
            //报送楼栋
            //content.Append(TypeFilterRefining("LYDS", beginNum++));
            //统筹招商
            content.Append(TypeFilterRefining("TCZS", beginNum++));
            return content;
        }
        /// <summary>
        /// TypeFilter代码提炼
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="col">table几列</param>
        /// <returns></returns>
        public string TypeFilterRefining(string keyword, int beginNum, int col = 4) {
            StringBuilder content = new StringBuilder();
            List<RoadFlow.Data.Model.Dictionary> list = new RoadFlow.Platform.Dictionary().GetAllChilds(keyword, false);
            if (keyword == "TCZS") {
                content.AppendFormat("<table data-name=\"{1}\" num=\"{0}\" style=\"width:50%;\" ><tr>", beginNum, keyword);
            } else {
                content.AppendFormat("<table data-name=\"{1}\" num=\"{0}\"><tr>", beginNum, keyword);
            }
            int i = 0;
            content.AppendFormat("<td><a class=\"item\" href=\"javascript:void(0)\" data-value=\"{1}\">{0}</a></td>", "全部", -1);
            i++;
            foreach (var item in list) {
                i++;
                content.AppendFormat("<td><a class=\"item\" href=\"javascript:void(0)\" data-value=\"{1}\">{0}</a></td>", item.Title, item.ID);
                if ((list.Count + 1) / col > 1) {//不止一行 加1的原因是前边有一个全部项。
                    if (i % col == 0 && i != list.Count) {
                        content.Append("</tr><tr>");
                    }
                } else {
                    if (i % col == 0) {
                        content.Append("</tr><tr>");
                    }
                }
            }
            content.Append("</tr></table>");
            return content.ToString();
        }
        /// <summary>
        /// 获取大厦详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string GetDetails(string id) {
            var info = new RoadFlow.Platform.BuildingsAndBuildingMonthInfoBLL().Get(Guid.Parse(id));//大楼信息
            DataTable ep = new RoadFlow.Platform.EnterpriseAndEnterpriseTaxBLL().GetAllByBuildingID(id);//入驻企业信息
            if (info == null) {
                return RoadFlow.Utility.ObjectExpand.ToJson("");
            } else {
                var dictionarys = new RoadFlow.Platform.DictionaryBLL().GetListAll();
                info.LYJBName = dictionarys.Find(p => p.ID == info.LYJB.Value) == null ? "" : dictionarys.Find(p => p.ID == info.LYJB.Value).Title;
                info.LYLXName = dictionarys.Find(p => p.ID == info.LYLX.Value) == null ? "" : dictionarys.Find(p => p.ID == info.LYLX.Value).Title;
                info.JSJDName = dictionarys.Find(p => p.ID == info.JSJD.Value) == null ? "" : dictionarys.Find(p => p.ID == info.JSJD.Value).Title;
                info.SSJDName = dictionarys.Find(p => p.ID == info.SSJD.Value) == null ? "" : dictionarys.Find(p => p.ID == info.SSJD.Value).Title;
                info.SY_ZJName = dictionarys.Find(p => p.ID == info.SY_ZJ.Value) == null ? "" : dictionarys.Find(p => p.ID == info.SY_ZJ.Value).Title;
                info.SW_ZJName = dictionarys.Find(p => p.ID == info.SW_ZJ.Value) == null ? "" : dictionarys.Find(p => p.ID == info.SW_ZJ.Value).Title;
                info.TCZSName = dictionarys.Find(p => p.ID == info.TCZS.Value) == null ? "" : dictionarys.Find(p => p.ID == info.TCZS.Value).Title;
                info.ZYKTName = dictionarys.Find(p => p.ID == info.ZYKT.Value) == null ? "" : dictionarys.Find(p => p.ID == info.ZYKT.Value).Title;
                info.LYCQQKName = dictionarys.Find(p => p.ID == info.LYCQQK.Value) == null ? "" : dictionarys.Find(p => p.ID == info.LYCQQK.Value).Title;
                //info = ConvertType(info);
                //ep = ConvertType(ep);
                Dictionary<string, string> columnName = new Dictionary<string, string>();
                return RoadFlow.Utility.ObjectExpand.ToJson(new { info, ep, columnName });
            }
        }
        #region 公用方法
        /// <summary>
        /// 转换数据编码
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public DataTable ConvertType(DataTable dt) {
            //取得编码字典
            var list = new RoadFlow.Platform.Dictionary().GetAll();
            //编码转换
            for (int i = 0; i < dt.Rows.Count; i++) {
                for (int j = 0; j < dt.Columns.Count; j++) {
                    if (list.FindAll(x => x.Code == dt.Columns[j].ColumnName).Count > 0) {
                        string temp = Convert.ToString(dt.Rows[i][j]);
                        dt.Rows[i][j] = list.Find(x => x.ID.ToString() == temp).Title;
                    }
                }
            }
            return dt;
        }
        /// <summary>
        /// 字段对应描述列表
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public Dictionary<string, string> ConvertColumn(DataTable dt) {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            RoadFlow.Platform.DBConnection conn = new RoadFlow.Platform.DBConnection();
            dic = conn.GetFields(conn.GetAll().First<RoadFlow.Data.Model.DBConnection>().ID, "[Buildings]");//获取所有字段名
            string name = string.Empty;
            for (int j = 0; j < dt.Columns.Count; j++) {
                if (dic.Keys.Contains(dt.Columns[j].ColumnName)) {
                    name = dic[dt.Columns[j].ColumnName];
                    if (!name.IsNullOrEmpty()) {
                        dic.Add(dt.Columns[j].ColumnName, name);
                    }
                }
            }
            return dic;
        }
        #endregion

    }
}