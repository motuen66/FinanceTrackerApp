using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace FinanceTracker.Domain
{
    [BsonCollection("users")]
    [MongoDB.Bson.Serialization.Attributes.BsonIgnoreExtraElements]
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string PasswordHash { get; set; } = null!; // Store hashed password
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    // Navigation properties (ignored for persistence; use dedicated repo methods / aggregation when needed)
    [MongoDB.Bson.Serialization.Attributes.BsonIgnore]
    public List<Transaction> Transactions { get; set; } = new List<Transaction>();
    [MongoDB.Bson.Serialization.Attributes.BsonIgnore]
    public List<Category> Categories { get; set; } = new List<Category>();
    [MongoDB.Bson.Serialization.Attributes.BsonIgnore]
    public List<SavingGoal> SavingGoals { get; set; } = new List<SavingGoal>();
    [MongoDB.Bson.Serialization.Attributes.BsonIgnore]
    public List<Budget> Budgets { get; set; } = new List<Budget>();
    }
}
