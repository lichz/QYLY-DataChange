using RoadFlow.Data.Model;
using RoadFlow.Utility;
using RoadFlow.Web.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Transactions;
using System.Web;
using System.Web.Mvc;

namespace WebMvc.Controllers
{
    /// <summary>
    /// 楼宇
    /// </summary>
    public class BuildingsController : Controller
    {
        public RoadFlow.Platform.BuildingsAndBuildingMonthInfoBLL BuildingsAndBuildingMonthInfoBLL { get; private set; }
        public RoadFlow.Platform.TableAttribute TableAttribute { get; private set; }
        public RoadFlow.Platform.BuildingsDataBLL BLL { get; private set; }
        public RoadFlow.Platform.DictionaryBLL DictionaryBLL { get; private set; }
        public int PageSize { get; private set; }

        public BuildingsController()
        {
            PageSize = 13;
            BuildingsAndBuildingMonthInfoBLL = new RoadFlow.Platform.BuildingsAndBuildingMonthInfoBLL();
            TableAttribute = new RoadFlow.Platform.TableAttribute();
            BLL = new RoadFlow.Platform.BuildingsDataBLL();
            DictionaryBLL = new RoadFlow.Platform.DictionaryBLL();
        }

        #region Action
        #region 视图
        //楼栋列表
        public ActionResult Index()
        {
            return View(BuildingInit(0));
        }

        [HttpPost]
        [ActionName("Index")]
        public ActionResult IndexPost()
        {
            return View(BuildingInit(0));
        }


        //街道楼栋列表
        public ActionResult Street()
        {
            return View(BuildingInit(1));
        }

        [HttpPost]
        [ActionName("Street")]
        public ActionResult StreetPost()
        {
            return View(BuildingInit(1));
        }

        //报送单位楼栋列表
        public ActionResult Submitted()
        {
            return View(BuildingInit(2));
        }

        [HttpPost]
        [ActionName("Submitted")]
        public ActionResult SubmittedPost()
        {
            return View(BuildingInit(2));
        }

        //楼宇详细信息
        public ActionResult Detail(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BuildingDetailViewModel viewModel = new BuildingDetailViewModel();
            viewModel.BuildingsAndBuildingMonthInfo = BuildingsAndBuildingMonthInfoBLL.Get(id.Value);
            viewModel.Enterprises = new RoadFlow.Platform.EnterpriseAndEnterpriseTaxBLL().GetAllByBuildingID(id.Value.ToString());
            viewModel.Dictionarys = DictionaryBLL.GetListAll();
            return View(viewModel);
        }

        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            return View(EditInit(id.Value));
        }

        [ActionName("Edit")]
        [HttpPost]
        public ActionResult EditPost(RoadFlow.Data.Model.BuildingsModel Buildings, RoadFlow.Data.Model.BuildingMonthInfoModel BuildingMonth, Guid modelID)
        {
            if (BuildingsAndBuildingMonthInfoBLL.ManageUpdate(Buildings, BuildingMonth, modelID) > 0)
            {
                ViewBag.Success = true;
            }
            return View(EditInit(modelID));
        }

        public ActionResult Delete(Guid id)
        {
            BuildingsAndBuildingMonthInfoBLL.ManageDelete(id);
            return Redirect(Url.Action("Index") + Request["query"]);
        }

