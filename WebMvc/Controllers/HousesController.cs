using RoadFlow.Web.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Net;
using System.Web.Mvc;

namespace WebMvc.Controllers
{
    //楼盘
    public class HousesController : MyController
    {
        private RoadFlow.Platform.DictionaryBLL bll = new RoadFlow.Platform.DictionaryBLL();
        private RoadFlow.Platform.ElementOrganizeBLL elementOrganizeBLL = new RoadFlow.Platform.ElementOrganizeBLL();
        //组织机构权限。（有哪些楼盘的权限）
        public ActionResult Index(Guid? id,RoadFlow.Data.Model.ElementOrganizeType? type)
        {
            if(id==null||type==null){
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            return View(IndexInit(id.Value,type.Value));
        }

        [HttpPost]
        [ActionName("Index")]
        public ActionResult IndexPost(HousesIndexViewModel model) {
            if(!ModelState.IsValid){
                ModelState.AddModelError("","验证失败。");
                return View(IndexInit(model.OrganizeID, model.Type));
            }
            elementOrganizeBLL.ModifyByTypeAndOrganizeID(model.Type,model.OrganizeID,model.Check);
            ViewBag.Success = WebMvc.Common.Tools.CloseReloadWin("保存成功。");
            return View(IndexInit(model.OrganizeID, model.Type));
        }

        private HousesIndexViewModel IndexInit(Guid id, RoadFlow.Data.Model.ElementOrganizeType type) {
            HousesIndexViewModel viewModel = new HousesIndexViewModel();
            viewModel.OrganizeID = id;
            viewModel.Type = type;
            //viewModel.List = bll.GetListByCode("LPMC"); //获取所有楼盘
            RoadFlow.Platform.OrganizeBLL organizeBLL = new RoadFlow.Platform.OrganizeBLL();
            viewModel.Name = organizeBLL.GetByID(id).Name;//当前设置机构名称
            //获取当前用户组织权限用于显示选中
            List<Guid> list = new List<Guid>();
            foreach (DataRow dr in elementOrganizeBLL.GetByTypeAndOrganizeID(type, id).Rows) {
                list.Add((Guid)dr["ElementID"]);
            }
            viewModel.Check = list;
            return viewModel;
        }



    }
}
