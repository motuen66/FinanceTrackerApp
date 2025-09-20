using System.ComponentModel.DataAnnotations;

namespace FinanceTracker.Services.DTOs.TransactionDtos
{
    public class TransactionMutationDto
    {
        [Required]
        [StringLength(200)]
        public string Note { get; set; } = null!;

        [Range(0, double.MaxValue)]
        public decimal Amount { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public string UserId { get; set; } = null!;

        [Required]
        [RegularExpression("Income|Expense", ErrorMessage = "Type must be either 'Income' or 'Expense'.")]
        public string Type { get; set; } = null!;

        [Required]
        public string CategoryId { get; set; } = null!;
    }
}
