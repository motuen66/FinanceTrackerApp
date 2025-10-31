using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FinanceTracker.Domain;
using FinanceTracker.Infrastructure.Database;
using FinanceTracker.Services.Interfaces.Repositories;
using MongoDB.Driver;
using MongoDB.Bson;

namespace FinanceTracker.Infrastructure.Repositories
{
    public class TransactionRepository : GenericRepository<Transaction>, ITransactionRepository
    {
        public TransactionRepository(MongoDbContext context) : base(context)
        {
        }
        public async Task<IEnumerable<Transaction>> GetByUserIdAndDateRangeAsync(string userId, DateTime from, DateTime to)
        {
            var builder = Builders<Transaction>.Filter;
            var filter = builder.Gte(t => t.Date, from) & builder.Lt(t => t.Date, to);

            if (!string.IsNullOrWhiteSpace(userId))
            {
                // userId is stored as ObjectId in mongodb (BsonRepresentation on model). Use ObjectId for filter.
                filter = filter & builder.Eq("userId", new MongoDB.Bson.ObjectId(userId));
            }

            return await Collection.Find(filter).ToListAsync();
        }
    }
}
