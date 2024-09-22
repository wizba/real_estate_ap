using System;

namespace RealEstateAPI.Models
{
    public class Image
    {
        public long Id { get; set; }
        public long PropertyListingId { get; set; }
        public string Name { get; set; }
        public string ImagePath { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public PropertyListing PropertyListing { get; set; }
    }
}
