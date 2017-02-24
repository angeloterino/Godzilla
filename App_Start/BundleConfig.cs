using System.Web;
using System.Web.Optimization;

namespace StrawmanApp
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                        "~/Scripts/jquery-ui-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.unobtrusive*",
                        "~/Scripts/jquery.validate*"));
            bundles.Add(new ScriptBundle("~/custom/js").Include(
                        "~/Scripts/custom.js"));
            bundles.Add(new ScriptBundle("~/jansybootstrap/jquery").Include(
                        "~/Scripts/jansybootstrap/jansy-bootstrap.*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));
            bundles.Add(new ScriptBundle("~/bundles/comments").Include(
                        "~/Scripts/comments/comments*"));
            bundles.Add(new ScriptBundle("~/bundles/boyForms").Include(
                        "~/Scripts/forms/boyForms*"));
            bundles.Add(new ScriptBundle("~/bundles/commentsForm").Include(
                        "~/Scripts/forms/commentsForm*"));
            bundles.Add(new ScriptBundle("~/bundles/tinymce")                        
                        .IncludeDirectory(
                        "~/Scripts/tinymce","*.js",true));
            bundles.Add(new StyleBundle("~/Content/tinymce")
                        .IncludeDirectory(
                        "~/Scripts/tinymce", "*.css", true));
            bundles.Add(new StyleBundle("~/Content/css").Include(
                        "~/Content/site.css",
                        "~/Content/CustomStyle.css"));

            bundles.Add(new StyleBundle("~/bundles/yeti").Include(
                        "~/Content/yeti/bootstrap.*", new System.Web.Optimization.CssRewriteUrlTransform()));

            bundles.Add(new StyleBundle("~/bundles/fontawesome").Include(
                        "~/Content/fontawesome/font-awesome.*", new System.Web.Optimization.CssRewriteUrlTransform()));

            bundles.Add(new StyleBundle("~/bundles/jansybootstrap/css").Include(
                        "~/Content/jansybootstrap/jansy-bootstrap.*", new System.Web.Optimization.CssRewriteUrlTransform()));

            bundles.Add(new StyleBundle("~/Content/themes/base/css").Include(
                        "~/Content/themes/base/*.css"));
        }
    }
}