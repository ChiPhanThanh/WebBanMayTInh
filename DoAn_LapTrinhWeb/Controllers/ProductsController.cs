using DoAn_LapTrinhWeb.Common;
using DoAn_LapTrinhWeb.Model;
using DoAn_LapTrinhWeb.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;

namespace DoAn_LapTrinhWeb.Controllers
{
    public class ProductsController : Controller
    {
        private DbContext db = new DbContext();
        public ActionResult Listgenreproductlaptop(int id,int ? page)
        {
            List<Product> listgenreproduct = db.Products.Where(n =>n.status =="1" && n.genre_id == id && n.type == ProductType.LAPTOP).ToList();
            string strName = db.Genres.SingleOrDefault(m => m.status=="1" && m.genre_id == id).genre_name;
            ViewBag.Genrename = strName;
            ViewBag.listgenreproduct = listgenreproduct;        
            return View("Listgenreproductlaptop", GetlistProduct(page));
        }
        //đánh số trang
        private IPagedList GetlistProduct(int? page)
        {
            int pageSize = 9; //1 trang hiện thỉ tối đa 9 sản phẩm
            int pageNumber = (page ?? 1); //đánh số trang
            var list = db.Products.OrderBy(m => m.product_name)
                .ToPagedList(pageNumber, pageSize);
            return list;
        }
        //list brand product
        public ActionResult Listbrandproductlaptop(int id, int? page)
        {
            List<Product> listbrandproduct = db.Products.Where(n => n.genre_id == id && n.status=="1").ToList();
            string strBrandName = db.Brands.SingleOrDefault(m => m.brand_id == id && m.status=="1").brand_name;
            ViewBag.Brandname = strBrandName;
            ViewBag.listbrandproduct = listbrandproduct;
            return View(GetProduct(m => m.status == "1" && m.type == ProductType.LAPTOP, page));
        }

        //lấy danh sách laptop
        public ActionResult Laptop(int? page)
        {
            ViewBag.Type = "Laptop";
            return View("Product", GetProduct(m => m.status == "1" && m.type == ProductType.LAPTOP, page));
           
        }
        //xem chi tiết từng sản phẩm
        //[Route("sanpham1/{slug}-{id:int}")]
        public ActionResult ProductDetail(int id)
        {
            var product = db.Products.SingleOrDefault(m =>m.status=="1" && m.product_id == id);
            if (product == null)
            {
                return Redirect("/");
            }
            //sản phẩm liên quan
            List<Product> relatedproduct = db.Products.Where(item => item.status == "1" && item.product_id != product.product_id && item.genre_id==product.genre_id).Take(3).ToList();
            ViewBag.relatedproduct = relatedproduct;
            List<Feedback> rating = db.Feedbacks.Where(m => m.stastus == "1").ToList();
            ViewBag.rating = rating;
            return View(product);
        }
        //lấy danh sách phụ kiện
        public ActionResult LaptopAccessories(int? page)
        {
            ViewBag.Type = "Phụ kiện";
            return View("Product", GetProduct(m => m.status == "1" && m.type == ProductType.ACCESSORY, page));
        }
        //Tìm kiếm sản phẩm
        public ActionResult SearchResult(string s, int? page)
        {
            ViewBag.Type = "Tìm kiếm";
            return View("Product", GetProduct(m => m.status == "1" && m.product_name.Contains(s), page));
        }   
        //đánh số trang
        private IPagedList GetProduct(Expression<Func<Product, bool>> expr, int? page)
        {
            int pageSize = 9; //1 trang hiện thỉ tối đa 9 sản phẩm
            int pageNumber = (page ?? 1); //đánh số trang
            var list = db.Products.Where(expr)
                .OrderByDescending(m => m.create_at)
                .ToPagedList(pageNumber, pageSize);
            return list;
        }
        // lấy danh sách sản phẩm
        public PartialViewResult Genrelistpartial()
        {
            //lay ds loai san pham
            return PartialView("Genrelistpartial", db.Genres.Take(7).ToList());
        }
        public PartialViewResult Brandlistpartial()
        { 
            //lay ds loai san pham
            return PartialView("Brandlistpartial",db.Brands.Take(7).ToList());
        }

    }
}