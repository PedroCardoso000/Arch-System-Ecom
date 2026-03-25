using Confluent.Kafka;
using System.Text.Json;
using Microsoft.Extensions.Configuration;

namespace ArchEcommerceSystem.Infrastructure.Kafka;

public class KafkaProducer : IDisposable
{
    private readonly IProducer<string, string> _producer;

    public KafkaProducer(IConfiguration configuration)
    {
        var producerConfig = new ProducerConfig
        {
            BootstrapServers = configuration["Kafka:BootstrapServers"]
        };

        _producer = new ProducerBuilder<string, string>(producerConfig).Build();
    }

    public async Task PublishAsync(string topic, string key, object message)
    {
        var json = JsonSerializer.Serialize(message);

        await _producer.ProduceAsync(topic, new Message<string, string>
        {
            Key = key,
            Value = json
        });
    }

    public void Dispose()
    {
        _producer.Flush(TimeSpan.FromSeconds(5));
        _producer.Dispose();
    }
}