using System;

namespace RealEstateAPI.Models
{
    public class PropertyListing
    {
        public long Id { get; set; }
        public long PropertyId { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
        public int Tax { get; set; }
        public int MaintenanceFee { get; set; }
        public int Bathrooms { get; set; }
        public int Bedrooms { get; set; }
        public int Fireplaces { get; set; }
        public string Parking { get; set; }
        public string Basement { get; set; }
        public int Size { get; set; }
        public int LandSize { get; set; }
        public int LandFrontage { get; set; }
        public bool Active { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public Property Property { get; set; }

        // Foreign Key
        public long PersonId { get; set; }

        // Navigation property
        public Person Person { get; set; }
    }
}
