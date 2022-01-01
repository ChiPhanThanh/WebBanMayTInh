using DoAn_LapTrinhWeb.Common;
using System.Web.Mvc;
using System.Web.Security;


namespace DoAn_LapTrinhWeb.Areas.Areas.Controllers
{

    //[Authorize(Roles = Const.ROLE_ADMIN_NAME)]  // Chỉ chấp nhận account đã đăng nhập và có role admin
 
    public class BaseController : Controller
    {
        public BaseController()
        {


            if (!System.Web.HttpContext.Current.User.IsInRole("Admin"))
            {
                System.Web.HttpContext.Current.Response.Redirect("~/Home/Index");
            }

        }
        //đăng xuất admin quay về trang chủ
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return Redirect("~/Home/Index");
        }
        //chuyển từ trang admin sang trang thông tin cá nhân
        public ActionResult ViewProfile()
        {
           
            return Redirect("~/Account/Editprofile");
        }
        //chuyển từ trang admin sang trang chủ
        public ActionResult BackToHome()
        {

            return Redirect("~/Home/Index");

        }
    }
}