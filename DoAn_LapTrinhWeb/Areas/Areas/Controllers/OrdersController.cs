using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using DoAn_LapTrinhWeb.Common.Helpers;
using DoAn_LapTrinhWeb.DTOs;
using DoAn_LapTrinhWeb.Model;

namespace DoAn_LapTrinhWeb.Areas.Areas.Controllers
{
    public class OrdersController : BaseController
    {
        private readonly DbContext db = new DbContext();

        // GET: Areas/Orders
        public ActionResult Index()
        {
            ViewBag.countTrash = db.Orders.Count(a => a.status == "0"); //  đếm tổng sp có trong thùng rác
            var results = from a in db.Oder_Detail
                join b in db.Orders on a.order_id equals b.order_id
                group a by new {a.order_id,b} into g
                where g.Key.b.status != "0"
                orderby g.Key.b.create_at descending
                select new OrderDTOs
                {
                    order_id = g.Key.order_id,
                    price = g.Sum(m=>m.price*m.quantity+30000),
                    status = g.Key.b.status,
                    create_at = g.Key.b.create_at,
                    Name = g.Key.b.Account.Name,
                    Phone = g.Key.b.Account.Phone

                };
            return View(results.ToList());
        }

        // GET: Areas/Orders/Details/5
        public ActionResult Trash(int? id)
        {
            var results = from a in db.Oder_Detail
                join b in db.Orders on a.order_id equals b.order_id
                group a by new {a.order_id,b} into g
                where g.Key.b.status == "0"
                orderby g.Key.b.create_at descending
                select new OrderDTOs
                {
                    order_id = g.Key.order_id,
                    price = g.Sum(m=>m.price),
                    status = g.Key.b.status,
                    create_at = g.Key.b.create_at,
                    Name = g.Key.b.Account.Name,
                    Phone = g.Key.b.Account.Phone
                };
            return View(results.ToList());
        }

        public ActionResult Details(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            // var order = db.Orders.Find(id);
            var order = (from a in db.Oder_Detail
                join b in db.Orders on a.order_id equals id
                group a by new {a.order_id,b} into g
                // where g.Key.b.status != "0"
                select new OrderDTOs
                {
                    order_id = g.Key.order_id,
                    price = g.Sum(m=>m.price),
                    status = g.Key.b.status,
                    create_at = g.Key.b.create_at,
                    create_by = g.Key.b.create_by,
                    update_at = g.Key.b.update_at,
                    update_by = g.Key.b.update_by,
                    Name = g.Key.b.Account.Name,
                    Phone = g.Key.b.Account.Phone,
                    Addres = g.Key.b.Account.Addres_1


                }).FirstOrDefault();
            if (order == null || id == null)
            {
                Notification.set_flash("Không tồn tại! (ID = " + id + ")", "warning");
                return RedirectToAction("Index");
            }
            ViewBag.orderDetails = db.Oder_Detail.Where(m => m.order_id == id).ToList();
            ViewBag.orderProduct = db.Products.ToList();
            return View(order);
        }

        public ActionResult DelTrash(int? id)
        {
            var order = db.Orders.SingleOrDefault(pro => pro.order_id == id);
            if (order != null)
            {
                order.status = "0";
                order.update_at = DateTime.Now;
                order.update_by = User.Identity.GetUsername();
                db.Entry(order).State = EntityState.Modified;
            }
            db.SaveChanges();
            Notification.set_flash("Đã hủy đơn hàng!" + " ID = " + id, "success");
            return RedirectToAction("Index");
        }

        public ActionResult Undo(int? id)
        {
            var order = db.Orders.SingleOrDefault(pro => pro.order_id == id);
            if (order != null)
            {
                order.status = "1";
                order.update_at = DateTime.Now;
                order.update_by = User.Identity.GetUsername();
                db.Entry(order).State = EntityState.Modified;
            }
            db.SaveChanges();
            Notification.set_flash("Khôi phục thành công!" + " ID = " + id, "success");
            return RedirectToAction("Trash");
        }
        public ActionResult ChangeWaitting(int? id)
        {
            var order = db.Orders.SingleOrDefault(pro => pro.order_id == id);
            if (order != null)
            {
                order.status = "1";
                order.update_at = DateTime.Now;
                order.update_by = User.Identity.GetUsername();
                db.Entry(order).State = EntityState.Modified;
            }
            db.SaveChanges();
            Notification.set_flash("Chuyển trạng thái sang Chờ xử lý!" + " ID = " + id, "success");
            return RedirectToAction("Index");
        }
        public ActionResult ChangeProcessing(int? id)
        {
            var order = db.Orders.SingleOrDefault(pro => pro.order_id == id);
            if (order != null)
            {
                order.status = "2";
                order.update_at = DateTime.Now;
                order.update_by = User.Identity.GetUsername();
                db.Entry(order).State = EntityState.Modified;
            }
            db.SaveChanges();
            Notification.set_flash("Chuyển trạng thái sang Đang xử lý!" + " ID = " + id, "success");
            return RedirectToAction("Index");
        }

        public ActionResult ChangeComplete(int? id)
        {
            var order = db.Orders.SingleOrDefault(pro => pro.order_id == id);
            if (order != null)
            {
                order.status = "3";
                order.update_at = DateTime.Now;
                order.update_by = User.Identity.GetUsername();
                db.Entry(order).State = EntityState.Modified;
            }
            db.SaveChanges();
            Notification.set_flash("Chuyển trạng thái sang Hoàn thành!" + " ID = " + id, "success");
            return RedirectToAction("Index");
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing) db.Dispose();
            base.Dispose(disposing);
        }
    }
}