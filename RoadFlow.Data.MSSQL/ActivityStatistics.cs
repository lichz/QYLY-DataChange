using RoadFlow.Data.Interface;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System;

namespace RoadFlow.Data.MSSQL{
    /// <summary>
    /// 活跃统计
    /// </summary>
    public class ActivityStatistics : IActivityStatistics{

        private DBHelper dbHelper = new DBHelper();
        string table = @"(select y.number,x.*
from
(select
dwname,
sum((case listname when  '物业报送3' then cnt else 0 end)) BuildingSubmitedNum,
sum((case listname when  '物业报送2' then cnt else 0 end)) BuildingNoSubmitedNum,
sum((case listname when  '物业报送1' then cnt else 0 end)) CompanyNoSubmitedNum,
sum((case listname when  '物业更新3' then cnt else 0 end)) BuildingModifiedNum,
sum((case listname when  '物业更新2' then cnt else 0 end)) BuildingNoModifiedNum,
sum((case listname when  '街道审核3' then cnt else 0 end)) StreetReviewedNum,
sum((case listname when  '街道审核2' then cnt else 0 end)) StreetNoReviewedNum,
sum((case listname when  ' 商务局审核3' then cnt else 0 end))+sum((case listname when  '商务局审核3' then cnt else 0 end)) BusinessBureauReviewedNum,
sum((case listname when  ' 商务局审核2' then cnt else 0 end))+sum((case listname when  '商务局审核2' then cnt else 0 end)) BusinessBureauNoReviewedNum,
sum((case listname when  '诉求发布3' then cnt else 0 end)) AppealNum,
sum((case listname when  '诉求回复3' then cnt else 0 end)) ReplyNum,
sum((case listname when  '通知公告发布3' then cnt else 0 end)) AnnouncementReleasedNum,
sum((case listname when  '通知公告查阅3' then cnt else 0 end)) AnnouncementReadedNum,
sum((case listname when  '拟制政策3' then cnt else 0 end)) PolicyNum,
sum((case listname when  '政策查阅3' then cnt else 0 end)) PolicyReadedNum,
0 as score
from
(select o.dwname,o.stepname+o.flag as listname,o.cnt from 
(select j.dwname,j.stepname,'3' as flag,count(*) as cnt from 
(select e.name as dwname,d.stepname,d.instanceid,d.senderName,d.completedtime1 from 
 ( select c.id,c.name,a.id as userid,a.name as username from users a,usersrelation b,dbo.Organize c
where c.id =b.organizeid and b.userid =a.id ) e,
 dbo.WorkFlowTask d
where 
e.userid =d.receiveid) j
where j.completedtime1 is not null {0}
group by j.dwname,j.stepname) o
union all
select o.dwname,o.stepname+o.flag as listname,o.cnt from 
(select j.dwname,j.stepname,'2' as flag,count(*) as cnt from 
(select e.name as dwname,d.stepname,d.instanceid,d.senderName,d.completedtime1,d.ReceiveTime from 
 ( select c.id,c.name,a.id as userid,a.name as username from users a,usersrelation b,dbo.Organize c
where c.id =b.organizeid and b.userid =a.id ) e,
 dbo.WorkFlowTask d
where 
e.userid =d.receiveid) j
where j.completedtime1 is  null {1}
group by j.dwname,j.stepname) o
union all
select o.dwname,o.stepname+o.falg as listname,o.cnt from 
(select j.dwname,j.stepname,'1' as falg,count(qyname) as cnt from 
(select e.name as dwname,d.stepname,d.instanceid,d.senderName,d.completedtime1,d.ReceiveTime,f.id as lyid,f.name as lyname,h.name as qyname from 
dbo.WorkFlowTask d,
( select c.id,c.name,a.id as userid,a.name as username from users a,usersrelation b,dbo.Organize c
where c.id =b.organizeid and b.userid =a.id ) e,
dbo.BuildingsData f,
dbo.EnterpriseAndEnterpriseTax h
where 
e.userid =d.receiveid and d.InstanceID =f.id and f.id =h.BuildingID and d.stepname ='物业报送' ) j
where j.completedtime1 is  null {2}
group by j.dwname,j.stepname) o 
)p
group by dwname ) x,
dbo.Organize y
where x.dwname =y.name
)";

        public DataTable GetPagerData(out int allPages, out int count, string query, int pageIndex, int pageSize) {
            StringBuilder where = new StringBuilder();
            List<SqlParameter> parList = new List<SqlParameter>();
            //Temp temp = RoadFlow.Utility.ObjectExpand.JsonConvertModel<Temp>(query);
            Temp temp = query.JsonConvertModel<Temp>();
            if (temp.TimeBegin.IsNullOrEmpty() && temp.TimeEnd.IsNullOrEmpty()) {
                table = String.Format(table, string.Empty, string.Empty, string.Empty);
            } else {
                string v1 = string.Empty;
                string v2 = string.Empty;
                if(!temp.TimeBegin.IsNullOrEmpty()){//有开始时间
                    v1 += " and [completedtime1]>='" + temp.TimeBegin+"'"; 
                    v2 += " and [ReceiveTime]>='" + temp.TimeBegin + "'";
                }
                if (!temp.TimeEnd.IsNullOrEmpty()) {//有结束时间
                    v1 += " and [completedtime1]<='" + temp.TimeEnd + "'";
                    v2 += " and [ReceiveTime]<='" + temp.TimeEnd + "'";
                }
                table = String.Format(table, v1, v2,v2);
            }
            
            if (query.IsNullOrEmpty()||query=="{}") {
                count = Int32.Parse(dbHelper.GetFieldValue("select Count(1) Count from " + table + " z2"));
            } else {
                count = Int32.Parse(dbHelper.GetFieldValue("select Count(1) Count from " + table + " z2", parList.ToArray()));
            }
            if (count % pageSize == 0) {//除尽
                allPages = count / pageSize;
            } else {
                allPages = count / pageSize + 1;
            }
            if (pageIndex > allPages) {
                pageIndex = allPages;
            }
            if (pageIndex <= 1) {
                pageIndex = 1;
            }
            string sql = MyExtensions.GetPaging(table, where.ToString(), pageSize, pageIndex, "Number");

            return dbHelper.GetDataTable(sql, parList.ToArray());
        }

        public class Temp {
            public string TimeBegin { get; set; }
            public string TimeEnd { get; set; }
        }

    }
}