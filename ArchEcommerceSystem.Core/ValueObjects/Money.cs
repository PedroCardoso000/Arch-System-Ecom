namespace ArchEcommerceSystem.Core.ValueObjects;

public class Money
{
    public decimal Value { get; }

    private Money() { }

    public Money(decimal value)
    {
        if (value < 0)
            throw new ArgumentException("Valor não pode ser negativo");

        Value = value;
    }

    public static Money operator +(Money a, Money b)
        => new Money(a.Value + b.Value);

    public static Money operator *(Money a, int multiplier)
        => new Money(a.Value * multiplier);
}