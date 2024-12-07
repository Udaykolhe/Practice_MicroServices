using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Practice.Service.EmployeeAPI.Model
{
    [Table("tbl_mst_employee")]
    public class Employee
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string? firstName { get; set; }
        [Required]
        public string? lastName { get; set; }
        [Required]
        public string? email { get; set; }
        [Required]
        [MaxLength(10)]
        public string? phone { get; set; }
        public string? city { get; set; }
        public string? state { get; set; }
        public string? zipcode { get; set; }
        public string? country { get; set; }
    }
}
