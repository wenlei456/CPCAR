using System.Web;
using System.Web.Optimization;
namespace CarParts.Online
{
    public class BundleConfig
    {
        // 有关 Bundling 的详细信息，请访问 http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/Content/jslib").Include(
                                                                   "~/Content/jslib/jquery-1.11.2.js",
                                                                  // "~/Content/jslib/knockout-2.1.0.js",
                                                                  "~/Content/bootstrap/js/bootstrap.min.js",
                                                                  "~/Content/jslib/jquery.validate.js",
                                                                  "~/Content/jslib/jquery.validate.unobtrusive.js",
                                                                   "~/Content/jslib/jquery.cookie.js" 
                                                                   ));
            bundles.Add(new ScriptBundle("~/bundles/js").Include(
                                                               "~/Content/js/common/base.js",
                                                              "~/Content/js/common/service-cart.js",                                                          
                                                                "~/Content/js/Business/BaseBll.js"
                                                               ));
            bundles.Add(new ScriptBundle("~/Content/jsliboff").Include(
                                                                 "~/Content/jslib/jquery-1.11.2.js",
   "~/Content/jslib/jquery.cookie.js"
                                                                 ));
            bundles.Add(new ScriptBundle("~/bundles/jsoff").Include(
                                                               "~/Content/js/common/base.js"
                                                               
                                                             
                                                               ));


            bundles.Add(new StyleBundle("~/Content/csslib").Include(
                                            "~/Content/bootstrap/css/bootstrap.css",
                                            // "~/Content/css/site.css",
                                            "~/Content/css/web.css"
                                            
                                            ));
        }
    }
}