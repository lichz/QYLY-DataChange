using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
namespace RoadFlow.Utility
{
    /// <summary>
    /// 导入导出Excel
    /// </summary>
    public class ExportExcel
    {
        /// <summary>
        /// 少量数据导出
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="name">导出Excel文件名</param>
        public static void Export(DataTable dt, string name)
        {
            //使用NPOI操作Excel表
            if (dt.Rows.Count <= 0) return;

            //创建工作薄
            var workbook = new HSSFWorkbook();
           
            WorkbookCreateCell(workbook, dt,null);
            WorkbookWrite(name,workbook);
        }

        /// <summary>
        /// 少量数据导出
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="name"></param>
        /// <param name="subtotal">结尾的总计数据等</param>
        public static void Export(DataTable dt, string name, List<List<string>> subtotal)
        {
            //使用NPOI操作Excel表
            if (dt.Rows.Count <= 0) return;

            //创建工作薄
            var workbook = new HSSFWorkbook();

            WorkbookCreateCell(workbook, dt, subtotal);
            WorkbookWrite(name, workbook);
        }

        /// <summary>
        /// 少量数据导出
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="name"></param>
        public static void Export<T>(List<T> list, string name)
        {
            //使用NPOI操作Excel表
            if (list.Count <= 0) return;

            //创建工作薄
            var workbook = new HSSFWorkbook();
            //Excel的Sheet对象
            var sheet = workbook.CreateSheet("sheet1");
            var style = workbook.CreateCellStyle();
            style.Alignment = HorizontalAlignment.Center;
            style.VerticalAlignment = VerticalAlignment.Center;
            style.WrapText = true;

            //设置导出字段标题
            var rowZdTitle = sheet.CreateRow(0);

            Tools.GetPropertiesAttribute<T,DisplayNameAttribute>(delegate (int col, DisplayNameAttribute displayName) 
            {//获取属性特质后续处理
                var cellZdTitle = rowZdTitle.CreateCell(col);
                cellZdTitle.SetCellValue(displayName.DisplayName);
                cellZdTitle.CellStyle = style;
            });

            //设置导出数据
            int row = 1;
            foreach (var item in list)
            {
                var trow = sheet.CreateRow(row++);
                Tools.GetPropertiesValue(delegate (PropertyInfo propertyInfo)
                {//取值前判断，返回是否continue
                    object[] objs = propertyInfo.GetCustomAttributes(typeof(DisplayNameAttribute), true);
                     if (objs == null || objs.Length == 0)
                     {
                         return true;
                     }
                     return false;
                 }, delegate (int col,object value, PropertyInfo propertyInfo) 
                 {//取值后处理，返回是否continue
                     var cell = trow.CreateCell(col);
                     if (value != null)
                     {
                         SetCellValue(cell, value);
                     }
                     cell.CellStyle = style;
                     return false;
                 }, item);

            }

            //保存excel文档
            sheet.ForceFormulaRecalculation = true;
            //文件流对象
            WorkbookWrite(name,workbook);

        }

        /// <summary>
        /// 导入到DataTable
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static DataTable ImportToTable(string filePath)
        {
            #region Ole
            //bool b = false;
            //string conn = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + filePath + ";Extended Properties='Excel 8.0;HOR=Yes;IMEX=1'";
            //OleDbConnection oleCon = new OleDbConnection(conn);
            //oleCon.Open();
            //DataTable dataTable = oleCon.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
            //string tableName = dataTable.Rows[0][2].ToString().Trim();
            //tableName = "[" + tableName.Replace("'", "") + "]";
            //string sql = "select * from " + tableName;
            //OleDbDataAdapter mycommand = new OleDbDataAdapter(sql, oleCon);
            ////DataSet ds = new DataSet();
            //DataTable dt = new DataTable();
            //mycommand.Fill(dt);
            //oleCon.Close();

            //int columnIndex = 0;

            //foreach (DataRow item in dt.Rows) {

            //}
            #endregion

            #region NPOI
            IWorkbook workbook;
            #region//初始化信息
            try
            {
                using (FileStream file = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    if (file.Name.Contains(".xlsx"))
                    {
                        workbook = new XSSFWorkbook(file);
                    }
                    else
                    {
                        workbook = new HSSFWorkbook(file);
                    }

                }
            }
            catch (Exception e)
            {
                throw e;
            }
            #endregion
            ISheet sheet = workbook.GetSheetAt(0);
            DataTable table = new DataTable();
            //获取sheet的首行
            IRow headerRow = sheet.GetRow(0);
            int cellCount = headerRow.LastCellNum;

            for (int i = headerRow.FirstCellNum; i < cellCount; i++)
            {
                DataColumn column = new DataColumn(GetColumnName(i), GetColumnType(i));
                table.Columns.Add(column);
            }
            //最后一列的标号  即总的行数
            int rowCount = sheet.LastRowNum;

            for (int i = (sheet.FirstRowNum + 1); i <= sheet.LastRowNum; i++)
            {
                IRow row = sheet.GetRow(i);
                DataRow dataRow = table.NewRow();
                try
                {
                    for (int j = row.FirstCellNum; j < cellCount; j++)
                    {
                        if (row.GetCell(j) != null)
                        {
                            dataRow[j] = GetCellValue(row.GetCell(j));
                        }
                    }
                }
                catch (ArgumentException e)
                {//某个设置值无效
                    continue;
                }
                table.Rows.Add(dataRow);
            }
            workbook = null;
            sheet = null;
            #endregion
            return table;
        }

