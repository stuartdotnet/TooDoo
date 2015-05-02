using System.Web;
using System.Web.Optimization;

namespace TooDoo
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/Libs/jquery-{version}.js",
                        "~/Scripts/Libs/jquery-ui-{version}.custom.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/Libs/jquery.validate*",
                        "~/Scripts/ToDoList/EditToDo.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/Libs/bootstrap.js",
                      "~/Scripts/Libs/respond.js"));

            bundles.Add(new ScriptBundle("~/bundles/todo").Include(
                      "~/Scripts/ToDoList/TodoList.js"));
            
            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/smoothness/jquery-ui-1.10.4.custom.css",
                      "~/Content/site.css"));
        }
    }
}
