using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using FinanceTracker.Infrastructure.Database;
using FinanceTracker.Services.Interfaces.Repositories;
using FinanceTracker.Domain;
using MongoDB.Bson;
using MongoDB.Driver;

namespace FinanceTracker.Infrastructure.Repositories
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        private readonly IMongoCollection<TEntity> _collection;
        private readonly MongoDbContext _context;
        public GenericRepository(MongoDbContext context)
        {
            var collectionAttribute = typeof(TEntity).GetCustomAttribute<BsonCollectionAttribute>();
            if(collectionAttribute == null)
            {
                throw new InvalidOperationException($"Can not find BsonCollectionAttribute for class {typeof(TEntity).Name}.");
            }
            _collection = context.GetCollection<TEntity>(collectionAttribute.CollectionName);
        }
        public async Task AddAsync(TEntity entity)
        {
            await _collection.InsertOneAsync(entity);
        }

        private ObjectId GetId(TEntity entity)
        {
            var idProperty = entity.GetType().GetProperty("Id");
            if(idProperty == null)
            {
                throw new InvalidOperationException("Entity does not have an Id property.");
            }
            var idValue = idProperty.GetValue(entity);
            return new ObjectId(idValue.ToString());
        }

        public async Task DeleteAsync(string id)
        {
            var filter = Builders<TEntity>.Filter.Eq("_id", new ObjectId(id));
            await _collection.DeleteOneAsync(filter);
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await _collection.Find(_ => true).ToListAsync(); 
            // use "_" to show that this variable is unuse
            // we can use Builder<TEntity>.Filter.Empty as a param
        }

        public async Task<TEntity> GetByIdAsync(string id)
        {
            var filter = Builders<TEntity>.Filter.Eq("_id", new ObjectId(id));
            return await _collection.Find(filter).FirstOrDefaultAsync();
        }

        public async Task UpdateAsync(TEntity entity)
        {
            var filter = Builders<TEntity>.Filter.Eq("_id", GetId(entity));
            await _collection.ReplaceOneAsync(filter, entity);
        }

        public async Task UpdatePartialAsync(string id, UpdateDefinition<TEntity> updateDefinition)
        {
            var filter = Builders<TEntity>.Filter.Eq("_id", new ObjectId(id));
            await _collection.UpdateOneAsync(filter, updateDefinition);
        }
    }
}
