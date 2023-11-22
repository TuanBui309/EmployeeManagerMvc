namespace Entity.Models
{
    public class Employee
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public DateTime DateOfBirth { get; set; }= DateTime.Now;
        public int? Age { get; set; } 
        public int? JobId { get; set; }
        public int? NationId { get; set; }
        public string? IdentityCardNumber { get; set; } = "";
        public string? PhoneNumber { get; set; } = "";
        public int CityId { get; set; }
        public int DistrictId { get; set; }
        public int WardId { get; set; }
        public virtual District? District { get; set; }
        public virtual Job? Job { get; set; }
        public virtual Nation? Nation { get; set; }
        public virtual Ward? Ward { get; set; }
        public virtual City? City { get; set; }
    }

}
