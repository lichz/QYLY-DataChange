using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebMvc.Controllers;
using RoadFlow.Data.Model;
using System.Collections.Generic;
using System.Data;

namespace WebMVCTest {
    [TestClass]
    public class ActivityStatisticsControllerTest {
        [TestMethod]
        public void TestGetAll() {
            //init
            //string query1 = "{}";
            //string query2 = RoadFlow.Utility.ObjectExpand.ToJson(new { TimeBegin = "2016/05/19" });

            //ActivityStatisticsController controller = new ActivityStatisticsController();
            //DataTable dt1 = controller.GetAll(query1, 1, 30);
            //DataTable dt2 = controller.GetAll(query2, 1, 30);

            
            ////断言
            //Assert.IsNotNull(dt1);
            //Assert.AreEqual(30, dt1.Rows.Count, "没有参数时岗位数目不对");
            //Assert.IsNotNull(dt2);
            //Assert.AreEqual(0, dt2.Rows.Count, "有参数时岗位数目不对");

            //RoadFlow.Platform.Enterprise enterPrise = new RoadFlow.Platform.Enterprise();
            //string pager = string.Empty;
            //string query = "&appid=f2b5cdba-eff2-4060-98fa-d580537c9a16&tabid=tab_f2b5cdbaeff2406098fad580537c9a16&Name=&flag=";
            //string DisplayItem = "ID,Num,Name,LYDS,LYJB,LYXXDZ,ZJZMJ,SY_ZMJ,SW_ZMJ";
            //string ryid = "'82a215a1-df82-4463-bbb4-024a0088fda5','bedac137-08a5-46ba-a75d-03630b8435a4','b9068379-111a-4afe-b4c8-08732eb2daa9','eddd82e1-7195-48ae-9aa7-09d746160223','0d94d0f3-2a15-485f-8574-0d282956bc90','ee727cb5-6652-4a03-aca1-0ed25f3f24f9','6a7cca2e-bae3-4b2b-a2e2-104fa72bbe18','7d388d7d-6876-4b23-8fd4-12119305a879','114f184a-6df0-433a-87fd-138f5caef9e6','dc7c98a7-b610-40ea-9bb7-139aec1d0031','9aaf30cb-8340-403c-bbe3-18c15b959547','f23611f7-6a06-464b-b41c-1bd31ac61a6c','4e0ba258-d1cc-4c70-a8df-247add69981b','e69248bd-ab03-4919-98a0-25872aa59f44','8424383e-1229-4df1-9f93-258ba3010b4b','b81ee359-d24e-4456-a56f-274fa395d526','ca16b309-b6d9-42f0-83c6-2a09f37158b9','db9f3fd0-a21c-4ae8-85ef-2afad0e0df97','471c87b6-56fd-435e-9f51-2e1e0d0e99e1','93746066-1453-4219-848b-37227b8c80a8','42178516-f3ff-4b71-baa9-3964659c59a2','8a5ef7e3-73b6-47c4-95a2-3b644b592e78','45525a23-f5ba-4f77-bd50-3bc53383c3fa','808ca174-34d9-4b6f-8d21-3dd77fda9baa','8c4e198b-fd69-4017-b9f4-40783d80747d','04161317-502e-42ed-ab54-409f7e2cd507','bdea8839-2ca5-49a5-a58e-43d7aef3bfb6','b0b6b2a0-3b36-46eb-8686-45b9aec77dfe','cd3801dd-6850-4a8e-b0e9-49ba1c1feea3','0b9e7783-a3be-40c5-921c-4ac6ec3eeb81','84c5c950-7aa1-4db7-9390-4b8b21829235','4e2a7e55-f78e-4c3b-aabc-4bca94b7ba8c','eb057af2-3f3c-4451-85a4-4e8a63c3ddfb','c6c14958-bb8a-4f3b-8815-5058e47acfb2','96f561d6-1bb8-4db5-8662-531e207bf88c','e8100214-6aee-458d-be35-556679e3b72e','2c713b4d-bc97-483d-8d17-62040131adb9','922d13ac-21f5-4b65-919f-632acaf4b1ad','1b1729ef-52ed-4e9f-b410-6446058459bb','8e4f78e9-025d-4f4d-8cec-65a1a30dc8f9','727722e1-40c4-4262-b389-6f5f743558c6','7b605296-e175-476f-9307-71370510e3ab','bcac8b0f-d913-4130-bfce-7297b6786809','07990e3d-f862-4a4b-adff-7462d8b93c8d','b6a954f1-90ec-4a02-9d22-74ff058ba4a9','cd572a2f-b683-402c-8839-75407b1d4b2c','5872a216-1653-42f6-9082-75ee4a3cabcf','b040d017-7153-485d-85f6-76b5d3e9c0f6','157d2910-626b-4cdc-bf9a-76e0724fcdfb','e9730694-adad-4339-8ca0-78a9749f7364','be0d888f-0e27-4e47-a277-819dd3e9ef45','e8f6a1aa-567b-41c0-a3c6-8704f49de9f8','2667080c-84c0-4a35-b1b0-87eb384c7165','84de53fa-db7a-46fb-bb48-885df388ebea','906782d2-1b5d-4120-a001-8cea6223e3a5','c4c69149-c0bb-482c-8a01-8dbb5a449ba1','7673005f-b8f4-4bb2-9ac2-8dc783548970','98a6eebd-7249-4d72-8b74-8fe35f24c9b5','a8c3414f-4205-4056-af08-91c11115f627','da700d14-af89-4af0-b603-92900b4b4d45','cdbc1313-ea1f-49bd-9f96-9431fe35bd9a','eb03262c-ab60-4bc6-a4c0-96e66a4229fe','3785fa29-4ab0-47b3-93d6-9a6f35ad2505','005a9592-82fe-4d25-b2c1-9b5ab4d865f5','cc35fb71-008c-4cae-b31a-a149f7716525','03564521-e6ec-4274-8b62-a4ff6cff09c0','b98bf67f-be74-49a7-975c-a938c05ffdf7','7fe69bb2-e4c3-4f81-9906-ab75bcce4e2f','1c6908a5-ea8a-4017-aca8-ac99ddef68c8','c521b3f6-4f2a-41cf-a29f-b3848d4838cb','5d61a460-2f03-482e-8ffa-b41252daaacc','40b21f18-b50c-47ef-a1f7-b9de3a9b9431','d0f0b583-2a15-466b-aafd-bfb22def6bdf','e3a5208f-c03c-4477-b95f-c062bc706a8b','835d984f-0aaa-4ffb-99b9-c0826af46d83','cca6b3ec-c27e-41ca-8af7-c09151089a32','9d106f4b-f9fa-40b1-9579-c1f6c3cc4c8d','c72bc770-714e-4758-bbb7-cc155939ca20','830f6d6c-6cdd-40f8-a49b-cd2ce7befe22','acba72d9-dae0-4dd2-881b-d2af7f0a6b6d','59b5efb0-560a-4ce5-9528-d3599635c328','56861237-fb5b-4e9d-9e71-d40bc006a6ed','8be71503-a466-4fbb-a540-d451afb6e7ed','8853835c-99db-4661-a6ed-d7af84aaced6','0d2ed160-c7d1-4b00-823d-db0b4fd46224','dff855c5-d2a7-4ae5-86cb-dc148af8ce87','4cc77b01-f797-4394-acba-dcf683f290ab','0fdb0353-380a-4f52-b5ac-de8a27ecccb1','42f62d07-d53f-4947-9483-e183d904c6f0','cf51663d-f8d7-4fc8-9a66-e36d635c1f1e','46418b5c-cb5d-459d-9186-e6b13604183b','cb07a8b9-5ff3-4f80-9d28-ecab28bbbbd7','b8826a48-cc1a-40c6-baf5-ece063f94a41','7daef810-7810-491e-8bea-ed12da3910ad','8dce6fd8-5cfd-4d82-8d9b-ee3ffa5f8486','d96eaed4-2bf2-4394-8944-f518db41862f','5fdc4251-a832-48fe-9dfd-f81ac70d9e31','eba1974a-6292-4c02-b4f5-faeb586a798e','24a4b13d-4cb3-4675-a490-fee91ffb4e6b','31e5c039-6cd5-4ad4-9412-ff4f3b19c7d3','c2de8bf1-b88c-43af-b57f-cf192b32e5ca'";
            //string ssjd = "daf1dbfe-68fa-4702-a9d3-ee0c4128f22e";

            //DataTable dt = enterPrise.GetPagerData(out pager, query, 30, 1, DisplayItem, "", ssjd, ryid, "");

            //Assert.AreEqual(10, dt.Rows.Count);

            
            

            string pager=string.Empty;
            string query=string.Empty;
            string ssjd=string.Empty;
            string wher=string.Empty;
            //RoadFlow.Platform.Buildings_Statistics statistics = new RoadFlow.Platform.Buildings_Statistics();
            //var dt = statistics.GetDataPage(out pager, query, 30, 1, ssjd, wher);



        }

    }
}
