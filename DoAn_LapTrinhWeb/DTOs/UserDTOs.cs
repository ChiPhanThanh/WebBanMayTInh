using System;

namespace DoAn_LapTrinhWeb.DTOs
{
    public class UserDTOs
    {
        public int account_id { get; set; }
        public string username { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Avatar { get; set; }
        public string Addres_1 { get; set; }
        public string Addres_2 { get; set; }
        public string Addres_3 { get; set; }
        public string create_by { get; set; }
        public DateTime create_at { get; set; }
        public string update_by { get; set; }
        public DateTime update_at { get; set; }
        public string status { get; set; }
    }
}