using System.Web.Mvc;

namespace DoAn_LapTrinhWeb.Areas.Areas
{
    public class AreasAreaRegistration : AreaRegistration
    {
        public override string AreaName => "Areas";

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Areas_default",
                "Admin/{controller}/{action}/{id}",
                new {action = "Index", Controller = "Dashboard", id = UrlParameter.Optional}
            );
        }
    }
}