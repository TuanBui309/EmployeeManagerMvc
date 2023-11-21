using Entity.Models;

namespace Entity.Services.ViewModels
{
    public class DegreeViewModel
    {
        public int? Id { get; set; }
        public int? EmployeeId { get; set; }
        public string DegreeName { get; set; } = "";
        public string DateRange { get; set; } = "";
        public string IssuedBy { get; set; } = "";
        public string DateOfExpiry { get; set; } = "";

    }
    public class DegreeView
    {
        public int Id { get; set; }
        public string EmployeeName { get; set; } = "";
        public string DegreeName { get; set; } = "";
        public string DateRange { get; set; } = "";
        public string IssuedBy { get; set; } = "";
        public string DateOfExpiry { get; set; } = "";
    }
}
