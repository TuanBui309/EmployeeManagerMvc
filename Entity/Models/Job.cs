using System.ComponentModel.DataAnnotations;
namespace Entity.Models
{
    public class Job
    {
        [Key]
        public int Id { get; set; }

        public string JobName { get; set; } = "";

    }

}
