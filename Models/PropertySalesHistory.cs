using System;

namespace RealEstateAPI.Models
{
    public class PropertySalesHistory
    {
        public long PropertyId { get; set; }
        public DateTime Date { get; set; }
        public int Price { get; set; }

        public Property Property { get; set; }
    }
}
