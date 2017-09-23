using RoadFlow.Data.Model;
using System;
using System.Data;
namespace RoadFlow.Data.Interface {
    public interface ISMS {
        int Add(SMSModel model);

        DataTable GetDataPage(out string pager, string query = "", int size = 15, int number = 1, string title = "",
            string wher = "");

        SMSModel GetModel(string id);

        SMSModel GetModelBySendUser(string id);

        int Del(string id);

        int Update(SMSModel model);
    }
}