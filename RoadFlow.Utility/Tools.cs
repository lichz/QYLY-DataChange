using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Reflection;

namespace RoadFlow.Utility
{
    public class Tools
    {

        public static System.IO.MemoryStream GetValidateImg(out string code, string bgImg = "/Images/vcodebg.png")
        {
            code = GetValidateCode();
            Random rnd = new Random();
            System.Drawing.Bitmap img = new System.Drawing.Bitmap((int)Math.Ceiling((code.Length * 17.2)), 28);
            System.Drawing.Image bg = System.Drawing.Bitmap.FromFile(HttpContext.Current.Server.MapPath(bgImg));
            System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(img);
            System.Drawing.Font font = new System.Drawing.Font("Arial", 16, (System.Drawing.FontStyle.Regular | System.Drawing.FontStyle.Italic));
            System.Drawing.Font fontbg = new System.Drawing.Font("Arial", 16, (System.Drawing.FontStyle.Regular | System.Drawing.FontStyle.Italic));
            System.Drawing.Drawing2D.LinearGradientBrush brush = new System.Drawing.Drawing2D.LinearGradientBrush(new System.Drawing.Rectangle(0, 0, img.Width, img.Height), System.Drawing.Color.Blue, System.Drawing.Color.DarkRed, 1.2f, true);
            g.DrawImage(bg, 0, 0, new System.Drawing.Rectangle(rnd.Next(bg.Width - img.Width), rnd.Next(bg.Height - img.Height), img.Width, img.Height), System.Drawing.GraphicsUnit.Pixel);
            g.DrawString(code, fontbg, System.Drawing.Brushes.White, 0, 1);
            g.DrawString(code, font, System.Drawing.Brushes.Green, 0, 1);//字颜色

            //画图片的背景噪音线 
            int x = img.Width;
            int y1 = rnd.Next(5, img.Height);
            int y2 = rnd.Next(5, img.Height);
            g.DrawLine(new System.Drawing.Pen(System.Drawing.Color.Green, 2), 1, y1, x - 2, y2);


            g.DrawRectangle(new System.Drawing.Pen(System.Drawing.Color.Transparent), 0, 10, img.Width - 1, img.Height - 1);
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            img.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            return ms;
        }

        private static string GetValidateCode()
        {    //产生五位的随机字符串
            int number;
            char code;
            string checkCode = String.Empty;
            System.Random random = new Random();

            for (int i = 0; i < 4; i++)
            {
                number = random.Next();

                if (number % 2 == 0)
                    code = (char)('0' + (char)(number % 10));
                else if (number % 3 == 0)
                    code = (char)('a' + (char)(number % 26));
                else
                    code = (char)('A' + (char)(number % 26));
                checkCode += code == '0' || code == 'O' ? "x" : code.ToString();
            }
            return checkCode;
        }

        /// <summary>
        /// 获取远程浏览器端 IP 地址
        /// </summary>
        /// <returns>返回 IPv4 地址</returns>
        public static string GetIPAddress()
        {
            string userHostAddress = HttpContext.Current.Request.UserHostAddress;
            if (userHostAddress.IsNullOrEmpty())
            {
                userHostAddress = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            }
            return userHostAddress;
        }

        /// <summary>
        /// 得到用户浏览器类型
        /// </summary>
        /// <returns></returns>
        public static string GetBrowse()
        {
            return System.Web.HttpContext.Current.Request.Browser.Type;
        }

