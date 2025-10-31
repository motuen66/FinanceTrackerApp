using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace FinanceTracker.Domain
{
    [BsonCollection("budgets")]
    [MongoDB.Bson.Serialization.Attributes.BsonIgnoreExtraElements]
    public class Budget
    {
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = null!;
        [BsonRepresentation(BsonType.ObjectId)]
        public string UserId { get; set; } = null!;
    [MongoDB.Bson.Serialization.Attributes.BsonIgnore]
    public User? User { get; set; }
    [BsonRepresentation(BsonType.ObjectId)]
    public string CategoryId { get; set; } = null!;
    [MongoDB.Bson.Serialization.Attributes.BsonIgnore]
    public Category? Category { get; set; }
    [BsonRepresentation(BsonType.Decimal128)]
    public decimal LimitAmount { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
    }
}
