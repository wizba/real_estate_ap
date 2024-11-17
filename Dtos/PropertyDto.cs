namespace real_estate_api.NewFolder
{
    public class PropertyDto
    {
        public string Address { get; set; }
        public string PostalCode { get; set; }
        public string Province { get; set; }
        public long CityId { get; set; }
        public long CommunityId { get; set; }
        public int DisseminationArea { get; set; }
        public int Longitude { get; set; }
        public int Latitude { get; set; }
        public DateTime ConstructedDate { get; set; }
        public string Ownership { get; set; }
        public string Type { get; set; }
        public string SubType { get; set; }
    }
}
