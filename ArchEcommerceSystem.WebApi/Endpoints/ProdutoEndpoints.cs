using Microsoft.EntityFrameworkCore;
using ArchEcommerceSystem.Infrastructure.Persistence;
using ArchEcommerceSystem.Core.Interfaces;
using ArchEcommerceSystem.Infrastructure.Repositories;
using ArchEcommerceSystem.UseCases.Handlers;
using ArchEcommerceSystem.UseCases.Queries;
using ArchEcommerceSystem.UseCases.Commands;
using ArchEcommerceSystem.Core.DomainServices;

namespace ArchEcommerceSystem.WebApi.Endpoints;

public static class ProdutoEndpoints
{
    public static void MapProdutoEndpoints(this WebApplication app)
    {
        app.MapGet("/produtos", async (GetProdutoHandler handler) =>
        {
            var result = await handler.Handle();
            return Results.Ok(result);
        });

        app.MapPost("/produtos", async (
            CreateProdutoCommand command,
            CreateProdutoHandler handler) =>
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