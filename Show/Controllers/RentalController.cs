using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using manage.Models;
using Show.Models;
using System.Data.Entity.SqlServer;
using System.Text;
using System.Data.Entity;

namespace Show.Controllers
{
    /// <summary>
    /// 楼宇租售
    /// </summary>
    public class RentalController : Controller
    {
        private Context db = new Context();
        private int pageSize = 8;

        #region Action
        public ActionResult Index(int? pageIndex)
        {
            return View(InitIndex());
        }
        /// <summary>
        /// 楼宇详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Details(Guid id)
        {
            return View(InitDetails(id));
        }
        #endregion

        #region Init
        public BuildingRentalViewModel InitIndex()
        {
            ////所属街道
            var ssjdParentID = db.Dictionarys.FirstOrDefault(p => p.Title == "所属街道").ID;
            var importantParentID = db.Dictionarys.FirstOrDefault(p => p.Code == "IsImportant").ID;
            var jsjdParentID = db.Dictionarys.FirstOrDefault(p => p.Code == "JSJD").ID;

            BuildingRentalViewModel viewModel = new BuildingRentalViewModel
            {
                SSJD = db.Dictionarys.Where(p => p.ParentID == ssjdParentID),
                IsImportant = db.Dictionarys.Where(p=>p.ParentID == importantParentID),
                JSJD = db.Dictionarys.Where(p => p.ParentID == jsjdParentID),
            };
            return viewModel;
        }
        public BuildingDetailViewModel InitDetails(Guid id)
        {
            BuildingDetailViewModel viewModel = new BuildingDetailViewModel();
            viewModel.Building = db.BuildingsAndBuildingMonthInfo.Include(p => p.SYZJ).Include(p => p.SSJDS).Include(p => p.HouseIDS).FirstOrDefault(p => p.ID == id);
            viewModel.HotBuilding = db.BuildingsAndBuildingMonthInfo.Include(p => p.SYZJ).Take(3).OrderByDescending(p => p.CreateTime);
            return viewModel;
        }
        #endregion

