using System.Web;
using System.Web.Optimization;

namespace Show {
    public class BundleConfig {
        // 有关绑定的详细信息，请访问 http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles) {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                "~/Scripts/jquery.js",
                "~/Scripts/jquery.SuperSlide.js",
                "~/Scripts/index.js"
                      ));
            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/base.css",
                      "~/Content/style.css"
                      ));
        }
    }
}
