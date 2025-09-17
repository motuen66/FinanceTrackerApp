using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace FinanceTracker.Infrastructure.Database
{
    public class MongoDbContext
    {
        private readonly IMongoDatabase _database; 

        public MongoDbContext(MongoDbSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            _database = client.GetDatabase(settings.DatabaseName);
        }

        public IMongoCollection<T> GetCollection<T>(string name)
        {
            return _database.GetCollection<T>(name);
        }

        internal IMongoCollection<object> GetCollection<T>(object collectionName)
        {
            throw new NotImplementedException();
        }
    }
}
