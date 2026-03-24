using ArchEcommerceSystem.Core.Interfaces;
using ArchEcommerceSystem.UseCases.DTOs;

namespace ArchEcommerceSystem.UseCases.Handlers;

public class GetProdutoHandler
{
    private readonly IProdutoRepository _repository;

    public GetProdutoHandler(IProdutoRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<ProdutoDto>> Handle()
    {
        var produtos = await _repository.GetAllAsync();

        return produtos.Select(p => new ProdutoDto
        {
            Id = p.Id,
            Nome = p.Nome,
            Preco = p.Preco.Value,
            Ativo = p.Ativo
        }).ToList();
    }
}