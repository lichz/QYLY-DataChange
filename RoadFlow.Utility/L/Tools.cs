using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Web;

namespace RoadFlow.Utility.L {
    /// <summary>
    /// Author L 2016/05/07
    /// </summary>
    public class Tools {

        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="table"></param>
        /// <param name="where"></param>
        /// <param name="pageSize">每页条数</param>
        /// <param name="pageIndex">第几页(第一页值为1)</param>
        /// <param name="pageIndex">唯一字段</param>
        /// <returns></returns>
        public static string GetPaging(string table, string where, int pageSize, int pageIndex, string uniqueField="ID") {
            int begin = pageSize*(pageIndex-1);
            int end = pageSize * pageIndex;
            StringBuilder sql = new StringBuilder();
            if (where.IsNullOrEmpty()) {
                sql.AppendFormat("SELECT w2.n, w1.* FROM {0} w1, (SELECT TOP {2} row_number() OVER (ORDER BY {3}) n, {3} FROM {0} as w3) w2 WHERE w1.{3} = w2.{3} AND w2.n > {1} ORDER BY w2.n ASC", table, begin, end,uniqueField);
            } else {
                sql.AppendFormat("SELECT w2.n, w1.* FROM {0} w1, (SELECT TOP {2} row_number() OVER (ORDER BY {4}) n, {4} FROM {0} as w3 where {3}) w2 WHERE w1.{4} = w2.{4} AND w2.n > {1} and {3} ORDER BY w2.n ASC", table, begin, end, where,uniqueField);
            }
            return sql.ToString();
        }

        /// <summary>
        /// 获取翻页
        /// </summary>
        /// <param name="count">总数</param>
        /// <param name="size"></param>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        public static string GetPagingHtml(int count,int size,int pageIndex) {
            //得到共有多少页
            long PageCount = count <= 0 ? 1 : count % size == 0 ? count / size : count / size + 1;

            if(PageCount<=1){
                return "";
            }

            long pNumber = pageIndex;
            if (pNumber < 1) {
                pNumber = 1;
            } else if (pNumber > PageCount) {
                pNumber = PageCount;
            }

            //如果只有一页则返回空分页字符串
            if (PageCount <= 1) {
                return "";
            }

            StringBuilder ReturnPagerString = new StringBuilder();
            string JsFunctionName = string.Empty;

            //样式：间距
            ReturnPagerString.Append("<style>.page a{ margin-left: 3px;margin-right: 3px;}</style>");

            //构造分页字符串
            int DisplaySize = 2;//中间显示的页数

            ReturnPagerString.Append("<div class=\"page\">");
            if (pNumber > 1) {
                ReturnPagerString.AppendFormat("<a href=\"javascript:;\" data-val=\"{0}\">上一页</a>", (pNumber - 1));
            } else if (pNumber == 1) {
                ReturnPagerString.AppendFormat("<a href=\"javascript:;\" data-val=\"{0}\">上一页</a>", pNumber);
            }

            //计算中间显示范围。
            long star = pNumber - DisplaySize / 2;
            long end = pNumber + DisplaySize / 2;
            if (star < 2) {
                end += 2 - star;
                star = 2;
            }
            if (end > PageCount - 1) {
                star -= end - (PageCount - 1);
                end = PageCount - 1;
            }
            if (star < 2)
                star = 2;

            //添加第一页
            if (star - 1 > 1)
                ReturnPagerString.AppendFormat("<a href=\"javascript:;\" data-val=\"{0}\">1…</a>", 1);
            else if (pNumber == 1) {
                ReturnPagerString.AppendFormat("<a class=\"on\" href=\"javascript:;\" data-val=\"{0}\">1</a>", 1);
            } else {
                ReturnPagerString.AppendFormat("<a href=\"javascript:;\" data-val=\"{0}\">1</a>", 1);
            }

            for (long i = star; i <= end; i++) {
                if (pNumber == i) {
                    ReturnPagerString.AppendFormat("<a class=\"on\" href=\"javascript:;\" data-val=\"{0}\">{0}</a>", i);
                } else {
                    ReturnPagerString.AppendFormat("<a href=\"javascript:;\" data-val=\"{0}\">{0}</a>", i);
                }

            }

            //添加最后页
            if (PageCount > end + 1)
                if (PageCount > 99) {//三位数及以上只显示"..."
                    ReturnPagerString.AppendFormat("<a href=\"javascript:;\" data-val=\"{0}\">…</a>", PageCount);
                } else {
                    ReturnPagerString.AppendFormat("<a href=\"javascript:;\" data-val=\"{0}\">…{0}</a>", PageCount);
                } else if (PageCount > 1) {
                if (pNumber == PageCount) {
                    ReturnPagerString.AppendFormat("<a class=\"on\" href=\"javascript:;\" data-val=\"{0}\">{0}</a>", PageCount);
                } else {
                    ReturnPagerString.AppendFormat("<a href=\"javascript:;\" data-val=\"{0}\">{0}</a>", PageCount);
                }
            }
            if (pNumber < PageCount) {
                ReturnPagerString.AppendFormat("<a href=\"javascript:;\" data-val=\"{0}\">下一页</a>", (pNumber + 1));
            } else if (pNumber == PageCount) {
                ReturnPagerString.AppendFormat("<a href=\"javascript:;\" data-val=\"{0}\">下一页</a>", pNumber);
            }

            ReturnPagerString.Append("</div>");
            //ReturnPagerString.Append("<div class=\"tata3 di\"><span>跳转到：</span><input class=\"tz_inp\" id=\"pageIndex\" type=\"text\"><input class=\"tz_btn\" type=\"button\" id=\"pageSubmit\" value=\"GO\"></div>");
            return ReturnPagerString.ToString();
        }
    }
}
