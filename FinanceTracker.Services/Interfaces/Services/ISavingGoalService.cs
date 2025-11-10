using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FinanceTracker.Domain;
using FinanceTracker.Services.DTOs.SavingGoalDtos;
using MongoDB.Driver;

namespace FinanceTracker.Services.Interfaces.Services
{
    public interface ISavingGoalService : IGenericService<SavingGoal>
    {
        Task<IEnumerable<SavingGoal>> GetByUserIdAsync(string userId);
        Task UpdateSavingGoalAsync(string id, SavingGoalMutationDto updateData);
    }
}
