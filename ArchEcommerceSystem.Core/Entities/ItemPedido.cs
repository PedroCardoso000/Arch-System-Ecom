using ArchEcommerceSystem.Core.ValueObjects;

namespace ArchEcommerceSystem.Core.Entities;

public class ItemPedido
{
    public Guid ProdutoId { get; private set; } 
    public Quantidade Quantidade { get; private set; } = null!;
    public Money PrecoUnitario { get; private set; } = null!;

    public Money Subtotal => PrecoUnitario * Quantidade.Value;
    
    private ItemPedido() { }

    public ItemPedido(Guid produtoId, Quantidade quantidade, Money precoUnitario)
    {
        ProdutoId = produtoId;
        Quantidade = quantidade;
        PrecoUnitario = precoUnitario;
    }
}