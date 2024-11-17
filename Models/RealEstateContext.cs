using Microsoft.EntityFrameworkCore;

namespace RealEstateAPI.Models
{
    //Defines the DbContext used by Entity Framework Core, which manages the entities and their relationships.
    public class RealEstateContext : DbContext
    {
        public RealEstateContext(DbContextOptions<RealEstateContext> options) : base(options) { }

        public DbSet<Property> Properties { get; set; }
        public DbSet<PropertySalesHistory> PropertySalesHistories { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Community> Communities { get; set; }
        public DbSet<PropertyListing> PropertyListings { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<Amenity> Amenities { get; set; }
        public DbSet<ListingAmenity> ListingAmenities { get; set; }

        // Add DbSet for Person
        public DbSet<Person> People { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Seller> Sellers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure composite keys for PropertySalesHistory
            modelBuilder.Entity<PropertySalesHistory>()
                .HasKey(ps => new { ps.PropertyId, ps.Date });

            // Configure composite keys for ListingAmenity
            modelBuilder.Entity<ListingAmenity>()
                .HasKey(la => new { la.AmenityId, la.PropertyListingId });

            // Optionally: Configure inheritance or roles for Person
            modelBuilder.Entity<Person>()
                .HasDiscriminator<string>("Role")  // Use Role as the discriminator
                .HasValue<Client>("Client")
                .HasValue<Seller>("Seller");

            base.OnModelCreating(modelBuilder);
        }
    }
}
