using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using DoAn_LapTrinhWeb.Common.Helpers;
using DoAn_LapTrinhWeb.Models;
using PagedList;

namespace DoAn_LapTrinhWeb.Areas.Areas.Controllers
{
    public class BrandsController : BaseController
    {
        private readonly DbContext _db = new DbContext();

        // GET: Areas/Brands
        public ActionResult Index(string searchString,int? size, int? page)
        {
            var pageSize = (size ?? 10);
            var pageNumber = (page ?? 1);
            ViewBag.countTrash = _db.Brands.Count(a => a.status == "0");

            var list = from a in _db.Brands
                where a.status != "0"
                orderby a.create_at descending
                select a;
            if (!string.IsNullOrEmpty(searchString))
            {
                list = from a in _db.Brands
                    where a.status != "0" && a.brand_name.Contains(searchString)
                    orderby a.create_at descending
                    select a;
            }
            return View(list.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult Trash(string searchString,int? size, int? page)
        {
            var pageSize = (size ?? 10);
            var pageNumber = (page ?? 1);
            ViewBag.countTrash = _db.Brands.Count(a => a.status == "0");

            var list = from a in _db.Brands
                where a.status == "0"
                orderby a.create_at descending
                select a;
            if (!string.IsNullOrEmpty(searchString))
            {
                list = from a in _db.Brands
                    where a.status == "0" && a.brand_name.Contains(searchString)
                    orderby a.create_at descending
                    select a;
            }
            return View(list.ToPagedList(pageNumber, pageSize));
        }

        // GET: Areas/Brands/Details/5
        public ActionResult Details(int? id)
        {
            var brand = _db.Brands.SingleOrDefault(a => a.brand_id == id);
            if (brand != null && id != null) return View(brand);
            Notification.set_flash("Không tồn tại! (ID = " + id + ")", "warning");
            return RedirectToAction("Index");

        }

        // GET: Areas/Brands/Create
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Brand brand)
        {
            try
            {
                var strSlug = brand.brand_name.ToAscii();
                brand.slug = strSlug;

                brand.create_at = DateTime.Now;
                brand.create_by = User.Identity.GetUsername();

                brand.update_at = DateTime.Now;
                brand.update_by = User.Identity.GetUsername();

                _db.Brands.Add(brand);
                _db.SaveChanges();
                Notification.set_flash("Thêm mới nhãn hàng thành công!", "success");
                return RedirectToAction("Index");
            }
            catch
            {
                Notification.set_flash("Lỗi", "danger");
            }
            return View(brand);
        }

        // GET: Areas/Brands/Edit/5
        public ActionResult Edit(int? id)
        {
            ViewBag.countTrash = _db.Brands.Count(m => m.status == "0");
            var brand = _db.Brands.SingleOrDefault(pro => pro.brand_id == id);
            if (brand != null) return View(brand);
            Notification.set_flash("Không tồn tại! (ID = " + id + ")", "warning");
            return RedirectToAction("Index");
            // return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

        }

        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Brand brand)
        {
            try
            {
                brand.update_at = DateTime.Now;
                brand.update_by = User.Identity.GetUsername();
                _db.Entry(brand).State = EntityState.Modified;
                _db.SaveChanges();
                Notification.set_flash("Đã cập nhật lại thông tin!", "success");
                return RedirectToAction("Index");
            }catch
            {
                Notification.set_flash("Lỗi", "danger");
            }
            return View(brand);
        }

        public ActionResult DelTrash(int? id) //bỏ sp vào thùng rác
        {
            var brand = _db.Brands.SingleOrDefault(pro => pro.brand_id == id);
            if (brand == null || id == null)
            {
                Notification.set_flash("Không tồn tại! (ID = " + id + ")", "warning");
                return RedirectToAction("Index");
            }
            brand.status = "0";
            brand.update_at = DateTime.Now;
            brand.update_by = User.Identity.GetUsername();
            _db.Entry(brand).State = EntityState.Modified;
            _db.SaveChanges();
            Notification.set_flash("Đã chuyển vào thùng rác! (ID = " + id + ")", "success");
            return RedirectToAction("Index");
        }

        public ActionResult Undo(int? id) // khôi phục từ thùng rác
        {
            var brand = _db.Brands.SingleOrDefault(pro => pro.brand_id == id);
            if (brand == null || id == null)
            {
                Notification.set_flash("Không tồn tại! (ID = " + id + ")", "warning");
                return RedirectToAction("Index");
            }
            brand.status = "1";
            brand.update_at = DateTime.Now;
            brand.update_by = User.Identity.GetUsername();
            _db.Entry(brand).State = EntityState.Modified;
            _db.SaveChanges();
            Notification.set_flash("Khôi phục thành công! (ID = " + id + ")", "success");
            return RedirectToAction("Trash");
        }

        // GET: Areas/Brands/Delete/5
        public ActionResult Delete(int? id) // xoá vĩnh viễn sp
        {
            var brand = _db.Brands.SingleOrDefault(pro => pro.brand_id == id);
            if (brand != null && id != null) return View(brand);
            Notification.set_flash("Không tồn tại! (ID = " + id + ")", "warning");
            return RedirectToAction("Trash");

        }

        // POST: Areas/Brands/Delete/5
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var brand = _db.Brands.SingleOrDefault(pro => pro.brand_id == id);
            _db.Brands.Remove(brand ?? throw new InvalidOperationException());
            _db.SaveChanges();
            Notification.set_flash("Đã xoá vĩnh viễn thương hiệu!", "danger");
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) _db.Dispose();
            base.Dispose(disposing);
        }
    }
}