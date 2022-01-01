using System;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using DoAn_LapTrinhWeb.Common.Helpers;
using DoAn_LapTrinhWeb.DTOs;
using DoAn_LapTrinhWeb.Models;
using PagedList;

namespace DoAn_LapTrinhWeb.Areas.Areas.Controllers
{
    public class ProductsAdminController : BaseController
    {
        private readonly DbContext db = new DbContext();

        // GET: Areas/ProductsAdmin
        public ActionResult Index(string searchString, string show, int? size, int? page) // hiển thị tất cả sp online
        {
            var pageSize = size ?? 5;
            var pageNumber = page ?? 1;

            ViewBag.countTrash = db.Products.Where(a => a.status == "0").Count(); //  đếm tổng sp có trong thùng rác
            var list = from a in db.Products
                join c in db.Genres on a.genre_id equals c.genre_id
                join d in db.Brands on a.brand_id equals d.brand_id
                join e in db.Discounts on a.disscount_id equals e.disscount_id
                where a.status == "1"
                orderby a.create_at descending // giảm dần
                select new ProductDTOs
                {
                    product_name = a.product_name,
                    quantity = a.quantity,
                    price = a.price,
                    Image = a.image,
                    genre_name = c.genre_name,
                    brand_name = d.brand_name,
                    product_id = a.product_id,
                    discount_name = e.discount_name
                };
            if (!string.IsNullOrEmpty(searchString))
            {
                if (show.Equals("1"))
                    list = list.Where(s => s.product_name.Contains(searchString));
                else if (show.Equals("2"))
                    list = list.Where(s => s.genre_name.Contains(searchString));
                else if (show.Equals("3")) list = list.Where(s => s.discount_name.Contains(searchString));
            }

            return View(list.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult Trash(string searchString, string show, int? size, int? page) // hiển thị tất cả sp online
        {
            var pageSize = size ?? 5;
            var pageNumber = page ?? 1;

            var list = from a in db.Products
                join c in db.Genres on a.genre_id equals c.genre_id
                join d in db.Brands on a.brand_id equals d.brand_id
                join e in db.Discounts on a.disscount_id equals e.disscount_id
                where a.status == "0"
                orderby a.create_at descending // giảm dần
                select new ProductDTOs
                {
                    product_name = a.product_name,
                    quantity = a.quantity,
                    price = a.price,
                    Image = a.image,
                    genre_name = c.genre_name,
                    brand_name = d.brand_name,
                    product_id = a.product_id,
                    discount_name = e.discount_name
                };

            if (!string.IsNullOrEmpty(searchString))
            {
                if (show.Equals("1"))
                    list = list.Where(s => s.product_name.Contains(searchString));
                else if (show.Equals("2"))
                    list = list.Where(s => s.genre_name.Contains(searchString));
                else
                    list = list.Where(s => s.discount_name.Contains(searchString));
            }

            return View(list.ToPagedList(pageNumber, pageSize));
        }

        // GET: Areas/ProductsAdmin/Details/5

        public ActionResult Details(int? id)
        {
            var product = (from a in db.Products
                join c in db.Genres on a.genre_id equals c.genre_id
                join d in db.Brands on a.brand_id equals d.brand_id
                join e in db.Discounts on a.disscount_id equals e.disscount_id
                where a.product_id == id
                orderby a.create_at descending // giảm dần
                select new ProductDTOs
                {
                    product_name = a.product_name,
                    quantity = a.quantity,
                    price = a.price,
                    Image = a.image,
                    genre_name = c.genre_name,
                    brand_name = d.brand_name,
                    product_id = a.product_id,
                    description = a.description,
                    create_at = a.create_at,
                    create_by = a.create_by,
                    update_at = a.update_at,
                    update_by = a.update_by,
                    discount_price = a.price - e.discount_price,
                    discount_name = e.discount_name,
                    discount_id = e.disscount_id
                }).FirstOrDefault();
            if (product == null || id == null)
            {
                Notification.set_flash("Không tồn tại! (ID = " + id + ")", "warning");
                return RedirectToAction("Index");
            }

            return View(product);
        }

        // GET: Areas/ProductsAdmin/Create
        public ActionResult Create() //Tạo sản phẩm
        {
            ViewBag.ListDiscount =
                new SelectList(db.Discounts.Where(m => m.status != "0"), "disscount_id", "discount_name", 0);
            ViewBag.ListBrand = new SelectList(db.Brands.Where(m => m.status != "0"), "brand_id", "brand_name", 0);
            ViewBag.ListGenre = new SelectList(db.Genres.Where(m => m.status != "0"), "genre_id", "genre_name", 0);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Product product)
        {
            ViewBag.ListDiscount =
                new SelectList(db.Discounts.Where(m => m.status != "0"), "disscount_id", "discount_name", 0);
            ViewBag.ListBrand = new SelectList(db.Brands.Where(m => m.status != "0"), "brand_id", "brand_name", 0);
            ViewBag.ListGenre = new SelectList(db.Genres.Where(m => m.status != "0"), "genre_id", "genre_name", 0);
            try
            {
                if (product.ImageUpload != null)
                {
                    var fileName = Path.GetFileNameWithoutExtension(product.ImageUpload.FileName);
                    var extension = Path.GetExtension(product.ImageUpload.FileName);
                    fileName = fileName + extension;
                    product.image = "/Content/Images/" + fileName;
                    product.ImageUpload.SaveAs(Path.Combine(Server.MapPath("~/Content/Images/"), fileName));
                }
                else
                {
                    product.image = "/Content/Images/no-image.jpg";
                }

                //var strSlug = product.product_name.ToAscii();
                product.status = "1";
                product.view = 0;
                product.buyturn = 0;
                product.type = product.type;

                //product.slug = strSlug;
                product.create_at = DateTime.Now;
                product.create_by = User.Identity.GetUsername();
                product.update_at = DateTime.Now;
                product.update_by = User.Identity.GetUsername();

                db.Products.Add(product);
                db.SaveChanges();
                Notification.set_flash("Thêm mới thành công!", "success");
                return RedirectToAction("Index");
            }
            catch
            {
                Notification.set_flash("Lỗi", "danger");
            }

            return View(product);
        }

        // GET: Areas/ProductsAdmin/Edit/5
        public ActionResult Edit(int? id)
        {
            ViewBag.ListDiscount =
                new SelectList(db.Discounts.Where(m => m.status != "0"), "disscount_id", "discount_name", 0);
            ViewBag.ListBrand = new SelectList(db.Brands.Where(m => m.status != "0"), "brand_id", "brand_name", 0);
            ViewBag.ListGenre = new SelectList(db.Genres.Where(m => m.status != "0"), "genre_id", "genre_name", 0);
            var product = db.Products.FirstOrDefault(x => x.product_id == id);
            if (product == null || id == null)
            {
                Notification.set_flash("Không tồn tại! (ID = " + id + ")", "warning");
                return RedirectToAction("Index");
            }

            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Product productDtOs)
        {
            ViewBag.ListDiscount =
                new SelectList(db.Discounts.Where(m => m.status != "0"), "disscount_id", "discount_name", 0);
            ViewBag.ListBrand = new SelectList(db.Brands.Where(m => m.status != "0"), "brand_id", "brand_name", 0);
            ViewBag.ListGenre = new SelectList(db.Genres.Where(m => m.status != "0"), "genre_id", "genre_name", 0);

            var product = db.Products.SingleOrDefault(x => x.product_id == productDtOs.product_id);
            try
            {
                if (product != null)
                {
                    product.product_name = productDtOs.product_name;
                    product.quantity = productDtOs.quantity;
                    product.description = productDtOs.description;
                    // product.descrip = productDtOs.descrip;
                    product.status = "1";
                    product.price = productDtOs.price;
                    product.specifications = productDtOs.specifications;
                    product.brand_id = productDtOs.brand_id;
                    product.genre_id = productDtOs.genre_id;
                    product.disscount_id = productDtOs.disscount_id;
                    product.type = productDtOs.type;
                    product.update_at = DateTime.Now;
                    product.update_by = User.Identity.GetUsername();
                    db.Entry(product).State = EntityState.Modified;
                }

                db.SaveChanges();
                Notification.set_flash("Đã cập nhật lại thông tin!", "success");
                return RedirectToAction("Index");
            }
            catch
            {
                Notification.set_flash("Lỗi", "danger");
            }

            return View(productDtOs);
        }

        public ActionResult DelTrash(int? id) //bỏ sp vào thùng rác
        {
            var product = db.Products.SingleOrDefault(a => a.product_id == id);
            if (product == null || id == null)
            {
                Notification.set_flash("Không tồn tại! (ID = " + id + ")", "warning");
                return RedirectToAction("Index");
            }

            product.status = "0";
            product.update_at = DateTime.Now;
            product.update_by = User.Identity.Name;
            db.Entry(product).State = EntityState.Modified;
            db.SaveChanges();
            Notification.set_flash("Đã chuyển vào thùng rác! (ID = " + id + ")", "success");
            return RedirectToAction("Index");
        }

        public ActionResult Undo(int? id) // khôi phục từ thùng rác
        {
            var product = db.Products.SingleOrDefault(a => a.product_id == id);
            if (product == null || id == null)
            {
                Notification.set_flash("Không tồn tại! (ID = " + id + ")", "warning");
                return RedirectToAction("Index");
            }

            product.status = "1";
            product.update_at = DateTime.Now;
            product.update_by = User.Identity.Name;
            db.Entry(product).State = EntityState.Modified;
            db.SaveChanges();
            Notification.set_flash("Khôi phục thành công! (ID = " + id + ")", "success");
            return RedirectToAction("Trash");
        }

        // GET: Areas/ProductsAdmin/Delete/5
        public ActionResult Delete(int? id) // xoá vĩnh viễn sp
        {
            var product = db.Products.SingleOrDefault(a => a.product_id == id);
            if (product == null || id == null)
            {
                Notification.set_flash("Không tồn tại! (ID = " + id + ")", "warning");
                return RedirectToAction("Trash");
            }

            return View(product);
        }

        // POST: Areas/ProductsAdmin/Delete/5
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var product = db.Products.SingleOrDefault(a => a.product_id == id);
            db.Products.Remove(product);
            db.SaveChanges();
            Notification.set_flash("Đã xoá vĩnh viễn! (ID = " + id + ")", "danger");
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) db.Dispose();
            base.Dispose(disposing);
        }
    }
}