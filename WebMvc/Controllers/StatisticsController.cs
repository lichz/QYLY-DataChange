using Newtonsoft.Json;
using RoadFlow.Utility;
using RoadFlow.Web.Model;
using System.Collections.Generic;
using System.Data;
using System.Web.Mvc;

namespace WebMvc.Controllers
{
    public class StatisticsController : Controller
    {
        public RoadFlow.Platform.BuildingsStreetStatisticsBLL BuildingsStreetStatisticsBLL { get; private set; }
        public RoadFlow.Platform.BuildingsAndBuildingMonthInfoBLL BuildingsAndBuildingMonthInfoBLL { get; private set; }

        public StatisticsController()
        {
            BuildingsStreetStatisticsBLL = new RoadFlow.Platform.BuildingsStreetStatisticsBLL();
            BuildingsAndBuildingMonthInfoBLL = new RoadFlow.Platform.BuildingsAndBuildingMonthInfoBLL();
        }

        #region Action
        #region 视图
        //街道统计
        public ActionResult Index()
        {
            StatisticsIndexViewModel viewModel = new StatisticsIndexViewModel();
            string ssjd = Request["ssjd"];
            RoadFlow.Platform.DictionaryBLL DictionaryBLL = new RoadFlow.Platform.DictionaryBLL();
            viewModel.ParaSSJD = new SelectList(DictionaryBLL.GetListByCode("SSJD").ToList<RoadFlow.Data.Model.DictionaryModel>(), "ID", "Title", ssjd); ;
            viewModel.List = BuildingsStreetStatisticsBLL.GetBySSJD(ssjd);
            return View(viewModel);
        }

        //楼盘面积统计
        public ActionResult HouseArea()
        {
            return View(HouseAreaInit());
        }

        //简单统计（空置率，落地率）
        public ActionResult Simple()
        {
            return View(SimpleInit());
        }

        //简单统计的各街道情况图
        public ActionResult SimpleStreetChart(int type)
        {
            if (type == 1)
            {//空置率
                ViewBag.ChartTitle = "空置率(%)";
                ViewBag.SerieName = "空置率";
                GetValuesByField("VacancyRate");
            }
            else if (type == 2)
            {//商业空置率
                ViewBag.ChartTitle = "商业空置率(%)";
                ViewBag.SerieName = "商业空置率";
                GetValuesByField("BusinessVacancyRate");
            }
            else if (type == 3)
            {//商务空置率
                ViewBag.ChartTitle = "商务空置率(%)";
                ViewBag.SerieName = "商务空置率";
                GetValuesByField("CommerceVacancyRate");
            }
            else if (type == 4)
            {//落地率
                ViewBag.ChartTitle = "落地率(%)";
                ViewBag.SerieName = "落地率";
                GetValuesByField("FloorRate");
            }
            return View();
        }

        #endregion
        #region 操作
        //街道统计导出
        public void Export(string SSJD, string expwher)
        {
            //获取导出列表
            DataTable dt = BuildingsStreetStatisticsBLL.GetBySSJD(Request["ssjd"]);
            ExportExcel.Export(dt.ExportExcelPreByName("街道统计"), "街道统计");
        }

        //楼盘面积统计导出
        public void HouseAreaExport(string SSJD, string expwher)
        {
            //获取导出列表
            DataTable dt = BuildingsAndBuildingMonthInfoBLL.GetStatisticsByHouseName(Request["ParaName"]);

            //导出列
            Dictionary<string, string> dicionary = new Dictionary<string, string>
            {
                { "HouseName", "楼盘名称" },
                { "ZJZMJ", "总建筑面积" }
            };
            ExportExcel.Export(dt.ExportExcelPre(dicionary), "楼盘面积统计");
        }

