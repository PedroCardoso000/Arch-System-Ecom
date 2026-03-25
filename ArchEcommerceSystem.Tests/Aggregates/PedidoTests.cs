using ArchEcommerceSystem.Core.Aggregates;
using ArchEcommerceSystem.Core.ValueObjects;
using ArchEcommerceSystem.Core.DomainServices;

namespace ArchEcommerceSystem.Tests.Aggregates;

public class PedidoTests
{

	[Fact]
	public void Deve_Calcular_Total_Corretamente()
	{
		var pedido = new Pedido(Guid.NewGuid());

		pedido.AdicionarItem(
			Guid.NewGuid(),
			new Quantidade(2),
			new Money(10),
			true
		);

		pedido.AdicionarItem(
			Guid.NewGuid(),
			new Quantidade(1),
			new Money(5),
			true
		);

		Assert.Equal(25, pedido.ValorTotal.Value);
	}

	[Fact]
	public void Nao_Deve_Confirmar_Pedido_Sem_Itens()
	{
		var pedido = new Pedido(Guid.NewGuid());

		Assert.Throws<InvalidOperationException>(() => pedido.ConfirmarPedido());
	}

	[Fact]
	public void Nao_Deve_Adicionar_Produto_Inativo()
	{
		var pedido = new Pedido(Guid.NewGuid());

		Assert.Throws<InvalidOperationException>(() =>
			pedido.AdicionarItem(
				Guid.NewGuid(),
				new Quantidade(1),
				new Money(10),
				false
			)
		);
	}

	[Fact]
	public void Deve_Disparar_Evento_Ao_Confirmar_Pedido()
	{
		var pedido = new Pedido(Guid.NewGuid());

		pedido.AdicionarItem(
			Guid.NewGuid(),
			new Quantidade(1),
			new Money(10),
			true
		);

		pedido.ConfirmarPedido();

		Assert.Single(pedido.DomainEvents);
	}
}