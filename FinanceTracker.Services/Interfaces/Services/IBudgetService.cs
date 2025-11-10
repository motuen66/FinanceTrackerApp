using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FinanceTracker.Domain;
using FinanceTracker.Services.DTOs.BudgetDtos;

namespace FinanceTracker.Services.Interfaces.Services
{
    public interface IBudgetService : IGenericService<Budget>
    {
        Task<List<BudgetViewDto>> GetAllIncludeCategoryAsync();
        Task<List<BudgetViewDto>> GetAllIncludeCategoryByUserIdAsync(string userId);
    }
}
