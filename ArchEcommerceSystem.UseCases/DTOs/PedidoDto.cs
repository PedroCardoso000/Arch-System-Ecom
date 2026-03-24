namespace ArchEcommerceSystem.UseCases.DTOs;

public class PedidoDto
{
    public Guid Id { get; set; }
    public Guid ClienteId { get; set; }
    public decimal ValorTotal { get; set; }
    public int Status { get; set; }
}