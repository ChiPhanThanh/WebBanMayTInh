using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using DoAn_LapTrinhWeb.DTOs;

namespace DoAn_LapTrinhWeb.Areas.Areas.Controllers
{
    public class AdminsController : BaseController
    {
        private readonly DbContext db = new DbContext();

        // GET: Areas/Admins
        public ActionResult Index()
        {
            ViewBag.countTrash =
                db.Accounts.Where(a => a.status == "0" && a.Role == "0").Count(); // đếm user status 0 và role 1
            var user = from a in db.Accounts              
                where a.status == "1" && a.Role == "0"
                orderby a.create_at descending // giảm dần
                select new UserDTOs
                {
                    account_id = a.account_id,
                    username = a.username,
                    Email = a.Email,
                    Role = a.Role,
                    Name = a.Name,
                    Phone = a.Phone,
                    Avatar = a.Avatar,
                };
            return View(user.ToList());
        }

        public ActionResult Trash()
        {
            var user = from a in db.Accounts
               
                where a.status == "0" && a.Role == "0"
                orderby a.create_at descending // giảm dần
                select new UserDTOs
                {
                    account_id = a.account_id,
                    username = a.username,
                    Email = a.Email,
                    Role = a.Role,
                    Name = a.Name,
                    Phone = a.Phone,
                    Avatar = a.Avatar,
                };
            return View(user.ToList());
        }

        // GET: Areas/Admins/Details/5
        public ActionResult Details(int? id)
        {
            var user = (from a in db.Accounts
                
                where a.status == "1" || a.Role == "0"
                orderby a.create_at descending // giảm dần
                select new UserDTOs
                {
                    account_id = a.account_id,
                    username = a.username,
                    Email = a.Email,
                    Role = a.Role,
                    Name = a.Name,
                    Phone = a.Phone,
                    Avatar = a.Avatar,
                    Addres_1 = a.Addres_1,
                    Addres_2 = a.Addres_2,
                    Addres_3 = a.Addres_3,
                    status = a.status,
                    create_at = a.create_at,
                    create_by = a.create_by,
                    update_at = a.update_at,
                    update_by = a.update_by
                }).FirstOrDefault();
            if (user == null || id == null)
            {
                Notification.set_flash("Không tồn tại! (ID = " + id + ")", "warning");
                return RedirectToAction("Index");
            }

            return View(user);
        }

        public ActionResult DelTrash(int? id) // chuyển status về 0
        {
            var user = db.Accounts.SingleOrDefault(a => a.account_id == id && a.Role == "0");
            var users = (from a in db.Accounts              
                where id == a.account_id
                select a).SingleOrDefault();

            if (user == null || id == null || users == null)
            {
                Notification.set_flash("Không tồn tại! (ID = " + id + ")", "warning");
                return RedirectToAction("Index");
            }


            user.status = "0";
            user.update_at = DateTime.Now;
            user.update_by = Session["UserName"].ToString();
            db.Entry(user).State = EntityState.Modified;

            users.status = "0";
            users.update_at = DateTime.Now;
            users.update_by = Session["UserName"].ToString();
            db.Entry(users).State = EntityState.Modified;
            db.SaveChanges();

            Notification.set_flash("Đã chuyển vào thùng rác! (ID = " + id + ")", "success");
            return RedirectToAction("Index");
        }

        public ActionResult Undo(int? id) // khôi phục từ thùng rác
        {
            var user = db.Accounts.SingleOrDefault(a => a.account_id == id && a.Role == "0");
            var users = (from a in db.Accounts
                
                where id == a.account_id
                select a).SingleOrDefault();

            if (user == null || id == null || users == null)
            {
                Notification.set_flash("Không tồn tại! (ID = " + id + ")", "warning");
                return RedirectToAction("Index");
            }

            user.status = "1";
            user.update_at = DateTime.Now;
            user.update_by = User.Identity.Name;
            db.Entry(user).State = EntityState.Modified;

            users.status = "1";
            users.update_at = DateTime.Now;
            users.update_by = User.Identity.Name;
            db.Entry(users).State = EntityState.Modified;
            db.SaveChanges();

            Notification.set_flash("Khôi phục thành công! (ID = " + id + ")", "success");
            return RedirectToAction("Trash");
        }

        public ActionResult ChangeRole(int? id) //Phân quyền thành User
        {
            var user = db.Accounts.SingleOrDefault(a => a.account_id == id && a.Role == "1");
            if (user == null || id == null)
            {
                Notification.set_flash("Không tồn tại! (ID = " + id + ")", "warning");
                return RedirectToAction("Index");
            }

            user.Role = "1";

            user.update_at = DateTime.Now;
            user.update_by = User.Identity.Name;
            db.Entry(user).State = EntityState.Modified;
            db.SaveChanges();

            Notification.set_flash("Đã phân quyền thành User! (ID = " + id + ")", "success");
            return RedirectToAction("Index");
        }


        // GET: Areas/Admins/Delete/5
        public ActionResult Delete(int? id)
        {
            var user = from a in db.Accounts            
                where a.status == "1" || a.Role == "0"
                orderby a.create_at descending // giảm dần
                select new UserDTOs
                {
                    account_id = a.account_id,
                    username = a.username,
                    Email = a.Email,
                    Role = a.Role,
                    Name = a.Name,
                    Phone = a.Phone,
                    Avatar = a.Avatar,
                };
            if (user == null || id == null)
            {
                Notification.set_flash("Không tồn tại! (ID = " + id + ")", "warning");
                return RedirectToAction("Trash");
            }

            return View(user.SingleOrDefault());

        }

        // POST: Areas/Admins/Delete/5
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {        
            var user = db.Accounts.SingleOrDefault(a => a.account_id == id && a.Role == "1");         
            db.Accounts.Remove(user);
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