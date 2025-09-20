using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace FinanceTracker.Services.DTOs.SavingGoalDtos
{
    public class SavingGoalMutationDto
    {
        public string Title { get; set; } = null!;
        public decimal GoalAmount { get; set; }
        public decimal CurrentAmount { get; set; }
        public DateTime GoalDate { get; set; }
        public bool IsCompleted { get; set; } = false;
        public string UserId { get; set; } = null!;
    }
}