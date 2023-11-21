using System.ComponentModel.DataAnnotations;

namespace Entity.Services.ViewModels
{

    public class EmployeeViewModel
    {
        public int? Id { get; set; }
        public string Name { get; set; } = "";
        public string DateOfBirth { get; set; } = "";
        public int? Age { get; set; }
        public int? JobId { get; set; }
        public int? NationId { get; set; }
        public string? IdentityCardNumber { get; set; } 
        public string? PhoneNumber { get; set; } 
        public int CityId { get; set; }
        public int DistrictId { get; set; }
        public int WardId { get; set; }
    }
    public class EmployeeViewExport
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string DateOfBirth { get; set; } = "";
        public int? Age { get; set; }
        public string JobName { get; set; } = "";
        public string NationName { get; set; } = "";
        public string? IdentityCardNumber { get; set; } = "";
        public string? PhoneNumber { get; set; } = "";
        public string CityName { get; set; } = "";
        public string DistrictName { get; set; } = "";
        public string WardName { get; set; } = "";
    }
}
