namespace ArchEcommerceSystem.Core.Interfaces;

using ArchEcommerceSystem.Core.Aggregates;


public interface IPedidoRepository
{
    Task<Pedido?> GetByIdAsync(Guid id);
    Task AddAsync(Pedido pedido);
    Task UpdateAsync(Pedido pedido);
}