using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace FinanceTracker.Domain
{
    [BsonCollection("savinggoals")]
    [MongoDB.Bson.Serialization.Attributes.BsonIgnoreExtraElements]
    public class SavingGoal
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = null!;
    [BsonElement("userId")]
    [BsonRepresentation(BsonType.ObjectId)]
    public string UserId { get; set; } = null!;
        [BsonElement("title")]
        public string Title { get; set; } = null!;
    [BsonElement("goalAmount")]
    [BsonRepresentation(BsonType.Decimal128)]
    public decimal GoalAmount { get; set; }
    [BsonElement("currentAmount")]
    [BsonRepresentation(BsonType.Decimal128)]
    public decimal CurrentAmount { get; set; }
        [BsonElement("goalDate")]
        public DateTime GoalDate { get; set; }
        [BsonElement("isCompleted")]
        public bool IsCompleted { get; set; } = false;
    }
}
