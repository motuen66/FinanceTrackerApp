using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FinanceTracker.Domain;
using FinanceTracker.Infrastructure.Database;
using FinanceTracker.Services.Interfaces.Repositories;
using FinanceTracker.Services.Interfaces.Services;
using MongoDB.Bson;
using MongoDB.Driver;

namespace FinanceTracker.Infrastructure.Repositories
{
    public class SavingGoalRepository : GenericRepository<SavingGoal>, ISavingGoalRepository
    {
        public SavingGoalRepository(MongoDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<SavingGoal>> GetByUserIdAsync(string userId)
        {
            var filter = Builders<SavingGoal>.Filter.Eq("userId", new ObjectId(userId));
            return await Collection.Find(filter).ToListAsync();
        }
    }
}
