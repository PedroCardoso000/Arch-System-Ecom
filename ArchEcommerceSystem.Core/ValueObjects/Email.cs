namespace ArchEcommerceSystem.Core.ValueObjects;

public class Email
{
    public string Value { get; private set; } = null!;

    private Email() { }

    public Email(string value)
    {
        if (string.IsNullOrWhiteSpace(value) || !value.Contains("@"))
            throw new ArgumentException("Email inválido");

        Value = value;
    }

    public override string ToString() => Value;
}