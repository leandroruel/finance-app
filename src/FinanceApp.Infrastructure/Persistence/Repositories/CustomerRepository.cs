using FinanceApp.Application.Common.Interfaces;
using FinanceApp.Domain.Customers;
using Microsoft.EntityFrameworkCore;

namespace FinanceApp.Infrastructure.Persistence.Repositories;

public sealed class CustomerRepository(FinanceDbContext dbContext) : ICustomerRepository
{
    public Task<Customer?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
        dbContext.Customers.FirstOrDefaultAsync(c => c.Id == id, cancellationToken);

    public Task<Customer?> GetByCPFAsync(string cpf, CancellationToken cancellationToken = default) =>
        dbContext.Customers.FirstOrDefaultAsync(c => c.CPF.Value == cpf, cancellationToken);

    public Task<bool> ExistsByEmailAsync(string email, CancellationToken cancellationToken = default) =>
        dbContext.Customers.AnyAsync(c => c.Email.Value == email.ToLowerInvariant(), cancellationToken);

    public async Task AddAsync(Customer customer, CancellationToken cancellationToken = default) =>
        await dbContext.Customers.AddAsync(customer, cancellationToken);
}
