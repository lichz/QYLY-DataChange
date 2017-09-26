using System;
using System.Collections.Generic;
using RoadFlow.Data.Model;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using RoadFlow.Data.Interface;

namespace RoadFlow.Data.MSSQL {
    public class SMS : ISMS {
        private DBHelper dbHelper = new DBHelper();

        public SMS() {
        }

        public int Add(SMSModel model) {
            //string sql = "insert into [SMS] ([Content],[CreateTime],[SendUser],[SendUserName], [SendTime],[SendTo]) " +
            //             "values(@Content,GETDATE(),@SendUser,@SendUserName,@SendTime,@SendTo)";
            string sql = "insert into [SMS] ([Content],[CreateTime],[SendUser],[SendUserName],[SendTo],[IsSended]) " +
                         "values(@Content,GETDATE(),@SendUser,@SendUserName,@SendTo,1)";
            SqlParameter[] para = new SqlParameter[]
            {
                new SqlParameter("@Content", SqlDbType.NVarChar) {Value = model.Content},
                new SqlParameter("@SendUser", SqlDbType.UniqueIdentifier) {Value = model.SendUser},
                new SqlParameter("@SendUserName", SqlDbType.NChar, 50) {Value = model.SendUserName},
                //new SqlParameter("@SendTime", SqlDbType.DateTime) {Value = model.SendTime},
                new SqlParameter("@SendTo", SqlDbType.VarChar) {Value = model.SendTo}
            };
            return dbHelper.Execute(sql,para);
        }

        public DataTable GetDataPage(out string pager, string query = "", int size = 15, int number = 1, string title = "", string wher = "") {
            StringBuilder where = new StringBuilder();
            List<SqlParameter> parList = new List<SqlParameter>();

            where.Append(" Status = 0 ");
            //if (!title.IsNullOrEmpty()) {
            //    where.Append("AND Title like '%" + title + "%'");
            //}

            long count;
            string fileList = " * ";
            string sql = dbHelper.GetPaerSql("[SMS]", fileList, wher + where.ToString(), "[CreateTime] DESC", size, number, out count, parList.ToArray());
            //pager = RoadFlow.Utility.Tools.GetPagerHtml(count, size, number, query);
            pager = RoadFlow.Utility.New.Tools.GetPagerHtml(count, size, number);
            return dbHelper.GetDataTable(sql, parList.ToArray());
        }

        public SMSModel GetModel(string id) {
            string sql = "select * from [SMS] where Id=@id";
            SqlParameter[] para = new SqlParameter[]
            {
                new SqlParameter("@id",SqlDbType.UniqueIdentifier,36){Value = id}
            };

            SMSModel model = new SMSModel();
            DataTable dr = dbHelper.GetDataTable(sql, para);
            model = RoadFlow.Utility.ModelHandler<SMSModel>.FillModel(dr.Rows[0]);
            return model;
        }

        public SMSModel GetModelBySendUser(string id) {
            string sql = "select * from [SMS] where [SendUser]=@id and [Status]=1";
            SqlParameter[] para = new SqlParameter[]
            {
                new SqlParameter("@id",SqlDbType.VarChar,36){Value = id}
            };

            SMSModel model = new SMSModel();
            DataTable dr = dbHelper.GetDataTable(sql, para);
            if(dr.Rows.Count==0){
                return null;
            }
            model = RoadFlow.Utility.ModelHandler<SMSModel>.FillModel(dr.Rows[0]);
            return model;
        }

        public int Del(string id) {
            string sql = "update [SMS] set [Status]=1 where [Id]= " + id;
            return dbHelper.Execute(sql);
        }

        public int Update(SMSModel model) {
            string sql = "update [SMS] set [Content]=@Content,[SendTime]=@SendTime";
            SqlParameter[] para = new SqlParameter[]
            {
                new SqlParameter("@Content",SqlDbType.NChar,500){Value = model.Content},
                new SqlParameter("@SendTime",SqlDbType.DateTime){Value = model.SendTime}
            };
            return dbHelper.Execute(sql,para);
        }

    }
}