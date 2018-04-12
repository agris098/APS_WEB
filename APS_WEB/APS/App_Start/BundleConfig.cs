using System.Web;
using System.Web.Optimization;

namespace APS
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                        "~/Scripts/bootstrap.js",
                        "~/Scripts/respond.js"));

            bundles.Add(new ScriptBundle("~/bundles/chat").Include(
            "~/Scripts/jquery.signalR-2.2.3.min.js",
            "~/Scripts/chat/chat.js"));

            bundles.Add(new ScriptBundle("~/bundles/Classifields").Include(
                        "~/Scripts/classifields-manager.js",
                        "~/Scripts/section.js"));
            bundles.Add(new ScriptBundle("~/bundles/classified-comments").Include(
            "~/Scripts/Classified/comments.js"));

            bundles.Add(new ScriptBundle("~/bundles/main").Include(
            "~/Scripts/main.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                        "~/Content/bootstrap.css",
                        "~/Content/site.css"));

            bundles.Add(new StyleBundle("~/Content/chat").Include(
            "~/Content/chat/chat.css"));

            bundles.Add(new StyleBundle("~/Content/Classifields").Include(
                        "~/Content/classifields.css",
                        "~/Content/section.css"));

            bundles.Add(new StyleBundle("~/Content/main").Include(
            "~/Content/main.css"));
            bundles.Add(new StyleBundle("~/Content/comments").Include(
            "~/Content/comments/classified-comments.css"));
        }
    }
}
