using Show.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace manage.Models {
    //活动
    public class Context : DbContext {
        private static string con = "name=PlatformConnection";
        public Context() : base(con) { Database.SetInitializer<Context>(null); }

        //去掉表名复数约定
        protected override void OnModelCreating(DbModelBuilder modelBuilder) {
            modelBuilder.Conventions.Remove<System.Data.Entity.ModelConfiguration.Conventions.PluralizingTableNameConvention>();
        }

        public System.Data.Entity.DbSet<ArticleModel> Articles { get; set; }

        public System.Data.Entity.DbSet<DictionaryModel> Dictionarys { get; set; }
        public System.Data.Entity.DbSet<BuildingModel> BuildingsAndBuildingMonthInfo { get; set; }


    }
}
