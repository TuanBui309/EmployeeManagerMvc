using System.ComponentModel.DataAnnotations;
namespace Entity.Models
{
    public class Ward
    {
        [Key]
        public int Id { get; set; }
        public int DistrictId { get; set; }
        public string WardName { get; set; }
        public District District { get; set; }
    }

}
