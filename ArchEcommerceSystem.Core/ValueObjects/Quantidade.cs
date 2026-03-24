namespace ArchEcommerceSystem.Core.ValueObjects;

public class Quantidade
{
    public int Value { get; }

    private Quantidade() {}

    public Quantidade(int value)
    {
        if (value <= 0)
            throw new ArgumentException("Quantidade deve ser maior que zero");

        Value = value;
    }
}