public class PedidoConfirmadoIntegrationEvent
{
    public Guid PedidoId { get; set; }
    public Guid ClienteId { get; set; }
    public decimal ValorTotal { get; set; }
    public DateTime DataConfirmacao { get; set; }
}