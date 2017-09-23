using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using RoadFlow.Utility;
using System.Data;

namespace RoadFlow.Data.MSSQL
{
    public class QueryDesign : RoadFlow.Data.Interface.IQueryDesign
    {
       private DBHelper dbHelper = new DBHelper();

        /// <summary>
        /// 读取所有的配置记录
        /// </summary>
        /// <returns></returns>
       public List<RoadFlow.Data.Model.QueryDesign> GetAll()
       {
           string sql = "SELECT * FROM QueryDesign ";
           SqlDataReader dataReader = dbHelper.GetDataReader(sql);
           List<RoadFlow.Data.Model.QueryDesign> List = dataReader.ReaderToList<RoadFlow.Data.Model.QueryDesign>();
           dataReader.Close();
           return List;
       }
       /// <summary>
       /// 根据主键查询一条记录
       /// </summary>
       public RoadFlow.Data.Model.QueryDesign Get(Guid id)
       {
           string sql = "SELECT * FROM QueryDesign WHERE ID=@ID";
           SqlParameter[] parameters = new SqlParameter[]{
				new SqlParameter("@ID", SqlDbType.UniqueIdentifier){ Value = id }
			};
           SqlDataReader dataReader = dbHelper.GetDataReader(sql, parameters);
           List<RoadFlow.Data.Model.QueryDesign> List = dataReader.ReaderToList<RoadFlow.Data.Model.QueryDesign>();
           dataReader.Close();
           return List.Count > 0 ? List[0] : null;
       }
        /// <summary>
        /// 根据配置名称查询配置记录
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
       public RoadFlow.Data.Model.QueryDesign Get(string name,Guid ryid)
       {
           string sql = "SELECT * FROM QueryDesign WHERE Name=@Name and ( CreateUserID = @CreateUserID or Status = 0 ) order by Status desc ";
           SqlParameter[] parameters = new SqlParameter[]{
				new SqlParameter("@Name", SqlDbType.NVarChar,50){ Value = name },
				new SqlParameter("@CreateUserID", SqlDbType.UniqueIdentifier){ Value = ryid }
			};
           SqlDataReader dataReader = dbHelper.GetDataReader(sql, parameters);
           List<RoadFlow.Data.Model.QueryDesign> List = dataReader.ReaderToList<RoadFlow.Data.Model.QueryDesign>();
           dataReader.Close();
           return List.Count > 0 ? List[0] : null;
       }
        /// <summary>
        /// 更新查询配置数据记录
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
       public int Update(RoadFlow.Data.Model.QueryDesign model)
       {
           string sql = @"UPDATE QueryDesign SET 
				Name=@Name,TableName=@TableName,CreateUserID=@CreateUserID,CreateUserName=@CreateUserName,ConnectionID=@ConnectionID,SearchJson=@SearchJson,DisplayItem=@DisplayItem,LastModifyTime=@LastModifyTime,Status=@Status
				WHERE ID=@ID";
           SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter("@ID", SqlDbType.UniqueIdentifier,-1){ Value = model.ID },
				new SqlParameter("@Name", SqlDbType.NVarChar, 50){ Value = model.Name },
                new SqlParameter("@TableName", SqlDbType.NVarChar, 50){ Value = model.TableName },
                new SqlParameter("@CreateUserID", SqlDbType.UniqueIdentifier,-1){ Value = model.CreateUserID },
                new SqlParameter("@CreateUserName", SqlDbType.NVarChar,50){ Value = model.CreateUserName },
                new SqlParameter("@ConnectionID", SqlDbType.UniqueIdentifier, 50){ Value = model.ConnectionID },
                new SqlParameter("@SearchJson", SqlDbType.NVarChar,10000){ Value = model.SearchJson },
                new SqlParameter("@DisplayItem", SqlDbType.NVarChar,10000){ Value = model.DisplayItem.IsNullOrEmpty()==true? "":model.DisplayItem },
                new SqlParameter("@LastModifyTime", SqlDbType.DateTime){ Value = DateTime.Now },
                new SqlParameter("@Status", SqlDbType.Int,4){ Value = model.Status }
			};
           return dbHelper.Execute(sql, parameters);
       }
       /// <summary>
       /// 添加配置记录
       /// </summary>
       /// <param name="model">RoadFlow.Data.Model.WorkFlowButtons实体类</param>
       /// <returns>操作所影响的行数</returns>
       public int Add(RoadFlow.Data.Model.QueryDesign model) {
           string sql = @"INSERT INTO QueryDesign([ID],[Name],[TableName],[CreateUserID],[CreateUserName],[ConnectionID],[SearchJson],[DisplayItem],[CreateTime],[LastModifyTime],[Status]) 
				VALUES(@ID,@Name,@TableName,@CreateUserID,@CreateUserName,@ConnectionID,@SearchJson,@DisplayItem,@CreateTime,@LastModifyTime,@Status)";
           List<SqlParameter> parameters = new List<SqlParameter>();
           parameters.Add(new SqlParameter("@ID", SqlDbType.UniqueIdentifier, -1) { Value = model.ID });
           parameters.Add(new SqlParameter("@TableName", SqlDbType.NVarChar, 50) { Value = model.TableName });
           parameters.Add(new SqlParameter("@Name", SqlDbType.NVarChar, 50) { Value = model.Name });
           parameters.Add(new SqlParameter("@CreateUserID", SqlDbType.UniqueIdentifier, -1) { Value = model.CreateUserID });
           parameters.Add(new SqlParameter("@CreateUserName", SqlDbType.NVarChar, 50) { Value = model.CreateUserName });
           parameters.Add(new SqlParameter("@ConnectionID", SqlDbType.UniqueIdentifier, -1) { Value = model.ConnectionID });
           parameters.Add(new SqlParameter("@SearchJson", SqlDbType.NVarChar, 5000) { Value = model.SearchJson });
           if (model.DisplayItem.IsNullOrEmpty()) {
               parameters.Add(new SqlParameter("@DisplayItem", SqlDbType.NVarChar, 10000) { Value = DBNull.Value });
           } else {
               parameters.Add(new SqlParameter("@DisplayItem", SqlDbType.NVarChar, 10000) { Value = model.DisplayItem });
           }
           parameters.Add(new SqlParameter("@CreateTime", SqlDbType.DateTime) { Value = DateTime.Now });
           parameters.Add(new SqlParameter("@LastModifyTime", SqlDbType.DateTime) { Value = DateTime.Now });
           parameters.Add(new SqlParameter("@Status", SqlDbType.Int, -1) { Value = model.Status });

           return dbHelper.Execute(sql, parameters.ToArray());
       }
       /// <summary>
       /// 删除配置记录
       /// </summary>
       public int Delete(Guid id)
       {
           string sql = "delete QueryDesign WHERE ID=@ID";
           SqlParameter[] parameters = new SqlParameter[]{
				new SqlParameter("@ID", SqlDbType.UniqueIdentifier){ Value = id }
			};
           return dbHelper.Execute(sql, parameters);
       }
    
    }
}
