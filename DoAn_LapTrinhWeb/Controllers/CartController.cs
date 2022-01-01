using DoAn_LapTrinhWeb.Common.Helpers;
using DoAn_LapTrinhWeb.Model;
using DoAn_LapTrinhWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace DoAn_LapTrinhWeb.Controllers
{
    public class CartController : Controller
    {
        // GET: Cart
        private DbContext db = new DbContext();
        public ActionResult Cart()
        {
            return View();
        }
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Addtocart(int id)
        {
            //lay thong tin xem gio hang da ton tai chua
            //List<giohang> gh = ThongTinGioHang();
            List<Cart> gh = Session["Cart"] as List<Cart>;
            if (gh == null)
            {
                gh = new List<Cart>();
                Session["Cart"] = gh;
            }
            // kiem tra san pham da co trong gio hnag chua
            Cart item = gh.Find(n => n.iMasp == id);
            if (item == null)
            {
                //san pham chua co trong gio hang
                Cart newitem = new Cart(id);
                gh.Add(newitem);
            }
            else
            {
                item.iSoLuong += 1;
            }
            return RedirectToAction("ViewCart");

        }
        public ActionResult ViewCart()
        {
            var cart = this.GetCart();
            ViewBag.Quans = cart.Item2;
            var listProduct = cart.Item1.OrderByDescending(x => x.product_id).ToList();
            return View(listProduct);
        }
        public ActionResult RemoveProduct(int id)
        {
            List<Cart> lst = Session["Cart"] as List<Cart>;
            Cart item = lst.Find(n => n.iMasp == id);
            lst.Remove(item);
            return RedirectToAction("ViewCart");
        }
        [HttpPost]
        public ActionResult UpdateCart(int id, FormCollection frm)
        {
            List<Cart> lst = Session["Cart"] as List<Cart>;
            Cart item = lst.Find(n => n.iMasp == id);
            int soluong = int.Parse(frm["txtSoluong"].ToString());
            item.iSoLuong = soluong;//cap nhat so luong moi
            return RedirectToAction("ViewCart");
        }

        [Authorize] // => Phải đăng nhập mới được phép vào action bên dưới
                    // => Có thể đặt trước Controller để áp dụng cho toàn bộ action trong controller đó
        public ActionResult Checkout()
        {
            int userId = User.Identity.GetUserId();
            var user = db.Accounts.SingleOrDefault(u => u.account_id == userId);
            var cart = this.GetCart();

            if (cart.Item2.Count < 1)
            {
                return RedirectToAction(nameof(ViewCart));
            }

            var products = cart.Item1;
            double total = 0d;
            double discount = 0d;
            double productPrice = 0d;
            for (int i = 0; i < products.Count; i++)
            {
                var item = products[i];
                productPrice = item.price;
                if (item.Discount != null)
                {
                    if (item.Discount.discount_star < DateTime.Now && item.Discount.discount_end > DateTime.Now)
                    {
                        productPrice = item.price - item.Discount.discount_price;
                    }
                }
                total += productPrice * cart.Item2[i];
            }

            // Áp dụng mã giảm giá
            if (Session["Discount"] != null)
            {
                discount = Convert.ToDouble(Session["Discount"].ToString());
                total -= discount;
            }
            ViewBag.Total = total;
            ViewBag.Discount = discount;
            return View(user);
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult> SaveOrder()
        {
            try
            {
                var cart = this.GetCart();
                var listProduct = new List<Product>();
                var order = new Order()
                {
                    account_id = User.Identity.GetUserId(),
                    create_at = DateTime.Now,
                    create_by = User.Identity.GetUserId().ToString(),
                    status = "1",
                    order_note = Request.Form["OrderNote"].ToString(),
                    delivery_id = 1,
                    oder_date = DateTime.Now.AddDays(2),
                    update_at = DateTime.Now,
                    payment_id = 1,
                    update_by = User.Identity.GetUserId().ToString(),
                    total = Convert.ToDouble(TempData["Total"])
                };
                for (int i = 0; i < cart.Item1.Count; i++)
                {
                    var item = cart.Item1[i];
                    var _price = item.price;
                    if (item.Discount != null)
                    {
                        if (item.Discount.discount_star < DateTime.Now && item.Discount.discount_end > DateTime.Now)
                        {
                            _price = item.price - item.Discount.discount_price;
                        }
                    }
                    order.Oder_Detail.Add(new Oder_Detail
                    {
                        create_at = DateTime.Now,
                        create_by = User.Identity.GetUserId().ToString(),
                        disscount_id = item.disscount_id,
                        genre_id = item.genre_id,
                        price = _price,
                        product_id = item.product_id,
                        quantity = cart.Item2[i],
                        status = "1",
                        update_at = DateTime.Now,
                        update_by = User.Identity.GetUserId().ToString(),
                        transection = "transection"
                    });
                    // Xóa cart
                    Response.Cookies["product_" + item.product_id].Expires = DateTime.Now.AddDays(-10);

                    // Thay đổi số lượng và số lượt mua của product 
                    var product = db.Products.SingleOrDefault(p => p.product_id == item.product_id);
                    product.buyturn += cart.Item2[i];
                    product.quantity = (Convert.ToInt32(product.quantity ?? "0") - cart.Item2[i]).ToString();
                    listProduct.Add(product);
                }
                db.Orders.Add(order);
                await db.SaveChangesAsync();
                TempData["AddOrderSuccess"] = true;
            }
            catch
            {
                TempData["AddOrderSuccess"] = false;
            }
            //return RedirectToAction("Index", "Home");

            return RedirectToAction("TrackingOrder", "Account");
        }

        public ActionResult UseDiscountCode(string code)
        {
            var discount = db.Discounts.SingleOrDefault(d => d.discount_code == code);
            if (discount != null)
            {
                if (discount.discount_star < DateTime.Now && discount.discount_end > DateTime.Now)
                {
                    Session["Discount"] = discount.discount_price;
                    return Json(new { success = true, discountPrice = discount.discount_price }, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(new { success = false, discountPrice = 0 }, JsonRequestBehavior.AllowGet);
        }

        private Tuple<List<Product>, List<int>> GetCart()
        {
            var cart = Request.Cookies.AllKeys.Where(c => c.IndexOf("product_") == 0);
            var productIds = new List<int>();
            var quantities = new List<int>();

            var errorProduct = new List<string>();
            var cValue = "";
            // Lấy mã sản phẩm & số lượng trong giỏ hàng
            foreach (var item in cart)
            {
                var tempArr = item.Split('_');
                if (tempArr.Length != 2)
                {
                    // Nếu không lấy được thì xem như sản phẩm đó bị lỗi
                    errorProduct.Add(item);
                    continue;
                }
                cValue = Request.Cookies[item].Value;
                productIds.Add(Convert.ToInt32(tempArr[1]));
                quantities.Add(Convert.ToInt32(String.IsNullOrEmpty(cValue) ? "0" : cValue));
            }

            // Select sản phẩm để hiển thị
            var listProduct = new List<Product>();
            foreach (var id in productIds)
            {
                var product = db.Products
                        .SingleOrDefault(p => p.status == "1" && p.product_id == id);
                if (product != null)
                {
                    listProduct.Add(product);
                }
                else
                {
                    // Trường hợp ko chọn được sản phẩm như trong giỏ hàng
                    // Đánh dấu sản phẩm trong giỏ hàng là sản phẩm lỗi
                    errorProduct.Add("product_" + id);
                    quantities.RemoveAt(productIds.IndexOf(id));
                }
            }
            // Xóa sản phẩm bị lỗi khỏi cart
            foreach (var err in errorProduct)
            {
                Response.Cookies[err].Expires = DateTime.Now.AddDays(-1);
            }
            return new Tuple<List<Product>, List<int>>(listProduct, quantities);
        }

    }
}