        /// <summary>
        /// 企业管理
        /// </summary>
        /// <param name="buildingID"></param>
        /// <returns></returns>
        public ActionResult ManageEnterprise(Guid? buildingID)
        {
            if (buildingID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            return View(ManageEnterpriseInit(buildingID.Value));
        }

        /// <summary>
        /// 企业编辑
        /// </summary>
        /// <param name="enterpriseID"></param>
        /// <returns></returns>
        public ActionResult EditEnterprise(Guid? enterpriseID)
        {
            if (enterpriseID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            return View(EditEnterpriseInit(enterpriseID.Value));
        }

        /// <summary>
        /// 企业编辑
        /// </summary>
        /// <param name="enterpriseID"></param>
        /// <returns></returns>
        [HttpPost]
        [ActionName("EditEnterprise")]
        public ActionResult EditEnterprisePost(EnterpriseModel enterprise, Guid enterpriseID)
        {
            if (enterpriseID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (new RoadFlow.Platform.EnterpriseAndEnterpriseTaxBLL().ManageUpdate(enterprise, enterpriseID) > 0)
            {
                ViewBag.Success = true;
            }
            else
            {
                ModelState.AddModelError("", "更新失败。");
            }
            return View(EditEnterpriseInit(enterpriseID));
        }

        //[ActionName("EditEnterprise")]
        //[HttpPost]
        //public ActionResult EditEnterprisePost(Guid buildingID,List<RoadFlow.Data.Model.EnterpriseModel> add, List<RoadFlow.Data.Model.EnterpriseModel> update,string deleteIDs)
        //{
        //    List<string> deleteList = new List<string>();
        //    if(!string.IsNullOrWhiteSpace(deleteIDs))
        //    {
        //         deleteList = deleteIDs.Split(',').ToList();
        //    }
        //    if (new RoadFlow.Platform.EnterpriseAndEnterpriseTaxBLL().ManageUpdate(buildingID, add, update, deleteList) > 0)
        //    {
        //        ViewBag.Success = true;
        //    }
        //    return View(EditEnterpriseInit(buildingID));
        //}

        //绑定楼盘
        public ActionResult BindHouseID(Guid? buildingID)
        {
            if (buildingID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            return View(BindHouseIDInit(buildingID.Value));
        }

        [HttpPost]
        [ActionName("BindHouseID")]
        public ActionResult BindHouseIDPost(Guid HouseID, Guid buildingID)
        {
            if (new RoadFlow.Platform.BuildingsAndBuildingMonthInfoBLL().ManageBindHouseID(HouseID, buildingID) > 0)
            {
                ViewBag.Script = "alert('绑定成功!');new RoadUI.Window().reloadOpener();new RoadUI.Window().close();";
            }
            else
            {
                ViewBag.Script = "alert('绑定失败!');";
            }
            return View(BindHouseIDInit(buildingID));
        }
        #endregion
        #region 操作（如异步查询，导出等。）
        /// <summary>
        /// 直接导出
        /// </summary>
        /// <param name="name"></param>
        /// <param name="SSJD"></param>
        /// <param name="expwher"></param>
        public void Export()
        {
            //获取导出列表
            Dictionary<KeyValuePair<string, RoadFlow.Data.Model.SQLFilterType>, object> where = IndexGetMoreWhere();//筛选条件
            #region 固定where
            if (!string.IsNullOrWhiteSpace(Request["Name"]))
            {
                where.Add(new KeyValuePair<string, RoadFlow.Data.Model.SQLFilterType>("Name", RoadFlow.Data.Model.SQLFilterType.CHARINDEX), Request["Name"]);
            }
            if (!string.IsNullOrWhiteSpace(Request["SSJD"]))
            {
                where.Add(new KeyValuePair<string, RoadFlow.Data.Model.SQLFilterType>("SSJD", RoadFlow.Data.Model.SQLFilterType.EQUAL), Request["SSJD"]);
            }
            #endregion
            DataTable dt = GetListByType(Request["exportType"], where);
            //导出列
            Dictionary<string, string> dicionary = new Dictionary<string, string>();
            List<ColItem> colItemList = GetColItemList();
            foreach (var item in colItemList)
            {
                if (item.chk == true)
                {
                    dicionary.Add(item.id, item.value);
                }
            }

            ExportExcel.Export(dt.ExportExcelPre(dicionary), "楼栋列表");
        }

        //重点楼宇设置
        public ActionResult ImportantBuilding(Guid id, Guid importantID)
        {
            //RoadFlow.Data.Model.BuildingsModel model = new BuildingsModel();
            //model.IsImportant = importantID;
            //BLL
            return View();
        }

        /// <summary>
        /// 管理员企业添加
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public JsonResult ManageCreateEnterprise(RoadFlow.Data.Model.EnterpriseModel model)
        {
            RoadFlow.Platform.EnterpriseAndEnterpriseTaxBLL bll = new RoadFlow.Platform.EnterpriseAndEnterpriseTaxBLL();
            Guid id = Guid.NewGuid();
            model.Name = HttpUtility.UrlDecode(model.Name);
            model.ZCD = HttpUtility.UrlDecode(model.ZCD);
            model.CSJJD = HttpUtility.UrlDecode(model.CSJJD);
            model.ID = id;
            if (bll.ManageAdd(model) > 0)
            {
                return Json(new { success = true, id = id }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { success = false, errorMessage = "添加失败。" }, JsonRequestBehavior.AllowGet);
        }

        ////管理员企业编辑
        //public JsonResult ManageEditEnterprise(RoadFlow.Data.Model.EnterpriseModel model,Guid enterpriseID)
        //{
        //    model.ZCD = HttpUtility.UrlDecode(model.ZCD);
        //    model.CSJJD = HttpUtility.UrlDecode(model.CSJJD);
        //    RoadFlow.Platform.EnterpriseAndEnterpriseTaxBLL bll = new RoadFlow.Platform.EnterpriseAndEnterpriseTaxBLL();
        //    if(bll.ManageUpdate(model,enterpriseID)>0)
        //    {
        //        return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        //    }
        //    return Json(new { success = false, errorMessage = "更新失败。" }, JsonRequestBehavior.AllowGet);
        //}

        /// <summary>
        /// 管理员企业删除
        /// </summary>
        /// <param name="enterpriseID"></param>
        /// <returns></returns>
        public JsonResult ManageDeleteEnterprise(Guid enterpriseID)
        {
            RoadFlow.Platform.EnterpriseAndEnterpriseTaxBLL bll = new RoadFlow.Platform.EnterpriseAndEnterpriseTaxBLL();
            if (bll.ManageDelete(enterpriseID) > 0)
            {
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { success = false, errorMessage = "删除失败。" }, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #endregion

        #region 公共方法
        #region 视图初始化
        /// <summary>
        /// 楼栋列表视图初始化（商务局，街道办，报送单位）
        /// </summary>
        /// <returns></returns>
        private BuildingIndexViewModel BuildingInit(int type)
        {
            BuildingIndexViewModel viewModel = new BuildingIndexViewModel();
            string pager = string.Empty;

            Dictionary<KeyValuePair<string, RoadFlow.Data.Model.SQLFilterType>, object> where = IndexGetMoreWhere();//更多条件
            #region 固定where
            if (!string.IsNullOrWhiteSpace(Request["Name"]))
            {
                viewModel.Name = Request["Name"];
                where.Add(new KeyValuePair<string, RoadFlow.Data.Model.SQLFilterType>("Name", RoadFlow.Data.Model.SQLFilterType.CHARINDEX), viewModel.Name);
            }
            if (!string.IsNullOrWhiteSpace(Request["SSJD"]))
            {
                viewModel.SSJD = Request["SSJD"];
                where.Add(new KeyValuePair<string, RoadFlow.Data.Model.SQLFilterType>("SSJD", RoadFlow.Data.Model.SQLFilterType.EQUAL), viewModel.SSJD);
            }
            #endregion
            if (type == 0)
            {//商务局
                viewModel.List = BuildingsAndBuildingMonthInfoBLL.GetPagerData(out pager, PageSize, RoadFlow.Utility.Tools.GetPageNumber(), where);
            }
            else if (type == 1)
            { //街道办
                var ssjd = new RoadFlow.Platform.DictionaryBLL().GetCurrentSSJD();
                viewModel.SSJD = ssjd == null ? "" : ssjd.Value.ToString();
                if (string.IsNullOrWhiteSpace(Request["SSJD"]))
                {
                    where.Add(new KeyValuePair<string, RoadFlow.Data.Model.SQLFilterType>("SSJD", RoadFlow.Data.Model.SQLFilterType.EQUAL), viewModel.SSJD);
                }
                viewModel.List = BuildingsAndBuildingMonthInfoBLL.GetPagerData(out pager, PageSize, RoadFlow.Utility.Tools.GetPageNumber(), where);
            }
            else
            { //报送单位
                viewModel.List = BuildingsAndBuildingMonthInfoBLL.GetCurrentUsersOrganizeList(out pager, PageSize, RoadFlow.Utility.Tools.GetPageNumber(), where);
            }

            viewModel.Pager = pager;
            viewModel.Display = GetColItemList();//读取显示字段配置信息
            viewModel.Dictionarys = DictionaryBLL.GetListAll();
            viewModel.SSJDList = new SelectList(DictionaryBLL.GetListByCode("SSJD").ToList<RoadFlow.Data.Model.DictionaryModel>(), "ID", "Title", viewModel.SSJD);
            return viewModel;
        }

        private BuildingEditViewModel EditInit(Guid id)
        {
            BuildingEditViewModel viewModel = new BuildingEditViewModel();
            viewModel.EditModel = BuildingsAndBuildingMonthInfoBLL.Get(id);
            viewModel.JSJDList = new SelectList(DictionaryBLL.GetListByCode("JSJD").ToList<RoadFlow.Data.Model.DictionaryModel>(), "ID", "Title", viewModel.EditModel.JSJD);
            viewModel.LYCQQKList = new SelectList(DictionaryBLL.GetListByCode("LYCQQK").ToList<RoadFlow.Data.Model.DictionaryModel>(), "ID", "Title", viewModel.EditModel.LYCQQK);
            viewModel.LYJBList = new SelectList(DictionaryBLL.GetListByCode("LYJB").ToList<RoadFlow.Data.Model.DictionaryModel>(), "ID", "Title", viewModel.EditModel.LYJB);
            viewModel.LYLXList = new SelectList(DictionaryBLL.GetListByCode("LYLX").ToList<RoadFlow.Data.Model.DictionaryModel>(), "ID", "Title", viewModel.EditModel.LYLX);
            viewModel.SSJDList = new SelectList(DictionaryBLL.GetListByCode("SSJD").ToList<RoadFlow.Data.Model.DictionaryModel>(), "ID", "Title", viewModel.EditModel.SSJD);
            viewModel.TCZSList = new SelectList(DictionaryBLL.GetListByCode("TCZS").ToList<RoadFlow.Data.Model.DictionaryModel>(), "ID", "Title", viewModel.EditModel.TCZS);
            viewModel.ZYKTList = new SelectList(DictionaryBLL.GetListByCode("ZYKT").ToList<RoadFlow.Data.Model.DictionaryModel>(), "ID", "Title", viewModel.EditModel.ZYKT);
            viewModel.SY_ZJList = new SelectList(DictionaryBLL.GetListByCode("SY_ZJ").ToList<RoadFlow.Data.Model.DictionaryModel>(), "ID", "Title", viewModel.EditModel.SY_ZJ);
            viewModel.SW_ZJList = new SelectList(DictionaryBLL.GetListByCode("SW_ZJ").ToList<RoadFlow.Data.Model.DictionaryModel>(), "ID", "Title", viewModel.EditModel.SW_ZJ);

            viewModel.IsImportantList = new SelectList(DictionaryBLL.GetListByCode("IsImportant").ToList<RoadFlow.Data.Model.DictionaryModel>(), "ID", "Title", viewModel.EditModel.IsImportant);
            return viewModel;
        }

        private BuildingManageEnterpriseViewModel ManageEnterpriseInit(Guid buildingID)
        {
            BuildingManageEnterpriseViewModel viewModel = new BuildingManageEnterpriseViewModel
            {
                Building = BuildingsAndBuildingMonthInfoBLL.Get(buildingID),
                Enterprises = new RoadFlow.Platform.EnterpriseAndEnterpriseTaxBLL().GetAllByBuildingID(buildingID.ToString()),
                Dictionarys = new RoadFlow.Platform.DictionaryBLL().GetAllChilds("Type")
            };
            return viewModel;
        }

        private BuildingEditEnterpriseViewModel EditEnterpriseInit(Guid enterpriseID)
        {
            BuildingEditEnterpriseViewModel viewModel = new BuildingEditEnterpriseViewModel()
            {
                Enterprise = new RoadFlow.Platform.EnterpriseAndEnterpriseTaxBLL().Get(enterpriseID),
                Dictionarys = new RoadFlow.Platform.DictionaryBLL().GetAllChilds("Type")
            };
            return viewModel;
        }

        private BuildingBindHouseIDViewModel BindHouseIDInit(Guid buildingID)
        {
            BuildingBindHouseIDViewModel viewModel = new BuildingBindHouseIDViewModel();
            var building = BLL.Get(buildingID);
            viewModel.Building = building;
            viewModel.Dictionary = DictionaryBLL.GetListByCode("LPMC").ToList<RoadFlow.Data.Model.DictionaryModel>();
            return viewModel;
        }
        #endregion

        /// <summary>
        /// 根据类型获取导出数据。（如报送单位，还是楼宇工作人员导出。）
        /// </summary>
        /// <returns></returns>
        public DataTable GetListByType(string type, Dictionary<KeyValuePair<string, RoadFlow.Data.Model.SQLFilterType>, object> where)
        {
            if (!string.IsNullOrWhiteSpace(type) && type == "submitted")
            {//报送单位
                return BuildingsAndBuildingMonthInfoBLL.GetCurrentUsersOrganizeList(where);
            }
            else
            { //工作人员。
                return BuildingsAndBuildingMonthInfoBLL.GetAll(where);
            }
        }

        /// <summary>
        /// 获取配置信息
        /// </summary>
        /// <returns></returns>
        public List<ColItem> GetColItemList()
        {
            QueryDesign Display = new RoadFlow.Platform.QueryDesign().Get("楼栋综合查询", RoadFlow.Platform.UsersBLL.CurrentUserID);
            List<ColItem> lst = Display.DisplayItem.IsNullOrEmpty() == true ? null : Display.DisplayItem.JsonConvertModel<List<ColItem>>();
            if (!lst.IsNullObj())
            {
                //删除没有标题的字段项目
                lst.RemoveAll(x => x.value.IsNullOrEmpty());
                //显示排序
                lst.Sort(new myComparer());
            }
            return lst;
        }
        private Dictionary<KeyValuePair<string, RoadFlow.Data.Model.SQLFilterType>, object> IndexGetMoreWhere()
        {
            Dictionary<KeyValuePair<string, RoadFlow.Data.Model.SQLFilterType>, object> where = new Dictionary<KeyValuePair<string, SQLFilterType>, object>();
            #region 多条件查询
            List<Form> list = new List<Form>();
            if (!Request.Form["query"].IsNullOrEmpty())
            {
                list.AddRange(Request.Form["query"].JsonConvertModel<List<Form>>());
            }
            for (int i = 0; i < list.Count; i++)
            {
                var model = list[i];
                var temp = list.Where(x => x.name == model.name).ToList();
                if (temp.Count == 1 && temp[0].value != "")
                {
                    where.Add(new KeyValuePair<string, RoadFlow.Data.Model.SQLFilterType>(temp[0].name, RoadFlow.Data.Model.SQLFilterType.CHARINDEX), temp[0].value);
                }
                else if (temp.Count == 2)
                {
                    if (temp[0].value != "" && temp[1].value != "")
                    {
                        if (temp[0].value.IsInt())
                        {
                            where.Add(new KeyValuePair<string, RoadFlow.Data.Model.SQLFilterType>(temp[0].name, RoadFlow.Data.Model.SQLFilterType.MIN), temp[0].value);
                            where.Add(new KeyValuePair<string, RoadFlow.Data.Model.SQLFilterType>(temp[0].name, RoadFlow.Data.Model.SQLFilterType.MAX), temp[1].value);
                        }
                        else
                        {
                            where.Add(new KeyValuePair<string, RoadFlow.Data.Model.SQLFilterType>(temp[0].name, RoadFlow.Data.Model.SQLFilterType.MIN), temp[0].value);
                            where.Add(new KeyValuePair<string, RoadFlow.Data.Model.SQLFilterType>(temp[0].name, RoadFlow.Data.Model.SQLFilterType.MAXNotEqual), temp[1].value);
                        }
                    }
                    else if (temp[0].value != "" && temp[1].value == "")
                    {
                        where.Add(new KeyValuePair<string, RoadFlow.Data.Model.SQLFilterType>(temp[0].name, RoadFlow.Data.Model.SQLFilterType.MIN), temp[0].value);
                    }
                    else if (temp[0].value == "" && temp[1].value != "")
                    {
                        where.Add(new KeyValuePair<string, RoadFlow.Data.Model.SQLFilterType>(temp[0].name, RoadFlow.Data.Model.SQLFilterType.MAX), temp[0].value);
                    }
                    i++;
                }
            }
            #endregion
            return where;
        }
        #endregion

    }



    /// <summary>
    /// TABLE显示顺序排序用
    /// </summary>
    public class myComparer : IComparer<ColItem>
    {
        //实现按升序排列
        public int Compare(ColItem x, ColItem y)
        {
            return (x.sortid.CompareTo(y.sortid));
        }
    }
}

