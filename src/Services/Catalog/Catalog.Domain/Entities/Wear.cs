using System.ComponentModel;
using Common;
namespace Catalog.Domain.Entities;

public enum Material
{
    [Description("натуральная кожа")]
    Leather,
    [Description("хлопок")]
    CottonFabric,
    [Description("синтетика")]
    SyntheticFabric,
    [Description("неизвестен")]
    Unknown
}

public class Wear : Item
{
    public Material Material { get; set; }
}