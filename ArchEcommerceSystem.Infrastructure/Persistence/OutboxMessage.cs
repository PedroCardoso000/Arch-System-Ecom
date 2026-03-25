namespace ArchEcommerceSystem.Infrastructure.Persistence;

public class OutboxMessage
{
    public Guid Id { get; set; }
    public string Type { get; set; } = string.Empty;
    public string Payload { get; set; } = string.Empty;
    public DateTime OccurredOn { get; set; }
    public DateTime? ProcessedOn { get; set; }
    public int RetryCount { get; set; } = 0;
    public DateTime? ErrorOn { get; set; }
}