namespace Promotion.Grpc.Services;

public partial class DecimalValue
{
    private const decimal NanoFactor = 1_000_000_000;
    public DecimalValue(long units, int nanos)
    {
        Units = units;
        Nanos = nanos;
    }
    public static implicit operator decimal(DecimalValue grpcDecimal) => grpcDecimal.Units + grpcDecimal.Nanos / NanoFactor;
    
    public DecimalValue(decimal value)
    {
        Units = decimal.ToInt64(value);
        Nanos = decimal.ToInt32((value - Units) * NanoFactor);
    }
}