using System.ComponentModel.DataAnnotations;

namespace FinanceTracker.Services.DTOs.BudgetDtos
{
    public class BudgetMutationDto
    {
        [Required]
        public string UserId { get; set; } = null!;

        [Required]
        public string CategoryId { get; set; } = null!;

        [Range(0, double.MaxValue)]
        public decimal LimitAmount { get; set; }

        [Range(1, 12)]
        public int Month { get; set; }

        [Range(2000, 2100)]
        public int Year { get; set; }
    }
}
