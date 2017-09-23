// // 
// // ==============================================================================
// // 
// // Version: 1.0
// // Compiler: Visual Studio 2013
// // Created: 2016-03-22 14:47
// // Updated: 2016-03-22 14:47
// //  
// // Author: Wu bo
// // Company: World
// // 
// // Project: RoadFlow.Data.Interface
// // Filename: IPost.cs
// // Description: 
// // 
// // ==============================================================================
using RoadFlow.Data.Model;
using System;
using System.Data;
namespace RoadFlow.Data.Interface
{
    public interface IPost
    {
        int AddPost(PostModel post);

        DataTable GetPostDataPage(out string pager, string query = "", int size = 15, int number = 1, string title = "",
            string wher = "");

        PostModel GetPostModel(int id);

        int DelPost(int id);

        int UpdatePost(PostModel post);
    }
}