using RoadFlow.Data.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace RoadFlow.Data.MSSQL
{
    public class TableAttribute : RoadFlow.Data.Interface.ITableAttribute
    {
        private DBHelper dbHelper = new DBHelper();
       public List<Column> GetTableAttr(string table)
       {
           string sql= string.Format(@"SELECT a.name as Value, b.value Name,d.name colType  from sys.syscolumns a LEFT JOIN sys.extended_properties b on a.id=b.major_id AND a.colid=b.minor_id AND b.name='MS_Description' join sysobjects c  on c.id=a.id and c.xtype='U' join systypes d on a.xtype=d.xusertype  WHERE object_id('{0}')=a.id ORDER BY a.colid", table);
             SqlDataReader dataReader = dbHelper.GetDataReader(sql);
             List<Column> List = new List<Column>();
               while (dataReader.Read())
               {
                   List.Add(new Column()
                   {
                       Name = dataReader.GetValue(dataReader.GetOrdinal("Name")).ToString(),
                       Value = dataReader.GetValue(dataReader.GetOrdinal("Value")).ToString(),
                       ColType = dataReader.GetValue(dataReader.GetOrdinal("ColType")).ToString()
                   });
               }
           return List;
       }

    }
}
