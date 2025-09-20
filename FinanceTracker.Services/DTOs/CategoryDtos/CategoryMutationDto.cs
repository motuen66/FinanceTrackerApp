using System.ComponentModel.DataAnnotations;

namespace FinanceTracker.Services.DTOs.CategoryDtos
{
    public class CategoryMutationDto
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = null!;

        [Required]
        [RegularExpression("Income|Expense", ErrorMessage = "Type must be either 'Income' or 'Expense'.")]
        public string Type { get; set; } = null!;

        [Required]
        public string UserId { get; set; } = null!;
    }
}
