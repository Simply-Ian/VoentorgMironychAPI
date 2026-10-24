namespace Basket.API.Models;

public class ShoppingCartItem
{
    public Guid ItemId { get; set; }
    
    /* Не uint, т. к. если сделать беззнаковым целым и клиент передает отрицательное число, ему вместо ошибки валидации
    возвращается 500 еще на этапе десериализации */
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public string? ItemTitle { get; set; }
    public string? ItemNote {get; set;}
}