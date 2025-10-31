using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FinanceTracker.Domain;
using FinanceTracker.Services.Interfaces.Repositories;
using FinanceTracker.Services.Interfaces.Services;

namespace FinanceTracker.Services
{
    public class TransactionService : GenericService<Transaction>, ITransactionService
    {
        private readonly ITransactionRepository _transactionRepository;
        public TransactionService(ITransactionRepository transactionRepository) : base(transactionRepository)
        {
            _transactionRepository = transactionRepository;
        }
        public Task<IEnumerable<Transaction>> GetByUserIdAndDateRangeAsync(string userId, DateTime from, DateTime to)
        {
            return _transactionRepository.GetByUserIdAndDateRangeAsync(userId, from, to);
        }
        public Task<IEnumerable<Transaction>> GetByFilterAsync(string? userId, DateTime? from, DateTime? to, string? type)
        {
            return _transactionRepository.GetByFilterAsync(userId, from, to, type);
        }
    }
}
