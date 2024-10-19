using System;

namespace RealEstateAPI.Models
{
    public class Client:Person
    {

        // Relationships
        public ICollection<PropertyListing> PropertyListings { get; set; }
    }
}
