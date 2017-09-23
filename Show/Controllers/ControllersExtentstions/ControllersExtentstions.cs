using manage.Models;
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

namespace Show.Controllers {
    public static class ControllersExtentstions {
        /// <summary>
        /// 用于dr[key]获取图片完整路径（会在视图中调用。）
        /// </summary>
        /// <param name="path">图片路径</param>
        /// <returns></returns>
        public static string GetFullPathByDr(object path) {
            if(path is DBNull){
                return string.Empty;
            } else {
                return System.Configuration.ConfigurationManager.AppSettings["ImageDomain"] + path;
            }
        }

        /// <summary>
        /// 获取图片完整路径（会在视图中调用。）
        /// </summary>
        /// <param name="path">图片路径</param>
        /// <returns></returns>
        public static string GetFullPath(string path) { 
            if(string.IsNullOrWhiteSpace(path)){
                return string.Empty;
            } else {
                return System.Configuration.ConfigurationManager.AppSettings["ImageDomain"]+ path;
            }
        }


        /// <summary>
        /// 租金筛选范围在对应dictionary租金范围的ids字符串。
        /// </summary>
        /// <param name="area">租金筛选范围</param>
        /// <returns></returns>
        public static string GetAreaIds(string area) {
            string ids =string.Empty;
            if(string.IsNullOrWhiteSpace(area)){
                return ids;
            }
            int begin = area.Split(',')[0].Convert<int>(0);
            int end = area.Split(',')[1].Convert<int>(0);

            if(end==0){//无上限
                ids = GetAreaIds(begin);
            } else {
                ids = GetAreaIds(begin,end);
            }
            return ids;
        }

        private static string GetAreaIds(int begin) {
            string ids = string.Empty;
            Context db = new Context();
            Show.Models.DictionaryModel parent = db.Dictionarys.Where(p => p.Code == "SY_ZJ").FirstOrDefault();
            IQueryable<Show.Models.DictionaryModel> dictionarys = db.Dictionarys.Where(p => p.ParentID == parent.ID).OrderBy(p=>p.Sort); //所有的租金范围
            int beginD = 0;
            int endD = 0;
            foreach(var item in dictionarys) {
                ConvertDicionary(item.Title,out beginD,out endD);
                
                if(endD==0){//都没有上限，肯定有交集
                    ids += "," + item.ID;
                }
                if(endD>begin){//结束大于begin参数则有交集
                    ids += "," + item.ID;
                }
            }
            if(!string.IsNullOrWhiteSpace(ids)){
                ids = ids.Remove(0,1);//去掉首个','
            }
            return ids;
        }

        private static string GetAreaIds(int begin,int end) {
            List<Guid> list = new List<Guid>();
            Context db = new Context();
            Show.Models.DictionaryModel parent = db.Dictionarys.Where(p => p.Code == "SY_ZJ").FirstOrDefault();
            IQueryable<Show.Models.DictionaryModel> dictionarys = db.Dictionarys.Where(p => p.ParentID == parent.ID).OrderBy(p => p.Sort); //所有的租金范围
            int beginD = 0;
            int endD = 0;
            foreach (var item in dictionarys) {
                ConvertDicionary(item.Title, out beginD, out endD);

                if (endD == 0&&end>beginD) {//endD=0表示endD没有上限
                    if(!list.Contains(item.ID)){
                        list.Add(item.ID);
                    }
                }
                if (endD > begin&&end>beginD) {
                    if (!list.Contains(item.ID)) {
                        list.Add(item.ID);
                    }
                }
           
            }
            string ids = string.Join(",", list);
            return ids;
        }

        /// <summary>
        /// 将字符串转成beginD,endD
        /// </summary>
        private static void ConvertDicionary(string title,out int beginD,out int endD) {
            beginD = 0; endD = 0;
            if (title.Contains("以上")) {
                beginD = title.Replace("元以上", "").Convert<int>(0);
            } else if (title.Contains("以下")) {
                endD = title.Replace("元以下", "").Convert<int>(0);
            } else {
                title = title.Replace("元","");
                beginD = title.Split('-')[0].Convert<int>(0);
                endD = title.Split('-')[1].Convert<int>(0);
            }
        }

    }
}