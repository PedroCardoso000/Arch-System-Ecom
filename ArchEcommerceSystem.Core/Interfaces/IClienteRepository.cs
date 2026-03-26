namespace ArchEcommerceSystem.Core.Interfaces;

using ArchEcommerceSystem.Core.Aggregates;


public interface IClienteRepository
{
    Task<Cliente?> GetByIdAsync(Guid id);
    Task AddAsync(Cliente cliente);
    Task<Cliente?> GetByEmailAsync(string email);
}