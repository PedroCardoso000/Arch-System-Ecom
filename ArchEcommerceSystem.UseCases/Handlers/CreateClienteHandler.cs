using ArchEcommerceSystem.Core.Interfaces;
using ArchEcommerceSystem.Core.Aggregates;
using ArchEcommerceSystem.Core.ValueObjects;

namespace ArchEcommerceSystem.UseCases.Handlers;

public class CreateClienteHandler
{
    private readonly IClienteRepository _repository;

    public CreateClienteHandler(IClienteRepository repository)
    {
        _repository = repository;
    }

    public async Task<Guid> Handle(CreateClienteCommand command)
    {
        var cliente = new Cliente(command.Nome, new Email(command.Email));

        await _repository.AddAsync(cliente);

        return cliente.Id;
    }
}