using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebMvc.Controllers;
using RoadFlow.Data.Model;
using System.Collections.Generic;
using System.Data;
using RoadFlow.Utility;

namespace WebMVCTest {
    [TestClass]
    public class EnterpriseControllerTest {
        [TestMethod]
        public void TestGetAllJob() {
            //init
            //EnterpriseController controller = new EnterpriseController();
            //List<Organize> list = controller.GetAllJob();

            ////断言
            //Assert.IsNotNull(list);
            //Assert.AreEqual(45,list.Count,"岗位数目不对");

            //List<RoadFlow.Data.Model.Users> tels = new RoadFlow.Data.MSSQL.Users().GetAll();

            //List<RoadFlow.Data.Model.Users> list = tels.FindAll(p => p.Tell != null);

            //string id = "A221D823-8F39-43BD-B578-850EF7549385";//报送单位ID
            //RoadFlow.Platform.Organize organize = new RoadFlow.Platform.Organize();
            //List<Users> list = organize.GetAllUsers(Guid.Parse(id));

            //foreach (Users item in list) {
            //    SMSMessage message = new SMSMessage {
            //        Mobile = item.Tell,
            //        //Content = "【青羊楼宇】" + "各位街道办及楼宇运营方：由于青羊楼宇综合服务平台于5月27日零时开始进行系统维护升级，维护期间无法正常使用，预计下周一（5月30日）恢复正常，给大家带来不便敬请谅解。   成都华保科技有限公司  2016年5月27日"
            //        //Content = "【青羊楼宇】" + "各位街道办及楼宇运营方：为方便填报问题解答及沟通交流，请加QQ群557042639"
            //        //Content ="各位街道办及楼宇运营方：由于青羊楼宇综合服务平台于5月27日零时开始进行系统维护升级，维护期间无法正常使用，预计下周一（5月30日）恢复正常，给大家带来不便敬请谅解。   成都华保科技有限公司  2016年5月27日"
            //        Content = "【青羊楼宇】" + "各楼宇企业，2016年市级服务业引导资金申报工作即将开始，请务必于6月25日前登陆青羊楼宇综合服务平台完成楼宇信息报送工作，已报送的，请做好信息的检查更新工作。电话：86247499"
            //    };
            //    new SMS().SendSMS(message);
            //}

            //SMSController c = new SMSController();
            //c.Index();

            //string item = "A221D823-8F39-43BD-B578-850EF7549385";
            //string item2 = "04F12BEB-D99D-43DF-AC9A-3042957D6BDA";

            //RoadFlow.Platform.Organize organize = new RoadFlow.Platform.Organize();
            //organize.GetAllChilds(Guid.Parse(item));
            //List<Users> list = organize.GetAllUsers(Guid.Parse(item));
            //List<Organize> list = organize.GetChilds(Guid.Parse(item));
            //List<Organize> list2 = organize.GetChilds(Guid.Parse(item2));

            //Assert.AreEqual(1, list.Count, "1");
            //Assert.AreEqual(2, list2.Count,"2");

            //EnterpriseImportController controller = new EnterpriseImportController();
            //controller.Import("/Content/UploadFiles/201607/13/test.xlsx");


            //DataTable dt = ExportExcel.ImportToTable("D:\\.system\\work\\project\\LYXT\\QYLY\\WebMvc\\Content\\UploadFiles\\201607\\13\\test2_NZ0L42.xlsx");
            //if (dt != null) {
            //    RoadFlow.Platform.EnterpriseTax tax = new RoadFlow.Platform.EnterpriseTax();
            //    List<RoadFlow.Data.Model.EnterpriseTax> list = dt.DataTableToList<RoadFlow.Data.Model.EnterpriseTax>();
            //    foreach (var item in list) {
            //        if (item.Name.IsNullOrEmpty() || item.TYSHXYDM.IsNullOrEmpty() || item.TYSHXYDM.Length < 14) {
            //            continue;
            //        }
            //        if (tax.GetModelByTYSHXYDMAndYear(item.TYSHXYDM, item.TaxYear) != null) {
            //            //tax.Add(item);
            //        }
            //    }
            //}

        }

        [TestMethod]
        public void TestGetAllDt() {

        }

    }
}