        /// <summary>
        /// 获取浏览器端操作系统名称
        /// </summary>
        /// <returns></returns>
        public static string GetOSName()
        {
            string osVersion = System.Web.HttpContext.Current.Request.Browser.Platform;
            string userAgent = System.Web.HttpContext.Current.Request.UserAgent;

            if (userAgent.Contains("NT 6.3"))
            {
                osVersion = "Windows8.1";
            }
            else if (userAgent.Contains("NT 6.2"))
            {
                osVersion = "Windows8";
            }
            else if (userAgent.Contains("NT 6.1"))
            {
                osVersion = "Windows7";
            }
            else if (userAgent.Contains("NT 6.0"))
            {
                osVersion = "WindowsVista";
            }
            else if (userAgent.Contains("NT 5.2"))
            {
                osVersion = "WindowsServer2003";
            }
            else if (userAgent.Contains("NT 5.1"))
            {
                osVersion = "WindowsXP";
            }
            else if (userAgent.Contains("NT 5"))
            {
                osVersion = "Windows2000";
            }
            else if (userAgent.Contains("NT 4"))
            {
                osVersion = "WindowsNT4.0";
            }
            else if (userAgent.Contains("Me"))
            {
                osVersion = "WindowsMe";
            }
            else if (userAgent.Contains("98"))
            {
                osVersion = "Windows98";
            }
            else if (userAgent.Contains("95"))
            {
                osVersion = "Windows95";
            }
            else if (userAgent.Contains("Mac"))
            {
                osVersion = "Mac";
            }
            else if (userAgent.Contains("Unix"))
            {
                osVersion = "UNIX";
            }
            else if (userAgent.Contains("Linux"))
            {
                osVersion = "Linux";
            }
            else if (userAgent.Contains("SunOS"))
            {
                osVersion = "SunOS";
            }
            return osVersion;
        }

        /// <summary>
        /// 得到页尺寸
        /// </summary>
        /// <returns></returns>
        public static int GetPageSize()
        {
            string size = System.Web.HttpContext.Current.Request["pagesize"] ?? "15";
            int size1;
            return size.IsInt(out size1) ? size1 : 15;
        }

        /// <summary>
        /// 得到页号
        /// </summary>
        /// <returns></returns>
        public static int GetPageNumber()
        {
            string number = System.Web.HttpContext.Current.Request["pageIndex"] ?? "1";
            int number1;
            return number.IsInt(out number1) ? number1 : 1;
        }


        /// <summary>
        /// 得到列表项
        /// </summary>
        /// <param name="list">列表, 标题,值</param>
        /// <param name="value">默认值</param>
        /// <param name="showEmpty">是不显示空选项</param>
        /// <param name="emptyTitle">空选项显示标题</param>
        /// <returns></returns>
        public static System.Web.UI.WebControls.ListItem[] GetListItems(IList<string[]> list, string value, bool showEmpty = false, string emptyTitle = "")
        {
            List<System.Web.UI.WebControls.ListItem> items = new List<System.Web.UI.WebControls.ListItem>();
            if (showEmpty)
            {
                items.Add(new System.Web.UI.WebControls.ListItem(emptyTitle, ""));
            }
            foreach (var li in list)
            {
                if (li.Length < 2)
                {
                    continue;
                }
                var item = new System.Web.UI.WebControls.ListItem(li[0], li[1]);
                item.Selected = !value.IsNullOrEmpty() && value == li[1] && !items.Exists(p => p.Selected);
                items.Add(item);
            }
            return items.ToArray();
        }

