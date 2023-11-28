namespace Entity.Services.ViewModels
{
    public class DistrictViewModel
    {
        public int Id { get; set; }

        public int CityId { get; set; }

        public string DistictName { get; set; } = "";
    }

    public class DistrictView
    {
        public int Id { get; set; }

        public string CityName { get; set; } = "";

        public string DistictName { get; set; } = "";
    }
}
