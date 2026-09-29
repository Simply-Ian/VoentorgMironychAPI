namespace Basket.API.Models;

public class ShoppingCartItem
{
    public Guid ItemId { get; set; }
    public uint Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public string? ItemTitle { get; set; }
    public string? ItemNote {get; set;}
}