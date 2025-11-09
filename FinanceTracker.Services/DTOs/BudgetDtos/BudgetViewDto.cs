using FinanceTracker.Services.DTOs.CategoryDtos;

namespace FinanceTracker.Services.DTOs.BudgetDtos
{
    public class BudgetViewDto
    {
        public string Id { get; set; } = null!;
        public string UserId { get; set; } = null!;
        public string CategoryId { get; set; } = null!;
        public decimal LimitAmount { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public string CategoryName { get; set; } = null!;
    }
}
