using System;
using System.Collections.Generic;

namespace RealEstateAPI.Models
{
    public class Property
    {
        public long Id { get; set; }
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
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
         
        public City City { get; set; }
        public Community Community { get; set; }
        public ICollection<PropertySalesHistory> SalesHistory { get; set; }
    }
}
