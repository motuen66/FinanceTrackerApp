using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FinanceTracker.Domain;
using FinanceTracker.Infrastructure.Database;
using FinanceTracker.Services.DTOs.BudgetDtos;
using FinanceTracker.Services.Interfaces.Repositories;
using MongoDB.Bson;
using MongoDB.Driver;

namespace FinanceTracker.Infrastructure.Repositories
{
    public class BudgetRepository : GenericRepository<Budget>, IBudgetRepository
    {
        public readonly IMongoCollection<Budget> _collection;
        public readonly MongoDbContext _context;

        public BudgetRepository(MongoDbContext context) : base(context)
        {
            _context = context;
            // Use lowercase collection names to match BsonCollection attributes and existing DB
            _collection = context.GetCollection<Budget>("budgets");
        }

        public async Task<IEnumerable<Budget>> GetByUserIdAsync(string userId)
        {
            var filter = Builders<Budget>.Filter.Eq("userId", new ObjectId(userId));
            return await Collection.Find(filter).ToListAsync();
        }
    }
}