        //简单统计（空置率，落地率）导出
        public void SimpleExport()
        {
            decimal businessVacancyRate, commerceVacancyRate, vacancyRate, floorRate;
            var list = BuildingsStreetStatisticsBLL.GetBySSJD(string.Empty);//街道数据
            CalculateVacancyRateAndFloorRate(list, out businessVacancyRate, out commerceVacancyRate, out vacancyRate, out floorRate);
            Dictionary<string, string> dicionary = new Dictionary<string, string>
            {
                { "Name", "街道名称" },
                { "VacancyRate", "总空置率" },
                { "BusinessVacancyRate", "商业空置率" },
                { "CommerceVacancyRate", "商务控制率" },
                { "FloorRate", "落地率" }
            };

            ExportExcel.Export(list.ExportExcelPre(dicionary), "简单统计", new List<List<string>>() {
                new List<string>()
                {
                    "总空置率：",
                    decimal.Round(businessVacancyRate,2).ToString(),
                    "商业空置率：",
                    decimal.Round(commerceVacancyRate,2).ToString(),
                    "商务空置率：",
                    decimal.Round(vacancyRate,2).ToString(),
                    "落地率：",
                    decimal.Round(floorRate,2).ToString(),
                }
            });
        }
        #endregion
        #endregion
        #region 公共方法
        #region 视图初始化
        private StatisticsHouseAreaViewModel HouseAreaInit()
        {
            StatisticsHouseAreaViewModel viewModel = new StatisticsHouseAreaViewModel()
            {
                List = BuildingList()
            };
            return viewModel;
        }

        private StatisticsSimpleViewModel SimpleInit()
        {
            decimal businessVacancyRate, commerceVacancyRate, vacancyRate, floorRate;
            var list = BuildingsStreetStatisticsBLL.GetBySSJD(string.Empty);//街道数据
            CalculateVacancyRateAndFloorRate(list, out businessVacancyRate, out commerceVacancyRate, out vacancyRate, out floorRate);
            StatisticsSimpleViewModel viewModel = new StatisticsSimpleViewModel()
            {
                List = list,
                BusinessVacancyRate = businessVacancyRate,
                CommerceVacancyRate = commerceVacancyRate,
                VacancyRate = vacancyRate,
                FloorRate = floorRate
            };
            return viewModel;
        }
        #endregion

        /// <summary>
        /// 遍历组装数据
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        private List<StatisticsHouseAreaViewModel.Temp> BuildingList()
        {
            var list = BuildingsAndBuildingMonthInfoBLL.GetStatisticsByHouseName(Request["ParaName"]).ToList<StatisticsHouseAreaViewModel.Temp>();
            return list;
        }

        /// <summary>
        /// 根据字段名获取值
        /// </summary>
        private void GetValuesByField(string field)
        {
            var list = BuildingsStreetStatisticsBLL.GetBySSJD(string.Empty);//街道数据

            var displayName = new List<string>();
            var values = new List<object>();
            foreach (DataRow dr in list.Rows)
            {
                displayName.Add(dr["Name"].ToString());
                values.Add(dr[field]);
            }
            ViewBag.DisplayName = JsonConvert.SerializeObject(displayName);
            ViewBag.Values = JsonConvert.SerializeObject(values);
        }

        private void CalculateVacancyRateAndFloorRate(DataTable dt, out decimal businessVacancyRate, out decimal commerceVacancyRate, out decimal vacancyRate, out decimal floorRate)
        {
            //计算总空置率、总落地率
            decimal allBusinessArea = 0;//总商业面积
            decimal businessVacancyArea = 0;//总商业空置面积
            decimal allCommerceArea = 0;//总商务面积
            decimal commerceVacancyArea = 0;//总商务空置面积
            decimal allArea = 0;//总面积
            int enterpriseCount = 0;//总企业数
            int QYEnterpriseCount = 0;//青羊企业数
            foreach (DataRow dr in dt.Rows)
            {
                allBusinessArea += dr["SY_ZMJ"].Convert<decimal>(0);
                businessVacancyArea += dr["SY_KZ_ZMJ"].Convert<decimal>(0);
                allCommerceArea += dr["SW_ZMJ"].Convert<decimal>(0);
                commerceVacancyArea += dr["SW_KZ_ZMJ"].Convert<decimal>(0);
                allArea += dr["ZJZMJ"].Convert<decimal>(0);
                enterpriseCount += dr["EnterpriseCount"].Convert<int>(0);
                QYEnterpriseCount += dr["QYEnterpriseCount"].Convert<int>(0);
            }

            businessVacancyRate = (allBusinessArea == 0 ? 0 : businessVacancyArea / allBusinessArea) * 100;
            commerceVacancyRate = (allCommerceArea == 0 ? 0 : commerceVacancyArea / allCommerceArea) * 100;
            vacancyRate = (allArea == 0 ? 0 : (businessVacancyArea + commerceVacancyArea) / allArea) * 100;
            floorRate = (enterpriseCount == 0 ? 0 : QYEnterpriseCount / (decimal)enterpriseCount) * 100;
        }
        #endregion

    }
}