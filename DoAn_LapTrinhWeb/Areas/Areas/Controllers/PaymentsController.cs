using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using DoAn_LapTrinhWeb.Model;

namespace DoAn_LapTrinhWeb.Areas.Areas.Controllers
{
    public class PaymentsController : BaseController
    {
        private readonly DbContext db = new DbContext();

        // GET: Areas/Payments
        public ActionResult Index()
        {
            ViewBag.countTrash = db.Payments.Where(a => a.status == "0").Count(); //  đếm tổng sp có trong thùng rác
            return View(db.Payments.Where(m => m.status == "1").OrderByDescending(m => m.create_at).ToList());
        }

        public ActionResult Trash()
        {
            return View(db.Payments.Where(m => m.status == "0").OrderByDescending(m => m.create_at).ToList());
        }

        // GET: Areas/Payments/Details/5
        public ActionResult Details(int? id)
        {
            var payment = db.Payments.SingleOrDefault(a => a.payment_id == id);
            if (payment == null || id == null)
            {
                Notification.set_flash("Không tồn tại! (ID = " + id + ")", "warning");
                return RedirectToAction("Index");
            }

            return View(payment);
        }

        // GET: Areas/Payments/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Areas/Payments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Payment payment)
        {
            if (ModelState.IsValid)
            {
                payment.create_at = DateTime.Now;
                payment.create_by = Session["UserName"].ToString();
                payment.update_at = DateTime.Now;
                payment.update_by = Session["UserName"].ToString();

                db.Payments.Add(payment);
                db.SaveChanges();

                Notification.set_flash("Thêm mới thành công!", "success");
                return RedirectToAction("Index");
            }

            return View(payment);
        }

        // GET: Areas/Payments/Edit/5
        public ActionResult Edit(int? id)
        {
            ViewBag.countTrash = db.Payments.Where(m => m.status == "0").Count();
            var payment = db.Payments.SingleOrDefault(a => a.payment_id == id);
            if (payment == null || id == null)
            {
                Notification.set_flash("Không tồn tại! (ID = " + id + ")", "warning");
                return RedirectToAction("Index");
            }

            return View(payment);
        }

        // POST: Areas/Payments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Payment payment)
        {
            if (ModelState.IsValid)
            {
                payment.update_at = DateTime.Now;
                payment.update_by = Session["UserName"].ToString();

                db.Entry(payment).State = EntityState.Modified;
                db.SaveChanges();

                Notification.set_flash("Đã cập nhật lại thông tin!", "success");
                return RedirectToAction("Index");
            }

            return View(payment);
        }

        public ActionResult DelTrash(int? id) //bỏ sp vào thùng rác
        {
            var payment = db.Payments.SingleOrDefault(a => a.payment_id == id);
            if (payment == null || id == null)
            {
                Notification.set_flash("Không tồn tại! (ID = " + id + ")", "warning");
                return RedirectToAction("Index");
            }

            payment.status = "0";

            payment.update_at = DateTime.Now;
            payment.update_by = Session["UserName"].ToString();

            db.Entry(payment).State = EntityState.Modified;
            db.SaveChanges();

            Notification.set_flash("Đã chuyển vào thùng rác! (ID = " + id + ")", "success");
            return RedirectToAction("Index");
        }

        public ActionResult Undo(int? id) // khôi phục từ thùng rác
        {
            var payment = db.Payments.SingleOrDefault(a => a.payment_id == id);
            if (payment == null || id == null)
            {
                Notification.set_flash("Không tồn tại! (ID = " + id + ")", "warning");
                return RedirectToAction("Index");
            }

            payment.status = "1";

            payment.update_at = DateTime.Now;
            payment.update_by = Session["UserName"].ToString();

            db.Entry(payment).State = EntityState.Modified;
            db.SaveChanges();

            Notification.set_flash("Khôi phục thành công! (ID = " + id + ")", "success");
            return RedirectToAction("Trash");
        }

        // GET: Areas/Payments/Delete/5
        public ActionResult Delete(int? id)
        {
            var payment = db.Payments.SingleOrDefault(a => a.payment_id == id);
            if (payment == null || id == null)
            {
                Notification.set_flash("Không tồn tại! (ID = " + id + ")", "warning");
                return RedirectToAction("Trash");
            }

            return View(payment);
        }

        // POST: Areas/Payments/Delete/5
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var payment = db.Payments.SingleOrDefault(a => a.payment_id == id);
            db.Payments.Remove(payment);
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