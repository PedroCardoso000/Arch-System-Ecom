namespace ArchEcommerceSystem.Core.Interfaces;

using ArchEcommerceSystem.Core.Aggregates;


public interface IProdutoRepository
{
    Task<Produto?> GetByIdAsync(Guid id);
    Task<List<Produto>> GetAllAsync();
    Task AddAsync(Produto produto);
}