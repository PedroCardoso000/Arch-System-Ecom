using ArchEcommerceSystem.Core.Interfaces;
using ArchEcommerceSystem.UseCases.Commands;

namespace ArchEcommerceSystem.UseCases.Handlers;

public class ConfirmarPedidoHandler
{
    private readonly IPedidoRepository _pedidoRepository;

    public ConfirmarPedidoHandler(IPedidoRepository pedidoRepository)
    {
        _pedidoRepository = pedidoRepository;
    }

    public async Task Handle(ConfirmarPedidoCommand command)
    {
        var pedido = await _pedidoRepository.GetByIdAsync(command.PedidoId)
            ?? throw new Exception("Pedido não encontrado");

        pedido.ConfirmarPedido();

        await _pedidoRepository.UpdateAsync(pedido);
    }
}