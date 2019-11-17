using System.Web;
using System.Web.Optimization;

namespace ComicsLibrary
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap/bootstrap.js"));

            bundles.Add(new ScriptBundle("~/bundles/Comics").Include(
                    "~/Scripts/jquery/jquery-{version}.js",
                    "~/Scripts/KnockoutJS/knockout-{version}.js",
                    "~/Scripts/KnockoutJS/ko-scrollHandler.js",
                    "~/Scripts/MomentJS/moment.min.js",
                    "~/Scripts/RequireJS/require.js",
                    "~/Scripts/comics/common.js",
                    "~/Scripts/comics/series.js",
                    "~/Scripts/comics/latest.js",
                    "~/Scripts/comics/current.js",
                    "~/Scripts/comics/library.js",
                    "~/Scripts/comics/search.js",
                    "~/Scripts/comics/index.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/site.css"));
        }
    }
}
