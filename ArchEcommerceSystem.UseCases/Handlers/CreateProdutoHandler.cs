using ArchEcommerceSystem.Core.Interfaces;
using ArchEcommerceSystem.Core.Aggregates;
using ArchEcommerceSystem.Core.ValueObjects;
using ArchEcommerceSystem.UseCases.Commands;

namespace ArchEcommerceSystem.UseCases.Handlers;

public class CreateProdutoHandler
{
    private readonly IProdutoRepository _repository;

    public CreateProdutoHandler(IProdutoRepository repository)
    {
        _repository = repository;
    }

    public async Task<Guid> Handle(CreateProdutoCommand command)
    {
        var produto = new Produto(command.Nome, new Money(command.Preco));

        await _repository.AddAsync(produto);

        return produto.Id;
    }
}