using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FinanceTracker.Domain;
using FinanceTracker.Infrastructure.Database;
using FinanceTracker.Services.Interfaces.Repositories;
using FinanceTracker.Services.Interfaces.Services;

namespace FinanceTracker.Infrastructure.Repositories
{
    public class SavingGoalRepository : GenericRepository<SavingGoal>, ISavingGoalRepository
    {
        public SavingGoalRepository(MongoDbContext context) : base(context)
        {
        }
    }
}
