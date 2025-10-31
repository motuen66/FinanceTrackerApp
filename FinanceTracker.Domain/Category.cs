using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace FinanceTracker.Domain
{
    [BsonCollection("categories")]
    [MongoDB.Bson.Serialization.Attributes.BsonIgnoreExtraElements]
    public class Category
    {
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Type { get; set; } = null!; // "Income" or "Expense"
    [BsonRepresentation(BsonType.ObjectId)]
    public string UserId { get; set; } = null!;
    [MongoDB.Bson.Serialization.Attributes.BsonIgnore]
    public User? User { get; set; }
    [MongoDB.Bson.Serialization.Attributes.BsonIgnore]
    public List<Transaction> Transactions { get; set; } = new List<Transaction>();
    }
}
