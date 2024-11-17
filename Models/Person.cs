using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RealEstateAPI.Models
{
    public class Person
    {
        public long Id { get; set; }

        [Required(ErrorMessage = "First name is required")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "last name is required")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }

        [EmailAddress]
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // Use this Role field to differentiate between Client and Seller
        [Required(ErrorMessage = "Role is required")]
        public string Role { get; set; }

        // If you have additional relationships, they go here
        public ICollection<PropertyListing> PropertyListings { get; set; }
    }
}