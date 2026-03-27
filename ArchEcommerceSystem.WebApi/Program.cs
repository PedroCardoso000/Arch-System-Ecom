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
using ArchEcommerceSystem.WebApi.Endpoints;

var builder = WebApplication.CreateBuilder(args);

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

builder.Services.AddSingleton<KafkaProducer>();

builder.Services.AddHostedService<OutboxWorker>();

builder.Services.AddScoped<CreatePedidoHandler>();
builder.Services.AddScoped<AddItemPedidoHandler>();
builder.Services.AddScoped<ConfirmarPedidoHandler>();
builder.Services.AddScoped<CreateClienteHandler>();   
builder.Services.AddScoped<CreateProdutoHandler>();
builder.Services.AddScoped<CalculadoraPedidoService>();

builder.Services.AddScoped<GetPedidoByIdHandler>();
builder.Services.AddScoped<GetProdutoHandler>();
builder.Services.AddScoped<GetClienteByIdHandler>();

builder.Services.AddScoped<IPedidoRepository, PedidoRepository>();
builder.Services.AddScoped<IClienteRepository, ClienteRepository>();
builder.Services.AddScoped<IProdutoRepository, ProdutoRepository>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapGet("/", () => "API funcional");

app.MapPedidoEndpoints();
app.MapClienteEndpoints();
app.MapProdutoEndpoints();


// =========================
// MIGRATION AUTO
// =========================

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    var retries = 30;

    while (retries > 0)
    {
        try
        {
            db.Database.EnsureCreated();
            db.Database.Migrate();
            break;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Tentando conectar no banco... {ex.Message}");
            retries--;
            Thread.Sleep(5000);
        }
    }
}

app.Run("http://0.0.0.0:8080");