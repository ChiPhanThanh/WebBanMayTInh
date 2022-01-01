using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace DoAn_LapTrinhWeb.Model
{
    public class Cart
    {
        private readonly DbContext db = new DbContext();

        // tao thuoc tinh cho moi item trong gio hang
        public Cart(int product_id)
        {
            iMasp = product_id;
            var sp = db.Products.Find(product_id);
            strTenSp = sp.product_name;
//            dGia = sp.price;
            strHinh = sp.image;
            iSoLuong = 1;
        }

        public int Id { get; set; }
        public int iMasp { get; set; }
        public string strTenSp { get; set; }

        public string strHinh { get; set; }


        public double dGia { get; set; }


        public int iSoLuong { get; set; }
        public double dThanhtien => dGia * iSoLuong;

    }
}