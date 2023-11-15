namespace Entity.Services.ViewModels
{
    public class CityViewModel
    {
        public int id { get; set; }
        public string CityName { get; set; }
    }
    public class InsertAddress
    {
        public string CityName { get; set; }
        public string DistrictName { get; set; }
        public string WardName { get; set; }
    }

}
