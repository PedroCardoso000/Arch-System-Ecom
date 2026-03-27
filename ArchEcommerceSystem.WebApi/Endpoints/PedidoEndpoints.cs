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

public static class PedidoEndpoints
{
    public static void MapPedidoEndpoints(this WebApplication app)
    {
        app.MapGet("/pedidos/{id}", async (
            Guid id,
            GetPedidoByIdHandler handler) =>
        {
            var result = await handler.Handle(new GetPedidoByIdQuery { Id = id });
            return result is null ? Results.NotFound() : Results.Ok(result);
        });

        app.MapPost("/pedidos", async (
            CreatePedidoCommand command,
            CreatePedidoHandler handler) =>
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

        app.MapPost("/pedidos/itens", async (
            AddItemPedidoCommand command,
            AddItemPedidoHandler handler) =>
        {
            try
            {
                await handler.Handle(command);
                return Results.Ok();
            }
            catch (Exception ex)
            {
                return Results.BadRequest(ex.Message);
            }
        });

        app.MapPost("/pedidos/{id}/confirmar", async (
            Guid id,
            ConfirmarPedidoHandler handler) =>
        {
            try
            {
                await handler.Handle(new ConfirmarPedidoCommand { PedidoId = id });
                return Results.Ok();
            }
            catch (Exception ex)
            {
                return Results.BadRequest(ex.Message);
            }
        });
    }
}