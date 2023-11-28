using System.ComponentModel.DataAnnotations;

namespace Entity.Models
{
    public class Degree
    {
        [Key]
        public int Id { get; set; }

        public int? EmployeeId { get; set; }

        public string DegreeName { get; set; } = "";

        public DateTime DateRange { get; set; }

        public string IssuedBy { get; set; } = "";

        public DateTime DateOfExpiry { get; set; }

        public Employee? Employee { get; set; }  
    }


}
