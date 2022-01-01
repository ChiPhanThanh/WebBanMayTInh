using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DoAn_LapTrinhWeb.Model
{
    public class Feedback_Image
    {
        [Key] public int image_id { get; set; }

        public int feedback_id { get; set; }

        public int account_id { get; set; }

        [Column(TypeName = "text")] public string image { get; set; }

        public DateTime create_at { get; set; }

        [Required] [StringLength(20)] public string create_by { get; set; }

        [StringLength(1)] public string status { get; set; }

        [Required] [StringLength(20)] public string update_by { get; set; }

        public DateTime? update_at { get; set; }

        public virtual Feedback Feedback { get; set; }
    }
}