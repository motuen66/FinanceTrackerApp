using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace FinanceTracker.Services.DTOs.SavingGoalDtos
{
    public class SavingGoalViewDto
    {
        public string Id { get; set; }
        public string Title { get; set; } = null!;
        public decimal GoalAmount { get; set; }
        public decimal CurrentAmount { get; set; }
        public DateTime GoalDate { get; set; }
        public bool IsCompleted { get; set; } = false;
    }
}