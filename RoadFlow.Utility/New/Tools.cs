//========================================
// Copyright © 2017
// 
// CLR版本 	: 4.0.30319.42000
// 计算机  	: USER-20170420WC
// 文件名  	: Tools.cs
// 创建人  	: kaifa5
// 创建时间	: 2017/9/25 14:06:40
// 文件版本	: 1.0.0
// 文件描述	: 
//========================================

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RoadFlow.Utility.New {
    /// <summary>
    /// 辅助工具静态封装类。
    /// </summary>
    public static class Tools {
        /// <summary>
        /// 获取指定长度的随机数字字符串（可用作数字验证码）。
        /// </summary>
        /// <param name="length">随机字符串的长度。</param>
        /// <returns>随机产生的字符串。</returns>
        public static string GetRandomNum(int length) {
            if (length < 1) {
                return string.Empty;
            }
            Guid seed = Guid.NewGuid();//保证循环快速调用时生成不同的随机数。
            Random random = new Random(seed.GetHashCode());
            StringBuilder builder = new StringBuilder(length);
            for (int i = 0; i < length; i++) {
                int number = random.Next();
                char code = Convert.ToChar(number % 10 + 48);
                builder.Append(code);
            }
            return builder.ToString();
        }

        /// <summary>
        /// 获取指定长度的随机字符串（包含小写字母、数字和大写字母）。
        /// </summary>
        /// <param name="length">随机字符串的长度。</param>
        /// <returns>随机产生的字符串。</returns>
        public static string GetRandomString(int length) {
            if (length < 1) {
                return string.Empty;
            }
            Guid seed = Guid.NewGuid();//保证循环快速调用时生成不同的随机数。
            Random random = new Random(seed.GetHashCode());
            StringBuilder builder = new StringBuilder(length);
            for (int i = 0; i < length; i++) {
                char code;
                int number = random.Next();
                switch (number % 3) {
                    case 0:
                        //小写字母
                        code = Convert.ToChar(number % 26 + 97);
                        break;
                    case 1:
                        //数字
                        code = Convert.ToChar(number % 10 + 48);
                        break;
                    default:
                        //大写字母
                        code = Convert.ToChar(number % 26 + 65);
                        break;
                }
                builder.Append(code);
            }
            return builder.ToString();
        }

        /// <summary>
        /// （后端）得到分页HTML。
        /// </summary>
        /// <param name="recordCount">记录总数。</param>
        /// <param name="pageSize">每页条数。</param>
        /// <param name="pageNumber">当前页。</param>
        /// <param name="queryString">查询字符串。</param>
        /// <returns>根据参数生成的翻页HTML代码。</returns>
        public static string GetPagerHtml(long recordCount, int pageSize, int pageNumber) {
            string queryString = string.Empty;
            long PageCount = recordCount <= 0 ? 1 : recordCount % pageSize == 0 ? recordCount / pageSize : recordCount / pageSize + 1;
            long pNumber = pageNumber;
            if (pNumber < 1) {
                pNumber = 1;
            } else if (pNumber > PageCount) {
                pNumber = PageCount;
            }
            if (PageCount <= 1) {
                return string.Empty;
            }
            StringBuilder ReturnPagerString = new StringBuilder(1500);
            string JsFunctionName = string.Empty;
            int DisplaySize = 10;
            ReturnPagerString.Append("<div>");
            ReturnPagerString.Append("<span style='margin-right:15px;'>共 " + recordCount.ToString() + " 条  每页 <input type='text' id='tnt_count' title='输入数字可改变每页显示条数' class='pagertxt' onchange=\"javascript:_toPage_" + JsFunctionName + "(" + pNumber.ToString() + ",this.value);\" value='" + pageSize.ToString() + "' /> 条  ");
            ReturnPagerString.Append("转到 <input type='text' id='paernumbertext' title='输入数字可跳转页' value=\"" + pNumber.ToString() + "\" onchange=\"javascript:_toPage_" + JsFunctionName + "(this.value," + pageSize.ToString() + ");\" class='pagertxt'/> 页</span>");
            if (pNumber > 1) {
                ReturnPagerString.Append("<a class=\"pager\" href=\"javascript:_toPage_" + JsFunctionName + "(" + (pNumber - 1).ToString() + "," + pageSize.ToString() + ");\"><span class=\"pagerarrow\">«</span></a>");
            }
            if (pNumber >= DisplaySize / 2 + 3) {
                ReturnPagerString.Append("<a class=\"pager\" href=\"javascript:_toPage_" + JsFunctionName + "(1," + pageSize.ToString() + ");\">1…</a>");
            } else {
                ReturnPagerString.Append("<a class=\"" + (1 == pNumber ? "pagercurrent" : "pager") + "\" href=\"javascript:_toPage_" + JsFunctionName + "(1," + pageSize.ToString() + ");\">1</a>");
            }
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
            if (star < 2) {
                star = 2;
            }
            for (long i = star; i <= end; i++) {
                ReturnPagerString.Append("<a class=\"" + (i == pNumber ? "pagercurrent" : "pager") + "\" href=\"javascript:_toPage_" + JsFunctionName + "(" + i.ToString() + "," + pageSize.ToString() + ");\">" + i.ToString() + "</a>");
            }
            if (pNumber <= PageCount - (DisplaySize / 2)) {
                ReturnPagerString.Append("<a class=\"pager\" href=\"javascript:_toPage_" + JsFunctionName + "(" + PageCount.ToString() + "," + pageSize.ToString() + ");\">…" + PageCount.ToString() + "</a>");
            } else if (PageCount > 1) {
                ReturnPagerString.Append("<a class=\"" + (PageCount == pNumber ? "pagercurrent" : "pager") + "\" href=\"javascript:_toPage_" + JsFunctionName + "(" + PageCount.ToString() + "," + pageSize.ToString() + ");\">" + PageCount.ToString() + "</a>");
            }
            if (pNumber < PageCount) {
                ReturnPagerString.Append("<a class=\"pager\" href=\"javascript:_toPage_" + JsFunctionName + "(" + (pNumber + 1).ToString() + "," + pageSize.ToString() + ");\"><span class=\"pagerarrow\">»</span></a>");
            }
            ReturnPagerString.Append("</div>");
            ReturnPagerString.Append("<script type=\"text/javascript\" lanuage=\"javascript\">");
            ReturnPagerString.Append("function _toPage_" + JsFunctionName + "(page,size){");
            ReturnPagerString.Append("$(\"form\").append(\"<input type='hidden' id='pageIndex' name='pageIndex' value='\"+page+\"' />\");$(\"form\").submit();");
            ReturnPagerString.Append("}");
            ReturnPagerString.Append("</script>");
            return ReturnPagerString.ToString();
        }

        /// <summary>
        /// show（前端）获取翻页HTML。
        /// </summary>
        /// <param name="count">总数。</param>
        /// <param name="size">每页数量。</param>
        /// <param name="pageIndex">当前页数。</param>
        /// <returns>根据参数生成的翻页HTML代码。</returns>
        public static string GetPagingHtml(int count, int size, int pageIndex) {
            long PageCount = count <= 0 ? 1 : count % size == 0 ? count / size : count / size + 1;
            if (PageCount <= 1) {
                return string.Empty;
            }
            long pNumber = pageIndex;
            if (pNumber < 1) {
                pNumber = 1;
            } else if (pNumber > PageCount) {
                pNumber = PageCount;
            }
            //如果只有一页则返回空分页字符串
            if (PageCount <= 1) {
                return string.Empty;
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
            if (star < 2) {
                star = 2;
            }
            //添加第一页
            if (star - 1 > 1) {
                ReturnPagerString.AppendFormat("<a href=\"javascript:;\" data-val=\"{0}\">1…</a>", 1);
            } else if (pNumber == 1) {
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
            if (PageCount > end + 1) {
                if (PageCount > 99) {//三位数及以上只显示"..."
                    ReturnPagerString.AppendFormat("<a href=\"javascript:;\" data-val=\"{0}\">…</a>", PageCount);
                } else {
                    ReturnPagerString.AppendFormat("<a href=\"javascript:;\" data-val=\"{0}\">…{0}</a>", PageCount);
                }
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
            return ReturnPagerString.ToString();
        }
    }
}