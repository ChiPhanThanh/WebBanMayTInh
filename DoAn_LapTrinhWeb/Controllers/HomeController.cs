using DoAn_LapTrinhWeb.Common;
using DoAn_LapTrinhWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
// using DoAn_LapTrinhWeb.Models;


namespace DoAn_LapTrinhWeb.Controllers
{
    [HandleError]
    public class HomeController : Controller
    {
        private DbContext _dbContext = new DbContext();

        public ActionResult Index()
        {
            List<Product> sanphamhot = _dbContext.Products.Where(item => item.status == "1"&& item.quantity != "0" && item.genre_id==ProductType.LAPTOP).OrderByDescending(item => item.buyturn).Take(4).ToList();
            ViewBag.sanphamhot = sanphamhot;
            // hiển thị những laptopgaming được mua nhiều nhất
            List<Product> laptopgaming = _dbContext.Products.Where(item => item.status == "1"&& item.quantity != "0" && item.genre_id==Genre.laptopgaming).OrderByDescending(item => item.buyturn).Take(4).ToList();
            ViewBag.laptopgaming = laptopgaming;
            // hiển thị những phụ kiện được mua nhiều nhất
            List<Product> hoctapvanphong = _dbContext.Products.Where(item => item.status == "1"&& item.quantity != "0" && item.genre_id == Genre.hoctapvanphong).OrderByDescending(item => item.buyturn).Take(4).ToList();
            ViewBag.hoctapvanphong = hoctapvanphong;
            // hiển thị những phụ kiện được mua nhiều nhất
            List<Product> dohoakithuat = _dbContext.Products.Where(item => item.status == "1"&& item.quantity != "0" && item.genre_id == Genre.dohoakithuat).OrderByDescending(item => item.buyturn).Take(4).ToList();
            ViewBag.dohoakithuat = dohoakithuat;
            // hiển thị những phụ kiện được mua nhiều nhất
            List<Product> phukien = _dbContext.Products.Where(item => item.status == "1" && item.quantity != "0" && (item.genre_id == Genre.chuotmaytinh
            || item.genre_id == Genre.banphimmaytinh || item.genre_id == Genre.loa || item.genre_id == Genre.usb || item.genre_id == Genre.ocungdidong
            || item.genre_id == Genre.tainghe)).Take(4).OrderByDescending(item => item.buyturn).ToList();
            ViewBag.phukien = phukien;
            // hiển thị những sản phẩm được mua nhiều nhất theo loại cao cấp sang trọng
            List<Product> caocapsangtrong = _dbContext.Products.Where(item => item.status == "1" && item.genre_id == Genre.caocapsangtrong).OrderByDescending(item => item.buyturn).Take(4).ToList();
            ViewBag.caocapsangtrong = caocapsangtrong;
            // Hiển thị laptop theo hãng
           
            return View();       
        }
        
        public ActionResult PageNotFound()
        {
            return View();
        }
        public ActionResult SentRequest()
        {

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SentRequest(Contact contact)
        {
            try
            {
                contact.name = contact.name;
                contact.email = contact.email;
                contact.create_by = "1";
                contact.update_at = DateTime.Now;
                contact.status = "1";
                contact.create_at = DateTime.Now;
                contact.update_by = "1";
                contact.content = contact.content;
                contact.phone = contact.phone;
                _dbContext.Contacts.Add(contact);
                _dbContext.SaveChanges();
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public PartialViewResult Laptop_Module()
        {
               //lay ds loai san pham
            return PartialView("Laptop_Module", _dbContext.Genres.ToList());
        }
    }
}