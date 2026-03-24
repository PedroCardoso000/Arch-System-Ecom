using ArchEcommerceSystem.Core.Entities;
using ArchEcommerceSystem.Core.ValueObjects;
using ArchEcommerceSystem.Core.DomainEvents;

namespace ArchEcommerceSystem.Core.Aggregates;

public class Pedido : BaseEntity
{
    public Guid ClienteId { get; private set; }
    public PedidoStatus Status { get; private set; }
    public DateTime DataCriacao { get; private set; }
    public Money ValorTotal { get; private set; } = null!;

    private readonly List<ItemPedido> _itens = new();
    public IReadOnlyCollection<ItemPedido> Itens => _itens;

    private Pedido () { } 

    public Pedido(Guid clienteId)
    {
        ClienteId = clienteId;
        Status = PedidoStatus.Pendente;
        DataCriacao = DateTime.UtcNow;
        ValorTotal = new Money(0);
    }

    public void AdicionarItem(Guid produtoId, Quantidade quantidade, Money precoUnitario, bool produtoAtivo)
    {
        if (!produtoAtivo)
            throw new InvalidOperationException("Produto inativo");

        var item = new ItemPedido(produtoId, quantidade, precoUnitario);
        _itens.Add(item);

        RecalcularTotal();
    }

    public void RemoverItem(Guid produtoId)
    {
        var item = _itens.FirstOrDefault(i => i.ProdutoId == produtoId);

        if (item != null)
        {
            _itens.Remove(item);
            RecalcularTotal();
        }
    }

    private void RecalcularTotal()
    {
        var total = _itens.Sum(i => i.Subtotal.Value);
        ValorTotal = new Money(total);
    }

    public void ConfirmarPedido()
    {
        if (!_itens.Any())
            throw new InvalidOperationException("Pedido não possui itens");

        Status = PedidoStatus.Confirmado;

        AddDomainEvent(new PedidoConfirmadoDomainEvent(
            Id,
            ClienteId,
            ValorTotal.Value
        ));
    }
}