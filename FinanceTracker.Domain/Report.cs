using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace FinanceTracker.Domain
{
    [BsonCollection("reports")]
    [MongoDB.Bson.Serialization.Attributes.BsonIgnoreExtraElements]
    public class Report
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = null!;

        public int Month { get; set; }
        public int Year { get; set; }

        [BsonRepresentation(BsonType.Decimal128)]
        public decimal TotalIncome { get; set; }

        [BsonRepresentation(BsonType.Decimal128)]
        public decimal TotalExpense { get; set; }

        [BsonRepresentation(BsonType.Decimal128)]
        public decimal Balance { get; set; }

        [BsonRepresentation(BsonType.Decimal128)]
        public decimal TotalSavings { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string UserId { get; set; } = null!;

        [MongoDB.Bson.Serialization.Attributes.BsonIgnore]
        public User? User { get; set; }

        // Prefer storing transaction ids instead of full objects
        [BsonRepresentation(BsonType.ObjectId)]
        public List<string> TransactionIds { get; set; } = new List<string>();

        public int TransactionsCount { get; set; }
    }
}
