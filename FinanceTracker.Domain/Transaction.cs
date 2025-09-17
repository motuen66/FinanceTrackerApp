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
    public class Transaction
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = null!;
        public string Note { get; set; } = null!;
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public string UserId { get; set; } = null!;
        public User? User { get; set; }
        public string Type { get; set; } = null!; // "Income" or "Expense"

        public string CategoryId { get; set; } = null!;
        public Category? Category { get; set; }
    }
}
