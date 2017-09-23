using RoadFlow.Data.Factory;
using RoadFlow.Data.Interface;
using RoadFlow.Data.Model;
using System;
using System.Data;
namespace RoadFlow.Platform {
    public class SMS {
        private ISMS iSMS;

        public SMS() {
            iSMS = Factory.GetSMS();
        }

        public int Add(SMSModel model) {
            return iSMS.Add(model);
        }

        public DataTable GetDataPage(out string pager, string query = "", int size = 15, int number = 1,
            string title = "", string wher = "") {
                return iSMS.GetDataPage(out pager, query, size, number, title, wher);
        }

        public SMSModel GetModel(string id) {
            return iSMS.GetModel(id);
        }

        /// <summary>
        /// 通过发送者Id获取model
        /// </summary>
        /// <param name="id">发送者Id</param>
        /// <returns></returns>
        public SMSModel GetModelBySendUser(string id) {
            return iSMS.GetModelBySendUser(id);
        }

        public int Del(string id) {
            return iSMS.Del(id);
        }

        public int Update(SMSModel model) {
            return iSMS.Update(model);
        }

    }
}