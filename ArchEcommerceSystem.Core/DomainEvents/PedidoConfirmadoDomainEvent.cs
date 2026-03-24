namespace ArchEcommerceSystem.Core.DomainEvents;

public class PedidoConfirmadoDomainEvent
{
    public Guid PedidoId { get; }
    public Guid ClienteId { get; }
    public decimal ValorTotal { get; }
    public DateTime DataConfirmacao { get; }

    public PedidoConfirmadoDomainEvent(Guid pedidoId, Guid clienteId, decimal valorTotal)
    {
        PedidoId = pedidoId;
        ClienteId = clienteId;
        ValorTotal = valorTotal;
        DataConfirmacao = DateTime.UtcNow;
    }
}