        public static System.Web.UI.WebControls.ListItem[] GetListItems(IList<string> list, string value, bool showEmpty = false, string emptyTitle = "")
        {
            List<string[]> newList = new List<string[]>();
            foreach (string str in list)
            {
                newList.Add(new string[] { str, str });
            }
            return GetListItems(newList, value, showEmpty, emptyTitle);
        }
        /// <summary>
        /// 将服务器控件列表项转换为select列表项
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        public static string GetOptionsString(System.Web.UI.WebControls.ListItem[] items)
        {
            StringBuilder options = new StringBuilder(items.Length * 50);
            foreach (var item in items)
            {
                options.AppendFormat("<option value=\"{0}\" {1}>", item.Value.Replace("\"", "'"), item.Selected ? "selected=\"selected\"" : "");
                options.Append(item.Text);
                options.Append("</option>");
            }
            return options.ToString();
        }
        /// <summary>
        /// 将服务器控件列表项转换为Checkbox项
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        public static string GetCheckBoxString(System.Web.UI.WebControls.ListItem[] items, string name, string[] values, string otherAttr = "")
        {
            StringBuilder options = new StringBuilder(items.Length * 50);
            foreach (var item in items)
            {
                string tempid = Guid.NewGuid().ToString("N");
                options.AppendFormat("<input type=\"checkbox\" value=\"{0}\" {1} id=\"{2}\" name=\"{3}\" {4} style=\"vertical-align:middle\" />",
                    item.Value.Replace("\"", "'"),
                    values != null && values.Contains(item.Value) ? "checked=\"checked\"" : "",
                    string.Format("{0}_{1}", name, tempid),
                    name,
                    otherAttr
                    );
                options.AppendFormat("<label style=\"vertical-align:middle;margin-right:2px;\" for=\"{0}\">", string.Format("{0}_{1}", name, tempid));
                options.Append(item.Text);
                options.Append("</label>");
            }
            return options.ToString();
        }
        /// <summary>
        /// 将服务器控件列表项转换为Radio项
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        public static string GetRadioString(System.Web.UI.WebControls.ListItem[] items, string name, string otherAttr = "")
        {
            StringBuilder options = new StringBuilder(items.Length * 50);
            foreach (var item in items)
            {
                string tempid = Guid.NewGuid().ToString("N");
                options.AppendFormat("<input type=\"radio\" value=\"{0}\" {1} id=\"{2}\" name=\"{3}\" {4} style=\"vertical-align:middle\" />",
                    item.Value.Replace("\"", "'"),
                    item.Selected ? "checked=\"checked\"" : "",
                    string.Format("{0}_{1}", name, tempid),
                    name,
                    otherAttr
                    );
                options.AppendFormat("<label style=\"vertical-align:middle;margin-right:2px;\" for=\"{0}\">", string.Format("{0}_{1}", name, tempid));
                options.Append(item.Text);
                options.Append("</label>");
            }
            return options.ToString();
        }
        /// <summary>
        /// 得到是否选择项
        /// </summary>
        /// <param name="value"></param>
        /// <param name="showEmpty"></param>
        /// <param name="emptyString"></param>
        /// <returns></returns>
        public static System.Web.UI.WebControls.ListItem[] GetYesNoListItems(string value, bool showEmpty = false, string emptyString = "")
        {
            List<string[]> list = new List<string[]>();
            list.Add(new string[] { "是", "1" });
            list.Add(new string[] { "否", "0" });
            return GetListItems(list, value, showEmpty, emptyString);
        }

        /// <summary>
        /// 得到sql语句in里的字符串
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns></returns>
        public static string GetSqlInString(string str, bool isSingleQuotes = true, string split = ",")
        {
            string[] strArray = str.Split(new string[] { split }, StringSplitOptions.RemoveEmptyEntries);

            return GetSqlInString(strArray, isSingleQuotes);
        }

        /// <summary>
        /// 得到sql语句in里的字符串
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="strArray"></param>
        /// <param name="isSingleQuotes">是否加单引号，字符串要加，数字不加</param>
        /// <returns></returns>
        public static string GetSqlInString<T>(T[] strArray, bool isSingleQuotes = true)
        {
            StringBuilder inStr = new StringBuilder(strArray.Length * 40);
            foreach (var s in strArray)
            {
                if (s.ToString().IsNullOrEmpty())
                {
                    continue;
                }
                if (isSingleQuotes)
                {
                    inStr.Append("'");
                }
                inStr.Append(s.ToString().Trim());
                if (isSingleQuotes)
                {
                    inStr.Append("'");
                }
                inStr.Append(",");

            }
            return inStr.ToString().TrimEnd(',');
        }

