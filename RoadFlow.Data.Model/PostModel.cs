// // 
// // ==============================================================================
// // 
// // Version: 1.0
// // Compiler: Visual Studio 2013
// // Created: 2016-03-22 11:42
// // Updated: 2016-03-22 11:42
// //  
// // Author: Wu bo
// // Company: World
// // 
// // Project: RoadFlow.Data.Model
// // Filename: Post.cs
// // Description: 
// // 
// // ==============================================================================

using System;
using System.Dynamic;

namespace RoadFlow.Data.Model
{
    public class PostModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Contents { get; set; }
        public DateTime AddTime { get; set; }
        public Guid AddUserId { get; set; }
        public string AddUserName { get; set; }
        public string Type { get;set;}
        public string Acreage { get; set; }
        //public decimal Price { get; set; }
        public string Adresse { get; set; }
        public string Mobile { get; set; }
        public int Status { get; set; }
        public int IsValid { get; set; }
    }
}