using FinanceApp.Domain.Accounts;

namespace FinanceApp.Application.Common.Interfaces;

public interface IAccountRepository
{
    Task<Account?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Account>> GetByCustomerIdAsync(Guid customerId, CancellationToken cancellationToken = default);
    Task AddAsync(Account account, CancellationToken cancellationToken = default);
}