        /// <summary>
        /// 导入到DataTable
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static List<T> ImportToTable<T>(string filePath)
        {
            #region NPOI
            IWorkbook workbook;
            #region//初始化信息
            try
            {
                using (FileStream file = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    if (file.Name.Contains(".xlsx"))
                    {
                        workbook = new XSSFWorkbook(file);
                    }
                    else
                    {
                        workbook = new HSSFWorkbook(file);
                    }

                }
            }
            catch (Exception e)
            {
                throw e;
            }
            #endregion
            ISheet sheet = workbook.GetSheetAt(0);
            List<T> list = new List<T>();//返回的集合
            Dictionary<int, PropertyInfo> cellIndexs = new Dictionary<int, PropertyInfo>();//导入的列下标和对应泛型中的属性。
            //获取sheet的首行(首行为导入列中文名称如"企业名称")
            IRow headerRow = sheet.GetRow(0);
            int cellCount = headerRow.LastCellNum;

            #region 获取当前excel可以导入的列
            //所有列(用于判断哪些列是需要的列)
            Type t = typeof(T);
            T instance = (T)Activator.CreateInstance(t);
            Dictionary<string, int> dictionary = new Dictionary<string, int>();//列名对应的下标
            for (int i = headerRow.FirstCellNum; i < cellCount; i++)
            {
                dictionary.Add(headerRow.GetCell(i).ToString(), i);
            }
            foreach (var pi in t.GetProperties())
            {
                object[] objs = pi.GetCustomAttributes(typeof(DisplayAttribute), true);
                DisplayAttribute attr = objs[0] as DisplayAttribute;
                if (dictionary.Keys.Contains(attr.Name) && pi.CanWrite)
                {//excel中属于泛型对象的列
                    cellIndexs.Add(dictionary[attr.Name], pi);
                }
            }
            #endregion

            //最后一列的标号  即总的行数
            int rowCount = sheet.LastRowNum;

            for (int i = (sheet.FirstRowNum + 1); i <= sheet.LastRowNum; i++)
            {
                T model = (T)Activator.CreateInstance(t);
                IRow row = sheet.GetRow(i);
                try
                {
                    foreach (var item in cellIndexs)
                    {
                        if (row.GetCell(item.Key) != null)
                        {
                            ICell cell = row.GetCell(item.Key);
                            object value = GetCellValue(cell);
                            if (item.Value.PropertyType.Name != value.GetType().Name)
                            {//类型不一样的时候，强转
                                value = value.Convert(item.Value.PropertyType);
                            }
                            item.Value.SetValue(model, value, null);
                        }
                    }
                    list.Add(model);
                }
                catch (ArgumentException e)
                {//某个设置值无效
                    continue;
                }
            }

            workbook = null;
            sheet = null;
            #endregion
            return list;
        }

        /// <summary>
        /// 导入到DataTable
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static List<object> ImportToTable(string filePath, Type t)
        {
            #region NPOI
            IWorkbook workbook;
            #region//初始化信息
            try
            {
                using (FileStream file = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    if (file.Name.Contains(".xlsx"))
                    {
                        workbook = new XSSFWorkbook(file);
                    }
                    else
                    {
                        workbook = new HSSFWorkbook(file);
                    }

                }
            }
            catch (Exception e)
            {
                throw e;
            }
            #endregion
            ISheet sheet = workbook.GetSheetAt(0);
            List<object> list = new List<object>();//返回的集合
            Dictionary<int, PropertyInfo> cellIndexs = new Dictionary<int, PropertyInfo>();//导入的列下标和对应泛型中的属性。
            //获取sheet的首行(首行为导入列中文名称如"企业名称")
            IRow headerRow = sheet.GetRow(0);
            int cellCount = headerRow.LastCellNum;

            #region 获取当前excel可以导入的列
            //所有列(用于判断哪些列是需要的列)
            object instance = Activator.CreateInstance(t);
            Dictionary<string, int> dictionary = new Dictionary<string, int>();//列名对应的下标
            for (int i = headerRow.FirstCellNum; i < cellCount; i++)
            {
                dictionary.Add(headerRow.GetCell(i).ToString(), i);
            }
            foreach (var pi in t.GetProperties())
            {
                object[] objs = pi.GetCustomAttributes(typeof(DisplayAttribute), true);
                DisplayAttribute attr = objs[0] as DisplayAttribute;
                if (dictionary.Keys.Contains(attr.Name) && pi.CanWrite)
                {//excel中属于泛型对象的列
                    cellIndexs.Add(dictionary[attr.Name], pi);
                }
            }
            #endregion

            //最后一列的标号  即总的行数
            int rowCount = sheet.LastRowNum;

            for (int i = (sheet.FirstRowNum + 1); i <= sheet.LastRowNum; i++)
            {
                object model = Activator.CreateInstance(t);
                IRow row = sheet.GetRow(i);
                try
                {
                    foreach (var item in cellIndexs)
                    {
                        if (row.GetCell(item.Key) != null)
                        {
                            ICell cell = row.GetCell(item.Key);
                            object value = GetCellValue(cell);
                            if (item.Value.PropertyType.Name != value.GetType().Name)
                            {//类型不一样的时候，强转
                                value = value.Convert(item.Value.PropertyType);
                            }
                            item.Value.SetValue(model, value, null);
                        }
                    }
                    list.Add(model);
                }
                catch (ArgumentException e)
                {//某个设置值无效
                    continue;
                }
            }

            workbook = null;
            sheet = null;
            #endregion
            return list;
        }

