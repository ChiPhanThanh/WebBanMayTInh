using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using DoAn_LapTrinhWeb.Common.Helpers;
using DoAn_LapTrinhWeb.Model;
using PagedList;

namespace DoAn_LapTrinhWeb.Areas.Areas.Controllers
{
    public class DiscountsController : BaseController
    {
        private readonly DbContext _db = new DbContext();

        // GET: Areas/Discounts
        public ActionResult Index(string searchString,int? size, int? page)
        {
            var pageSize = (size ?? 10);
            var pageNumber = (page ?? 1);
            ViewBag.countTrash = _db.Discounts.Count(a => a.status == "0");

            var list = from a in _db.Discounts
                where a.status != "0"
                orderby a.create_at descending
                select a;
            if (!string.IsNullOrEmpty(searchString))
            {
                list = from a in _db.Discounts
                    where a.status != "0" && a.discount_name.Contains(searchString)
                    orderby a.create_at descending
                    select a;
            }
            return View(list.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult Trash(string searchString,int? size, int? page)
        {
            var pageSize = (size ?? 10);
            var pageNumber = (page ?? 1);
            ViewBag.countTrash = _db.Discounts.Count(a => a.status == "0");

            var list = from a in _db.Discounts
                where a.status == "0"
                orderby a.create_at descending
                select a;
            if (!string.IsNullOrEmpty(searchString))
            {
                list = from a in _db.Discounts
                    where a.status != "0" && a.discount_name.Contains(searchString)
                    orderby a.create_at descending
                    select a;
            }
            return View(list.ToPagedList(pageNumber, pageSize));
        }

        // GET: Areas/Discounts/Details/5
        public ActionResult Details(int? id)
        {
            var discount = _db.Discounts.SingleOrDefault(a => a.disscount_id == id);
            if (discount == null || id == null)
            {
                Notification.set_flash("Không tồn tại! (ID = " + id + ")", "warning");
                return RedirectToAction("Index");
            }

            return View(discount);
        }

        // GET: Areas/Discounts/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Areas/Discounts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Discount discount)
        {
            try
            {
                discount.create_at = DateTime.Now;
                discount.create_by = User.Identity.GetUsername();
                discount.update_at = DateTime.Now;
                discount.update_by = User.Identity.GetUsername();
                _db.Discounts.Add(discount);
                _db.SaveChanges();
                Notification.set_flash("Thêm mới thành công!", "success");
                return RedirectToAction("Index");
            }
            catch
            {
                Notification.set_flash("Lỗi!", "danger");
            }
            return View(discount);
        }

        // GET: Areas/Discounts/Edit/5
        public ActionResult Edit(int? id)
        {
            var discount = _db.Discounts.SingleOrDefault(a => a.disscount_id == id);
            if (discount != null && id != null) return View(discount);
            Notification.set_flash("Không tồn tại! (ID = " + id + ")", "warning");
            return RedirectToAction("Index");
        }

        // POST: Areas/Discounts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Discount discount)
        {
            try
            {
                discount.update_at = DateTime.Now;
                discount.update_by = User.Identity.GetUsername();
                _db.Entry(discount).State = EntityState.Modified;
                _db.SaveChanges();
                Notification.set_flash("Đã cập nhật lại thông tin!", "success");
                return RedirectToAction("Index");
            }
            catch
            {
                Notification.set_flash("Lỗi!", "danger");
            }
            return View(discount);
        }

        public ActionResult DelTrash(int? id) //bỏ sp vào thùng rác
        {
            var discount = _db.Discounts.SingleOrDefault(a => a.disscount_id == id);
            if (discount == null || id == null)
            {
                Notification.set_flash("Không tồn tại! (ID = " + id + ")", "warning");
                return RedirectToAction("Index");
            }
            discount.status = "0";
            discount.update_at = DateTime.Now;
            discount.update_by = User.Identity.GetUsername();
            _db.Entry(discount).State = EntityState.Modified;
            _db.SaveChanges();
            Notification.set_flash("Đã chuyển vào thùng rác! (ID = " + id + ")", "success");
            return RedirectToAction("Index");
        }

        public ActionResult Undo(int? id) // khôi phục từ thùng rác
        {
            var discount = _db.Discounts.SingleOrDefault(a => a.disscount_id == id);
            if (discount == null || id == null)
            {
                Notification.set_flash("Không tồn tại! (ID = " + id + ")", "warning");
                return RedirectToAction("Index");
            }
            discount.status = "1";
            discount.update_at = DateTime.Now;
            discount.update_by = User.Identity.GetUsername();
            _db.Entry(discount).State = EntityState.Modified;
            _db.SaveChanges();
            Notification.set_flash("Khôi phục thành công! (ID = " + id + ")", "success");
            return RedirectToAction("Trash");
        }

        // GET: Areas/Discounts/Delete/5
        public ActionResult Delete(int? id)
        {
            var discount = _db.Discounts.SingleOrDefault(a => a.disscount_id == id);
            if (discount == null || id == null)
            {
                Notification.set_flash("Không tồn tại! (ID = " + id + ")", "warning");
                return RedirectToAction("Trash");
            }

            return View(discount);
        }

        // POST: Areas/Discounts/Delete/5
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var discount = _db.Discounts.SingleOrDefault(a => a.disscount_id == id);
            _db.Discounts.Remove(discount ?? throw new InvalidOperationException());
            _db.SaveChanges();

            Notification.set_flash("Đã xoá vĩnh viễn! (ID = " + id + ")", "danger");
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) _db.Dispose();
            base.Dispose(disposing);
        }
    }
}