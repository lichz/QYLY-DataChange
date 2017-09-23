using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Show.Models
{
    /// <summary>
    /// 楼宇展示
    /// </summary>
    public class BuildingIndexViewModel
    {
        public IQueryable<BuildingModel> Buildings;
        /// <summary>
        /// 所属街道
        /// </summary>
        public IQueryable<DictionaryModel> SSJD;
        /// <summary>
        /// 已建楼宇
        /// </summary>
        public Guid BuildedID;
        /// <summary>
        /// 在建楼宇
        /// </summary>
        public Guid BuildingID;
        /// <summary>
        /// 重点楼宇
        /// </summary>
        public Guid ImportantID;
        /// <summary>
        /// 其他楼宇
        /// </summary>
        public Guid OtherID;
        /// <summary>
        /// 总个数
        /// </summary>
        public int BuildingCount { get; set; }
        /// <summary>
        /// 街道ID
        /// </summary>
        public string SSJDName;
        /// <summary>
        /// 楼宇名称
        /// </summary>
        public string Name;
        /// <summary>
        /// 分页
        /// </summary>
        public string Pager;
    }
    /// <summary>
    /// 楼宇租售
    /// </summary>
    public class BuildingRentalViewModel
    {
        /// <summary>
        /// 所属街道
        /// </summary>
        public IQueryable<DictionaryModel> SSJD;
        /// <summary>
        /// 是否重点楼宇
        /// </summary>
        public IQueryable<DictionaryModel> IsImportant;
        /// <summary>
        /// 建设阶段
        /// </summary>
        public IQueryable<DictionaryModel> JSJD;

        //public IQueryable<BuildingModel> Buildings;
        ///// <summary>
        ///// 总个数
        ///// </summary>
        //public int BuildingCount { get; set; }
        ///// <summary>
        ///// 分页
        ///// </summary>
        //public string Pager;
    }
    /// <summary>
    /// 楼宇详情
    /// </summary>
    public class BuildingDetailViewModel
    {
        public BuildingModel Building;
        public IQueryable<BuildingModel> HotBuilding;
    }
}