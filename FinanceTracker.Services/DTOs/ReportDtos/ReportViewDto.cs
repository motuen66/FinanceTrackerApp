namespace FinanceTracker.Services.DTOs.ReportDtos
{
    public class ReportViewDto
    {
        public string Id { get; set; } = null!;
        public int Month { get; set; }
        public int Year { get; set; }
        public decimal TotalIncome { get; set; }
        public decimal TotalExpense { get; set; }
        public decimal Balance { get; set; }
        public decimal TotalSavings { get; set; }
        public string UserId { get; set; } = null!;
    }
}
