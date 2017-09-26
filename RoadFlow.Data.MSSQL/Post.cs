using System;
using System.Collections.Generic;
using RoadFlow.Data.Model;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using RoadFlow.Data.Interface;

namespace RoadFlow.Data.MSSQL
{
    public class Post : IPost
    {
        private DBHelper dbHelper = new DBHelper();

        public Post()
        {
        }

        public int AddPost(PostModel post)
        {
            //string sql = "insert into Post (Title,Contents,AddTime,AddUserId,AddUserName, Type,Acreage,Price,Adresse,Mobile,IsValid) " +
            //             "values('" + post.Title + "','" + post.Contents + "',GETDATE(),'" + post.AddUserId + "','" + post.AddUserName + "','" + post.Type + "','" + post.Acreage + "','" + post.Price + "','" + post.Adresse + "','" + post.Mobile + "','" + post.IsValid + "')";
            string sql = "insert into Post (Title,Contents,AddTime,AddUserId, Type,Acreage,Adresse,Mobile,IsValid) " +
                         "values('" + post.Title + "','" + post.Contents + "',GETDATE(),'" + post.AddUserId + "','" + post.Type + "','" + post.Acreage + "','" + post.Adresse + "','" + post.Mobile + "','" + post.IsValid + "')";

            SqlParameter[] para = new SqlParameter[]
            {
                new SqlParameter("@Title", SqlDbType.NVarChar, 100) {Value = post.Title},
                new SqlParameter("@Contents", SqlDbType.VarChar) {Value = post.Contents},
                new SqlParameter("@AddUserId", SqlDbType.UniqueIdentifier) {Value = post.AddUserId}
            };

            return dbHelper.Execute(sql);
        }

        public DataTable GetPostDataPage(out string pager, string query = "", int size = 15, int number = 1, string title = "", string wher = "")
        {
            StringBuilder where = new StringBuilder();
            List<SqlParameter> parList = new List<SqlParameter>();

            where.Append(" Status = 0 ");
            if (!title.IsNullOrEmpty())
            {
                where.Append("AND Title like '%" + title + "%'");
            }

            long count;
            string fileList = " * ";
            string sql = dbHelper.GetPaerSql("Post", fileList, wher + where.ToString(), "Id DESC", size, number, out count, parList.ToArray());
            //pager = RoadFlow.Utility.Tools.GetPagerHtml(count, size, number, query);
            pager = RoadFlow.Utility.New.Tools.GetPagerHtml(count, size, number);
            return dbHelper.GetDataTable(sql, parList.ToArray());
        }

        public PostModel GetPostModel(int id)
        {
            string sql = "select * from Post where Id=@id";
            SqlParameter[] para=new SqlParameter[]
            {
                new SqlParameter("@id",SqlDbType.Int,4){Value = id}
            };

            PostModel model=new PostModel();
            DataTable dr = dbHelper.GetDataTable(sql, para);
            model = RoadFlow.Utility.ModelHandler<PostModel>.FillModel(dr.Rows[0]);
            return model;
        }

        public int DelPost(int id)
        {
            string sql = "update Post set Status=1 where Id= " + id;

            return dbHelper.Execute(sql);
        }

        public int UpdatePost(PostModel post)
        {
            //string sql = "update Post set Title='" + post.Title + "',Contents='" + post.Contents + "',AddTime=GETDATE(),Type='" + post.Type + "',Acreage='" + post.Acreage + "',Price='" + post.Price + "',Adresse='" + post.Adresse + "',Mobile='" + post.Mobile + "',IsValid='" + post.IsValid + "' where Id=" + post.Id;
            string sql = "update Post set Title='" + post.Title + "',Contents='" + post.Contents + "',AddTime=GETDATE(),Type='" + post.Type + "',Acreage='" + post.Acreage + "',Adresse='" + post.Adresse + "',Mobile='" + post.Mobile + "',IsValid='" + post.IsValid + "' where Id=" + post.Id;

            return dbHelper.Execute(sql);
        }

    }
}