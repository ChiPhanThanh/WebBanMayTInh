using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using DoAn_LapTrinhWeb.Models;

namespace DoAn_LapTrinhWeb.Areas.Areas.Controllers
{
    public class ContactsController : BaseController
    {
        private readonly DbContext db = new DbContext();

        // GET: Areas/Contacts
        public ActionResult Index()
        {
            ViewBag.countTrash = db.Contacts.Where(a => a.status == "0").Count(); //  đếm tổng sp có trong thùng rác
            return View(db.Contacts.Where(m => m.status == "1").OrderByDescending(m => m.create_at).ToList());
        }

        public ActionResult Trash()
        {
            return View(db.Contacts.Where(m => m.status == "0").OrderByDescending(m => m.create_at).ToList());
        }

        public ActionResult Reply(int? id)
        {
            var contact = db.Contacts.SingleOrDefault(a => a.contact_id == id);
            if (contact == null || id == null)
            {
                Notification.set_flash("Không tồn tại liên hệ từ khách hàng!", "danger");
                return RedirectToAction("Index");
            }

            return View(contact);
        }

        public ActionResult Reply(Contact contact)
        {
            if (ModelState.IsValid)
            {
                contact.update_at = DateTime.Now;
                contact.update_by = Session["UserName"].ToString();

                db.Entry(contact).State = EntityState.Modified;
                db.SaveChanges();
                Notification.set_flash("Đã trả lời liên hệ!", "success");
                return RedirectToAction("Index");
            }

            return View(contact);
        }

        public ActionResult DelTrash(int? id) //bỏ sp vào thùng rác
        {
            var contact = db.Contacts.SingleOrDefault(a => a.contact_id == id);
            if (contact == null || id == null)
            {
                Notification.set_flash("Không tồn tại! (ID = " + id + ")", "warning");
                return RedirectToAction("Index");
            }

            contact.status = "0";

            contact.update_at = DateTime.Now;
            contact.update_by = Session["UserName"].ToString();

            db.Entry(contact).State = EntityState.Modified;
            db.SaveChanges();

            Notification.set_flash("Đã chuyển vào thùng rác! (ID = " + id + ")", "success");
            return RedirectToAction("Index");
        }

        public ActionResult Undo(int? id) // khôi phục từ thùng rác
        {
            var contact = db.Contacts.SingleOrDefault(a => a.contact_id == id);
            if (contact == null || id == null)
            {
                Notification.set_flash("Không tồn tại! (ID = " + id + ")", "warning");
                return RedirectToAction("Index");
            }

            contact.status = "1";

            contact.update_at = DateTime.Now;
            contact.update_by = Session["UserName"].ToString();

            db.Entry(contact).State = EntityState.Modified;
            db.SaveChanges();

            Notification.set_flash("Khôi phục thành công! (ID = " + id + ")", "success");
            return RedirectToAction("Trash");
        }

        // GET: Areas/Contacts/Delete/5
        public ActionResult Delete(int? id)
        {
            var contact = db.Contacts.SingleOrDefault(a => a.contact_id == id);
            if (contact == null || id == null)
            {
                Notification.set_flash("Không tồn tại! (ID = " + id + ")", "warning");
                return RedirectToAction("Trash");
            }

            return View(contact);
        }

        // POST: Areas/Contacts/Delete/5
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var contact = db.Contacts.SingleOrDefault(a => a.contact_id == id);
            db.Contacts.Remove(contact);
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