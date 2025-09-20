using System.ComponentModel.DataAnnotations;

namespace FinanceTracker.Services.DTOs.UserDtos
{
    public class UserMutationDto
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = null!;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;

        [Required]
        [StringLength(200)]
        public string PasswordHash { get; set; } = null!;
    }
}
