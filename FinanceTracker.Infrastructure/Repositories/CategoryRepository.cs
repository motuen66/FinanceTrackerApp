using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FinanceTracker.Domain;
using FinanceTracker.Infrastructure.Database;
using FinanceTracker.Services.Interfaces.Repositories;
using MongoDB.Bson;
using MongoDB.Driver;

namespace FinanceTracker.Infrastructure.Repositories
{
    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(MongoDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Category>> GetByUserIdAsync(string userId)
        {
            var filter = Builders<Category>.Filter.Eq("userId", new ObjectId(userId));
            return await Collection.Find(filter).ToListAsync();
        }
    }
}
