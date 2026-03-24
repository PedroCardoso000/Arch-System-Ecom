using ArchEcommerceSystem.Core.ValueObjects;

namespace ArchEcommerceSystem.Tests.ValueObjects;

public class EmailTests
{
    [Fact]
    public void Deve_Lancar_Excecao_Para_Email_Invalido()
    {
        Assert.Throws<ArgumentException>(() =>
            new Email("email-invalido")
        );
    }

    [Fact]
    public void Deve_Aceitar_Email_Valido()
    {
        var email = new Email("teste@email.com");

        Assert.Equal("teste@email.com", email.Value);
    }
}