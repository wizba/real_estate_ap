using System;
using System;
using Constants.SourceType;

namespace RealEstateAPI.Models
{
    public class Image
    {
        public long Id { get; set; }

        // SourceType enum will define the type of the entity (Person, Property, etc.)
        public SourceType SourceType { get; set; }

        // SourceId will store the ID of the associated source (Person, Property, etc.)
        public long SourceId { get; set; }

        public required string Name { get; set; }
        public required string ImagePath { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

}
