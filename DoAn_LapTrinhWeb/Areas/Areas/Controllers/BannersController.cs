using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using DoAn_LapTrinhWeb.Model;

namespace DoAn_LapTrinhWeb.Areas.Areas.Controllers
{
    public class BannersController : BaseController
    {
        private readonly DbContext db = new DbContext();

        // GET: Areas/Banners
        public ActionResult Index()
        {
            ViewBag.countTrash = db.Banners.Where(a => a.status == "0").Count(); //  đếm tổng sp có trong thùng rác
            return View(db.Banners.Where(m => m.status == "1").OrderByDescending(m => m.create_at).ToList());
        }

        public ActionResult Trash()
        {
            return View(db.Banners.Where(m => m.status == "0").OrderByDescending(m => m.create_at).ToList());
        }

        // GET: Areas/Banners/Details/5
        public ActionResult Details(int? id)
        {
            var banner = db.Banners.SingleOrDefault(a => a.banner_id == id);
            if (banner == null || id == null)
            {
                Notification.set_flash("Không tồn tại! (ID = " + id + ")", "warning");
                return RedirectToAction("Index");
            }

            return View(banner);
        }

        // GET: Areas/Banners/Create
        public ActionResult Create()
        {
            ViewBag.countTrash = db.Banners.Where(a => a.status == "0").Count(); //  đếm tổng sp có trong thùng rác
            return View();
        }

        // POST: Areas/Banners/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Banner banner)
        {
            if (ModelState.IsValid)
            {
                var strSlug = banner.banner_name.ToAscii();
                banner.slug = strSlug;

                banner.create_at = DateTime.Now;
                banner.create_by = Session["UserName"].ToString();
                banner.update_at = DateTime.Now;
                banner.update_by = Session["UserName"].ToString();

                db.Banners.Add(banner);
                db.SaveChanges();

                Notification.set_flash("Thêm mới thành công!", "success");
                return RedirectToAction("Index");
            }

            return View(banner);
        }

        // GET: Areas/Banners/Edit/5
        public ActionResult Edit(int? id)
        {
            ViewBag.countTrash = db.Banners.Where(m => m.status == "0").Count();
            var banner = db.Banners.SingleOrDefault(a => a.banner_id == id);
            if (banner == null || id == null)
            {
                Notification.set_flash("Không tồn tại! (ID = " + id + ")", "warning");
                return RedirectToAction("Index");
            }

            return View(banner);
        }

        // POST: Areas/Banners/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Banner banner)
        {
            if (ModelState.IsValid)
            {
                var strSlug = banner.banner_name.ToAscii();
                banner.slug = strSlug;

                banner.update_at = DateTime.Now;
                banner.update_by = Session["UserName"].ToString();

                db.Entry(banner).State = EntityState.Modified;
                db.SaveChanges();

                Notification.set_flash("Đã cập nhật lại thông tin!", "success");
                return RedirectToAction("Index");
            }

            return View(banner);
        }

        public ActionResult DelTrash(int? id) //bỏ sp vào thùng rác
        {
            var banner = db.Banners.SingleOrDefault(a => a.banner_id == id);
            if (banner == null || id == null)
            {
                Notification.set_flash("Không tồn tại! (ID = " + id + ")", "warning");
                return RedirectToAction("Index");
            }

            banner.status = "0";

            banner.update_at = DateTime.Now;
            banner.update_by = Session["UserName"].ToString();

            db.Entry(banner).State = EntityState.Modified;
            db.SaveChanges();

            Notification.set_flash("Đã chuyển vào thùng rác! (ID = " + id + ")", "success");
            return RedirectToAction("Index");
        }

        public ActionResult Undo(int? id) // khôi phục từ thùng rác
        {
            var banner = db.Banners.SingleOrDefault(a => a.banner_id == id);
            if (banner == null || id == null)
            {
                Notification.set_flash("Không tồn tại! (ID = " + id + ")", "warning");
                return RedirectToAction("Index");
            }

            banner.status = "1";

            banner.update_at = DateTime.Now;
            banner.update_by = Session["UserName"].ToString();

            db.Entry(banner).State = EntityState.Modified;
            db.SaveChanges();

            Notification.set_flash("Khôi phục thành công! (ID = " + id + ")", "success");
            return RedirectToAction("Trash");
        }

        // GET: Areas/Banners/Delete/5
        public ActionResult Delete(int? id)
        {
            var banner = db.Banners.SingleOrDefault(a => a.banner_id == id);
            if (banner == null || id == null)
            {
                Notification.set_flash("Không tồn tại! (ID = " + id + ")", "warning");
                return RedirectToAction("Trash");
            }

            return View(banner);
        }

        // POST: Areas/Banners/Delete/5
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var banner = db.Banners.SingleOrDefault(a => a.banner_id == id);
            db.Banners.Remove(banner);
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