using ArchEcommerceSystem.Core.Interfaces;
using ArchEcommerceSystem.UseCases.DTOs;
using ArchEcommerceSystem.UseCases.Queries;

namespace ArchEcommerceSystem.UseCases.Handlers;

public class GetPedidoByIdHandler
{
    private readonly IPedidoRepository _repository;

    public GetPedidoByIdHandler(IPedidoRepository repository)
    {
        _repository = repository;
    }

    public async Task<PedidoDto?> Handle(GetPedidoByIdQuery query)
    {
        var pedido = await _repository.GetByIdAsync(query.Id);

        if (pedido == null) return null;

        return new PedidoDto
        {
            Id = pedido.Id,
            ClienteId = pedido.ClienteId,
            ValorTotal = pedido.ValorTotal.Value,
            Status = (int)pedido.Status
        };
    }
}