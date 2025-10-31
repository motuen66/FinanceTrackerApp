using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FinanceTracker.Domain;

namespace FinanceTracker.Services.Interfaces.Services
{
    public interface ITransactionService : IGenericService<Transaction>
    {
        Task<IEnumerable<Transaction>> GetByUserIdAndDateRangeAsync(string userId, DateTime from, DateTime to);
    }
}
