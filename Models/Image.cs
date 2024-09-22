using Constants.SourceType;

public class Image
{
    public long Id { get; set; }
    public string Name { get; set; }
    public string ImagePath { get; set; }
    public SourceType SourceType { get; set; }  // Enum for source type (Person, Property, etc.)
    public long SourceId { get; set; }          // Foreign key to link the image with its source
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
