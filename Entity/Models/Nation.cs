using System.ComponentModel.DataAnnotations;
namespace Entity.Models
{
    public class Nation
    {
        [Key]
        public int Id { get; set; }
        public string NationName { get; set; }

    }

}
