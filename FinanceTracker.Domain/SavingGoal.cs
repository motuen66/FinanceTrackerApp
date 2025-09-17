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
    public class SavingGoal
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = null!;
        [BsonElement("userId")]
        public string UserId { get; set; } = null!;
        [BsonElement("title")]
        public string Title { get; set; } = null!;
        [BsonElement("goalAmount")]
        public decimal GoalAmount { get; set; }
        [BsonElement("currentAmount")]
        public decimal CurrentAmount { get; set; }
        [BsonElement("goalDate")]
        public DateTime GoalDate { get; set; }
        [BsonElement("isCompleted")]
        public bool IsCompleted { get; set; } = false;
    }
}
