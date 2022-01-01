using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace DoAn_LapTrinhWeb.Model
{
    [Table("Banner")]
    public class Banner
    {
        [SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Banner()
        {
            Banner_Detail = new HashSet<Banner_Detail>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int banner_id { get; set; }

        [Required] [StringLength(100)] public string banner_name { get; set; }

        public DateTime? banner_star { get; set; }

        public DateTime? banner_end { get; set; }

        [StringLength(100)] public string description { get; set; }

        [Column(TypeName = "text")] public string image { get; set; }

        [StringLength(1)] public string status { get; set; }

        [Required] [StringLength(20)] public string create_by { get; set; }

        public DateTime? create_at { get; set; }

        [Required] [StringLength(20)] public string update_by { get; set; }

        public DateTime? update_at { get; set; }

        public string slug { get; set; }

        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Banner_Detail> Banner_Detail { get; set; }
    }
}