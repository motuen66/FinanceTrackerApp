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
    public class Budget
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string UserId { get; set; } = null!;
        public User? User { get; set; }
        public string CategoryId { get; set; } = null!;
        Category Category { get; set; } = null!;
        public decimal LimitAmount { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
    }
}
