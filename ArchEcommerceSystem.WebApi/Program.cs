using Microsoft.EntityFrameworkCore;
using ArchEcommerceSystem.Infrastructure.Persistence;
using ArchEcommerceSystem.Core.Interfaces;
using ArchEcommerceSystem.Infrastructure.Repositories;
using ArchEcommerceSystem.UseCases.Handlers;
using ArchEcommerceSystem.UseCases.Queries;
using ArchEcommerceSystem.UseCases.Commands;
using ArchEcommerceSystem.Infrastructure.Workers;
using ArchEcommerceSystem.Infrastructure.Kafka;

var builder = WebApplication.CreateBuilder(args);

// DB
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        sqlOptions =>
        {
            sqlOptions.EnableRetryOnFailure(
                maxRetryCount: 5,
                maxRetryDelay: TimeSpan.FromSeconds(10),
                errorNumbersToAdd: null);
        }));

// Kafka
builder.Services.AddSingleton<KafkaProducer>();

// Worker
builder.Services.AddHostedService<OutboxWorker>();

// Handlers (COMMANDS)
builder.Services.AddScoped<CreatePedidoHandler>();
builder.Services.AddScoped<AddItemPedidoHandler>();
builder.Services.AddScoped<ConfirmarPedidoHandler>();
builder.Services.AddScoped<CreateClienteHandler>();   
builder.Services.AddScoped<CreateProdutoHandler>();  

// Handlers (QUERIES)
builder.Services.AddScoped<GetPedidoByIdHandler>();
builder.Services.AddScoped<GetProdutoHandler>();
builder.Services.AddScoped<GetClienteByIdHandler>();

// Repositories
builder.Services.AddScoped<IPedidoRepository, PedidoRepository>();
builder.Services.AddScoped<IClienteRepository, ClienteRepository>();
builder.Services.AddScoped<IProdutoRepository, ProdutoRepository>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapGet("/", () => "API rodando");


// =========================
// GETs
// =========================

app.MapGet("/produtos", async (GetProdutoHandler handler) =>
{
    var result = await handler.Handle();
    return Results.Ok(result);
});

app.MapGet("/pedidos/{id}", async (
    Guid id,
    GetPedidoByIdHandler handler) =>
{
    var result = await handler.Handle(new GetPedidoByIdQuery { Id = id });
    return result is null ? Results.NotFound() : Results.Ok(result);
});

app.MapGet("/clientes/{id}", async (
    Guid id,
    GetClienteByIdHandler handler) =>
{
    var result = await handler.Handle(new GetClienteByIdQuery { Id = id });
    return result is null ? Results.NotFound() : Results.Ok(result);
});


// =========================
// POSTs
// =========================

// Pedido
app.MapPost("/pedidos", async (
    CreatePedidoCommand command,
    CreatePedidoHandler handler) =>
{
    var id = await handler.Handle(command);
    return Results.Ok(id);
});

app.MapPost("/pedidos/{id}/itens", async (
    Guid id,
    AddItemPedidoCommand command,
    AddItemPedidoHandler handler) =>
{
    command.PedidoId = id;
    await handler.Handle(command);
    return Results.Ok();
});

app.MapPost("/pedidos/{id}/confirmar", async (
    Guid id,
    ConfirmarPedidoHandler handler) =>
{
    await handler.Handle(new ConfirmarPedidoCommand { PedidoId = id });
    return Results.Ok();
});


// Cliente
app.MapPost("/clientes", async (
    CreateClienteCommand command,
    CreateClienteHandler handler) =>
{
    var id = await handler.Handle(command);
    return Results.Ok(id);
});

// Produto
app.MapPost("/produtos", async (
    CreateProdutoCommand command,
    CreateProdutoHandler handler) =>
{
    var id = await handler.Handle(command);
    return Results.Ok(id);
});


// =========================
// MIGRATION AUTO
// =========================

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    var retries = 10;

    while (retries > 0)
    {
        try
        {
            db.Database.Migrate();
            break;
        }
        catch
        {
            retries--;
            Thread.Sleep(3000);
        }
    }
}

app.Run("http://0.0.0.0:8080");