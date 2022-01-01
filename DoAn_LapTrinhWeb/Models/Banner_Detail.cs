using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DoAn_LapTrinhWeb.Models;

namespace DoAn_LapTrinhWeb.Model
{
    public class Banner_Detail
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int banner_id { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int product_id { get; set; }

        [Key]
        [Column(Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int genre_id { get; set; }

        [Key]
        [Column(Order = 3)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int disscount_id { get; set; }

        [StringLength(1)] public string status { get; set; }

        [Required] [StringLength(20)] public string create_by { get; set; }

        public DateTime create_at { get; set; }

        [Required] [StringLength(20)] public string update_by { get; set; }

        public DateTime update_at { get; set; }


        public virtual Banner Banner { get; set; }

        public virtual Product Product { get; set; }
    }
}