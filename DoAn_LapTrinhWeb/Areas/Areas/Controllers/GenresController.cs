using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using DoAn_LapTrinhWeb.Common.Helpers;
using DoAn_LapTrinhWeb.Model;
using PagedList;

namespace DoAn_LapTrinhWeb.Areas.Areas.Controllers
{
    public class GenresController : BaseController
    {
        private readonly DbContext _db = new DbContext();

        // GET: Areas/Genres
        public ActionResult Index(string searchString,int? size, int? page)
        {
            var pageSize = (size ?? 10);
            var pageNumber = (page ?? 1);
            ViewBag.countTrash = _db.Genres.Count(a => a.status == "0");

            var list = from a in _db.Genres
                where a.status != "0"
                orderby a.create_at descending
            select a;
            if (!string.IsNullOrEmpty(searchString))
            {
                list = from a in _db.Genres
                    where a.status != "0" && a.genre_name.Contains(searchString)
                    orderby a.create_at
                    select a;
            }

            return View(list.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult Trash(string searchString,int? size, int? page)
        {
            var pageSize = (size ?? 10);
            var pageNumber = (page ?? 1);

            var list = from a in _db.Genres
                where a.status == "0"
                orderby a.create_at descending
                select a;
            if (!string.IsNullOrEmpty(searchString))
            {
                list = from a in _db.Genres
                    where a.status == "0" && a.genre_name.Contains(searchString)
                    orderby a.create_at
                    select a;
            }

            return View(list.ToPagedList(pageNumber, pageSize));        }

        // GET: Areas/Genres/Details/5
        public ActionResult Details(int? id)
        {
            var genre = _db.Genres.SingleOrDefault(a => a.genre_id == id);
            if (genre == null||id == null)
            {
                Notification.set_flash("Không tồn tại! (ID = " + id + ")", "warning");
                return RedirectToAction("Index");
            }

            return View(genre);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Genre genre)
        {
           try
           {
               var strSlug = genre.genre_name.ToAscii();
               genre.slug = strSlug;

               genre.status = "1";
               genre.create_at = DateTime.Now;
               genre.create_by = User.Identity.GetUsername();
               genre.update_at = DateTime.Now;
               genre.update_by = User.Identity.GetUsername();

               _db.Genres.Add(genre);
               _db.SaveChanges();
               Notification.set_flash("Thêm mới thể loại thành công!", "success");
               return RedirectToAction("Index");
           }
           catch
           {
               Notification.set_flash("Lỗi", "danger");
           }

           return View(genre);
        }

        // GET: Areas/Genres/Edit/5
        public ActionResult Edit(int? id)
        {
            ViewBag.countTrash = _db.Genres.Where(m => m.status == "0").Count();
            var genre = _db.Genres.SingleOrDefault(a => a.genre_id == id);
            if (genre == null)
            {
                Notification.set_flash("404!", "warning");
                return RedirectToAction("Index");
            }

            return View(genre);
        }

        // POST: Areas/Genres/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Genre genre)
        {
            try
            {
                // var strSlug = genre.genre_name.ToAscii();
                // genre.slug = strSlug;

                genre.update_at = DateTime.Now;
                genre.update_by = User.Identity.GetUsername();

                _db.Entry(genre).State = EntityState.Modified;
                _db.SaveChanges();
                Notification.set_flash("Đã cập nhật lại thông tin thể loại!", "success");
                return RedirectToAction("Index");
            }
            catch
            {
                Notification.set_flash("404!", "warning");
            }

            return View(genre);
        }

        public ActionResult DelTrash(int? id)
        {
            var genre = _db.Genres.SingleOrDefault(a => a.genre_id == id);
            //Product product = db.Products.Find(id);
            genre.status = "0";
            genre.update_at = DateTime.Now;
            genre.update_by = User.Identity.GetUsername();
            _db.Entry(genre).State = EntityState.Modified;
            _db.SaveChanges();
            Notification.set_flash("Đã chuyển thể loại vào thùng rác!" + " ID = " + id, "success");
            return RedirectToAction("Index");
        }

        public ActionResult Undo(int? id)
        {
            var genre = _db.Genres.SingleOrDefault(a => a.genre_id == id);

            genre.status = "1";

            genre.update_at = DateTime.Now;
            genre.update_by = User.Identity.GetUsername();
            _db.Entry(genre).State = EntityState.Modified;
            _db.SaveChanges();
            Notification.set_flash("Khôi phục thành công!" + " ID = " + id, "success");
            return RedirectToAction("Trash");
        }

        // GET: Areas/Genres/Delete/5

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                Notification.set_flash("Không tồn tại !", "warning");
                return RedirectToAction("Trash");
            }

            var genre = _db.Genres.SingleOrDefault(a => a.genre_id == id);
            if (genre == null)
            {
                Notification.set_flash("Không tồn tại !", "warning");
                return RedirectToAction("Trash");
            }

            return View(genre);
        }


        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                var genre = _db.Genres.SingleOrDefault(a => a.genre_id == id);
                _db.Genres.Remove(genre);
                _db.SaveChanges();

                Notification.set_flash("Đã xóa vĩnh viễn thể loại!", "danger");
                return RedirectToAction("Index");
            }
            catch
            {
                Notification.set_flash("Còn sản phẩm trong thể loại nên không thể xoá!", "warning");
                return RedirectToAction("Index");

            }



        }
        // POST: Areas/Genres/Delete/5

        protected override void Dispose(bool disposing)
        {
            if (disposing) _db.Dispose();
            base.Dispose(disposing);
        }
    }
}