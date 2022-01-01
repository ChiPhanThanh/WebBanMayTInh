using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using DoAn_LapTrinhWeb.Model;

namespace DoAn_LapTrinhWeb.Areas.Areas.Controllers
{
    public class DeliveriesController : BaseController
    {
        private readonly DbContext db = new DbContext();

        // GET: Areas/Deliveries
        public ActionResult Index()
        {
            ViewBag.countTrash = db.Deliveries.Where(a => a.status == "0").Count(); //  đếm tổng sp có trong thùng rác
            return View(db.Deliveries.Where(m => m.status == "1").OrderByDescending(m => m.create_at).ToList());
        }

        // GET: Areas/Deliveries/Trash
        public ActionResult Trash()
        {
            return View(db.Deliveries.Where(m => m.status == "0").OrderByDescending(m => m.create_at).ToList());
        }

        // GET: Areas/Deliveries/Details/5
        public ActionResult Details(int? id)
        {
            var delivery = db.Deliveries.SingleOrDefault(a => a.delivery_id == id);
            if (delivery == null || id == null)
            {
                Notification.set_flash("Không tồn tại! (ID = " + id + ")", "warning");
                return RedirectToAction("Index");
            }

            return View(delivery);
        }

        // GET: Areas/Deliveries/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Areas/Deliveries/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Delivery delivery)
        {
            if (ModelState.IsValid)
            {
                delivery.create_at = DateTime.Now;
                delivery.create_by = Session["UserName"].ToString();
                delivery.update_at = DateTime.Now;
                delivery.update_by = Session["UserName"].ToString();

                db.Deliveries.Add(delivery);
                db.SaveChanges();

                Notification.set_flash("Thêm mới thành công!", "success");
                return RedirectToAction("Index");
            }

            return View(delivery);
        }

        // GET: Areas/Deliveries/Edit/5
        public ActionResult Edit(int? id)
        {
            ViewBag.countTrash = db.Deliveries.Where(m => m.status == "0").Count();
            var delivery = db.Deliveries.SingleOrDefault(a => a.delivery_id == id);
            if (delivery == null || id == null)
            {
                Notification.set_flash("Không tồn tại! (ID = " + id + ")", "warning");
                return RedirectToAction("Index");
            }

            return View(delivery);
        }

        // POST: Areas/Deliveries/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Delivery delivery)
        {
            if (ModelState.IsValid)
            {
                delivery.update_at = DateTime.Now;
                delivery.update_by = Session["UserName"].ToString();

                db.Entry(delivery).State = EntityState.Modified;
                db.SaveChanges();

                Notification.set_flash("Đã cập nhật lại thông tin!", "success");
                return RedirectToAction("Index");
            }

            return View(delivery);
        }

        public ActionResult DelTrash(int? id) //bỏ sp vào thùng rác
        {
            var delivery = db.Deliveries.SingleOrDefault(a => a.delivery_id == id);
            if (delivery == null || id == null)
            {
                Notification.set_flash("Không tồn tại! (ID = " + id + ")", "warning");
                return RedirectToAction("Index");
            }

            delivery.status = "0";

            delivery.update_at = DateTime.Now;
            delivery.update_by = Session["UserName"].ToString();

            db.Entry(delivery).State = EntityState.Modified;
            db.SaveChanges();

            Notification.set_flash("Đã chuyển vào thùng rác! (ID = " + id + ")", "success");
            return RedirectToAction("Index");
        }

        public ActionResult Undo(int? id) // khôi phục từ thùng rác
        {
            var delivery = db.Deliveries.SingleOrDefault(a => a.delivery_id == id);
            if (delivery == null || id == null)
            {
                Notification.set_flash("Không tồn tại! (ID = " + id + ")", "warning");
                return RedirectToAction("Index");
            }

            delivery.status = "1";

            delivery.update_at = DateTime.Now;
            delivery.update_by = Session["UserName"].ToString();

            db.Entry(delivery).State = EntityState.Modified;
            db.SaveChanges();

            Notification.set_flash("Khôi phục thành công! (ID = " + id + ")", "success");
            return RedirectToAction("Trash");
        }

        // GET: Areas/Deliveries/Delete/5
        public ActionResult Delete(int? id)
        {
            var delivery = db.Deliveries.SingleOrDefault(a => a.delivery_id == id);
            if (delivery == null || id == null)
            {
                Notification.set_flash("Không tồn tại! (ID = " + id + ")", "warning");
                return RedirectToAction("Trash");
            }

            return View(delivery);
        }

        // POST: Areas/Deliveries/Delete/5
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var delivery = db.Deliveries.SingleOrDefault(a => a.delivery_id == id);
            db.Deliveries.Remove(delivery);
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