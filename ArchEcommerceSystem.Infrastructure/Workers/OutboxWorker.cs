using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using ArchEcommerceSystem.Infrastructure.Persistence;
using ArchEcommerceSystem.Infrastructure.Kafka;
using ArchEcommerceSystem.Core.DomainEvents;
using System.Text.Json;

namespace ArchEcommerceSystem.Infrastructure.Workers;

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
                .Where(x => x.ProcessedOn == null && x.RetryCount < 5)
                .OrderBy(x => x.OccurredOn)
                .Take(50)
                .ToListAsync(stoppingToken);

            foreach (var message in messages)
            {
                try
                {
                    if (message.Type == nameof(PedidoConfirmadoDomainEvent))
                    {
                        var domainEvent = JsonSerializer.Deserialize<PedidoConfirmadoDomainEvent>(message.Payload);

                        if (domainEvent == null)
                            throw new Exception("Erro ao deserializar evento");

                        var integrationEvent = new PedidoConfirmadoIntegrationEvent
                        {
                            EventId = Guid.NewGuid(),
                            PedidoId = domainEvent.PedidoId,
                            ClienteId = domainEvent.ClienteId,
                            ValorTotal = domainEvent.ValorTotal,
                            DataConfirmacao = DateTime.UtcNow
                        };

                        await producer.PublishAsync(
                            "pedido-confirmado",
                            integrationEvent.PedidoId.ToString(),
                            integrationEvent
                        );
                    }

                    message.ProcessedOn = DateTime.UtcNow;
                }
                catch (Exception ex)
                {
                    message.RetryCount++;

                    Console.WriteLine(
                        $"Erro ao processar mensagem {message.Id} | Tentativa {message.RetryCount}: {ex.Message}"
                    );

                    if (message.RetryCount >= 5)
                    {
                        message.ErrorOn = DateTime.UtcNow;

                        Console.WriteLine(
                            $"Mensagem {message.Id} marcada como erro definitivo."
                        );
                    }
                }
            }

            await context.SaveChangesAsync(stoppingToken);

            await Task.Delay(5000, stoppingToken);
        }
    }
}