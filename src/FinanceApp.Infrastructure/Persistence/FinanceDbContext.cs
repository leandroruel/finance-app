using FinanceApp.Application.Common.Interfaces;
using FinanceApp.Domain.Accounts;
using FinanceApp.Domain.Cards;
using FinanceApp.Domain.Common;
using FinanceApp.Domain.Customers;
using FinanceApp.Domain.Transactions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FinanceApp.Infrastructure.Persistence;

public class FinanceDbContext(DbContextOptions<FinanceDbContext> options, IPublisher publisher)
    : DbContext(options), IUnitOfWork
{
    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<Account> Accounts => Set<Account>();
    public DbSet<Transaction> Transactions => Set<Transaction>();
    public DbSet<Card> Cards => Set<Card>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(FinanceDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        // Collect domain events before saving
        var entitiesWithEvents = ChangeTracker
            .Entries<Entity>()
            .Select(e => e.Entity)
            .Where(e => e.DomainEvents.Any())
            .ToList();

        var result = await base.SaveChangesAsync(cancellationToken);

        // Publish domain events after successful save
        foreach (var entity in entitiesWithEvents)
        {
            var events = entity.DomainEvents.ToList();
            entity.ClearDomainEvents();
            foreach (var domainEvent in events)
                await publisher.Publish(domainEvent, cancellationToken);
        }

        return result;
    }
}
