using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace RoadFlow.Data.Factory {
    public class Factory {
        public static Data.Interface.IAppLibrary GetAppLibrary() {
            return new Data.MSSQL.AppLibrary();
        }

        public static Data.Interface.IQueryDesign GetQueryDesign() {
            return new Data.MSSQL.QueryDesign();
        }

        public static Data.Interface.IDBConnection GetDBConnection() {
            return new Data.MSSQL.DBConnection();
        }

        public static Data.Interface.IDictionary GetDictionary() {
            return new Data.MSSQL.Dictionary();
        }

        public static Data.Interface.ILog GetLog() {
            return new Data.MSSQL.Log();
        }

        public static Data.Interface.ITableAttribute GetTableAttribute() {
            return new Data.MSSQL.TableAttribute();
        }
        public static Data.Interface.IOrganize GetOrganize() {
            return new Data.MSSQL.Organize();
        }

        public static Data.Interface.IRole GetRole() {
            return new Data.MSSQL.Role();
        }

        public static Data.Interface.IRoleApp GetRoleApp() {
            return new Data.MSSQL.RoleApp();
        }

        public static Data.Interface.IUsers GetUsers() {
            return new Data.MSSQL.Users();
        }

        public static Data.Interface.IUsersApp GetUsersApp() {
            return new Data.MSSQL.UsersApp();
        }

        public static Data.Interface.IUsersInfo GetUsersInfo() {
            return new Data.MSSQL.UsersInfo();
        }

        public static Data.Interface.IUsersRelation GetUsersRelation() {
            return new Data.MSSQL.UsersRelation();
        }

        public static Data.Interface.IUsersRole GetUsersRole() {
            return new Data.MSSQL.UsersRole();
        }

        public static Data.Interface.IWorkFlow GetWorkFlow() {
            return new Data.MSSQL.WorkFlow();
        }

        public static Data.Interface.IWorkFlowArchives GetWorkFlowArchives() {
            return new Data.MSSQL.WorkFlowArchives();
        }

        public static Data.Interface.IWorkFlowButtons GetWorkFlowButtons() {
            return new Data.MSSQL.WorkFlowButtons();
        }

        public static Data.Interface.IWorkFlowComment GetWorkFlowComment() {
            return new Data.MSSQL.WorkFlowComment();
        }

        public static Data.Interface.IWorkFlowData GetWorkFlowData() {
            return new Data.MSSQL.WorkFlowData();
        }

        public static Data.Interface.IWorkFlowDelegation GetWorkFlowDelegation() {
            return new Data.MSSQL.WorkFlowDelegation();
        }

        public static Data.Interface.IWorkFlowForm GetWorkFlowForm() {
            return new Data.MSSQL.WorkFlowForm();
        }

        public static Data.Interface.IWorkFlowTask GetWorkFlowTask() {
            return new Data.MSSQL.WorkFlowTask();
        }

        public static Data.Interface.IWorkGroup GetWorkGroup() {
            return new Data.MSSQL.WorkGroup();
        }

        public static Data.Interface.ISMS GetSMS() {
            return new Data.MSSQL.SMS();
        }

        public static Data.Interface.IPost GetPost() {
            return new Data.MSSQL.Post();
        }

        public static Data.Interface.IActivityStatistics GetActivityStatistics() {
            return new Data.MSSQL.ActivityStatistics();
        }

        public static Data.Interface.IBase GetBase(string tableName,string order) {
            return new Data.MSSQL.Base(tableName,order);
        }

        public static Data.Interface.IBase GetBase(string tableName,string modifyTableName, string order) {
            return new Data.MSSQL.Base(tableName,modifyTableName, order);
        }

        public static Data.Interface.IExecute GetExecute() {
            return new Data.MSSQL.Execute();
        }
 
    }
}
