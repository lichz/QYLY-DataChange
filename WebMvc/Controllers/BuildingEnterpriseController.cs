using RoadFlow.Data.Model;
using RoadFlow.Utility;
using RoadFlow.Web.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace WebMvc.Controllers
{
    /// <summary>
    /// 楼栋企业
    /// </summary>
    public class BuildingEnterpriseController : Controller
    {
        public RoadFlow.Platform.EnterpriseAndEnterpriseTaxBLL BLL { get; private set; }
        public RoadFlow.Platform.EnterpriseTaxBLL EnterpriseTaxBLL { get; private set; }
        public int PageSize { get; private set; }

        public BuildingEnterpriseController()
        {
            PageSize = 13;
            EnterpriseTaxBLL = new RoadFlow.Platform.EnterpriseTaxBLL();
            BLL = new RoadFlow.Platform.EnterpriseAndEnterpriseTaxBLL();
        }

        #region Action
        public ActionResult Index()
        {
            return View(IndexInit());
        }

        [HttpPost]
        [ActionName("Index")]
        public ActionResult IndexPost()
        {
            return View(IndexInit());
        }

        /// <summary>
        /// 企业编辑
        /// </summary>
        /// <param name="enterpriseID"></param>
        /// <returns></returns>
        public ActionResult Edit(Guid? enterpriseID)
        {
            if (enterpriseID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            return View(EditInit(enterpriseID.Value));
        }

        /// <summary>
        /// 企业编辑
        /// </summary>
        /// <param name="enterpriseID"></param>
        /// <returns></returns>
        [HttpPost]
        [ActionName("Edit")]
        public ActionResult EditPost(EnterpriseModel enterprise, Guid enterpriseID)
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
            return View(EditInit(enterpriseID));
        }

        //企业税收导入
        public ActionResult TaxImport()
        {
            return View(TaxImportInit());
        }

        [HttpPost]
        [ActionName("TaxImport")]
        public ActionResult TaxImportPost()
        {
            return View(TaxImportInit());
        }

        /// <summary>
        /// 税收管理
        /// </summary>
        /// <returns></returns>
        public ActionResult TaxManage(Guid enterpriseID)
        {
            BuildingEnterpriseTaxManageViewModel viewModel = new BuildingEnterpriseTaxManageViewModel()
            {
                Enterprise = BLL.Get(enterpriseID),
                Taxs = EnterpriseTaxBLL.GetAllByEnterpriseID(enterpriseID)
            };
            return View(viewModel);
        }

        ////企业税收编辑
        //public ActionResult TaxEdit(Guid id)
        //{
        //    return View(TaxEditInit(id));
        //}

        //[HttpPost]
        //[ActionName("TaxEdit")]
        //public ActionResult TaxEditPost(RoadFlow.Data.Model.EnterpriseTaxModel model,Guid modelID)
        //{
        //    if (EnterpriseTaxBLL.Update(model, modelID)>0)
        //    {
        //        ViewBag.Success = true;
        //    }
        //    return View(TaxEditInit(modelID));
        //}

        ////企业税收添加
        //public ActionResult TaxCreate(Guid enterpriseID)
        //{
        //    return View(TaxCreateInit(enterpriseID));
        //}

        //[HttpPost]
        //[ActionName("TaxCreate")]
        //public ActionResult TaxCreatePost(RoadFlow.Data.Model.EnterpriseTaxModel model)
        //{
        //    if (EnterpriseTaxBLL.Add(model) > 0)
        //    {
        //        ViewBag.Success = true;
        //    }
        //    return View(TaxCreateInit(model.EnterpriseID.Value));
        //}


        #region 操作（如异步查询，导出等。）
        //企业税收删除
        //public string TaxDelete(Guid id)
        //{
        //    if(EnterpriseTaxBLL.Delete(id)>0)
        //    {
        //        return "success";
        //    }
        //    return "删除失败。";
        //}

        /// <summary>
        /// 企业删除
        /// </summary>
        /// <returns></returns>
        public JsonResult Delete(Guid enterpriseID)
        {
            if (BLL.ManageDelete(enterpriseID) > 0)
            {
                return Json(new { success = true });
            }
            return Json(new { success = false, errorMessage = "删除失败。" });
        }

        /// <summary>
        /// 管理员添加
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public JsonResult ManageCreateTax(EnterpriseTaxModel model)
        {
            var result = EnterpriseTaxBLL.Add(model);
            if (result.Success)
            {
                return Json(new { success=true,id=result.Data });
            }
            return Json(new { success=false, errorMessage ="保存失败，请检查输入格式。" });
        }

        /// <summary>
        /// 管理员更新
        /// </summary>
        /// <param name="model"></param>
        /// <param name="taxID"></param>
        /// <returns></returns>
        public JsonResult ManageEditTax(EnterpriseTaxModel model,Guid taxID)
        {
            if (EnterpriseTaxBLL.Update(model,taxID) > 0)
            {
                return Json(new { success = true });
            }
            return Json(new { success = false,errorMessage = "保存失败，请检查输入格式。" });
        }

        /// <summary>
        /// 管理员删除
        /// </summary>
        /// <param name="taxID"></param>
        /// <returns></returns>
        public JsonResult ManageDeleteTax(Guid taxID)
        {
            if (EnterpriseTaxBLL.Delete(taxID) > 0)
            {
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }

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
            #endregion
            DataTable dt = BLL.GetAll(where);
            //导出列
            Dictionary<string, string> dicionary = new Dictionary<string, string>();
            dicionary.Add("Name", "企业名称");
            dicionary.Add("BuildingName", "所属楼栋");
            dicionary.Add("ZCD", "企业注册地");
            dicionary.Add("TYSHXYDM", "统一信用代码");
            dicionary.Add("CSJJD", "税收解缴地");
            dicionary.Add("Type", "企业类型");
            dicionary.Add("InTotalArea", "入驻面积");
            dicionary.Add("RentArea", "租用面积");
            dicionary.Add("PersonalUseArea", "自用面积");
            //dicionary.Add("NationalTax", "国税");
            //dicionary.Add("LandTax", "地税");
            dicionary.Add("Tax", "税收");

            ExportExcel.Export(dt.ExportExcelPre(dicionary), "企业列表");
        }
        #endregion
        #endregion

        #region 公共方法
        #region 视图初始化
        private BuildingEnterpriseIndexViewModel IndexInit()
        {
            BuildingEnterpriseIndexViewModel viewModel = new BuildingEnterpriseIndexViewModel();
            DataTable dt = new DataTable();
            string pager = string.Empty;

            Dictionary<KeyValuePair<string, RoadFlow.Data.Model.SQLFilterType>, object> where = IndexGetMoreWhere();//更多条件
            #region 固定where
            if (!string.IsNullOrWhiteSpace(Request["Name"]))
            {
                viewModel.ParaName = Request["Name"];
                where.Add(new KeyValuePair<string, RoadFlow.Data.Model.SQLFilterType>("Name", RoadFlow.Data.Model.SQLFilterType.CHARINDEX), viewModel.ParaName);
            }
            #endregion
            dt = BLL.GetPagerData(out pager, PageSize, RoadFlow.Utility.Tools.GetPageNumber(), where);
            viewModel.Dictionary = new RoadFlow.Platform.DictionaryBLL().GetListAll();
            viewModel.List = dt;
            viewModel.Pager = pager;
            return viewModel;
        }

        private BuildingEditEnterpriseViewModel EditInit(Guid enterpriseID)
        {
            BuildingEditEnterpriseViewModel viewModel = new BuildingEditEnterpriseViewModel()
            {
                Enterprise = new RoadFlow.Platform.EnterpriseAndEnterpriseTaxBLL().Get(enterpriseID),
                Dictionarys = new RoadFlow.Platform.DictionaryBLL().GetAllChilds("Type")
            };
            return viewModel;
        }

        private BuildingEnterpriseTaxImportViewModel TaxImportInit()
        {
            BuildingEnterpriseTaxImportViewModel viewModel = new BuildingEnterpriseTaxImportViewModel();
            DataTable dt = new DataTable();
            string pager = string.Empty;

            Dictionary<KeyValuePair<string, RoadFlow.Data.Model.SQLFilterType>, object> where = IndexGetMoreWhere();//更多条件
            #region 固定where
            if (!string.IsNullOrWhiteSpace(Request["Name"]))
            {
                viewModel.ParaName = Request["Name"];
                where.Add(new KeyValuePair<string, RoadFlow.Data.Model.SQLFilterType>("Name", RoadFlow.Data.Model.SQLFilterType.CHARINDEX), viewModel.ParaName);
            }
            #endregion
            dt = EnterpriseTaxBLL.GetPagerData(out pager, PageSize, RoadFlow.Utility.Tools.GetPageNumber(), where);
            viewModel.Dictionary = new RoadFlow.Platform.DictionaryBLL().GetListAll();
            viewModel.List = dt;
            viewModel.Pager = pager;
            return viewModel;
        }

        private RoadFlow.Data.Model.EnterpriseTaxModel TaxEditInit(Guid id)
        {
            var model = EnterpriseTaxBLL.Get(id);
            model.EnterpriseModel = BLL.Get(model.EnterpriseID.Value);
            return model;
        }

        private RoadFlow.Data.Model.EnterpriseAndEnterpriseTaxModel TaxCreateInit(Guid enterpriseID)
        {
            var model = BLL.Get(enterpriseID);
            return model;
        }
        #endregion
        //private List<RoadFlow.Data.Model.ColItem> GetColItemList() {
        //    RoadFlow.Data.Model.QueryDesign Display = new RoadFlow.Platform.QueryDesign().Get("企业查询", RoadFlow.Platform.Users.CurrentUserID);
        //    List<RoadFlow.Data.Model.ColItem> lst = Display.DisplayItem.IsNullOrEmpty() == true ? null : Display.DisplayItem.JsonConvertModel<List<RoadFlow.Data.Model.ColItem>>();
        //    if (!lst.IsNullObj()) {
        //        //删除没有标题的字段项目
        //        lst.RemoveAll(x => x.value.IsNullOrEmpty());
        //        ////权限控制报送企业查看到税收字段
        //        //if (Request.QueryString["flag"] == "1") {
        //        //    lst.RemoveAll(x => x.id == "E_Tax1");
        //        //    lst.RemoveAll(x => x.id == "E_Tax2");
        //        //}
        //        //显示排序
        //        lst.Sort(new myComparer());
        //    }
        //    return lst;
        //}
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
}
