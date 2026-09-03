namespace Catalog.Application.Contracts;

public class CreateItemContract
{
    public string? Title { get; set; }
    public string? ShortDescription { get; set; }
    public string? FullDescription { get; set; }
    public Uri? ImageUrl { get; set; }
    public Guid? BrandId { get; set; }
    public IEnumerable<Guid>? CategoryIds { get; set; }
    public decimal Price { get; set; }
    public bool IsAgeRestricted { get; set; }
}