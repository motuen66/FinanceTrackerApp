using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FinanceTracker.Domain;
using FinanceTracker.Services.DTOs.TransactionDtos;

namespace FinanceTracker.Services.Interfaces.Services
{
    public interface ITransactionService : IGenericService<Transaction>
    {
        Task<IEnumerable<Transaction>> GetByUserIdAndDateRangeAsync(string userId, DateTime from, DateTime to);
        Task<IEnumerable<Transaction>> GetByFilterAsync(string? userId, DateTime? from, DateTime? to, string? type);

        Task<List<TransactionViewDto>> GetAllWithCategoryNameAsync();
        Task<List<TransactionViewDto>> GetByRangeWithCategoryNameAsync(string userId, DateTime from, DateTime to);
        Task<List<TransactionViewDto>> GetByFilterWithCategoryNameAsync(string? userId, DateTime? from, DateTime? to, string? type);
        Task<TransactionViewDto?> GetByIdWithCategoryNameAsync(string id);
    }
}