        #region 公共方法
        /// <summary>
        /// 文件流输出
        /// </summary>
        private static void WorkbookWrite(string name, HSSFWorkbook workbook)
        {
            //文件流对象
            var stream = new MemoryStream();
            workbook.Write(stream);
            HttpContext.Current.Response.AddHeader("Content-Disposition", string.Format("attachment; filename={0}.xls", HttpUtility.UrlEncode(name + "_" + DateTime.Now.ToString("yyyy-MM-dd"), System.Text.Encoding.UTF8)));
            HttpContext.Current.Response.BinaryWrite(stream.ToArray());
            HttpContext.Current.Response.End();
            stream.Close();
            stream.Dispose();
        }

        /// <summary>
        /// 创建设置单元格值。
        /// </summary>
        private static void WorkbookCreateCell(HSSFWorkbook workbook, DataTable dt, List<List<string>> endList)
        {
            //Excel的Sheet对象
            var sheet = workbook.CreateSheet("sheet1");
            var style = workbook.CreateCellStyle();
            style.Alignment = HorizontalAlignment.Center;
            style.VerticalAlignment = VerticalAlignment.Center;
            style.WrapText = true;

            //设置导出字段标题
            var rowZdTitle = sheet.CreateRow(0);
            for (var i = 0; i < dt.Columns.Count; i++)
            {
                var cellZdTitle = rowZdTitle.CreateCell(i);
                cellZdTitle.SetCellValue(dt.Columns[i].Caption);
                cellZdTitle.CellStyle = style;
            }
            //设置导出数据
            for (var row = 0; row < dt.Rows.Count; row++)
            {
                var trow = sheet.CreateRow(row + 1);
                for (var col = 0; col < dt.Columns.Count; col++)
                {
                    var cell = trow.CreateCell(col);
                    var temp = dt.Rows[row][col];
                    cell = SetCellValue(cell, temp);

                    cell.CellStyle = style;
                }
            }

            if (endList != null)
            {
                int row = dt.Rows.Count + 2;
                for (var i = 0; i < endList.Count; i++)
                {
                    var trow = sheet.CreateRow(row + i);
                    for (var j = 0; j < endList[i].Count; j++)
                    {
                        var cell = trow.CreateCell(j);
                        cell.SetCellValue(endList[i][j]);
                        cell.CellStyle = style;
                    }
                }
            }

            //保存excel文档
            sheet.ForceFormulaRecalculation = true;
        }

        private static string GetColumnName(int index)
        {
            if (index == 0)
            {
                return "Name";
            }
            else if (index == 1)
            {
                return "TYSHXYDM";
            }
            else if (index == 2)
            {
                return "Tax1";
            }
            else if (index == 3)
            {
                return "Tax2";
            }
            else if (index == 4)
            {
                return "TaxYear";
            }
            return string.Empty;
        }

        private static Type GetColumnType(int index)
        {
            if (index == 0)
            {
                return typeof(String);
            }
            else if (index == 1)
            {
                return typeof(String);
            }
            else if (index == 2)
            {
                return typeof(Decimal);
            }
            else if (index == 3)
            {
                return typeof(Decimal);
            }
            else if (index == 4)
            {
                return typeof(int);
            }
            return typeof(String);
        }

        private static object GetCellValue(ICell cell)
        {
            if (cell.CellType == CellType.String)
            {
                return cell.StringCellValue;
            }
            else if (cell.CellType == CellType.Numeric)
            {
                if (DateUtil.IsCellDateFormatted(cell))
                {
                    return cell.DateCellValue.Year;
                }
                if (cell.NumericCellValue.ToString().Contains("E"))
                { //表示格式
                    return cell.NumericCellValue.ToString("0");
                }
                else
                {
                    return cell.NumericCellValue;
                }
            }
            else
            {
                return cell.ToString();
            }
        }

        /// <summary>
        /// 数据导出前的类型判断和转换
        /// </summary>
        /// <returns></returns>
        private static ICell SetCellValue(ICell cell, object value)
        {
            if (value.GetType() == typeof(decimal))
            {
                cell.SetCellValue((double)(decimal)value);
            }
            else if (value.GetType() == typeof(int))
            {
                cell.SetCellValue((double)(int)value);
            }
            else
            {
                cell.SetCellValue(value.ToString());
            }
            return cell;
        }
        #endregion

    }

    #region 模型
    #endregion
}
