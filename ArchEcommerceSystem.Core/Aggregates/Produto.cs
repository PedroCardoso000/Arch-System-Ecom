using ArchEcommerceSystem.Core.Entities;
using ArchEcommerceSystem.Core.ValueObjects;

namespace ArchEcommerceSystem.Core.Aggregates;

public class Produto : BaseEntity
{
    public string Nome { get; private set; } = null!;
    public Money Preco { get; private set; } = null!;
    public bool Ativo { get; private set; }
    
    private Produto() { }

    public Produto(string nome, Money preco)
    {
        if (string.IsNullOrWhiteSpace(nome))
            throw new ArgumentException("Nome é obrigatório");
           

        Nome = nome;
        Preco = preco;
        Ativo = true;
    }

    public void Desativar() => Ativo = false;
}