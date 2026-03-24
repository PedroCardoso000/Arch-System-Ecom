using ArchEcommerceSystem.Core.Entities;
using ArchEcommerceSystem.Core.ValueObjects;

namespace ArchEcommerceSystem.Core.DomainServices;

public class CalculadoraPedidoService
{
    public Money CalcularTotal(IEnumerable<ItemPedido> itens)
    {
        var total = itens.Sum(i => i.Subtotal.Value);
        return new Money(total);
    }
}