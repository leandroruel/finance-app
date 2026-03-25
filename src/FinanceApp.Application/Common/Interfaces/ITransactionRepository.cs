using FinanceApp.Domain.Transactions;

namespace FinanceApp.Application.Common.Interfaces;

public interface ITransactionRepository
{
    Task<Transaction?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Transaction>> GetByAccountIdAsync(Guid accountId, int skip = 0, int take = 50, CancellationToken cancellationToken = default);
    Task AddAsync(Transaction transaction, CancellationToken cancellationToken = default);
}
