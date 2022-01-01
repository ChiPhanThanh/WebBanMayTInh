using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace DoAn_LapTrinhWeb.Controllers
{
    public class DiscountController : Controller
    {
        // GET: Discount
        private DbContext db = new DbContext();
        public ActionResult Listbanner()
        {
           
            return View();
        }
        public ActionResult Bannerdetail()
        {
            return View();
        }
    }
}