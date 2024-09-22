using System;
using System.Collections.Generic;

namespace RealEstateAPI.Models
{
    public class Person
    {
        public long Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // Use this Role field to differentiate between Client and Seller
        public string Role { get; set; }

        // If you have additional relationships, they go here
        public ICollection<PropertyListing> PropertyListings { get; set; }
    }
}