        #region 异步
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="pageIndex">页下标</param>
        /// <param name="rentalType">租售类型</param>
        /// <param name="ssjd">所属街道</param>
        /// <param name="zmj">总面积</param>
        /// <param name="rentalMoney">租售价格</param>
        /// <param name="name">楼宇名称</param>
        /// <returns></returns>
        public JsonResult GetJsonList(int pageIndex, string rentalType, string ssjd, string zmj, string rentalMoney, string name,Guid isImportant,Guid jsjd)
        {
            StringBuilder html = new StringBuilder();
            IQueryable<BuildingModel> list = db.BuildingsAndBuildingMonthInfo.Where(p=>p.IsImportant== isImportant && p.JSJD == jsjd && p.Status == Status.Normal).Include(p => p.SWZJ).Include(p => p.SYZJ);
            #region where
            //所属街道
            if (!string.IsNullOrWhiteSpace(ssjd))
            {
                Guid ssjdGuid = Guid.Parse(ssjd);
                list = list.Where(p => p.SSJD == ssjdGuid);
            }
            //总面积
            if (!string.IsNullOrWhiteSpace(zmj))
            {
                int begin = zmj.Split(',')[0].Convert<int>(0);
                if (begin != 0)
                {
                    list = list.Where(p => p.ZJZMJ > begin);
                }
                int end = zmj.Split(',')[1].Convert<int>(0);
                if (end != 0)
                {
                    list = list.Where(p => p.ZJZMJ < end);
                }
            }
            //租售金额
            if (!string.IsNullOrWhiteSpace(rentalMoney))
            {
                //出租
                string zjStr = ControllersExtentstions.GetAreaIds(rentalMoney);
                if (string.IsNullOrWhiteSpace(zjStr))
                {
                    List<Guid> ids = new List<Guid>();
                    foreach (var item in zjStr.Split(','))
                    {
                        ids.Add(Guid.Parse(item));
                    }
                    list = list.Where(t => ids.Contains(t.SY_ZJ.Value) || ids.Contains(t.SW_ZJ.Value));
                }
                //出售
                int begin = rentalMoney.Split(',')[0].Convert<int>(0);
                if (begin != 0)
                {
                    list = list.Where(p => p.SY_XSJJ > begin || p.SW_XSJJ > begin);
                }
                int end = rentalMoney.Split(',')[1].Convert<int>(0);
                if (end != 0)
                {
                    list = list.Where(p => p.SY_XSJJ < end || p.SW_XSJJ < end);
                }
            }
            //楼宇名称
            if (!string.IsNullOrWhiteSpace(name))
            {
                list = list.Where(p => p.Name.Contains(name));
            }
            #endregion
            int totalCount = list.Count();
            string pager = MyExtensions.GetPagingHtml(totalCount, pageSize, pageIndex);
            list = list.OrderByDescending(p => p.CreateTime).Skip(pageSize * (pageIndex - 1)).Take(pageSize);
            return Json(new { success = true, html = GetHtml(list, rentalType).ToString(), pager = pager, totalCount = totalCount, }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 公共方法

        public StringBuilder GetHtml(IQueryable<BuildingModel> list, string rentalType)
        {
            StringBuilder html = new StringBuilder();
            int count = 0;
            foreach (var item in list)
            {
                count++;
                if (count % 2 == 1)
                {
                    html.Append(" <li class=\"clearfix\">");
                    html.Append("<div class=\"fl clearfix pr\">");
                    // html.Append(" <li class=\"fl clearfix pr\" style=\"margin-right:0;float:right;\">");
                }
                else
                {
                    html.Append("<div class=\"fr clearfix pr\">");
                }

                
                html.AppendFormat(" <a href=\"{0}\"><img class=\"fl\" src=\"{1}\">", Url.Action("Details", new { id = item.ID }), ControllersExtentstions.GetFullPath(string.IsNullOrWhiteSpace(item.XGT) ? "" : item.XGT.Split('|')[0]));//左侧图
                //中间部分
                html.Append(" <div class=\"fl\">");
                html.AppendFormat(" <h4>{0}</h4>", item.Name);
                html.AppendFormat("<div>地址：<span>{0}</span> </div>", item.LYXXDZ);
                html.AppendFormat(" <div>招商电话：<span>{0}</span></div>", item.ChinaMerchantsTel);
                html.AppendFormat("<div>面积：<span>{0}㎡</span>  </div>", item.ZJZMJ);
                html.AppendFormat(" <div>发布日期：<span>{0}</span></div>", ((DateTime)item.CreateTime).ToString("yyyy-MM-dd"));
                html.Append("</div>");
                //右侧部分
                if (!string.IsNullOrWhiteSpace(rentalType))
                {
                    if (rentalType == "出租")
                    {
                        html.AppendFormat(" <div class=\"pa pa1\"> <span class=\"txt1\">商业租金</span><span class=\"money\">{0}</span>元/㎡ · 月</div>", item.SYZJ==null?"": item.SYZJ.Title);
                        html.AppendFormat(" <div class=\"pa pa2\"><span class=\"txt1\">商务租金</span><span class=\"money\">{0}</span>元/㎡ · 月 </div>", item.SWZJ == null ? "" : item.SWZJ.Title);
                    }
                    else if (rentalType == "出售")
                    {
                        html.AppendFormat(" <div class=\"pa pa3\"><span class=\"txt1\">商业价格</span><span class=\"money\">{0}</span>元/㎡ · 月</div>", item.SY_XSJJ == null ? 0 : item.SY_XSJJ);
                        html.AppendFormat("<div class=\"pa pa4\"><span class=\"txt1\">商务价格</span><span class=\"money\">{0}</span>元/㎡ · 月</div>", item.SW_XSJJ == null ? 0 : item.SW_XSJJ);
                    }
                    else  //全部
                    {
                        html.AppendFormat(" <div class=\"pa pa1\"> <span class=\"txt1\">商业租金</span><span class=\"money\">{0}</span>元/㎡ · 月</div>", item.SYZJ == null ? "" : item.SYZJ.Title);
                        html.AppendFormat(" <div class=\"pa pa2\"><span class=\"txt1\">商务租金</span><span class=\"money\">{0}</span>元/㎡ · 月 </div>", item.SWZJ == null ? "" : item.SWZJ.Title);
                        html.AppendFormat(" <div class=\"pa pa3\"><span class=\"txt1\">商业价格</span><span class=\"money\">{0}</span>元/㎡ · 月</div>", item.SY_XSJJ == null ? 0 : item.SY_XSJJ);
                        html.AppendFormat("<div class=\"pa pa4\"><span class=\"txt1\">商务价格</span><span class=\"money\">{0}</span>元/㎡ · 月</div>", item.SW_XSJJ == null ? 0 : item.SW_XSJJ);
                    }
                }

                html.Append(" </a>");
                html.Append("</div>");

                if (count%2==0)
                {
                    html.Append("</li>");
                }
            }
            return html;
        }
        #endregion
    }
}