        /// <summary>
        /// 产生不重复随机数
        /// </summary>
        /// <param name="count">共产生多少随机数</param>
        /// <param name="minValue">最小值</param>
        /// <param name="maxValue">最大值</param>
        /// <returns>int[]数组</returns>
        public static int[] GetRandomNum(int count, int minValue, int maxValue)
        {
            Random rnd = new Random(Guid.NewGuid().GetHashCode());

            int length = maxValue - minValue + 1;
            byte[] keys = new byte[length];
            rnd.NextBytes(keys);
            int[] items = new int[length];
            for (int i = 0; i < length; i++)
            {
                items[i] = i + minValue;
            }
            Array.Sort(keys, items);
            int[] result = new int[count];
            Array.Copy(items, result, count);
            return result;
        }

        /// <summary>
        /// 产生随机字符串
        /// </summary>
        /// <returns>字符串位数</returns> 
        public static string GetRandomString(int length = 5)
        {
            int number;
            char code;
            string checkCode = String.Empty;
            System.Random random = new Random(Guid.NewGuid().GetHashCode());

            for (int i = 0; i < length + 1; i++)
            {
                number = random.Next();

                if (number % 2 == 0)
                    code = (char)('0' + (char)(number % 10));
                else
                    code = (char)('A' + (char)(number % 26));
                checkCode += code.ToString();
            }
            return checkCode;
        }

        /// <summary>
        /// 产生随机字母
        /// </summary>
        /// <returns>字符串位数</returns>
        public static string GetRandomLetter(int length = 2)
        {
            int number;
            char code;
            string checkCode = String.Empty;
            System.Random random = new Random(Guid.NewGuid().GetHashCode());
            for (int i = 0; i < length; i++)
            {
                number = random.Next();
                code = (char)('A' + (char)(number % 26));
                checkCode += code.ToString();
            }
            return checkCode;
        }

        /// <summary>
        /// 得到一个文件的大小(单位KB)
        /// </summary>
        /// <returns></returns>
        public static string GetFileSize(string file)
        {
            if (!System.IO.File.Exists(file))
            {
                return "";
            }
            System.IO.FileInfo fi = new System.IO.FileInfo(file);

            return (fi.Length / 1000).ToString("###,###");
        }

        public static string DataTableToJsonString(System.Data.DataTable dt)
        {
            LitJson.JsonData json = new LitJson.JsonData();
            foreach (System.Data.DataRow dr in dt.Rows)
            {
                LitJson.JsonData drJson = new LitJson.JsonData();
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    LitJson.JsonData drJson1 = new LitJson.JsonData();
                    drJson1 = dr[i].ToString();
                    drJson.Add(drJson1);
                }
                json.Add(drJson);
            }
            return json.ToJson();
        }

        #region Author L
        #region 模型反射
        /// <summary>
        /// 获取属性特质
        /// </summary>
        /// <typeparam name="Model">模型类泛型</typeparam>
        /// <typeparam name="Attribute">特质泛型</typeparam>
        /// <param name="after">GetPropertiesAttribute获取特质之后的处理</param>
        public static void GetPropertiesAttribute<Model, Attribute>(AfterGetPropertiesAttribute<Attribute> after) where Attribute : class
        {
            Type t = typeof(Model);
            Model instance = (Model)Activator.CreateInstance(t);
            int count = 0;//循环计数
            foreach (PropertyInfo info in t.GetProperties())
            {
                object[] objs = info.GetCustomAttributes(typeof(Attribute), true);
                if (objs == null || objs.Length == 0)
                {
                    continue;
                }
                Attribute attr = objs[0] as Attribute;
                after(count, attr);
                count++;
            }
        }

        /// <summary>
        /// 获取属性值
        /// </summary>
        /// <typeparam name="Model">模型类泛型</typeparam>
        /// <param name="before">取值判断,参数PropertyInfo propertyInfo</param>
        /// <param name="after">后续处理,参数(int count, object value,PropertyInfo propertyInfo)</param>
        /// <param name="model"></param>
        public static void GetPropertiesValue<Model>(BeforeGetPropertiesValue before, AfterGetPropertiesValue after, Model model)
        {
            Type t = typeof(Model);
            int count = 0;//循环计数
            foreach (PropertyInfo info in t.GetProperties())
            {
                //取值判断
                if (before(info)|| !info.CanRead)
                {
                    continue;
                };
                var v = info.GetValue(model, null);//取值
                if (after(count, v, info))
                {
                    continue;
                }//后续处理
                count++;
            }
        }

