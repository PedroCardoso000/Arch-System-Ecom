using ArchEcommerceSystem.Core.DomainServices;
using ArchEcommerceSystem.Core.Entities;
using ArchEcommerceSystem.Core.ValueObjects;

namespace ArchEcommerceSystem.Tests.DomainServices;

public class CalculadoraPedidoTests
{
    [Fact]
    public void Deve_Calcular_Total_Dos_Itens()
    {
        var calculadora = new CalculadoraPedidoService();

        var itens = new List<ItemPedido>
        {
            new ItemPedido(Guid.NewGuid(), new Quantidade(2), new Money(10)),
            new ItemPedido(Guid.NewGuid(), new Quantidade(1), new Money(5))
        };

        var total = calculadora.CalcularTotal(itens);

        Assert.Equal(25, total.Value);
    }
}