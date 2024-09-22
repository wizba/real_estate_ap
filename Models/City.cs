namespace RealEstateAPI.Models
{
    public class City
    {
        public long Id { get; set; }
        public string CityName { get; set; }
        public string Slug { get; set; }

        public ICollection<Property> Properties { get; set; }
    }
}
