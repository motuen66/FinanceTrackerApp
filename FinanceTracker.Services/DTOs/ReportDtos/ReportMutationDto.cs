using System.ComponentModel.DataAnnotations;

namespace FinanceTracker.Services.DTOs.ReportDtos
{
    public class ReportMutationDto
    {
        [Range(1, 12)]
        public int Month { get; set; }

        [Range(2000, 2100)]
        public int Year { get; set; }

        [Range(0, double.MaxValue)]
        public decimal TotalIncome { get; set; }

        [Range(0, double.MaxValue)]
        public decimal TotalExpense { get; set; }

        public decimal Balance { get; set; }

        [Range(0, double.MaxValue)]
        public decimal TotalSavings { get; set; }

        [Required]
        public string UserId { get; set; } = null!;
    }
}
