using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace RoadFlow.Data.Model
{
    public class ArticleModel
    {
        [DisplayName("ID")]
        public Guid? Id { get; set; }
        [DisplayName("标题")]
        public string Title { get; set; } //文章标题
        [DisplayName("文章类型")]
        public Guid? Type { get; set; } //类型ID
        [DisplayName("简介")]
        public string BriefIntroduction { get; set; }//简介
        [DisplayName("文章内容")]
        public string Content { get; set; } //文章内容
        [DisplayName("发布日期")]
        public DateTime? PublishTime { get; set; } //发布日期
        [DisplayName("创建日期")]
        public DateTime? CreateTime { get; set; } //创建日期
        [DisplayName("状态")]
        public int? Status { get; set; }  //状态
    }
}
