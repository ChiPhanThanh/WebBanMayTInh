using System.Linq;
using System.Web.Mvc;
using DoAn_LapTrinhWeb.DTOs;
using DoAn_LapTrinhWeb.Model;

namespace DoAn_LapTrinhWeb.Areas.Areas.Controllers
{
    public class DashboardController : BaseController
    {
        private readonly DbContext db = new DbContext();

        // GET: Areas/Dashboard
        
        public ActionResult Index()
        {
            // ViewBag.CountOrder = db.Oder_Detail.Where(m => m.status == "1").Count();
            ViewBag.CountOrderWaitting = (from a in db.Oder_Detail
                join b in db.Orders on a.order_id equals b.order_id
                group a by new {a.order_id,b} into g
                where g.Key.b.status == "1"
                orderby g.Key.b.create_at descending
                select new OrderDTOs
                {
                    order_id = g.Key.order_id,
                    price = g.Sum(m=>(int?)m.price) ?? 0,
                    status = g.Key.b.status,
                    create_at = g.Key.b.create_at
                }).Count();

            ViewBag.CountOrderProcessing = (from a in db.Oder_Detail
                join b in db.Orders on a.order_id equals b.order_id
                group a by new {a.order_id,b} into g
                where g.Key.b.status == "2"
                orderby g.Key.b.create_at descending
                select new OrderDTOs
                {
                    order_id = g.Key.order_id,
                    price = g.Sum(m=>(int?)m.price) ?? 0,
                    status = g.Key.b.status,
                    create_at = g.Key.b.create_at
                }).Count();

            ViewBag.CountOrderComplete = (from a in db.Oder_Detail
                join b in db.Orders on a.order_id equals b.order_id
                group a by new {a.order_id,b} into g
                where g.Key.b.status == "3"
                orderby g.Key.b.create_at descending
                select new OrderDTOs
                {
                    order_id = g.Key.order_id,
                    price = g.Sum(m=>(int?)m.price)??0,
                    status = g.Key.b.status,
                    create_at = g.Key.b.create_at
                }).Count();

            ViewBag.CountOrderCanceled = (from a in db.Oder_Detail
                join b in db.Orders on a.order_id equals b.order_id
                group a by new {a.order_id,b} into g
                where g.Key.b.status == "0"
                orderby g.Key.b.create_at descending
                select new OrderDTOs
                {
                    order_id = g.Key.order_id,
                    price = g.Sum(m=> (int?)m.price) ?? 0,
                    status = g.Key.b.status,
                    create_at = g.Key.b.create_at
                }).Count();
            ViewBag.CountContact = db.Contacts.Count(m => m.contact_id == 1);
            ViewBag.CountTurnover = db.Orders.Where(m=> m.status == "3").Sum(x=> (int?)x.total) ?? 0;
            ViewBag.CountProducts = db.Products.Count(m => m.status == "1");
            ViewBag.CountUsers = db.Accounts.Count(m => m.status == "1");

            return View();
        }
 
    }
}