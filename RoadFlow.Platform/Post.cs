// // 
// // ==============================================================================
// // 
// // Version: 1.0
// // Compiler: Visual Studio 2013
// // Created: 2016-03-22 14:53
// // Updated: 2016-03-22 14:53
// //  
// // Author: Wu bo
// // Company: World
// // 
// // Project: RoadFlow.Platform
// // Filename: Post.cs
// // Description: 
// // 
// // ==============================================================================

using RoadFlow.Data.Factory;
using RoadFlow.Data.Interface;
using RoadFlow.Data.Model;
using System;
using System.Data;
namespace RoadFlow.Platform
{
    public class Post
    {
        private IPost ipost;

        public Post()
        {
            ipost = Factory.GetPost();
        }

        public int AddPost(PostModel post)
        {
            return ipost.AddPost(post);
        }

        public DataTable GetPostDataPage(out string pager, string query = "", int size = 15, int number = 1,
            string title = "", string wher = "")
        {
            return ipost.GetPostDataPage(out  pager, query, RoadFlow.Utility.Tools.GetPageSize(), RoadFlow.Utility.Tools.GetPageNumber(), title, wher);
        }

        public PostModel GetPostModel(int id)
        {
            return ipost.GetPostModel(id);
        }

        public int DelPost(int id)
        {
            return ipost.DelPost(id);
        }

        public int UpdatePost(PostModel post)
        {
            return ipost.UpdatePost(post);
        }

    }
}