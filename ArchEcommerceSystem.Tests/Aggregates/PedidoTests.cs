using ArchEcommerceSystem.Core.Aggregates;
using ArchEcommerceSystem.Core.ValueObjects;
using ArchEcommerceSystem.Core.DomainServices;

namespace ArchEcommerceSystem.Tests.Aggregates;

public class PedidoTests
{
	private readonly CalculadoraPedidoService _calculadora = new();

	[Fact]
	public void Deve_Calcular_Total_Corretamente()
	{
		// Arrange
		var pedido = new Pedido(Guid.NewGuid());

		// Act
		pedido.AdicionarItem(
			Guid.NewGuid(),
			new Quantidade(2),
			new Money(10),
			true,
			_calculadora
		);

		pedido.AdicionarItem(
			Guid.NewGuid(),
			new Quantidade(1),
			new Money(5),
			true,
			_calculadora
		);

		// Assert
		Assert.Equal(25, pedido.ValorTotal.Value);
	}

	[Fact]
	public void Nao_Deve_Confirmar_Pedido_Sem_Itens()
	{
		// Arrange
		var pedido = new Pedido(Guid.NewGuid());

		// Act & Assert
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
				false,
				_calculadora
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
			true,
			_calculadora
		);

		pedido.ConfirmarPedido();

		Assert.Single(pedido.DomainEvents);
	}
}