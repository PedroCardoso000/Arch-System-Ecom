using ArchEcommerceSystem.Core.Entities;
using ArchEcommerceSystem.Core.ValueObjects;

namespace ArchEcommerceSystem.Core.Aggregates;

public class Cliente : BaseEntity
{
    public string Nome { get; private set; } = null!;
    public Email Email { get; private set; } = null!;
    public DateTime DataCadastro { get; private set; }

    private Cliente() { }

    public Cliente(string nome, Email email)
    {
        if (string.IsNullOrWhiteSpace(nome))
            throw new ArgumentException("Nome é obrigatório");


        Nome = nome;
        Email = email;
        DataCadastro = DateTime.UtcNow;
    }
}