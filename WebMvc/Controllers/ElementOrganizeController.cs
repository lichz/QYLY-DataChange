using RoadFlow.Data.Model;
using RoadFlow.Utility;
using RoadFlow.Web.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Transactions;
using System.Web;
using System.Web.Mvc;

namespace WebMvc.Controllers
{
    /// <summary>
    /// 街道税收、报送单位的（楼栋权限，企业变更、税收权限）
    /// </summary>
    public class ElementOrganizeController : Controller {
        public RoadFlow.Platform.ElementOrganizeBLL BLL { get; private set; }
        public RoadFlow.Platform.OrganizeBLL OrganizeBLL { get; private set; }
        public ElementOrganizeController() { 
            BLL = new RoadFlow.Platform.ElementOrganizeBLL();
            OrganizeBLL = new RoadFlow.Platform.OrganizeBLL();
        }


        //街道直报关联
        public ActionResult Index() { 
            ElementOrganizeIndexViewModel viewModel = new ElementOrganizeIndexViewModel();
            DataTable dt = OrganizeBLL.GetAllStreet(); //所有街道
            viewModel.List = dt;

            viewModel.Permission = GetPermission(dt, ElementOrganizeType.ToStreet);
            return View(viewModel);
        }

        //报送单位权限
        public ActionResult Submit() {
            ElementOrganizeIndexViewModel viewModel = new ElementOrganizeIndexViewModel();
            DataTable dt = OrganizeBLL.GetAllSubmit(); 
            viewModel.List = dt;
           
            viewModel.Permission = GetPermission(dt, ElementOrganizeType.BuildingModify);
            return View(viewModel);
        }

        /// <summary>
        /// 查询拥有权限。
        /// </summary>
        /// <returns></returns>
        private Dictionary<Guid, string> GetPermission(DataTable dt,ElementOrganizeType type)
        {
            Dictionary<Guid, string> permission = new Dictionary<Guid, string>(); //权限列表
            List<RoadFlow.Data.Model.DictionaryModel> dictionary = new RoadFlow.Platform.DictionaryBLL().GetListByCode("LPMC").ToList<RoadFlow.Data.Model.DictionaryModel>();
            List<string> list = new List<string>();
            foreach (DataRow dr in dt.Rows)
            {
                list.Clear();
                foreach (DataRow item in BLL.GetByTypeAndOrganizeID(type, (Guid)dr["ID"]).Rows)
                {
                    list.Add(dictionary.Find(p => p.ID == (Guid)item["ElementID"]).Title);
                }
                permission.Add((Guid)dr["ID"], string.Join(",", list));
            }

            return permission;
        }
    }

}

