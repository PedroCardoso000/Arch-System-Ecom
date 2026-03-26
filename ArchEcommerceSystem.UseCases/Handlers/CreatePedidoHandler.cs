using ArchEcommerceSystem.Core.Aggregates;
using ArchEcommerceSystem.Core.Interfaces;
using ArchEcommerceSystem.UseCases.Commands;

namespace ArchEcommerceSystem.UseCases.Handlers;

public class CreatePedidoHandler
{
    private readonly IPedidoRepository _pedidoRepository;
    private readonly IClienteRepository _clienteRepository;

    public CreatePedidoHandler(IPedidoRepository pedidoRepository, IClienteRepository clienteRepository)
    {
        _pedidoRepository = pedidoRepository;
        _clienteRepository = clienteRepository;
    }

    public async Task<Guid> Handle(CreatePedidoCommand command)
    {
        var cliente = await _clienteRepository.GetByIdAsync(command.ClienteId)
              ?? throw new Exception("Cliente não encontrado");

        
        var pedido = new Pedido(command.ClienteId);

        await _pedidoRepository.AddAsync(pedido);

        return pedido.Id;
    }
}