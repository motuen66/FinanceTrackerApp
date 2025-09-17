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
    public class SavingGoalService : GenericService<SavingGoal>, ISavingGoalService
    {
        private readonly ISavingGoalRepository _savingGoalRepository;
        public SavingGoalService(ISavingGoalRepository savingGoalRepository) : base(savingGoalRepository)
        {
            _savingGoalRepository = savingGoalRepository;
        }
    }
}
