using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FinanceTracker.Domain;
using FinanceTracker.Services.DTOs.SavingGoalDtos;
using FinanceTracker.Services.Interfaces.Repositories;
using FinanceTracker.Services.Interfaces.Services;
using MongoDB.Driver;

namespace FinanceTracker.Services
{
    public class SavingGoalService : GenericService<SavingGoal>, ISavingGoalService
    {
        private readonly ISavingGoalRepository _savingGoalRepository;
        public SavingGoalService(ISavingGoalRepository savingGoalRepository) : base(savingGoalRepository)
        {
            _savingGoalRepository = savingGoalRepository;
        }

        public async Task UpdateSavingGoalAsync(string id, SavingGoalMutationDto updateData)
        {
            var updateDefinitionBuilder = Builders<SavingGoal>.Update;
            var updates = new List<UpdateDefinition<SavingGoal>>();

            if (updateData.Title != null)
            {
                updates.Add(updateDefinitionBuilder.Set(sg => sg.Title, updateData.Title));
            }

            if (updateData.IsCompleted != null)
            {
                updates.Add(updateDefinitionBuilder.Set(sg => sg.IsCompleted, updateData.IsCompleted));
            }

            if (updateData.GoalAmount != null)
            {
                updates.Add(updateDefinitionBuilder.Set(sg => sg.GoalAmount, updateData.GoalAmount));
            }

            if (updateData.CurrentAmount != null)
            {
                updates.Add(updateDefinitionBuilder.Set(sg => sg.CurrentAmount, updateData.CurrentAmount));
            }

            if (updateData.GoalDate != null)
            {
                updates.Add(updateDefinitionBuilder.Set(sg => sg.GoalDate, updateData.GoalDate));
            }
            if(!updates.Any())
            {
                return;
            }
            var finalUpdateDefinition = updateDefinitionBuilder.Combine(updates);
            await _savingGoalRepository.UpdatePartialAsync(id, finalUpdateDefinition);
        }
    }
}
