using System;

namespace DoAn_LapTrinhWeb.DTOs
{
    public class OrderDTOs
    {
        public int order_id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Addres { get; set; }
        public string create_by { get; set; }
        public DateTime create_at { get; set; }
        public string update_by { get; set; }
        public DateTime update_at { get; set; }
        public string status { get; set; }
        public int payment_id { get; set; }
        public int delivery_id { get; set; }
        public double price { get; set; }
        public int account_id { get; set; }

    }
}