        /// <summary>
        /// 获取属性值
        /// </summary>
        /// <typeparam name="Model">模型类泛型</typeparam>
        /// <param name="after">后续处理,参数(int count, object value,PropertyInfo propertyInfo)</param>
        /// <param name="model"></param>
        public static void GetPropertiesValue<Model>(AfterGetPropertiesValue after, Model model)
        {
            Type t = typeof(Model);
            int count = 0;//循环计数
            foreach (PropertyInfo info in t.GetProperties())
            {
                if (!info.CanRead)
                {
                    continue;
                }
                var v = info.GetValue(model, null);//取值
                if (after(count, v, info))
                {
                    continue;
                };//后续处理
                count++;
            }
        }

        public static void GetPropertiesValueByDynamic(AfterGetPropertiesValueByDynamic after, dynamic model)
        {
            Type t = model.GetType();
            foreach (PropertyInfo info in t.GetProperties())
            {
                if (!info.CanRead)
                {
                    continue;
                }
                var v = info.GetValue(model, null);//取值
                if (after( v, info))
                {
                    continue;
                };//后续处理
            }
        }

        /// <summary>
        /// 设置属性值
        /// </summary>
        /// <typeparam name="Model">模型类泛型</typeparam>
        public static Model SetPropertiesValue<Model>(SetPropertiesValueOfGetValue getValue)
        {
            Type t = typeof(Model);
            Model result = (Model)Activator.CreateInstance(t);

            foreach (PropertyInfo pi in t.GetProperties())
            {
                if (pi.CanWrite)
                {
                    var value = getValue(pi);
                    if (pi.PropertyType.IsGenericType && pi.PropertyType.GetGenericTypeDefinition().Equals(typeof(Nullable<>)) && Nullable.GetUnderlyingType(pi.PropertyType).IsEnum)
                    {
                        pi.SetValue(result, Enum.Parse(Nullable.GetUnderlyingType(pi.PropertyType), value.ToString()), null);
                    }
                    else
                    {
                        pi.SetValue(result, value, null);
                    }
                }
            }

            return result;
        }
        #endregion

        #endregion
    }

    /// <summary>
    /// SetPropertiesValue中的获取value部分
    /// </summary>
    /// <param name="propertyInfo"></param>
    /// <returns></returns>
    public delegate object SetPropertiesValueOfGetValue(PropertyInfo propertyInfo);

    /// <summary>
    /// GetPropertiesValue获取值之前的处理
    /// </summary>
    /// <param name="propertyInfo">模型属性</param>
    /// <returns>是否continue</returns>
    public delegate bool BeforeGetPropertiesValue(PropertyInfo propertyInfo);

    /// <summary>
    /// GetPropertiesValue获取值之后的处理
    /// </summary>
    /// <param name="count">循环计数</param>
    /// <param name="value">属性值</param>
    /// <param name="propertyInfo">属性</param>
    /// <returns>是否continue</returns>
    public delegate bool AfterGetPropertiesValue(int count, object value,PropertyInfo propertyInfo);

    /// <summary>
    /// GetPropertiesValueByDynamic获取值之后的处理
    /// </summary>
    /// <param name="value">属性值</param>
    /// <param name="propertyInfo">属性</param>
    /// <returns>是否continue</returns>
    public delegate bool AfterGetPropertiesValueByDynamic(object value, PropertyInfo propertyInfo);

    /// <summary>
    /// GetPropertiesAttribute获取特质之后的处理
    /// </summary>
    /// <param name="count">循环计数</param>
    public delegate void AfterGetPropertiesAttribute<Attribute>(int count, Attribute attribute);
}
