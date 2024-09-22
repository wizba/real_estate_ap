namespace RealEstateAPI.Models
{
    public class Community
    {
        public long Id { get; set; }
        public string CommunityName { get; set; }
        public string Slug { get; set; }

        public ICollection<Property> Properties { get; set; }
    }
}
