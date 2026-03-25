using FinanceApp.Application.Common.Interfaces;
using FinanceApp.Domain.Accounts;
using Microsoft.EntityFrameworkCore;

namespace FinanceApp.Infrastructure.Persistence.Repositories;

public sealed class AccountRepository(FinanceDbContext dbContext) : IAccountRepository
{
    public Task<Account?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
        dbContext.Accounts.FirstOrDefaultAsync(a => a.Id == id, cancellationToken);

    public Task<IEnumerable<Account>> GetByCustomerIdAsync(Guid customerId, CancellationToken cancellationToken = default) =>
        Task.FromResult(dbContext.Accounts.Where(a => a.CustomerId == customerId).AsEnumerable());

    public async Task AddAsync(Account account, CancellationToken cancellationToken = default) =>
        await dbContext.Accounts.AddAsync(account, cancellationToken);
}
