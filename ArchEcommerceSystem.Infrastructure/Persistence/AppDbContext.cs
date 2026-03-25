using Microsoft.EntityFrameworkCore;
using ArchEcommerceSystem.Core.Aggregates;
using ArchEcommerceSystem.Core.Entities;
using System.Text.Json;

namespace ArchEcommerceSystem.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public DbSet<Cliente> Clientes => Set<Cliente>();
    public DbSet<Produto> Produtos => Set<Produto>();
    public DbSet<Pedido> Pedidos => Set<Pedido>();
    public DbSet<OutboxMessage> OutboxMessages => Set<OutboxMessage>();

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var domainEvents = ChangeTracker
            .Entries<BaseEntity>()
            .SelectMany(x => x.Entity.DomainEvents)
            .ToList();

        foreach (var domainEvent in domainEvents)
        {
            var outboxMessage = new OutboxMessage
            {
                Id = Guid.NewGuid(),
                Type = domainEvent.GetType().Name,
                Payload = JsonSerializer.Serialize(domainEvent),
                OccurredOn = DateTime.UtcNow
            };

            OutboxMessages.Add(outboxMessage);
        }

        var result = await base.SaveChangesAsync(cancellationToken);

        foreach (var entity in ChangeTracker.Entries<BaseEntity>())
        {
            entity.Entity.ClearDomainEvents();
        }

        return result;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Produto
        modelBuilder.Entity<Produto>(entity =>
        {
            entity.OwnsOne(p => p.Preco, money =>
            {
                money.Property(m => m.Value)
                     .HasColumnName("Preco")
                     .HasPrecision(18, 2)
                     .IsRequired();
            });
        });

        // Pedido
        modelBuilder.Entity<Pedido>(entity =>
        {
            entity.OwnsOne(p => p.ValorTotal, money =>
            {
                money.Property(m => m.Value)
                     .HasColumnName("ValorTotal")
                           .HasPrecision(18, 2)
                     .IsRequired();
            });

            entity.OwnsMany(p => p.Itens, item =>
            {
                item.WithOwner();

                item.OwnsOne(i => i.PrecoUnitario, money =>
                {
                    money.Property(m => m.Value)
                         .HasColumnName("PrecoUnitario")
                               .HasPrecision(18, 2);
                });

                item.OwnsOne(i => i.Quantidade, q =>
                {
                    q.Property(x => x.Value)
                     .HasColumnName("Quantidade");
                });
            });
        });

        // Cliente
        modelBuilder.Entity<Cliente>(entity =>
        {
            entity.OwnsOne(c => c.Email, email =>
            {
                email.Property(e => e.Value)
                     .HasColumnName("Email")
                     .IsRequired();
            });
        });

        base.OnModelCreating(modelBuilder);
    }


}