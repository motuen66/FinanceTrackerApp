using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace FinanceTracker.Domain
{
    [BsonCollection("transactions")]
    [MongoDB.Bson.Serialization.Attributes.BsonIgnoreExtraElements]
    public class Transaction
    {
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = null!;
        public string Note { get; set; } = null!;
    [BsonRepresentation(BsonType.Decimal128)]
    public decimal Amount { get; set; }
        public DateTime Date { get; set; }
    [BsonRepresentation(BsonType.ObjectId)]
    public string UserId { get; set; } = null!;
    [MongoDB.Bson.Serialization.Attributes.BsonIgnore]
    public User? User { get; set; }
        public string Type { get; set; } = null!; // "Income" or "Expense"

    [BsonRepresentation(BsonType.ObjectId)]
    public string CategoryId { get; set; } = null!;
    [MongoDB.Bson.Serialization.Attributes.BsonIgnore]
    public Category? Category { get; set; }
    }
}
