using ArchEcommerceSystem.Core.Aggregates;
using ArchEcommerceSystem.Core.Interfaces;
using ArchEcommerceSystem.UseCases.Commands;

namespace ArchEcommerceSystem.UseCases.Handlers;

public class CreatePedidoHandler
{
    private readonly IPedidoRepository _pedidoRepository;

    public CreatePedidoHandler(IPedidoRepository pedidoRepository)
    {
        _pedidoRepository = pedidoRepository;
    }

    public async Task<Guid> Handle(CreatePedidoCommand command)
    {
        var pedido = new Pedido(command.ClienteId);

        await _pedidoRepository.AddAsync(pedido);

        return pedido.Id;
    }
}