using System;

namespace RealEstateAPI.Models
{
    public class Seller:Person
    {

        // Relationships
        public ICollection<PropertyListing> PropertyListings { get; set; }
    }
}
