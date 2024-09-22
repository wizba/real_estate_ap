namespace RealEstateAPI.Models
{
    public class ListingAmenity
    {
        public long AmenityId { get; set; }
        public long PropertyListingId { get; set; }

        public Amenity Amenity { get; set; }
        public PropertyListing PropertyListing { get; set; }
    }
}
