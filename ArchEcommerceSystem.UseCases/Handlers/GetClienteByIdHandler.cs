using ArchEcommerceSystem.Core.Interfaces;
using ArchEcommerceSystem.UseCases.DTOs;
using ArchEcommerceSystem.UseCases.Queries;

namespace ArchEcommerceSystem.UseCases.Handlers;

public class GetClienteByIdHandler
{
    private readonly IClienteRepository _repository;

    public GetClienteByIdHandler(IClienteRepository repository)
    {
        _repository = repository;
    }

    public async Task<ClienteDto?> Handle(GetClienteByIdQuery query)
    {
        var cliente = await _repository.GetByIdAsync(query.Id);

        if (cliente == null) return null;

        return new ClienteDto
        {
            Id = cliente.Id,
            Nome = cliente.Nome,
            Email = cliente.Email.Value
        };
    }
}