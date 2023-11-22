using System.ComponentModel.DataAnnotations;
namespace Entity.Models
{
	public class City
	{
		[Key]
        public int Id { get; set; }
		public string CityName { get; set; } = "";
		public ICollection<District>? Districts { get; set; }
	}
	

}
