using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FinanceTracker.Domain;

namespace FinanceTracker.Services.Interfaces.Repositories
{
    public interface IBudgetRepository : IGenericRepository<Budget>
    {
        Task<IEnumerable<Budget>> GetByUserIdAsync(string userId);
    }
}