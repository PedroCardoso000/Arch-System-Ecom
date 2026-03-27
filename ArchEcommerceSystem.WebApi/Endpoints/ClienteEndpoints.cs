using Microsoft.EntityFrameworkCore;
using ArchEcommerceSystem.Infrastructure.Persistence;
using ArchEcommerceSystem.Core.Interfaces;
using ArchEcommerceSystem.Infrastructure.Repositories;
using ArchEcommerceSystem.UseCases.Handlers;
using ArchEcommerceSystem.UseCases.Queries;
using ArchEcommerceSystem.UseCases.Commands;
using ArchEcommerceSystem.Infrastructure.Workers;
using ArchEcommerceSystem.Infrastructure.Kafka;
using ArchEcommerceSystem.Core.DomainServices;

namespace ArchEcommerceSystem.WebApi.Endpoints;

public static class ClienteEndpoints
{
    public static void MapClienteEndpoints(this WebApplication app)
    {
        app.MapGet("/clientes/{id}", async (
            Guid id,
            GetClienteByIdHandler handler) =>
        {
            var result = await handler.Handle(new GetClienteByIdQuery { Id = id });
            return result is null ? Results.NotFound() : Results.Ok(result);
        });

        app.MapPost("/clientes", async (
            CreateClienteCommand command,
            CreateClienteHandler handler) =>
        {
            try
            {
                var id = await handler.Handle(command);
                return Results.Ok(id);
            }
            catch (Exception ex)
            {
                return Results.BadRequest(ex.Message);
            }
        });
    }
}