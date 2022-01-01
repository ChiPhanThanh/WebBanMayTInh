using System.Web.Optimization;

namespace DoAn_LapTrinhWeb
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                "~/Scripts/jquery-{version}.min.js"
            ));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                "~/Scripts/modernizr-*"));
            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                //"~/Scripts/bootstrap.js",
                "~/Scripts/my_js/bootstrap.min.js"
            ));
            //banner slide js
            bundles.Add(new ScriptBundle("~/bundles/slidebanner").Include(
                "~/Scripts/revolution/js/jquery.themepunch.tools.min.js",
                "~/Scripts/revolution/js/jquery.themepunch.revolution.min.js",
                "~/Scripts/revolution/js/extensions/revolution.extension.actions.min.js",
                "~/Scripts/revolution/js/extensions/revolution.extension.carousel.min.js",
                "~/Scripts/revolution/js/extensions/revolution.extension.kenburn.min.js",
                "~/Scripts/revolution/js/extensions/revolution.extension.layeranimation.min.js",
                "~/Scripts/revolution/js/extensions/revolution.extension.migration.min.js",
                "~/Scripts/revolution/js/extensions/revolution.extension.navigation.min.js",
                "~/Scripts/revolution/js/extensions/revolution.extension.parallax.min.js",
                "~/Scripts/revolution/js/extensions/revolution.extension.slideanims.min.js",
                "~/Scripts/revolution/js/extensions/revolution.extension.video.min.js"
            ));
            //Custom js
            bundles.Add(new ScriptBundle("~/bundles/customjs").Include(
                "~/Scripts/my_js/menumaker.js",
                "~/Scripts/my_js/wow.js",
                "~/Scripts/my_js/custom.js"
            ));
            //acount js
            bundles.Add(new ScriptBundle("~/bundles/jsaccount").Include(
                "~/Scripts/my_js/account_js/login.js"
            ));
            // Scroll Back To Top js
            bundles.Add(new ScriptBundle("~/bundles/rollbacktop").Include(
                "~/Scripts/my_js/vanillatop.min.js"
            ));

            //--------------------------------------------------------------------------------------------------------------
            //--------------------------------------------------------------------------------------------------------------
            // Scroll Back To Top css
            bundles.Add(new StyleBundle("~/bundles/rollbacktopcss").Include(
                "~/Content/my_css/vanillatop.min.css"
            ));
            //custom css
            bundles.Add(new StyleBundle("~/bundles/customcss").Include(
                "~/Content/my_css/responsive.css",
                "~/Content/my_css/colors1.css",
                "~/Content/my_css/custom.css",
                "~/Content/my_css/animate.css"
            ));
            //revolution css
            bundles.Add(new StyleBundle("~/bundles/revolution").Include(
                "~/Content/revolution/css/settings.css",
                "~/Content/revolution/css/layers.css",
                "~/Content/revolution/css/navigation.css"
            ));
            //Style css
            bundles.Add(new StyleBundle("~/Content/css").Include(
                "~/Content/bootstrap.min.css",
                "~/Content/site.css"));
        }
    }
}