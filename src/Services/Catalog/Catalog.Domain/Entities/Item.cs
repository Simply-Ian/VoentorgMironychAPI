namespace Catalog.Domain.Entities;

public class Item : BaseEntity
{
    public string? Title { get; set; }
    public string? ShortDescription { get; set; }
    public string? FullDescription { get; set; }

    public Uri? ImageUrl
    {
        get;
        set
        {
            if (value is null || value.IsWellFormedOriginalString()) field = value;
        }
    }
    public Brand? Brand { get; set; }
    public Category[] Categories { get; set; } = [];

    public decimal Price
    {
        get;
        set
        {
            if (value > 0) field = value;
        }
    }

    public bool IsAgeRestricted { get; set; } = false;
}