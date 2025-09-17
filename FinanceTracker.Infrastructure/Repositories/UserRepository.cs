using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FinanceTracker.Domain;
using FinanceTracker.Infrastructure.Database;
using FinanceTracker.Services.Interfaces.Repositories;
using MongoDB.Driver;

namespace FinanceTracker.Infrastructure.Repositories
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        private readonly IMongoCollection<User> _users;
        public UserRepository(MongoDbContext context) : base(context)
        {
            _users = context.GetCollection<User>("users");
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            var filter = Builders<User>.Filter.Eq("email", email);
            return await _users.Find(filter).FirstOrDefaultAsync();
        }
    }
}
