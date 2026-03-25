using ArchEcommerceSystem.Core.Interfaces;
using ArchEcommerceSystem.Core.ValueObjects;
using ArchEcommerceSystem.UseCases.Commands;
using ArchEcommerceSystem.Core.DomainServices; 

namespace ArchEcommerceSystem.UseCases.Handlers;

public class AddItemPedidoHandler
{
    private readonly IPedidoRepository _pedidoRepository;
    private readonly IProdutoRepository _produtoRepository;

    public AddItemPedidoHandler(
        IPedidoRepository pedidoRepository,
        IProdutoRepository produtoRepository)
    {
        _pedidoRepository = pedidoRepository;
        _produtoRepository = produtoRepository;
    }

    public async Task Handle(AddItemPedidoCommand command)
    {
        var pedido = await _pedidoRepository.GetByIdAsync(command.PedidoId)
            ?? throw new Exception("Pedido não encontrado");

        var produto = await _produtoRepository.GetByIdAsync(command.ProdutoId)
            ?? throw new Exception("Produto não encontrado");

        pedido.AdicionarItem(
            produto.Id,
            new Quantidade(command.Quantidade),
            produto.Preco,
            produto.Ativo
        );

        await _pedidoRepository.UpdateAsync(pedido);
    }
}