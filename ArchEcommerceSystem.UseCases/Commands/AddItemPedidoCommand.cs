namespace ArchEcommerceSystem.UseCases.Commands;

public class AddItemPedidoCommand
{
    public Guid PedidoId { get; set; }
    public Guid ProdutoId { get; set; }
    public int Quantidade { get; set; }
}