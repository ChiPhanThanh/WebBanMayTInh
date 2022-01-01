using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace DoAn_LapTrinhWeb
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            //rút gọn link tìm kiếm sản phẩm
            routes.MapRoute(
              name: "search",
              url: "timkiem",
             defaults: new { Controller = "Products", action = "SearchResult" }
           );        
            //rút gọn link chi tiết sản phẩm
            routes.MapRoute(
              name: "chi tiet san pham",
              url: "{slug}-{id}",
             defaults: new { Controller = "Products", action = "ProductDetail"}
           );
            //rút gọn link laptop
            routes.MapRoute(
              name: "Laptop",
              url: "laptop",
             defaults: new { Controller = "Products", action = "Laptop" }
           );
            //rút gọn link thể loại 
            routes.MapRoute(
              name: "list genre product",
              url: "{slug}-{id}",
             defaults: new { Controller = "Products", action = "Listgenreproductlaptop" }
           );
            //rút gọn link phụ kiện
            routes.MapRoute(
              name: "Phu kien",
              url: "phukien",
             defaults: new { Controller = "Products", action = "LaptopAccessories", id = UrlParameter.Optional }
           );
            //rút gọn link phụ kiện
            routes.MapRoute(
              name: "những sản phẩm thuộc thể loại",
              url: "genre-product",
             defaults: new { Controller = "Products", action = "Listgenreproduct", id = UrlParameter.Optional }
           );
            //rút gọn link giỏ hàng
            routes.MapRoute(
               name: "Thanh toan",
               url: "thanhtoan",
               defaults: new { controller = "Cart", action = "Checkout", id = UrlParameter.Optional }
            );
            //rút gọn link giỏ hàng
            routes.MapRoute(
                name: "Gio Hang",
                url: "giohang",
                defaults: new { controller = "Cart", action = "ViewCart", id = UrlParameter.Optional }
            );
            //rút gọn link tin tức
            routes.MapRoute(
              name: "News",
              url: "tintuc",  
              defaults: new { controller = "News", action = "News", id = UrlParameter.Optional }
           );
            //rút gọn link tin tức
            routes.MapRoute(
              name: "News detail",
              url: "chitiettintuc",
              defaults: new { controller = "News", action = "NewsDetail", id = UrlParameter.Optional }
           );
            //rút gọn link khuyến mãi
            routes.MapRoute(
              name: "khuyen mai",
              url: "khuyenmai",
              defaults: new { controller = "Discount", action = "Listbanner", id = UrlParameter.Optional }
           );
            //rút gọn link chi tiet sản phẩm khuyến mãi
            routes.MapRoute(
              name: "chi tiet san pham khuyen mai",
              url: "chitietkhuyenmai",
              defaults: new { controller = "Discount", action = "Bannerdetail", id = UrlParameter.Optional }
           );
            //rút gọn link thong tin ca nhan
            routes.MapRoute(
              name: "Thong tin ca nhan",
              url: "thongtinnguoidung",
              defaults: new { controller = "Account", action = "Editprofile", id = UrlParameter.Optional }
           );
            //rút gọn link forgot password
            routes.MapRoute(
              name: "forgotpassword",
              url: "quenmatkhau",
              defaults: new { controller = "Account", action = "ForgotPassword", id = UrlParameter.Optional }
           );
            //rút gọn link đăng ký
            routes.MapRoute(
                name: "Dang ky",
                url: "dangky",
                defaults: new { controller = "Account", action = "Register", id = UrlParameter.Optional }
            );
            //thay đổi mật khảu
            routes.MapRoute(
              name: "doi mat khau",
              url: "doimatkhau",
              defaults: new { controller = "Account", action = "ChangePassword", id = UrlParameter.Optional }
           );
            //xem chi tiết đơn hàng
            routes.MapRoute(
              name: "chi tiet don hang",
              url: "chitietdonhang",
              defaults: new { controller = "Account", action = "OrderDetail", id = UrlParameter.Optional }
           );
            //rút gọn link quản lý đơn hàng
            routes.MapRoute(
              name: "xem don hang",
              url: "donhang",
              defaults: new { controller = "Account", action = "TrackingOrder", id = UrlParameter.Optional }
           );
            //reset password
            routes.MapRoute(
              name: "Reset password",
              url: "capnhatmatkhau",
              defaults: new { controller = "Account", action = "ResetPassword", id = UrlParameter.Optional }
           );
            //gửi yêu cầu hồ trợ
            routes.MapRoute(
              name: "sent request",
              url: "guiyeucau",
              defaults: new { controller = "Home", action = "SentRequest", id = UrlParameter.Optional }
           );
            //set error 404
            routes.MapRoute(
              name: "Page Not Found",
              url: "pagenotfound/{id}",
              defaults: new { controller = "Home", action = "PageNotFound", id = UrlParameter.Optional }
           );
            //link mặc định khi khởi động
            routes.MapRoute(
             name: "Default",
             url: "{controller}/{action}/{id}",
             defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
         );
        }
    }
}
