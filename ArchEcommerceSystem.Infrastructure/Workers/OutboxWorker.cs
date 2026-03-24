namespace ArchEcommerceSystem.Infrastructure.Workers;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using ArchEcommerceSystem.Infrastructure.Persistence;
using ArchEcommerceSystem.Infrastructure.Kafka;
using ArchEcommerceSystem.Core.DomainEvents;
using System.Text.Json;


public class OutboxWorker : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;

    public OutboxWorker(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using var scope = _serviceProvider.CreateScope();

            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var producer = scope.ServiceProvider.GetRequiredService<KafkaProducer>();

            var messages = await context.OutboxMessages
                .Where(x => x.ProcessedOn == null)
                .ToListAsync(stoppingToken);

            foreach (var message in messages)
            {
                if (message.Type == "PedidoConfirmadoDomainEvent")
                {
                    var domainEvent = JsonSerializer.Deserialize<PedidoConfirmadoDomainEvent>(message.Payload);

                    var integrationEvent = new PedidoConfirmadoIntegrationEvent
                    {
                        PedidoId = domainEvent!.PedidoId,
                        ClienteId = domainEvent.ClienteId,
                        ValorTotal = domainEvent.ValorTotal,
                        DataConfirmacao = DateTime.UtcNow
                    };

                    await producer.PublishAsync("pedido-confirmado", integrationEvent);
                }

                message.ProcessedOn = DateTime.UtcNow;
            }

            await context.SaveChangesAsync(stoppingToken);

            await Task.Delay(5000, stoppingToken);
        }
    }
}