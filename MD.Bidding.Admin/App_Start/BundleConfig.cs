using System.Web;
using System.Web;
using System.Web.Optimization;
using System.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;

namespace MD.Bidding.Admin
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));

            var bundleStyles = new StyleBundle("~/bundles/app_styles")
                   .Include("~/bower_components/angular-notify/dist/angular-notify.min.css")
                   .Include("~/bower_components/fontawesome/css/font-awesome.css")
                   .Include("~/bower_components/metisMenu/dist/metisMenu.css")
                   .Include("~/bower_components/animate.css/animate.css")
                   .Include("~/bower_components/sweetalert/lib/sweet-alert.css")
                   .Include("~/bower_components/fullcalendar/dist/fullcalendar.min.css")
                   .Include("~/bower_components/bootstrap/dist/css/bootstrap.css")
                   .Include("~/bower_components/summernote/dist/summernote.css")
                   .Include("~/bower_components/ng-grid/ng-grid.min.css")
                   .Include("~/bower_components/angular-ui-tree/dist/angular-ui-tree.min.css")
                   .Include("~/bower_components/bootstrap-tour/build/css/bootstrap-tour.min.css")
                   .Include("~/bower_components/datatables_plugins/integration/bootstrap/3/dataTables.bootstrap.css")
                   .Include("~/bower_components/angular-xeditable/dist/css/xeditable.css")
                   .Include("~/bower_components/ui-select/dist/select.min.css")
                   .Include("~/bower_components/bootstrap-touchspin/dist/jquery.bootstrap-touchspin.min.css")
                   .Include("~/bower_components/awesome-bootstrap-checkbox/awesome-bootstrap-checkbox.css")
                   .Include("~/bower_components/blueimp-gallery/css/blueimp-gallery.min.css")
                   .Include("~/bower_components/angular-ui-grid/ui-grid.min.css")
                   .Include("~/bower_components/iCheck/skins/square/green.css")
                   .Include("~/bower_components/iCheck/skins/square/purple.css")
                   .Include("~/fonts/pe-icon-7-stroke/css/pe-icon-7-stroke.css")
                   .Include("~/fonts/pe-icon-7-stroke/css/helper.css")
                   .Include("~/styles/ng-table.min.css")
                   .Include("~/app/styles/isteven-multi-select.css")
                   .Include("~/styles/style.css");

            bundleStyles.Orderer = new NonOrderingBundleOrderer();
            bundles.Add(bundleStyles);

            BundleTable.EnableOptimizations = Convert.ToBoolean(ConfigurationManager.AppSettings["BundleTable.EnableOptimizations"]);
        }
    }

    class NonOrderingBundleOrderer : IBundleOrderer
    {
        public IEnumerable<BundleFile> OrderFiles(BundleContext context, IEnumerable<BundleFile> files)
        {
            return files;
        }
    }
}
