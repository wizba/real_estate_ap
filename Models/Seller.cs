using System;

namespace RealEstateAPI.Models
{
    public class Seller:Person
    {
        private Seller()
        {
            Role = "Seller";
        }
        // Relationships
        public ICollection<PropertyListing> PropertyListings { get; set; }
    }
}
